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
            List<Product> lineItems = new List<Product>();

            Main:
            Console.WriteLine("******************************************************");
            Console.WriteLine("** Welcome to Bangazon Command Line Ordering System **");
            Console.WriteLine("******************************************************");
            Console.WriteLine("1. Create an Account");
            Console.WriteLine("2. Create a Payment Option");
            Console.WriteLine("3. Order a Product");
            Console.WriteLine("4. Complete an Order");
            Console.WriteLine("5. See Product Popularity");
            Console.WriteLine("6. Leave Bangazon!");
            Console.Write("Your choice: ");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
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
                    command.Append("'"  + custId + "',");
                    command.Append("'" + cname.Split(' ')[0] + "',");
                    command.Append("'" + cname.Split(' ')[1] + "',");
                    command.Append("'" + addr1 + "',");
                    command.Append("'" + city + "',");
                    command.Append("'" + state + "',");
                    command.Append("'" + zip + "',");
                    command.Append("'" + phone + "'");
                    command.Append(")");
                    doSqlNonQuery(command.ToString());
                    break;
                case "2":
                    List<Customer> customerList = new List<Customer>();
                    Console.WriteLine("Which customer?");
                    customerList = loadCustomerList();
                    int customerIndexChosen = dispAndInputCustomerList(customerList);
                    string customerIdChosen = customerList[customerIndexChosen].CustomerId;

                    List<PaymentType> paymentTypeList = new List<PaymentType>();
                    Console.Write("\nEnter payment type:\n");
                    string query2 = @"SELECT pt.paymentTypeId, pt.name FROM PaymentType pt";
                    using (SqlConnection connection = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB; AttachDbFilename=\"C:\\Users\\shu\\workspace\\cs\\Bangazon\\Bangazon\\Bangazon.mdf\"; Integrated Security= True"))
                    using (SqlCommand cmd = new SqlCommand(query2, connection))
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
                                    PaymentType pt = new PaymentType(reader[0] as int? ?? 0, reader[1] as string);
                                    paymentTypeList.Add(pt);
                                    Console.WriteLine("{0}. {1}", pt.paymentTypeId, pt.name);
                                }

                            }
                        }
                    }
                    Console.Write("> ");
                    int paymentTypeChosen = Int32.Parse(Console.ReadLine());
                    Console.Write("\nEnter account number:\n> ");
                    string account = Console.ReadLine();

                    StringBuilder command2 = new StringBuilder();
                    command2.Append("INSERT INTO PaymentTypesAvailable ");
                    command2.Append("(customerId, paymentType, account) ");
                    command2.Append("VALUES ('" + customerIdChosen + "', " + paymentTypeChosen + ", '" + account + "')");
                    doSqlNonQuery(command2.ToString());
                    break;
                case "3":
                    ChooseProduct:
                    List<Product> productList = new List<Product>();
                    Console.WriteLine("Which product?");
                    string query3 = @"SELECT p.productId, p.name, p.price FROM Product p";
                    using (SqlConnection connection = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB; AttachDbFilename=\"C:\\Users\\shu\\workspace\\cs\\Bangazon\\Bangazon\\Bangazon.mdf\"; Integrated Security= True"))
                    using (SqlCommand cmd = new SqlCommand(query3, connection))
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
                                    Product p = new Product(index, reader[0] as int? ?? 0, reader[1] as string, reader[2] as decimal? ?? 0);
                                    productList.Add(p);
                                    Console.WriteLine("{0}. {1} {2}", p.listIndex + 1, p.name, p.price);
                                    index++;
                                }
                            }
                        }
                    }
                    Console.WriteLine("9. Back to Main Menu");
                    Console.Write("> ");
                    int productIndexChosen = Int32.Parse(Console.ReadLine());
                    if (productIndexChosen == 9) break;
                    lineItems.Add(productList[productIndexChosen]);
                    Console.WriteLine("Added product index {0} {1}", productIndexChosen, productList[productIndexChosen].name);
                    goto ChooseProduct;
                case "4":
                    if (lineItems.Count == 0)
                    {
                        Console.WriteLine("Please add some products to your order first. Press any key to return to main menu.");
                        int foo = Console.Read();
                        goto Main;
                    }
                    // complete order
                    decimal totalPrice = 0;
                    foreach (Product p in lineItems)
                    {
                        totalPrice += p.price;
                    }
                    Console.WriteLine("Your order total is ${0:0.00}. Ready to purchase",totalPrice);
                    Console.Write("(Y/N)? ");
                    string yesOrNo = Console.ReadLine();
                    if (yesOrNo == "N" || yesOrNo == "n") break;

                    List<Customer> customerList2 = new List<Customer>();
                    Console.WriteLine("\nWhich customer is placing the order?");
                    customerList2 = loadCustomerList();
                    int customerIndexChosen2 = dispAndInputCustomerList(customerList2);
                    string customerIdChosen2 = customerList2[customerIndexChosen2].CustomerId;

                    // get payment type

                    Console.WriteLine("Creating order...");

                    // determine next Invoice ID number
                    int maxInvoiceId = 0;
                    string query4 = @"SELECT MAX invoiceId FROM Invoices";
                    using (SqlConnection connection = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB; AttachDbFilename=\"C:\\Users\\shu\\workspace\\cs\\Bangazon\\Bangazon\\Bangazon.mdf\"; Integrated Security= True"))
                    using (SqlCommand cmd = new SqlCommand(query4, connection))
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
                                    maxInvoiceId = reader[0] as int? ?? 0;
                                }
                            }
                        }
                    }

                    // add new row to Invoices table
                    StringBuilder command5 = new StringBuilder();
                    command5.Append("INSERT INTO Invoices ");
                    command5.Append("(invoiceId, customerId) ");
                    command5.Append("VALUES (");
                    command5.Append((maxInvoiceId + 1 ).ToString() + ", ");
                    command5.Append(customerIdChosen2.ToString());
                    command5.Append(")");
                    doSqlNonQuery(command5.ToString());

                    // add new rows to LineItems table
                    foreach (Product p in lineItems)
                    {
                        StringBuilder command4 = new StringBuilder();
                        command4.Append("INSERT INTO LineItems");
                        command4.Append("(invoiceId, productId) ");
                        command4.Append("VALUES (");
                        command4.Append((maxInvoiceId + 1).ToString() + ",");
                        command4.Append(p.productId);
                        command4.Append(")");
                        doSqlNonQuery(command4.ToString());
                    }
                    Console.WriteLine("Invoice added.");
                    break;
                case "5":
                    Console.WriteLine("Not yet implemented");
                    break;
                case "6":
                    Console.WriteLine("See ya");
                    break;
                default:
                    break;
            }
            goto Main;
        }

        static int doSqlNonQuery(string command)
        {
            System.Data.SqlClient.SqlConnection sqlConnection1 =
                new System.Data.SqlClient.SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB; AttachDbFilename=\"C:\\Users\\shu\\workspace\\cs\\Bangazon\\Bangazon\\Bangazon.mdf\"; Integrated Security= True");

            System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = command;
            cmd.Connection = sqlConnection1;

            sqlConnection1.Open();
            cmd.ExecuteNonQuery();
            sqlConnection1.Close();

            return 0;
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
                Console.WriteLine("{0} {1} {2}", c.listIndex, c.FirstName, c.LastName);
            }
            Console.Write("> ");
            string customerIndexChosen = Console.ReadLine();
            return Int32.Parse(customerIndexChosen);
        }
    }
}
