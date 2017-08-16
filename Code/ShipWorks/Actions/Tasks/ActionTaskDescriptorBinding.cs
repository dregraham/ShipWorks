using System;
using System.Reflection;
using Autofac;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.ApplicationCore;
using ShipWorks.Data;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores;

namespace ShipWorks.Actions.Tasks
{
    /// <summary>
    /// Binds an ActionTaskDescriptor to the StoreType or store instance it will go with.  If the descriptor
    /// is storetype or instance neutral, then it's just not bound to anything.
    /// </summary>
    public class ActionTaskDescriptorBinding
    {
        ActionTaskDescriptor descriptor;

        StoreTypeCode? storeTypeCode;
        long? storeID;

        /// <summary>
        /// Constructor for a descriptor that is not bound to anything
        /// </summary>
        public ActionTaskDescriptorBinding(ActionTaskDescriptor descriptor)
        {
            this.descriptor = descriptor;
        }

        /// <summary>
        /// Constructor for a descriptor that is bound to the given type code
        /// </summary>
        public ActionTaskDescriptorBinding(ActionTaskDescriptor descriptor, StoreTypeCode storeTypeCode)
        {
            this.descriptor = descriptor;
            this.storeTypeCode = storeTypeCode;
        }

        /// <summary>
        /// Constructor for a descriptor that is bound to the given store id
        /// </summary>
        public ActionTaskDescriptorBinding(ActionTaskDescriptor descriptor, long storeID)
        {
            this.descriptor = descriptor;
            this.storeID = storeID;
        }

        /// <summary>
        /// Constructor for a task of the given type bound to the given store.  It is automatically determined whether the task is StoreType based
        /// or StoreInstance based and the appropriate binding is created.
        /// </summary>
        public ActionTaskDescriptorBinding(Type taskType, StoreEntity store)
        {
            descriptor = ActionTaskManager.GetDescriptor(taskType);
            if (descriptor == null)
            {
                throw new InvalidOperationException(string.Format("Type '{0}' does not have an associated descriptor.", taskType.FullName));
            }

            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                ActionTask instance = descriptor.CreateInstance(lifetimeScope);
                if (instance is StoreTypeTaskBase)
                {
                    storeTypeCode = (StoreTypeCode) store.TypeCode;
                }

                if (instance is StoreInstanceTaskBase)
                {
                    storeID = store.StoreID;
                }
            }
        }

        /// <summary>
        /// The StoreType of the binding
        /// </summary>
        public StoreTypeCode? StoreTypeCode
        {
            get { return storeTypeCode; }
        }

        /// <summary>
        /// The StoreID of the binding if there is one
        /// </summary>
        public long? StoreID
        {
            get { return storeID; }
        }

        /// <summary>
        /// The name of the task with the bound store specific information appended
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string FullName
        {
            get
            {
                string name = descriptor.BaseName;

                if (storeTypeCode != null)
                {
                    // See if this type code is the only type code we have like it in the system
                    bool onlyThisType = StoreManager.GetUniqueStoreTypes(true).Count == 1 && StoreManager.GetUniqueStoreTypes(true)[0].TypeCode == storeTypeCode.Value;

                    if (!onlyThisType)
                    {
                        name += string.Format(" ({0})", StoreTypeManager.GetType(storeTypeCode.Value).StoreTypeName);
                    }
                }

                if (storeID != null)
                {
                    bool onlyStore = StoreManager.GetEnabledStores().Count == 1 && StoreManager.GetEnabledStores()[0].StoreID == storeID.Value;

                    if (!onlyStore)
                    {
                        StoreEntity store = StoreManager.GetStore(storeID.Value);

                        name += string.Format(" ({0})", store != null ? store.StoreName : "Store Deleted");
                    }
                }

                return name;
            }
        }

        /// <summary>
        /// The base name of the task, without any store specific information appended.
        /// </summary>
        public string BaseName
        {
            get
            {
                return descriptor.BaseName;
            }
        }

        /// <summary>
        /// An identifier that uniquely identifies the action.  Once applied, this identifier cannot be changed,
        /// as it is used in a key in the serialization process.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string Identifier
        {
            get
            {
                return descriptor.Identifier;
            }
        }

        /// <summary>
        /// Get the task category - used for UI display purposes
        /// </summary>
        public ActionTaskCategory Category
        {
            get
            {
                return descriptor.Category;
            }
        }

        /// <summary>
        /// Create a new instance of the ActionTask for this type.
        /// </summary>
        public ActionTask CreateInstance(ILifetimeScope lifetimeScope)
        {
            // Create the blank task instance
            ActionTask task = descriptor.CreateInstance(lifetimeScope);

            // Create a new default task to be added
            ActionTaskEntity taskEntity = new ActionTaskEntity();
            taskEntity.ActionTaskID = -EntityUtility.GetEntitySeed(EntityType.ActionTaskEntity);
            taskEntity.TaskIdentifier = Identifier;
            taskEntity.TaskSettings = "<Settings />";
            taskEntity.StepIndex = -1;

            taskEntity.InputSource = (int) ActionTaskInputSource.TriggeringRecord;
            taskEntity.InputFilterNodeID = -1;

            taskEntity.FilterCondition = false;
            taskEntity.FilterConditionNodeID = -1;

            taskEntity.FlowSuccess = (int) ActionTaskFlowOption.NextStep;
            taskEntity.FlowSkipped = (int) ActionTaskFlowOption.NextStep;
            taskEntity.FlowError = (int) ActionTaskFlowOption.NextStep;

            // Initialize the task
            task.Initialize(taskEntity);

            // If its store-typed, set it
            if (storeTypeCode != null)
            {
                ((StoreTypeTaskBase) task).StoreTypeCode = storeTypeCode.Value;
            }

            // If its store-instanced, set it
            if (storeID != null)
            {
                ((StoreInstanceTaskBase) task).StoreID = storeID.Value;
            }

            return task;
        }

        /// <summary>
        /// Equals
        /// </summary>
        public override bool Equals(object obj)
        {
            ActionTaskDescriptorBinding other = obj as ActionTaskDescriptorBinding;
            if ((object) other == null)
            {
                return false;
            }

            return
                other.descriptor.Identifier == this.descriptor.Identifier &&
                other.storeID == this.storeID &&
                other.storeTypeCode == this.storeTypeCode;
        }

        /// <summary>
        /// Operator==
        /// </summary>
        public static bool operator ==(ActionTaskDescriptorBinding left, ActionTaskDescriptorBinding right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Operator!=
        /// </summary>
        public static bool operator !=(ActionTaskDescriptorBinding left, ActionTaskDescriptorBinding right)
        {
            return !(left.Equals(right));
        }

        /// <summary>
        /// Hash code
        /// </summary>
        public override int GetHashCode()
        {
            return descriptor.Identifier.GetHashCode() + storeTypeCode.GetHashCode() + storeID.GetHashCode();
        }
    }
}
