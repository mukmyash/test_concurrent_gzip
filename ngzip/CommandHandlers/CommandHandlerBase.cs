using ngzip.Infrastructure;
using ngzip.Infrastructure.FileSliser;
using ngzip.Infrastructure.GZIP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ngzip
{
    public abstract class CommandHandlerBase
    {
        protected void HandleBase(IGZipCommand command, FileSliser fileSlicer, IGZipProcess gzipProcess)
        {
            var mergePartition = StartMergeThread(command.OutputFileInfo.FullName);

            List<Thread> threadPool = new List<Thread>(5);
            for (var i = 0; i < 6; i++)
                threadPool.Add(StartGzipProcess(gzipProcess));

            SliseFile(fileSlicer);

            var partSequince = FilePartSequince.GetInstance();
            foreach (var gzipThread in threadPool)
                gzipThread.Join();
            partSequince.End();

            mergePartition.Join();
        }

        private Thread StartGzipProcess(IGZipProcess gzipProcess)
        {
            Thread gzipThread = new Thread(param =>
            {
                var partCollection = FilePartCollection.GetInstance();
                var partSequince = FilePartSequince.GetInstance();

                foreach (var partInfo in partCollection.GetFilePart())
                {
                    using (var partFileStream = new MemoryStream(partInfo.content))
                    {
                        var gzipPath = gzipProcess.Process(partInfo.partNumber, partFileStream);
                        partSequince.AppendPartInfo(partInfo.partNumber, gzipPath);
                    }
                }
            });
            gzipThread.Start();
            return gzipThread;
        }

        /// <summary>
        /// Режем входящий файл.
        /// </summary>
        /// <param name="fileSlicer">Резак...</param>
        private void SliseFile(FileSliser fileSlicer)
        {
            int partNumber = 0;
            var partCollection = FilePartCollection.GetInstance();
            foreach (var partFilePath in fileSlicer.SliseFile())
            {
                partCollection.AppendPartInfo(partNumber++, partFilePath);
            }
            partCollection.End();
        }

        /// <summary>
        /// Запускает поток который собирает все порции в один файл.
        /// </summary>
        /// <param name="outFilePath">путь в выходной файл</param>
        /// <returns></returns>
        private Thread StartMergeThread(string outFilePath)
        {
            Thread mergePartition = new Thread(param =>
            {
                new PartitionMerge().Merge((string)param);
            });
            mergePartition.Start(outFilePath);
            return mergePartition;
        }
    }
}
