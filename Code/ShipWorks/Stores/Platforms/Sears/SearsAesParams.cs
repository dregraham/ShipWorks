using Interapptive.Shared.Security;

namespace ShipWorks.Stores.Platforms.Sears
{
    public class SearsAesParams : IAesParams
    {
        public byte[] InitializationVector { get; } = {84, 104, 101, 68, 111, 111, 115, 107, 101, 114, 110, 111, 111, 100, 108, 101};

        public byte[] Key { get; }

        public string EmptyValue { get; }
    }
}