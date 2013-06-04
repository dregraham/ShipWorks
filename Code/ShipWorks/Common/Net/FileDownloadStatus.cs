using System;
using System.Collections.Generic;
using System.Text;

namespace ShipWorks.Common.Net
{
    /// <summary>
    /// Status of the file download
    /// </summary>
    public enum FileDownloadStatus
    {
        Downloading,
        Error,
        Complete,
        Canceled
    }
}
