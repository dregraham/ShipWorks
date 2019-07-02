namespace ShipWorks.ApplicationCore.Licensing.Warehouse
{
    /// <summary>
    /// Log shipments to the hub
    /// </summary>
    public interface IHubShipmentLogger
    {
        void LogProcessedShipments();

        void LogVoidedShipments();
    }
}
