# AsiaPayPaymentSDK

[![NuGet](https://img.shields.io/nuget/v/AsiaPayPaymentSDK.svg)](https://www.nuget.org/packages/AsiaPayPaymentSDK/)
[![.NET Version](https://img.shields.io/badge/.NET-8.0-blue.svg)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/badge/license-MIT-green.svg)](LICENSE)

AsiaPayPaymentSDK is a **.NET 8 SDK** designed to simplify payment processing using the **AsiaPay API**.  
It provides seamless integration for merchants to handle **transactions, refunds, and order queries**.

---

## 📌 Features

✅ **Token-Based Authentication**  
✅ **Pre-Order Processing**  
✅ **Payment Token Handling**  
✅ **Order Queries & Refunds**  
✅ **QR Code Generation for Payments**  

---

## 🚀 Installation

### Using NuGet

Run the following command in the **.NET CLI**:

```sh
dotnet add package AsiaPayPaymentSDK --version 1.0.0
```

Or using **Package Manager Console (PMC)**:

```sh
Install-Package AsiaPayPaymentSDK -Version 1.0.0
```

---

## 🔥 Quick Start

### **1️⃣ Initialize `AsiaPayService`**

```csharp
public static class PaymentConstants
{
        public const string CheckUrl = "https://apitest.asiapay.iq:5443/payment/web/paygate";
        public const string ServerUrl = "https://apitest.asiapay.iq:5443/apiaccess";
        public const string CheckH5MidPageUrl = "https://apitest.asiapay.iq:5443/demo/middle_page/index.html?businessType=CrossAppPay";
        public const string AppSecret = "AppSecret";
        public const string XAppKey = "XAppKey";
        public const string AppId = "AppId";
        public const string? MerchCode = "MerchCode";
        public const string PrivateKey = "PrivateKey";
}

using AsiaPayPaymentSDK.AsiaPay;

var httpClient = new HttpClient();
var paymentService = new AsiaPayService(
    httpClient, 
    PaymentConstants.ServerUrl,
    PaymentConstants.CheckH5MidPageUrl, 
    PaymentConstants.AppSecret,
    PaymentConstants.XAppKey, 
    PaymentConstants.AppId, 
    PaymentConstants.MerchCode, 
    PaymentConstants.PrivateKey
);
```

### **2️⃣ Create an Order in AsiaPay**
```csharp
var timestamp = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
var nonceStr = Guid.NewGuid().ToString("N").ToLower();

var request = new AsiaPayPreOrderRequest
{
    Timestamp = timestamp,
    NonceStr = nonceStr,
    BizContent = new BizContentPreOrderRequest
    {
        NotifyUrl = "https://pshtiwan.free.beeceptor.com",
        RedirectUrl = "https://pshtiwan.free.beeceptor.com",
        AppId = PaymentConstants.AppId,
        MerchCode = PaymentConstants.MerchCode,
        MerchOrderId = Guid.NewGuid().ToString("N"),
        Title = "Tested By pshtiwan",
        TotalAmount = "500",
        TransCurrency = "IQD",
        TimeoutExpress = "5m"
    }
};

var createResponse = await paymentService.PreOrderAsync(request);

if (createResponse?.BizContent?.PrepayId == null)
{
    Console.WriteLine("Failed to create order.");
    return;
}

Console.WriteLine($"Order Created: {createResponse.BizContent.PrepayId}");
```

### **3️⃣ Generate Payment URL & QR Code**
```csharp
var generateQr = await paymentService.GetApplyPayTokenAsync(
    createResponse.BizContent.PrepayId, timestamp, nonceStr
);

string paymentUrl = $"https://pay.com?tradeType=PWA&appId={PaymentConstants.AppId}&merchCode={PaymentConstants.MerchCode}&prepayId={createResponse.BizContent.PrepayId}&payToken={generateQr.BizContent?.PayToken}";

Console.WriteLine($"Payment URL: {paymentUrl}");

Console.WriteLine("_________________________");

// Generate QR Code
Console.WriteLine("QrCode:data:image/png;base64," + paymentService.GenerateQrCodeBase64(paymentUrl));

Console.WriteLine("_________________________");

// Generate H5 Checkout URL
Console.WriteLine("OpenApp:" + paymentService.GenerateH5CheckoutUrl(createResponse.BizContent.PrepayId));
```

### **4️⃣ Query an Order**
```csharp
var queryOrder = await paymentService.QueryOrderAsync("52da1851e46c4017a7637f0249b007fe");
Console.WriteLine($"Order Details: {queryOrder}");
```

### **5️⃣ Process Refund**
```csharp
var refund = await paymentService.RefundOrderAsync("f8bfc6d157c74b32b99c1c434be51b3a");
Console.WriteLine($"Refund Response: {refund}");
```

--- 

## 🌎 Connect With Me
📧 **Email:** pshitiwan.eng@gmail.com  
💼 **LinkedIn:** [Pshitiwan Hassan](https://www.linkedin.com/in/pshtiwan-ahmed)  
🐙 **GitHub:** [pshtiwan-hassan](https://github.com/pshtiwan-hassan)  
