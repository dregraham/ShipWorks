using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;
using ShipWorks.SqlServer.Data.Auditing;

public partial class Triggers
{
    [SqlTrigger(Target = "Customer", Event = "FOR INSERT, UPDATE, DELETE")]
    public static void CustomerAuditTrigger()
    {
        AuditService.AuditExecutingTrigger("Customer");
    }

    [SqlTrigger(Target = "Order", Event="FOR INSERT, UPDATE, DELETE")]
    public static void OrderAuditTrigger()
    {
        AuditService.AuditExecutingTrigger("Order");
    }

    [SqlTrigger(Target = "OrderItem", Event = "FOR INSERT, UPDATE, DELETE")]
    public static void OrderItemAuditTrigger()
    {
        AuditService.AuditExecutingTrigger("OrderItem");
    }

    [SqlTrigger(Target = "OrderItemAttribute", Event = "FOR INSERT, UPDATE, DELETE")]
    public static void OrderItemAttributeAuditTrigger()
    {
        AuditService.AuditExecutingTrigger("OrderItemAttribute");
    }

    [SqlTrigger(Target = "OrderCharge", Event = "FOR INSERT, UPDATE, DELETE")]
    public static void OrderChargeAuditTrigger()
    {
        AuditService.AuditExecutingTrigger("OrderCharge");
    }

    [SqlTrigger(Target = "Shipment", Event = "FOR INSERT, UPDATE, DELETE")]
    public static void ShipmentAuditTrigger()
    {
        AuditService.AuditExecutingTrigger("Shipment");
    }

    [SqlTrigger(Target = "Note", Event = "FOR INSERT, UPDATE, DELETE")]
    public static void NoteAuditTrigger()
    {
        AuditService.AuditExecutingTrigger("Note");
    }

    [SqlTrigger(Target = "OtherShipment", Event = "FOR INSERT, UPDATE, DELETE")]
    public static void OtherShipmentAuditTrigger()
    {
        AuditService.AuditExecutingTrigger("OtherShipment");
    }

    [SqlTrigger(Target = "PostalShipment", Event = "FOR INSERT, UPDATE, DELETE")]
    public static void PostalShipmentAuditTrigger()
    {
        AuditService.AuditExecutingTrigger("PostalShipment");
    }

    [SqlTrigger(Target = "StampsShipment", Event = "FOR INSERT, UPDATE, DELETE")]
    public static void StampsShipmentAuditTrigger()
    {
        AuditService.AuditExecutingTrigger("StampsShipment");
    }

    [SqlTrigger(Target = "EndiciaShipment", Event = "FOR INSERT, UPDATE, DELETE")]
    public static void EndiciaShipmentAuditTrigger()
    {
        AuditService.AuditExecutingTrigger("EndiciaShipment");
    }

    [SqlTrigger(Target = "FedExShipment", Event = "FOR INSERT, UPDATE, DELETE")]
    public static void FedExShipmentAuditTrigger()
    {
        AuditService.AuditExecutingTrigger("FedExShipment");
    }

    [SqlTrigger(Target = "UpsShipment", Event = "FOR INSERT, UPDATE, DELETE")]
    public static void UpsShipmentAuditTrigger()
    {
        AuditService.AuditExecutingTrigger("UpsShipment");
    }

    [SqlTrigger(Target = "iParcelShipment", Event = "FOR INSERT, UPDATE, DELETE")]
    public static void iParcelShipmentAuditTrigger()
    {
        AuditService.AuditExecutingTrigger("iParcelShipment");
    }

    [SqlTrigger(Target = "EbayOrder", Event = "FOR INSERT, UPDATE, DELETE")]
    public static void EbayOrderAuditTrigger()
    {
        AuditService.AuditExecutingTrigger("EbayOrder");
    }
}
