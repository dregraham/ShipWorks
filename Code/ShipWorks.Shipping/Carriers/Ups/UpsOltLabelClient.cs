using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Ups
{
    public class UpsOltLabelClient : IUpsLabelClient
    {
        public Task<TelemetricResult<IDownloadedLabelData>> GetLabel(ShipmentEntity shipment)
        {
            throw new NotImplementedException();
        }
    }
}
