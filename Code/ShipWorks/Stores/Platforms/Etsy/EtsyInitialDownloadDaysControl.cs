using System;
using System.ComponentModel;
using Interapptive.Shared.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;

namespace ShipWorks.Stores.Platforms.Etsy
{
	/// <summary>
	/// Allow the user to select the country that their Etsy store is in
	/// </summary>
	[ToolboxItem(true)]
    public partial class EtsyInitialDownloadDaysControl : AccountSettingsControlBase
    {
        private InitialDownloadPolicy policy;

        /// <summary>
        /// Constructor
        /// </summary>
        public EtsyInitialDownloadDaysControl()
        {
            InitializeComponent();
            policy = new InitialDownloadPolicy(InitialDownloadRestrictionType.DaysBack)
            {
                DefaultDaysBack = 14,
                MaxDaysBack = 30
            };
        }

        /// <summary>
        /// Load settings from the store entity
        /// </summary>
        public override void LoadStore(StoreEntity store)
        {
            // validate we get an EtsyStoreEntity
            var amazonStore = store as EtsyStoreEntity;
            if (amazonStore == null)
            {
                throw new ArgumentException("EtsyStoreEntity expected.", nameof(store));
            }

            textBoxInitialDays.Text = amazonStore.InitialDownloadDays.ToString();

            base.LoadStore(store);
        }

        /// <summary>
        /// Save user-entered values back to the entity
        /// </summary>
        public override bool SaveToEntity(StoreEntity store)
        {
            // validate we get an EtsyStoreEntity
            var amazonStore = store as EtsyStoreEntity;
            if (amazonStore == null)
            {
                throw new ArgumentException("EtsyStoreEntity expected.", nameof(store));
            }

            // Make sure the entered something
            if (textBoxInitialDays.Text.Trim().Length == 0)
            {
                MessageHelper.ShowInformation(this, "Enter the number of days ShipWorks should go back.");
                return false;
            }

            if (!int.TryParse(textBoxInitialDays.Text.Trim(), out var daysBack))
            {
                MessageHelper.ShowInformation(this, "Enter a valid number of days.");
                return false;
            }

            if (daysBack <= 0 || policy.MaxDaysBack != null && daysBack > policy.MaxDaysBack)
            {
                if (policy.MaxDaysBack != null)
                {
                    MessageHelper.ShowInformation(this,$"The number of days back must be between 1 and {policy.MaxDaysBack}.");
                }
                else
                {
                    MessageHelper.ShowInformation(this, "The number of days back must be greater than zero.");
                }

                return false;
            }

            amazonStore.InitialDownloadDays = daysBack;

            return true;
        }
    }
}
