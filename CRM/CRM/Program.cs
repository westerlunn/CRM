using System;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;
using ConsoleTableExt;

namespace CRM
{
    class Program
    {


        static void Main(string[] args)
        {
            //GetDatabase.GetCustomers();
            GetDatabase.Menu();
            //GetDatabase.UpdateCustomer(5, "Anton", "Oq", "anoq@gmail.com", "0708909876");
            //GetDatabase.RemoveCustomer(6);
            //GetDatabase.CreateCustomer("Karl", "Blå", "hejhopp@plupp.se", "0709876543");
            //GetDatabase.GetInfoFromDatabase();
        }

    }

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
        }

        public static void Menu()
        {
            while (true)
            {
                Console.WriteLine("1: View customers \n2: Create customer \n3: Remove customer \n4: Update customer");
                var userChoice = Console.ReadLine();

                if (userChoice == "1")
                {
                    GetCustomers();
                }

                if (userChoice == "2")
                {
                    Console.Write("First name: ");
                    var firstName = Console.ReadLine();
                    Console.Write("Last name: ");
                    var lastName = Console.ReadLine();
                    Console.Write("Email: ");
                    var mail = Console.ReadLine();
                    Console.Write("Phone number: ");
                    var number = Console.ReadLine();
                    CreateCustomer(firstName, lastName, mail, number);
                }

                if (userChoice == "3")
                {
                    Console.WriteLine("Enter the ID of the customer you want to remove: ");
                    var id = Convert.ToInt32(Console.ReadLine());
                    RemoveCustomer(id);
                }

                if (userChoice == "4")
                {
                    Console.Write("Customer ID: ");
                    var id = Convert.ToInt32(Console.ReadLine());
                    Console.Write("First name: ");
                    var firstName = Console.ReadLine();
                    Console.Write("Last name: ");
                    var lastName = Console.ReadLine();
                    Console.Write("Email: ");
                    var mail = Console.ReadLine();
                    Console.Write("Phone number: ");
                    var number = Console.ReadLine();

                    UpdateCustomer(id, firstName, lastName, mail, number);
                }

                if (userChoice == "5")
                    break;
            }
        }

        public static void GetCustomers()
        {
            var sql = @"SELECT [ID], [firstName], [lastName], [email], [phoneNumber] FROM Customer";

            using (SqlConnection connection = new SqlConnection(conString))
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                using (DataTable dataTable = new DataTable())
                {
                    dataTable.TableName = "customers";
                    dataTable.Columns.Add("ID", typeof(string));
                    dataTable.Columns.Add("Name", typeof(string));
                    dataTable.Columns.Add("Email", typeof(string));
                    dataTable.Columns.Add("Number", typeof(string));

                    while (reader.Read())
                    {
                        dataTable.Rows.Add(reader.GetInt32(0), reader.GetString(1) + " " + reader.GetString(2), reader.GetString(3), reader
                            .GetString(4));
                    }
                    ConsoleTableBuilder
                        .From(dataTable)
                        .WithFormat(ConsoleTableBuilderFormat.Minimal)
                        .ExportAndWriteLine();
                }
                   
            }
        }

        public static void CreateCustomer(string firstName, string lastName, string mail, string number)
        {
            
            var newCustomerCommand = $@"insert into Customer(firstName, lastName, email, phoneNumber) values ('{firstName}', '{lastName}', '{mail}', '{number}')";

            using (SqlConnection connection = new SqlConnection(conString))
            using (SqlCommand command = new SqlCommand(newCustomerCommand, connection))
            {
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public static void RemoveCustomer(Int32 id)   
        {
            var deleteCommand = $"DELETE FROM Customer WHERE ID = @IDNumber";

            using (SqlConnection connection = new SqlConnection(conString))
            using (SqlCommand command = new SqlCommand(deleteCommand, connection))
            {
                connection.Open();

                command.Parameters.Add("@IDNumber", SqlDbType.Int);  
                command.Parameters["@IDNumber"].Value = id;
                command.ExecuteNonQuery();
            }
        }

        public static void UpdateCustomer(Int32 id, string firstName, string lastName, string mail, string number)
        {
            var updateCommand = $"UPDATE Customer SET firstName = @firstName, lastName = @lastName, email = @mail, phoneNumber = @number WHERE ID = @IDNumber";

            using (SqlConnection connection = new SqlConnection(conString))
            using (SqlCommand command = new SqlCommand(updateCommand, connection))
            {
                connection.Open();

                command.Parameters.Add("@IDNumber", SqlDbType.Int);
                command.Parameters["@IDNumber"].Value = id;

                command.Parameters.AddWithValue("@firstName", firstName);
                command.Parameters.AddWithValue("@lastName", lastName);
                command.Parameters.AddWithValue("@mail", mail);
                command.Parameters.AddWithValue("@number", number);

                command.ExecuteNonQuery();
            }
        }

    }
}
