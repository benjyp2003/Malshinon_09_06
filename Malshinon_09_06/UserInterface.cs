using System;

namespace Malshinon_09_06
{
    internal static class UserInterface
    {
        public static void ShowMenu()
        {

        }

        /// <summary>
        /// Prompts the user for first and last name, returning an array where
        /// index 0 = first name, index 1 = last name.
        /// </summary>
        public static string[] GetFullName()
        {
            Console.WriteLine("┌──────────────────────────────────────────────┐");
            Console.WriteLine("│       Welcome! Please Enter your Name        │");
            Console.WriteLine("└──────────────────────────────────────────────┘");

            string firstName = "";
            string lastName = "";
            while (true)
            {
                Console.Write("First Name: ");
                firstName = Console.ReadLine() ?? "";
                if (firstName != "")
                { break; }
                else { Console.WriteLine("Please enter a valid name."); }
            }

            while (true)
            {
                Console.Write("Last Name: ");
                lastName = Console.ReadLine() ?? "";
                if (lastName != "")
                { break; }
                else { Console.WriteLine("Please enter a valid name."); }
            }
            

            Console.WriteLine();
            Console.WriteLine($" Welcome, {firstName} {lastName}!");
            Console.WriteLine();

            return new[] { firstName, lastName };
        }

        /// <summary>
        /// Prompts the user to enter some text, then returns it.
        /// </summary>   
        public static string GetReport()
        {
            Console.WriteLine("┌──────────────────────────────────────────────┐");
            Console.WriteLine("│           Please Enter your report           |");
            Console.WriteLine("└──────────────────────────────────────────────┘");

            string report = "";
            while (true)
            {
                Console.Write("> ");
                report = Console.ReadLine();
                if (report != "")
                {
                    break;
                }
                else { Console.WriteLine("Enter a valid report."); }
            }
            

            Console.WriteLine();
            Console.WriteLine($" You entered: \"{report}\"");
            Console.WriteLine();

            return report;
        }
    }
}
