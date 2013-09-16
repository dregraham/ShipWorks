using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Interapptive.Shared.Business;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Shipping.Carriers.Postal.Express1.WebServices.CustomerService;

namespace ShipWorks.Shipping.Carriers.Postal.Express1
{
    /// <summary>
    /// Gateway class for integrating with Express1
    /// </summary>
    class Express1RegistrationGateway : IExpress1RegistrationGateway
    {
        private readonly IExpress1ConnectionDetails connectionDetails;

        /// <summary>
        /// Create a new instance of the Express1 gateway
        /// </summary>
        /// <param name="connectionDetails">Details that define the connection to use</param>
        public Express1RegistrationGateway(IExpress1ConnectionDetails connectionDetails)
        {
            this.connectionDetails = connectionDetails;
        }

        /// <summary>
        /// Register an account with Express1
        /// </summary>
        /// <param name="customerData"></param>
        /// <returns></returns>
        public CustomerCredentials Register(CustomerRegistrationData customerData)
        {
            // populate security information
            customerData.SecurityInfo = GetSecurityInfo();

            using (CustomerService service = CreateCustomerService("Signup"))
            {
                return service.RegisterCustomer(connectionDetails.ApiKey,
                                                    GetCustomerInfoString(Guid.NewGuid().ToString()),
                                                    customerData);
            }
        }

        /// <summary>
        /// Customer Service creation
        /// </summary>
        private CustomerService CreateCustomerService(string logName)
        {
            // configure the endpoint
            return new CustomerService(new ApiLogEntry(ApiLogSource.UspsExpress1Endicia, logName))
                {
                    Url = connectionDetails.TestServer
                              ? "http://www.express1dev.com/Services/CustomerService.svc"
                              : "https://service.express1.com/Services/CustomerService.svc"
                };
        }

        /// <summary>
        /// Encrypts a string based on Express1's requirements 
        /// </summary>
        private string EncryptData(string dataString)
        {
            using (Rijndael rijndael = Rijndael.Create())
            {
                // per Express1
                rijndael.BlockSize = 128;
                rijndael.KeySize = 256;
                rijndael.Mode = CipherMode.CBC;
                rijndael.Padding = PaddingMode.PKCS7;

                // our secret stuff
                string key = connectionDetails.CertificateId.Substring(0, 32);
                string iv = "F9sl139fakvN#1kl";

                byte[] keyBytes = Encoding.ASCII.GetBytes(key);
                byte[] ivBytes = Encoding.ASCII.GetBytes(iv);
                byte[] dataBytes = Encoding.ASCII.GetBytes(dataString);

                using (MemoryStream memStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memStream, rijndael.CreateEncryptor(keyBytes, ivBytes), CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(dataBytes, 0, dataBytes.Length);
                        cryptoStream.Close();

                        // return the encrypted data, base64 encoded
                        return Convert.ToBase64String(memStream.ToArray());
                    }
                }
            }
        }

        /// <summary>
        /// Get the encrypted customerInfo string
        /// </summary>
        public string GetCustomerInfoString(string customerID)
        {
            // create the tokenized string to encrypt
            string info = string.Format("{0}|{1}|Account", connectionDetails.FranchiseId, customerID);

            // encrypt per Express1's requirements
            return EncryptData(info);
        }

        /// <summary>
        /// Get the signup url for the web-based signup method
        /// </summary>
        public string GetSignupUrl(string customerInfo)
        {
            string urlBase = (connectionDetails.TestServer) ? 
                "http://www.express1dev.com/CustomerWeb/Gateway.aspx" : 
                "https://service.express1.com/Customer/Gateway.aspx";
            
            // format the url
            return string.Format("{0}?app={1}&info={2}", urlBase, connectionDetails.ApiKey, HttpUtility.UrlEncode(customerInfo));
        }

        /// <summary>
        /// Creates the SecurityInfo data to send to Express1 along with registration
        /// for fraud detection purposes
        /// </summary>
        private static SecurityInfo GetSecurityInfo()
        {
            return new SecurityInfo
                {
                    RemoteIPAddress = GetPublicIP(),
                    SessionID = ShipWorksSession.InstanceID.ToString(),
                    UserAgent = "ShipWorks"
                };
        }

        /// <summary>
        /// Gets the public ip address of this computer
        /// </summary>
        private static string GetPublicIP()
        {
            try
            {
                WebRequest request = WebRequest.Create("http://checkip.dyndns.org");

                // if we don't get a near-immediate response, don't wait
                request.Timeout = 1000;

                using (WebResponse response = request.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        string text = reader.ReadToEnd();

                        Regex addressRegex = new Regex(@"Current IP Address: (\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3})");
                        string ip = addressRegex.Match(text).Groups[1].Value;

                        return !String.IsNullOrEmpty(ip) ? ip : "Unknown";
                    }
                }
            }
            catch (Exception)
            {
                return "Unknown";
            }
        }
    }
}
