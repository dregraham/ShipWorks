namespace ShipWorks.Shipping.Carriers.UPS.WebServices.OpenAccount
{
    /// <summary>
    /// Common interface for Ups Address Types
    /// </summary>
    public interface IAddressType
    {
        string City { get; set; }
        string CompanyName { get; set; }
        string ContactName { get; set; }
        string CountryCode { get; set; }
        string EmailAddress { get; set; }
        PhoneType Phone { get; set; }
        string PostalCode { get; set; }
        string StateProvinceCode { get; set; }
        string StreetAddress { get; set; }
        string Suite { get; set; }
    }
}