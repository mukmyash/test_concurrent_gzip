using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ngzip.Exceptions
{
    public class NGZipInputFileNotFoundException : NGZipExceptionBase
    {
        public NGZipInputFileNotFoundException(string fileName)
        {
            FileName = fileName ?? throw new ArgumentNullException(nameof(fileName));
        }

        public string FileName { get; }
        public override string Description => $"Файл с именем '{FileName}' не найден.";

        public override IEnumerable<string> StepForFix
        {
            get
            {
                yield return $"A) Убедитесь что файл '{FileName}' существует.";
                yield return $"B) Убедитесь что файл '{FileName}' не заблокирован другим процессом .";
                yield return $"C) Переместите файл '{FileName}' в другое место.";
                yield return $"*) Свяжитесь с автором ПО.";
            }
        }
    }
}
