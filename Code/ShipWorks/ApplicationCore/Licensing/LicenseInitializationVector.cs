using Interapptive.Shared.Security;

namespace ShipWorks.ApplicationCore.Licensing
{
    public class LicenseInitializationVector : IInitializationVector
    {
        public byte[] Value { get; } = {125, 42, 69, 178, 253, 78, 1, 17, 77, 56, 129, 11, 25, 225, 201, 14};
    }
}