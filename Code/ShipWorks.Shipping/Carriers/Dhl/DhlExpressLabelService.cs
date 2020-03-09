using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.ShipEngine;
using ShipEngine.ApiClient.Model;
using ShipWorks.ApplicationCore.Logging;
using Autofac.Features.Indexed;
using Interapptive.Shared.ComponentRegistration;
using log4net;
using System.Threading.Tasks;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.Carriers.Dhl
{
    /// <summary>
    /// Dhl Express Implementation
    /// </summary>
    [KeyedComponent(typeof(ILabelService), ShipmentTypeCode.DhlExpress)]
    public class DhlExpressLabelService : ILabelService
    {
        private readonly IDhlExpressLabelClientFactory labelClientFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public DhlExpressLabelService(IDhlExpressLabelClientFactory labelClientFactory)
        {
            this.labelClientFactory = labelClientFactory;
        }

        /// <summary>
        /// Create a label
        /// </summary>
        public Task<TelemetricResult<IDownloadedLabelData>> Create(ShipmentEntity shipment) => 
            labelClientFactory.Create(shipment).CreateLabel(shipment);
        

        public void Void(ShipmentEntity shipment)
        {
            throw new NotImplementedException();
        }
    }
}
