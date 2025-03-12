using System.Text.Json.Serialization;

namespace AsiaPayPaymentSDK.AsiaPay.QueryOrder
{
    public class AsiaPayQueryOrderRequest
    {
        [JsonPropertyName("biz_content")]
        public BizContentQueryOrderRequest? BizContent { get; set; }

        [JsonPropertyName("method")]
        public string Method { get; set; } = "payment.queryorder";

        [JsonPropertyName("nonce_str")]
        public string? NonceStr { get; set; }

        [JsonPropertyName("sign")]
        public string? Sign { get; set; }

        [JsonPropertyName("sign_type")]
        public string SignType { get; set; } = "SHA256WithRSA";

        [JsonPropertyName("timestamp")]
        public string? Timestamp { get; set; }

        [JsonPropertyName("version")]
        public string Version { get; set; } = "1.0";
    }
}
