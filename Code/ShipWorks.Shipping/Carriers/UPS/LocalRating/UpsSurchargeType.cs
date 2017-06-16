using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Shipping.Carriers.UPS.LocalRating
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum UpsSurchargeType
    {
        [Description("No Signature")]
        NoSignature = 1,

        [Description("Signature Required")]
        SignatureRequired = 2,

        [Description("Adult Signature Required")]
        AdultSignatureRequired = 3,

        [Description("Shipper Release")]
        ShipperRelease = 4,

        [Description("Carbon Neutral - Air")]
        CarbonNeutralAir = 5,

        [Description("Carbon Neutral - Ground")]
        CarbonNeutralGround = 6,

        [Description("Additional Handling")]
        AdditionalHandling = 7,

        [Description("Dry Ice")]
        DryIce = 8,

        [Description("Verbal Confirmation")]
        VerbalConfirmation = 9,

        [Description("Collect on Delivery")]
        CollectonDelivery = 10,

        [Description("Saturday Delivery")]
        SaturdayDelivery = 11,

        [Description("Third-party Billing")]
        ThirdpartyBilling = 12,

        [Description("UPS Returns: Electronic Return Label")]
        UpsReturnsElectronicReturnLabel = 13,

        [Description("UPS Returns: Print Return Label")]
        UpsReturnsPrintReturnLabel = 14,

        [Description("UPS Returns: Print and Mail")]
        UpsReturnsPrintandMail = 15,

        [Description("UPS Returns: Returns Plus, 1 Attempt")]
        UpsReturnsReturnsPlusOneAttempt = 16,

        [Description("UPS Returns: Returns Plus, 3 Attempts")]
        UpsReturnsReturnsPlusThreeAttempts = 17,

        [Description("Large Package")]
        LargePackage = 18,

        [Description("Delivery Area Commercial - Air")]
        DeliveryAreaCommercialAir = 19,

        [Description("Delivery Area Residential - Air")]
        DeliveryAreaResidentialAir = 20,

        [Description("Delivery Area Commercial Extended - Air")]
        DeliveryAreaCommercialExtendedAir = 21,

        [Description("Delivery Area Residential Extended - Air")]
        DeliveryAreaResidentialExtendedAir = 22,

        [Description("Delivery Area Commercial - Ground")]
        DeliveryAreaCommercialGround = 23,

        [Description("Delivery Area Residential - Ground")]
        DeliveryAreaResidentialGround = 24,

        [Description("Delivery Area Commercial Extended - Ground")]
        DeliveryAreaCommercialExtendedGround = 25,

        [Description("Delivery Area Residential Extended - Ground")]
        DeliveryAreaResidentialExtendedGround = 26,

        [Description("Remote Area - Alaska")]
        RemoteAreaAlaska = 27,

        [Description("Remote Area - Hawaii")]
        RemoteAreaHawaii = 28,

        [Description("Residential - Air")]
        ResidentialAir = 29,

        [Description("Residential - Ground")]
        ResidentialGround = 30,

        [Description("Fuel - Air")]
        FuelAir = 31,

        [Description("Fuel - Ground")]
        FuelGround = 32,

        [Description("Saturday Pickup")]
        SaturdayPickup = 33,

        [Description("NDA Early Over 150 LBS")]
        NdaEarlyOver150Lbs = 34,

        [Description("Declared Value - Minimum Charge")]
        DeclaredValueMinimumCharge = 35,

        [Description("Declared Value - Price Per Hundred")]
        DeclaredValuePricePerHundred = 36
    }
}