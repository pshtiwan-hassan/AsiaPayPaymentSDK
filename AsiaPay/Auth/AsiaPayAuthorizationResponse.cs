 

using System.Text.Json.Serialization;

namespace AsiaPayPaymentSDK.AsiaPay.Auth
{
    public class AsiaPayAuthorizationResponse
    {
        [JsonPropertyName("token")]
        public string? Token { get; set; }

        [JsonPropertyName("effectiveDate")]
        public string? EffectiveDate { get; set; }

        [JsonPropertyName("expirationDate")]
        public string? ExpirationDate { get; set; }
    }
}
