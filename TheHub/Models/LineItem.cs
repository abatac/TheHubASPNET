using Newtonsoft.Json;

namespace TheHub.Models
{
    public class LineItem
    {
        [JsonProperty("order_item_number")] public string OrderItemNumber { get; set; }

        [JsonProperty("quantity_ordered")] public int? QuantityOrdered { get; set; }

        [JsonProperty("unit_price")] public float? UnitPrice { get; set; }

        [JsonProperty("total_price")] public float? TotalPrice { get; set; }

        [JsonProperty("product_code")] public string ProductCode { get; set; }

    }
}