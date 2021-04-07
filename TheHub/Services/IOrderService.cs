using System.Threading.Tasks;
using TheHub.Models;

namespace TheHub.Services
{
    public interface IOrderService
    {
        OrderResponse Add(Order order);

        Shipment GetShipmentDetails(string salesforceOrderId);

        Task<OrderStatusResponse> OrderStatus(OrderStatusRequest order);
    }
}