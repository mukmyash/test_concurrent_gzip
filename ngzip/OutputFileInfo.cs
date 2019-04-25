using ngzip.Exceptions;
using ngzip.Infrastructure;
using System;
using System.IO;

namespace ngzip
{
    public class OutputFileInfo : IDisposable
    {
        private static OutputFileInfo Instance;
        public static OutputFileInfo GetInstance(string filePath)
        {
            if (filePath == null)
                throw new NGZipOutputFileNotSetException();

            if (Instance == null)
                Instance = new OutputFileInfo(filePath);

            return Instance;
        }

        private Stream _outputStream;

        private OutputFileInfo(string filePath)
        {
            var fileInfo = new FileInfo(filePath);
            if (fileInfo.Exists)
                throw new NGZipOutputFileExistsException(fileInfo.FullName);

            _outputStream = fileInfo.OpenWrite(); //new FileStream(fileInfo.FullName, FileMode.Append, FileAccess.Write);
        }

        public void AppendPart(byte[] content)
        {
            _outputStream.Write(content, 0, content.Length);
        }

        public void Dispose()
        {
            _outputStream.Dispose();
        }
    }
}
