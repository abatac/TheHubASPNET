using Newtonsoft.Json;

namespace TheHub.Models
{
    public class OrderStatusRequest
    {
        [JsonProperty("salesforce_order_number")] public string SalesforceOrderNumber { get; set; }

        [JsonProperty("status")] public string Status { get; set; }
    }
}