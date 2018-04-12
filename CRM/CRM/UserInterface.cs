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
                Console.WriteLine("1: View customers \n2: Create customer \n3: Remove customer \n4: Update customer \n5: Quit");
                var userChoice = Console.ReadLine();

                switch (userChoice)
                {
                    case "1":
                        PrintCustomers();
                        break;
                    case "2":
                        CreateCustomer();
                        break;
                    case "3":
                        RemoveCustomer();
                        break;
                    case "4":
                        EditCustomer();
                        break;
                }

                if (userChoice == "5")
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

        private static void CreateCustomer()
        {
            Console.Write("First name: ");
            var firstName = Console.ReadLine();
            Console.Write("Last name: ");
            var lastName = Console.ReadLine();
            Console.Write("Email: ");
            var mail = Console.ReadLine();
            Console.Write("Phone number: ");
            var number = Console.ReadLine();
            GetDatabase.AddCustomerToDatabase(firstName, lastName, mail, number);
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
