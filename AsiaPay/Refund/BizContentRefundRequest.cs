using System.Text.Json.Serialization;

namespace AsiaPayPaymentSDK.AsiaPay.Refund;

public class BizContentRefundRequest 
{
    [JsonPropertyName("appid")]
    public string? AppId { get; set; }  

    [JsonPropertyName("merch_code")]
    public string? MerchCode { get; set; }  

    [JsonPropertyName("merch_order_id")]
    public string? MerchOrderId { get; set; } 

    [JsonPropertyName("refund_reason")]
    public string? RefundReason { get; set; }  

    [JsonPropertyName("refund_request_no")]
    public string? RefundRequestNo { get; set; }  
}