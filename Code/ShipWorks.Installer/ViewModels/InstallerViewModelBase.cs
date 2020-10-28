using System;
using System.Reflection;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ShipWorks.Installer.Enums;
using ShipWorks.Installer.Services;

namespace ShipWorks.Installer.ViewModels
{
    /// <summary>
    /// Base class for installer view models
    /// </summary>
    [Obfuscation]
    public abstract class InstallerViewModelBase : ViewModelBase
    {
        protected readonly MainViewModel mainViewModel;
        protected readonly INavigationService<NavigationPageType> navigationService;

        public InstallerViewModelBase(MainViewModel mainViewModel,
            INavigationService<NavigationPageType> navigationService,
            NavigationPageType nextPage)
        {
            this.navigationService = navigationService;
            this.mainViewModel = mainViewModel;
            NextPage = nextPage;
        }

        /// <summary>
        /// The page to go to when NextCommand is called
        /// </summary>
        public NavigationPageType NextPage;

        /// <summary>
        /// Command for going to the next page
        /// </summary>
        public RelayCommand NextCommand => new RelayCommand(NextExecute, NextCanExecute);

        /// <summary>
        /// Command for going to the previous page
        /// </summary>
        public RelayCommand BackCommand => new RelayCommand(navigationService.GoBack);

        /// <summary>
        /// Command handler for the NextCommand
        /// </summary>
        protected virtual void NextExecute()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Determines if the NextCommand can execute
        /// </summary>
        protected virtual bool NextCanExecute()
        {
            throw new NotImplementedException();
        }
    }
}
