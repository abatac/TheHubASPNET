using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace TheHub.Models
{
    public class Order : IValidatableObject
    {
        [JsonProperty("order_number")] public string OrderNumber { get; set; }

        [JsonProperty("account_id")] public string AccountId { get; set; }

        [JsonProperty("customer_purchase_order")]
        [Required(ErrorMessage = "customer_purchase_order is required", AllowEmptyStrings = false)]
        public string CustomerPurchaseOrder { get; set; }

        [JsonProperty("description")] public string Description { get; set; }

        [JsonProperty("total_amount")] public string TotalAmount { get; set; }

        [JsonProperty("order_date")] public string OrderDate { get; set; }

        [JsonProperty("billing_street")] public string BillingStreet { get; set; }

        [JsonProperty("billing_city")] public string BillingCity { get; set; }

        [JsonProperty("billing_state")] public string BillingState { get; set; }

        [JsonProperty("billing_postal_code")] public string BillingPostalCode { get; set; }

        [JsonProperty("billing_country")] public string BillingCountry { get; set; }

        [JsonProperty("bill_to_attention")] public string BillToAttention { get; set; }

        [JsonProperty("shipping_street")] public string ShippingStreet { get; set; }

        [JsonProperty("shipping_city")] public string ShippingCity { get; set; }

        [JsonProperty("shipping_state")] public string ShippingState { get; set; }

        [JsonProperty("shipping_postal_code")] public string ShippingPostalCode { get; set; }

        [JsonProperty("shipping_country")] public string ShippingCountry { get; set; }

        [JsonProperty("ship_to_contact")] public string ShipToContact { get; set; }

        [JsonProperty("ship_to_attention")] public string ShipToAttention { get; set; }

        [JsonProperty("shipping_method")] public string ShippingMethod { get; set; }

        [JsonProperty("pct_sales_rep")] public string PctSalesRep { get; set; }

        [JsonProperty("expected_delivery_date")]
        public string ExpectedDeliveryDate { get; set; }

        [JsonProperty("subscription_start_date")]
        [Required(ErrorMessage = "subscription_start_date is required", AllowEmptyStrings = false)]
        public string SubscriptionStartDate { get; set; }

        [JsonProperty("warranty_terms")] public string WarrantyTerm { get; set; }

        [JsonProperty("epicor_account_id")] public string EpicorAccountId { get; set; }

        [JsonProperty("line_items")] public List<LineItem> LineItems { get; set; }

        [JsonProperty("order_subscription_term")] public int OrderSubscriptionTerm { get; set; }

        [JsonProperty("monthly_subscription")] public double MonthlySubscription { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return new List<ValidationResult>();
        }
    }
}