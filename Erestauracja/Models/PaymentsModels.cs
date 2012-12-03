using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Erestauracja.Models
{
    public class PaymentsModels
    {
    }

    public class PayPal
    {
        public string cmd { get; set; }
        public string business { get; set; }
        public string no_shipping { get; set; }
        public string @return { get; set; }
        public string cancel_return { get; set; }
        public string notify_url { get; set; }
        public string currency_code { get; set; }
        public string item_number { get; set; }
        public string shipping { get; set; }
        public string amount { get; set; }
        public string mc_gross { get; set; }
        public string txn_id { get; set; }
        
    }

}