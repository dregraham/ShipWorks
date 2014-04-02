using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Xml.XPath;
using Interapptive.Shared.Utility;

namespace ShipWorks.AddressValidation
{
    public class AddressValidationWebClient : IAddressValidationWebClient
    {
        /// <summary>
        /// Validates the address.
        /// </summary>
        /// <exception cref="AddressValidationException">
        /// Address Validation failed to return Return Code
        /// or
        /// Address could not be found because enither a valid City, State, nor valid 5-digit Zip Code was present.
        /// or
        /// The State in the address is invalid. Note that only US State and U.S. Territories and possession abbreviations are valid.
        /// or
        /// The City in the address submitted is invalid. Remember, city names cannot begin with numbers.
        /// or
        /// The address as submitted could not be found. Check for excessive abbreviations in the street address line or in the City name.
        /// or
        /// City, State and ZIP Code are valid, but street address is not a match.
        /// or
        /// Address does not exist.
        /// or
        /// Unknown error validating address.
        /// </exception>
        public List<AddressValidationResult> ValidateAddress(string street1, string street2, string city, string state, String zip)
        {
            XPathNavigator zip1Result = QueryDialAZip("ZIP1", street1, street2, city, state, zip);

            int returnCode = XPathUtility.Evaluate(zip1Result, "//Dial-A-ZIP_Response/ReturnCode", 0);

            switch (returnCode)
            {
                case 0:
                {
                    throw new AddressValidationException("Address Validation failed to return Return Code");
                }
                case 11:
                {
                    throw new AddressValidationException("Address could not be found because neither a valid City, State, nor valid 5-digit Zip Code was present.");
                }
                case 12:
                {
                    throw new AddressValidationException("The State in the address is invalid. Note that only US State and U.S. Territories and possession abbreviations are valid.");
                }
                case 13:
                {
                    throw new AddressValidationException("The City in the address submitted is invalid. Remember, city names cannot begin with numbers.");
                }
                case 21:
                {
                    throw new AddressValidationException("The address as submitted could not be found. Check for excessive abbreviations in the street address line or in the City name.");
                }
                case 22:
                {
                    return ValidateAddressMultiple(street1, street2, city, state, zip);
                }
                case 25:
                {
                    throw new AddressValidationException("City, State and ZIP Code are valid, but street address is not a match.");
                }
                case 31:
                {
                    bool addressExists = XPathUtility.Evaluate(zip1Result, "//Dial-A-ZIP_Response/AddrExists", false);
                    if (addressExists)
                    {
                        string validatedZip = GetZipPlus4(zip1Result);

                        return new List<AddressValidationResult>()
                        {
                            new AddressValidationResult()
                            {
                                Street1 = XPathUtility.Evaluate(zip1Result, "//Dial-A-ZIP_Response/AddrLine1", String.Empty),
                                Street2 = XPathUtility.Evaluate(zip1Result, "//Dial-A-ZIP_Response/AddrLine2", String.Empty),
                                City = XPathUtility.Evaluate(zip1Result, "//Dial-A-ZIP_Response/City", String.Empty),
                                StateProvCode = XPathUtility.Evaluate(zip1Result, "//Dial-A-ZIP_Response/State", String.Empty),
                                PostalCode = validatedZip,
                                CountryCode = "US",
                                IsValid = true
                            }
                        };
                    }
                    else
                    {
                        throw new AddressValidationException("Address does not exist.");
                    }
                }
                case 32:
                {
                    return ValidateAddressMultiple(street1, street2, city, state, zip);
                }
                default:
                {
                    throw new AddressValidationException("Unknown error validating address.");
                }
            }
        }

        /// <summary>
        /// Zips the specified zip1 result.
        /// </summary>
        private static string GetZipPlus4(XPathNavigator zip1Result)
        {
            string zip = XPathUtility.Evaluate(zip1Result, "//Dial-A-ZIP_Response/ZIP5", String.Empty);
            string plus4 = XPathUtility.Evaluate(zip1Result, "//Dial-A-ZIP_Response/Plus4", String.Empty);
            if (!string.IsNullOrEmpty(plus4))
            {
                zip = string.Format("{0}-{1}", zip, plus4);
            }
            return zip;
        }

