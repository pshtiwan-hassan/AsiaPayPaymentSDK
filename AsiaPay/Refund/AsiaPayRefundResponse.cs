using System.Text.Json.Serialization;

namespace AsiaPayPaymentSDK.AsiaPay.Refund;

public class AsiaPayRefundResponse
{
  

    [JsonPropertyName("biz_content")]
    public required BizContentRefundResponse BizContent { get; set; }

    [JsonPropertyName("code")]
    public string? Code { get; set; }

    [JsonPropertyName("msg")]
    public string? Message { get; set; }

    [JsonPropertyName("nonce_str")]
    public string? NonceStr { get; set; }

    [JsonPropertyName("result")]
    public string? Result { get; set; }

    [JsonPropertyName("sign")]
    public string? Sign { get; set; }

    [JsonPropertyName("sign_type")]
    public string? SignType { get; set; }
}