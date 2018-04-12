using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using ShipWorks.Core.UI;
using ShipWorks.Data.Administration;

namespace ShipWorks.Stores.Orders.Archive
{
    /// <summary>
    /// View model to select a single database
    /// </summary>
    [Component]
    public class SingleDatabaseSelectorViewModel : ISingleDatabaseSelectorViewModel, INotifyPropertyChanged
    {
        private readonly Func<ISingleDatabaseSelectorDialog> createDialog;
        private readonly IMessageHelper messageHelper;
        private readonly PropertyChangedHandler handler;

        private ISqlDatabaseDetail selectedDatabase;
        private ISingleDatabaseSelectorDialog dialog;

        /// <summary>
        /// Constructor
        /// </summary>
        public SingleDatabaseSelectorViewModel(Func<ISingleDatabaseSelectorDialog> createDialog, IMessageHelper messageHelper)
        {
            this.messageHelper = messageHelper;
            this.createDialog = createDialog;

            handler = new PropertyChangedHandler(this, () => PropertyChanged);

            Cancel = new RelayCommand(() => CloseDialog(dialog, false));
            Accept = new RelayCommand(() => CloseDialog(dialog, true), () => SelectedDatabase != null);
        }

        /// <summary>
        /// A property has changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Cancels the dialog
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand Cancel { get; }

        /// <summary>
        /// Accepts the dialog
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand Accept { get; }

        /// <summary>
        /// Gets the selected database chosen the user.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ISqlDatabaseDetail SelectedDatabase
        {
            get => selectedDatabase;
            set => handler.Set(nameof(SelectedDatabase), ref selectedDatabase, value);
        }

        /// <summary>
        /// List of database options
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IEnumerable<ISqlDatabaseDetail> Databases { get; set; }

        /// <summary>
        /// Gets whether a single database instance will be returned or not.
        /// </summary>
        public ISqlDatabaseDetail SelectSingleDatabase(IEnumerable<ISqlDatabaseDetail> databaseDetails)
        {
            if (databaseDetails.IsCountLessThan(2))
            {
                return databaseDetails.FirstOrDefault();
            }

            Databases = databaseDetails.OrderBy(x => x.Name);

            dialog = createDialog();
            dialog.DataContext = this;

            return messageHelper.ShowDialog(dialog) == true ? SelectedDatabase : null;
        }

        /// <summary>
        /// Close the dialog
        /// </summary>
        private void CloseDialog(ISingleDatabaseSelectorDialog localDialog, bool result)
        {
            if (localDialog != null)
            {
                localDialog.DialogResult = result;
                localDialog.Close();
            }
        }
    }
}
