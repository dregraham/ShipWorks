using Interapptive.Shared.Business.Geography;
using Interapptive.Shared.UI;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.SparkPay;
using ShipWorks.Stores.Platforms.SparkPay.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;

namespace ShipWorks.Stores.UI.Platforms.SparkPay.WizardPages
{
    class SparkPayAccountViewModel : INotifyPropertyChanged, ISparkPayAccountViewModel
    {
        private readonly Func<SparkPayStoreEntity, SparkPayStatusCodeProvider> statusCodeProviderFactory;
        IMessageHelper messageHelper;
        SparkPayWebClient webClient;
        readonly PropertyChangedHandler handler;
        public event PropertyChangedEventHandler PropertyChanged;
        string token;
        string url;

        /// <summary>
        /// Constructor
        /// </summary>
        public SparkPayAccountViewModel(Func<SparkPayStoreEntity, SparkPayStatusCodeProvider> statusCodeProviderFactory, IMessageHelper messageHelper, SparkPayWebClient webClient)
        {
            this.statusCodeProviderFactory = statusCodeProviderFactory;
            this.messageHelper = messageHelper;
            this.webClient = webClient;
            handler = new PropertyChangedHandler(this, () => PropertyChanged);
        }

        /// <summary>
        /// The token
        /// </summary>
        public string Token
        {
            get { return token; }
            set { handler.Set(nameof(Token), ref token, value); }
        }

        /// <summary>
        /// The store url
        /// </summary>
        public string Url
        {
            get { return url; }
            set { handler.Set(nameof(Url), ref url, value); }
        }

        /// <summary>
        /// Saves the store info
        /// </summary>
        public bool Save(SparkPayStoreEntity store)
        {
            Dictionary<string, StoresResponse> validatedCredentials = ValidCredentials();
            
            if (validatedCredentials != null && validatedCredentials.Any())
            {
                StoresResponse storeResponse = validatedCredentials.FirstOrDefault().Value;

                if (storeResponse.total_count > 0)
                {
                    Store sparkPayStore = storeResponse.stores.FirstOrDefault();
                    if (sparkPayStore != null)
                    {
                        store.StoreName = sparkPayStore.name;
                        store.Company = sparkPayStore.company_name;
                        store.StoreUrl = sparkPayStore.domain_name;
                        store.Street1 = sparkPayStore.address_line_1;
                        store.Street2 = sparkPayStore.address_line_2;
                        store.City = sparkPayStore.city;
                        store.StateProvCode = Geography.GetStateProvCode(sparkPayStore.state);
                        store.PostalCode = sparkPayStore.postal_code;
                        store.CountryCode = Geography.GetCountryCode(sparkPayStore.country);
                        store.Email = sparkPayStore.email;
                        store.Fax = sparkPayStore.fax;
                    }
                }

                store.Token = Token;
                store.StoreUrl = validatedCredentials.FirstOrDefault().Key;
                
                SparkPayStatusCodeProvider statusCodeProvider = statusCodeProviderFactory(store);

                statusCodeProvider.UpdateFromOnlineStore();

                return true;
            }
            return false;
        }

        private Dictionary<string, StoresResponse> ValidCredentials()
        {
            if (string.IsNullOrEmpty(Token))
            {
                messageHelper.ShowError("Please enter your Store URL");
                return null;
            }

            if (string.IsNullOrEmpty(Url))
            {
                messageHelper.ShowError("Please enter your Access Token");
                return null;
            }

            Dictionary<string, StoresResponse> response = new Dictionary<string, StoresResponse>();

            try
            {
                // Try getting a list of stores based on the Uri we generated
                string apiUrl = GetApiUrl(new Uri(Url));
                response.Add(apiUrl, GetStore(Token, apiUrl));
            }
            catch (UriFormatException)
            {
                messageHelper.ShowError("The Url entered is not a valid SparkPay store url.");
            }
            catch (SparkPayException ex)
            {
                WebException innerException = ex.InnerException as WebException;
                if (innerException != null && innerException.Status == WebExceptionStatus.ProtocolError)
                {
                    HttpWebResponse webResponse = innerException.Response as HttpWebResponse;
                    if (webResponse != null && webResponse.StatusCode == HttpStatusCode.NotFound)
                    {
                        // Path was wrong so we try again using the string that was entered
                        try
                        {
                            response.Add(Url, GetStore(Token, Url));
                        }
                        catch (SparkPayException x)
                        {
                            messageHelper.ShowError($"The Url entered is not a valid SparkPay store url. {x.Message}");
                        }
                    }
                }
                else
                {
                    messageHelper.ShowError($"The Url entered is not a valid SparkPay store url. {ex.Message}");
                }
            }

            return response;
        }

        private StoresResponse GetStore(string token, string uri)
        {
            return webClient.GetStores(token, $"{uri}/stores");
        }

        private static string GetApiUrl(Uri uri)
        {
            return $"https://{uri.Host}/api/v1";
        }
    }
}
