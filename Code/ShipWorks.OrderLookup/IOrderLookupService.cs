using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.OrderLookup
{
    public interface IOrderLookupService
    {
        Task<OrderEntity> FindOrder(string scanMsgScannedText);
    }
}