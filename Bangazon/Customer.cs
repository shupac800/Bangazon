using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bangazon
{
    public class Customer
    {
        public int listIndex { get; set; }
        public string CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }

        // constructor
        public Customer(int listIndex, string CustomerId, string FirstName, string LastName, string Address1, string Address2, string City, string State, string Zip, string Phone)
        {
            this.listIndex = listIndex;
            this.CustomerId = CustomerId;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.Address1 = Address1;
            this.Address2 = Address2;
            this.City = City;
            this.State = State;
            this.Zip = Zip;
            this.Phone = Phone;
        }
    }
}
