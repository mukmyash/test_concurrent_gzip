using ngzip.Infrastructure;
using System.IO;

namespace ngzip.Infrastructure.SliseStrategy
{
    public interface ISliseStrategy
    {
        byte[] Slise(int partNumber, FileStream InfileStream);
    }
}