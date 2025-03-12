

using System.Text.Json.Serialization;

namespace AsiaPayPaymentSDK.AsiaPay.PreOrder
{

    public class BizContentPreOrderRequest
    {
        [JsonPropertyName("notify_url")]
        public string? NotifyUrl { get; set; }

        [JsonPropertyName("redirect_url")]
        public string? RedirectUrl { get; set; }

        [JsonPropertyName("appid")]
        public string? AppId { get; set; }

        [JsonPropertyName("merch_code")]
        public string? MerchCode { get; set; }

        [JsonPropertyName("merch_order_id")]
        public string? MerchOrderId { get; set; }

        [JsonPropertyName("trade_type")]
        public string? TradeType { get; set; } = "Checkout";

        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("total_amount")]
        public string? TotalAmount { get; set; }

        [JsonPropertyName("trans_currency")]
        public string? TransCurrency { get; set; }

        [JsonPropertyName("timeout_express")]
        public string? TimeoutExpress { get; set; }
    }
}
