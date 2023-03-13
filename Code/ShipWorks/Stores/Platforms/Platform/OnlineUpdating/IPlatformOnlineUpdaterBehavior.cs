﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipWorks.Stores.Platforms.Platform.OnlineUpdating
{
    public interface IPlatformOnlineUpdaterBehavior
    {
        bool UseSwatId { get; }

        bool IncludeSalesOrderItems { get; }
    }
}
