using System.Reflection;
using Newtonsoft.Json;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.ShipEngine.DTOs.CarrierAccount
{
    /// <summary>
    /// Request DTO for registering new FedEx account
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public class FedExRegistrationRequest
    {
        /// <summary>
        /// Empty constructor for serialization
        /// </summary>
        public FedExRegistrationRequest()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public FedExRegistrationRequest(FedExAccountEntity account)
        {
            Nickname = account.Description;
            AccountNumber = account.AccountNumber;
            Company = account.Company;
            FirstName = account.FirstName;
            LastName = account.LastName;
            Phone = account.Phone;
            Address1 = account.Street1;
            Address2 = account.Street2;
            City = account.City;
            State = account.StateProvCode;
            PostalCode = account.PostalCode;
            CountryCode = account.CountryCode;
            Email = account.Email;
            AgreeToEula = true;
            IsSandbox = true;
        }

        [JsonProperty("nickname")]
        public string Nickname { get; set; }

        [JsonProperty("account_number")]
        public string AccountNumber { get; set; }

        [JsonProperty("company")]
        public string Company { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("address1")]
        public string Address1 { get; set; }

        [JsonProperty("address2")]
        public string Address2 { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("postal_code")]
        public string PostalCode { get; set; }

        [JsonProperty("country_code")]
        public string CountryCode { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("agree_to_eula")]
        public bool AgreeToEula { get; set; }

        [JsonProperty("is_sandbox")]
        public bool IsSandbox { get; set; }

    }
}
