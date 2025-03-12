using System.Text.Json.Serialization;
 

namespace AsiaPayPaymentSDK.AsiaPay.PayToken;

public class BizContentPayTokenResponse
{
    [JsonPropertyName("pay_token")]
    public string? PayToken { get; set; } 

    [JsonPropertyName("effect_time")]
    public string? EffectTime { get; set; }

    [JsonPropertyName("expire_time")]
    public string? ExpireTime { get; set; }

    [JsonPropertyName("max_auth_times")]
    public string? MaxAuthTimes { get; set; }


}