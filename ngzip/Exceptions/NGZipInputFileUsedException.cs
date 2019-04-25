using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ngzip.Exceptions
{
    public class NGZipInputFileUsedException : NGZipExceptionBase
    {
        public NGZipInputFileUsedException(string fileName)
        {
            FileName = fileName ?? throw new ArgumentNullException(nameof(fileName));
        }

        public string FileName { get; }
        public override string Description => $"Файл '{FileName}' используется другим процессом.";

        public override IEnumerable<string> StepForFix
        {
            get
            {
                yield return $"A) Закройте файл '{FileName}' в другой программе.";
                yield return $"*) Свяжитесь с автором ПО.";
            }
        }
    }
}
