using System;

namespace Malshinon_09_06
{
    internal static class UserInterface
    {
        public static void ShowAnalysisMenu()
        {
            Console.WriteLine("┌───────────────────────────────────────────────┐");
            Console.WriteLine("│                Analysis  Menu                 │");
            Console.WriteLine("├───────────────────────────────────────────────┤");
            Console.WriteLine("│ 1 - Get a new Report.                         │");
            Console.WriteLine("│ 2 - Get Persons Info by name.                 │");
            Console.WriteLine("│ 3 - Get Persons Info by ID.                   │");
            Console.WriteLine("│ 4 - Get all Targets.                          │");
            Console.WriteLine("│ 5 - Get all Reporters.                        │");
            Console.WriteLine("│ 6 - Get all potential agents.                 │");
            Console.WriteLine("│ 7 - Get all dangerous targets.                │");
            Console.WriteLine("│ 8 - Get all active Alerts.                    │");
            Console.WriteLine("│ 9 - Get Reporters average report length.      │");
            Console.WriteLine("│ 0 - Exit                                      │");
            Console.WriteLine("└───────────────────────────────────────────────┘\n");
            Console.Write("Enter your choice: ");
        }

        /// <summary>
        /// Prompts the user for first and last name, returning an array where
        /// index 0 = first name, index 1 = last name.
        /// </summary>
        public static string[] GetFullName()
        {
            Console.Clear();
            Console.WriteLine("┌──────────────────────────────────────────────┐");
            Console.WriteLine("│       Welcome to the reporting system!       │");
            Console.WriteLine("│       Please Enter the Reporters Name.       │");
            Console.WriteLine("└──────────────────────────────────────────────┘");

            string firstName = "";
            string lastName = "";
            while (true)
            {
                Console.Write("First Name: ");
                firstName = Console.ReadLine().Trim() ?? "";
                if (firstName != "")
                { break; }
                else { Console.WriteLine("Please enter a valid name."); }
            }

            while (true)
            {
                Console.Write("Last Name: ");
                lastName = Console.ReadLine().Trim() ?? "";
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
            Console.WriteLine("│         Please Enter the report text.        |");
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
