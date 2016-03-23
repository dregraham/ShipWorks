using Autofac;
using ShipWorks.Stores.Platforms.SparkPay;


namespace ShipWorks.Stores.UI.Platforms.SparkPay
{
    /// <summary>
    /// Make necessary registrations for SparkPay
    /// </summary>
    public class SparkPayModule : Module
    {
        /// <summary>
        /// Loads the specified builder.
        /// </summary>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SparkPayStoreType>()
                .Keyed<StoreType>(StoreTypeCode.SparkPay)
                .ExternallyOwned();

        }
    }
}