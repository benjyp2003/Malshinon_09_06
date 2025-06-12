using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Malshinon_09_06.Models
{
    internal class FullName
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string fullName => FirstName + " " + LastName; 

        public FullName(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }
    }
}
