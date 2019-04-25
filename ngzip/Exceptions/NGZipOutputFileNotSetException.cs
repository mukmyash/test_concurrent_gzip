using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ngzip.Exceptions
{
    public class NGZipOutputFileNotSetException : NGZipExceptionBase
    {
        public NGZipOutputFileNotSetException()
        {
        }

        public override string Description => $"Не указано имя выходного файла.";

        public override IEnumerable<string> StepForFix
        {
            get
            {
                yield return $"A) Укажите имя выходного файла.";
                yield return $"*) Свяжитесь с автором ПО.";
            }
        }
    }
}
