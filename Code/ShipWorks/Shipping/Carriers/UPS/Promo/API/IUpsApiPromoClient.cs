using System.IO;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ShipWorks.Shipping.Carriers.UPS.WebServices.Promo;

namespace ShipWorks.Shipping.Carriers.UPS.Promo.API
{
    /// <summary>
    /// Interface for UPS Promo Client
    /// </summary>
    public interface IUpsApiPromoClient
    {
        /// <summary>
        /// Activates the UPS Promo using the given code
        /// </summary>
        PromoActivation Activate(string acceptanceCode);

        /// <summary>
        /// Gets the Promo Acceptance Terms
        /// </summary>
        PromoAcceptanceTerms GetAgreement();
    }

    
    /// <summary>
    /// Todo: get rid of this
    /// This relies on a file at c:\FakeUpsApiPromoClient.json. 
    /// An example of this file is in this directory.
    /// </summary>
    class FakeUpsApiPromoClient : IUpsApiPromoClient
    {
        private ILog log;
        private UpsPromo promo;

        private bool GetAgreementThrowsError = false;
        private bool GetAgreementIsSuccessful = true;
        private bool ActivateThrowsError = false;
        private bool ActivateIsSuccessful = true;

        public FakeUpsApiPromoClient(UpsPromo promo)
        {
            log = LogManager.GetLogger(typeof(FakeUpsApiPromoClient));
            log.Info($"FakeUpsApiPromoClient recieved a promo with account {promo.AccountNumber}.");

            const string fakeConfigFileName = @"c:\FakeUpsApiPromoClient.json";
            if (File.Exists(fakeConfigFileName))
            {
                JObject fakeConfig = JObject.Parse(File.ReadAllText(fakeConfigFileName));

                GetAgreementThrowsError = fakeConfig["GetAgreement"]["ThrowsError"].Value<bool>();
                GetAgreementIsSuccessful = fakeConfig["GetAgreement"]["IsSuccessful"].Value<bool>();
                ActivateThrowsError = fakeConfig["Activate"]["ThrowsError"].Value<bool>();
                ActivateIsSuccessful = fakeConfig["Activate"]["IsSuccessful"].Value<bool>();
            }
            
            this.promo = promo;
        }

        public PromoActivation Activate(string acceptanceCode)
        {
            log.Info($"FakeUpsApiPromoClient.Activate({acceptanceCode})");

            if (ActivateThrowsError)
            {
                throw new UpsPromoException("Throwing because of FakeUpsApiPromoClient.json says so.");
            }

            return new PromoActivation(new PromoDiscountResponse()
            {
                Response = new ResponseType()
                {
                    ResponseStatus = new CodeDescriptionType()
                    {
                        Code = ActivateIsSuccessful ? "1" : "0"
                    }
                }
            });
        }

        public PromoAcceptanceTerms GetAgreement()
        {
            string acceptanceCode = "42";
            log.Info($"FakeUpsApiPromoClient.GetAgreement() returning acceptance code of {acceptanceCode}, AgreementURL of www.google.com and PromoDescription of \"Mocked Descriptoin\"");

            if (GetAgreementThrowsError)
            {
                throw new UpsPromoException("Throwing because of FakeUpsApiPromoClient.json says so.");
            }

            return new PromoAcceptanceTerms(new PromoDiscountAgreementResponse()
            {
                PromoAgreement = new PromoAgreementType()
                {
                    AgreementURL = "www.google.com",
                    AcceptanceCode = acceptanceCode
                },
                PromoDescription = "Mocked Description", Response = new ResponseType()
                {
                    ResponseStatus = new CodeDescriptionType()
                    {
                        Code = GetAgreementIsSuccessful ? "1" : "0"
                    }
                }
            });
        }
    }
}