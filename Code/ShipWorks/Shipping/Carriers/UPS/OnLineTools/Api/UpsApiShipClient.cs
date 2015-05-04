using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Net;
using ShipWorks.Data.Model.EntityClasses;
using System.Xml;
using ShipWorks.Email;
using ShipWorks.Shipping.Carriers.Postal.Endicia.WebServices.LabelService;
using ShipWorks.Shipping.Carriers.UPS.BestRate;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api.ElementWriters;
using ShipWorks.Shipping.Carriers.UPS.UpsEnvironment;
using ShipWorks.Shipping.Carriers.UPS.WorldShip;
using ShipWorks.Shipping.Settings;
using ShipWorks.Data;
using Interapptive.Shared.Utility;
using System.Text.RegularExpressions;
using System.Xml.XPath;
using System.Diagnostics;
using ShipWorks.Data.Connection;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using ShipWorks.Stores.Platforms.ChannelAdvisor.WebServices.Order;
using ShipWorks.UI;
using Interapptive.Shared.Business;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.UPS.ServiceManager;
using ShipWorks.Common.IO.Hardware.Printers;

namespace ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api
{
    /// <summary>
    /// API wrapper for shipping via UPS
    /// </summary>
    public static class UpsApiShipClient
    {
        /// <summary>
        /// Process the given UPS shipment
        /// </summary>
        public static void ProcessShipment(ShipmentEntity shipment)
        {
            XmlDocument confirmResponse = ProcessShipConfirm(shipment);

            // Create the XPath engine and get the digest
            XPathNavigator xpath = confirmResponse.CreateNavigator();

            ProcessShipAccept(shipment, xpath);
        }

