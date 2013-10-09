using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.Api.v2013.Environment;

using Ship2013 = ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using Registration2013 = ShipWorks.Shipping.Carriers.FedEx.WebServices.v2013.Registration;
using PackageMovement2013 = ShipWorks.Shipping.Carriers.FedEx.WebServices.PackageMovement;
using Close2013 = ShipWorks.Shipping.Carriers.FedEx.WebServices.v2013.Close;
using Track2013 = ShipWorks.Shipping.Carriers.FedEx.WebServices.v2013.Track;
using ShipWorks.Shipping.Carriers.FedEx.Api.v2013.Enums;
using Rates2013 = ShipWorks.Shipping.Carriers.FedEx.WebServices.v2013.Rate;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.v2013
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
        public static Ship2013.RequestedShipment GetShipServiceRequestedShipment(CarrierRequest request)
        {
            object nativeRequest = request.NativeRequest;

            if (nativeRequest == null)
            {
                throw new CarrierException("The native request is not allowed to be Null.");
            }

            if (request.NativeRequest is Ship2013.CancelPendingShipmentRequest)
            {
                throw new CarrierException("CancelPendingShipmentRequest is not a valid request type.");
            }
            
            if (request.NativeRequest is Ship2013.DeleteShipmentRequest)
            {
                throw new CarrierException("DeleteShipmentRequest is not a valid request type.");
            }
            
            if (!(nativeRequest is Ship2013.ProcessShipmentRequest) && !(nativeRequest is Ship2013.CreatePendingShipmentRequest) && !(nativeRequest is Ship2013.ValidateShipmentRequest))
            {
                throw new CarrierException(request.NativeRequest.ToString() + " is not a valid request type.");
            }

            Ship2013.RequestedShipment requestedShipment = (Ship2013.RequestedShipment)FedExApiCore.DuckGetProperty(request.NativeRequest, "RequestedShipment");

            // Create a new one if it doesn't already exist.
            if (requestedShipment == null)
            {
                requestedShipment = new Ship2013.RequestedShipment();
                FedExApiCore.Duck(request.NativeRequest, "RequestedShipment", requestedShipment);
            }

            return requestedShipment;
        }

        /// <summary>
        /// Gets the FedEx Drop off type for the shipment
        /// </summary>
        public static Ship2013.DropoffType GetShipmentDropoffType(FedExDropoffType dropoffType)
        {
            switch (dropoffType)
            {
                case FedExDropoffType.BusinessServiceCenter: return Ship2013.DropoffType.BUSINESS_SERVICE_CENTER;
                case FedExDropoffType.DropBox:return Ship2013.DropoffType.DROP_BOX;
                case FedExDropoffType.RegularPickup: return Ship2013.DropoffType.REGULAR_PICKUP;
                case FedExDropoffType.RequestCourier: return Ship2013.DropoffType.REQUEST_COURIER;
                case FedExDropoffType.Station: return Ship2013.DropoffType.STATION;
            }

            throw new InvalidOperationException("Invalid FedEx ServiceType " + dropoffType);
        }


        /// <summary>
        /// Get the API service type based on our internal value
        /// </summary>
        public static Ship2013.ServiceType GetApiServiceType(FedExServiceType serviceType)
        {
            switch (serviceType)
            {
                case FedExServiceType.PriorityOvernight: return Ship2013.ServiceType.PRIORITY_OVERNIGHT;
                case FedExServiceType.StandardOvernight: return Ship2013.ServiceType.STANDARD_OVERNIGHT;
                case FedExServiceType.FirstOvernight: return Ship2013.ServiceType.FIRST_OVERNIGHT;
                case FedExServiceType.FedEx2Day: return Ship2013.ServiceType.FEDEX_2_DAY;
                case FedExServiceType.FedExExpressSaver: return Ship2013.ServiceType.FEDEX_EXPRESS_SAVER;
                case FedExServiceType.InternationalPriority: return Ship2013.ServiceType.INTERNATIONAL_PRIORITY;
                case FedExServiceType.InternationalEconomy: return Ship2013.ServiceType.INTERNATIONAL_ECONOMY;
                case FedExServiceType.InternationalFirst: return Ship2013.ServiceType.INTERNATIONAL_FIRST;
                case FedExServiceType.FedEx1DayFreight: return Ship2013.ServiceType.FEDEX_1_DAY_FREIGHT;
                case FedExServiceType.FedEx2DayFreight: return Ship2013.ServiceType.FEDEX_2_DAY_FREIGHT;
                case FedExServiceType.FedEx3DayFreight: return Ship2013.ServiceType.FEDEX_3_DAY_FREIGHT;
                case FedExServiceType.FedExGround: return Ship2013.ServiceType.FEDEX_GROUND;
                case FedExServiceType.GroundHomeDelivery: return Ship2013.ServiceType.GROUND_HOME_DELIVERY;
                case FedExServiceType.InternationalPriorityFreight: return Ship2013.ServiceType.INTERNATIONAL_PRIORITY_FREIGHT;
                case FedExServiceType.InternationalEconomyFreight: return Ship2013.ServiceType.INTERNATIONAL_ECONOMY_FREIGHT;
                case FedExServiceType.SmartPost: return Ship2013.ServiceType.SMART_POST;
                case FedExServiceType.FedEx2DayAM: return Ship2013.ServiceType.FEDEX_2_DAY_AM;
            }

            throw new InvalidOperationException("Invalid FedEx ServiceType " + serviceType);
        }

        /// <summary>
        /// Creates the shipping API web authentication detail.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>A WebAuthenticationDetail object for a shipping API request.</returns>
        public static Ship2013.WebAuthenticationDetail CreateShippingWebAuthenticationDetail(FedExSettings settings)
        {
            return new Ship2013.WebAuthenticationDetail
            {
                CspCredential = new Ship2013.WebAuthenticationCredential
                {
                    Key = settings.CspCredentialKey,
                    Password = settings.CspCredentialPassword
                },

                UserCredential = new Ship2013.WebAuthenticationCredential
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
        public static Ship2013.ClientDetail CreateShippingClientDetail(FedExAccountEntity account, FedExSettings settings)
        {
            return new Ship2013.ClientDetail
            {
                AccountNumber = account.AccountNumber,
                ClientProductId = settings.ClientProductId,
                ClientProductVersion = settings.ClientProductVersion,
                MeterNumber = account.MeterNumber
            };
        }

        /// <summary>
        /// Creates the registration API web authentication detail.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>A WebAuthenticationDetail for a registration API request.</returns>
        public static Registration2013.WebAuthenticationDetail CreateRegistrationWebAuthenticationDetail(FedExSettings settings)
        {
            return new Registration2013.WebAuthenticationDetail
            {
                CspCredential = new Registration2013.WebAuthenticationCredential
                {
                    Key = settings.CspCredentialKey,
                    Password = settings.CspCredentialPassword
                },

                UserCredential = new Registration2013.WebAuthenticationCredential
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
        public static Registration2013.ClientDetail CreateRegistrationClientDetail(FedExAccountEntity account, FedExSettings settings)
        {
            return new Registration2013.ClientDetail
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
        public static PackageMovement2013.WebAuthenticationDetail CreatePackageMovementWebAuthenticationDetail(FedExSettings settings)
        {
            return new PackageMovement2013.WebAuthenticationDetail
            {
                CspCredential = new PackageMovement2013.WebAuthenticationCredential
                {
                    Key = settings.CspCredentialKey,
                    Password = settings.CspCredentialPassword
                },

                UserCredential = new PackageMovement2013.WebAuthenticationCredential
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
        public static Rates2013.WebAuthenticationDetail CreateRateWebAuthenticationDetail(FedExSettings settings)
        {
            return new Rates2013.WebAuthenticationDetail
            {
                CspCredential = new Rates2013.WebAuthenticationCredential
                {
                    Key = settings.CspCredentialKey,
                    Password = settings.CspCredentialPassword
                },

                UserCredential = new Rates2013.WebAuthenticationCredential
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
        public static PackageMovement2013.ClientDetail CreatePackageMovementClientDetail(FedExAccountEntity account, FedExSettings settings)
        {
            return new PackageMovement2013.ClientDetail
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
        public static Close2013.WebAuthenticationDetail CreateCloseWebAuthenticationDetail(FedExSettings settings)
        {
            return new Close2013.WebAuthenticationDetail
            {
                CspCredential = new Close2013.WebAuthenticationCredential
                {
                    Key = settings.CspCredentialKey,
                    Password = settings.CspCredentialPassword
                },

                UserCredential = new Close2013.WebAuthenticationCredential
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
        /// <returns>A ClientDetail object for a close API request.</returns>
        public static Close2013.ClientDetail CreateCloseClientDetail(FedExAccountEntity account, FedExSettings settings)
        {
            return new Close2013.ClientDetail
            {
                AccountNumber = account.AccountNumber,
                ClientProductId = settings.ClientProductId,
                ClientProductVersion = settings.ClientProductVersion,
                MeterNumber = account.MeterNumber
            };
        }

        /// <summary>
        /// Creates the shipping API client detail.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <param name="settings">The settings.</param>
        /// <returns>A ClientDetail object for a rate API request.</returns>
        public static Rates2013.ClientDetail CreateRateClientDetail(FedExAccountEntity account, FedExSettings settings)
        {
            return new Rates2013.ClientDetail
            {
                AccountNumber = account.AccountNumber,
                ClientProductId = settings.ClientProductId,
                ClientProductVersion = settings.ClientProductVersion,
                MeterNumber = account.MeterNumber
            };
        }

        /// <summary>
        /// Creates the void API web authentication detail.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>A WebAuthenticationDetail for a registration API request.</returns>
        public static Ship2013.WebAuthenticationDetail CreateVoidWebAuthenticationDetail(FedExSettings settings)
        {
            return new Ship2013.WebAuthenticationDetail
            {
                CspCredential = new Ship2013.WebAuthenticationCredential
                {
                    Key = settings.CspCredentialKey,
                    Password = settings.CspCredentialPassword
                },

                UserCredential = new Ship2013.WebAuthenticationCredential
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
        /// <param name="settings">The settings.</param>
        /// <returns>A ClientDetail object for a void API request.</returns>
        public static Ship2013.ClientDetail CreateVoidClientDetail(FedExAccountEntity account, FedExSettings settings)
        {
            return new Ship2013.ClientDetail
            {
                AccountNumber = account.AccountNumber,
                ClientProductId = settings.ClientProductId,
                ClientProductVersion = settings.ClientProductVersion,
                MeterNumber = account.MeterNumber
            };
        }

        /// <summary>
        /// Creates the Track API web authentication detail.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>A WebAuthenticationDetail for a registration API request.</returns>
        public static Track2013.WebAuthenticationDetail CreateTrackWebAuthenticationDetail(FedExSettings settings)
        {
            return new Track2013.WebAuthenticationDetail
            {
                CspCredential = new Track2013.WebAuthenticationCredential
                {
                    Key = settings.CspCredentialKey,
                    Password = settings.CspCredentialPassword
                },

                UserCredential = new Track2013.WebAuthenticationCredential
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
        /// <param name="settings">The settings.</param>
        /// <returns>A ClientDetail object for a void API request.</returns>
        public static Track2013.ClientDetail CreateTrackClientDetail(FedExAccountEntity account, FedExSettings settings)
        {
            return new Track2013.ClientDetail
            {
                AccountNumber = account.AccountNumber,
                ClientProductId = settings.ClientProductId,
                ClientProductVersion = settings.ClientProductVersion,
                MeterNumber = account.MeterNumber
            };
        }
    }
}
