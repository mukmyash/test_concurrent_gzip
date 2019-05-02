using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ngzip.Infrastructure;
using ngzip.Infrastructure.FileSliser;
using ngzip.Infrastructure.GZIP;

namespace ngzip
{
    public class CompressCommandHandler : CommandHandlerBase
    {
        public void Handle(CompressCommand command, ThreadCancelationToken token)
        {
            var fileSliser = new FileSliserBuilder(command.InputFileInfo.FullName)
                .UseFixedSizeStrategy(command.PartSizeMB)
                .Build();
            var gzipProcess = new CompressProcess();

            base.HandleBase(command, fileSliser, gzipProcess, token);
        }
    }
}
