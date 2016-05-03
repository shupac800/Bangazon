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
            Console.WriteLine("******************************************************");
            Console.WriteLine("** Welcome to Bangazon Command Line Ordering System **");
            Console.WriteLine("******************************************************");
            List<string> displayList = new List<string>
                { "Create an Account",
                  "Create a Payment Option",
                  "Order a Product",
                  "Complete an Order",
                  "See Product Popularity",
                  "Leave Bangazon" };
            InputOutput.displayMenu(displayList);
            switch (InputOutput.getChoice())
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



        static List<Customer> loadCustomerList()
        {
            List<Customer> customerList = new List<Customer>();

            string query = @"SELECT c.CustomerId,c.FirstName,c.LastName,c.Address1,c.Address2,c.City,c.State,c.ZipCode,c.Phone FROM Customer c";
            using (SqlConnection connection = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB; AttachDbFilename=\"C:\\Users\\shu\\workspace\\cs\\Bangazon\\Bangazon\\Bangazon.mdf\"; Integrated Security= True"))
            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    // Check if the reader has any rows at all before starting to read.
                    if (reader.HasRows)
                    {
                        // Read advances to the next row.
                        int index = 0;
                        while (reader.Read())
                        {
                            customerList.Add(new Customer(index, reader[0] as string, reader[1] as string, reader[2] as string, reader[3] as string, reader[4] as string, reader[5] as string, reader[6] as string, reader[7] as string, reader[8] as string));
                            index++;
                        }
                    }
                }
            }
            return customerList;
        }

        static int dispAndInputCustomerList(List<Customer> customerList)
        { 
            foreach (Customer c in customerList)
            {
                Console.WriteLine("{0}. {1} {2}", c.listIndex + 1, c.FirstName, c.LastName);
            }
            Console.Write("> ");
            string customerIndexChosen = Console.ReadLine();
            return Int32.Parse(customerIndexChosen) - 1;
        }
    }
}
