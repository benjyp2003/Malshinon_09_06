﻿using Malshinon_09_06.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Malshinon_09_06.DAL
{
    internal class Person
    {
        public int? Id { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string SecretCode { get;  }
        public string Type { get; set; } 
        public int NumReports { get; set; } 
        public int NumMentions { get; set; }

        public FullName FullName => new FullName(FirstName, LastName);

        public Person(int? id, string firstName, string lastNmae, string secretCode = "", string type = "Reporter", int numReports = 0, int numMensions = 0)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastNmae;
            if (secretCode == "")
            {
                SecretCode = GenerateCode.Generate();
            }
            else
            {
                SecretCode = secretCode;
            }
            Type = type;
            NumReports = numReports;
            NumMentions = numMensions;
        }

        public override string ToString()
        {
            return $"ID: {Id} \n" +
                   $"FirstName: {FirstName} \n" +
                   $"LasstName: {LastName} \n" +
                   $"SecretCode: {SecretCode} \n" +
                   $"Type: {Type} \n" +
                   $"Num of Reports: {NumReports} \n" +
                   $"Num of Mensionss {NumMentions} \n";
                     
        }
    }
}
