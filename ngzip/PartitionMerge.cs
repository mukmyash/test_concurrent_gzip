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
        public void Merge(string outputhPath)
        {
            var filePartSequince = FilePartSequince.GetInstance();
            using (var outputFile = OutputFileInfo.GetInstance(outputhPath))
            {
                foreach (var filePartPath in filePartSequince.GetFilePart())
                    outputFile.AppendPart(filePartPath);
            }
        }
    }
}
