using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bangazon
{
    public class PaymentType
    {
        public int paymentTypeId { get; set; }
        public string name { get; set; }
        public string account { get; set; }

        // constructor
        public PaymentType(int paymentTypeId, string name)
        {
            this.paymentTypeId = paymentTypeId;
            this.name = name;
        }
    }
}
