using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ngzip.Infrastructure.GZIP
{
    public class DecompressProcess : IGZipProcess
    {
        public byte[] Process(int partNumber, Stream contentStream)
        {
            using (var outputStream = new MemoryStream())
            {
                using (GZipStream gzip = new GZipStream(contentStream,
                    CompressionMode.Decompress, false))
                {
                    gzip.CopyTo(outputStream);
                }
                return outputStream.ToArray();
            }
        }
    }
}
