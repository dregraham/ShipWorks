﻿using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Interapptive.Shared.ComponentRegistration;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;
using ShipWorks.Shipping.Carriers.Postal.Usps.Contracts;
using ShipWorks.Shipping.Carriers.Postal.Usps.Express1.Net;
using ShipWorks.Shipping.Carriers.Postal.WebTools;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Tracking;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.Express1
{
    /// <summary>
    /// Shipment type for Express 1 for USPS shipments.
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = false)]
    [KeyedComponent(typeof(IUspsShipmentType), ShipmentTypeCode.Express1Usps)]
    public class Express1UspsShipmentType : UspsShipmentType
    {
        /// <summary>
        /// Create an instance of the Express1 USPS Shipment Type
        /// </summary>
        public Express1UspsShipmentType()
        {
            AccountRepository = new Express1UspsAccountRepository();
        }

        /// <summary>
        /// Gets the shipment type code.
        /// </summary>
        public override ShipmentTypeCode ShipmentTypeCode
        {
            get
            {
                return ShipmentTypeCode.Express1Usps;
            }
        }

        /// <summary>
        /// Gets the type of the reseller.
        /// </summary>
        public override UspsResellerType ResellerType
        {
            get { return UspsResellerType.Express1; }
        }

        /// <summary>
        /// Gets a value indicating whether this shipment type has accounts
        /// </summary>
        public override bool HasAccounts
        {
            get
            {
                return UspsAccountManager.Express1Accounts.Any();
            }
        }

        /// <summary>
        /// The user-displayable name of the shipment type
        /// </summary>
        [Obfuscation(Exclude = true)]
        public override string ShipmentTypeName
        {
            get
            {
                return "USPS (Express1)";
            }
        }

        /// <summary>
        /// Supports getting counter rates.
        /// </summary>
        public override bool SupportsCounterRates
        {
            get { return false; }
        }

        /// <summary>
        /// Creates the web client to use to contact the underlying carrier API.
        /// </summary>
        /// <returns>An instance of IUspsWebClient.</returns>
        public override IUspsWebClient CreateWebClient()
        {
            return new Express1UspsWebClient(AccountRepository, new LogEntryFactory(), CertificateInspector);
        }

        /// <summary>
        /// Creates the Express1/USPS service control.
        /// </summary>
        /// <param name="rateControl">A handle to the rate control so the selected rate can be updated when
        /// a change to the shipment, such as changing the service type, matches a rate in the control</param>
        protected override ServiceControlBase InternalCreateServiceControl(RateControl rateControl)
        {
            return new Express1UspsServiceControl(rateControl);
        }

        /// <summary>
        /// Update the dynamic data of the shipment
        /// </summary>
        /// <param name="shipment"></param>
        public override void UpdateDynamicShipmentData(ShipmentEntity shipment)
        {
            base.UpdateDynamicShipmentData(shipment);
            shipment.InsuranceProvider = (int) InsuranceProvider.ShipWorks;
            shipment.Insurance = shipment.Postal.Usps.Insurance;
        }

        /// <summary>
        /// Gets an instance to the best rate shipping broker for the Express1 for USPS shipment type based on the shipment configuration.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>An instance of an Express1UspsBestRateBroker.</returns>
        public override IBestRateShippingBroker GetShippingBroker(ShipmentEntity shipment)
        {
            return new NullShippingBroker();
        }

        /// <summary>
        /// Will just assign the contract type of the account to Unknown and save the account to the repository.
        /// </summary>
        /// <param name="account">The account.</param>
        public override void UpdateContractType(UspsAccountEntity account)
        {
            // If the ContractType is unknown, we must not have tried to check this account yet.
            // Just assign the contract type to NotApplicable; we don't need to worry about Express1 accounts
            if (account != null && account.ContractType == (int) UspsAccountContractType.Unknown)
            {
                account.ContractType = (int) UspsAccountContractType.NotApplicable;
                AccountRepository.Save(account);
            }
        }

        /// <summary>
        /// Update the label format of carrier specific unprocessed shipments
        /// </summary>
        public override void UpdateLabelFormatOfUnprocessedShipments(SqlAdapter adapter, int newLabelFormat, RelationPredicateBucket bucket)
        {
            // Don't update Express1 entries because they could overwrite Usps records
        }

        /// <summary>
        /// Gets all of the confirmation types that are available to a particular implementation of PostalShipmentType.
        /// </summary>
        /// <returns>A collection of all the confirmation types that are available to a Express1 (USPS) shipment.</returns>
        public override IEnumerable<PostalConfirmationType> GetAllConfirmationTypes()
        {
            // The adult signature types are not available
            return new List<PostalConfirmationType>
            {
                PostalConfirmationType.None,
                PostalConfirmationType.Delivery,
                PostalConfirmationType.Signature,
                PostalConfirmationType.AdultSignatureRequired,
                PostalConfirmationType.AdultSignatureRestricted
            };
        }

        /// <summary>
        /// Track the given Express1 shipment
        /// </summary>
        public override TrackingResult TrackShipment(ShipmentEntity shipment)
        {
            return PostalWebClientTracking.TrackShipment(shipment.TrackingNumber);
        }
    }
}
