using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using Interapptive.Shared.Messaging;
using ShipWorks.ApplicationCore;
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
        private readonly IObservable<IShipWorksMessage> messages;
        private readonly Func<IInsuranceBehaviorChangeViewModel> createInsuranceBehaviorChange;
        private readonly IShipmentTypeManager shipmentTypeManager;

        IDisposable subscription;

        /// <summary>
        /// Constructor
        /// </summary>
        public ProfileAppliedPipeline(IObservable<IShipWorksMessage> messages,
            Func<IInsuranceBehaviorChangeViewModel> createInsuranceBehaviorChange,
            IShipmentTypeManager shipmentTypeManager)
        {
            this.messages = messages;
            this.createInsuranceBehaviorChange = createInsuranceBehaviorChange;
            this.shipmentTypeManager = shipmentTypeManager;
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

            subscription = messages.OfType<ProfileAppliedMessage>()
                .Subscribe(ProcessMessage);
        }

        /// <summary>
        /// Compiles original and new insurance selections and uses it to notify a InsuranceBehaviorChange
        /// </summary>
        public void ProcessMessage(ProfileAppliedMessage message)
        {
            IDictionary<long, bool> originalInsuranceSelections = new Dictionary<long, bool>();
            IDictionary<long, bool> updatedInsuranceSelections = new Dictionary<long, bool>();

            foreach (ShipmentEntity originalShipment in message.OriginalShipments)
            {
                long shipmentID = originalShipment.ShipmentID;
                ShipmentEntity updatedShipment = message.UpdatedShipments.Single(s => s.ShipmentID == shipmentID);
                if (updatedShipment.ShipmentTypeCode != originalShipment.ShipmentTypeCode)
                {
                    originalInsuranceSelections.Add(shipmentID, shipmentTypeManager.Get(originalShipment).GetPackageAdapters(originalShipment).Any(p => p.InsuranceChoice?.Insured ?? true));
                    updatedInsuranceSelections.Add(shipmentID, shipmentTypeManager.Get(updatedShipment).GetPackageAdapters(updatedShipment).Any(p => p.InsuranceChoice?.Insured ?? true));
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
