using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ngzip.Exceptions
{
    public class NGZipInputFileNotSetException : NGZipExceptionBase
    {
        public NGZipInputFileNotSetException()
        {
        }

        public override string Description => $"Не указано имя файла для архивации.";

        public override IEnumerable<string> StepForFix
        {
            get
            {
                yield return $"Укажите имя файла для архивации.";
            }
        }
    }
}
