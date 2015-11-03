using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;
using System.Xml;
using System.Xml.XPath;
using Interapptive.Shared.Utility;
using ShipWorks.Users.Audit;
using log4net;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.HelperClasses;
using System.IO;
using Interapptive.Shared;

namespace ShipWorks.Stores
{
    /// <summary>
    /// Basic status code caching/reading functionality for online stores
    /// </summary>
    public abstract class OnlineStatusCodeProvider<T> : StatusCodeProvider<T>
    {
        // Logger 
        static readonly ILog log = LogManager.GetLogger(typeof(OnlineStatusCodeProvider<T>));

        // Store entity using status codes
        StoreEntity store;

        // Field in the StoreEntity which stores the status codes blob
        IEntityField2 storeStatusCodesField;

        /// <summary>
        /// Constructor
        /// </summary>
        protected OnlineStatusCodeProvider(StoreEntity store, IEntityField2 storeStatusCodesField)
        {
            this.store = store;
            this.storeStatusCodesField = storeStatusCodesField;
        }

        /// <summary>
        /// Expose the store entity to derived classes
        /// </summary>
        protected StoreEntity Store
        {
            get { return store; }
        }

        /// <summary>
        /// Derived classes override to pull all the available status codes from their online store
        /// </summary>
        protected abstract Dictionary<T, string> GetCodeMapFromOnline();

        /// <summary>
        /// Gets the value of the store's StatusCodes field
        /// </summary>
        protected override string GetLocalStatusCodesXml()
        {
            return (string) store.Fields[storeStatusCodesField.FieldIndex].CurrentValue;
        }

        /// <summary>
        /// Update status codes from the online store
        /// </summary>
        [NDependIgnoreLongMethod]
        public void UpdateFromOnlineStore()
        {
            Dictionary<string, string> updates = new Dictionary<string, string>();
            bool changes = false;

            // Copy the old map and load the new one
            Dictionary<T, string> oldMap = CodeMap;

            // get the new/updated status code map
            Dictionary<T, string> newMap = GetCodeMapFromOnline();

            // null forces us to stop
            if (newMap == null)
            {
                return;
            }

            // Update to use this map now
            CodeMap = newMap;

            // Go throug all pairs in the new map
            foreach (KeyValuePair<T, string> pair in newMap)
            {
                T code = pair.Key;
                string name = pair.Value;

                string oldName;
                if (oldMap.TryGetValue(code, out oldName))
                {
                    // Description changed
                    if (name != oldName)
                    {
                        changes = true;
                        updates[oldName] = name;
                        log.InfoFormat("Status code change: [{0}] => [{1}]", oldName, name);
                    }
                }
                else
                {
                    // This is a new value
                    changes = true;
                    log.InfoFormat("Status code added: [{0}]", name);
                }
            }

            // Go through all pairs in the old to see if any are missing
            foreach (KeyValuePair<T, string> pair in oldMap)
            {
                if (!newMap.ContainsKey(pair.Key))
                {
                    changes = true;
                    log.InfoFormat("Status code deleted: [{0}]", pair.Value);
                }
            }

            // If there are changes, we need to do some updates
            if (changes)
            {
                // Update the StatusCodes on the store
                SetLocalStatusCodesXml(SerializeCodeMapToXml(newMap));

                // We may be doing this in the wizard, in which case the store will be new - and there is no DB work to do
                if (!store.IsNew)
                {

                    using (AuditBehaviorScope auditScope = new AuditBehaviorScope(
                        AuditBehaviorUser.SuperUser,
                        new AuditReason(AuditReasonType.Default),
                        AuditState.Disabled))
                    {
                        using (SqlAdapter adapter = new SqlAdapter(true))
                        {
                            StoreManager.SaveStore(store, adapter);

                            // Update all the OnlineStatusCode text that changed
                            foreach (KeyValuePair<string, string> pair in updates)
                            {
                                // Set it to the new value
                                OrderEntity prototype = new OrderEntity();
                                prototype.OnlineStatus = pair.Value;

                                // Where it was the old value
                                RelationPredicateBucket bucket =
                                    new RelationPredicateBucket(OrderFields.OnlineStatus == pair.Key &
                                                                OrderFields.StoreID == store.StoreID);

                                // GO
                                adapter.UpdateEntitiesDirectly(prototype, bucket);
                            }

                            adapter.Commit();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Sets the value of the store's StatusCodes field
        /// </summary>
        private void SetLocalStatusCodesXml(string codesXml)
        {
            store.SetNewFieldValue(storeStatusCodesField.FieldIndex, codesXml);
        }
    }
}
