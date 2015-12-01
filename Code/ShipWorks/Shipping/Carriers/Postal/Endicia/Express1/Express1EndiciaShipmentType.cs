using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Endicia.Express1.Registration;
using ShipWorks.Shipping.Carriers.Postal.Express1.Registration;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia.Express1
{
    /// <summary>
    /// Shipment type for Express 1 shipments.
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = false)]
    public class Express1EndiciaShipmentType : EndiciaShipmentType
    {
        /// <summary>
        /// Create an instance of the Express1 Endicia Shipment Type
        /// </summary>
        public Express1EndiciaShipmentType()
        {
            AccountRepository = new Express1EndiciaAccountRepository();
        }

        /// <summary>
        /// Postal Shipment Type
        /// </summary>
        public override ShipmentTypeCode ShipmentTypeCode => ShipmentTypeCode.Express1Endicia;

        /// <summary>
        /// The user-displayable name of the shipment type
        /// </summary>
        [Obfuscation(Exclude = true)]
        public override string ShipmentTypeName
        {
            get
            {
                return
                    (ShippingManager.IsShipmentTypeActivated(ShipmentTypeCode.Usps) ||
                    ShippingManager.IsShipmentTypeActivated(ShipmentTypeCode.Express1Usps)) ?
                    "USPS (Express1 for Endicia)" : "USPS (Express1)";
            }
        }

        /// <summary>
        /// Reseller type
        /// </summary>
        public override EndiciaReseller EndiciaReseller => EndiciaReseller.Express1;

        /// <summary>
        /// Gets the processing synchronizer to be used during the PreProcessing of a shipment.
        /// </summary>
        protected override IShipmentProcessingSynchronizer GetProcessingSynchronizer()
        {
            return new Express1EndiciaShipmentProcessingSynchronizer();
        }

        /// <summary>
        /// Create the Service Control
        /// </summary>
        /// <param name="rateControl">A handle to the rate control so the selected rate can be updated when
        /// a change to the shipment, such as changing the service type, matches a rate in the control</param>
        protected override ServiceControlBase InternalCreateServiceControl(RateControl rateControl)
        {
            return new Express1EndiciaServiceControl(rateControl);
        }

        /// <summary>
        /// Gets the package types that have been available for this shipment type
        /// </summary>
        public override IEnumerable<int> GetAvailablePackageTypes(IExcludedPackageTypeRepository repository)
        {
            // All package types including cubic are available to Express1/Endicia
            return EnumHelper.GetEnumList<PostalPackagingType>()
                .Select(x => x.Value)
                .Cast<int>()
                .Except(GetExcludedPackageTypes(repository));
        }

        /// <summary>
        /// Gets counter rates for a postal shipment
        /// </summary>
        /// <param name="shipment">Shipment for which to retrieve rates</param>
        protected override RateGroup GetCounterRates(ShipmentEntity shipment)
        {
            return GetCachedRates<EndiciaException>(shipment, entity => { throw new EndiciaException("An account is required to view Express1 rates."); });
        }

        /// <summary>
        /// Create the setup wizard for configuring an Express 1 account.
        /// </summary>
        public override ShipmentTypeSetupWizardForm CreateSetupWizard()
        {
            Express1Registration registration = new Express1Registration(ShipmentTypeCode, new EndiciaExpress1RegistrationGateway(), new EndiciaExpress1RegistrationRepository(), 
                new EndiciaExpress1PasswordEncryptionStrategy(), new Express1RegistrationValidator());

            EndiciaAccountManagerControl accountManagerControl = new EndiciaAccountManagerControl();
            EndiciaOptionsControl optionsControl = new EndiciaOptionsControl(EndiciaReseller.Express1);
            EndiciaBuyPostageDlg postageDlg = new EndiciaBuyPostageDlg();

            return new Express1SetupWizard(postageDlg, accountManagerControl, optionsControl, registration, EndiciaAccountManager.Express1Accounts);
        }

        /// <summary>
        /// Get the Express1 MailClass code for the given service
        /// </summary>
        public override string GetMailClassCode(PostalServiceType serviceType, PostalPackagingType packagingType)
        {
            // Express1 is not supporting the July 2013 Express/Priority Express updates, so this is the same
            // as the virtual method it overrides with the exception of ExpressMail returns "Express" instead
            // of "PriorityExpress"
            switch (serviceType)
            {
                case PostalServiceType.ExpressMail: return "Express";
                case PostalServiceType.FirstClass: return "First";
                case PostalServiceType.LibraryMail: return "LibraryMail";
                case PostalServiceType.MediaMail: return "MediaMail";
                case PostalServiceType.StandardPost: return "StandardPost";
                case PostalServiceType.ParcelSelect: return "ParcelSelect";
                case PostalServiceType.PriorityMail: return "Priority";
                case PostalServiceType.CriticalMail: return "CriticalMail";

                case PostalServiceType.InternationalExpress: return "ExpressMailInternational";
                case PostalServiceType.InternationalPriority: return "PriorityMailInternational";

                case PostalServiceType.InternationalFirst:
                    {
                        return PostalUtility.IsEnvelopeOrFlat(packagingType) ? "FirstClassMailInternational" : "FirstClassPackageInternationalService";
                    }
            }

            if (ShipmentTypeManager.IsEndiciaDhl(serviceType))
            {
                return EnumHelper.GetApiValue(serviceType);
            }

            throw new EndiciaException(string.Format("{0} is not supported when shipping with Endicia.", PostalUtility.GetPostalServiceTypeDescription(serviceType)));
        }

        /// <summary>
        /// Gets an instance to the best rate shipping broker for the Express1 for Endicia shipment type based on the shipment configuration.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>An instance of an Express1EndiciaBestRateBroker.</returns>
        public override IBestRateShippingBroker GetShippingBroker(ShipmentEntity shipment)
        {
            return new NullShippingBroker();
        }

        /// <summary>
        /// Supports getting counter rates.
        /// </summary>
        public override bool SupportsCounterRates => false;

        /// <summary>
        /// Update the label format of carrier specific unprocessed shipments
        /// </summary>
        public override void UpdateLabelFormatOfUnprocessedShipments(SqlAdapter adapter, int newLabelFormat, RelationPredicateBucket bucket)
        {
            // Don't update Express1 entries because they could overwrite Endicia records
        }

        /// <summary>
        /// Gets the filtered rates based on any excluded services configured for this postal shipment type.
        /// </summary>
        protected override List<RateResult> FilterRatesByExcludedServices(ShipmentEntity shipment, List<RateResult> rates)
        {
            List<PostalServiceType> availableServiceTypes = GetAvailableServiceTypes().Select(s => (PostalServiceType)s).ToList(); ;

            if (shipment.Postal.Endicia.OriginalEndiciaAccountID == null)
            {
                availableServiceTypes.Add((PostalServiceType)shipment.Postal.Service);
            }

            List<RateResult> rateResults = rates.Where(r => r.Tag is PostalRateSelection && availableServiceTypes.Contains(((PostalRateSelection)r.Tag).ServiceType)).ToList();

            rateResults.ForEach(r => r.ShipmentType = ShipmentTypeCode);

            return rateResults;
        }
    }
}
