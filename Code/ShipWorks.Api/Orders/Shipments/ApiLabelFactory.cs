﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data;

namespace ShipWorks.Api.Orders.Shipments
{
    /// <summary>
    /// Generates a label 
    /// </summary>
    [Component]
    public class ApiLabelFactory : IApiLabelFactory
    {
        private readonly IDataResourceManager dataResourceManager;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dataResourceManager"></param>
        public ApiLabelFactory(IDataResourceManager dataResourceManager)
        {
            this.dataResourceManager = dataResourceManager;
        }

        /// <summary>
        /// Gets labels for ID
        /// </summary>
        /// <param name="consumerID">Either a shipment or package ID</param>
        public IEnumerable<LabelData> GetLabels(long consumerID)
        {
            return dataResourceManager.GetConsumerResourceReferences(consumerID)
                .Select(r => CreateLabel(r));
        }

        private static LabelData CreateLabel(DataResourceReference r)
        {
            return new LabelData(r.Label, Convert.ToBase64String(r.ReadAllBytes()));
        }
    }
}
