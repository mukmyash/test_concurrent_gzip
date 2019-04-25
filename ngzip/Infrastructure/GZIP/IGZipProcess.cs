using System.IO;

namespace ngzip.Infrastructure.GZIP
{
    public interface IGZipProcess
    {
        byte[] Process(int partNumber, Stream contentStream);
    }
}