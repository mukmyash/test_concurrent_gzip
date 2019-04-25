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
    public class FileSliser
    {
        string _filePath;

        private readonly ISliseStrategy _sliseStrategy;
        public FileSliser(string filePath, ISliseStrategy sliseStrategy)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new NGZipInputFileNotSetException();

            var fileInfo = new FileInfo(filePath);
            if (!fileInfo.Exists)
                throw new NGZipInputFileNotFoundException(fileInfo.FullName);

            _filePath = fileInfo.FullName;

            _sliseStrategy = sliseStrategy ?? throw new ArgumentNullException(nameof(sliseStrategy));
        }

        public IEnumerable<byte[]> SliseFile()
        {
            FileStream fileStream;

            try
            {
                fileStream = new FileStream(_filePath, FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException)
            {
                throw new NGZipInputFileUsedException(_filePath);
            }

            using (fileStream)
            {
                int partNumber = 0;
                while (fileStream.Position < fileStream.Length)
                {
                    yield return _sliseStrategy.Slise(++partNumber, fileStream);
                }
            }
        }
    }
}
