using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.Business;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.ShipEngine.DTOs.Registration
{
    /// <summary>
    /// Factory for creating a UpsRegistrationRequest
    /// </summary>
    public class UpsRegistrationRequestFactory : IUpsRegistrationRequestFactory
    {
        private readonly INetworkUtility networkUtility;
        private readonly IUpsCredentials upsCredentials;

        /// <summary>
        /// Constructor
        /// </summary>
        public UpsRegistrationRequestFactory(INetworkUtility networkUtility, IUpsCredentials upsCredentials)
        {
            this.networkUtility = networkUtility;
            this.upsCredentials = upsCredentials;
        }

        /// <summary>
        /// Create a UpsRegistrationRequest object
        /// </summary>
        public async Task<GenericResult<string>> Create(PersonAdapter person)
        {
            var registration = new UpsRegistrationRequest()
            {
                Nickname = person.Email,
                UpsId = upsCredentials.UserId,
                Password = upsCredentials.Password,
                AccessLicense = upsCredentials.AccessKey,
                DeveloperKey = upsCredentials.DeveloperKey,
                Email = person.Email,
                Address = new DTOs.Registration.Address
                {
                    Name = person.UnparsedName,
                    Phone = person.Phone,
                    CompanyName = person.Company,
                    AddressLine1 = person.Street1,
                    AddressLine2 = person.Street2,
                    AddressLine3 = person.Street3,
                    CityLocality = person.City,
                    StateProvince = person.StateProvCode,
                    PostalCode = person.PostalCode5,
                    CountryCode = person.CountryCode
                },
                WeightUnits = "pound",
                EndUserIpAddress = networkUtility.GetIPAddress(),
                SoftwareProvider = "ShipWorks",
                SoftwareProductName = "ShipWorks"
            };

            return registration;
        }
    }
}
