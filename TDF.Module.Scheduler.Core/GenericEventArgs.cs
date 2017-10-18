using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDF.Module.Scheduler.Core
{
    public class GenericEventArgs<T> : EventArgs
    {
        public T Data { get; set; }

        public GenericEventArgs(T data)
        {
            this.Data = data;
        }
    }
}
