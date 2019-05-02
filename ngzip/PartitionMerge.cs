using ngzip.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ngzip
{
    public class PartitionMerge
    {
        public void Merge(string outputhPath, ThreadCancelationToken token)
        {
            var filePartSequince = FilePartSequince.GetInstance();
            var outputFile = OutputFileInfo.GetInstance(outputhPath);

            using (outputFile)
            {
                foreach (var filePartPath in filePartSequince.GetFilePart())
                {
                    if (token.IsCancel)
                    {
                        outputFile.Delete();
                        break;
                    }

                    try
                    {
                        outputFile.AppendPart(filePartPath);
                    }
                    catch (Exception e)
                    {
                        outputFile.Delete();
                        token.Cancel(e);
                    }
                }

                if (token.IsCancel)
                {
                    outputFile.Delete();
                }
            }
        }
    }
}
