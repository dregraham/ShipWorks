using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShipWorks.Shipping.ShipEngine.DTOs;

namespace ShipWorks.Shipping.ShipEngine.API
{
    public interface ICarriersApi
    {
        Task<CarrierListResponse> CarriersListAsync(string apiKey, string onBehalfOf = null);
    }

    public class CarriersApi : ICarriersApi
    {
        private readonly string endpoint = string.Empty;

        public CarriersApi()
        {

        }

        public CarriersApi(string endpoint)
        {
            this.endpoint = endpoint;
        }

        public Task<CarrierListResponse> CarriersListAsync(string apiKey, string onBehalfOf = null)
        {
            throw new NotImplementedException();
        }
    }
}
