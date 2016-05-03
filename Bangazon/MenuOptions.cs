using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bangazon
{
    public class MenuOptions
    {
        public static List<Customer> AddCustomer()
        {
            Console.Write("\nEnter new customer ID\n> ");
            string custId = Console.ReadLine();
            Console.Write("\nEnter customer name\n> ");
            string cname = Console.ReadLine();
            Console.Write("\nEnter street address\n> ");
            string addr1 = Console.ReadLine();
            Console.Write("\nEnter city\n> ");
            string city = Console.ReadLine();
            Console.Write("\nEnter state\n> ");
            string state = Console.ReadLine();
            Console.Write("\nEnter postal code\n> ");
            string zip = Console.ReadLine();
            Console.Write("\nEnter phone number\n> ");
            string phone = Console.ReadLine();

            StringBuilder command = new StringBuilder();
            command.Append("INSERT INTO Customer ");
            command.Append("(CustomerId, FirstName, LastName, Address1, City, State, ZipCode, Phone) ");
            command.Append("VALUES (");
            command.Append("'" + custId + "',");
            command.Append("'" + cname.Split(' ')[0] + "',");
            command.Append("'" + cname.Split(' ')[1] + "',");
            command.Append("'" + addr1 + "',");
            command.Append("'" + city + "',");
            command.Append("'" + state + "',");
            command.Append("'" + zip + "',");
            command.Append("'" + phone + "'");
            command.Append(")");

            DatabaseOps.executeNonQuery(command.ToString());  // do SQL INSERT
            Console.WriteLine("Added new customer");
            return DatabaseOps.loadCustomers();  // return list that includes newly added customer
        }

        public static void AddPaymentType()
        {
            List<Customer> customerList = DatabaseOps.loadCustomers();
            Console.WriteLine("Which customer?");
            // better: instead of loop, use LINQ to create display list
            List<string> displayList = new List<string>();
            foreach (Customer c in customerList)
            {
                displayList.Add(c.FirstName + c.LastName);
            }
            IO.displayMenu(displayList);
            int customerIndexChosen = IO.getChoice();
            string customerIdChosen = customerList[customerIndexChosen].CustomerId;

            List<PaymentType> paymentTypeList = DatabaseOps.loadPaymentTypes();
            Console.Write("\nEnter payment type:\n");
            // better: instead of loop, use LINQ to create display list
            displayList = new List<String>();
            foreach (PaymentType pt in paymentTypeList)
            {
                displayList.Add(pt.name);
            }
            IO.displayMenu(displayList);
            int paymentTypeIndexChosen = IO.getChoice();
            int paymentTypeIdChosen = paymentTypeList[paymentTypeIndexChosen].paymentTypeId;

            Console.Write("\nEnter account number:\n> ");
            string account = Console.ReadLine();

            StringBuilder command = new StringBuilder();
            command.Append("INSERT INTO PaymentTypesAvailable ");
            command.Append("(customerId, paymentTypeId, account) ");
            command.Append("VALUES ('" + customerIdChosen + "', " + paymentTypeIdChosen + ", '" + account + "')");

            DatabaseOps.executeNonQuery(command.ToString());  // do SQL INSERT
            Console.WriteLine("Added payment type");
        }

        public static List<Product> ChooseProducts(List<Product> productList, List<Product> lineItems)
        {
            PickAProduct:
            Console.WriteLine("Which product?");
            // better: instead of loop, use LINQ to create display list
            List<string> displayList = new List<string>();
            foreach (Product p in productList)
            {
                displayList.Add(p.name);
            }
            IO.displayMenu(displayList);
            Console.WriteLine("9. Back to Main Menu");
            int productIndexChosen = IO.getChoice();
            if (productIndexChosen == 8) return lineItems;
            lineItems.Add(productList[productIndexChosen]);
            Console.WriteLine("Added product index {0}: {1}", productIndexChosen + 1, productList[productIndexChosen].name);
            goto PickAProduct;
        }

        public static List<Product> CloseOrder(List<Product> lineItems, List<Customer> customerList)
        {
            if (lineItems.Count == 0)
            {
                Console.WriteLine("Please add some products to your order first. Press enter to return to main menu.");
                Console.ReadLine();
                return lineItems;
            }

            decimal totalPrice = 0;
            foreach (Product p in lineItems)
            {
                totalPrice += p.price;
            }
            Console.WriteLine("Your order total is ${0:0.00}. Ready to purchase", totalPrice);
            Console.Write("(Y/N)? ");
            string yesOrNo = Console.ReadLine();
            if (yesOrNo == "N" || yesOrNo == "n") return lineItems;

            // get customerId
            Console.WriteLine("\nWhich customer is placing the order?");
            // better: instead of loop, use LINQ to create display list
            List<string> displayListC = new List<string>();
            foreach (Customer c in customerList)
            {
                displayListC.Add(c.FirstName + c.LastName);
            }
            IO.displayMenu(displayListC);
            int customerIndexChosen = IO.getChoice();
            string customerIdChosen = customerList[customerIndexChosen].CustomerId;

            // get paymentTypes available for this customer
            List<PaymentType> paymentTypesAvailable = DatabaseOps.loadPaymentTypesAvailable(customerIdChosen);
            Console.WriteLine("Which payment type?");
            // better: instead of loop, use LINQ to create display list
            List<string> displayListPTA = new List<string>();
            foreach (PaymentType pt in paymentTypesAvailable)
            {
                displayListPTA.Add(pt.name);
            }
            IO.displayMenu(displayListPTA);
            int paymentTypeIndexChosen = IO.getChoice();
            int paymentTypeIdChosen = paymentTypesAvailable[paymentTypeIndexChosen].paymentTypeId;

            DatabaseOps.createOrder(customerIdChosen, paymentTypeIdChosen, lineItems);
            Console.WriteLine("Invoice added.\n");
            lineItems = new List<Product>(); // clear lineItems list
            return lineItems;
        }

        public static void ReportPopularProducts(List<Product> productList)
        {
            Console.WriteLine("\n** Product Popularity and Revenue **\n");
            List<string> report = DatabaseOps.getPopularProducts(productList);
            foreach (string s in report)
            {
                Console.WriteLine(s);
            }
            Console.WriteLine("\nPress enter to return to main menu.");
            Console.ReadLine();
        }
    }
}