        /// <summary>
        /// Process the confirm phase (1) of the ups shipment
        /// </summary>
        private static XmlDocument ProcessShipConfirm(ShipmentEntity shipment)
        {
            UpsAccountEntity account = UpsApiCore.GetUpsAccount(shipment, new UpsAccountRepository());

            // Create the client for connecting to the UPS server
            XmlTextWriter xmlWriter = UpsWebClient.CreateRequest(UpsOnLineToolType.ShipConfirm, account);

            UpsShipmentEntity ups = shipment.Ups;
            UpsRateType accountRateType = (UpsRateType) account.RateType;

            bool isSurePost = UpsUtility.IsUpsSurePostService((UpsServiceType) ups.Service);

            // Labels
            xmlWriter.WriteStartElement("LabelSpecification");

            // Thermal
            if (shipment.RequestedLabelFormat != (int) ThermalLanguage.None)
            {
                shipment.ActualLabelFormat = shipment.RequestedLabelFormat;

                xmlWriter.WriteStartElement("LabelPrintMethod");
                xmlWriter.WriteElementString("Code", shipment.RequestedLabelFormat == (int) ThermalLanguage.EPL ? "EPL" : "ZPL");
                xmlWriter.WriteEndElement();
                xmlWriter.WriteStartElement("LabelStockSize");
                xmlWriter.WriteElementString("Height", "4");
                xmlWriter.WriteElementString("Width", "6");
                xmlWriter.WriteEndElement();
            }

            // GIF
            else
            {
                shipment.ActualLabelFormat = null;

                xmlWriter.WriteStartElement("LabelPrintMethod");
                xmlWriter.WriteElementString("Code", "GIF");
                xmlWriter.WriteEndElement();
                xmlWriter.WriteStartElement("LabelImageFormat");
                xmlWriter.WriteElementString("Code", "GIF");
                xmlWriter.WriteEndElement();
            }

            xmlWriter.WriteEndElement();

            // Main node
            xmlWriter.WriteStartElement("Shipment");

            // Write SurePost fields.
            WriteSurePostShipment(ups, xmlWriter);

            // Write MI shipment values
            WriteMiShipmentValues(ups, xmlWriter);

            // Return Service Code
            if (shipment.ReturnShipment && !isSurePost)
            {
                xmlWriter.WriteStartElement("ReturnService");
                xmlWriter.WriteElementString("Code", UpsApiCore.GetReturnServiceCode((UpsReturnServiceType)ups.ReturnService));
                xmlWriter.WriteEndElement();
            }

            UpsServiceManagerFactory serviceManagerFactory = new UpsServiceManagerFactory(shipment);
            IUpsServiceManager upsServiceManager = serviceManagerFactory.Create(shipment);
            UpsServiceMapping upsServiceMapping = upsServiceManager.GetServices(shipment).Single(x => x.UpsServiceType == (UpsServiceType) ups.Service);

            // Service Code
            xmlWriter.WriteStartElement("Service");
            xmlWriter.WriteElementString("Code", upsServiceMapping.ShipServiceCode);
            xmlWriter.WriteEndElement();

            UpsApiCore.WriteShipmentReference(ups.ReferenceNumber, ups, xmlWriter, true);
            UpsApiCore.WriteShipmentReference(ups.ReferenceNumber2, ups, xmlWriter, true);

            // International stuff
            if (!ShipmentTypeManager.GetType(shipment).IsDomestic(shipment))
            {
                string customsDescription = ups.CustomsDescription;

                // MI only allows 35 characters for customs descriptions
                if (UpsUtility.IsUpsMiService((UpsServiceType) ups.Service))
                {
                    customsDescription = customsDescription.Length <= 35 ? customsDescription : customsDescription.Substring(0, 35);
                }
                xmlWriter.WriteElementString("Description", customsDescription);

                // Documents Only
                if (ups.CustomsDocumentsOnly)
                {
                    xmlWriter.WriteElementString("DocumentsOnly", null);
                }

                // Trial and error to figure out when InvoiceLineTotal is required:
                //                                  From			
                //                          US		                CA/PR	
                //              Return	    Not Return	    Return      Not Return
                //=================================================================
                //To	US	 |  Not Allowed	Not Allowed	    Required	Not Allowed
                //    CA/PR	 |  Not Allowed	Required	    Not Allowed	Not Allowed
                if ((shipment.ReturnShipment && ((shipment.AdjustedOriginCountryCode() == "CA" || shipment.AdjustedOriginCountryCode() == "PR") && shipment.AdjustedShipCountryCode() == "US")) ||
                    (!shipment.ReturnShipment && (shipment.AdjustedOriginCountryCode() == "US" && (shipment.AdjustedShipCountryCode() == "CA" || shipment.AdjustedShipCountryCode() == "PR"))))
                {
                    xmlWriter.WriteStartElement("InvoiceLineTotal");
                    xmlWriter.WriteElementString("CurrencyCode", UpsUtility.GetCurrency(account));
                    xmlWriter.WriteElementString("MonetaryValue", shipment.CustomsValue.ToString("0"));
                    xmlWriter.WriteEndElement();
                }
            }

            PersonAdapter origin = new PersonAdapter(shipment, "Origin");
            PersonAdapter recipient = new PersonAdapter(shipment, "Ship");

            // Shipper
            xmlWriter.WriteStartElement("Shipper");
            xmlWriter.WriteElementString("Name", origin.Company);
            xmlWriter.WriteElementString("AttentionName", new PersonName(origin).FullName);
            xmlWriter.WriteElementString("ShipperNumber", account.AccountNumber);
            xmlWriter.WriteElementString("PhoneNumber", Regex.Replace(origin.Phone, @"\D", ""));
            xmlWriter.WriteElementString("EMailAddress", UpsUtility.GetCorrectedEmailAddress(origin.Email));
            UpsApiCore.WriteAddressXml(xmlWriter, origin);
            xmlWriter.WriteEndElement();

            // if this is a return shipment, need to change ShipTo/ShipFrom
            if (shipment.ReturnShipment && !isSurePost)
            {
                PersonAdapter temp = origin;
                origin = recipient;
                recipient = temp;
            }

            #region Determine name, company, and attention

            string companyOrName = "";
            string attention = "";

            // If company is set, use it as the main name
            if (recipient.Company.Length > 0)
            {
                companyOrName = recipient.Company;
                attention = new PersonName(recipient).FullName;
            }
            else
            {
                companyOrName = new PersonName(recipient).FullName;
                
                // SurePost displays the Attention field on the label, not company, so ve sure to set it.
                if (isSurePost)
                {
                    attention = companyOrName;
                }
            }

            // International requires attention
            if (attention.Length == 0 && (!ShipmentTypeManager.GetType(shipment).IsDomestic(shipment) || PostalUtility.IsMilitaryState(shipment.ShipStateProvCode)))
            {
                attention = companyOrName;
            }

            #endregion

            // ShipTo
            xmlWriter.WriteStartElement("ShipTo");
            xmlWriter.WriteElementString("CompanyName", StringUtility.Truncate(companyOrName, 35));
            xmlWriter.WriteElementString("AttentionName", StringUtility.Truncate(attention, 35));
            xmlWriter.WriteElementString("PhoneNumber", Regex.Replace(recipient.Phone, @"\D", ""));
            xmlWriter.WriteElementString("EMailAddress", UpsUtility.GetCorrectedEmailAddress(recipient.Email));
            UpsApiCore.WriteAddressXml(xmlWriter, recipient, ResidentialDeterminationService.DetermineResidentialAddress(shipment) ? "ResidentialAddress" : (string) null );
            xmlWriter.WriteEndElement();

            // ShipFrom
            string attentionName = new PersonName(origin).FullName;
            string fromCompany = origin.Company;
            if (fromCompany.Length == 0 && shipment.ReturnShipment)
            {
                fromCompany = attentionName;
            }

            xmlWriter.WriteStartElement("ShipFrom");
            xmlWriter.WriteElementString("CompanyName", StringUtility.Truncate(fromCompany, 35));
            xmlWriter.WriteElementString("AttentionName", StringUtility.Truncate(attentionName, 35));
            xmlWriter.WriteElementString("PhoneNumber", Regex.Replace(origin.Phone, @"\D", ""));
            UpsApiCore.WriteAddressXml(xmlWriter, origin);
            xmlWriter.WriteEndElement();

            // Request Negotiated Rates?
            if (accountRateType == UpsRateType.Negotiated)
            {
                // Rate Information
                xmlWriter.WriteStartElement("RateInformation");

                // Requesting Negotiated Rates 
                xmlWriter.WriteElementString("NegotiatedRatesIndicator", "");

                // Close element
                xmlWriter.WriteEndElement();
            }

            // Write billing options.  Each method makes sure it's domestic or international before writing anything.
            WriteDomesticBilling(ups, account, xmlWriter);
            WriteInternationalBilling(ups, account, xmlWriter);

            // Commercial invoice requires Sold To 
            if (!ShipmentTypeManager.GetType(shipment).IsDomestic(shipment) && ups.CommercialPaperlessInvoice && !isSurePost)
            {
                xmlWriter.WriteStartElement("SoldTo");
                xmlWriter.WriteElementString("CompanyName", companyOrName);
                xmlWriter.WriteElementString("AttentionName", attention);
                xmlWriter.WriteElementString("PhoneNumber", Regex.Replace(recipient.Phone, @"\D", ""));
                UpsApiCore.WriteAddressXml(xmlWriter, recipient);
                xmlWriter.WriteEndElement();
            }

            // Start element
            xmlWriter.WriteStartElement("ShipmentServiceOptions");

            WriteReturnShipmentOptions(account, ups, xmlWriter);

            // Commercial invoice
            WriteInternationalFormsXml(account, ups, xmlWriter);

            // If they want saturday delivery, and it could be delivered on a saturday, set that flag.
            // Note: we don't usually use the selected service for figuring what the rates are - but we do here, since we only want
            // to use the saturday flag if the user can acutally see the saturday checkbox.
            if (ups.SaturdayDelivery && UpsUtility.CanDeliverOnSaturday((UpsServiceType) ups.Service, shipment.ShipDate))
            {
                xmlWriter.WriteElementString("SaturdayDelivery", "");
            }

            // Email
            WriteEmailNotificationXml(ups, xmlWriter);

            if (ups.CarbonNeutral)
            {
                // Element just needs to be present
                xmlWriter.WriteElementString("UPScarbonneutralIndicator", string.Empty);
            }

            // Write out Delivery Confirmation, if valid
            UpsDeliveryConfirmationElementWriter upsDeliveryConfirmationElementWriter = new UpsDeliveryConfirmationElementWriter(xmlWriter);
            upsDeliveryConfirmationElementWriter.WriteShipmentDeliveryConfirmationElement(shipment.Ups);

            // Close element
            xmlWriter.WriteEndElement();

            UpsServiceType serviceType = (UpsServiceType) ups.Service;
            if (UpsUtility.IsUpsSurePostService(serviceType))
            {
                // Write the package XML with the SurePost specific writers
                UpsApiCore.WritePackagesXml(ups, xmlWriter, true, new UpsSurePostPackageWeightWriter(xmlWriter, serviceType), new UpsSurePostPackageServiceOptionsElementWriter(xmlWriter));
            }
            else
            {
                // Write the package XML with the "standard" writers
                UpsApiCore.WritePackagesXml(ups, xmlWriter, true, new UpsPackageWeightElementWriter(xmlWriter), new UpsPackageServiceOptionsElementWriter(xmlWriter));
            }

            return UpsWebClient.ProcessRequest(xmlWriter, new TrustingCertificateInspector());
        }
        
