﻿using System.Collections.Generic;
using System.Reflection;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.UI.RatingPanel
{
    /// <summary>
    /// Properties for the rating panel view model
    /// </summary>
    public partial class RatingPanelViewModel
    {
        private bool showAccount;
        private bool showShipping;
        private bool showTaxes;
        private bool showDuties;
        private bool showFootnotes;
        private bool isLoading;
        private bool showEmptyMessage;
        private string emptyMessage;
        private bool allowSelection;

        private IEnumerable<RateResult> rates;
        private IEnumerable<object> footnotes;

        /// <summary>
        /// List of rates that should be displayed
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IEnumerable<RateResult> Rates
        {
            get { return rates; }
            set { handler.Set(nameof(Rates), ref rates, value); }
        }

        /// <summary>
        /// Should the account column be displayed
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool ShowAccount
        {
            get => showAccount;
            set => handler.Set(nameof(ShowAccount), ref showAccount, value);
        }

        /// <summary>
        /// Should the shipping column be displayed
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool ShowShipping
        {
            get { return showShipping; }
            set { handler.Set(nameof(ShowShipping), ref showShipping, value); }
        }

        /// <summary>
        /// Should the taxes column be displayed
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool ShowTaxes
        {
            get { return showTaxes; }
            set { handler.Set(nameof(ShowTaxes), ref showTaxes, value); }
        }

        /// <summary>
        /// Should the duties column be displayed
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool ShowDuties
        {
            get { return showDuties; }
            set { handler.Set(nameof(ShowDuties), ref showDuties, value); }
        }

        /// <summary>
        /// Should footnotes be displayed
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool ShowFootnotes
        {
            get { return showFootnotes; }
            set { handler.Set(nameof(ShowFootnotes), ref showFootnotes, value); }
        }

        /// <summary>
        /// List of footnotes
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IEnumerable<object> Footnotes
        {
            get { return footnotes; }
            set { handler.Set(nameof(Footnotes), ref footnotes, value); }
        }

        /// <summary>
        /// Are rates currently being loaded
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool IsLoading
        {
            get { return isLoading; }
            set { handler.Set(nameof(IsLoading), ref isLoading, value); }
        }

        /// <summary>
        /// Are rates allowed to be selected
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool AllowSelection
        {
            get { return allowSelection; }
            set { handler.Set(nameof(AllowSelection), ref allowSelection, value); }
        }

        /// <summary>
        /// Should the empty message be shown
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool ShowEmptyMessage
        {
            get { return showEmptyMessage; }
            set { handler.Set(nameof(ShowEmptyMessage), ref showEmptyMessage, value); }
        }

        /// <summary>
        /// Empty message to be shown
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string EmptyMessage
        {
            get { return emptyMessage; }
            set { handler.Set(nameof(EmptyMessage), ref emptyMessage, value); }
        }
    }
}
