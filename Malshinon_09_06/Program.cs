using Malshinon_09_06.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Malshinon_09_06
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                PeopleDal dal = new PeopleDal();
                Identification check = new Identification();


                UserInterface u = new UserInterface();
                string[] a = u.GetFullName();

                string firstName = a[0];
                string lastName = a[1];
                string text = u.GetText();

                bool isInTable = check.IsARegisterdPerson(lastName, firstName);
                if (!isInTable)
                {
                    Console.WriteLine("add p");
                    //dal.AddPerson(new Person(firstName, lastName));
                }
                else
                {
                    Console.WriteLine("Person already exists");
                }
                string temp = null;

                foreach (string s in FilterNameFromText.FilterAndGetName(text))
                {
                    if (check.IsARegisterdPerson(s, temp))
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
            catch (Exception ex) 
            { Console.WriteLine(ex); }
        }
    }
}
