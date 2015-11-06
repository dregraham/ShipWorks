using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using ShipWorks.Shipping.Carriers.UPS;

namespace ShipWorks.Shipping.UI.Carriers.Ups
{
    public class UpsShippingModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<UpsAccountRepository>()
                .AsImplementedInterfaces();
        }
    }
}
