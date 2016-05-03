using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bangazon
{
    public class Menu
    {
        public static void main()
        {
            List<Product> productList = DatabaseOps.loadProducts();
            List<Customer> customerList = DatabaseOps.loadCustomers();
            List<Product> lineItems = new List<Product>();

            MainLoop:
            Console.WriteLine("\n******************************************************");
            Console.WriteLine("** Welcome to Bangazon Command Line Ordering System **");
            Console.WriteLine("******************************************************");
            List<string> displayList = new List<string>
                    { "Create an Account",
                      "Create a Payment Option",
                      "Order a Product",
                      "Complete an Order",
                      "See Product Popularity",
                      "Leave Bangazon" };
            IO.displayMenu(displayList);
            switch (IO.getChoice())
            {
                case 0:
                    customerList = MenuOptions.AddCustomer();
                    break;
                case 1:
                    MenuOptions.AddPaymentType();
                    break;
                case 2:
                    lineItems = MenuOptions.ChooseProducts(productList, lineItems);
                    break;
                case 3:
                    lineItems = MenuOptions.CloseOrder(lineItems, customerList);
                    break;
                case 4:
                    MenuOptions.ReportPopularProducts(productList);
                    break;
                case 5:
                    goto End;
                default:
                    break;
            }
            goto MainLoop;
            End:
            Console.WriteLine("See ya");
        }
    }
}
