//using System.Collections.Generic;
//using System.Reflection;
//using Interapptive.Shared.Utility;
//using Newtonsoft.Json;

//namespace ShipWorks.Shipping.ShipEngine.DTOs
//{
//    public class ListCarriersResponse : BaseShipEngineResponse
//    {
//        public List<Carrier> Carriers { get; set; }
//    }

//    public class Carrier
//    {
//        public string CarrierId { get; set; }

//        public string CarrierCode { get; set; }

//        public string AccountNumber { get; set; }

//        public bool? RequiresFundedAmount { get; set; }

//        public double? Balance { get; set; }

//        public string Nickname { get; set; }

//        public string FriendlyName { get; set; }

//        public bool? Primary { get; set; }

//        public bool? HasMultiPackageSupportingServices { get; set; }

//        public bool? SupportsLabelMessages { get; set; }

//        public List<Service> Services { get; set; }

//        public List<Package> Packages { get; set; }

//        public List<CarrierAdvancedOption> Options { get; set; }
//    }

//    public class Service
//    {
//        public string CarrierId { get; set; }

//        public string CarrierCode { get; set; }

//        public string ServiceCode { get; set; }

//        public string Name { get; set; }

//        public bool? Domestic { get; set; }

//        public bool? International { get; set; }

//        public bool? IsMultiPackageSupported { get; set; }
//    }

//    public class Package
//    {
//        public string PackageId { get; set; }

//        public string PackageCode { get; set; }

//        public string Name { get; set; }

//        public Dimensions Dimensions { get; set; }

//        public string Description { get; set; }
//    }

//    public class CarrierAdvancedOption
//    {
//        public string Name { get; set; }

//        public string DefaultValue { get; set; }

//        public string Description { get; set; }
//    }
//}
