using System.Text.Json.Serialization;

namespace AsiaPayPaymentSDK.AsiaPay.QueryOrder;

public class AsiaPayQueryOrderResponse
{
    [JsonPropertyName("errorCode")]
    public string? ErrorCode { get; set; }  

    [JsonPropertyName("errorMsg")]
    public string? ErrorMessage { get; set; }  
    [JsonPropertyName("msg")]
    public string? Message { get; set; }

    [JsonPropertyName("nonce_str")]
    public string? NonceStr { get; set; }

    [JsonPropertyName("sign")]
    public string? Sign { get; set; }

    [JsonPropertyName("sign_type")]
    public string? SignType { get; set; }

    [JsonPropertyName("biz_content")]
    public BizContentQueryOrderResponse? BizContent { get; set; }
}