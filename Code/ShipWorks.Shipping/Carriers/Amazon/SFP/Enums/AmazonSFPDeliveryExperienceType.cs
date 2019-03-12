using System.ComponentModel;
using System.Reflection;
using Interapptive.Shared.Utility;
using ShipWorks.Users.Audit;

namespace ShipWorks.Shipping.Carriers.Amazon.SFP.Enums
{
    /// <summary>
    /// Valid Amazon delivery experience values
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    [AuditDisplayFormat(AuditDisplayFormat.Formats.AmazonDeliveryExperienceType)]
    public enum AmazonSFPDeliveryExperienceType
    {
        // There are currently no services that support NoTracking
        //[Description("No Tracking")]
        //[ApiValue("NoTracking")]
        //NoTracking = 0,

        [Description("Delivery Confirmation With Signature")]
        [ApiValue("DeliveryConfirmationWithSignature")]
        DeliveryConfirmationWithSignature = 1,

        [Description("Delivery Confirmation Without Signature")]
        [ApiValue("DeliveryConfirmationWithoutSignature")]
        DeliveryConfirmationWithoutSignature = 2,

        [Description("Delivery Confirmation With Adult Signature")]
        [ApiValue("DeliveryConfirmationWithAdultSignature")]
        DeliveryConfirmationWithAdultSignature = 3,
    }
}
