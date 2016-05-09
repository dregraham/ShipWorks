﻿using Autofac;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.UI.Controls;
using ShipWorks.UI.Services;

namespace ShipWorks.UI
{
    public class UIModule : Module
    {
        /// <summary>
        /// Loads the specified builder.
        /// </summary>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CustomerLicenseActivationControlHost>()
                .As<ICustomerLicenseActivation>();

            builder.RegisterType<CustomerLicenseActivationViewModel>()
                .AsImplementedInterfaces();

            builder.RegisterType<MessageHelperWrapper>()
                .AsImplementedInterfaces();
        }
    }
}