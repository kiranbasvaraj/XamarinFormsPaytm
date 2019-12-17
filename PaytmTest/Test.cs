using Newtonsoft.Json;
using PaytmTest.Helpers;
using PaytmTest.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PaytmTest
{
   public class Test
    {
        private static void TestMth()
        {

            HttpClient httpClient = new HttpClient();

            Dictionary<String, String> paytmParams = new Dictionary<String, String>();
            paytmParams.Add("MID", "mSHsYo00906835990906");
            paytmParams.Add("ORDERID", "ORD1819002");//ORD16
            String checksum = paytm.CheckSum.generateCheckSum("NFFt60!A_H4evT!B", paytmParams);// t & NMHWtt@CrLpb1 &
           // paytmParams.Add("CHECKSUMHASH", checksum);
            //string paytmUrl = "https://securegw.paytm.in/order/status";

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

    }
}
