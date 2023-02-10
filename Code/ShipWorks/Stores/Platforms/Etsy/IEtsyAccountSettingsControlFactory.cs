using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Management;

namespace ShipWorks.Stores.Platforms.Etsy
{
    public interface IEtsyAccountSettingsControlFactory
    {
        AccountSettingsControlBase Create(IStoreEntity store);
    }
}