        /// <summary>
        /// Writes the Billing payment info for the shipment IF it's an International shipment.
        /// </summary>
        private static void WriteInternationalBilling(UpsShipmentEntity ups, UpsAccountEntity account, XmlWriter xmlWriter)
        {
            if (!ShipmentTypeManager.GetType(ups.Shipment).IsDomestic(ups.Shipment))
            {
                // Payment info (SurePost must be sender)
                if (ups.PayorType == (int) UpsPayorType.Sender || UpsUtility.IsUpsSurePostService((UpsServiceType) ups.Service))
                {
                    xmlWriter.WriteStartElement("ItemizedPaymentInformation");
                        // Write the transportation shipment charge
                        xmlWriter.WriteStartElement("ShipmentCharge");
                            xmlWriter.WriteElementString("Type", "01");
                            xmlWriter.WriteStartElement("BillShipper");
                                xmlWriter.WriteElementString("AccountNumber", account.AccountNumber);
                            xmlWriter.WriteEndElement();
                        xmlWriter.WriteEndElement();

                        // Now write the tax/duty shipment charge
                        WriteInternationalBillingDutiesTaxesShipmentCharge(ups, account, xmlWriter);

                    xmlWriter.WriteEndElement();
                }
                else if (ups.PayorType == (int) UpsPayorType.ThirdParty)
                {
                    xmlWriter.WriteStartElement("ItemizedPaymentInformation");
                        // Write the transportation shipment charge
                        xmlWriter.WriteStartElement("ShipmentCharge");
                            xmlWriter.WriteElementString("Type", "01");
                            xmlWriter.WriteStartElement("BillThirdParty");
                                xmlWriter.WriteStartElement("BillThirdPartyShipper");
                                    xmlWriter.WriteElementString("AccountNumber", ups.PayorAccount);
                                    xmlWriter.WriteStartElement("ThirdParty");
                                        xmlWriter.WriteStartElement("Address");
                                            xmlWriter.WriteElementString("PostalCode", ups.PayorPostalCode);
                                            xmlWriter.WriteElementString("CountryCode", ups.PayorCountryCode);
                                        xmlWriter.WriteEndElement();
                                    xmlWriter.WriteEndElement();
                                xmlWriter.WriteEndElement();
                            xmlWriter.WriteEndElement();
                        xmlWriter.WriteEndElement();

                        // Now write the tax/duty shipment charge
                        WriteInternationalBillingDutiesTaxesShipmentCharge(ups, account, xmlWriter);

                    xmlWriter.WriteEndElement();
                }
                else
                {
                    xmlWriter.WriteStartElement("ItemizedPaymentInformation");
                        // Write the transportation shipment charge
                        xmlWriter.WriteStartElement("ShipmentCharge");
                            xmlWriter.WriteElementString("Type", "01");
                    
                            xmlWriter.WriteStartElement("BillReceiver");
                                xmlWriter.WriteElementString("AccountNumber", ups.PayorAccount);
                                xmlWriter.WriteStartElement("Address");
                                    xmlWriter.WriteElementString("PostalCode", ups.PayorPostalCode);
                                xmlWriter.WriteEndElement();
                            xmlWriter.WriteEndElement();

                        xmlWriter.WriteEndElement();

                        // Now write the tax/duty shipment charge
                        WriteInternationalBillingDutiesTaxesShipmentCharge(ups, account, xmlWriter);

                    xmlWriter.WriteEndElement();
                }
            }
        }

        /// <summary>
        /// Write the ShipmentCharge needed for the specific shipment charge type selected
        /// </summary>
        private static void WriteInternationalBillingDutiesTaxesShipmentCharge(UpsShipmentEntity ups, UpsAccountEntity account, XmlWriter xmlWriter)
        {
            UpsShipmentChargeType upsShipmentChargeType = (UpsShipmentChargeType)ups.ShipmentChargeType;

            if (UpsUtility.IsUpsMiService((UpsServiceType) ups.Service))
            {
                // No duties payor designation is allowed for MI
                return;
            }

            // If the user didn't enter an account number for billing the receiver, just return 
            if (upsShipmentChargeType == UpsShipmentChargeType.BillReceiver && (string.IsNullOrWhiteSpace(ups.ShipmentChargeAccount) || string.IsNullOrWhiteSpace(ups.ShipmentChargePostalCode)))
            {
                return;
            }

            // Now write the tax/duty shipment charge
            xmlWriter.WriteStartElement("ShipmentCharge");
                xmlWriter.WriteElementString("Type", "02");

                switch (upsShipmentChargeType)
                {
                    case UpsShipmentChargeType.BillShipper:
                        xmlWriter.WriteStartElement("BillShipper");
                            xmlWriter.WriteElementString("AccountNumber", account.AccountNumber);
                        xmlWriter.WriteEndElement();
                        break;
                    case UpsShipmentChargeType.BillReceiver:
                        xmlWriter.WriteStartElement("BillReceiver");
                            xmlWriter.WriteElementString("AccountNumber", ups.ShipmentChargeAccount);
                            xmlWriter.WriteStartElement("Address");
                                xmlWriter.WriteElementString("PostalCode", ups.ShipmentChargePostalCode);
                            xmlWriter.WriteEndElement();
                        xmlWriter.WriteEndElement();
                        break;
                    case UpsShipmentChargeType.BillThirdParty:
                        xmlWriter.WriteStartElement("BillThirdParty");
                        xmlWriter.WriteStartElement("BillThirdPartyConsignee");
                                xmlWriter.WriteElementString("AccountNumber", ups.ShipmentChargeAccount);
                                xmlWriter.WriteStartElement("ThirdParty");
                                    xmlWriter.WriteStartElement("Address");
                                        xmlWriter.WriteElementString("PostalCode", ups.ShipmentChargePostalCode);
                                        xmlWriter.WriteElementString("CountryCode", ups.ShipmentChargeCountryCode);
                                    xmlWriter.WriteEndElement();
                                xmlWriter.WriteEndElement();
                            xmlWriter.WriteEndElement();
                        xmlWriter.WriteEndElement();
                        break;
                }
            xmlWriter.WriteEndElement();
        }

