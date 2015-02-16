//using System;
//using System.Collections.Generic;
//using System.Drawing.Imaging;
//using System.Linq;
//using System.Windows.Forms;
//using Interapptive.Shared.Business;
//using Interapptive.Shared.Net;
//using log4net;
//using SD.LLBLGen.Pro.ORMSupportClasses;
//using ShipWorks.ApplicationCore.Licensing;
//using ShipWorks.ApplicationCore.Logging;
//using ShipWorks.Common.IO.Hardware.Printers;
//using ShipWorks.Data;
//using ShipWorks.Data.Model.EntityClasses;
//using ShipWorks.Data.Model.HelperClasses;
//using ShipWorks.Editions;
//using ShipWorks.Shipping.Carriers.BestRate;
//using ShipWorks.Shipping.Carriers.BestRate.Footnote;
//using ShipWorks.Shipping.Carriers.Postal.Express1;
//using ShipWorks.Shipping.Carriers.Postal.Stamps.Express1;
//using ShipWorks.Shipping.Carriers.Postal.Usps;
//using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;
//using ShipWorks.Shipping.Carriers.Postal.Usps.BestRate;
//using ShipWorks.Shipping.Carriers.Postal.Usps.Contracts;
//using ShipWorks.Shipping.Carriers.Postal.Usps.Express1.Net;
//using ShipWorks.Shipping.Carriers.Postal.Usps.RateFootnotes.Promotion;
//using ShipWorks.Shipping.Carriers.Postal.Usps.Registration.Promotion;
//using ShipWorks.Shipping.Editing;
//using ShipWorks.Shipping.Editing.Rating;
//using ShipWorks.Shipping.Profiles;
//using ShipWorks.Shipping.Settings;
//using ShipWorks.Shipping.Settings.Origin;
//using ShipWorks.Templates.Processing.TemplateXml.ElementOutlines;

//namespace ShipWorks.Shipping.Carriers.Postal.Stamps
//{
//    public class StampsShipmentType : PostalShipmentType
//    {

        

        

        
        


        
        


        


        
        
        


//        private RateGroup GetRatesFromApi(ShipmentEntity shipment)
//        {
//            List<RateResult> express1Rates = null;
//            ShippingSettingsEntity settings = ShippingSettings.Fetch();
//            bool isExpress1Restricted = ShipmentTypeManager.GetType(ShipmentTypeCode.Express1Stamps).IsShipmentTypeRestricted;

//            // See if this shipment should really go through Express1
//            if (shipment.ShipmentType == (int)ShipmentTypeCode.Stamps &&
//               settings.UspsAutomaticExpress1 && !isExpress1Restricted &&
//               Express1Utilities.IsValidPackagingType((PostalServiceType?)null, (PostalPackagingType)shipment.Postal.PackagingType))
//            {
//                var express1Account = UspsAccountManager.GetAccount(settings.UspsAutomaticExpress1Account);

//                if (express1Account == null)
//                {
//                    throw new UspsException("The Express1 account to automatically use when processing with Stamps.com has not been selected.");
//                }

//                // We temporarily turn this into an Exprss1 shipment to get rated
//                shipment.ShipmentType = (int)ShipmentTypeCode.Express1Stamps;
//                shipment.Postal.Usps.OriginalUspsAccountID = shipment.Postal.Usps.UspsAccountID;
//                shipment.Postal.Usps.UspsAccountID = express1Account.UspsAccountID;

//                try
//                {
//                    // Currently this actually recurses into this same method
//                    express1Rates = (ShouldRetrieveExpress1Rates) ?
//                        ShipmentTypeManager.GetType(shipment).GetRates(shipment).Rates.ToList() :
//                        new List<RateResult>();
//                }
//                catch (ShippingException)
//                {
//                    // Eat the exception; we don't want to stop someone from using Stamps if Express1 can't get rates
//                }
//                finally
//                {
//                    shipment.ShipmentType = (int)ShipmentTypeCode.Stamps;
//                    shipment.Postal.Usps.UspsAccountID = shipment.Postal.Usps.OriginalUspsAccountID.Value;
//                    shipment.Postal.Usps.OriginalUspsAccountID = null;
//                }
//            }

