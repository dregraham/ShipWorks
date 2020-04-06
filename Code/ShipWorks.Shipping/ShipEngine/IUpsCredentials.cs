namespace ShipWorks.Shipping.ShipEngine
{
    public interface IUpsCredentials
    {
        string AccessKey { get; }
        string DeveloperKey { get; }
        string Password { get; }
        string UserId { get; }
    }
}