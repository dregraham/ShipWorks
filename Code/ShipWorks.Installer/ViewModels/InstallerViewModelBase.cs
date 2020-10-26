using System;
using System.Reflection;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ShipWorks.Installer.Services;

namespace ShipWorks.Installer.ViewModels
{
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

        public NavigationPageType NextPage;

        public RelayCommand NextCommand => new RelayCommand(NextExecute, NextCanExecute);

        protected virtual void NextExecute()
        {
            throw new NotImplementedException();
        }

        protected virtual bool NextCanExecute()
        {
            throw new NotImplementedException();
        }
    }
}
