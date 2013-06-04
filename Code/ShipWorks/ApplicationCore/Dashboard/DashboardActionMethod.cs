using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.Utility;
using ShipWorks.Common.Threading;

namespace ShipWorks.ApplicationCore.Dashboard
{
    /// <summary>
    /// A dashboard action that results in the invocation of a method.
    /// </summary>
    class DashboardActionMethod : DashboardAction
    {
        Action<Control, object> callback;
        object userState;

        /// <summary>
        /// The construcotr
        /// </summary>
        public DashboardActionMethod(string markup, Action callback)
            : this(markup, (Control control, object userState) => { callback(); })
        {

        }

        /// <summary>
        /// The construcotr
        /// </summary>
        public DashboardActionMethod(string markup, Action<Control, object> callback)
            : this(markup, callback, null)
        {

        }

        /// <summary>
        /// The construcotr
        /// </summary>
        public DashboardActionMethod(string markup, Action<Control, object> callback, object userState)
            : base(markup)
        {
            this.callback = callback;
            this.userState = userState;
        }

        /// <summary>
        /// Excecute the callback
        /// </summary>
        protected override void PerformAction(Control owner)
        {
            callback(owner, userState);
        }
    }
}
