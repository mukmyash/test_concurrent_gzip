using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ngzip.Infrastructure.SliseStrategy
{
    public class FixedSizeStrategy : ISliseStrategy
    {
        const int MB = 1024 * 1024;

        /// <summary>
        /// Размер в байтах
        /// </summary>
        private readonly int _sizeInByte;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="partSizeMB">Размер партиции в мегабайтах.</param>
        /// <param name="resultFileName">Имя файла (которое пилим)</param>
        public FixedSizeStrategy(int sizeInMB)
        {
            _sizeInByte = sizeInMB * MB;
        }

        public byte[] Slise(int partNumber, FileStream InfileStream)
        {
            var bufferSize = Math.Min(_sizeInByte, InfileStream.Length - InfileStream.Position);
            byte[] buffer = new byte[bufferSize];
            InfileStream.Read(buffer, 0, buffer.Length);
            return buffer;
        }
    }
}