        /// <summary>
        /// Writes the Billing payment info for the shipment IF it's a domestic shipment.
        /// </summary>
        private static void WriteDomesticBilling(UpsShipmentEntity ups, UpsAccountEntity account, XmlWriter xmlWriter)
        {
            if (ShipmentTypeManager.GetType(ups.Shipment).IsDomestic(ups.Shipment))
            {
                // Payment info (SurePost must be sender)
                if (ups.PayorType == (int) UpsPayorType.Sender ||
                    UpsUtility.IsUpsSurePostService((UpsServiceType) ups.Service))
                {
                    xmlWriter.WriteStartElement("PaymentInformation");
                    xmlWriter.WriteStartElement("Prepaid");
                    xmlWriter.WriteStartElement("BillShipper");
                    xmlWriter.WriteElementString("AccountNumber", account.AccountNumber);
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndElement();
                }
                else if (ups.PayorType == (int) UpsPayorType.ThirdParty)
                {
                    xmlWriter.WriteStartElement("PaymentInformation");
                    xmlWriter.WriteStartElement("BillThirdParty");
                    xmlWriter.WriteStartElement("BillThirdPartyShipper");
                    xmlWriter.WriteElementString("AccountNumber", ups.PayorAccount);
                    xmlWriter.WriteStartElement("ThirdParty");
                    xmlWriter.WriteStartElement("Address");
                    xmlWriter.WriteElementString("PostalCode", ups.PayorPostalCode);
                    xmlWriter.WriteElementString("CountryCode", ups.PayorCountryCode);
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndElement();
                }
                else
                {
                    xmlWriter.WriteStartElement("PaymentInformation");
                    xmlWriter.WriteStartElement("FreightCollect");
                    xmlWriter.WriteStartElement("BillReceiver");
                    xmlWriter.WriteElementString("AccountNumber", ups.PayorAccount);
                    xmlWriter.WriteStartElement("Address");
                    xmlWriter.WriteElementString("PostalCode", ups.PayorPostalCode);
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndElement();
                }
            }
        }

        /// <summary>
        /// Writes the appropriate Return Service service options
        /// </summary>
        private static void WriteReturnShipmentOptions(UpsAccountEntity account, UpsShipmentEntity ups, XmlTextWriter xmlWriter)
        {
            // only the Electronic Return return type needs recipient information
            if (ups.Shipment.ReturnShipment && ups.ReturnService == (int)UpsReturnServiceType.ElectronicReturnLabel)
            {
                // Start LabelDelivery
                xmlWriter.WriteStartElement("LabelDelivery");

                // always ask for Label Recovery and Receipt Recovery URL to be returned
                xmlWriter.WriteElementString("LabelLinkIndicator", "");

                // start email
                xmlWriter.WriteStartElement("EMailMessage");

                xmlWriter.WriteElementString("EMailAddress", UpsUtility.GetCorrectedEmailAddress(ups.Shipment.ShipEmail));

                // it is coming from the UPS email address
                xmlWriter.WriteElementString("FromEMailAddress", UpsUtility.GetCorrectedEmailAddress(account.Email));

                // use undeliverable address if supplied.  UPS will default this to the FromEmailAddress
                if (ups.ReturnUndeliverableEmail.Length > 0)
                {
                    xmlWriter.WriteElementString("UndeliverableEMailAddress", UpsUtility.GetCorrectedEmailAddress(ups.ReturnUndeliverableEmail));
                }

                // Reference number to appear in the subject
                xmlWriter.WriteElementString("SubjectCode", "01");

                // end email
                xmlWriter.WriteEndElement();

                // Close LabelDelivery
                xmlWriter.WriteEndElement();
            }
        }

