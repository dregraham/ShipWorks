using System.Collections.Generic;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Common.IO.Hardware
{
    public interface IDeviceManager
    {
        IEnumerable<IDeviceEntity> DevicesReadOnly { get; }
    }
}