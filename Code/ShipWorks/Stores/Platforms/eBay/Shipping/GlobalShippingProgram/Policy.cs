using System.Collections.Generic;
using System.Linq;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Stores.Platforms.Ebay.Shipping.GlobalShippingProgram.Rules;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using System;
using ShipWorks.Data.Model;

namespace ShipWorks.Stores.Platforms.Ebay.Shipping.GlobalShippingProgram
{
    /// <summary>
    /// Encapsulates the rules for determining whether an eBay order is part of the Global Shipping Program
    /// and should be shipped to the domestic facility along with how to configure a shipment to be shipped
    /// to the GSP domestic facility rather than directly to the buyer.
    /// </summary>
    public class Policy
    {
        private List<IGlobalShippingProgramRule> ruleSet;

        /// <summary>
        /// Initializes a new instance of the <see cref="Policy"/> class with a default set of rules.
        /// </summary>
        public Policy()
        {
            ruleSet = new List<IGlobalShippingProgramRule>();

            ruleSet.Add(new EligibilityRule());
            ruleSet.Add(new SelectedShippingMethodRule());
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Policy"/> class.
        /// </summary>
        /// <param name="ruleSet">The rule set.</param>
        public Policy(IEnumerable<IGlobalShippingProgramRule> ruleSet)
        {
            this.ruleSet = new List<IGlobalShippingProgramRule>(ruleSet);
        }

        /// <summary>
        /// Gets the rules for the policy.
        /// </summary>
        public IEnumerable<IGlobalShippingProgramRule> Rules
        {
            get { return ruleSet; }
        }

        /// <summary>
        /// Determines whether [the specified shipment] [is eligible for global shipping program].
        /// </summary>
        /// <param name="ebayOrder">The order.</param>
        /// <returns>
        ///   <c>true</c> if [the specified shipment] [is eligible for global shipping program]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsEligibleForGlobalShippingProgram(EbayOrderEntity ebayOrder)
        {
            bool isEligible = false;

            if (ebayOrder != null)
            {
                // All the rules must be satisfied for the order to be eligible for the program
                isEligible = ruleSet.All(r => r.Evaluate(ebayOrder));
            }

            return isEligible;
        }

        /// <summary>
        /// Configures the shipment for eBay's global shipping program by modifying the shipping address to
        /// be that of the domestic facility rather than the buyer.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <param name="ebayOrder">The ebay order.</param>
        /// <returns>A List of the ShipmentFieldIndex values denoting the fields that were changed.</returns>
        public List<ShipmentFieldIndex> ConfigureShipmentForGlobalShippingProgram(ShipmentEntity shipment, EbayOrderEntity ebayOrder)
        {
            // Note: according to eBay, the facility address is purposely incomplete and omits
            // Suite 400 in the sandbox environment as an attempt to prevent inadvertant shipments 
            // for sandbox orders. As of 9/6/2012, the full address in production should be:
            // Reference# (which is mocked on sandbox)
            // 1850 Airport Exchange Blvd
            // Suite 400
            // Erlanger KY 41018
            // United States

            List<ShipmentFieldIndex> modifiedFieldList = new List<ShipmentFieldIndex>();

            if (shipment != null && IsEligibleForGlobalShippingProgram(ebayOrder))
            {
                // This is a GSP order, so we'll swap out the address of the shipment so it is the 
                // address of the facility

                // There is not a company name field in the eBay shipping address for a GSP order, 
                // so we'll set the ShipCompany property as the reference ID since it must be immediately 
                // above the address. We can't use the name fields because a few of the carriers require 
                // a recipient name.
                SetShipmentFieldValue(modifiedFieldList, shipment, ShipmentFieldIndex.ShipCompany, ebayOrder.GspReferenceID);

                modifiedFieldList.AddRange(ConfigureRecipientName(shipment, ebayOrder, ebayOrder.GspReferenceID));
                modifiedFieldList.AddRange(ConfigureShippingAddress(shipment, ebayOrder));
                modifiedFieldList.AddRange(ConfigureContactInfo(shipment));

                if (shipment.Postal != null && shipment.Postal.Usps != null)
                {
                    // We don't want USPS to perform address validation on this shipment
                    shipment.Postal.Usps.RequireFullAddressValidation = false;                    
                }

                // GSP shipments will always be commercial and we need to set the residential result 
                // to false otherwise we encounter errors with shipping carriers. We don't add this 
                // to the modified field list because we want this to be permanent
                shipment.ResidentialDetermination = (int)ResidentialDeterminationType.Commercial;
                shipment.ResidentialResult = false;
            }
            
            return modifiedFieldList;
        }

        /// <summary>
        /// A helper method that sets the shipment field value and updates the list of changed fields to denote the change.
        /// </summary>
        /// <param name="changedFields">The list of ShipmentFieldIndex values to use to track which field has changed.</param>
        /// <param name="shipment">The shipment.</param>
        /// <param name="fieldIndex">Index of the field to change the value of.</param>
        /// <param name="value">The value.</param>
        private static void SetShipmentFieldValue(List<ShipmentFieldIndex> changedFields, ShipmentEntity shipment, ShipmentFieldIndex fieldIndex, object value)
        {
            shipment.SetNewFieldValue((int)fieldIndex, value);

            if (changedFields != null && !changedFields.Contains(fieldIndex))
            {
                // Only tracking the fields that changed, not the number of times it changes, so only 
                // add the field index if it hasn't already been noted as being changed.
                changedFields.Add(fieldIndex);
            }
        }

        /// <summary>
        /// Configures the name of the recipient.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <param name="ebayOrder">The ebay order.</param>
        /// <param name="gspReferenceID"></param>
        /// <returns>A List of the ShipmentFieldIndex values denoting the fields that were changed.</returns>
        private static IEnumerable<ShipmentFieldIndex> ConfigureRecipientName(ShipmentEntity shipment, EbayOrderEntity ebayOrder, string gspReferenceID)
        {
            List<ShipmentFieldIndex> modifiedFieldList = new List<ShipmentFieldIndex>();

            if (shipment.ShipmentType == (int)ShipmentTypeCode.Endicia || shipment.ShipmentType == (int)ShipmentTypeCode.Express1Endicia)
            {
                // These carriers place the company name field above the recipient's name, so we're going to 
                // set the name to empty. (This is okay according to a call with eBay on 8/30/2012)
                SetShipmentFieldValue(modifiedFieldList, shipment, ShipmentFieldIndex.ShipFirstName, string.Empty);
                SetShipmentFieldValue(modifiedFieldList, shipment, ShipmentFieldIndex.ShipMiddleName, string.Empty);
                SetShipmentFieldValue(modifiedFieldList, shipment, ShipmentFieldIndex.ShipLastName, string.Empty);
            }
            else if (shipment.ShipmentType == (int) ShipmentTypeCode.UpsOnLineTools && UpsUtility.IsUpsSurePostService((UpsServiceType)shipment.Ups.Service))
            {
                // UPS SurePost does not have a company name, so we'll need to use the name field to hold the GSP reference id
                SetShipmentFieldValue(modifiedFieldList, shipment, ShipmentFieldIndex.ShipFirstName, string.Empty);
                SetShipmentFieldValue(modifiedFieldList, shipment, ShipmentFieldIndex.ShipMiddleName, string.Empty);
                SetShipmentFieldValue(modifiedFieldList, shipment, ShipmentFieldIndex.ShipLastName, gspReferenceID);
            }
            else
            {
                // The layout of other carriers have the company name below the recipient's name, so
                // we can set the first and last name based on the GSP address info. 

                // Note: Postal WebTools and USPS require a recipient name otherwise we could 
                // just set the name to an empty string for all carriers
                SetShipmentFieldValue(modifiedFieldList, shipment, ShipmentFieldIndex.ShipFirstName, ebayOrder.GspFirstName);
                SetShipmentFieldValue(modifiedFieldList, shipment, ShipmentFieldIndex.ShipMiddleName, string.Empty);
                SetShipmentFieldValue(modifiedFieldList, shipment, ShipmentFieldIndex.ShipLastName, ebayOrder.GspLastName);
            }

            return modifiedFieldList;
        }

        /// <summary>
        /// Configures the shipping address.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <param name="ebayOrder">The ebay order.</param>
        /// <returns>A List of the ShipmentFieldIndex values denoting the fields that were changed.</returns>
        private List<ShipmentFieldIndex> ConfigureShippingAddress(ShipmentEntity shipment, EbayOrderEntity ebayOrder)
        {
            List<ShipmentFieldIndex> modifiedFieldList = new List<ShipmentFieldIndex>();

            // We only have two lines for the street address for the GSP facility, so make sure the 
            // third line is an empty string
            SetShipmentFieldValue(modifiedFieldList, shipment, ShipmentFieldIndex.ShipStreet1, ebayOrder.GspStreet1);
            SetShipmentFieldValue(modifiedFieldList, shipment, ShipmentFieldIndex.ShipStreet2, ebayOrder.GspStreet2);
            SetShipmentFieldValue(modifiedFieldList, shipment, ShipmentFieldIndex.ShipStreet3, string.Empty);

            SetShipmentFieldValue(modifiedFieldList, shipment, ShipmentFieldIndex.ShipCity, ebayOrder.GspCity);
            SetShipmentFieldValue(modifiedFieldList, shipment, ShipmentFieldIndex.ShipStateProvCode, ebayOrder.GspStateProvince);
            
            // Only grab the first 5 digits of the postal code (there was a problem where eBay was started sending invalid 9 digit 
            // postal codes such as 41018-319). This is added here in addition to the downloader, so customers that have already 
            // downloaded orders with the invalid postal code can still ship their orders without having to manually update the data.
            SetShipmentFieldValue(modifiedFieldList, shipment, ShipmentFieldIndex.ShipPostalCode, ebayOrder.GspPostalCode.Substring(0, 5));
            SetShipmentFieldValue(modifiedFieldList, shipment, ShipmentFieldIndex.ShipCountryCode, ebayOrder.GspCountryCode);


            return modifiedFieldList;
        }

        /// <summary>
        /// Configures the contact info for a GSP shipment by removing the contact information.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>A List of the ShipmentFieldIndex values denoting the fields that were changed.</returns>
        private List<ShipmentFieldIndex> ConfigureContactInfo(ShipmentEntity shipment)
        {
            List<ShipmentFieldIndex> modifiedFieldList = new List<ShipmentFieldIndex>();

            // Remove any contact information about the recipient
            SetShipmentFieldValue(modifiedFieldList, shipment, ShipmentFieldIndex.ShipEmail, string.Empty);

            if (shipment.ShipmentType != (int)ShipmentTypeCode.FedEx)
            {
                // Fedex requires a phone number when printing labels, so only wipe the phone 
                // number if it the carrier is not FedEx
                SetShipmentFieldValue(modifiedFieldList, shipment, ShipmentFieldIndex.ShipPhone, string.Empty);
            }

            return modifiedFieldList;
        }
    }
}
