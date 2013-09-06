using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.Actions.Tasks;
using ShipWorks.Data.Model;
using ShipWorks.Actions.Tasks.Common.Editors;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Actions;
using ShipWorks.Shipping;
using log4net;

namespace ShipWorks.Stores.Platforms.Volusion.CoreExtensions.Actions
{
    /// <summary>
    /// Task for uploading shipment deails to Volusion
    /// </summary>
    [ActionTask("Upload shipment details", "VolusionShipmentUpload", ActionTaskCategory.UpdateOnline)]
    public class VolusionShipmentUploadTask : StoreTypeTaskBase
    {
        static readonly ILog log = LogManager.GetLogger(typeof(VolusionShipmentUploadTask));

        bool sendEmail = true;

        /// <summary>
        /// Option for allowing Volusion to send the shipment details email
        /// </summary>
        public bool SendEmail
        {
            get { return sendEmail; }
            set { sendEmail = value; }
        }

        /// <summary>
        /// Limit this task to just Volusion stores.
        /// </summary>
        public override bool SupportsType(StoreType storeType)
        {
            return storeType is VolusionStoreType;
        }

        /// <summary>
        /// Create the UI for configuring this task
        /// </summary>
        /// <returns></returns>
        public override ActionTaskEditor CreateEditor()
        {
            return new VolusionShipmentUploadTaskEditor(this);
        }

        /// <summary>
        /// Descriptive label which appears on the task editor
        /// </summary>
        public override string InputLabel
        {
            get
            {
                return "Upload tracking number of:";
            }
        }

        /// <summary>
        /// This task only operates on shipments
        /// </summary>
        public override EntityType? InputEntityType
        {
            get
            {
                return EntityType.ShipmentEntity;
            }
        }

        /// <summary>
        /// Execute the details upload
        /// </summary>
        public override void Run(List<long> inputKeys, ActionStepContext context)
        {
            foreach (long entityID in inputKeys)
            {
                List<long> storeKeys = DataProvider.GetRelatedKeys(entityID, EntityType.StoreEntity);
                if (storeKeys.Count == 0)
                {
                    // Store or shipment disapeared
                    continue;
                }

                VolusionStoreEntity storeEntity = StoreManager.GetStore(storeKeys[0]) as VolusionStoreEntity;
                if (storeEntity == null)
                {
                    // This isnt a generic store or the store went away
                    continue;
                }

                try
                {
                    ShipmentEntity shipment = ShippingManager.GetShipment(entityID);
                    if (shipment == null)
                    {
                        log.InfoFormat("Not uploading tracking number for shipment {0}, shipment was deleted.", entityID);
                        continue;
                    }

                    VolusionOnlineUpdater updater = new VolusionOnlineUpdater(storeEntity);
                    updater.UploadShipmentDetails(shipment, sendEmail, context.CommitWork);
                }
                catch (VolusionException ex)
                {
                    throw new ActionTaskRunException(ex.Message, ex);
                }
            }
        }
    }
}
