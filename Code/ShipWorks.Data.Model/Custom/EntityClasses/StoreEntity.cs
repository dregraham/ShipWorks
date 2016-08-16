using System;
using Interapptive.Shared.Business;
using ShipWorks.Stores;

namespace ShipWorks.Data.Model.EntityClasses
{
    public partial class StoreEntity
    {
        // Before setup is complete, to ensure the StoreName index is not violated when the store is saved, we
        // force a guid to be used as the name.  These two fields help this to be transparent to the Add Store Wizard.
        string preSetupCompleteInternalName = null;
        string preSetupCompleteUserName = null;

        // We cache this so we only have to look it up once
        static string baseObjectName = new StoreEntity().LLBLGenProEntityName;

        /// <summary>
        /// By default we will assume that setup is complete.  This is so that if we use StoreEntity as a 'Prototype' for saving a field or two only, that the code thinking
        /// that setup is not complete will not kick-in and change the store names to guids
        /// </summary>
        protected override void OnInitClassMembersComplete()
        {
            base.OnInitClassMembersComplete();

            Fields[(int) StoreFieldIndex.SetupComplete].ForcedCurrentValueWrite(true);
        }

        /// <summary>
        /// Special processing to ensure change tracking for entity hierarchy
        /// </summary>
        protected override void OnBeforeEntitySave()
        {
            // If this store is not yet setup, then we need to preserve its name as a Guid to prevent a IX_StoreName exception
            // from occurring before the user has picked\finalized the store name decision.
            if (!SetupComplete)
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

            if (!SetupComplete)
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
