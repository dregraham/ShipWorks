using System;
using Interapptive.Shared.Collections;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.Custom;
using System.Data.SqlTypes;

namespace ShipWorks.Data.Model.EntityClasses
{
    partial class CommonEntityBase
    {
        static readonly ILog log = LogManager.GetLogger(typeof(CommonEntityBase));

        bool ignoreConcurrency = false;

        // Indicates if we are in process of saving a new entity or just updating
        bool savingNewEntity = false;

        /// <summary>
        /// Raised when an entity is persisted to the database - deleted or saved
        /// </summary>
        public static event EntityPersistedEventHandler EntityPersisted;

        /// <summary>
        /// Controls if this Entity ignores timestamp concurrency checking.  Only matters if the entity class is already registered
        /// with TimestampConcurrencyFactory.
        /// </summary>
        public bool IgnoreConcurrency
        {
            get { return ignoreConcurrency; }
            set { ignoreConcurrency = value; }
        }

        /// <summary>
        /// Rollback all fields to what they were when loaded first loaded from the database.  If the entity
        /// is new, nothing is done.
        /// </summary>
        public void RollbackChanges()
        {
            if (IsNew)
            {
                return;
            }

            if (!IsDirty)
            {
                return;
            }

            foreach (EntityField2 field in Fields)
            {
                field.ForcedCurrentValueWrite(field.DbValue, field.DbValue);
                field.IsChanged = false;
            }

            IsDirty = false;
        }

        /// <summary>
        /// Used to quickly initialize all null strings in an entity to the empty string, in cases
        /// where not all fields are convenient to set before saving.
        /// </summary>
        public void InitializeNullsToDefault()
        {
            foreach (EntityField2 field in Fields)
            {
                if (!field.IsPrimaryKey && !field.IsReadOnly && !field.IsNullable && field.CurrentValue == null)
                {
                    if (field.DataType == typeof(string))
                    {
                        field.CurrentValue = "";
                    }

                    if (field.DataType.IsEnum)
                    {
                        field.CurrentValue = 0;
                    }

                    if (field.DataType == typeof(double) ||
                        field.DataType == typeof(float) ||
                        field.DataType == typeof(decimal) ||
                        field.DataType == typeof(int) ||
                        field.DataType == typeof(long) ||
                        field.DataType == typeof(short))
                    {
                        field.CurrentValue = Convert.ChangeType(0, field.DataType);
                    }

                    if (field.DataType == typeof(bool))
                    {
                        field.CurrentValue = false;
                    }

                    if (field.DataType == typeof(DateTime))
                    {
                        field.CurrentValue = SqlDateTime.MinValue.Value;
                    }
                }
            }
        }

        /// <summary>
        /// PreProcess a value before it gets set.
        /// </summary>
        protected override void PreProcessValueToSet(IFieldInfo fieldToSet, ref object valueToSet)
        {
            //
            // Truncate so that we don't get string overflow exceptions.
            //
            // At first I really wanted to avoid doing this so that instead of truncations we'd see the crashes
            // and then we could better deal with it.  But dealing with real world data just seemed to make this not possible.
            // Some data was just soo much longer than we should allow for certain fields, that we couldnt just "make it big enough",
            // and since we're not going to make it big enough - the only other option is to truncate.
            string text = valueToSet as string;
            if (text != null && fieldToSet.MaxLength > 0 && text.Length > fieldToSet.MaxLength)
            {
                string truncated = text.Substring(0, fieldToSet.MaxLength);

                string warning = string.Format("Truncating {6}.{7} from {0} to {1} - {2}.{3} from '{4}' to '{5}'", text.Length, fieldToSet.MaxLength, fieldToSet.ContainingObjectName, fieldToSet.Name, text, truncated, fieldToSet.ContainingObjectName, fieldToSet.Name);

                log.Warn(warning);

                // Update the value to be set
                valueToSet = truncated;
            }

            base.PreProcessValueToSet(fieldToSet, ref valueToSet);
        }

        /// <summary>
        /// Indicates if this entity has a field that is in the table at the base of the entity hierarchy that is dirty.
        /// </summary>
        protected bool HasBaseDirtyField(string baseObjectName)
        {
            foreach (EntityField2 field in Fields)
            {
                if (field.IsChanged)
                {
                    // If these are different, its a base field been promoted to the derived type.  So
                    // we know its a base field that is dirty.
                    if (field.ContainingObjectName == baseObjectName)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Called before the entity starts to save
        /// </summary>
        protected override void OnValidateEntityBeforeSave()
        {
            savingNewEntity = this.IsNew;

            base.OnValidateEntityBeforeSave();
        }

        /// <summary>
        /// Called after entity save has completed
        /// </summary>
        protected override void OnValidateEntityAfterSave()
        {
            base.OnValidateEntityAfterSave();

            RaiseEntityPersisted(this, savingNewEntity ? EntityPersistedAction.Insert : EntityPersistedAction.Update);
        }

        /// <summary>
        /// This entity has been deleted
        /// </summary>
        protected override void OnAuditDeleteOfEntity()
        {
            base.OnAuditDeleteOfEntity();

            RaiseEntityPersisted(this, EntityPersistedAction.Delete);
        }

        /// <summary>
        /// After a value has been set
        /// </summary>
        /// <remarks>This will reset the dirty flag if the new value matches what we expect is in the db</remarks>
        protected override void OnSetValueComplete(int fieldIndex)
        {
            base.OnSetValueComplete(fieldIndex);

            IEntityField2 field = Fields[fieldIndex];
            if (!IsNew && field.IsChanged && FieldUtilities.ValuesAreEqual(field.DbValue, field.CurrentValue))
            {
                field.IsChanged = false;

                if (Fields.None(x => x.IsChanged))
                {
                    Fields.IsDirty = false;
                    IsDirty = false;
                }
            }
        }

        /// <summary>
        /// Raise the EntityPersisted event
        /// </summary>
        private static void RaiseEntityPersisted(CommonEntityBase entity, EntityPersistedAction action)
        {
            EntityPersisted?.Invoke(entity, new EntityPersistedEventArgs(action));
        }
    }
}
