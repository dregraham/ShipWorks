using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.ScanForms
{
    public interface IScanFormGateway
    {
        /// <summary>
        /// Gets the scan form from the shipping carrier and populates the properties of the given scan form.
        /// </summary>
        /// <param name="scanForm">The scan form being populated.</param>
        /// <param name="shipments">The shipments the scan form is being generated for.</param>
        /// <returns>A carrier-specific scan form entity object.</returns>
        IEntity2 FetchScanForm(ScanForm scanForm, IEnumerable<ShipmentEntity> shipments);
    }
}
