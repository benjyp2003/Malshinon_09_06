using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Malshinon_09_06
{
    internal static class GenerateCode
    {
        static public string Generate()
        {
            string chars = "0123456789abcdefghijklmnopqrstuvwxyz!@#$%";
            string code = "";
            Random rand = new Random();
            for (int i = 0; i < rand.Next(5, 20); i++)
            {
                code += chars[rand.Next(chars.Length)];
            }
            return code;
        }
    }
}
