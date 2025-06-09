using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Malshinon_09_06.DAL
{
    internal static class FilterNameFromText
    {
        static public List<string> FilterAndGetName(string text)
        {
            List<string> names = new List<string>();
            foreach (string word in text.Split(' '))
            {
                if (char.IsUpper(word[0]))
                {
                    names.Add(word);
                }
            }
            return names;   
        }
    }
}
