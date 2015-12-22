using Autofac;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.Amazon;
using ShipWorks.Shipping.Carriers.Amazon.Api;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.UI.Carriers.Amazon;

namespace ShipWorks.Shipping.UI
{
    /// <summary>
    /// IoC registration module for this assembly
    /// </summary>
    public class ShippingModule : Module
    {
        /// <summary>
        /// Load the module configuration
        /// </summary>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ShippingProfileEditorDlg>();

            builder.RegisterType<ShippingManagerWrapper>()
                .AsImplementedInterfaces();
        }
    }
}
