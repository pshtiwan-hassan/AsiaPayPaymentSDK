using System.Text.Json.Serialization;

namespace AsiaPayPaymentSDK.AsiaPay.PreOrder;

public class BizContentPreOrderResponse
{
    [JsonPropertyName("merch_order_id")]
    public string? MerchOrderId { get; set; }

    [JsonPropertyName("prepay_id")]
    public string? PrepayId { get; set; }
}