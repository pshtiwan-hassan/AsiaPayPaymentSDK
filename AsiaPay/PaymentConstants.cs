namespace AsiaPayPaymentSDK.AsiaPay
{
    public static class PaymentConstants
    {
        public const string CheckUrl = "https://apitest.asiapay.iq:5443/payment/web/paygate";
        public const string ServerUrl = "https://apitest.asiapay.iq:5443/apiaccess";
        public const string CheckH5MidPageUrl = "https://apitest.asiapay.iq:5443/demo/middle_page/index.html?businessType=CrossAppPay";
        
        // SECURITY WARNING: These should be loaded from secure configuration sources
        // such as environment variables, Azure Key Vault, or secure configuration files
        // DO NOT hardcode sensitive values in production code
        public const string AppSecret = "REPLACE_WITH_ENVIRONMENT_VARIABLE_OR_SECURE_CONFIG";
        public const string XAppKey = "REPLACE_WITH_ENVIRONMENT_VARIABLE_OR_SECURE_CONFIG";
        public const string AppId = "REPLACE_WITH_ENVIRONMENT_VARIABLE_OR_SECURE_CONFIG";
        public const string? MerchCode = "REPLACE_WITH_ENVIRONMENT_VARIABLE_OR_SECURE_CONFIG";
        public const string PrivateKey = "REPLACE_WITH_ENVIRONMENT_VARIABLE_OR_SECURE_CONFIG";
    }
}
