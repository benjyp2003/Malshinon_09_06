using Malshinon_09_06.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Malshinon_09_06.DAL
{
    internal class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SecretCode { get; set; }
        public string Type { get; set; } 
        public int Num_reports { get; set; } 
        public int Num_mentions { get; set; }

        public FullName FullName => new FullName(FirstName, LastName);

        public Person(string firstName, string lastNmae, string type = "Reporter")
        {
            FirstName = firstName;
            LastName = lastNmae;
            SecretCode = GenerateCode.Generate();
            Type = type;
            Num_reports = 0;
            Num_mentions = 0;
        }
    }
}