//            List<RateResult> stampsRates = CreateWebClient().GetRates(shipment);

//            // For Stamps, we want to either promote Express1 or show the Express1 savings
//            if (shipment.ShipmentType == (int)ShipmentTypeCode.Stamps)
//            {
//                if (ShouldRetrieveExpress1Rates && !IsRateDiscountMessagingRestricted)
//                {
//                    // Merge the discounted Express1 rates into the stamps.com rates
//                    return MergeDiscountedRates(shipment, stampsRates, express1Rates, settings);
//                }

//                RateGroup rateGroup = new RateGroup(stampsRates);
//                AddUspsRatePromotionFootnote(shipment, rateGroup);

//                return rateGroup;
//            }
//            else
//            {
//                // Express1 rates - return rates filtered by what is available to the user
//                RateGroup express1Group = BuildExpress1RateGroup(stampsRates, ShipmentTypeCode.Express1Stamps, ShipmentTypeCode.Express1Stamps);
//                if (IsRateDiscountMessagingRestricted)
//                {
//                    // (Express1) rate discount messaging is restricted, so we're allowed to add the USPS (Stamps.com Expedited)
//                    // promo footnote to show single account marketing dialog
//                    express1Group.AddFootnoteFactory(new UspsRatePromotionFootnoteFactory(this, shipment, true));
//                }

//                return express1Group;
//            }
//        }
        
//        /// <summary>
//        /// Merges the discounted rates with the Stamps.com rates.
//        /// </summary>
//        /// <param name="shipment">The shipment.</param>
//        /// <param name="stampsRates">The stamps rates.</param>
//        /// <param name="discountedRates">The discounted rates.</param>
//        /// <param name="settings">The settings.</param>
//        /// <returns>A RateGroup containing the merged rate results.</returns>
//        private RateGroup MergeDiscountedRates(ShipmentEntity shipment, List<RateResult> stampsRates, List<RateResult> discountedRates, ShippingSettingsEntity settings)
//        {
//            List<RateResult> finalRates = new List<RateResult>();

//            // Go through each Stamps rate
//            foreach (RateResult stampsRate in stampsRates)
//            {
//                PostalRateSelection stampsRateDetail = (PostalRateSelection)stampsRate.OriginalTag;
//                stampsRate.ShipmentType = ShipmentTypeCode.Stamps;

//                // If it's a rate they could (or have) saved on with Express1, we modify it
//                if (stampsRate.Selectable &&
//                    stampsRateDetail != null &&
//                    Express1Utilities.IsPostageSavingService(stampsRateDetail.ServiceType))
//                {
//                    // See if Express1 returned a rate for this service
//                    RateResult discountedRate = DetermineDiscountedRate(discountedRates, stampsRateDetail, stampsRate);

//                    // If Express1 returned a rate, check to make sure it is a lower amount
//                    if (discountedRate != null && discountedRate.Amount <= stampsRate.Amount)
//                    {
//                        finalRates.Add(discountedRate);
//                    }
//                    else
//                    {
//                        finalRates.Add(stampsRate);
//                    }
//                }
//                else
//                {
//                    RateResult discountedRate = DetermineDiscountedRate(discountedRates, stampsRateDetail, stampsRate);
//                    if (discountedRate != null)
//                    {
//                        stampsRate.ProviderLogo = discountedRate.ProviderLogo;
//                    }

//                    finalRates.Add(stampsRate);
//                }
//            }

//            RateGroup finalGroup = new RateGroup(finalRates.Select(e => { e.ShipmentType = ShipmentTypeCode.Stamps; return e; }).ToList());

//            // No longer show any Express1 related footnotes/promotions, but we always want to show the 
//            // USPS (Stamps.com Expedited) promotion when Express 1 is restricted and the account has not
//            // been converted from a commercial account
//            if (UspsAccountManager.Express1Accounts.Any() && !settings.StampsUspsAutomaticExpedited)
//            {
//                // Show the single account dialog if the customer has Express1 accounts and hasn't converted to USPS (Stamps.com Expedited)
//                finalGroup.AddFootnoteFactory(new UspsRatePromotionFootnoteFactory(this, shipment, true));
//            }
//            else if (AccountRepository.GetAccount(shipment.Postal.Usps.UspsAccountID).ContractType == (int)UspsAccountContractType.Commercial)
//            {
//                // Show the promotional footer for discounted rates 
//                finalGroup.AddFootnoteFactory(new UspsRatePromotionFootnoteFactory(this, shipment, false));
//            }