        /// <summary>
        /// Write the XML for generating UPS commercial invoice
        /// </summary>
        private static void WriteInternationalFormsXml(UpsAccountEntity account, UpsShipmentEntity ups, XmlTextWriter xmlWriter)
        {
            bool isSurePost = UpsUtility.IsUpsSurePostService((UpsServiceType)ups.Service);
            bool isMailInnovations = UpsUtility.IsUpsMiService((UpsServiceType)ups.Service);
            bool isMilitarySurePost = isSurePost && PostalUtility.IsMilitaryState(ups.Shipment.ShipStateProvCode);

            // Has to be internation or Military SurePost
            if (ShipmentTypeManager.GetType(ups.Shipment).IsDomestic(ups.Shipment) && !isMilitarySurePost)
            {
                return;
            }

            if (!(isSurePost || isMailInnovations) && ups.CommercialPaperlessInvoice)
            {
                xmlWriter.WriteStartElement("InternationalForms");
                xmlWriter.WriteElementString("FormType", "01");

                if (ups.PaperlessAdditionalDocumentation)
                {
                    xmlWriter.WriteElementString("AdditionalDocumentIndicator", string.Empty);
                }

                // Go through each customs product
                foreach (ShipmentCustomsItemEntity product in ups.Shipment.CustomsItems)
                {
                    xmlWriter.WriteStartElement("Product");
                    xmlWriter.WriteElementString("Description", product.Description);

                    xmlWriter.WriteStartElement("Unit");
                    xmlWriter.WriteElementString("Number", ((int) product.Quantity).ToString());
                    xmlWriter.WriteElementString("Value", product.UnitValue.ToString());
                    xmlWriter.WriteStartElement("UnitOfMeasurement");
                    xmlWriter.WriteElementString("Code", "EA");
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndElement();

                    if (!string.IsNullOrEmpty(product.HarmonizedCode))
                    {
                        xmlWriter.WriteElementString("CommodityCode", product.HarmonizedCode);
                    }

                    xmlWriter.WriteElementString("OriginCountryCode", product.CountryOfOrigin);

                    xmlWriter.WriteEndElement();
                }

                // Invoice stuff
                xmlWriter.WriteElementString("InvoiceNumber", ups.Shipment.Order.OrderNumberComplete);
                xmlWriter.WriteElementString("InvoiceDate", ups.Shipment.ShipDate.ToString("yyyyMMdd"));

                UpsTermsOfSale termsOfShipment = (UpsTermsOfSale) ups.CommercialInvoiceTermsOfSale;
                UpsExportReason exportReason = (UpsExportReason) ups.CommercialInvoicePurpose;

                // TermsOfShipment
                if (termsOfShipment != UpsTermsOfSale.NotSpecified)
                {
                    xmlWriter.WriteElementString("TermsOfShipment", GetTermsOfShipmentApiCode(termsOfShipment));
                }

                // ReasonForExport
                xmlWriter.WriteElementString("ReasonForExport", GetReasonForExportApiCode(exportReason));

                // Comments
                if (!string.IsNullOrEmpty(ups.CommercialInvoiceComments))
                {
                    xmlWriter.WriteElementString("Comments", ups.CommercialInvoiceComments);
                }

                // Freight charges
                if (ups.CommercialInvoiceFreight != 0)
                {
                    xmlWriter.WriteStartElement("FreightCharges");
                    xmlWriter.WriteElementString("MonetaryValue", ups.CommercialInvoiceFreight.ToString("0.00"));
                    xmlWriter.WriteEndElement();
                }

                // Insurance charges
                if (ups.CommercialInvoiceInsurance != 0)
                {
                    xmlWriter.WriteStartElement("InsuranceCharges");
                    xmlWriter.WriteElementString("MonetaryValue", ups.CommercialInvoiceInsurance.ToString("0.00"));
                    xmlWriter.WriteEndElement();
                }

                // Other charges
                if (ups.CommercialInvoiceOther != 0)
                {
                    xmlWriter.WriteStartElement("OtherCharges");
                    xmlWriter.WriteElementString("MonetaryValue", ups.CommercialInvoiceOther.ToString("0.00"));
                    xmlWriter.WriteElementString("Description", "Other");
                    xmlWriter.WriteEndElement();
                }

                xmlWriter.WriteElementString("CurrencyCode", UpsUtility.GetCurrency(account));
                xmlWriter.WriteEndElement();
            }
            else if (isSurePost || isMailInnovations)
            {
                // Write any Postal CN22 info needed
                WritePostalCN22(ups, xmlWriter);   
            }
        }

        /// <summary>
        /// Writes the appropriate SurePost shipment xml
        /// </summary>
        private static void WriteSurePostShipment(UpsShipmentEntity ups, XmlTextWriter xmlWriter)
        {
            UpsServiceType serviceType = (UpsServiceType) ups.Service;
            // Only write the fields if the service is a SurePost service
            if (UpsUtility.IsUpsSurePostService(serviceType))
            {
                // Start SurePostShipment
                xmlWriter.WriteStartElement("SurePostShipment");

                if (serviceType == UpsServiceType.UpsSurePostLessThan1Lb)
                {
                    // If it's SurePost less than a pound, set the sub class
                    xmlWriter.WriteElementString("SubClassification", EnumHelper.GetApiValue((UpsPostalSubclassificationType)ups.Subclassification));
                }

                // The documentation didn't give us a value for Carrier Leave if No Response.  It only says that if no endorsement is specified, 
                // UPS will default to Carrier Leave if No Response.  So only write the endorsement if CarrierLeaveIfNoResponse is not selected.
                if ((UspsEndorsementType)ups.Endorsement != UspsEndorsementType.CarrierLeaveIfNoResponse)
                {
                    xmlWriter.WriteElementString("USPSEndorsement", EnumHelper.GetApiValue((UspsEndorsementType) ups.Endorsement));
                }

                // Close SurePostShipment
                xmlWriter.WriteEndElement();
            }
        }

        /// <summary>
        /// Writes the MI xml values
        /// </summary>
        private static void WriteMiShipmentValues(UpsShipmentEntity ups, XmlTextWriter xmlWriter)
        {
            UpsServiceType serviceType = (UpsServiceType)ups.Service;

            // Only write the fields if the service is an MI service
            if (UpsUtility.IsUpsMiService(serviceType))
            {
				// No Service Selected is the only valid value for international.
                UspsEndorsementType uspsEndorsementType = (UspsEndorsementType)ups.Endorsement;
                if (serviceType == UpsServiceType.UpsMailInnovationsIntEconomy ||
                    serviceType == UpsServiceType.UpsMailInnovationsIntPriority)
                {
                    uspsEndorsementType = UspsEndorsementType.NoServiceSelected;
                }

                xmlWriter.WriteElementString("USPSEndorsement", EnumHelper.GetApiValue(uspsEndorsementType));
                
                xmlWriter.WriteElementString("SubClassification", EnumHelper.GetApiValue((UpsPostalSubclassificationType)ups.Subclassification));
                
                xmlWriter.WriteElementString("IrregularIndicator", EnumHelper.GetApiValue((UpsIrregularIndicatorType)ups.IrregularIndicator));

                // Blanks are not allowed
                string costCenter = UpsApiCore.ProcessUspsTokenField(ups.CostCenter, ups.ShipmentID, string.Empty);
                xmlWriter.WriteElementString("CostCenter", costCenter.Replace(" ", string.Empty));

                string packageID = UpsApiCore.ProcessUspsTokenField(ups.UspsPackageID, ups.ShipmentID, string.Empty);
                xmlWriter.WriteElementString("PackageID", packageID);

                // If an international shipment, write out the MILabelCN22Indicator so that the label will be the combined label with CN22 form
                if (!ShipmentTypeManager.GetType(ups.Shipment).IsDomestic(ups.Shipment))
                {
                    xmlWriter.WriteElementString("MILabelCN22Indicator", string.Empty);
                }
            }
        }