        /// <summary>
        /// Validates the address and returns multiple.
        /// </summary>
        private static List<AddressValidationResult> ValidateAddressMultiple(string street1, string street2, string city, string state, string zip)
        {
            XPathNavigator zipMResults = QueryDialAZip("ZIPM", street1, street2, city, state, zip);

            XPathNodeIterator addressIterator = zipMResults.Select("//DAZMultipleMatch/AddressList/Address");

            List<AddressValidationResult> validationResults = new List<AddressValidationResult>();

            while (addressIterator.MoveNext())
            {
                XPathNavigator address = addressIterator.Current;

                string pLow = XPathUtility.Evaluate(address, "/PLow", string.Empty);
                string pHigh = XPathUtility.Evaluate(address, "/PHigh", string.Empty);
                string sLow = XPathUtility.Evaluate(address, "/SLow", string.Empty);
                string sHigh = XPathUtility.Evaluate(address, "/SHigh", string.Empty);
                string preDir = XPathUtility.Evaluate(address, "/PreDir", string.Empty);
                string street = XPathUtility.Evaluate(address, "/Street", string.Empty);
                string suff = XPathUtility.Evaluate(address, "/Suff", string.Empty);
                string postDir = XPathUtility.Evaluate(address, "/PostDir", string.Empty);
                string unit = XPathUtility.Evaluate(address, "/Unit", string.Empty);
                string validatedCity = XPathUtility.Evaluate(address, "/City", string.Empty);
                string validatedState = XPathUtility.Evaluate(address, "/State", string.Empty);
                string zip5 = XPathUtility.Evaluate(address, "/ZIP5", string.Empty);
                string lPlus4 = XPathUtility.Evaluate(address, "/LPLUS4", string.Empty);
                string hPlus4 = XPathUtility.Evaluate(address, "/HPLUS4", string.Empty);

                // odd or even
                string pCode = XPathUtility.Evaluate(address, "/PCode", string.Empty);
                string sCode = XPathUtility.Evaluate(address, "/SCode", string.Empty);

                StringBuilder validatedStreet1 = new StringBuilder();

                // Low is the same as high, or high is 0 and doesn't matter
                if (string.IsNullOrEmpty(pHigh) || pHigh == pLow)
                {
                    validatedStreet1.Append(addSpaceIfNotEmpty(pLow));
                }
                else
                {
                    validatedStreet1.AppendFormat(GetRangeText(pCode, pLow, pHigh));
                }

                validatedStreet1.Append(addSpaceIfNotEmpty(preDir));
                validatedStreet1.Append(addSpaceIfNotEmpty(street));
                validatedStreet1.Append(addSpaceIfNotEmpty(suff));
                validatedStreet1.Append(addSpaceIfNotEmpty(postDir));

                StringBuilder validatedStreet2 = new StringBuilder();
                validatedStreet2.Append(addSpaceIfNotEmpty(unit));

                if (!string.IsNullOrEmpty(sHigh) || sHigh == sLow)
                {
                    validatedStreet2.Append(addSpaceIfNotEmpty(sLow));
                }
                else
                {
                    validatedStreet2.AppendFormat(GetRangeText(sCode, sLow, sHigh));
                }

                AddressValidationResult addressValidationResult = new AddressValidationResult()
                {
                    Street1 = validatedStreet1.ToString(),
                    Street2 = validatedStreet2.ToString(),
                    City = validatedCity,
                    StateProvCode = validatedState,
                    PostalCode = GetMultiZipPlus4(zip5, lPlus4, hPlus4),
                    CountryCode = "USA"
                };

                validationResults.Add(addressValidationResult);
            }

            return validationResults;
        }

        /// <summary>
        /// Gets the zip plus4. Supports ranges
        /// </summary>
        private static string GetMultiZipPlus4(string zip5, string lPlus4, string hPlus4)
        {
            if (string.IsNullOrEmpty(lPlus4) && string.IsNullOrEmpty(hPlus4))
            {
                return zip5;
            }

            if (string.IsNullOrEmpty(lPlus4))
            {
                return string.Format("{0}-{1}", zip5, hPlus4);
            }

            // We know lPlus4 has a value:
            if (string.IsNullOrEmpty(hPlus4) || lPlus4 == hPlus4)
            {
                return string.Format("{0}-{1}", zip5, lPlus4);
            }

            return string.Format("(Range {0} - {1}) ", lPlus4, hPlus4);
        }

        /// <summary>
        /// Adds the space if not empty.
        /// </summary>
        private static string addSpaceIfNotEmpty(string myString)
        {
            if (!string.IsNullOrEmpty(myString))
            {
                myString += " ";
            }
            return myString;
        }

        /// <summary>
        /// Gets the range text
        /// </summary>
        private static string GetRangeText(string code, string low, string high)
        {
            string addressPart;
            if (string.IsNullOrEmpty(code))
            {
                addressPart = string.Format("(Range {0} - {1}) ", low, high);
            }
            else
            {
                addressPart = string.Format("({0} Range {1} - {2}) ",
                    code.Equals("o", StringComparison.OrdinalIgnoreCase) ? "Odd" : "Even",
                    low,
                    high);
            }

            return addressPart;
        }

        /// <summary>
        /// Queries Endicia. Command can be Zip1 or ZipM
        /// </summary>
        private static XPathNavigator QueryDialAZip(string command, string street1, string street2, string city, string state, String zip)
        {
            try
            {
                string zip1Query = string.Format(
                    "<VERIFYADDRESS>" +
                    "<COMMAND>{0}</COMMAND>" +
                    "<SERIALNO>{1}</SERIALNO>" +
                    "<PASSWORD>{2}</PASSWORD>" +
                    "<USER>{1}</USER>" +
                    "<ADDRESS0 />" +
                    "<ADDRESS1>{3}</ADDRESS1>" +
                    "<ADDRESS2>{4}</ADDRESS2>" +
                    "<ADDRESS3>{5}, {6} {7}</ADDRESS3>" +
                    "</VERIFYADDRESS>", command, "512251", "endicia7458", street1, street2, city, state, zip);

                string url =
                    string.Format(
                        "http://www.dial-a-zip.com/XML-Dial-A-ZIP/DAZService.asmx/MethodZIPValidate?input={0}",
                        HttpUtility.UrlEncode(zip1Query));

                WebRequest request = WebRequest.Create(url);

                Stream responseStream = request.GetResponse().GetResponseStream();

                if (responseStream == null)
                {
                    throw new AddressValidationException("Error validating address.");
                }

                XPathDocument xPathZip1Result = new XPathDocument(responseStream);

                return xPathZip1Result.CreateNavigator();
            }
            catch (Exception ex)
            {
                throw new AddressValidationException("Error validating address.", ex);
            }
        }
    }
}