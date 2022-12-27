using System.Collections.Generic;
using Newtonsoft.Json;

namespace TheHub.Models
{
    public class Shipment
    {
        [JsonProperty("epicor_order_number")] public string EpicorOrderNumber { get; set; }

        [JsonProperty("salesforce_order_number")]
        public string SalesForceOrderNumber { get; set; }

        [JsonProperty("salesforce_account_id")]
        public string SalesForceAccountId { get; set; }

        [JsonProperty("packing_slip_number")]
        public string PackingSlipNumber { get; set; }
        [JsonProperty("shipment_details")] public List<ShipmentDetail> ShipmentDetails { get; set; }

        [JsonProperty("shipped_date")]
        public string ShippedDate { get; set; }

        [JsonProperty("bill_to_customer_name")]
        public string BillToCustomerName { get; set; }

        [JsonProperty("tracking_id")]
        public string TrackingId { get; set; }

        [JsonProperty("ship_to_city")]
        public string ShipToCity { get; set; }

        [JsonProperty("ship_to_state")]
        public string ShipToState { get; set; }

        [JsonProperty("ship_to_name")]
        public string ShipToName { get; set; }

    }
}