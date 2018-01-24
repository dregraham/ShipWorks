using System;
using ShipWorks.Stores.Communication;

namespace ShipWorks.Stores.Platforms.Odbc.Download
{
    /// <summary>
    /// OnDemandDownloadException
    /// </summary>
    public class OnDemandDownloadException : DownloadException
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OnDemandDownloadException(bool showPopup, string message, Exception ex)
            : base(message, ex)
        {
            ShowPopup = showPopup;
        }

        /// <summary>
        /// Should we show a popup when this exception is thrown?
        /// </summary>
        public bool ShowPopup { get; }
    }
}
