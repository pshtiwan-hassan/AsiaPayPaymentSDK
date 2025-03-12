using System.Text.Json.Serialization;

namespace AsiaPayPaymentSDK.AsiaPay.Refund
{
    public class AsiaPayRefundRequest
    {
       

        [JsonPropertyName("biz_content")]
        public required BizContentRefundRequest BizContent { get; set; }

        [JsonPropertyName("method")]
        public string Method { get; set; } = "payment.refund";

        [JsonPropertyName("nonce_str")]
        public required string? NonceStr { get; set; }

        [JsonPropertyName("sign")]
        public string? Sign { get; set; }

        [JsonPropertyName("sign_type")]
        public string SignType { get; set; } = "SHA256WithRSA";

        [JsonPropertyName("timestamp")]
        public required string? Timestamp { get; set; }

        [JsonPropertyName("version")]
        public string Version { get; set; } = "1.0";
    }
}
