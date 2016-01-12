using System.Collections.Generic;

namespace ShipWorks.ApplicationCore.Licensing
{
    public interface ILicenseFactory
    {
        IEnumerable<ILicense> GetLicenses();
    }
}