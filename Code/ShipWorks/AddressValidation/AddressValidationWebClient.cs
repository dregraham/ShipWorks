using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Xml.XPath;
using Interapptive.Shared.Business;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Shipping.Carriers;

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
        public AddressValidationWebClientValidateAddressResult ValidateAddress(string street1, string street2, string city, string state, string zip)
        {
            AddressValidationWebClientValidateAddressResult validationResult = new AddressValidationWebClientValidateAddressResult()
            {
                AddressValidationResults = new List<AddressValidationResult>(),
                AddressValidationError = string.Empty
            };

            XPathNavigator zip1Result = QueryDialAZip("ZIP1", street1, street2, city, state, zip);

            int returnCode = XPathUtility.Evaluate(zip1Result, "Dial-A-ZIP_Response/ReturnCode", 0);
            
            switch (returnCode)
            {
                case 11:
                    validationResult.AddressValidationError = "Address could not be found because neither a valid City, State, nor valid 5-digit Zip Code was present.";
                    break;

                case 12:
                    validationResult.AddressValidationError = "The State in the address is invalid. Note that only US State and U.S. Territories and possession abbreviations are valid.";
                    break;

                case 13:
                    validationResult.AddressValidationError = "The City in the address submitted is invalid. Remember, city names cannot begin with numbers.";
                    break;

                case 21:
                    validationResult.AddressValidationError = "The address as submitted could not be found. Check for excessive abbreviations in the street address line or in the City name.";
                    break;

                case 25:
                    validationResult.AddressValidationError = "City, State and ZIP Code are valid, but street address is not a match.";
                    break;

                    // Multiple options
                case 22:
                case 32:
                    XPathNavigator zipMResult = QueryDialAZip("ZIPM", street1, street2, city, state, zip);
                    validationResult.AddressValidationResults = ParseZipM(zipMResult);
                    break;

                case 31:

                    if (!DoesAddressExist(zip1Result))
                    {
                        validationResult.AddressValidationError = "Address is not a deliverable address, might be a parking lot or vacant lot";
                    }
                    else
                    {
                        string validatedZip = GetZipPlus4(zip1Result);

                        AddressValidationResult addressValidationResult = new AddressValidationResult()
                        {
                            Street1 = XPathUtility.Evaluate(zip1Result, "/Dial-A-ZIP_Response/AddrLine1", String.Empty),
                            Street2 = XPathUtility.Evaluate(zip1Result, "/Dial-A-ZIP_Response/AddrLine2", String.Empty),
                            Street3 = string.Empty,
                            City = XPathUtility.Evaluate(zip1Result, "/Dial-A-ZIP_Response/City", String.Empty),
                            StateProvCode = Geography.GetStateProvCode(XPathUtility.Evaluate(zip1Result, "/Dial-A-ZIP_Response/State", String.Empty)),
                            PostalCode = validatedZip,
                            CountryCode = Geography.GetCountryCode("US"),
                            ResidentialStatus = GetResidentialStatus(zip1Result),
                            POBox = GetPOBox(zip1Result, "/Dial-A-ZIP_Response/RecType"),
                            IsValid = true
                        };

                        ParseStreet1(addressValidationResult);

                        validationResult.AddressValidationResults = new List<AddressValidationResult>() { addressValidationResult };
                    }
                    break;

                default:
                    throw new AddressValidationException("Address Validation failed to return a known Return Code");
            }

            ApplyAddressCasing(validationResult.AddressValidationResults);

            return validationResult;
        }

        /// <summary>
        /// Gets whether the specified address result is a PO Box or not
        /// </summary>
        private static ValidationDetailStatusType GetPOBox(XPathNavigator zip1Result, string path)
        {
            string poBox = XPathUtility.Evaluate(zip1Result, path, String.Empty);

            if (poBox == "P")
            {
                return ValidationDetailStatusType.True;
            }

            if (string.IsNullOrEmpty(poBox))
            {
                return ValidationDetailStatusType.Unknown;
            }

            return ValidationDetailStatusType.False;
        }

        /// <summary>
        /// Gets whether the specified address result is commercial or residential
        /// </summary>
        private static ValidationDetailStatusType GetResidentialStatus(XPathNavigator zip1Result)
        {
            string residentialStatus = XPathUtility.Evaluate(zip1Result, "/Dial-A-ZIP_Response/RDI", String.Empty);

            if (residentialStatus == "R")
            {
                return ValidationDetailStatusType.True;
            }

            if (residentialStatus == "B")
            {
                return ValidationDetailStatusType.False;
            }

            return ValidationDetailStatusType.Unknown;
        }

        private static void ParseStreet1(AddressValidationResult addressValidationResult)
        {
            if (string.IsNullOrEmpty(addressValidationResult.Street1))
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

            string lineToParse = addressValidationResult.Street1;

            foreach (var designator in unitDesignators)
            {
                int designatorLocation = lineToParse.IndexOf(designator, StringComparison.InvariantCultureIgnoreCase);
             
                if (designatorLocation >=0)
                {
                    addressValidationResult.Street1 = lineToParse.Substring(0, designatorLocation).Trim();
                    addressValidationResult.Street2 = lineToParse.Substring(designatorLocation).Trim();

                    break;
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
        /// Does the address exist.
        /// </summary>
        private static bool DoesAddressExist(XPathNavigator zip1Result)
        {
            return XPathUtility.Evaluate(zip1Result, "//Dial-A-ZIP_Response/AddrExists", false);
        }

        /// <summary>
        /// Applies Address Casing
        /// </summary>
        private static void ApplyAddressCasing(IEnumerable<AddressValidationResult> validationResults)
        {
            if (validationResults == null)
            {
                return;
            }

            foreach (AddressValidationResult validationResult in validationResults)
            {
                validationResult.Street1 = AddressCasing.Apply(validationResult.Street1);
                validationResult.Street2 = AddressCasing.Apply(validationResult.Street2);
                validationResult.Street3 = AddressCasing.Apply(validationResult.Street3);
                validationResult.City = AddressCasing.Apply(validationResult.City);
            }
        }

        /// <summary>
        /// Validates the address and returns multiple.
        /// </summary>
        private static List<AddressValidationResult> ParseZipM(XPathNavigator zipMResults)
        {
            XPathNodeIterator addressIterator = zipMResults.Select("//DAZMultipleMatch/AddressList/Address");

            List<AddressValidationResult> validationResults = new List<AddressValidationResult>();

            while (addressIterator.MoveNext())
            {
                XPathNavigator address = addressIterator.Current;

                string pLow = XPathUtility.Evaluate(address, "MPNum", string.Empty).TrimStart('0');
                string pHigh = XPathUtility.Evaluate(address, "PHigh", string.Empty).TrimStart('0');
                string sLow = XPathUtility.Evaluate(address, "SLow", string.Empty).TrimStart('0');
                string sHigh = XPathUtility.Evaluate(address, "SHigh", string.Empty).TrimStart('0');
                string preDir = XPathUtility.Evaluate(address, "PreDir", string.Empty);
                string street = XPathUtility.Evaluate(address, "Street", string.Empty);
                string suff = XPathUtility.Evaluate(address, "Suff", string.Empty);
                string postDir = XPathUtility.Evaluate(address, "PostDir", string.Empty);
                string unit = XPathUtility.Evaluate(address, "Unit", string.Empty);
                string validatedCity = XPathUtility.Evaluate(address, "City", string.Empty);
                string validatedState = XPathUtility.Evaluate(address, "State", string.Empty);
                string zip5 = XPathUtility.Evaluate(address, "ZIP5", string.Empty);
                string lPlus4 = XPathUtility.Evaluate(address, "LPLUS4", string.Empty);
                string hPlus4 = XPathUtility.Evaluate(address, "HPLUS4", string.Empty);

                // odd or even
                string pCode = XPathUtility.Evaluate(address, "PCode", string.Empty);
                string sCode = XPathUtility.Evaluate(address, "SCode", string.Empty);

                StringBuilder validatedStreet1 = new StringBuilder();

                // Low is the same as high, or high is 0 and doesn't matter
                if (string.IsNullOrEmpty(pHigh) || pHigh == pLow)
                {
                    validatedStreet1.Append(AddSpaceIfNotEmpty(pLow));
                }
                else
                {
                    validatedStreet1.AppendFormat(GetRangeText(pCode, pLow, pHigh));
                }

                validatedStreet1.Append(AddSpaceIfNotEmpty(preDir));
                validatedStreet1.Append(AddSpaceIfNotEmpty(street));
                validatedStreet1.Append(AddSpaceIfNotEmpty(suff));
                validatedStreet1.Append(AddSpaceIfNotEmpty(postDir));

                StringBuilder validatedStreet2 = new StringBuilder();
                validatedStreet2.Append(AddSpaceIfNotEmpty(unit));

                if (string.IsNullOrEmpty(sHigh) || sHigh == sLow)
                {
                    validatedStreet2.Append(AddSpaceIfNotEmpty(sLow));
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
                    StateProvCode = Geography.GetStateProvCode(validatedState),
                    PostalCode = GetMultiZipPlus4(zip5, lPlus4, hPlus4),
                    CountryCode = Geography.GetCountryCode("US"),
                    POBox = GetPOBox(address, "Type")
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
        private static string AddSpaceIfNotEmpty(string myString)
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
                EnsureSecureConnection("https://www.dial-a-zip.com/");

                string endiciaAccountNumber = TangoCredentialStore.Instance.EndiciaAccountNumber;
                string endiciaApiUserPassword = TangoCredentialStore.Instance.EndiciaApiUserPassword;

            string zip1Query = string.Format(
                    "<VERIFYADDRESS><COMMAND>{0}</COMMAND><SERIALNO>{1}</SERIALNO><PASSWORD>{2}</PASSWORD><USER>{1}</USER>"+
                    "<ADDRESS0 /><ADDRESS1>{3}</ADDRESS1><ADDRESS2>{4}</ADDRESS2><ADDRESS3>{5}, {6} {7}</ADDRESS3></VERIFYADDRESS>", 
                    command, endiciaAccountNumber, endiciaApiUserPassword, street2, street1, city, state, zip);

            string url =
                string.Format(
                        "https://www.dial-a-zip.com/XML-Dial-A-ZIP/DAZService.asmx/MethodZIPValidate?input={0}",
                    HttpUtility.UrlEncode(zip1Query));

            try
            {
                WebRequest request = WebRequest.Create(url);
                
                XPathDocument xPathZip1Result;

                using (Stream responseStream = request.GetResponse().GetResponseStream())
                {
                    if (responseStream == null)
                    {
                        LogValidationAttempt(command, url, null);
                        throw new AddressValidationException("Error validating address.");
                    }

                    xPathZip1Result = new XPathDocument(responseStream);
                }

                // Log successful validations only when requested
                if (InterapptiveOnly.MagicKeysDown)
                {
                    LogValidationAttempt(command, url, xPathZip1Result);
                }

                return xPathZip1Result.CreateNavigator();
            }
            catch (Exception ex)
            {
                LogValidationAttempt(command, url, null);
                throw WebHelper.TranslateWebException(ex, typeof(AddressValidationException));
            }
        }

        private static void EnsureSecureConnection(string url)
        {
            CertificateInspector certificateInspector =
                new CertificateInspector(TangoCredentialStore.Instance.EndiciaCertificateVerificationData);
            CertificateRequest certificateRequest = new CertificateRequest(new Uri(url), certificateInspector);
            CertificateSecurityLevel certificateSecurityLevel = certificateRequest.Submit();

            if (certificateSecurityLevel != CertificateSecurityLevel.Trusted)
            {
                throw new AddressValidationException(
                    "ShipWorks is unable to make a secure connection to the Address Validation Server.");
            }
        }
		
		/// <summary>
        /// Log the validation look up
        /// </summary>
        private static void LogValidationAttempt(string command, string url, XPathDocument xPathZipResult)
        {
            ApiLogEntry apiLogEntry = new ApiLogEntry(ApiLogSource.DialAZip, command);
            apiLogEntry.LogRequest(url);

            if (xPathZipResult != null)
            {
                apiLogEntry.LogResponse(xPathZipResult);
            }
        }
    }
}