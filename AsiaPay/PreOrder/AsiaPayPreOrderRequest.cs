using System.Text.Json.Serialization;


namespace AsiaPayPaymentSDK.AsiaPay.PreOrder
{
    public class AsiaPayPreOrderRequest
    {
        [JsonPropertyName("timestamp")]
        public string? Timestamp { get; set; }

        [JsonPropertyName("method")]
        public string? Method { get; set; } = "payment.preorder";

        [JsonPropertyName("nonce_str")]
        public string? NonceStr { get; set; }

        [JsonPropertyName("sign_type")]
        public string? SignType { get; set; } = "SHA256WithRSA";

        [JsonPropertyName("version")]
        public string? Version { get; set; } = "1.0";

        [JsonPropertyName("biz_content")]
        public BizContentPreOrderRequest BizContent { get; set; } = new BizContentPreOrderRequest();

        [JsonPropertyName("sign")]
        public string? Sign { get; set; }
    }
}
