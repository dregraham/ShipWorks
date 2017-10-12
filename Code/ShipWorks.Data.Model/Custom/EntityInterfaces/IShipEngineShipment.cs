using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipWorks.Data.Model.EntityInterfaces
{
    /// <summary>
    /// Interface for a ShipmentEntity used by ShipEngine
    /// </summary>
    public interface IShipEngineShipment
    {
        /// <summary> The Contents property of the Entity DhlExpressShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlExpressShipment"."Contents"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 Contents { get; }

        /// <summary> The NonDelivery property of the Entity DhlExpressShipment<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "DhlExpressShipment"."NonDelivery"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 NonDelivery { get; }
    }
}
