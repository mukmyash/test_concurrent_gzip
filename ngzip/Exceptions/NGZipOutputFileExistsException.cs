using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ngzip.Exceptions
{
    public class NGZipOutputFileExistsException : NGZipExceptionBase
    {
        public NGZipOutputFileExistsException(string fileName)
        {
            FileName = fileName ?? throw new ArgumentNullException(nameof(fileName));
        }

        public string FileName { get; }
        public override string Description => $"Файл с именем '{FileName}' существует.";

        public override IEnumerable<string> StepForFix
        {
            get
            {
                yield return $"A) Удалите файл '{FileName}'.";
                yield return $"B) Переименуйте файл '{FileName}'.";
                yield return $"C) Переместите файл '{FileName}' в другое место.";
                yield return $"*) Свяжитесь с автором ПО.";
            }
        }
    }
}
