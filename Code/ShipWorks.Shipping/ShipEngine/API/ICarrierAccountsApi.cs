using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShipWorks.Shipping.ShipEngine.DTOs;

namespace ShipWorks.Shipping.ShipEngine.API
{
    public interface ICarrierAccountsApi
    {
        Task<ConnectAccountResponseDTO> DHLExpressAccountCarrierConnectAccountAsync(DHLExpressAccountInformationDTO model, string apiKey, string onBehalfOf = null);

        Task<ConnectAccountResponseDTO> AsendiaAccountCarrierConnectAccountAsync(AsendiaAccountInformationDTO model, string apiKey, string onBehalfOf = null);

    }

    public class CarrierAccountsApi : ICarrierAccountsApi
    {
        private readonly string endpoint = string.Empty;

        public CarrierAccountsApi()
        {

        }

        public CarrierAccountsApi(string endpoint)
        {
            this.endpoint = endpoint;
        }

        public Task<ConnectAccountResponseDTO> DHLExpressAccountCarrierConnectAccountAsync(DHLExpressAccountInformationDTO model, string apiKey, string onBehalfOf = null)
        {
            throw new NotImplementedException();
        }

        public Task<ConnectAccountResponseDTO> AsendiaAccountCarrierConnectAccountAsync(AsendiaAccountInformationDTO model, string apiKey,
            string onBehalfOf = null)
        {
            throw new NotImplementedException();
        }
    }
}
