﻿using System;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Input;
using Autofac.Features.Indexed;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Settings.OneBalance;

namespace ShipWorks.Shipping.UI.Settings.OneBalance
{
    /// <summary>
    /// View model for the controls that need to show a setup wizard
    /// </summary>
    [KeyedComponent(typeof(IOneBalanceShowSetupWizardViewModel), ShipmentTypeCode.UpsOnLineTools)]
    public class OneBalanceShowSetupDialogViewModel : ViewModelBase, IOneBalanceShowSetupWizardViewModel
    {
        private readonly IIndex<ShipmentTypeCode, IOneBalanceSetupWizard> setupWizardFactory;
        private readonly IWin32Window window;
        private readonly IOneBalanceAccountHelper accountHelper;

        public event EventHandler SetupComplete;

        /// <summary>
        /// Constructor
        /// </summary>
        public OneBalanceShowSetupDialogViewModel(IIndex<ShipmentTypeCode, IOneBalanceSetupWizard> setupWizardFactory,
            IWin32Window window,
            IOneBalanceAccountHelper accountHelper)
        {
            this.setupWizardFactory = setupWizardFactory;
            this.window = window;
            this.accountHelper = accountHelper;
            ShowSetupWizardCommand = new RelayCommand<ShipmentTypeCode>((shipmentTypeCode) => InternalShowSetupWizard(shipmentTypeCode));
        }

        /// <summary>
        /// RelayCommand for Showing the setup wizard
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand ShowSetupWizardCommand { get; }

        /// <summary>
        /// This is to avoid CA2214: Do not call overridable methods in constructors
        /// </summary>
        /// <param name="shipmentTypeCode"></param>
        private void InternalShowSetupWizard(ShipmentTypeCode shipmentTypeCode)
        {
            ShowSetupWizard(shipmentTypeCode);
        }

        /// <summary>
        /// Shows the setup wizard to the user
        /// </summary>
        protected virtual void ShowSetupWizard(ShipmentTypeCode shipmentTypeCode)
        {
            var result = accountHelper.GetUspsAccount(shipmentTypeCode);
            if (result.Failure)
            {
                MessageHelper.ShowError(window, result.Exception.Message);
                return;
            }

            var setupWizard = setupWizardFactory[shipmentTypeCode];
            setupWizard.SetupOneBalanceAccount(window);
            RaiseSetupComplete();
        }

        /// <summary>
        /// Raises the SetupComplete event
        /// </summary>
        private void RaiseSetupComplete()
        {
            SetupComplete?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Refreshes the account dependent attributes of the controls 
        /// </summary>
        public virtual void Refresh()
        {
            // Currently this method doesn't need to do anything in this base class
        }
    }
}