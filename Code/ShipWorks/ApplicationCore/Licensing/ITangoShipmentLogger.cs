namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// Log shipments to Tango
    /// </summary>
    public interface ITangoShipmentLogger
    {
        void LogProcessedShipments();
    }
}