        /// <summary>
        /// Writes the appropriate Postal CN22 xml
        /// </summary>
        private static void WritePostalCN22(UpsShipmentEntity ups, XmlTextWriter xmlWriter)
        {
            UpsServiceType serviceType = (UpsServiceType)ups.Service;

            if (!ups.Shipment.CustomsItemsLoaded)
            {
                CustomsManager.LoadCustomsItems(ups.Shipment, false);
            }

            if (!ups.Shipment.CustomsItems.Any())
            {
                return;
            }
            // Only write the fields if the service is a SurePost service
            if (!UpsUtility.IsUpsMiOrSurePostService(serviceType))
            {
                return;
            }

            xmlWriter.WriteStartElement("InternationalForms");
            xmlWriter.WriteElementString("FormType", "09");
            xmlWriter.WriteElementString("FormGroupIdName", "CN22 Form");

            string otherDescription = string.Empty;

            // Only 3 are allowed
            foreach (var shipmentCustomsItem in ups.Shipment.CustomsItems.Take(3))
            {
                // Start Product
                xmlWriter.WriteStartElement("Product");

                string customsItemDescription = Regex.Replace(shipmentCustomsItem.Description, "[^A-Za-z0-9 _]", string.Empty);
                customsItemDescription = customsItemDescription.Length < 35 ? customsItemDescription : customsItemDescription.Substring(0, 35);

                xmlWriter.WriteElementString("Description", customsItemDescription);
                xmlWriter.WriteEndElement();

                otherDescription += string.Format("{0} ", shipmentCustomsItem.Description);
            }

            // Start CN22Form
            xmlWriter.WriteStartElement("CN22Form");
                
            // 6 = 4x6 
            // 1 = 8.5x11
            xmlWriter.WriteElementString("LabelSize", "6");

            // Docs say only 1 per page is supported
            xmlWriter.WriteElementString("PrintsPerPage", "1");

            // Valid values are pdf,png,gif,zpl,star,epl2 and spl
            if (ups.Shipment.RequestedLabelFormat != (int) ThermalLanguage.None)
            {
                xmlWriter.WriteElementString("LabelPrintType", ups.Shipment.RequestedLabelFormat == (int) ThermalLanguage.EPL ? "EPL2" : "ZPL");
            }
            else
            {
                xmlWriter.WriteElementString("LabelPrintType", "gif");   
            }

            UpsCN22GoodsType cn22Type = ups.CustomsDocumentsOnly ? UpsCN22GoodsType.Documents : UpsCN22GoodsType.Other;
            xmlWriter.WriteElementString("CN22Type", EnumHelper.GetApiValue(cn22Type));

            // If CN22 Type is Other, CN22OtherDescription is required
            if (cn22Type == UpsCN22GoodsType.Other)
            {
                // CN22OtherDescription can only be at most 20 characters
                xmlWriter.WriteElementString("CN22OtherDescription", otherDescription.Length > 20 ? otherDescription.Substring(0, 20) : otherDescription);
            }

            // UPS only allows 1 customs item for MI shipments
            int maximumAllowedCustomsItems = UpsUtility.IsUpsMiService(serviceType) ? 1 : 3;
            
            foreach (var shipmentCustomsItem in ups.Shipment.CustomsItems.Take(maximumAllowedCustomsItems))
            {
                // Start InternationalForms
                xmlWriter.WriteStartElement("CN22Content");

                xmlWriter.WriteElementString("CN22ContentQuantity", shipmentCustomsItem.Quantity.ToString());
                xmlWriter.WriteElementString("CN22ContentDescription", shipmentCustomsItem.Description);

                // Start CN22ContentWeight
                xmlWriter.WriteStartElement("CN22ContentWeight");

                // Start UnitOfMeasurement
                xmlWriter.WriteStartElement("UnitOfMeasurement");


                double weight = shipmentCustomsItem.Weight;

                // Get the settings for this shipment/package type so we can determine weight unit of measure and declared value setting
                // Before calling this method, the shipment/package type should be validated so that a setting is always found if we make it this far.
                UpsServicePackageTypeSetting upsSetting = UpsServicePackageTypeSetting.ServicePackageValidationSettings.First(s => s.ServiceType == (UpsServiceType)ups.Service &&
                                                            s.PackageType == (UpsPackagingType)ups.Packages[0].PackagingType);

                weight = WeightUtility.Convert(WeightUnitOfMeasure.Pounds, upsSetting.WeightUnitOfMeasure, weight);

                // Weight unit of measure values are lbs and ozs
                xmlWriter.WriteElementString("Code", upsSetting.WeightUnitOfMeasure == WeightUnitOfMeasure.Pounds ? "LBS" : "OZS");
                    
                // Close UnitOfMeasurement
                xmlWriter.WriteEndElement();

                xmlWriter.WriteElementString("Weight", upsSetting.WeightUnitOfMeasure == WeightUnitOfMeasure.Pounds ? weight.ToString("N2") : weight.ToString("N1"));

                // Close CN22ContentWeight
                xmlWriter.WriteEndElement();

                decimal totalValue = shipmentCustomsItem.UnitValue * (decimal)shipmentCustomsItem.Quantity;
                xmlWriter.WriteElementString("CN22ContentTotalValue", totalValue.ToString("N2"));

                // Currently only USD is supported
                xmlWriter.WriteElementString("CN22ContentCurrencyCode", "USD");

                // Close CN22Content
                xmlWriter.WriteEndElement();
            }
            
            // Close CN22Form
            xmlWriter.WriteEndElement();

            // Close InternationalForms
            xmlWriter.WriteEndElement();
        }

        /// <summary>
        /// Get the API code that corresponds to the given reason
        /// </summary>
        private static string GetReasonForExportApiCode(UpsExportReason exportReason)
        {
            switch (exportReason)
            {
                case UpsExportReason.Sale: return "SALE";
                case UpsExportReason.Gift: return "GIFT";
                case UpsExportReason.Sample: return "SAMPLE";
                case UpsExportReason.Return: return "RETURN";
                case UpsExportReason.Repair: return "REPAIR";
                case UpsExportReason.InterCompanyData: return "INTERCOMPANYDATA";
            }

            throw new InvalidOperationException("Invalid UPS export reason: " + exportReason);
        }

