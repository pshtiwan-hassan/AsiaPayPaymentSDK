using System.Text.Json.Serialization;

namespace AsiaPayPaymentSDK.AsiaPay.QueryOrder;

public class BizContentQueryOrderResponse
{
    [JsonPropertyName("merch_order_id")]
    public string? MerchOrderId { get; set; }

    [JsonPropertyName("order_status")]
    public string? OrderStatus { get; set; }

    [JsonPropertyName("payment_order_id")]
    public string? PaymentOrderId { get; set; }

    [JsonPropertyName("trans_time")]
    public string? TransTime { get; set; }

    [JsonPropertyName("trans_currency")]
    public string? TransCurrency { get; set; }

    [JsonPropertyName("total_amount")]
    public decimal? TotalAmount { get; set; }

    [JsonPropertyName("trans_id")]
    public string? TransId { get; set; }
}