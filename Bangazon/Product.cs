using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bangazon
{
    public class Product
    {
        public int listIndex { get; set; }
        public int productId { get; set; }
        public string name { get; set; }
        public decimal price { get; set; }

        // constructor
        public Product(int listIndex, int productId, string name, decimal price)
        {
            this.listIndex = listIndex;
            this.productId = productId;
            this.name = name;
            this.price = price;
        }
    }
}