        /// <summary>
        /// Get the API code to use for the given terms of shipment
        /// </summary>
        public static string GetTermsOfShipmentApiCode(UpsTermsOfSale termsOfShipment)
        {
            switch (termsOfShipment)
            {
                case UpsTermsOfSale.NotSpecified: return "";
                case UpsTermsOfSale.CostFreight: return "CFR";
                case UpsTermsOfSale.CostInsuranceFreight: return "CIF";
                case UpsTermsOfSale.CarriageInsurancePaid: return "CIP";
                case UpsTermsOfSale.CarriagePaidTo: return "CPT";
                case UpsTermsOfSale.DeliveredAtFrontier: return "DAF";
                case UpsTermsOfSale.DeliveryDutyPaid: return "DDP";
                case UpsTermsOfSale.DeliveryDutyUnpaid: return "DDU";
                case UpsTermsOfSale.DeliveredExQuay: return "DEQ";
                case UpsTermsOfSale.DeliveredExShip: return "DES";
                case UpsTermsOfSale.ExWorks: return "EXW";
                case UpsTermsOfSale.FreeAlongsideShip: return "FAS";
                case UpsTermsOfSale.FreeCarrier: return "FCA";
                case UpsTermsOfSale.FreeOnBoard: return "FOB";
            }

            throw new InvalidOperationException("Invalid UPS terms of shipment: " + termsOfShipment);
        }

        /// <summary>
        /// Appends the ShipConfirm Email Notification stuff into the XML stream
        /// </summary>
        private static void WriteEmailNotificationXml(UpsShipmentEntity ups, XmlTextWriter xmlWriter)
        {
            List<string> shipRecipients = new List<string>();
            List<string> exceptRecipients = new List<string>();
            List<string> deliverRecipients = new List<string>();

            AddEmailToNotificationLists(ups.EmailNotifySender, ups.Shipment.OriginEmail, shipRecipients, exceptRecipients, deliverRecipients);
            AddEmailToNotificationLists(ups.EmailNotifyRecipient, ups.Shipment.ShipEmail, shipRecipients, exceptRecipients, deliverRecipients);
            AddEmailToNotificationLists(ups.EmailNotifyOther, ups.EmailNotifyOtherAddress, shipRecipients, exceptRecipients, deliverRecipients);

            shipRecipients = shipRecipients.Where(r => r.Trim().Length > 0).Distinct().ToList();
            exceptRecipients = exceptRecipients.Where(r => r.Trim().Length > 0).Distinct().ToList();
            deliverRecipients = deliverRecipients.Where(r => r.Trim().Length > 0).Distinct().ToList();

            bool headerWritten = false;

            // Process each notification type
            AppendEmailNotificationTypeXml(ups, xmlWriter, shipRecipients, "6", ref headerWritten); // '6' is the UPS code for Ship Notify
            AppendEmailNotificationTypeXml(ups, xmlWriter, exceptRecipients, "7", ref headerWritten); // '7' is the UPS code for Exception Notify
            AppendEmailNotificationTypeXml(ups, xmlWriter, deliverRecipients, "8", ref headerWritten); // '8' is the UPS code for Delivery Notify
        }

        /// <summary>
        /// Add the given email address to the given lists depending on the value of notifyTypes
        /// </summary>
        private static void AddEmailToNotificationLists(int notifyTypes, string email, List<string> shipRecipients, List<string> exceptRecipients, List<string> deliverRecipients)
        {
            email = UpsUtility.GetCorrectedEmailAddress(email);

            if ((notifyTypes & (int) UpsEmailNotificationType.Ship) != 0)
            {
                shipRecipients.Add(email);
            }

            if ((notifyTypes & (int) UpsEmailNotificationType.Exception) != 0)
            {
                exceptRecipients.Add(email);
            }

            if ((notifyTypes & (int) UpsEmailNotificationType.Deliver) != 0)
            {
                deliverRecipients.Add(email);
            }
        }

        /// <summary>
        /// Add all the recipients of a particular notification type to the XML 
        /// </summary>
        private static void AppendEmailNotificationTypeXml(UpsShipmentEntity ups, XmlTextWriter xmlWriter, List<string> recipients, string upsCode, ref bool headerWritten)
        {
            // If there are none, just get out
            if (recipients.Count == 0)
            {
                return;
            }

            // Begin
            xmlWriter.WriteStartElement("Notification");
            xmlWriter.WriteElementString("NotificationCode", upsCode);

            // Message container
            xmlWriter.WriteStartElement("EMailMessage");

            // Add in each recipient
            foreach (string recipient in recipients)
            {
                xmlWriter.WriteElementString("EMailAddress", UpsUtility.GetCorrectedEmailAddress(recipient));
            }

            if (!headerWritten)
            {
                xmlWriter.WriteElementString("UndeliverableEMailAddress", UpsUtility.GetCorrectedEmailAddress(ups.Shipment.OriginEmail));
                xmlWriter.WriteElementString("FromName", UpsUtility.GetCorrectedEmailAddress(ups.EmailNotifyFrom));
                xmlWriter.WriteElementString("Memo", ups.EmailNotifyMessage);
                xmlWriter.WriteElementString("SubjectCode", (ups.EmailNotifySubject == (int) UpsEmailNotificationSubject.ReferenceNumber) ? "01" : "");

                headerWritten = true;
            }

            // Close message container
            xmlWriter.WriteEndElement();

            // End
            xmlWriter.WriteEndElement();
        }

