using CCDLocationsV2.Services;
using Newtonsoft.Json;
using paytm;
using PaytmTest.Model;
using PaytmTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PaytmTest
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PayAmount : ContentPage
    {
        public PayAmount()
        {
            InitializeComponent();
            {

            }
        }
        private async Task InitTransaction()
        {

            Dictionary<String, String> TxnAmt = new Dictionary<String, String>();
            TxnAmt.Add("value", "10.00");
            TxnAmt.Add("currency", "INR");

            Dictionary<String, String> useInfo = new Dictionary<String, String>();
            useInfo.Add("custId", "ABCD001");
            //   /* var x = TxnAmt.ToString();*/ string s1 = string.Join(";", TxnAmt.Select(x => x.Key + "=" + x.Value).ToArray());
            ///*    var z = useInfo.ToString();*/ string s2 = string.Join(";", useInfo.Select(x => x.Key + "=" + x.Value).ToArray());
            ///

            var s1 = JsonConvert.SerializeObject(TxnAmt);
            var s2 = JsonConvert.SerializeObject(useInfo);
            Dictionary<String, String> paytmParams = new Dictionary<String, String>();
            paytmParams.Add("requestType", "Payment");
            paytmParams.Add("mid", "mSHsYo00906835990906");
            paytmParams.Add("orderId", "Order1922001");//ORD16
            paytmParams.Add("websiteName", "WEBSTAGING");
            paytmParams.Add("txnAmount", s1);
            paytmParams.Add("userInfo", s2);//ORD16
            paytmParams.Add("callbackUrl", "https://securegw.paytm.in/theia/paytmCallback?ORDER_ID=Order1922001");//ORD16


            String checksum = paytm.CheckSum.generateCheckSum("NFFt60!A_H4evT!B", paytmParams);
            InitTransactionModel payload = new InitTransactionModel();

            payload.head = new head()
            {
                signature = checksum
            };

            payload.body = new body()
            {
                requestType = "Payment",
                mid = "mSHsYo00906835990906",
                orderId = "Order1922001",
                websiteName = "WEBSTAGING",
                txnAmount = new TxnAmount() { currency = "INR", value = "2.00" },
                userInfo = new UserInfo() { custId = "ABCD001" },
                callbackUrl = "https://securegw.paytm.in/theia/paytmCallback?ORDER_ID=Order1922001"
            };

            await RestClient.RestClientInstance.PostAsync<string>("https://securegw-stage.paytm.in/theia/api/v1/initiateTransaction?mid=mSHsYo00906835990906&orderId=Order1922001", false, payload);
            // t & NMHWtt@CrLpb1 &
            // paytmParams.Add("CHECKSUMHASH", checksum);
            //try
            //{
            //    var PayLoad = JsonConvert.SerializeObject(paytmParams);
            //    var content = new StringContent(PayLoad, Encoding.UTF8, "application/json");

            //    var rees = await httpClient.PostAsync(paytmUrl, content);

            //    if (rees.IsSuccessStatusCode)
            //    {
            //        var resul = await rees.Content.ReadAsStringAsync();

            //        var res = JsonConvert.DeserializeObject<PaytmModel>(resul);//change model to paytm model

            //        if (res.RESPCODE == "01")
            //        {

            //            paytmUrl = "https://securegw.paytm.in/refund/apply";

            //            paytmParams.Clear();
            //            paytmParams.Add("MID", "aWvphG91697488499583");
            //            paytmParams.Add("REFID", "RefundORD1");
            //            paytmParams.Add("TXNID", res.TXNID);
            //            paytmParams.Add("ORDERID", res.ORDERID);
            //            paytmParams.Add("REFUNDAMOUNT", res.TXNAMOUNT);
            //            paytmParams.Add("TXNTYPE", "REFUND");
            //            paytmParams.Add("CHECKSUMHASH", checksum);

            //            checksum = paytm.CheckSum.generateCheckSum("t&NMHWtt@CrLpb1&", paytmParams);

            //            PayLoad = JsonConvert.SerializeObject(paytmParams);
            //            content = new StringContent(PayLoad, Encoding.UTF8, "application/json");

            //            rees = await httpClient.PostAsync(paytmUrl, content);
            //            if (rees.IsSuccessStatusCode)
            //            {
            //                resul = await rees.Content.ReadAsStringAsync();
            //                await Navigation.PopAsync();
            //            }

            //        }
            //        else if (res.STATUS == "TXN_FAILURE")
            //        {

            //        }
            //    }
            //}
            //catch (Exception ex)
            //{

            //}
        }
        private async void Button_Clicked(object sender, EventArgs e)
        {
            // await checksum();


            Guid orderid = Guid.NewGuid();
            await Navigation.PushAsync(new MainPage(orderid.ToString()));
        }

        public async Task checksum()
        {
            //Dictionary<String, String> paytmParams = new Dictionary<String, String>();
            //paytmParams.Add("MID", "mSHsYo00906835990906");
            //paytmParams.Add("ORDERID", "7675776e-559e-4e22-93f8-f4072fe71fac");//ORD16
            //String checksum = paytm.CheckSum.generateCheckSum("NFFt60!A_H4evT!B", paytmParams);// t & NMHWtt@CrLpb1 &


            Dictionary<String, String> paytmParams = new Dictionary<String, String>();
            paytmParams.Add("MID", "mSHsYo00906835990906");
            paytmParams.Add("ORDERID", "7675776e-559e-4e22-93f8-f4072fe71fac");
            String checksum = paytm.CheckSum.generateCheckSum("NFFt60!A_H4evT!B", paytmParams);
            paytmParams.Add("CHECKSUMHASH", checksum);
            string paytmUrl = "https://securegw-stage.paytm.in/order/status";
            await RestClient.RestClientInstance.PostAsync<string>(paytmUrl, false, paytmParams);
        }
    }
}