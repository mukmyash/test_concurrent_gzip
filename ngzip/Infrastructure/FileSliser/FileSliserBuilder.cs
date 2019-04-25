using ngzip.Exceptions;
using ngzip.Infrastructure.SliseStrategy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ngzip.Infrastructure.FileSliser
{
    public class FileSliserBuilder
    {
        private readonly string _filePath;
        private readonly string _fileName;
        private ISliseStrategy _sliseStrategy;

        public FileSliserBuilder(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new NGZipInputFileNotSetException();

            var fileInfo = new FileInfo(filePath);
            if (!fileInfo.Exists)
                throw new NGZipInputFileNotFoundException(fileInfo.FullName);

            _fileName = fileInfo.Name;
            _filePath = filePath;
        }

        // NOTE: Должно быть Extension метод....
        public FileSliserBuilder UseFixedSizeStrategy(int sizeInMB)
        {
            _sliseStrategy = new FixedSizeStrategy(sizeInMB);
            return this;
        }

        // NOTE: Должно быть Extension метод....
        public FileSliserBuilder UseGZIPCompressStrategy()
        {
            _sliseStrategy = new GZipCompressStrategy();
            return this;
        }

        public FileSliser Build()
        {
            return new FileSliser(_filePath, _sliseStrategy);
        }
    }
}
