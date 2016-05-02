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
            List<string> customer = new List<string>();

            List<string> lineItems = new List<string>();

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
                    doSql(command.ToString());
                    break;
                case "2":
                    List<Customer> customerList = new List<Customer>();
                    Console.WriteLine("Which customer?");
                    string query = @"
SELECT
  c.CustomerId,c.FirstName,c.LastName,c.Address1,c.Address2,c.City,c.State,c.ZipCode,c.Phone
FROM Customer c
";

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
                                while (reader.Read())
                                {
                                    customerList.Add(new Customer(reader[0].ToString(),reader[1].ToString(),reader[2].ToString(),reader[3].ToString(),reader[4].ToString(),reader[5].ToString(),reader[6].ToString(),reader[7].ToString(),reader[8].ToString()));
                                    // customer.Add(reader[0],reader[1],reader[2],reader[3],reader[4],reader[5],reader[6],reader[7],reader[8]);
                                }

                            }
                        }
                    }
                    foreach (Customer c in customerList)
                    {
                        Console.WriteLine("{0} {1} {2}", c.CustomerId, c.FirstName, c.LastName);
                    }
                    Console.Write("> ");
                    string customerIdChosen = Console.ReadLine();

                    List<PaymentType> paymentTypeList = new List<PaymentType>();
                    Console.Write("\nEnter payment type:\n");
                    string query2 = @"
SELECT
  pt.paymentTypeId, pt.name
FROM PaymentType pt
";

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
                                    paymentTypeList.Add(new PaymentType(reader[0] as int? ?? 0,reader[1] as string));
                                    // customer.Add(reader[0],reader[1],reader[2],reader[3],reader[4],reader[5],reader[6],reader[7],reader[8]);
                                }

                            }
                        }
                    }
                    foreach (PaymentType pt in paymentTypeList)
                    {
                        Console.WriteLine("{0}. {1}", pt.paymentTypeId, pt.name);
                    }
                    Console.Write("> ");
                    string paymentTypeChosen = Console.ReadLine();
                    Console.Write("\nEnter account number:\n> ");
                    string account = Console.ReadLine();

                    StringBuilder command2 = new StringBuilder();
                    command2.Append("UPDATE Customer");
                    command2.Append(" SET PaymentTypeId = " + paymentTypeChosen + ",");
                    command2.Append(" Account = '" + account + "'");
                    command2.Append(" WHERE CustomerId = '" + customerIdChosen +"'");
                    Console.WriteLine("Built string: " + command2.ToString());
                    doSql(command2.ToString());
                    break;
                case "3":
                    List<string> product = new List<string>();
                    product.Add("Fabspeed exhaust header");
                    product.Add("Alcantara sport steering wheel");
                    product.Add("ShupacTech high-flow air filter");
                    product.Add("Back to main menu");
                    int j = 0;
                    foreach (string p in product)
                    {
                        Console.WriteLine("{0}. {1}", j + 1, product[j]);
                        j++;
                    }
                    Console.Write("> ");
                    string productChosen = Console.ReadLine();
                    lineItems.Add(productChosen);
                    break;
                case "4":
                    if (lineItems.Count == 0)
                    {
                        Console.WriteLine("Please add some products to your order first. Press any key to return to main menu.");
                        ConsoleKeyInfo foo = Console.ReadKey();
                        Main();
                    }
                    // complete order
                    Main();
                    break;
                case "5":
                    break;
                case "6":
                    Console.WriteLine("See ya");
                    break;
                default:
                    break;
            }
        }

        static int doSql(string command)
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
    }
}
