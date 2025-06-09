using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Malshinon_09_06.DAL
{
    internal class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SecretCode { get; set; }
        public string Type { get; set; }
        int Num_reports { get; set; }
        int Num_mentions { get; set; }

        public Person(int id, string firstName, string lastNmae, string secretCode, string type, int numReports, int numMentions)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastNmae;
            SecretCode = secretCode;
            Type = type;
            Num_reports = numReports;
            Num_mentions = numMentions;
        }
    }
}
