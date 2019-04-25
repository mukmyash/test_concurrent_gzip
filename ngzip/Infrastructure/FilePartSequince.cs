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
    internal class FilePartSequince
    {
        private static FilePartSequince Instance;

        public static FilePartSequince GetInstance()
        {
            if (Instance == null)
                Instance = new FilePartSequince();
            return Instance;
        }

        private FilePartSequince()
        {
            _lastPart = 0;
            _fileParts = new ConcurrentDictionary<int, byte[]>();
        }

        static object locker = new object();

        /// <summary>
        /// Если true то новые блоки больше приниматься не будут.
        /// </summary>
        bool _isEnd = false;

        /// <summary>
        /// Последний прочитанный партишн.
        /// </summary>
        volatile int _lastPart;

        /// <summary>
        /// Хранит информацию о номере партишена и пути к файлу.
        /// </summary>
        ConcurrentDictionary<int, byte[]> _fileParts;

        public IEnumerable<byte[]> GetFilePart()
        {
            lock (locker)
                while (!_fileParts.IsEmpty || !_isEnd)
                {
                    byte[] result;
                    while (!_fileParts.TryRemove(_lastPart, out result))
                    {
                        if (_isEnd)
                            break;
                        Monitor.Wait(locker);
                    }
                    if (result != null)
                    {
                        Console.WriteLine($"Merge: {_lastPart} part");
                        yield return result;
                        Interlocked.Increment(ref _lastPart);
                    }
                }
        }

        public void AppendPartInfo(int part, byte[] content)
        {
            Console.WriteLine($"Merge Append: {part} part");
            if (!_fileParts.TryAdd(part, content))
                throw new Exception("Что-то пошло нетак");
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
