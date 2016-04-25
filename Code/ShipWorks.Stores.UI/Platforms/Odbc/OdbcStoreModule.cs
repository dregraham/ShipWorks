﻿using Autofac;
using ShipWorks.Stores.Platforms.Odbc;

namespace ShipWorks.Stores.UI.Odbc
{
    public class OdbcStoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<OdbcStoreType>()
                .Keyed<StoreType>(StoreTypeCode.Odbc)
                .ExternallyOwned();
        }
    }
}
