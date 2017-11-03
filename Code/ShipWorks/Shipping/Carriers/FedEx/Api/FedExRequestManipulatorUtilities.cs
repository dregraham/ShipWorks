using System;
using System.Collections.Generic;
using Interapptive.Shared;
using Interapptive.Shared.Business;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Enums;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Shipping.Carriers.FedEx.Api
{
    /// <summary>
    /// Utility class for common functionality used by request manipulators
    /// </summary>
    public static class FedExRequestManipulatorUtilities
    {
        /// <summary>
        /// Gets the Ship Service WSDL RequestedShipment for the request
        /// </summary>
        /// <param name="request">The FedEx carrier request</param>
        /// <returns>RequestedShipment from the Ship Service WSDL request if not null.  Otherwise returns a new RequestedShipment.</returns>
        /// <exception cref="CarrierException">If the shipment request is not supported a CarrierException is thrown.</exception>
        public static RequestedShipment GetShipServiceRequestedShipment(CarrierRequest request)
        {
            object nativeRequest = request.NativeRequest;

            if (nativeRequest == null)
            {
                throw new CarrierException("The native request is not allowed to be Null.");
            }

            if (request.NativeRequest is DeleteShipmentRequest)
            {
                throw new CarrierException("DeleteShipmentRequest is not a valid request type.");
            }

            if (!(nativeRequest is ProcessShipmentRequest) &&
                !(nativeRequest is ValidateShipmentRequest))
            {
                throw new CarrierException(request.NativeRequest + " is not a valid request type.");
            }

            RequestedShipment requestedShipment = (RequestedShipment) DuckGetProperty(request.NativeRequest, "RequestedShipment");

            // Create a new one if it doesn't already exist.
            if (requestedShipment == null)
            {
                requestedShipment = new RequestedShipment();
                Duck(request.NativeRequest, "RequestedShipment", requestedShipment);
            }

            return requestedShipment;
        }

        /// <summary>
        /// Create a FedEx API contact object from the given person
        /// </summary>
        public static T CreateContact<T>(PersonAdapter person) where T : new()
        {
            T contact = new T();

            Duck(contact, "PersonName", new PersonName(person).FullName);
            Duck(contact, "CompanyName", person.Company);
            Duck(contact, "EMailAddress", person.Email);
            Duck(contact, "FaxNumber", person.Fax);
            Duck(contact, "PhoneNumber", person.Phone);

            return contact;
        }

        /// <summary>
        /// Create a FedEx API "Parsed" contact
        /// </summary>
        public static T CreateParsedContact<T>(PersonAdapter person) where T : new()
        {
            T contact = new T();

            // We need to know what "ParsedPersonName" type to create
            object parsedPerson = Activator.CreateInstance(typeof(T).GetProperty("PersonName").PropertyType);

            Duck(parsedPerson, "FirstName", person.FirstName);
            Duck(parsedPerson, "LastName", person.LastName);
            Duck(contact, "PersonName", parsedPerson);

            Duck(contact, "CompanyName", person.Company);
            Duck(contact, "EMailAddress", person.Email);
            Duck(contact, "FaxNumber", person.Fax);
            Duck(contact, "PhoneNumber", person.Phone);

            return contact;
        }

        /// <summary>
        /// Create a FedEx API address object from the given person
        /// </summary>
        public static T CreateAddress<T>(PersonAdapter person) where T : new()
        {
            T address = new T();

            List<string> streetLines = new List<string>();
            streetLines.Add(person.Street1);

            if (!String.IsNullOrEmpty(person.Street2))
            {
                streetLines.Add(person.Street2);
            }

            if (!String.IsNullOrEmpty(person.Street3))
            {
                streetLines.Add(person.Street3);
            }

            Duck(address, "StreetLines", streetLines.ToArray());
            Duck(address, "City", person.City);
            Duck(address, "PostalCode", person.PostalCode);
            Duck(address, "StateOrProvinceCode", person.StateProvCode);
            Duck(address, "CountryCode", person.AdjustedCountryCode(ShipmentTypeCode.FedEx));

            return address;
        }

        /// <summary>
        /// Our pseudo duck typing that sets the given property to the specified value on the given object
        /// </summary>
        private static void Duck(object duck, string property, object value)
        {
            duck.GetType().GetProperty(property).SetValue(duck, value, null);
        }

        /// <summary>
        /// Our pseudo duck typing that gets the given property of the given object
        /// </summary>
        public static object DuckGetProperty(object duck, string property)
        {
            return duck.GetType().GetProperty(property).GetValue(duck, null);
        }

        /// <summary>
        /// Gets the FedEx Drop off type for the shipment
        /// </summary>
        public static DropoffType GetShipmentDropoffType(FedExDropoffType dropoffType)
        {
            switch (dropoffType)
            {
                case FedExDropoffType.BusinessServiceCenter: return DropoffType.BUSINESS_SERVICE_CENTER;
                case FedExDropoffType.DropBox: return DropoffType.DROP_BOX;
                case FedExDropoffType.RegularPickup: return DropoffType.REGULAR_PICKUP;
                case FedExDropoffType.RequestCourier: return DropoffType.REQUEST_COURIER;
                case FedExDropoffType.Station: return DropoffType.STATION;
            }

            throw new InvalidOperationException("Invalid FedEx ServiceType " + dropoffType);
        }

        /// <summary>
        /// Get the API service type based on our internal value
        /// </summary>
        public static ServiceType GetApiServiceType(FedExServiceType serviceType)
        {
            return (ServiceType) EnumHelper.GetApiValue<ServiceType>(serviceType);
        }

        /// <summary>
        /// Creates the shipping API web authentication detail.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>A WebAuthenticationDetail object for a shipping API request.</returns>
        public static WebAuthenticationDetail CreateShippingWebAuthenticationDetail(FedExSettings settings)
        {
            return new WebAuthenticationDetail
            {
                ParentCredential = new WebAuthenticationCredential
                {
                    Key = settings.CspCredentialKey,
                    Password = settings.CspCredentialPassword
                },

                UserCredential = new WebAuthenticationCredential
                {
                    Key = settings.UserCredentialsKey,
                    Password = settings.UserCredentialsPassword
                }
            };
        }

        /// <summary>
        /// Creates the shipping API client detail.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <returns>A ClientDetail object for a shipping API request.</returns>
        public static ClientDetail CreateShippingClientDetail(FedExAccountEntity account)
        {
            return new ClientDetail
            {
                AccountNumber = account.AccountNumber,
                MeterNumber = account.MeterNumber
            };
        }

        /// <summary>
        /// Creates the registration API web authentication detail.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>A WebAuthenticationDetail for a registration API request.</returns>
        public static WebServices.Registration.WebAuthenticationDetail CreateRegistrationWebAuthenticationDetail(FedExSettings settings)
        {
            return new WebServices.Registration.WebAuthenticationDetail
            {
                ParentCredential = new WebServices.Registration.WebAuthenticationCredential
                {
                    Key = settings.CspCredentialKey,
                    Password = settings.CspCredentialPassword
                },

                UserCredential = new WebServices.Registration.WebAuthenticationCredential
                {
                    Key = settings.UserCredentialsKey,
                    Password = settings.UserCredentialsPassword
                }
            };
        }

        /// <summary>
        /// Creates the shipping API client detail.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <returns>A ClientDetail object for a shipping API request.</returns>
        public static WebServices.Registration.ClientDetail CreateRegistrationClientDetail(FedExAccountEntity account)
        {
            return new WebServices.Registration.ClientDetail
            {
                AccountNumber = account.AccountNumber,
                MeterNumber = account.MeterNumber
            };
        }

        /// <summary>
        /// Creates the package movement API web authentication detail.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>A WebAuthenticationDetail for a registration API request.</returns>
        public static WebServices.PackageMovement.WebAuthenticationDetail CreatePackageMovementWebAuthenticationDetail(FedExSettings settings)
        {
            return new WebServices.PackageMovement.WebAuthenticationDetail
            {
                CspCredential = new WebServices.PackageMovement.WebAuthenticationCredential
                {
                    Key = settings.CspCredentialKey,
                    Password = settings.CspCredentialPassword
                },

                UserCredential = new WebServices.PackageMovement.WebAuthenticationCredential
                {
                    Key = settings.UserCredentialsKey,
                    Password = settings.UserCredentialsPassword
                }
            };
        }

        /// <summary>
        /// Creates the package movement API web authentication detail.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>A WebAuthenticationDetail for a rate API request.</returns>
        public static WebServices.Rate.WebAuthenticationDetail CreateRateWebAuthenticationDetail(FedExSettings settings)
        {
            return new WebServices.Rate.WebAuthenticationDetail
            {
                ParentCredential = new WebServices.Rate.WebAuthenticationCredential
                {
                    Key = settings.CspCredentialKey,
                    Password = settings.CspCredentialPassword
                },

                UserCredential = new WebServices.Rate.WebAuthenticationCredential
                {
                    Key = settings.UserCredentialsKey,
                    Password = settings.UserCredentialsPassword
                }
            };
        }

        /// <summary>
        /// Creates the shipping API client detail.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <param name="settings">The settings.</param>
        /// <returns>A ClientDetail object for a shipping API request.</returns>
        public static WebServices.PackageMovement.ClientDetail CreatePackageMovementClientDetail(FedExAccountEntity account, FedExSettings settings)
        {
            return new WebServices.PackageMovement.ClientDetail
            {
                AccountNumber = account.AccountNumber,
                ClientProductId = settings.ClientProductId,
                ClientProductVersion = settings.ClientProductVersion,
                MeterNumber = account.MeterNumber
            };
        }

        /// <summary>
        /// Creates the package movement API web authentication detail.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>A WebAuthenticationDetail for a registration API request.</returns>
        public static WebServices.Close.WebAuthenticationDetail CreateCloseWebAuthenticationDetail(FedExSettings settings)
        {
            return new WebServices.Close.WebAuthenticationDetail
            {
                ParentCredential = new WebServices.Close.WebAuthenticationCredential
                {
                    Key = settings.CspCredentialKey,
                    Password = settings.CspCredentialPassword
                },

                UserCredential = new WebServices.Close.WebAuthenticationCredential
                {
                    Key = settings.UserCredentialsKey,
                    Password = settings.UserCredentialsPassword
                }
            };
        }

        /// <summary>
        /// Creates the shipping API client detail.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <returns>A ClientDetail object for a close API request.</returns>
        public static WebServices.Close.ClientDetail CreateCloseClientDetail(IFedExAccountEntity account)
        {
            return new WebServices.Close.ClientDetail
            {
                AccountNumber = account.AccountNumber,
                MeterNumber = account.MeterNumber
            };
        }

        /// <summary>
        /// Creates the shipping API client detail.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <returns>A ClientDetail object for a rate API request.</returns>
        public static WebServices.Rate.ClientDetail CreateRateClientDetail(IFedExAccountEntity account)
        {
            if (account == null)
            {
                account = new FedExAccountEntity();
            }

            return new WebServices.Rate.ClientDetail
            {
                AccountNumber = account.AccountNumber,
                MeterNumber = account.MeterNumber
            };
        }

        /// <summary>
        /// Creates the void API web authentication detail.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>A WebAuthenticationDetail for a registration API request.</returns>
        public static WebAuthenticationDetail CreateVoidWebAuthenticationDetail(FedExSettings settings)
        {
            return new WebAuthenticationDetail
            {
                ParentCredential = new WebAuthenticationCredential
                {
                    Key = settings.CspCredentialKey,
                    Password = settings.CspCredentialPassword
                },

                UserCredential = new WebAuthenticationCredential
                {
                    Key = settings.UserCredentialsKey,
                    Password = settings.UserCredentialsPassword
                }
            };
        }

        /// <summary>
        /// Creates the void API client detail.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <returns>A ClientDetail object for a void API request.</returns>
        public static ClientDetail CreateVoidClientDetail(FedExAccountEntity account)
        {
            return new ClientDetail
            {
                AccountNumber = account.AccountNumber,
                MeterNumber = account.MeterNumber
            };
        }

        /// <summary>
        /// Creates the Track API web authentication detail.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>A WebAuthenticationDetail for a registration API request.</returns>
        public static WebServices.Track.WebAuthenticationDetail CreateTrackWebAuthenticationDetail(FedExSettings settings)
        {
            return new WebServices.Track.WebAuthenticationDetail
            {
                ParentCredential = new WebServices.Track.WebAuthenticationCredential
                {
                    Key = settings.CspCredentialKey,
                    Password = settings.CspCredentialPassword
                },

                UserCredential = new WebServices.Track.WebAuthenticationCredential
                {
                    Key = settings.UserCredentialsKey,
                    Password = settings.UserCredentialsPassword
                }
            };
        }

        /// <summary>
        /// Creates the void API client detail.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <returns>A ClientDetail object for a void API request.</returns>
        public static WebServices.Track.ClientDetail CreateTrackClientDetail(FedExAccountEntity account)
        {
            return new WebServices.Track.ClientDetail
            {
                AccountNumber = account.AccountNumber,
                MeterNumber = account.MeterNumber
            };
        }

        /// <summary>
        /// Returns true if code is gu or guam regardless of case.
        /// </summary>
        public static bool IsGuam(string code) => code.Equals("GU", StringComparison.OrdinalIgnoreCase) ||
                                                   code.Equals("Guam", StringComparison.OrdinalIgnoreCase);
    }
}
