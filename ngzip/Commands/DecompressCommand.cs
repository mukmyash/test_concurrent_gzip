using ngzip.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ngzip
{
    public class DecompressCommand : IGZipCommand
    {
        public DecompressCommand(string gZipFilePath, string filePath = null)
        {
            if (string.IsNullOrWhiteSpace(gZipFilePath))
            {
                throw new NGZipInputFileNotSetException();
            }

            var archiveFileInfo = new FileInfo(gZipFilePath);
            if (!archiveFileInfo.Exists)
            {
                throw new NGZipInputFileNotFoundException(gZipFilePath);
            }


            if (string.IsNullOrWhiteSpace(filePath))
            {
                var fileName = archiveFileInfo.Name.Remove(archiveFileInfo.Name.Length - archiveFileInfo.Extension.Length);
                filePath = Path.Combine(archiveFileInfo.DirectoryName, fileName);
            }

            var fileInfo = new FileInfo(filePath);
            if (fileInfo.Exists)
            {
                throw new NGZipOutputFileExistsException(filePath);
            }
            OutputFileInfo = fileInfo;
            InputFileInfo = archiveFileInfo;
        }

        public int PartSizeMB { get; } = 1;

        public FileInfo InputFileInfo { get; }

        public FileInfo OutputFileInfo { get; }
    }
}
