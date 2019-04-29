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
                var result = outputStream.ToArray();
                // Добавляем в секцию MTIME информацию о размере блока.
                // По хорошему нужно былов секцию FEXTRA. НО и тут тоже не плохо =)
                // это ведь тестовое.
                // https://tools.ietf.org/html/rfc1952#page-5
                BitConverter.GetBytes(result.Length).CopyTo(result, 4);
                return result;
            }
        }
    }
}
