using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Bangazon
{
    public class DatabaseOps
    {
        const string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB; AttachDbFilename=\"C:\\Users\\shu\\workspace\\cs\\Bangazon\\Bangazon\\Bangazon.mdf\"; Integrated Security= True";
        
        public static void executeNonQuery(string command)  // use this for INSERTs
        {
            SqlConnection sqlConnection1 = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = command;
            cmd.Connection = sqlConnection1;
            sqlConnection1.Open();
            cmd.ExecuteNonQuery();
            sqlConnection1.Close();
        }

        public static List<Customer> loadCustomers()
        {
            List<Customer> customerList = new List<Customer>();
            string query = @"SELECT c.CustomerId,c.FirstName,c.LastName,c.Address1,c.Address2,c.City,c.State,c.ZipCode,c.Phone FROM Customer c";
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                connection.Open();
                using (SqlDataReader r = cmd.ExecuteReader())
                {
                    // Check if the reader has any rows at all before starting to read.
                    if (r.HasRows)
                    {
                        // Read advances to the next row.
                        int index = 0;
                        while (r.Read())
                        {
                            Customer c = new Customer(index++, r[0] as string, r[1] as string, r[2] as string, r[3] as string, r[4] as string, r[5] as string, r[6] as string, r[7] as string, r[8] as string);
                            customerList.Add(c);
                        }
                    }
                }
                connection.Close();
            }
            return customerList;
        }

        public static List<Product> loadProducts()
        {
            List<Product> productList = new List<Product>();
            string query = @"SELECT p.productId, p.name, p.price FROM Product p";
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                connection.Open();
                using (SqlDataReader r = cmd.ExecuteReader())
                {
                    // Check if the reader has any rows at all before starting to read.
                    if (r.HasRows)
                    {
                        // Read advances to the next row.
                        int index = 0;
                        while (r.Read())
                        {
                            Product p = new Product(index++, r[0] as int? ?? 0, r[1] as string, r[2] as decimal? ?? 0);
                            productList.Add(p);
                        }
                    }
                }
                connection.Close();
            }
            return productList;
        }

        public static List<PaymentType> loadPaymentTypes()
        {
            List<PaymentType> paymentTypeList = new List<PaymentType>();
            string query = @"SELECT pt.paymentTypeId, pt.name FROM PaymentType pt";
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                connection.Open();
                using (SqlDataReader r = cmd.ExecuteReader())
                {
                    // Check if the reader has any rows at all before starting to read.
                    if (r.HasRows)
                    {
                        // Read advances to the next row.
                        int index = 0;
                        while (r.Read())
                        {
                            PaymentType pt = new PaymentType(index++, r[0] as int? ?? 0, r[1] as string);
                            paymentTypeList.Add(pt);
                        }
                    }
                }
                connection.Close();
            }
            return paymentTypeList;
        }

        public static List<PaymentType> loadPaymentTypesAvailable(string customerId)
        {
            List<PaymentType> paymentTypesAvailable = new List<PaymentType>();
            string query = @"SELECT pt.paymentTypeId, pt.name FROM PaymentType pt INNER JOIN PaymentTypesAvailable pta ON pta.paymentTypeId = pt.paymentTypeId WHERE pta.customerId = '" + customerId + "'";
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                connection.Open();
                using (SqlDataReader r = cmd.ExecuteReader())
                {
                    // Check if the reader has any rows at all before starting to read.
                    if (r.HasRows)
                    {
                        // Read advances to the next row.
                        int index = 0;
                        while (r.Read())
                        {
                            PaymentType pta = new PaymentType(index++, r[0] as int? ?? 0, r[1] as string);
                            paymentTypesAvailable.Add(pta);
                        }
                    }
                }
                connection.Close();
            }
            return paymentTypesAvailable;
        }

        public static void createOrder(string customerId, int paymentTypeId, List<Product> lineItems)
        {
            // determine next Invoice ID number
            int maxInvoiceId = 1000; // use 1000 if Invoices table is empty
            string query = @"SELECT MAX(invoiceId) FROM Invoices";
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                connection.Open();
                using (SqlDataReader r = cmd.ExecuteReader())
                {
                    if (r.HasRows)
                    {
                        // Read advances to the next row.
                        int index = 0;
                        while (r.Read())
                        {
                            maxInvoiceId = r[0] as int? ?? 0;
                        }
                    }
                }
                connection.Close();
            }

            // add new row to Invoices table
            StringBuilder command5 = new StringBuilder();
            command5.Append("INSERT INTO Invoices ");
            command5.Append("(invoiceId, customerId, paymentTypeId) ");
            command5.Append("VALUES (");
            command5.Append((maxInvoiceId + 1).ToString() + ", ");
            command5.Append("'" + customerId + "', ");
            command5.Append(paymentTypeId.ToString());
            command5.Append(")");
            DatabaseOps.executeNonQuery(command5.ToString());

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
                DatabaseOps.executeNonQuery(command4.ToString());
            }
        }

    }
}
