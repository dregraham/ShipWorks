namespace ShipWorks.Escalator
{
    public interface IShipWorksCommunicationBridge
    {
        event ShipWorksCommunicationBridge.DelegateMessage OnMessage;

        void StartPipeServer();
    }
}