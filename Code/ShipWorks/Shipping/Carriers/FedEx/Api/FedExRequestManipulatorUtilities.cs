using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Enums;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using ShipWorks.Shipping.Carriers.FedEx.Enums;

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
        public static WebServices.Ship.RequestedShipment GetShipServiceRequestedShipment(CarrierRequest request)
        {
            object nativeRequest = request.NativeRequest;

            if (nativeRequest == null)
            {
                throw new CarrierException("The native request is not allowed to be Null.");
            }

            if (request.NativeRequest is WebServices.Ship.CancelPendingShipmentRequest)
            {
                throw new CarrierException("CancelPendingShipmentRequest is not a valid request type.");
            }
            
            if (request.NativeRequest is WebServices.Ship.DeleteShipmentRequest)
            {
                throw new CarrierException("DeleteShipmentRequest is not a valid request type.");
            }
            
            if (!(nativeRequest is ProcessShipmentRequest) && !(nativeRequest is CreatePendingShipmentRequest) && !(nativeRequest is WebServices.Ship.ValidateShipmentRequest))
            {
                throw new CarrierException(request.NativeRequest.ToString() + " is not a valid request type.");
            }

            WebServices.Ship.RequestedShipment requestedShipment = (WebServices.Ship.RequestedShipment)FedExApiCore.DuckGetProperty(request.NativeRequest, "RequestedShipment");

            // Create a new one if it doesn't already exist.
            if (requestedShipment == null)
            {
                requestedShipment = new WebServices.Ship.RequestedShipment();
                FedExApiCore.Duck(request.NativeRequest, "RequestedShipment", requestedShipment);
            }

            return requestedShipment;
        }

        /// <summary>
        /// Gets the FedEx Drop off type for the shipment
        /// </summary>
        public static WebServices.Ship.DropoffType GetShipmentDropoffType(FedExDropoffType dropoffType)
        {
            switch (dropoffType)
            {
                case FedExDropoffType.BusinessServiceCenter: return WebServices.Ship.DropoffType.BUSINESS_SERVICE_CENTER;
                case FedExDropoffType.DropBox:return WebServices.Ship.DropoffType.DROP_BOX;
                case FedExDropoffType.RegularPickup: return WebServices.Ship.DropoffType.REGULAR_PICKUP;
                case FedExDropoffType.RequestCourier: return WebServices.Ship.DropoffType.REQUEST_COURIER;
                case FedExDropoffType.Station: return WebServices.Ship.DropoffType.STATION;
            }

            throw new InvalidOperationException("Invalid FedEx ServiceType " + dropoffType);
        }


        /// <summary>
        /// Get the API service type based on our internal value
        /// </summary>
        public static WebServices.Ship.ServiceType GetApiServiceType(FedExServiceType serviceType)
        {
            switch (serviceType)
            {
                case FedExServiceType.PriorityOvernight: return WebServices.Ship.ServiceType.PRIORITY_OVERNIGHT;
                case FedExServiceType.StandardOvernight: return WebServices.Ship.ServiceType.STANDARD_OVERNIGHT;
                case FedExServiceType.FirstOvernight: return WebServices.Ship.ServiceType.FIRST_OVERNIGHT;
                case FedExServiceType.FedEx2Day: return WebServices.Ship.ServiceType.FEDEX_2_DAY;
                case FedExServiceType.FedExExpressSaver: return WebServices.Ship.ServiceType.FEDEX_EXPRESS_SAVER;
                case FedExServiceType.InternationalPriority: return WebServices.Ship.ServiceType.INTERNATIONAL_PRIORITY;
                case FedExServiceType.InternationalEconomy: return WebServices.Ship.ServiceType.INTERNATIONAL_ECONOMY;
                case FedExServiceType.InternationalFirst: return WebServices.Ship.ServiceType.INTERNATIONAL_FIRST;
                case FedExServiceType.FedEx1DayFreight: return WebServices.Ship.ServiceType.FEDEX_1_DAY_FREIGHT;
                case FedExServiceType.FedEx2DayFreight: return WebServices.Ship.ServiceType.FEDEX_2_DAY_FREIGHT;
                case FedExServiceType.FedEx3DayFreight: return WebServices.Ship.ServiceType.FEDEX_3_DAY_FREIGHT;
                case FedExServiceType.FedExGround: return WebServices.Ship.ServiceType.FEDEX_GROUND;
                case FedExServiceType.GroundHomeDelivery: return WebServices.Ship.ServiceType.GROUND_HOME_DELIVERY;
                case FedExServiceType.InternationalPriorityFreight: return WebServices.Ship.ServiceType.INTERNATIONAL_PRIORITY_FREIGHT;
                case FedExServiceType.InternationalEconomyFreight: return WebServices.Ship.ServiceType.INTERNATIONAL_ECONOMY_FREIGHT;
                case FedExServiceType.SmartPost: return WebServices.Ship.ServiceType.SMART_POST;
                case FedExServiceType.FedEx2DayAM: return WebServices.Ship.ServiceType.FEDEX_2_DAY_AM;
                case FedExServiceType.FirstFreight: return ServiceType.FEDEX_FIRST_FREIGHT;
            }

            throw new InvalidOperationException("Invalid FedEx ServiceType " + serviceType);
        }

        /// <summary>
        /// Creates the shipping API web authentication detail.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>A WebAuthenticationDetail object for a shipping API request.</returns>
        public static WebServices.Ship.WebAuthenticationDetail CreateShippingWebAuthenticationDetail(FedExSettings settings)
        {
            return new WebServices.Ship.WebAuthenticationDetail
            {
                CspCredential = new WebServices.Ship.WebAuthenticationCredential
                {
                    Key = settings.CspCredentialKey,
                    Password = settings.CspCredentialPassword
                },

                UserCredential = new WebServices.Ship.WebAuthenticationCredential
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
        public static WebServices.Ship.ClientDetail CreateShippingClientDetail(FedExAccountEntity account, FedExSettings settings)
        {
            return new WebServices.Ship.ClientDetail
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
        public static WebServices.v2013.Registration.WebAuthenticationDetail CreateRegistrationWebAuthenticationDetail(FedExSettings settings)
        {
            return new WebServices.v2013.Registration.WebAuthenticationDetail
            {
                CspCredential = new WebServices.v2013.Registration.WebAuthenticationCredential
                {
                    Key = settings.CspCredentialKey,
                    Password = settings.CspCredentialPassword
                },

                UserCredential = new WebServices.v2013.Registration.WebAuthenticationCredential
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
        public static WebServices.v2013.Registration.ClientDetail CreateRegistrationClientDetail(FedExAccountEntity account, FedExSettings settings)
        {
            return new WebServices.v2013.Registration.ClientDetail
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
                CspCredential = new WebServices.Rate.WebAuthenticationCredential
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
                CspCredential = new WebServices.Close.WebAuthenticationCredential
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
        /// <param name="settings">The settings.</param>
        /// <returns>A ClientDetail object for a close API request.</returns>
        public static WebServices.Close.ClientDetail CreateCloseClientDetail(FedExAccountEntity account, FedExSettings settings)
        {
            return new WebServices.Close.ClientDetail
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
        public static WebServices.Rate.ClientDetail CreateRateClientDetail(FedExAccountEntity account, FedExSettings settings)
        {
            return new WebServices.Rate.ClientDetail
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
        public static WebServices.Ship.WebAuthenticationDetail CreateVoidWebAuthenticationDetail(FedExSettings settings)
        {
            return new WebServices.Ship.WebAuthenticationDetail
            {
                CspCredential = new WebServices.Ship.WebAuthenticationCredential
                {
                    Key = settings.CspCredentialKey,
                    Password = settings.CspCredentialPassword
                },

                UserCredential = new WebServices.Ship.WebAuthenticationCredential
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
        public static WebServices.Ship.ClientDetail CreateVoidClientDetail(FedExAccountEntity account, FedExSettings settings)
        {
            return new WebServices.Ship.ClientDetail
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
        public static WebServices.v2013.Track.WebAuthenticationDetail CreateTrackWebAuthenticationDetail(FedExSettings settings)
        {
            return new WebServices.v2013.Track.WebAuthenticationDetail
            {
                CspCredential = new WebServices.v2013.Track.WebAuthenticationCredential
                {
                    Key = settings.CspCredentialKey,
                    Password = settings.CspCredentialPassword
                },

                UserCredential = new WebServices.v2013.Track.WebAuthenticationCredential
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
        public static WebServices.v2013.Track.ClientDetail CreateTrackClientDetail(FedExAccountEntity account, FedExSettings settings)
        {
            return new WebServices.v2013.Track.ClientDetail
            {
                AccountNumber = account.AccountNumber,
                ClientProductId = settings.ClientProductId,
                ClientProductVersion = settings.ClientProductVersion,
                MeterNumber = account.MeterNumber
            };
        }
    }
}
