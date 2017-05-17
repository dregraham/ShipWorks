using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShipWorks.Data.Administration
{
    /// <summary>
    /// View model for the New User Experience dialog
    /// </summary>
    public interface INewUserExperienceViewModel
    {
        /// <summary>
        /// Set owner interaction hooks
        /// </summary>
        void SetOwnerInteraction(IWin32Window win32Window, Action onClose);
    }
}
