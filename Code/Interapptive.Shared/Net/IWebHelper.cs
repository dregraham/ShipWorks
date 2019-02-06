using System;

namespace Interapptive.Shared.Net
{
    /// <summary>
    /// Utility class for working with the www
    /// </summary>
    public interface IWebHelper
    {
        /// <summary>
        /// Open the given URL in the users default browser.
        /// </summary>
        void OpenUrl(string uri);

        /// <summary>
        /// Open the given URL in the users default browser.
        /// </summary>
        void OpenUrl(Uri uri);
    }
}
