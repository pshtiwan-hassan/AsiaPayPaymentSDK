using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using AsiaPayPaymentSDK.AsiaPay.Auth;
using AsiaPayPaymentSDK.AsiaPay.PayToken;
using AsiaPayPaymentSDK.AsiaPay.PreOrder;
using AsiaPayPaymentSDK.AsiaPay.QueryOrder;
using AsiaPayPaymentSDK.AsiaPay.Refund;
using QRCoder;

namespace AsiaPayPaymentSDK.AsiaPay
{
    public class AsiaPayService(
        HttpClient httpClient,
        string baseUrl,
        string CheckH5MidPageUrl,
        string appSecret,
        string appKey,
        string appId,
        string? merchCode,
        string privateKey)
    {
        private string? _cachedToken;
        private DateTime _tokenExpiration = DateTime.MinValue;

        public async Task<AsiaPayAuthorizationResponse?> GetAccessTokenAsync()
        {
            if (!string.IsNullOrEmpty(_cachedToken) && DateTime.UtcNow < _tokenExpiration)
                return new AsiaPayAuthorizationResponse { Token = _cachedToken.Replace("Bearer ", "") };

            var url = $"{baseUrl}/payment/gateway/payment/v1/token";
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(JsonSerializer.Serialize(new { appSecret = appSecret }), Encoding.UTF8, "application/json")
            };

            request.Headers.Add("X-APP-Key", appKey);
            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonSerializer.Deserialize<AsiaPayAuthorizationResponse>(responseBody);

            if (tokenResponse?.Token != null)
            {
                _cachedToken = tokenResponse.Token;
                _tokenExpiration = DateTime.ParseExact(tokenResponse.ExpirationDate!, "yyyyMMddHHmmss", null);
            }

            return tokenResponse;
        }


        private async Task<string> MakeAuthorizedRequestAsync(HttpMethod method, string url, object? data = null)
        {
            for (int attempt = 1; attempt <= 2; attempt++)
            {
                try
                {
                    var tokenResponse = await GetAccessTokenAsync();
                    if (string.IsNullOrEmpty(tokenResponse?.Token))
                        throw new UnauthorizedAccessException("Unable to acquire access token");

                    var request = new HttpRequestMessage(method, url)
                    {
                        Content = data == null ? null : new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json")
                    };

                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.Token.Replace("Bearer ", ""));
                    request.Headers.Add("X-APP-Key", appKey);

                    LogRequest(request, data);
                    var response = await httpClient.SendAsync(request);

                    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized && attempt == 1)
                    {
                        Console.WriteLine("Access token expired. Regenerating token...");
                        _cachedToken = null;
                        continue;
                    }

                    if (!response.IsSuccessStatusCode)
                    {
                        string errorResponse = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"Error Response ({response.StatusCode}): {errorResponse}");

                        // Return error details instead of throwing
                        return errorResponse;
                    }

