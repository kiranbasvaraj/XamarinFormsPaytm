using CCDLocationsV2.Services;
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
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        Helper helper;
        string Oid;
        public MainPage(string orderId)
        {
            InitializeComponent();
            helper = new Helper();
            Oid = orderId;
            helper.CreateWebView(orderId);
            web.Navigated += Web_Navigated;
        }

        //private void Web_Navigated(object sender, WebNavigatedEventArgs e)
        //{
        //    throw new NotImplementedException();
        //}

        private async void Web_Navigated(object sender, WebNavigatedEventArgs e)
        {
            var result = e.Result;
            Debug.WriteLine("result:" + result);
            var source = e.Source;
            Debug.WriteLine("result:" + source);
            var url = e.Url;
            Debug.WriteLine("result:" + url);
            var navigation = e.NavigationEvent;
            Debug.WriteLine("result:" + navigation);

            HttpClient httpClient = new HttpClient();

            Dictionary<String, String> paytmParams = new Dictionary<String, String>();
            paytmParams.Add("MID", "your mid");
            paytmParams.Add("ORDERID", "7675776e-559e-4e22-93f8-f4072fe71fac");
            String checksum = paytm.CheckSum.generateCheckSum("your merchent key", paytmParams);
            paytmParams.Add("CHECKSUMHASH", checksum);
            string paytmUrl = "https://securegw-stage.paytm.in/order/status";


            //  paytmParams.Add("CHECKSUMHASH", checksum);


            //    string paytmUrl = "https://securegw-stage.paytm.in/order/status";

            try
            {
                //var PayLoad = JsonConvert.SerializeObject(paytmParams);
                //var content = new StringContent(PayLoad, Encoding.UTF8, "application/json");
                ////await Task.Delay(10000);
                //var rees = await httpClient.PostAsync(paytmUrl, content);
                var res = await RestClient.RestClientInstance.PostAsync<PaytmModel>(paytmUrl, false, paytmParams);


                if (res.RESPCODE == "01")
                {
                    await DisplayAlert("Success", "Transaction Successful", "Ok");

                    // paytmUrl = "https://securegw.paytm.in/refund/apply";
                    string refundurl = "https://securegw-stage.paytm.in/refund/apply";

                    paytmParams.Clear();
                    paytmParams.Add("MID", "your mid");
                    paytmParams.Add("REFID", "RefundORD1");
                    paytmParams.Add("TXNID", res.TXNID);
                    paytmParams.Add("ORDERID", res.ORDERID);
                    paytmParams.Add("REFUNDAMOUNT", res.TXNAMOUNT);
                    paytmParams.Add("TXNTYPE", "REFUND");
                    paytmParams.Add("CHECKSUMHASH", checksum);

                    checksum = paytm.CheckSum.generateCheckSum(helper.merchantKey, paytmParams);

                    //PayLoad = JsonConvert.SerializeObject(paytmParams);
                    //content = new StringContent(PayLoad, Encoding.UTF8, "application/json");
                    var res2 = await RestClient.RestClientInstance.PostAsync<PaytmModel>(refundurl, false, paytmParams);
                    ////rees = await httpClient.PostAsync(paytmUrl, content);
                    //if (rees.IsSuccessStatusCode)
                    //{
                    //    resul = await rees.Content.ReadAsStringAsync();
                    //    await Navigation.PopAsync();
                    //}
                     if (res.STATUS == "TXN_FAILURE")
                    {
                        await DisplayAlert("Failure", "Transaction Failed", "Ok");
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            web.Source = helper.htmlSource;
        }

    }
}
