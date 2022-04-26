using System.Collections.Generic;

namespace ShipWorks.Shipping.ShipEngine.DTOs
{
    /// <summary>
    /// The base class for responses from ShipEngine
    /// </summary>
    public class BaseShipEngineResponse
    {
        public string RequestId { get; set; }

        public List<ShipEngineError> Errors { get; set; }
    }

    public class ShipEngineError
    {
        public string ErrorSource { get; set; }

        public string ErrorType { get; set; }

        public string ErrorCode { get; set; }

        public string Message { get; set; }
    }
}
