using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon.Api;
using ShipWorks.Stores;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    public class AmazonCredentials : INotifyPropertyChanged
    {
        private readonly IAmazonShippingWebClient webClient;
        private readonly IStoreManager storeManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonCredentials(IAmazonShippingWebClient webClient, IStoreManager storeManager)
        {
            this.webClient = webClient;
            this.storeManager = storeManager;
        }

        /// <summary>
        /// Initialize the class
        /// </summary>
        public void Initialize()
        {
            List<AmazonStoreEntity> stores = storeManager.GetAllStores()
                .OfType<AmazonStoreEntity>()
                .Where(x => x.Enabled)
                .GroupBy(x => new { x.MerchantID, x.AuthToken })
                .Select(x => x.FirstOrDefault())
                .ToList();

            AmazonStoreEntity store = stores.Count == 1 ? stores[0] : null;

            MerchantId = store != null ? store.MerchantID : string.Empty;
            AuthToken = store != null ? store.AuthToken : string.Empty;
        }

        /// <summary>
        /// A property has changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Amazon account merchant id
        /// </summary>
        public string MerchantId { get; set; }

        /// <summary>
        /// Amazon account authentication token
        /// </summary>
        public string AuthToken { get; set; }

        /// <summary>
        /// Was the validation successful
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Message from result of validation
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Validate the credentials
        /// </summary>
        public void Validate()
        {
            if (!string.IsNullOrWhiteSpace(MerchantId) && !string.IsNullOrWhiteSpace(AuthToken))
            {
                AmazonValidateCredentialsResponse response = webClient.ValidateCredentials(MerchantId, AuthToken);

                Success = response.Success;
                Message = response.Message;
            }
            else
            {
                Success = false;
                Message = "MerchantId and AuthToken are required";
            }
        }

        /// <summary>
        /// Raise the property changed event
        /// </summary>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
