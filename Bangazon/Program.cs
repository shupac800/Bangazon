using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Bangazon
{
    class Program
    {
        static void Main()
        {
            List<Product> productList = DatabaseOps.loadProducts();
            List<Customer> customerList = DatabaseOps.loadCustomers();
            List<Product> lineItems = new List<Product>();

            Main:
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
                    customerList = MainMenu.AddCustomer();
                    Console.WriteLine("Added new customer");
                    break;
                case 1:
                    MainMenu.AddPaymentType();
                    Console.WriteLine("Added payment type");
                    break;
                case 2:
                    lineItems = MainMenu.ChooseProducts(productList, lineItems);
                    break;
                case 3:
                    if (lineItems.Count == 0)
                    {
                        Console.WriteLine("Please add some products to your order first. Press enter to return to main menu.");
                        Console.ReadLine();
                        goto Main;
                    }
                    lineItems = MainMenu.CloseOrder(lineItems, customerList);
                    break;
                case 4:
                    // for each product, report how many were sold and how many customers bought it
                    foreach (Product p in productList)
                    {
                        string query7 = @"SELECT COUNT(DISTINCT y.foo) as customers, count(y.bar) as units
FROM (SELECT i.customerId as foo, li.productId as bar from Invoices i 
INNER JOIN LineItems li ON li.invoiceId = i.invoiceId WHERE li.productId = '" + p.productId + "') y";
                        using (SqlConnection connection = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB; AttachDbFilename=\"C:\\Users\\shu\\workspace\\cs\\Bangazon\\Bangazon\\Bangazon.mdf\"; Integrated Security= True"))
                        using (SqlCommand cmd = new SqlCommand(query7, connection))
                        {
                            connection.Open();
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                // Check if the reader has any rows at all before starting to read.
                                if (reader.HasRows)
                                {
                                    // Read advances to the next row.
                                    while (reader.Read())
                                    {
                                        int customersWhoBought = reader[0] as int? ?? 0;
                                        int unitsSold = reader[1] as int? ?? 0;
                                        Console.WriteLine("{0} ordered {1} times by {2} customers for total revenue of ${3:0.00}", p.name, unitsSold, customersWhoBought, unitsSold * p.price);
                                    }
                                }
                            }
                            connection.Close();
                        }
                    }
                    Console.WriteLine("Press enter to return to main menu.");
                    Console.ReadLine();
                    goto Main;
                case 5:
                    Console.WriteLine("See ya");
                    goto End;
                default:
                    break;
            }
            goto Main;
            End:
            byte dummy = 0;  // have to have something after "End" label
        }
    }
}
