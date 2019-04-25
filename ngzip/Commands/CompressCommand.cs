using ngzip.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ngzip
{
    public class CompressCommand : IGZipCommand
    {
        public CompressCommand(string filePath, string gZipFilePath = null)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new NGZipInputFileNotSetException();
            }
            if (string.IsNullOrWhiteSpace(gZipFilePath))
            {
                gZipFilePath = $"{filePath}.gz";
            }

            var fileInfo = new FileInfo(filePath);
            if (!fileInfo.Exists)
            {
                throw new NGZipInputFileNotFoundException(filePath);
            }
            var archiveFileInfo = new FileInfo(gZipFilePath);
            if (archiveFileInfo.Exists)
            {
                throw new NGZipOutputFileExistsException(gZipFilePath);
            }

            InputFileInfo = fileInfo;
            OutputFileInfo = archiveFileInfo;
        }

        public int PartSizeMB { get; } = 1;

        public FileInfo InputFileInfo { get; }

        public FileInfo OutputFileInfo { get; }
    }
}
