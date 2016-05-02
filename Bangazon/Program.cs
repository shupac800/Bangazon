using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bangazon
{
    class Program
    {
        static void Main()
        {
            List<string> customer = new List<string>();
            customer.Add("Porky Pig");

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
                    Console.WriteLine("Built string: " + command);
                    doSql(command.ToString());
                    Console.WriteLine("should've worked");

                    break;
                case "2":
                    Console.WriteLine("Which customer?");
                    int i = 0;
                    foreach (string c in customer)
                    {
                        Console.WriteLine("{0}. {1}", i + 1, customer[i]);
                        i++;
                    }
                    Console.Write("> ");
                    string customerChosen = Console.ReadLine();
                    Console.Write("\nEnter payment type (e.g. AmEx, VISA, Checking)\n> ");
                    string paymentChosen = Console.ReadLine();
                    Console.Write("\nEnter account number\n> ");
                    string accountChosen = Console.ReadLine();
                    break;
                case "3":
                    List<string> product = new List<string>();
                    product.Add("Fabspeed exhaust header");
                    product.Add("Alcantara sport steering wheel");
                    product.Add("ShupacTech performance air filter");
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
