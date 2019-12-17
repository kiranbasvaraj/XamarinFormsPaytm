using System;
using System.Collections.Generic;
using System.Text;

namespace PaytmTest.Models
{

    public class PostTransactionStatusModel
    {
        public string MID { get; set; }
        public string ORDERID { get; set; }
        public string CHECKSUMHASH { get; set; }
    }

}
