using System.Text.Json.Serialization; 

namespace AsiaPayPaymentSDK.AsiaPay.PayToken
{
    public class BizContentApplyPayToken
    {
        [JsonPropertyName("appid")]
        public string? AppId { get; set; }
        [JsonPropertyName("trade_type")]
        public string? TradeType { get; set; } = "Checkout";
        [JsonPropertyName("referer_url")]
        public string RefererUrl { get; set; } = "https://pay.com";

        [JsonPropertyName("resource_type")]
        public string ResourceType { get; set; } = "PayToken";

        [JsonPropertyName("raw_request")]
        public string? RawRequest { get; set; }
    }
}
