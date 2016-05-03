using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bangazon
{
    public class PaymentType
    {
        public int listIndex { get; set; }
        public int paymentTypeId { get; set; }
        public string name { get; set; }

        // constructor
        public PaymentType(int listIndex, int paymentTypeId, string name)
        {
            this.listIndex = listIndex;
            this.paymentTypeId = paymentTypeId;
            this.name = name;
        }
    }
}
