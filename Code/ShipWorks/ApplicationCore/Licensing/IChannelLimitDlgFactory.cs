namespace ShipWorks.ApplicationCore.Licensing
{
    public interface IChannelLimitDlgFactory
    {
        IChannelLimitDlg GetChannelLimitDlg(ICustomerLicense customerLicense);
    }
}