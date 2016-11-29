using System.Collections.Generic;

namespace ShipWorks.Stores.Platforms.Magento.DTO.Interfaces
{
    public interface IPayment
    {
        string AccountStatus { get; set; }
        IList<string> AdditionalInformation { get; set; }
        double AmountOrdered { get; set; }
        double BaseAmountOrdered { get; set; }
        double BaseShippingAmount { get; set; }
        string CcExpYear { get; set; }
        string CcLast4 { get; set; }
        string CcSsStartMonth { get; set; }
        string CcSsStartYear { get; set; }
        int EntityId { get; set; }
        IExtensionAttributes ExtensionAttributes { get; set; }
        string Method { get; set; }
        int ParentId { get; set; }
        double ShippingAmount { get; set; }
    }
}