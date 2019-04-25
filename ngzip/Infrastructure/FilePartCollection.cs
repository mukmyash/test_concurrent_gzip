using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ngzip.Infrastructure
{
    public class FilePartCollection
    {
        //SemaphoreSlim semaphore = new SemaphoreSlim(5, 5);

        private static FilePartCollection Instance;

        public static FilePartCollection GetInstance()
        {
            lock (locker)
            {
                if (Instance == null)
                    Instance = new FilePartCollection();
            }
            return Instance;
        }

        private FilePartCollection()
        {
            _fileParts = new ConcurrentQueue<(int partNumber, byte[] filePath)>();
        }

        static object locker = new object();

        /// <summary>
        /// Больше блоки приниматься не будут.
        /// </summary>
        bool _isEnd = false;

        /// <summary>
        /// Хранит информацию о пути к файлу.
        /// </summary>
        ConcurrentQueue<(int partNumber, byte[] content)> _fileParts;

        public IEnumerable<(int partNumber, byte[] content)> GetFilePart()
        {
                while (!_fileParts.IsEmpty || !_isEnd)
                {
                    (int partNumber, byte[] content) result;
                    while (!_fileParts.TryDequeue(out result))
                    {
                        if (_isEnd)
                            break;
                    lock (locker)
                        Monitor.Wait(locker);
                    }
                    if (result.content != null)
                    {
                        Console.WriteLine($"GZIP: {result.partNumber} part");
                        yield return result;
                    }

                    //if (semaphore.CurrentCount != 5)
                    //    semaphore.Release();
                }
        }

        public void AppendPartInfo(int partNumber, byte[] content)
        {
           // semaphore.Wait();

            Console.WriteLine($"GZIP Append:  {partNumber} part");
            if (_isEnd)
                throw new Exception("Объект заблокирован на прием новых порций.");

            _fileParts.Enqueue((partNumber, content));
            lock (locker)
                Monitor.Pulse(locker);
        }

        public void End()
        {
            _isEnd = true;
            lock (locker)
                Monitor.PulseAll(locker);
        }
    }
}
