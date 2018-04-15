using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using ConsoleTableExt;

namespace CRM
{
    class UserInterface
    {
        public static void Menu()
        {
            while (true)
            {
                Console.WriteLine("1: View customers \n2: Create customer \n3: Add phone number to existing customer \n4: Remove customer \n5: Update customer \n6: Quit");
                var userChoice = Console.ReadLine();

                switch (userChoice)
                {
                    case "1":
                        PrintCustomers();
                        break;
                    case "2":
                        var id = CreateCustomer();
                        AddPhone(id);
                        break;
                    case "3":
                        CreatePhone();
                        break;
                    case "4":
                        RemoveCustomer();
                        break;
                    case "5":
                        EditCustomer();
                        break;
                }

                if (userChoice == "6")
                    break;
            }
        }

        private static void PrintCustomers()
        {
            ConsoleTableBuilder
                .From(GetDatabase.GetCustomersFromDatabase())
                .WithFormat(ConsoleTableBuilderFormat.Minimal)
                .ExportAndWriteLine();
        }

        private static void RemoveCustomer()
        {
            Console.WriteLine("Enter the ID of the customer you want to remove: ");
            var id = Convert.ToInt32(Console.ReadLine());
            GetDatabase.RemoveCustomerFromDatabase(id);
        }

        private static int CreateCustomer()
        {
            Console.Write("First name: ");
            var firstName = Console.ReadLine();
            Console.Write("Last name: ");
            var lastName = Console.ReadLine();
            Console.Write("Email: ");
            var mail = Console.ReadLine();
            
            return GetDatabase.NewAddCustomerToDatabase(firstName, lastName, mail);
            
        }

        private static void AddPhone(int customerId)
        {
            Console.Write("Phone number: ");
            var phoneNumber = Console.ReadLine();
            GetDatabase.AddPhoneNumber(customerId, phoneNumber);
        }

        private static void CreatePhone()
        {
            Console.Write("Add number to customer ID: ");
            var customerId = Convert.ToInt32(Console.ReadLine());
            AddPhone(customerId);
        }

        private static void EditCustomer()
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

            GetDatabase.UpdateCustomer(id, firstName, lastName, mail, number);
        }
    }
}
