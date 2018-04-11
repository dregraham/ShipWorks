using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
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

        /// <summary>
        /// Constructor
        /// </summary>
        public SingleDatabaseSelectorViewModel(Func<ISingleDatabaseSelectorDialog> createDialog, IMessageHelper messageHelper)
        {
            this.messageHelper = messageHelper;
            this.createDialog = createDialog;

            handler = new PropertyChangedHandler(this, () => PropertyChanged);
        }

        /// <summary>
        /// A property has changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

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

            Databases = databaseDetails;

            var dialog = createDialog();
            dialog.DataContext = this;

            return messageHelper.ShowDialog(dialog) == true ? SelectedDatabase : null;
        }
    }
}
