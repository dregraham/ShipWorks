using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Shipping.Carriers.Api
{
    /// <summary>
    /// An interface for manipulating the attributes of a carrier request.
    /// </summary>
    public interface ICarrierRequestManipulator
    {
        /// <summary>
        /// Manipulates the specified request.
        /// </summary>
        /// <param name="request">The request being manipulated.</param>
        void Manipulate(CarrierRequest request);
    }
}
