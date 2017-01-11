using System.Windows.Forms;

namespace Interapptive.Shared.Utility
{
    /// <summary>
    /// Application utility methods
    /// </summary>
    public static class ApplicationUtility
    {
        /// <summary>
        /// Returns true if any open forms are modal.  False otherwise.
        /// </summary>
        /// <returns></returns>
        public static bool AnyModalDialogs()
        {
            foreach (Form form in Application.OpenForms)
            {
                if (form.Modal)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
