using System;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using System.Xml;
using ShipWorks.Shipping.Carriers.UPS.ServiceManager;
using System.Linq;

namespace ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api.ElementWriters
{
    /// <summary>
    /// A class to write out the "Service" XML element of the package element of a UPS request for a SurePost
    /// request. This is primarily intended for the rate request as requests for SurePost rates require that
    /// the UPS service code value be provided.
    /// </summary>
    public class UpsSurePostRateServiceElementWriter : UpsRateServiceElementWriter
    {
        private readonly UpsServiceType serviceType;

        private readonly ShipmentEntity shipment;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpsSurePostRateServiceElementWriter" /> class.
        /// </summary>
        /// <param name="xmlWriter">The XML writer.</param>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="shipment">The shipment.</param>
        /// <exception cref="System.ArgumentException"></exception>
        public UpsSurePostRateServiceElementWriter(XmlWriter xmlWriter, UpsServiceType serviceType, ShipmentEntity shipment)
            : base(xmlWriter)
        {
            if (!UpsUtility.IsUpsSurePostService(serviceType))
            {
                throw new ArgumentException(string.Format("The {0} service type provided is not a SurePost service.", EnumHelper.GetDescription(serviceType)));
            }

            this.serviceType = serviceType;
            this.shipment = shipment;
        }

        /// <summary>
        /// Writes the service element to the XML writer.
        /// </summary>
        public override void WriteServiceElement()
        {
            UpsServiceManagerFactory serviceManagerFactory = new UpsServiceManagerFactory(shipment);
            IUpsServiceManager upsServiceManager = serviceManagerFactory.Create(shipment);
            UpsServiceMapping upsServiceMapping = upsServiceManager.GetServices(shipment).Single(x => x.UpsServiceType == serviceType);

            xmlWriter.WriteStartElement("Service");
            xmlWriter.WriteElementString("Code", upsServiceMapping.RateServiceCode);
            xmlWriter.WriteEndElement();
        }
    }
}
