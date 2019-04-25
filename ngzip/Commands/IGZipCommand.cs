using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ngzip
{
    public interface IGZipCommand
    {
        FileInfo InputFileInfo { get; }
        FileInfo OutputFileInfo { get; }
        int PartSizeMB { get; }
    }
}
