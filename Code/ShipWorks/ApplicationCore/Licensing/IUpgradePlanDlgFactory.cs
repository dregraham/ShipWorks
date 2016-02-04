namespace ShipWorks.ApplicationCore.Licensing
{
    public interface IUpgradePlanDlgFactory
    {
        IDialog Create(string message);
    }
}