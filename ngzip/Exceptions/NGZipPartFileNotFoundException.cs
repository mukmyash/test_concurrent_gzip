using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ngzip.Exceptions
{
    class NGZipPartFileNotFoundException : NGZipExceptionBase
    {
        public NGZipPartFileNotFoundException(string fileName)
        {
            FileName = fileName ?? throw new ArgumentNullException(nameof(fileName));
        }

        public string FileName { get; }
        public override string Description => $"Одна из партиций файлов отсутствует '{FileName}'.";

        public override IEnumerable<string> StepForFix
        {
            get
            {
                yield return $"A) Убедитесь что у вас есть права на доступ к каталогу '{new DirectoryInfo(Path.Combine(".", "partition")).FullName}'.";
                yield return $"*) Свяжитесь с автором ПО.";
            }
        }
    }
}
