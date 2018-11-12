using System;
using Interapptive.Shared.Business;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Stores;

namespace ShipWorks.Data.Model.EntityClasses
{
    public partial class StoreEntity
    {
        // Before setup is complete, to ensure the StoreName index is not violated when the store is saved, we
        // force a guid to be used as the name.  These two fields help this to be transparent to the Add Store Wizard.
        string preSetupCompleteInternalName = null;
        string preSetupCompleteUserName = null;
        private bool isSettingUp;

        // We cache this so we only have to look it up once
        static string baseObjectName = ((IEntityCore) new StoreEntity()).LLBLGenProEntityName;

        /// <summary>
        /// Start setting up the store
        /// </summary>
        /// <remarks>
        /// Start the setup process which ensures a valid, unique name by setting
        /// it to a guid instead of the user selection.
        /// </remarks>
        public void StartSetup()
        {
            isSettingUp = true;
            SetupComplete = false;
        }

        /// <summary>
        /// Finish setting up the store
        /// </summary>
        /// <remarks>
        /// This will stop the store name guid override
        /// </remarks>
        public void CompleteSetup()
        {
            isSettingUp = false;
            SetupComplete = true;
        }

        /// <summary>
        /// Special processing to ensure change tracking for entity hierarchy
        /// </summary>
        protected override void OnBeforeEntitySave()
        {
            // If this store is not yet setup, then we need to preserve its name as a Guid to prevent a IX_StoreName exception
            // from occurring before the user has picked\finalized the store name decision.
            if (isSettingUp)
            {
                // If the user has selected a name remember it
                preSetupCompleteUserName = StoreName ?? string.Empty;

                // Lazy create our temporary store name guid
                if (preSetupCompleteInternalName == null)
                {
                    preSetupCompleteInternalName = Guid.NewGuid().ToString();
                }

                // Force the guid to be used as the name so their is no index violation before setup is complete
                StoreName = preSetupCompleteInternalName;
            }

            if (!HasBaseDirtyField(baseObjectName))
            {
                // Force the timestamp to update
                Fields[(int) StoreFieldIndex.TypeCode].IsChanged = true;
                Fields.IsDirty = true;
            }

            base.OnBeforeEntitySave();
        }

        /// <summary>
        /// Called after a successful save has completed
        /// </summary>
        protected override void OnValidateEntityAfterSave()
        {
            base.OnValidateEntityAfterSave();

            if (isSettingUp && !string.IsNullOrEmpty(preSetupCompleteUserName))
            {
                // Restore the name the user wanted
                StoreName = preSetupCompleteUserName;
            }
        }

        /// <summary>
        /// Address as a person adapter
        /// </summary>
        public PersonAdapter Address
        {
            get { return new PersonAdapter(this, string.Empty); }
            set { PersonAdapter.Copy(value, Address); }
        }

        /// <summary>
        /// Strongly typed store type code
        /// </summary>
        public StoreTypeCode StoreTypeCode
        {
            get { return (StoreTypeCode) TypeCode; }
            set { TypeCode = (int) value; }
        }
    }
}