//            return finalGroup;
//        }

//        /// <summary>
//        /// Determines and returns a discounted RateResult if one exists.
//        /// </summary>
//        private static RateResult DetermineDiscountedRate(List<RateResult> discountedRates, PostalRateSelection stampsRateDetail, RateResult stampsRate)
//        {
//            RateResult discountedRate = null;
//            if (stampsRateDetail != null && discountedRates != null && discountedRates.Any(express1Rate => express1Rate.Selectable == stampsRate.Selectable))
//            {
//                discountedRate = discountedRates.Where(express1Rate => express1Rate.Selectable == stampsRate.Selectable)
//                                                .FirstOrDefault(express1Rate => ((PostalRateSelection)express1Rate.OriginalTag).ServiceType == stampsRateDetail.ServiceType &&
//                                                                                ((PostalRateSelection)express1Rate.OriginalTag).ConfirmationType == stampsRateDetail.ConfirmationType);

//                if (discountedRate != null)
//                {
//                    discountedRate.ShipmentType = stampsRate.ShipmentType;
//                }
//            }

//            return discountedRate;
//        }

        
//        /// <summary>
//        /// Allows the shipment type to run any pre-processing work that may need to be performed prior to
//        /// actually processing the shipment. In most cases this is checking to see if an account exists
//        /// and will call the counterRatesProcessing callback provided when trying to process a shipment
//        /// without any accounts for this shipment type in ShipWorks, otherwise the shipment is unchanged.
//        /// </summary>
//        /// <param name="shipment"></param>
//        /// <param name="counterRatesProcessing"></param>
//        /// <param name="selectedRate"></param>
//        /// <returns>
//        /// The updates shipment (or shipments) that is ready to be processed. A null value may
//        /// be returned to indicate that processing should be halted completely.
//        /// </returns>
//        public override List<ShipmentEntity> PreProcess(ShipmentEntity shipment, Func<CounterRatesProcessingArgs, DialogResult> counterRatesProcessing, RateResult selectedRate)
//        {
//            List<ShipmentEntity> shipments = new List<ShipmentEntity>();

//            // We need to handle the case where the Stamps.com shipment type was selected to process the shipment and there
//            // aren't any "native" Stamps.com accounts; we need to basically push the shipment over to USPS to create the 
//            // account under USPS and process it there
//            if (GetProcessingSynchronizer().HasAccounts && shipment.ShipmentType == (int) ShipmentTypeCode.Stamps)
//            {
//                // There is an existing "native" Stamps.com account, so do the pre-processing normally
//                shipments = base.PreProcess(shipment, counterRatesProcessing, selectedRate);

//                // Take this opportunity to try to update contract type of the account
//                if (shipment.Postal != null && shipment.Postal.Usps != null)
//                {
//                    UspsAccountEntity account = AccountRepository.GetAccount(shipment.Postal.Usps.UspsAccountID);
//                    UpdateContractType(account);
//                }
//            }
//            else
//            {
//                // There aren't any Stamps.com accounts, so we need to run the pre-process of the UspsShipmentType in order
//                // to create the account/process the shipment
//                shipment.ShipmentType = (int) ShipmentTypeCode.Usps;

//                UspsShipmentType uspsShipmentType = new UspsShipmentType();
//                shipments = uspsShipmentType.PreProcess(shipment, counterRatesProcessing, selectedRate);
//            }

//            return shipments;
//        }

//        /// <summary>
//        /// Process the shipment
//        /// </summary>
//        public override void ProcessShipment(ShipmentEntity shipment)
//        {
//            ValidateShipment(shipment);

//            bool useExpress1 = Express1Utilities.IsPostageSavingService(shipment) && !IsRateDiscountMessagingRestricted &&
//                Express1Utilities.IsValidPackagingType((PostalServiceType)shipment.Postal.Service, (PostalPackagingType)shipment.Postal.PackagingType) &&
//                ShippingSettings.Fetch().UspsAutomaticExpress1;

