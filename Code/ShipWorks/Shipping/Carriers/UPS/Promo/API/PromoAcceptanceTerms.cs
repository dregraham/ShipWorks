using log4net;
using ShipWorks.Shipping.Carriers.UPS.WebServices.Promo;

namespace ShipWorks.Shipping.Carriers.UPS.Promo.API
{
    /// <summary>
    /// Holds Promo Terms and Conditions info
    /// </summary>
    public class PromoAcceptanceTerms
    {
        private readonly ILog log;

        /// <summary>
        /// Initializes a new instance of the <see cref="PromoAcceptanceTerms"/> class.
        /// </summary>
        public PromoAcceptanceTerms(PromoDiscountAgreementResponse response)
            : this(response, LogManager.GetLogger(typeof(PromoAcceptanceTerms)))
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PromoAcceptanceTerms" /> class.
        /// </summary>
        /// <exception cref="UpsPromoException">Thrown when the response code pf the response is not 1.</exception>
        public PromoAcceptanceTerms(PromoDiscountAgreementResponse response, ILog log)
        {
            this.log = log;

            if (response.Response.ResponseStatus.Code != "1")
            {
                this.log.InfoFormat("PromoDiscountAgreementResponse status code is {0} with a description of {1}",
                    response.Response.ResponseStatus.Code, response.Response.ResponseStatus.Description);
                
                throw new UpsPromoException("There was a problem communicating with UPS to accept the terms and conditions of the UPS promo. Please try again later.");
            }

            URL = response.PromoAgreement.AgreementURL;
            Description = response.PromoDescription;
            AcceptanceCode = response.PromoAgreement.AcceptanceCode;
            IsAccepted = false;
        }

        /// <summary>
        /// Accepts the terms.
        /// </summary>
        public void AcceptTerms()
        {
            IsAccepted = true;
            log.Info("UPS promo terms and conditions have been accepted.");
        }

        /// <summary>
        /// URL to the terms and conditions
        /// </summary>        
        public string URL { get; }

        /// <summary>
        /// Description of the promo 
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Code used to activate discount 
        /// </summary>
        public string AcceptanceCode { get; }
        
        /// <summary>
        /// Gets a value indicating whether this instance is accepted.        
        /// </summary>
        /// <value><c>true</c> if this the terms and conditions are accepted; otherwise, <c>false</c>.</value>
        public bool IsAccepted { get; private set; }
    }
}