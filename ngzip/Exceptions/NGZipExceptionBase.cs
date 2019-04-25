using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ngzip.Exceptions
{
    public abstract class NGZipExceptionBase : Exception
    {
        public abstract string Description { get; }
        public abstract IEnumerable<string> StepForFix { get; }

        public override string ToString()
        {
            var stepForFix = string.Join(Environment.NewLine, StepForFix);

            return string.Concat(
                "MESSAGE", Environment.NewLine, Message, Environment.NewLine, Environment.NewLine,
                "DESCRIPTION", Environment.NewLine, Description, Environment.NewLine, Environment.NewLine,
                "STEPS FOR FIX", Environment.NewLine, stepForFix, Environment.NewLine, Environment.NewLine);
        }
    }
}
