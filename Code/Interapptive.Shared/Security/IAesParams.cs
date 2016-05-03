namespace Interapptive.Shared.Security
{
    public interface IAesParams
    {
        byte[] InitializationVector { get; }

        byte[] Key { get; }

        string EmptyValue { get; }
    }
}
