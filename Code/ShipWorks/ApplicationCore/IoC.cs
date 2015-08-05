using Autofac;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.Amazon;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Shipping.Carriers.iParcel;
using ShipWorks.Shipping.Carriers.None;
using ShipWorks.Shipping.Carriers.OnTrac;
using ShipWorks.Shipping.Carriers.Other;
using ShipWorks.Shipping.Carriers.Postal.Endicia;
using ShipWorks.Shipping.Carriers.Postal.Endicia.Express1;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.Usps.Express1;
using ShipWorks.Shipping.Carriers.Postal.WebTools;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools;
using ShipWorks.Shipping.Carriers.UPS.WorldShip;

namespace ShipWorks.ApplicationCore
{
    /// <summary>
    /// Entry point for the Inversion of Control container
    /// </summary>
    public static class IoC
    {
        /// <summary>
        /// Static constructor
        /// </summary>
        static IoC()
        {
            Current = BuildContainer();
        }

        /// <summary>
        /// Get the current IoC container
        /// </summary>
        public static IContainer Current { get; private set; }

        /// <summary>
        /// Build the main IoC container
        /// </summary>
        /// <returns></returns>
        private static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();
            
            builder.RegisterType<UpsOltShipmentType>().Keyed<ShipmentType>(ShipmentTypeCode.UpsOnLineTools).ExternallyOwned();
            builder.RegisterType<WorldShipShipmentType>().Keyed<ShipmentType>(ShipmentTypeCode.UpsWorldShip).ExternallyOwned();
            builder.RegisterType<FedExShipmentType>().Keyed<ShipmentType>(ShipmentTypeCode.FedEx).ExternallyOwned();
            builder.RegisterType<EndiciaShipmentType>().Keyed<ShipmentType>(ShipmentTypeCode.Endicia).ExternallyOwned();
            builder.RegisterType<Express1EndiciaShipmentType>().Keyed<ShipmentType>(ShipmentTypeCode.Express1Endicia).ExternallyOwned();
            builder.RegisterType<Express1UspsShipmentType>().Keyed<ShipmentType>(ShipmentTypeCode.Express1Usps).ExternallyOwned();
            builder.RegisterType<PostalWebShipmentType>().Keyed<ShipmentType>(ShipmentTypeCode.PostalWebTools).ExternallyOwned();
            builder.RegisterType<OtherShipmentType>().Keyed<ShipmentType>(ShipmentTypeCode.Other).ExternallyOwned();
            builder.RegisterType<NoneShipmentType>().Keyed<ShipmentType>(ShipmentTypeCode.None).ExternallyOwned();
            builder.RegisterType<OnTracShipmentType>().Keyed<ShipmentType>(ShipmentTypeCode.OnTrac).ExternallyOwned();
            builder.RegisterType<iParcelShipmentType>().Keyed<ShipmentType>(ShipmentTypeCode.iParcel).ExternallyOwned();
            builder.RegisterType<BestRateShipmentType>().Keyed<ShipmentType>(ShipmentTypeCode.BestRate).ExternallyOwned();
            builder.RegisterType<UspsShipmentType>().Keyed<ShipmentType>(ShipmentTypeCode.Usps).ExternallyOwned();
            builder.RegisterType<AmazonShipmentType>().Keyed<ShipmentType>(ShipmentTypeCode.Amazon).ExternallyOwned();

            return builder.Build();
        }
    }
}
