﻿using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using log4net;
using ShipWorks.Installer.Enums;
using ShipWorks.Installer.Services;
using ShipWorks.Installer.Utilities;

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
        protected readonly ILog log;

        public InstallerViewModelBase(MainViewModel mainViewModel,
            INavigationService<NavigationPageType> navigationService,
            NavigationPageType nextPage, ILog log)
        {
            this.log = log;
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
        public RelayCommand BackCommand => new RelayCommand(BackExecute);

        /// <summary>
        /// Command for canceling the dialog
        /// </summary>
        public RelayCommand CancelCommand => new RelayCommand(CancelExecute);

        /// <summary>
        /// Command for going to the next page
        /// </summary>
        public IAsyncCommand NextCommandAsync => new AsyncCommand(NextExecuteAsync, NextCanExecute, log);

        /// <summary>
        /// Command handler for the NextCommand
        /// </summary>
        protected virtual Task NextExecuteAsync()
        {
            throw new NotImplementedException();
        }

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

        /// <summary>
        /// Command handler for the back command
        /// </summary>
        protected virtual void BackExecute()
        {
            navigationService.GoBack();
        }

        /// <summary>
        /// Command handler for the cancel command
        /// </summary>
        public void CancelExecute()
        {
            if (!mainViewModel.IsClosing)
            {
                var shouldClose = mainViewModel.Close(false);

                if (shouldClose)
                {
                    Application.Current.MainWindow.Close();
                }
            }
        }
    }
}
