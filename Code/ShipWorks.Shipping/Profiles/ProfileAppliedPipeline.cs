using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using ShipWorks.ApplicationCore;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Insurance;

namespace ShipWorks.Shipping.Profiles
{
    /// <summary>
    /// Notifies the insurance notification dialog when a profile is applied
    /// and the shipment type is changed.
    /// </summary>
    public class ProfileAppliedPipeline : IInitializeForCurrentUISession
    {
        private readonly IMessenger messenger;
        private readonly Func<IInsuranceBehaviorChangeViewModel> createInsuranceBehaviorChange;

        IDisposable subscription;

        /// <summary>
        /// Constructor
        /// </summary>
        public ProfileAppliedPipeline(IMessenger messenger,
            Func<IInsuranceBehaviorChangeViewModel> createInsuranceBehaviorChange)
        {
            this.messenger = messenger;
            this.createInsuranceBehaviorChange = createInsuranceBehaviorChange;
        }

        /// <summary>
        /// Subscribes to the ProfileAppliedMessage
        /// </summary>
        public void InitializeForCurrentSession()
        {
            // We should never initialize an already initialized session. We'll re-subscribe in release but when
            // debugging, we should get alerted that this is happening
            Debug.Assert(subscription == null, "Subscription is already initialized");
            EndSession();

            subscription = messenger.OfType<ProfileAppliedMessage>()
                .Subscribe(ProcessMessage);
        }

        /// <summary>
        /// Compiles original and new insurance selections and uses it to notify a InsuranceBehaviorChange
        /// </summary>
        private void ProcessMessage(ProfileAppliedMessage message)
        {
            IDictionary<long, bool> originalInsuranceSelections = new Dictionary<long, bool>();
            IDictionary<long, bool> updatedInsuranceSelections = new Dictionary<long, bool>();

            foreach (ShipmentEntity originalShipment in message.OriginalShipments.Where(s => !s.IsNew))
            {
                long shipmentID = originalShipment.ShipmentID;
                ShipmentEntity updatedShipment = message.UpdatedShipments.Single(s => s.ShipmentID == shipmentID);
                if (updatedShipment.ShipmentTypeCode != originalShipment.ShipmentTypeCode)
                {
                    originalInsuranceSelections.Add(shipmentID, originalShipment.Insurance);
                    updatedInsuranceSelections.Add(shipmentID, updatedShipment.Insurance);
                }
            }

            if (originalInsuranceSelections.Any())
            {
                createInsuranceBehaviorChange().Notify(originalInsuranceSelections, updatedInsuranceSelections);
            }
        }
        
        /// <summary>
        /// Unsubscribe
        /// </summary>
        public void EndSession()
        {
            subscription?.Dispose();
        }

        /// <summary>
        /// Disposes
        /// </summary>
        public void Dispose()
        {
            EndSession();
        }
    }
}
