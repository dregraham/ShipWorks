﻿using Interapptive.Shared.Utility;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShipWorks.UI.Controls.ChannelConfirmDelete
{
    /// <summary>
    /// ViewModel for the ChannelLimitDlg
    /// </summary>
    public class ChannelConfirmDeleteViewModel : IConfirmChannelDeleteViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly PropertyChangedHandler handler;
        private readonly IStoreManager storeManager;
        private string message;
        private string intro;
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="storeType"></param>
        public ChannelConfirmDeleteViewModel(IStoreManager storeManager)
        {
            handler = new PropertyChangedHandler(this, () => PropertyChanged);
            this.storeManager = storeManager;
        }

        public void Load(StoreTypeCode storeType)
        {
            intro = $"Delete channel {EnumHelper.GetDescription(storeType)} and all of its content?";
            message = GetDeleteMessage(storeType);
        }

        private string GetDeleteMessage(StoreTypeCode storeType)
        {
            return $"I understand this permanently deletes all data for the channel, including stores ({GetStoreNamesToDelete(storeType)}), customers, orders, and shipments.";
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

            return builder.ToString();
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
    }
}
