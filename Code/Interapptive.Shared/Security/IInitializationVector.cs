namespace Interapptive.Shared.Security
{
    public interface IInitializationVector
    {
        byte[] Value { get; }
    }
}
