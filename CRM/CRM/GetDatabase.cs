using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ConsoleTableExt;

namespace CRM
{
    public class GetDatabase
    {
        private static string conString = @"Server = (localdb)\mssqllocaldb; Database = Kundregister; Trusted_Connection = True";

        public static void GetInfoFromDatabase()
        {
            var sql = @"SELECT [ID], [firstName], [lastName], [email], [phoneNumber] FROM Customer";

            using (SqlConnection connection = new SqlConnection(conString))
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var id = reader.GetInt32(0);
                    var firstName = reader.GetString(1);
                    var lastName = reader.GetString(2);
                    var email = reader.GetString(3);
                    var phoneNumber = reader.GetString(4);

                    System.Console.WriteLine($"ID: {id}");
                    System.Console.WriteLine($"Name : {firstName} {lastName}");
                    System.Console.WriteLine($"Contact information: {email}, {phoneNumber}");
                }
            }
        }  // Ta bort när jag är redo

        public static DataTable GetCustomersFromDatabase()
        {
            //var sql = @"SELECT [ID], [firstName], [lastName], [email], [phoneNumber] FROM Customer";
           
            var sql = @"SELECT Customer.ID, firstName, lastName, email, phoneNumber 
                        from Customer 
                        full join PhoneNumbers ON Customer.ID = PhoneNumbers.customerID";
            DataTable dataTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(conString))
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                using (dataTable)
                {
                    dataTable.TableName = "customers";
                    dataTable.Columns.Add("ID", typeof(string));
                    dataTable.Columns.Add("Name", typeof(string));
                    dataTable.Columns.Add("Email", typeof(string));
                    dataTable.Columns.Add("Number", typeof(string));

                    while (reader.Read())
                    {
                        dataTable.Rows.Add(reader.GetInt32(0), reader.GetString(1) + " " + reader.GetString(2), reader.GetString(3), reader.GetString(4));
                    }
                }
            }

            return dataTable;
        }

        public static List<Customer> GetCustomersListFromDatabase()
        {
            var customerList = new List<Customer>();
            var c = new Customer();
            var sql = @"SELECT Customer.ID, firstName, lastName, email, phoneNumber 
                        from Customer 
                        full join PhoneNumbers ON Customer.ID = PhoneNumbers.customerID";
            using (var connection = new SqlConnection(conString))
            using (var command = new SqlCommand(sql, connection))
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var customer = new Customer()
                    {
                        Id = reader.GetInt32(0),
                        FirstName = reader.GetString(1),
                        LastName = reader.GetString(2),
                        Email = reader.GetString(3)
                    };

                    customer.PhoneNumbers = GetPhoneNumberList(customer.Id);

                    customerList.Add(customer);
                }
            }
            return customerList;
        }

        public static List<string> GetPhoneNumberList(int customerID)
        {
            var sql = @"SELECT phoneNumber 
                        FROM PhoneNumbers 
                        WHERE customerID = @customerID";

            using (SqlConnection connection = new SqlConnection(conString))
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                connection.Open();

                command.Parameters.AddWithValue("customerID", customerID);

                SqlDataReader reader = command.ExecuteReader();
                var phoneNumbers = new List<string>();

                while (reader.Read())
                {
                    phoneNumbers.Add(reader.GetString(0));
                }

                return phoneNumbers;
            }
        }

        public static void AddCustomerToDatabase(string firstName, string lastName, string mail, string number)
        {
            
            var newCustomerCommand = $@"insert into Customer(firstName, lastName, email, phoneNumber) values ('{firstName}', '{lastName}', '{mail}', '{number}')";

            using (SqlConnection connection = new SqlConnection(conString))
            using (SqlCommand command = new SqlCommand(newCustomerCommand, connection))
            {
                connection.Open();
                command.ExecuteNonQuery();
            }
        } //not using

        public static int NewAddCustomerToDatabase(string firstName, string lastName, string mail)
        {
            var newCustomerCommand = $@"insert into Customer (firstName, lastName, email) values (@firstName, @lastName, @mail) SELECT SCOPE_IDENTITY()";

            using (SqlConnection connection = new SqlConnection(conString))
            using (SqlCommand command = new SqlCommand(newCustomerCommand, connection))
            {
                connection.Open();

                command.Parameters.AddWithValue("@firstName", firstName);
                command.Parameters.AddWithValue("@lastName", lastName);
                command.Parameters.AddWithValue("@mail", mail);
                //command.Parameters.AddWithValue("@number", number);

                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        public static void AddPhoneNumber(int customerId, string phoneNumber)
        {
            var newPhoneNumberCommand = @"insert into PhoneNumbers(customerID, phoneNumber) values (@customerId, @number)";

            using (SqlConnection connection = new SqlConnection(conString))
            using (SqlCommand command = new SqlCommand(newPhoneNumberCommand, connection))
            {
                connection.Open();

                command.Parameters.AddWithValue("@customerId", customerId);
                command.Parameters.AddWithValue("@number", phoneNumber);

                command.ExecuteNonQuery();
            }
        }
       
        public static void RemoveCustomerFromDatabase(Int32 id)   
        {
            //var deleteCommand = $"DELETE FROM Customer WHERE ID = @IDNumber";
            //var deleteCommand = $"DELETE FROM PhoneNumbers \r\nwhere id = @customerID;\r\nDELETE FROM Customer\r\nWHERE ID = @IDNumber;";
            var deleteCommand =
                $"DELETE FROM PhoneNumbers \r\nWHERE customerID = @IDNumber;\r\nDELETE FROM Customer\r\nWHERE Customer.ID = @IDNumber;";

            using (SqlConnection connection = new SqlConnection(conString))
            using (SqlCommand command = new SqlCommand(deleteCommand, connection))
            {
                connection.Open();
                //command.Parameters.Add("@customerID", SqlDbType.Int);
                //command.Parameters["@customerID"].Value = id; 
                command.Parameters.Add("@IDNumber", SqlDbType.Int);  
                command.Parameters["@IDNumber"].Value = id;
                command.ExecuteNonQuery();
            }
        }

        public static void RemovePhoneNumberFromDatabase(int id, string number)
        {
            var deleteCommand = "DELETE FROM PhoneNumbers WHERE customerID = @ID AND phoneNumber = @number";
            
            using (var connection = new SqlConnection(conString))
            using (var command = new SqlCommand(deleteCommand, connection))
            {
                connection.Open();

                command.Parameters.AddWithValue("ID", id);
                command.Parameters.AddWithValue("number", number);

                command.ExecuteNonQuery();
            }
        }
        
        public static void UpdateCustomer(Int32 id, string firstName, string lastName, string mail) //, string number
        {
            //var updateCommand = $"UPDATE Customer SET firstName = @firstName, lastName = @lastName, email = @mail, phoneNumber = @number WHERE ID = @IDNumber";
            var updateCommand =
                $"UPDATE Customer SET firstName = @firstName, lastName = @lastName, email = @mail WHERE ID = @IDNumber;";
            // UPDATE PhoneNumbers SET phoneNumber = @number WHERE customerID = @IDNumber;
            //var updateCommand = "DELETE FROM PhoneNumbers WHERE customerID = @IDNumber; UPDATE Customer SET firstName = @firstName, lastName = @lastName, email = @mail WHERE ID = @IDNumber; UPDATE PhoneNumbers SET phoneNumber = @number WHERE customerID = @IDNumber;";

            using (SqlConnection connection = new SqlConnection(conString))
            using (SqlCommand command = new SqlCommand(updateCommand, connection))
            {
                connection.Open();

                command.Parameters.Add("@IDNumber", SqlDbType.Int);
                command.Parameters["@IDNumber"].Value = id;

                command.Parameters.AddWithValue("@firstName", firstName);
                command.Parameters.AddWithValue("@lastName", lastName);
                command.Parameters.AddWithValue("@mail", mail);
                //command.Parameters.AddWithValue("@number", number);

                command.ExecuteNonQuery();
            }
        }

    }
}