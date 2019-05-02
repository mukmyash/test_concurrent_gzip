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
        protected void HandleBase(IGZipCommand command, FileSliser fileSlicer, IGZipProcess gzipProcess, ThreadCancelationToken token)
        {
            var mergePartition = StartMergeThread(command.OutputFileInfo.FullName, token);

            List<Thread> threadPool = new List<Thread>(5);
            for (var i = 0; i < 6; i++)
                threadPool.Add(StartGzipProcess(gzipProcess, token));

            SliseFile(fileSlicer, token);

            var partSequince = FilePartSequince.GetInstance();
            foreach (var gzipThread in threadPool)
                gzipThread.Join();
            partSequince.End();

            mergePartition.Join();
        }

        private Thread StartGzipProcess(IGZipProcess gzipProcess, ThreadCancelationToken token)
        {
            Thread gzipThread = new Thread(param =>
            {
                var cancelToken = param as ThreadCancelationToken;

                var partCollection = FilePartCollection.GetInstance();
                var partSequince = FilePartSequince.GetInstance();

                foreach (var partInfo in partCollection.GetFilePart())
                {
                    if (cancelToken.IsCancel)
                        break;

                    try
                    {
                        using (var partFileStream = new MemoryStream(partInfo.content))
                        {
                            var gzipPath = gzipProcess.Process(partInfo.partNumber, partFileStream);
                            partSequince.AppendPartInfo(partInfo.partNumber, gzipPath);
                        }
                    }
                    catch (Exception e)
                    {
                        cancelToken.Cancel(e);
                    }
                }
            });
            gzipThread.Start(token);
            return gzipThread;
        }

        /// <summary>
        /// Режем входящий файл.
        /// </summary>
        /// <param name="fileSlicer">Резак...</param>
        private void SliseFile(FileSliser fileSlicer, ThreadCancelationToken token)
        {
            int partNumber = 0;
            var partCollection = FilePartCollection.GetInstance();
            try
            {
                foreach (var partFilePath in fileSlicer.SliseFile())
                {
                    if (token.IsCancel)
                        break;
                    partCollection.AppendPartInfo(partNumber++, partFilePath);
                }

                // эта конструкция должна находиться здесь, для того что бы все остальные потоки прочухали 
                // что что-то не так и откатили изменения.
                partCollection.End();
            }
            catch (Exception e)
            {
                token.Cancel(e);
            }
        }

        /// <summary>
        /// Запускает поток который собирает все порции в один файл.
        /// </summary>
        /// <param name="outFilePath">путь в выходной файл</param>
        /// <returns></returns>
        private Thread StartMergeThread(string outFilePath, ThreadCancelationToken tokent)
        {
            Thread mergePartition = new Thread(param =>
            {
                (string filePath, ThreadCancelationToken cancelToken)? castParam = param as (string filePath, ThreadCancelationToken cancelToken)?;
                new PartitionMerge().Merge(castParam.Value.filePath, castParam.Value.cancelToken);
            });
            var threadParam = (filePath: outFilePath, cancelToken: tokent);
            mergePartition.Start(threadParam);
            return mergePartition;
        }
    }
}
