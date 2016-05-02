using Interapptive.Shared.Security;

namespace ShipWorks.Stores.Platforms.Sears
{
    public class SearsInitializationVector : IInitializationVector
    {
        public byte[] Value { get; } = {84, 104, 101, 68, 111, 111, 115, 107, 101, 114, 110, 111, 111, 100, 108, 101};
    }
}