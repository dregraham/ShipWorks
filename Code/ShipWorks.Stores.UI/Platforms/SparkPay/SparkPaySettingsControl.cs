using System;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.UI.Platforms.SparkPay.WizardPages;

namespace ShipWorks.Stores.UI.Platforms.SparkPay
{
    /// <summary>
    /// Represents a settings control for the Sparkpay store
    /// </summary>
    public partial class SparkPaySettingsControl : AccountSettingsControlBase
    {
        private readonly ISparkPayAccountViewModel viewModel;

        /// <summary>
        /// Constructor
        /// </summary>
        private SparkPaySettingsControl(ISparkPayAccountViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;
            sparkPayAccountControl.DataContext = viewModel;
        }

        /// <summary>
        ///     Load store settings from the entity to the GUI
        /// </summary>
        public override void LoadStore(StoreEntity store)
        {
            SparkPayStoreEntity sparkPayStore = store as SparkPayStoreEntity;
            if (sparkPayStore == null)
            {
                throw new ArgumentException("A non SparkPay store was passed to SparkPay store account settings.");
            }

            viewModel.Load(sparkPayStore);
        }

        /// <summary>
        /// Saves the user selected settings back to the store entity
        /// </summary>
        public override bool SaveToEntity(StoreEntity store)
        {
            SparkPayStoreEntity sparkPayStore = store as SparkPayStoreEntity;
            if (sparkPayStore == null)
            {
                throw new ArgumentException("A non SparkPay store was passed to SparkPay store account settings.");
            }

            return viewModel.Save(sparkPayStore);
        }

        /// <summary>
        ///     For determining if the connection needs to be tested
        /// </summary>
        protected virtual bool ConnectionVerificationNeeded(SparkPayStoreEntity store)
        {
            return store.Fields[(int) SparkPayStoreFieldIndex.Token].IsChanged ||
                   store.Fields[(int) SparkPayStoreFieldIndex.StoreUrl].IsChanged;
        }
    }
}