using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Interapptive.Shared.Extensions
{
    /// <summary>
    /// Extension methods for Tasks
    /// </summary>
    public static class TaskExtensions
    {
        /// <summary>
        /// Re-throw an exception on the UI thread
        /// </summary>
        public static Task RethrowException(this Task task, Func<Control> getOwner)
        {
            return task.ContinueWith(x =>
            {
                if (x.IsFaulted && x.Exception != null)
                {
                    getOwner().BeginInvoke((Action) (() => { throw x.Exception; }));
                }
            });
        }
    }
}