                    return await response.Content.ReadAsStringAsync();
                }
                catch (HttpRequestException httpEx)
                {
                    Console.WriteLine($"HTTP Request Exception: {httpEx.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unexpected Exception: {ex.Message}");
                }
            }

            return JsonSerializer.Serialize(new { StatusCode = 500, ErrorMessage = "Failed to process request after retries." });
        }



        // Helper Method for Logging the Request
        private static void LogRequest(HttpRequestMessage request, object? data)
        {
            Console.WriteLine($"------ HTTP Request ({request.Method}) ------");
            Console.WriteLine($"URL: {request.RequestUri}");
            foreach (var header in request.Headers)
                Console.WriteLine($"  {header.Key}: {string.Join(", ", header.Value)}");

            if (data != null)
                Console.WriteLine("Content: " + JsonSerializer.Serialize(data ));

            Console.WriteLine("--------------------------------------------");
        }
        public async Task<AsiaPayPreOrderResponse?> PreOrderAsync(AsiaPayPreOrderRequest request)
        {

           
            // Generate signature
            var parameters = new Dictionary<string, string?>
            {
                { "appid", request.BizContent.AppId },
                { "merch_code", request.BizContent.MerchCode },
                { "merch_order_id", request.BizContent.MerchOrderId },
                { "method",request.Method },
                { "nonce_str", request.NonceStr },
                { "notify_url", request.BizContent.NotifyUrl },
                { "redirect_url", request.BizContent.RedirectUrl },
                { "timeout_express", request.BizContent.TimeoutExpress },
                { "timestamp", request.Timestamp },
                { "title", request.BizContent.Title },
                { "total_amount", request.BizContent.TotalAmount },
                { "trade_type", request.BizContent.TradeType },
                { "trans_currency", request.BizContent.TransCurrency },
                { "version", request.Version }
            };
            string signSourceString = PrepareSign(parameters);
            Console.WriteLine("Sign Source String: " + signSourceString);

            request.Sign = GenerateSignature(signSourceString, privateKey);


            // Send the request
            var responseJson = await MakeAuthorizedRequestAsync(HttpMethod.Post, $"{baseUrl}/payment/gateway/payment/v1/merchant/preOrder", request);

            var response = JsonSerializer.Deserialize<AsiaPayPreOrderResponse>(responseJson);
            //response.Timestamp = request.Timestamp;
            return response;
        }
      

        public async Task<AsiaPayApplyPayTokenResponse> GetApplyPayTokenAsync(string? prepayId, string? timestamp, string? nonceStr)
        { 

            var request = new AsiaPayApplyPayTokenRequest
            {
                Timestamp = timestamp,
                NonceStr = nonceStr,
                SignType = "SHA256WithRSA",
                Version = "1.0",
                BizContent = new BizContentApplyPayToken
                {
                    AppId = appId,
                    TradeType = "Checkout",
                    RefererUrl = "https://pay.com",
                    ResourceType = "PayToken",
                    RawRequest = GenerateRawRequest(nonceStr, timestamp, prepayId)
                }
            };

            var parameters = new Dictionary<string, string?>
            {
                { "appid", appId },
                { "method","payment.applypaytoken"},
                { "nonce_str", nonceStr },
                { "referer_url", "https://pay.com" },
                { "resource_type", "PayToken" },
                { "timestamp", timestamp },
                { "trade_type", "Checkout" },
                { "version", "1.0" }
            };


            request.Sign = GenerateSignature(PrepareSign(parameters), privateKey);

            string responseJson = await MakeAuthorizedRequestAsync(HttpMethod.Post, $"{baseUrl}/payment/gateway/payment/v1/auth/applyPayToken", request);
            var response = JsonSerializer.Deserialize<AsiaPayApplyPayTokenResponse>(responseJson);

            if (response?.BizContent?.PayToken == null)
                throw new Exception("Failed to retrieve pay token from response");


            Console.WriteLine("Apply Pay Token Response: " + responseJson);

            return response;
        }

        private string GenerateRawRequest(string? nonceStr, string? timestamp, string? prepayId)
        {
            var parameters = new Dictionary<string, string?>
            {
                { "appid", appId },
                { "merch_code", merchCode },
                { "nonce_str", nonceStr ?? string.Empty },
                { "prepay_id", prepayId },
                { "timestamp", timestamp ?? string.Empty }
            };

            string sign = GenerateSignature(PrepareSign(parameters), privateKey);
            parameters.Add("sign", sign);
            parameters.Add("sign_type", "SHA256WithRSA");

            return string.Join("&", parameters.Select(kvp => $"{kvp.Key}={kvp.Value}"));
        }

        public string GenerateQrCodeBase64(string text)
        {
            using QRCodeGenerator qrGenerator = new();
            using QRCodeData qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.M);
            using PngByteQRCode qrCode = new(qrCodeData);

            byte[] qrBytes = qrCode.GetGraphic(3);
            return Convert.ToBase64String(qrBytes);
        }


        public async Task<AsiaPayRefundResponse?> RefundOrderAsync(string? merchOrderId)
        {
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
            var nonceStr = Guid.NewGuid().ToString("N").ToLower();

            var request = new AsiaPayRefundRequest
            {
                Timestamp = timestamp,
                NonceStr = nonceStr,
                SignType = "SHA256WithRSA",
                Version = "1.0",
                BizContent = new BizContentRefundRequest
                {
                    AppId = appId,
                    MerchCode = merchCode,
                    MerchOrderId = merchOrderId,
                    RefundReason = "forTesting",
                    RefundRequestNo = timestamp
                }
            };
            var parameters = new Dictionary<string, string?>
            {
                { "appid", appId },
                { "merch_code", merchCode },
                { "merch_order_id", merchOrderId }      ,
                {"method","payment.refund"},
                { "nonce_str", nonceStr },
                { "refund_reason", "forTesting" },
                { "refund_request_no", timestamp },
                { "timestamp", timestamp },
                { "version", "1.0" }
            };



            request.Sign = GenerateSignature(PrepareSign(parameters), privateKey);

            string responseJson = await MakeAuthorizedRequestAsync(HttpMethod.Post, $"{baseUrl}/payment/gateway/payment/v1/merchant/refund", request);
            var response = JsonSerializer.Deserialize<AsiaPayRefundResponse>(responseJson);

            return response;
        }

     

        public async Task<AsiaPayQueryOrderResponse?> QueryOrderAsync(string? merchOrderId)
        {
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
            var nonceStr = Guid.NewGuid().ToString("N").ToLower();


            var parameters = new Dictionary<string, string?>
            {
                { "appid", appId },
                { "merch_code", merchCode },
                { "merch_order_id", merchOrderId },
                { "method","payment.queryorder" },
                { "nonce_str", nonceStr },
                { "timestamp", timestamp },
                { "version", "1.0" }
            };

            var prepareSign = PrepareSign(parameters);

            var request = new AsiaPayQueryOrderRequest
            {
                Timestamp = timestamp,
                NonceStr = nonceStr,
                SignType = "SHA256WithRSA",
                Version = "1.0",
                BizContent = new BizContentQueryOrderRequest
                {
                    AppId = appId,
                    MerchCode = merchCode,
                    MerchOrderId = merchOrderId
                },
                Sign = GenerateSignature(prepareSign, privateKey)
            };

            string responseJson = await MakeAuthorizedRequestAsync(HttpMethod.Post, $"{baseUrl}/payment/gateway/payment/v1/merchant/queryOrder", request);
            var response = JsonSerializer.Deserialize<AsiaPayQueryOrderResponse>(responseJson);

            return response;
        }

       
        public string GenerateH5CheckoutUrl(string prepayId)
        {
            string signSourceString = $"appid={appId}&merch_code={merchCode}&prepay_id={prepayId}";
            string signValue = GenerateSignature(signSourceString, privateKey);

            string rowRequest = $"{signSourceString}&sign={signValue}&sign_type=SHA256WithRSA";
            string encodeRowRequest = HttpUtility.UrlEncode(rowRequest);

            var appCheckOutData = new
            {
                type = "PhoneH5",
                schemaAndroid = "asiapay://h5checkout",
                //schemaIOS = "asiapay://h5checkout",
                middlePageUrl = $"{CheckH5MidPageUrl}",
                checkOutStr = $"tradeType=Cross-App&rawRequest={encodeRowRequest}"
            };

            string appCheckOutString = JsonSerializer.Serialize(appCheckOutData);
            string appCheckOutStringBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(appCheckOutString));

            string h5Url = $"{CheckH5MidPageUrl}&params={appCheckOutStringBase64}";
            return h5Url;
        }


        private static string PrepareSign(Dictionary<string, string?> parameters)
        {
            //parameters.Add("method", method);
            return string.Join("&", parameters.Select(kvp => $"{kvp.Key}={kvp.Value}"));
        }


        private  static string GenerateSignature(string data, string privateKeyContent)
        {
            // Clean the private key content
            privateKeyContent = privateKeyContent
                .Replace("-----BEGIN PRIVATE KEY-----", "")
                .Replace("-----END PRIVATE KEY-----", "")
                .Replace("\n", "")
                .Replace("\r", ""); // Ensure all newline characters are removed

            // Convert data and private key to byte arrays
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            byte[] keyBytes = Convert.FromBase64String(privateKeyContent);

            // Import the private key and generate the signature
            using var rsa = RSA.Create();
            rsa.ImportPkcs8PrivateKey(keyBytes, out _);

            byte[] signedData = rsa.SignData(dataBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pss);
            // Return the signature as a Base64-encoded string
            return Convert.ToBase64String(signedData);
        }

    }
}
