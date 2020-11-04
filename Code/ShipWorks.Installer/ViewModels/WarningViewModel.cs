using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using log4net;
using ShipWorks.Installer.Enums;
using ShipWorks.Installer.Extensions;
using ShipWorks.Installer.Models;
using ShipWorks.Installer.Services;

namespace ShipWorks.Installer.ViewModels
{
    /// <summary>
    /// View Model for the warning view
    /// </summary>
    [Obfuscation]
    public class WarningViewModel : InstallerViewModelBase
    {
        private string warning;
        private readonly Dictionary<InstallError, string> errorMessages = new Dictionary<InstallError, string>()
        {
            { InstallError.SystemCheck, "You don't meet the minimum system requirments:" },
            { InstallError.DownloadingShipWorks, "There was a problem downloading ShipWorks." },
            { InstallError.Database, "There was a problem installing or connecting to your database." },
            { InstallError.InstallShipWorks, "There was a problem installing ShipWorks." },
            { InstallError.Unknown, "An unknown error occured."  }
        };

        /// <summary>
        /// Constructor
        /// </summary>
        public WarningViewModel(MainViewModel mainViewModel, INavigationService<NavigationPageType> navigationService,
            Func<Type, ILog> logFactory) :
            base(mainViewModel, navigationService, NavigationPageType.UseShipWorks, logFactory(typeof(WarningViewModel)))
        {
            OpenWebsiteCommand = new RelayCommand(() => ProcessExtensions.StartWebProcess("https://support.shipworks.com/hc/en-us/requests/new"));

            var message = errorMessages[mainViewModel.InstallSettings.Error];
            if (mainViewModel.InstallSettings.Error == InstallError.SystemCheck)
            {
                message += BuildSystemCheckMessage(mainViewModel.InstallSettings.CheckSystemResult);
            }
            Warning = message;
        }

        public ICommand OpenWebsiteCommand { get; }
        /// <summary>
        /// The warning message to show
        /// </summary>
        public string Warning
        {
            get => warning;
            set => Set(ref warning, value);
        }

        /// <summary>
        /// Builds a system check error message based off the System Check result
        /// </summary>
        private string BuildSystemCheckMessage(SystemCheckResult systemCheck)
        {
            var message = "\n";

            if (!systemCheck.CpuMeetsRequirement)
            {
                message += $"\u2022 {systemCheck.CpuDescription}\n";
            }
            if (!systemCheck.RamMeetsRequirement)
            {
                message += $"\u2022 {systemCheck.RamDescription}\n";
            }
            if (!systemCheck.HddMeetsRequirement)
            {
                message += $"\u2022 {systemCheck.HddDescription}\n";
            }
            if (!systemCheck.OsMeetsRequirement)
            {
                message += $"\u2022 {systemCheck.OsDescription}\n";
            }

            return message;
        }
    }
}
