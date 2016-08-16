using Interapptive.Shared.Utility;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace ShipWorks.UI.Controls.ChannelConfirmDelete
{
    /// <summary>
    /// ViewModel for the ChannelLimitDlg
    /// </summary>
    public class ChannelConfirmDeleteViewModel : IConfirmChannelDeleteViewModel, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly PropertyChangedHandler handler;
        private readonly IStoreManager storeManager;
        private string message;
        private string intro;


        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelConfirmDeleteViewModel"/> class.
        /// </summary>
        /// <param name="storeManager">The IStoreManager to use for getting a list of all stores in ShipWorks.</param>
        public ChannelConfirmDeleteViewModel(IStoreManager storeManager)
        {
            handler = new PropertyChangedHandler(this, () => PropertyChanged);
            this.storeManager = storeManager;
        }

        /// <summary>
        /// The intro message to display to the user
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string Intro
        {
            get { return intro; }
            set { handler.Set(nameof(Intro), ref intro, value); }
        }

        /// <summary>
        /// The delete message to display to the user
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string Message
        {
            get { return message; }
            set { handler.Set(nameof(Message), ref message, value); }
        }

        /// <summary>
        /// Load the view model
        /// </summary>
        public void Load(StoreTypeCode storeType)
        {
            intro = $"Delete {EnumHelper.GetDescription(storeType)} channel and all of its content?";
            message = GetDeleteMessage(storeType);
        }

        /// <summary>
        /// Gets appropriate messaging for store type
        /// </summary>
        private string GetDeleteMessage(StoreTypeCode storeType)
        {
            return $"I understand this permanently deletes all data for the channel, including stores{GetStoreNamesToDelete(storeType)}, customers, orders, and shipments.";
        }

        /// <summary>
        /// Creates the delete message
        /// </summary>
        private string GetStoreNamesToDelete(StoreTypeCode storeType)
        {
            StringBuilder builder = new StringBuilder();

            IEnumerable<StoreEntity> storesToDelete = storeManager.GetAllStores().Where(s => s.TypeCode == (int)storeType);

            foreach (StoreEntity store in storesToDelete)
            {
                if (builder.Length != 0)
                {
                    builder.Append(", ");
                }
                builder.Append(store.StoreName);
            }

            if (builder.Length != 0)
            {
                builder.Insert(0, " (");
                builder.Append(")");
            }

            return builder.ToString();
        }
    }
}
