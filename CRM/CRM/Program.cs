using System;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;

namespace CRM
{
    class Program
    {


        static void Main(string[] args)
        {
            GetDatabase.UpdateCustomer(5, "Anton", "Oq", "anoq@gmail.com", "0708909876");
            //GetDatabase.RemoveCustomer(6);
            //GetDatabase.CreateCustomer("Karl", "Blå", "hejhopp@plupp.se", "0709876543");
            GetDatabase.GetInfoFromDatabase();
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
