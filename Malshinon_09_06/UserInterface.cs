using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Malshinon_09_06
{
    internal class UserInterface
    {
        /// <summary>
        /// This func asks the user to enter a full name, and returns an array where
        /// index 0 has the first name and index 1 has the last name.
        /// </summary>
        /// <returns>An array of 2 strings: first name and last name.</returns>

        public string[] GetFullName()
        {
            Console.WriteLine("Please enter your first name:");
            string firstName = Console.ReadLine();
            Console.WriteLine("please enter your Last name: ");
            string lastName = Console.ReadLine();
            Console.WriteLine($"Welcome {firstName} {lastName} !");
            return new string[] { firstName, lastName };
        }

        /// <summary>
        /// Get report text from user.
        /// </summary>
        /// <returns> string </returns>
        public string GetText()
        {
            Console.WriteLine("Enter your text:");
            string text = Console.ReadLine();
            return text;
        }
    }
}
