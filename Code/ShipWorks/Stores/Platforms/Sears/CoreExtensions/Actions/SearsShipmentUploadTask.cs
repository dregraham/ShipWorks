﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.Stores;
using ShipWorks.Stores.Platforms.BuyDotCom;
using log4net;
using ShipWorks.Actions.Tasks;
using ShipWorks.Data.Model;
using ShipWorks.Actions.Tasks.Common.Editors;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Actions;

namespace ShipWorks.Stores.Platforms.Sears.CoreExtensions.Actions
{
    /// <summary>
    /// Task for uploading shipment details to Sears
    /// </summary>
    [ActionTask("Upload shipment details", "SearsShipmentUploadTask")]
    public class SearsShipmentUploadTask : StoreTypeTaskBase
    {
        /// <summary>
        /// This task is for shipments
        /// </summary>
        public override EntityType? InputEntityType
        {
            get
            {
                return EntityType.ShipmentEntity;
            }
        }

        /// <summary>
        /// Descriptive label which appears on the task editor
        /// </summary>
        public override string InputLabel
        {
            get
            {
                return "Upload the shipment details for:";
            }
        }

        /// <summary>
        /// Instantiates the editor for the action
        /// </summary>
        public override ActionTaskEditor CreateEditor()
        {
            return new BasicShipmentUploadTaskEditor();
        }

        /// <summary>
        /// Indicates if the task is supported for the specified store
        /// </summary>
        public override bool SupportsType(StoreType storeType)
        {
            return storeType.TypeCode == StoreTypeCode.Sears;
        }

        /// <summary>
        /// Run the task
        /// </summary>
        protected override void Run(List<long> inputKeys)
        {
            foreach (long shipmentID in inputKeys)
            {
                try
                {
                    SearsOnlineUpdater updater = new SearsOnlineUpdater();
                    updater.UploadShipmentDetails(shipmentID);
                }
                catch (SearsException ex)
                {
                    throw new ActionTaskRunException(ex.Message, ex);
                }
            }
        }
    }
}
