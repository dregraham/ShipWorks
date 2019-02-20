using System;
using System.Threading.Tasks;

namespace ShipWorks.Escalator
{
    public interface IShipWorksUpgrade
    {
        Task Upgrade(string tangoCustomerId);
        Task Upgrade(Version version);
    }
}