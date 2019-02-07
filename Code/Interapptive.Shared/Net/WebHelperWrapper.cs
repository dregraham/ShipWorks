using System;
using System.ComponentModel;
using System.Windows.Forms;
using Interapptive.Shared.ComponentRegistration;

namespace Interapptive.Shared.Net
{
    /// <summary>
    /// Utility class for working with the www
    /// </summary>
    [Component]
    public class WebHelperWrapper : IWebHelper
    {
        private readonly Func<Control> ownerFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public WebHelperWrapper(Func<Control> ownerFactory)
        {
            this.ownerFactory = ownerFactory;
        }

        /// <summary>
        /// Open the given URL in the users default browser.
        /// </summary>
        public void OpenUrl(string uri) => OpenUrl(new Uri(uri));

        /// <summary>
        /// Open the given URL in the users default browser.
        /// </summary>
        public void OpenUrl(Uri uri) =>
            WebHelper.OpenUrl(uri, ownerFactory());
    }
}
