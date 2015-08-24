using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Shipping.Editing;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.ShipSense.Packaging;
using ShipWorks.Templates.Processing.TemplateXml;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Templates.Processing;
using ShipWorks.Shipping.Carriers.BestRate;
using Autofac;

namespace ShipWorks.Shipping.Carriers.None
{
    /// <summary>
    /// "None" shipment type implementation
    /// </summary>
    class NoneShipmentType : ShipmentType
    {
        /// <summary>
        /// The ShipmentType code of this type
        /// </summary>
        public override ShipmentTypeCode ShipmentTypeCode
        {
            get { return ShipmentTypeCode.None; }
        }

        /// <summary>
        /// The UserControl for editing the settings of this type
        /// </summary>
        /// <param name="rateControl">A handle to the rate control so the selected rate can be updated when
        /// a change to the shipment, such as changing the service type, matches a rate in the control</param>
        protected override ServiceControlBase InternalCreateServiceControl(RateControl rateControl)
        {
            return new NoneServiceControl(rateControl);
        }

        /// <summary>
        /// Gets the package adapter for the shipment.
        /// </summary>
        public override IEnumerable<IPackageAdapter> GetPackageAdapters(ShipmentEntity shipment)
        {
            return new List<IPackageAdapter>()
            {
                new NullPackageAdapter()
            };
        }

        /// <summary>
        /// Ensures that the carrier specific data for the shipment
        /// </summary>
        public override void LoadShipmentData(ShipmentEntity shipment, bool refreshIfPresent)
        {

        }

        /// <summary>
        /// Get the carrier specific description of the shipping service used
        /// </summary>
        public override string GetServiceDescription(ShipmentEntity shipment)
        {
            return "";
        }

        /// <summary>
        /// No parcels for 'None' shipments
        /// </summary>
        public override int GetParcelCount(ShipmentEntity shipment)
        {
            return 0;
        }

        /// <summary>
        /// Get the parcel data for the shipment
        /// </summary>
        public override ShipmentParcel GetParcelDetail(ShipmentEntity shipment, int parcelIndex)
        {
            throw new NotSupportedException("GetParcelDetail not supported for none.");
        }

        /// <summary>
        /// Gets the processing synchronizer to be used during the PreProcessing of a shipment.
        /// </summary>
        protected override IShipmentProcessingSynchronizer GetProcessingSynchronizer()
        {
            // PreProcess is overridden to do nothing, so there is nothing to synchronize
            return null;
        }

        /// <summary>
        /// There's nothing to do for this shipment type, so it just returns a single 
        /// item list of the shipment provided.
        /// </summary> 
        public override List<ShipmentEntity> PreProcess(ShipmentEntity shipment, Func<CounterRatesProcessingArgs, System.Windows.Forms.DialogResult> counterRatesProcessing, RateResult selectedRate, ILifetimeScope lifetimeScope)
        {
            return new List<ShipmentEntity>() { shipment };
        }

        /// <summary>
        /// Process the shipment
        /// </summary>
        public override void ProcessShipment(ShipmentEntity shipment)
        {
            throw new ShippingException("No carrier is selected for the shipment.");
        }

        /// <summary>
        /// Gets an instance to the best rate shipping broker for the None shipment type based on the shipment configuration.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>An instance of a NullShippingBroker.</returns>
        public override IBestRateShippingBroker GetShippingBroker(ShipmentEntity shipment)
        {
            return new NullShippingBroker();
        }
    }
}
