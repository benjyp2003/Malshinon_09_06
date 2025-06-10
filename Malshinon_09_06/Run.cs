using Malshinon_09_06.DAL;
using Malshinon_09_06.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Malshinon_09_06
{
    internal class RunningFunctions
    {
        static RunningFunctions Instance = null;
        static PeopleDal Dal = new PeopleDal();
        static Identification Check = new Identification();
        RunningFunctions() {  }

        public RunningFunctions GetInstance()
        {
            if (Instance == null)
            {
                Instance = new RunningFunctions();
            }
            return Instance;
        }

        public void GetUserInfo()
        {
            try
            {
                UserInterface u = new UserInterface();
                string[] a = u.GetFullName();

                string firstName = a[0];
                string lastName = a[1];
                FullName fullName = new FullName(firstName, lastName);

                string text = u.GetText();

                bool isInTable = Check.IsARegisterdPerson(lastName, firstName);
                if (!isInTable)
                {
                    Console.WriteLine("add p");
                    //dal.AddPerson(new Person(firstName, lastName));
                }
                else
                {
                    Console.WriteLine("Person already exists");
                }
            }
            catch (Exception ex)
            { Console.WriteLine(ex); }
        }

        public static void AnalyzeText(string text)
        {

            string temp = null;

            foreach (string s in FilterNameFromText.FilterAndGetName(text))
            {
                if (Check.IsARegisterdPerson(s, temp))
                {
                    Console.WriteLine($"already exists {temp}, {s}");
                }
                else
                {
                    Console.WriteLine($"adding {temp}, {s}");
                }
                temp = s;
            }

        }

        public static void SaveReport()
    }
}
