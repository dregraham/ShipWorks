using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace ShipWorks.Data.Grid.Columns
{
    /// <summary>
    /// Enumerates the available grid column definition sets.
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum GridColumnDefinitionSet
    {
        [Description("Orders")]
        Orders,

        [Description("Customers")]
        Customers,

        [Description("Shipments")]
        ShipmentStandard,

        [Description("Download History")]
        DownloadLog,

        [Description("Outgoing Email")]
        EmailOutbound,

        [Description("Action Errors")]
        ActionErrors,

        [Description("Notes")]
        Notes,

        [Description("Charges")]
        Charges,

        [Description("Payment Details")]
        PaymentDetails,

        [Description("Order Items")]
        OrderItems,

        [Description("Shipment Panel")]
        ShipmentPanel,

        [Description("Outgoing Email Panel")]
        EmailOutboundPanel,

        [Description("Orders Panel")]
        OrderPanel,

        [Description("Printed")]
        PrintResult,

        [Description("Email Outbound Relation")]
        EmailOutboundRelation,

        [Description("Audit")]
        Audit,

        [Description("Audit Changes")]
        AuditChanges,

        [Description("Service Status")]
        ServiceStatus
    }
}
