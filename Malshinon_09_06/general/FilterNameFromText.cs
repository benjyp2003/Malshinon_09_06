using Malshinon_09_06.Models;
using Mysqlx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Malshinon_09_06.DAL
{
    internal static class FilterNameFromText
    {
        static public FullName FilterAndGetName(string text)
        {
            try
            {
                bool flag = false;
                string firstName = "";
                string lastName = "";
                string[] textArr= text.Split(' ');
                foreach (string word in textArr)
                {
                    if (flag)
                    {
                        lastName = word;
                        break;
                    }
                    if (char.IsUpper(word[0]))
                    {
                        firstName = word;
                        flag = true;
                    }                    
                }
                return new FullName(firstName, lastName);
            }
            catch (Exception ex)
            { Console.WriteLine("Error: " + ex + " at FilterAndGetName"); }
            return null;   
        }
    }
}
