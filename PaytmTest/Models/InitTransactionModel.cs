using System;
using System.Collections.Generic;
using System.Text;

namespace PaytmTest.Model
{
    public class TxnAmount
    {
        public string value { get; set; }
        public string currency { get; set; }
    }

    public class UserInfo
    {
        public string custId { get; set; }
    }

    public class body
    {
        public string requestType { get; set; }
        public string mid { get; set; }
        public string orderId { get; set; }
        public string websiteName { get; set; }
        public TxnAmount txnAmount { get; set; }
        public UserInfo userInfo { get; set; }
        public string callbackUrl { get; set; }
    }

    public class head
    {
        public string signature { get; set; }
    }

    public class InitTransactionModel
    {
        public body body { get; set; }
        public head head { get; set; }
    }

}
