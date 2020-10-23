using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using GalaSoft.MvvmLight;
using ShipWorks.Installer.Services;

namespace ShipWorks.Installer.Models
{
    /// <summary>
    /// DTO for current install settings
    /// </summary>
    public class InstallSettings : ObservableObject
    {
        private SystemCheckResult checkSystemResult;

        /// <summary>
        /// Results from system check
        /// </summary>
        public SystemCheckResult CheckSystemResult
        {
            get => checkSystemResult;
            set => Set(ref checkSystemResult, value);
        }
    }
}
