using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Data.Administration.UpdateFrom2x.Database.Tasks
{
    /// <summary>
    /// Types of record IDs in V2 Archiving and V3 Migration
    /// </summary>
    public enum MigrationRowKeyType
    {
        // v2 archiving
        Order = 0,
        Shipment = 1,
        Customer = 2,
        Email = 3,
        OrderItem = 4,
        UpsShipment = 5,
        FedexShipment = 6,
        Client = 7,

        // v3 upgrade
        Store = 8,
        OrderItemAttribute = 9,
        OrderCharge = 10,
        OrderPaymentDetail = 11,
        EndiciaAccount = 12,
        UpsAccount = 13,
        UpsPackage = 14,
        FedexPackage = 15,
        EmailAccount = 16,
        User = 17,
        FedexAccount = 18
    }
}
