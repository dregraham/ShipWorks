﻿using System;
using System.Reflection;
using System.Timers;
using System.Windows;
using FontAwesome5;
using log4net;
using ShipWorks.Installer.Enums;
using ShipWorks.Installer.Models;
using ShipWorks.Installer.Services;

namespace ShipWorks.Installer.ViewModels
{
    /// <summary>
    /// View model for system check
    /// </summary>
    [Obfuscation]
    public class SystemCheckViewModel : InstallerViewModelBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SystemCheckViewModel(MainViewModel mainViewModel,
            INavigationService<NavigationPageType> navigationService,
            ISystemCheckService systemCheckService,
            Func<Type, ILog> logFactory) :
            base(mainViewModel, navigationService, NavigationPageType.Eula, logFactory(typeof(SystemCheckViewModel)))
        {
            var result = systemCheckService.CheckSystem();
            mainViewModel.InstallSettings.CheckSystemResult = result;

            if (!result.AllRequirementsMet)
            {
                mainViewModel.InstallSettings.Error = InstallError.SystemCheck;
            }

            //Wait for a second because this can happen so fast that the screen will flash
            Timer t = new Timer()
            {
                Interval = 1000,
                AutoReset = false,
            };

            t.Elapsed += (object sender, ElapsedEventArgs e) => Application.Current.Dispatcher.Invoke(() => ProcessResult(result));
            t.Start();
        }

        /// <summary>
        /// Process the result
        /// </summary>
        private void ProcessResult(SystemCheckResult result)
        {
            mainViewModel.InstallSettings.CheckSystemResult = result;

            if (NextCanExecute())
            {
                MoveNext();
            }
            else
            {
                MoveToWarn();
            }
        }

        /// <summary>
        /// Move to the next page
        /// </summary>
        private void MoveNext()
        {
            mainViewModel.SystemCheckIcon = EFontAwesomeIcon.Regular_CheckCircle;
            navigationService.NavigateTo(NextPage);
        }

        /// <summary>
        /// Move to the warning page
        /// </summary>
        private void MoveToWarn()
        {
            mainViewModel.SystemCheckIcon = EFontAwesomeIcon.Solid_ExclamationCircle;
            mainViewModel.UseShipWorksIcon = EFontAwesomeIcon.Solid_ExclamationCircle;
            navigationService.NavigateTo(NavigationPageType.Warning);
        }

        /// <summary>
        /// NextExecute command handler
        /// </summary>
        protected override void NextExecute()
        {
            MoveNext();
        }

        /// <summary>
        /// CanNextExecute handler
        /// </summary>
        protected override bool NextCanExecute()
        {
            var checkSystemResult = mainViewModel.InstallSettings?.CheckSystemResult;
            return checkSystemResult != null && checkSystemResult.AllRequirementsMet;
        }
    }
}