//            UspsAccountEntity express1Account = UspsAccountManager.GetAccount(ShippingSettings.Fetch().UspsAutomaticExpress1Account);

//            if (useExpress1)
//            {
//                if (express1Account == null)
//                {
//                    throw new ShippingException("The Express1 account to automatically use when processing with Stamps.com has not been selected.");
//                }

//                int originalShipmentType = shipment.ShipmentType;
//                long? originalStampsAccountID = shipment.Postal.Usps.OriginalUspsAccountID;
//                long stampsAccountID = shipment.Postal.Usps.UspsAccountID;

//                try
//                {
//                    // Check Stamps.com amount
//                    List<RateResult> stampsRates = CreateWebClient().GetRates(shipment);
//                    RateResult stampsRate = stampsRates.Where(er => er.Selectable).FirstOrDefault(er =>
//                                                                                                  ((PostalRateSelection)er.OriginalTag).ServiceType == (PostalServiceType)shipment.Postal.Service
//                                                                                                  && ((PostalRateSelection)er.OriginalTag).ConfirmationType == (PostalConfirmationType)shipment.Postal.Confirmation);

//                    // Check Express1 amount
//                    shipment.ShipmentType = (int)ShipmentTypeCode.Express1Stamps;
//                    shipment.Postal.Usps.OriginalUspsAccountID = shipment.Postal.Usps.UspsAccountID;
//                    shipment.Postal.Usps.UspsAccountID = express1Account.UspsAccountID;

//                    List<RateResult> express1Rates = new Express1UspsWebClient().GetRates(shipment);
//                    RateResult express1Rate = express1Rates.Where(er => er.Selectable).FirstOrDefault(er =>
//                                                                                                      ((PostalRateSelection)er.OriginalTag).ServiceType == (PostalServiceType)shipment.Postal.Service
//                                                                                                      && ((PostalRateSelection)er.OriginalTag).ConfirmationType == (PostalConfirmationType)shipment.Postal.Confirmation);

//                    // Now set useExpress1 to true only if the express 1 rate is less than the Stamps amount
//                    if (stampsRate != null && express1Rate != null)
//                    {
//                        useExpress1 = express1Rate.Amount <= stampsRate.Amount;
//                    }
//                    else
//                    {
//                        // If we can't figure it out for sure, don't use it
//                        useExpress1 = false;
//                    }
//                }
//                catch (UspsException stampsException)
//                {
//                    throw new ShippingException(stampsException.Message, stampsException);
//                }
//                finally
//                {
//                    // Reset back to the original values
//                    shipment.ShipmentType = originalShipmentType;
//                    shipment.Postal.Usps.OriginalUspsAccountID = originalStampsAccountID;
//                    shipment.Postal.Usps.UspsAccountID = stampsAccountID;
//                }
//            }

//            // See if this shipment should really go through Express1
//            if (useExpress1)
//            {
//                // Now we turn this into an Express1 shipment...
//                shipment.ShipmentType = (int)ShipmentTypeCode.Express1Stamps;
//                shipment.Postal.Usps.OriginalUspsAccountID = shipment.Postal.Usps.UspsAccountID;
//                shipment.Postal.Usps.UspsAccountID = express1Account.UspsAccountID;

//                Express1StampsShipmentType shipmentType = (Express1StampsShipmentType)ShipmentTypeManager.GetType(shipment);

//                // Process via Express1
//                shipmentType.UpdateDynamicShipmentData(shipment);
//                shipmentType.ProcessShipment(shipment);
//            }
//            else
//            {
//                // This would be set if they have tried to process as Express1 but it failed... make sure its clear since we are not using it.
//                shipment.Postal.Usps.OriginalUspsAccountID = null;

//                try
//                {
//                    CreateWebClient().ProcessShipment(shipment);
//                }
//                catch (UspsException ex)
//                {
//                    throw new ShippingException(ex.Message, ex);
//                }
//            }
//        }

        
        

        

        

        

        
        

        

        

        


        

        
        
//    }
//}
