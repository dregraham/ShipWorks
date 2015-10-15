using System;
using System.Collections.Generic;
using Interapptive.Shared.Business;
using ShipWorks.AddressValidation.Enums;

namespace ShipWorks.AddressValidation
{
    /// <summary>
    /// Address data as returned from the call to the validation service
    /// </summary>
    public class AddressValidationResult : IAddressAdapter
    {
        /// <summary>
        /// Creates a new AddressValidationResult
        /// </summary>
        public AddressValidationResult()
        {
            // Default all the fields to empty strings instead of nulls to
            // reduce the amount of null checks before copying
            Street1 = string.Empty;
            Street2 = string.Empty;
            Street3 = string.Empty;
            City = string.Empty;
            StateProvCode = string.Empty;
            PostalCode = string.Empty;
            CountryCode = string.Empty;
            ResidentialStatus = ValidationDetailStatusType.Unknown;
            POBox = ValidationDetailStatusType.Unknown;
            AddressType = AddressType.NotChecked;
        }

        /// <summary>
        /// Street 1 of the address
        /// </summary>
        public string Street1 { get; set; }

        /// <summary>
        /// Street 2 of the address
        /// </summary>
        public string Street2 { get; set; }

        /// <summary>
        /// Street 3 of the address
        /// </summary>
        public string Street3 { get; set; }

        /// <summary>
        /// City of the address
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// State or province code of the address
        /// </summary>
        public string StateProvCode { get; set; }

        /// <summary>
        /// Postal code of the address
        /// </summary>
        public string PostalCode { get; set; }

        /// <summary>
        /// Country code of the address
        /// </summary>
        public string CountryCode { get; set; }

        /// <summary>
        /// Whether the address is residential or commercial
        /// </summary>
        public ValidationDetailStatusType ResidentialStatus { get; set; }

        /// <summary>
        /// Whether the address is a PO Box
        /// </summary>
        public ValidationDetailStatusType POBox { get; set; }

        /// <summary>
        /// Address type, the type of address
        /// </summary>
        public AddressType AddressType { get; set; }

        /// <summary>
        /// Defines whether an address is valid and usable
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// Checks whether this result is equal to the address in the adapter
        /// </summary>
        public bool IsEqualTo(AddressAdapter adapter)
        {
            if (adapter == null)
            {
                return false;
            }

            return
                Street1 == adapter.Street1 &&
                Street2 == adapter.Street2 &&
                Street3 == adapter.Street3 &&
                City == adapter.City &&
                StateProvCode == adapter.StateProvCode &&
                PostalCode == adapter.PostalCode &&
                CountryCode == adapter.CountryCode;
        }

        /// <summary>
        /// Copies the address of this result into the specified adapter
        /// </summary>
        public void CopyTo(AddressAdapter adapter)
        {
            if (adapter == null)
            {
                return;
            }

            adapter.Street1 = Street1;
            adapter.Street2 = Street2;
            adapter.Street3 = Street3;
            adapter.City = City;
            adapter.StateProvCode = StateProvCode;
            adapter.PostalCode = PostalCode;
            adapter.CountryCode = CountryCode;
            adapter.ResidentialStatus = (int) ResidentialStatus;
            adapter.POBox = (int) POBox;
            adapter.AddressType = (int)AddressType;
        }

        /// <summary>
        /// Parse the first street field into street1 and street2, if possible
        /// </summary>
        public void ParseStreet1()
        {
            if (string.IsNullOrEmpty(Street1))
            {
                return;
            }

            List<string> unitDesignators = new List<String>()
            {
                " APT ",
                " BSMT ",
                " BLDG ",
                " DEPT ",
                " FL ",
                " FRNT ",
                " HNGR ",
                " KEY ",
                " LBBY ",
                " LOT ",
                " LOWR ",
                " OFC ",
                " PH ",
                " PIER ",
                " REAR ",
                " RM ",
                " SIDE ",
                " SLIP ",
                " SPC ",
                " STOP ",
                " STE ",
                " TRLR ",
                " UNIT ",
                " UPPR ",
                " # "
            };

            string lineToParse = Street1;

            foreach (var designator in unitDesignators)
            {
                int designatorLocation = lineToParse.IndexOf(designator, StringComparison.OrdinalIgnoreCase);

                if (designatorLocation >= 0)
                {
                    Street1 = lineToParse.Substring(0, designatorLocation).Trim();
                    Street2 = lineToParse.Substring(designatorLocation).Trim();

                    break;
                }
            }
        }

        /// <summary>
        /// Applies Address Casing
        /// </summary>
        public void ApplyAddressCasing()
        {
            Street1 = AddressCasing.Apply(Street1);
            Street2 = AddressCasing.Apply(Street2);
            Street3 = AddressCasing.Apply(Street3);
            City = AddressCasing.Apply(City);
        }
    }
}     