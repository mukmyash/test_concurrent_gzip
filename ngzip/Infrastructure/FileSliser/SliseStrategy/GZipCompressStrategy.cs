using ngzip.Infrastructure.SliseStrategy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ngzip.Infrastructure.SliseStrategy
{
    /// <summary>
    /// Рубит сжатый GZIP'ом файл на пачки.
    /// </summary>
    public class GZipCompressStrategy : ISliseStrategy
    {
        public byte[] Slise(int partNumber, FileStream InfileStream)
        {
            // Достаем из заголовка MTIME информацию о длине блока
            // при архивации мы туда спрятали эту инфу... =)
            var lengthBuffer = new byte[8];
            InfileStream.Read(lengthBuffer, 0, lengthBuffer.Length);
            var blockLength = BitConverter.ToInt32(lengthBuffer, 4);

            // Читаем данные для последующей разорхивации.
            var compressedData = new byte[blockLength];
            lengthBuffer.CopyTo(compressedData, 0);
            InfileStream.Read(compressedData, 8, blockLength - 8);

            return compressedData;
        }
    }
}
