﻿using System;
using System.Reactive.Linq;
using System.Reflection;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Core.Messaging;
using ShipWorks.Messaging.Messages.SingleScan;

namespace ShipWorks.SingleScan.AutoPrintConfirmation
{
    /// <summary>
    /// ViewModel for <see cref="AutoPrintConfirmationDialog" />
    /// </summary>
    /// <seealso cref="T:ShipWorks.SingleScan.AutoPrintConfirmation.IAutoPrintConfirmationDlgViewModel" />\
    [Component]
    public class AutoPrintConfirmationDlgViewModel : IAutoPrintConfirmationDlgViewModel
    {
        private readonly IMessenger messenger;
        private IDisposable barcodeAcceptanceMessageSubscription;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoPrintConfirmationDlgViewModel"/> class.
        /// </summary>
        /// <param name="messenger">The messenger.</param>
        public AutoPrintConfirmationDlgViewModel(IMessenger messenger)
        {
            this.messenger = messenger;

            ContinueOptionalClickCommand = new RelayCommand(AcceptOptional);
            ContinueClickCommand = new RelayCommand(Accept);
            CancelClickCommand = new RelayCommand(Cancel);
        }

        /// <summary>
        /// Gets the optional continue click command.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand ContinueOptionalClickCommand { get; }

        /// <summary>
        /// Gets the continue click command.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand ContinueClickCommand { get; }

        /// <summary>
        /// Gets the cancel click command.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand CancelClickCommand { get; }

        /// <summary>
        /// Gets the text to display to the user.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string DisplayText { get; private set; }

        /// <summary>
        /// Gets the text to display to the user in the optional continue button.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string ContinueOptionalText { get; private set; }

        /// <summary>
        /// Gets the text to display to the user in the continue button.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string ContinueText { get; private set; }

        /// <summary>
        /// Gets or sets the method to close the window.
        /// </summary>
        public Action<bool?> Close { get; set; }

        /// <summary>
        /// Initializes the ViewModel with text to display and the barcode that when scan, accepts the dialog.
        /// </summary>
        public void Load(string barcodeAcceptanceText, string displayText, string continueText, string continueOptionalText)
        {
            DisplayText = displayText;
            ContinueText = continueText;
            ContinueOptionalText = continueOptionalText;
            barcodeAcceptanceMessageSubscription = messenger.OfType<SingleScanMessage>()
                .Where(x => x.ScannedText == barcodeAcceptanceText)
                .Subscribe(x => Accept());
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            barcodeAcceptanceMessageSubscription?.Dispose();
        }
        /// <summary>
        /// Accepts this instance with an optional third value.
        /// </summary>
        private void AcceptOptional()
        {
            Close?.Invoke(false);
        }

        /// <summary>
        /// Accepts this instance.
        /// </summary>
        private void Accept()
        {
            Close?.Invoke(true);
        }

        /// <summary>
        /// Cancels this instance.
        /// </summary>
        private void Cancel()
        {
            Close?.Invoke(null);
        }
    }
}