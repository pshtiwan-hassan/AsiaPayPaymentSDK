using System.Text.Json.Serialization;


namespace AsiaPayPaymentSDK.AsiaPay.PreOrder
{
    public class AsiaPayPreOrderResponse
    {
        [JsonPropertyName("result")]
        public string? Result { get; set; }

        [JsonPropertyName("code")]
        public string? Code { get; set; }

        [JsonPropertyName("msg")]
        public string? Message { get; set; }

        [JsonPropertyName("nonce_str")]
        public string? NonceStr { get; set; }

        [JsonPropertyName("sign")]
        public string? Sign { get; set; }

        [JsonPropertyName("sign_type")]
        public string? SignType { get; set; }

        [JsonPropertyName("biz_content")]
        public BizContentPreOrderResponse? BizContent { get; set; } 
        
        //public string Timestamp { get; set; }
    }
}
