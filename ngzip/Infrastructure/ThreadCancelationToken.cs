using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ngzip.Infrastructure
{
    public class ThreadCancelationToken
    {
        public delegate void OnCancelEventHandler(Exception e);
        public event OnCancelEventHandler OnCancel;

        public bool IsCancel { get; private set; }
        public void Cancel(Exception ex)
        {
            if (IsCancel)
                return;

            IsCancel = true;
            OnCancel?.Invoke(ex);
        }
    }
}
