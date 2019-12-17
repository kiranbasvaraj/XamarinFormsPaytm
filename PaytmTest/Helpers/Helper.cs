using paytm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms;

namespace PaytmTest.Helpers
{
    public class Helper
    {
        public HtmlWebViewSource htmlSource = new HtmlWebViewSource();
      public  String merchantKey = "your merchant key";
        public string CreateWebView(string orderId)
        {
            
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("MID", "your mid");
            parameters.Add("CHANNEL_ID", "WEB");
            parameters.Add("INDUSTRY_TYPE_ID", "Retail");
            parameters.Add("WEBSITE", "company");
            parameters.Add("EMAIL", "sample@abc.com");
            parameters.Add("MOBILE_NO", "");
            parameters.Add("CUST_ID", "cust1234");
            parameters.Add("ORDER_ID", orderId);
            parameters.Add("TXN_AMOUNT", "1");
            parameters.Add("CALLBACK_URL", "https://securegw.paytm.in/theia/paytmCallback?ORDER_ID="+ orderId); //This parameter is not mandatory. Use this to pass the callback url dynamically.

            string checksum = CheckSum.generateCheckSum(merchantKey, parameters);


            Debug.WriteLine("Check Sum:" + checksum);

            /* for Staging */
            String url = "https://securegw-stage.paytm.in/order/process";

            /* for Production */
            // String url = "https://securegw.paytm.in/order/process";

            /* Prepare HTML Form and Submit to Paytm */
            string outputHtml = "";
            outputHtml += "<html>";
            outputHtml += "<head>";
            outputHtml += "<title>Merchant Checkout Page</title>";
            outputHtml += "</head>";
            outputHtml += "<body>";
            outputHtml += "<center><h1>Please do not refresh this page...</h1></center>";
            outputHtml += "<form method='post' action='" + url + "' name='paytm_form'>";
            foreach (string key in parameters.Keys)
            {
                outputHtml += "<input type='hidden' name='" + key + "' value='" + parameters[key] + "'>";
            }
            outputHtml += "<input type='hidden' name='CHECKSUMHASH' value='" + checksum + "'>";
            outputHtml += "</form>";
            outputHtml += "<script type='text/javascript'>";
            outputHtml += "document.paytm_form.submit();";
            outputHtml += "</script>";
            outputHtml += "</body>";
            outputHtml += "</html>";
            htmlSource.Html = outputHtml;
            return outputHtml;
        }
    }
}
