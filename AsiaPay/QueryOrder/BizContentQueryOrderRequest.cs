using System.Text.Json.Serialization;

namespace AsiaPayPaymentSDK.AsiaPay.QueryOrder;

public class BizContentQueryOrderRequest
{
    [JsonPropertyName("appid")]
    public required string AppId { get; set; } 

    [JsonPropertyName("merch_code")]
    public string? MerchCode { get; set; }  

    [JsonPropertyName("merch_order_id")]
    public string? MerchOrderId { get; set; } 
}