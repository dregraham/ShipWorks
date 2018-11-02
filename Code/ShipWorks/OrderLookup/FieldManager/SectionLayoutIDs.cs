using System.Reflection;

namespace ShipWorks.OrderLookup.FieldManager
{
    /// <summary>
    /// Field IDs used in section layouts (panels)
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public enum SectionLayoutIDs
    {
        Undefined,
        From,
        To,
        Items,
        LabelOptions,
        ShipmentDetails,
        Rates,
        Customs,
        USPSReference,
        FedExSignatureAndReference,
        FedExEmailNotifications,
        UPSReference,
        UPSQuantumViewNotify,
    }
}
