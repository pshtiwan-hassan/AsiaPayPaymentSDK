using System.Text.Json.Serialization;

namespace AsiaPayPaymentSDK.AsiaPay.Refund;

public class BizContentRefundResponse 
{
    [JsonPropertyName("merch_code")]
    public string? MerchCode { get; set; } 

    [JsonPropertyName("merch_order_id")]
    public string? MerchOrderId { get; set; }  

    [JsonPropertyName("refund_amount")]
    public string? RefundAmount { get; set; }  

    [JsonPropertyName("refund_currency")]
    public string? RefundCurrency { get; set; } 

    [JsonPropertyName("refund_order_id")]
    public string? RefundOrderId { get; set; } 

    [JsonPropertyName("refund_status")]
    public string? RefundStatus { get; set; }  

    [JsonPropertyName("refund_time")]
    public string? RefundTime { get; set; }  

    [JsonPropertyName("trans_order_id")]
    public string? TransOrderId { get; set; } 
}