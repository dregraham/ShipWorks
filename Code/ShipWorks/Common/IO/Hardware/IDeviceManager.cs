using System.Collections.Generic;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Common.IO.Hardware
{
    /// <summary>
    /// Manage devices
    /// </summary>
    public interface IDeviceManager
    {
        /// <summary>
        /// All the devices
        /// </summary>
        IEnumerable<IDeviceEntity> DevicesReadOnly { get; }
    }
}