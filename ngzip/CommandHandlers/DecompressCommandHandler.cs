using ngzip.Infrastructure.FileSliser;
using ngzip.Infrastructure.GZIP;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ngzip
{
    public class DecompressCommandHandler : CommandHandlerBase
    {
        public void Handle(DecompressCommand command)
        {
            var fileSliser = new FileSliserBuilder(command.InputFileInfo.FullName)
                .UseGZIPCompressStrategy()
                .Build();
            var gzipProcess = new DecompressProcess();
            base.HandleBase(command, fileSliser, gzipProcess);
        }

    }
}
