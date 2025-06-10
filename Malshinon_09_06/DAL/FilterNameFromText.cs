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
                foreach (string word in text.Split(' '))
                {
                    if (char.IsUpper(word[0]) & !flag)
                    {
                        firstName = word;
                        flag = true;
                    }
                    else
                    {
                        return new FullName(firstName, word);
                    }
                }
            }
            catch (Exception ex)
            { Console.WriteLine("Error: " + ex); }
            return null;   
        }
    }
}
