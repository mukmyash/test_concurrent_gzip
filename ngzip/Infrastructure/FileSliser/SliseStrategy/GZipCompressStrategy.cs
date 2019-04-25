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
            // Очень долго отрабатывает. по данным профилировщика ~6сек.... 0_0 
            var filelength = InfileStream.Length;

            using (var partStream = new MemoryStream())
            {
                if (InfileStream.Position != 0)
                    partStream.WriteByte(31);

                int state = 0;

                while (InfileStream.Position < filelength)
                {
                    var readedByte = (byte)InfileStream.ReadByte();

                    state = GetNewState(state, readedByte);
                    if (state > 4)
                    {
                        return partStream.ToArray();
                    }

                    partStream.WriteByte(readedByte);
                }

                return partStream.ToArray();
            }
        }

        /// <summary>
        /// Определеят новое значение состояния. (поиск конечной последовательности байт (0 0 16 0))
        /// </summary>
        /// <param name="state">Значение состояния полученое на прошло байте</param>
        /// <param name="readedByte">текущий байт</param>
        /// <returns></returns>
        private static int GetNewState(int state, byte readedByte)
        {
            switch (state)
            {
                case 0:
                    if (readedByte == 0)
                        state++;
                    else
                        state = 0;
                    break;
                case 1:
                    if (readedByte == 0)
                        state++;
                    else
                        state = 0;
                    break;
                case 2:
                    if (readedByte == 16)
                        state++;
                    else
                        state = 0;
                    break;
                case 3:
                    if (readedByte == 0)
                        state++;
                    else
                        state = 0;
                    break;
                case 4:
                    if (readedByte == 31)
                        state++;
                    else
                        state = 0;
                    break;
            }

            return state;
        }
    }
}