        /// <summary>
        /// Process the accept phase of the ship request
        /// </summary>
        private static void ProcessShipAccept(ShipmentEntity shipment, XPathNavigator shipConfirmNavigator)
        {
            UpsAccountEntity account = UpsApiCore.GetUpsAccount(shipment, new UpsAccountRepository());

            // Create the client for connecting to the UPS server
            XmlTextWriter xmlWriter = UpsWebClient.CreateRequest(UpsOnLineToolType.ShipAccept, account);
            string shipmentDigest = XPathUtility.Evaluate(shipConfirmNavigator, "//ShipmentDigest", "");
            xmlWriter.WriteElementString("ShipmentDigest", shipmentDigest);

            XmlDocument acceptResponse = UpsWebClient.ProcessRequest(xmlWriter);

            // Create the XPath engine
            XPathNavigator xpath = acceptResponse.CreateNavigator();

            // Update shipment info
            shipment.ShipmentCost = XPathUtility.Evaluate(xpath, "//TotalCharges/MonetaryValue", 0m);
            shipment.TrackingNumber = XPathUtility.Evaluate(xpath, "//ShipmentIdentificationNumber", "");

            shipment.Ups.PublishedCharges = shipment.ShipmentCost;

            // If the shipper is configured for Negotiated Rates, look for returned special rate
            if (account.RateType == (int) UpsRateType.Negotiated)
            {
                decimal specialRate = XPathUtility.Evaluate(xpath, "//NegotiatedRates/NetSummaryCharges/GrandTotal/MonetaryValue", -1M);
                if (specialRate >= 0)
                {
                    // mark that this was a negotiated rate
                    shipment.Ups.NegotiatedRate = true;
                    shipment.ShipmentCost = specialRate;
                }
            }

            // Set the billing weight and type
            double billedWeight = XPathUtility.Evaluate(shipConfirmNavigator, "//BillingWeight/Weight", shipment.TotalWeight);
            shipment.BilledWeight = billedWeight;
            if (shipment.BilledWeight > shipment.TotalWeight)
            {
                shipment.BilledType = (int) BilledType.DimensionalWeight;
            }
            else
            {
                shipment.BilledType = (int)BilledType.ActualWeight; 
            }

            // Get all the packages for the shipment
            int packageIndex = 0;

            // Get all the package results nodes
            XPathNodeIterator packageNodes = xpath.Select("//PackageResults");

            // Should be exactly the same
            Debug.Assert(packageNodes.Count == shipment.Ups.Packages.Count);

            // Go through each one
            while (packageNodes.MoveNext() && packageIndex < shipment.Ups.Packages.Count)
            {
                // Get the navigator for this package
                XPathNavigator packageNode = packageNodes.Current.Clone();

                UpsPackageEntity currentPackage = shipment.Ups.Packages[packageIndex];

                // Extract the package data
                currentPackage.TrackingNumber = XPathUtility.Evaluate(packageNode, "TrackingNumber", "").Trim();

                if (UpsUtility.IsUpsMiService((UpsServiceType) shipment.Ups.Service))
                {
                    currentPackage.UspsTrackingNumber = XPathUtility.Evaluate(packageNode, "USPSPICNumber", "").Trim();
                    shipment.Ups.UspsTrackingNumber = currentPackage.UspsTrackingNumber;
                    shipment.Ups.Cn22Number = XPathUtility.Evaluate(packageNode, "CN22Number", "");

                    // If we have a CN22Number, use it for the shipment and package UspsTrackingNumber (if the shipment/package UspsTrackingNumber is blank)
                    if (!string.IsNullOrWhiteSpace(shipment.Ups.Cn22Number))
                    {
                        if (string.IsNullOrWhiteSpace(shipment.Ups.UspsTrackingNumber))
                        {
                            shipment.Ups.UspsTrackingNumber = shipment.Ups.Cn22Number;
                        }

                        if (string.IsNullOrWhiteSpace(currentPackage.UspsTrackingNumber))
                        {
                            currentPackage.UspsTrackingNumber = shipment.Ups.Cn22Number;
                        }
                    }

                    // Now that the USPS tracking number should be correct, we need to update the 
                    // main tracking number to be the USPS tracking number.
                    // From Bryan Cazan @ UPS:
                    //   For domestic MI tracking, please use the USPSPICNumber as the tracking number that is passed on to marketplaces. 
                    //   Please ignore the 8000 number given. That number will cause issues with tracking for customers. 
                    if (!string.IsNullOrWhiteSpace(currentPackage.UspsTrackingNumber))
                    {
                        currentPackage.TrackingNumber = currentPackage.UspsTrackingNumber;
                        shipment.TrackingNumber = currentPackage.TrackingNumber;
                    }
                }

                if (shipment.ReturnShipment && !UpsUtility.ReturnServiceHasLabels((UpsReturnServiceType)shipment.Ups.ReturnService))
                {
                    // no labels
                }
                else
                {
                    // Create all the images
                    CreateLabelImages(packageNode, currentPackage, (ThermalLanguage?) shipment.ActualLabelFormat);
                }

                // Next package
                packageIndex++;
            }

            SaveCN22Label(xpath, shipment);
        }

        /// <summary>
        /// Saves the CN22 label.
        /// </summary>
        /// <param name="upsResponse">The ups response.</param>
        /// <param name="shipment">The shipment.</param>
        private static void SaveCN22Label(XPathNavigator upsResponse, ShipmentEntity shipment)
        {

            if (XPathUtility.Evaluate(upsResponse, "/ShipmentAcceptResponse/ShipmentResults/Form/FormGroupIdName", "") != "CN22 Form")
            {
                return;
            }

            string cn22Label = XPathUtility.Evaluate(upsResponse, "/ShipmentAcceptResponse/ShipmentResults/Form/Image/GraphicImage", "");

            if (string.IsNullOrWhiteSpace(cn22Label))
            {
                return;
            }

            using (MemoryStream stream = new MemoryStream(Convert.FromBase64String(cn22Label)))
            {
                DataResourceManager.CreateFromBytes(stream.ToArray(), shipment.Ups.Packages.First().UpsPackageID, "Customs");
            }
        }

        /// <summary>
        /// Create the label images for the given package
        /// </summary>
        private static void CreateLabelImages(XPathNavigator packageNode, UpsPackageEntity package, ThermalLanguage? labelType)
        {
            string labelBase64 = XPathUtility.Evaluate(packageNode, "LabelImage/GraphicImage", "");

            // Save the label iamges
            using (SqlAdapter adapter = new SqlAdapter())
            {
                // If we had saved an image for this package previously, but the shipment errored out later (like for an MPS), then clear before
                // we start.
                ObjectReferenceManager.ClearReferences(package.UpsPackageID);

                using (MemoryStream stream = new MemoryStream(Convert.FromBase64String(labelBase64)))
                {
                    if (labelType == null)
                    {
                        using (Image imageLabel = Image.FromStream(stream))
                        {
                            stream.Position = 0;

                            imageLabel.RotateFlip(RotateFlipType.Rotate90FlipNone);

                            using (Image imageCrop = DisplayHelper.CropImage(imageLabel, 0, 0, 800, 1201))
                            {
                                using (MemoryStream imageStream = new MemoryStream())
                                {
                                    imageCrop.Save(imageStream, ImageFormat.Gif);

                                    DataResourceManager.CreateFromBytes(imageStream.ToArray(), package.UpsPackageID, "LabelImage");

                                    // imageCrop.Save(, ImageFormat.Gif);
                                }
                            }
                        }
                    }
                    else
                    {
                        DataResourceManager.CreateFromBytes(stream.ToArray(), package.UpsPackageID, "LabelImage");
                    }
                }
            }
        }
    }
}
