using System.Reflection;
using Interapptive.Shared.Business;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.ShipEngine.DTOs.Registration
{
    /// <summary>
    /// Factory for creating a UpsRegistrationRequest
    /// </summary>
    [Component]
    public class UpsRegistrationRequestFactory : IUpsRegistrationRequestFactory
    {
        private readonly INetworkUtility networkUtility;
        private readonly IUpsCredentials upsCredentials;
        private readonly IAssembly assembly;

        /// <summary>
        /// Constructor
        /// </summary>
        public UpsRegistrationRequestFactory(INetworkUtility networkUtility, IUpsCredentials upsCredentials, IAssembly assembly)
        {
            this.networkUtility = networkUtility;
            this.upsCredentials = upsCredentials;
            this.assembly = assembly;
        }

        /// <summary>
        /// Create a UpsRegistrationRequest object
        /// </summary>
        public UpsRegistrationRequest Create(PersonAdapter person, string deviceIdentity)
        {
            UpsRegistrationRequest registration = new UpsRegistrationRequest()
            {
                Nickname = person.Email,
                UpsId = upsCredentials.UserId,
                Password = upsCredentials.Password,
                AccessLicense = upsCredentials.AccessKey,
                DeveloperKey = upsCredentials.DeveloperKey,
                Email = person.Email,
                Address = new Address
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
                EndUserIpAddress = GetIPAddress(),
                DeviceIdentity = deviceIdentity,
                SoftwareProvider = "ShipWorks",
                SoftwareProductName = "ShipWorks"
            };

            return registration;
        }

        /// <summary>
        /// Get IP Address
        /// </summary>
        private string GetIPAddress()
        {
            if (assembly.GetExecutingAssemblyName().Version.Major > 1)
            {
                return networkUtility.GetPublicIPAddress();
            }
            else
            {
                return networkUtility.GetIPAddress();
            }
        }
    }
}
