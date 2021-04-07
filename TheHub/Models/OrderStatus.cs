using Newtonsoft.Json;

namespace TheHub.Models
{
    public class OrderStatus
    {
        [JsonProperty("epicor_order_number")] public string EpicorOrderNumber { get; set; }

        [JsonProperty("order_status")] public string Status { get; set; }
    }
}