using System.Text.Json.Serialization;

namespace AsiaPayPaymentSDK.AsiaPay.PayToken;

public class AsiaPayApplyPayTokenRequest
{
    [JsonPropertyName("timestamp")]
    public string? Timestamp { get; set; }

    [JsonPropertyName("method")]
    public string Method { get; set; } = "payment.applypaytoken";

    [JsonPropertyName("nonce_str")]
    public string? NonceStr { get; set; }

    [JsonPropertyName("sign_type")]
    public string SignType { get; set; } = "SHA256WithRSA";

    [JsonPropertyName("version")]
    public string Version { get; set; } = "1.0";

    [JsonPropertyName("sign")]
    public string? Sign { get; set; }

    [JsonPropertyName("biz_content")]
    public BizContentApplyPayToken? BizContent { get; set; }
}