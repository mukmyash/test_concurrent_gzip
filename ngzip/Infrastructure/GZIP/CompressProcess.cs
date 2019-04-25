using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ngzip.Infrastructure.GZIP
{
    public class CompressProcess : IGZipProcess
    {
        public byte[] Process(int partNumber, Stream contentStream)
        {
            using (var outputStream = new MemoryStream())
            {
                using (GZipStream gzipStream = new GZipStream(outputStream,
                    CompressionLevel.Fastest, false))
                {
                    contentStream.CopyTo(gzipStream);
                }
                return outputStream.ToArray();
            }
        }
    }
}
