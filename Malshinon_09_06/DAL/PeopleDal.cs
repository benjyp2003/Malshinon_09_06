﻿using Google.Protobuf.Compiler;
using Malshinon_09_06.Models;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.X509.Qualified;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Malshinon_09_06.DAL
{
    internal class PeopleDal : Dal
    {
        private static PeopleDal instance = null;
        protected override MySqlConnection OpenConnection()
        {
            if (Conn == null)
            {
                Conn = new MySqlConnection(ConnStr);
            }

            if (Conn.State != System.Data.ConnectionState.Open)
            {
                Conn.Open();
                Console.WriteLine("Connection successful.");
            }

            return Conn;
        }

        protected override void CloseConnection()
        {
            if (Conn != null && Conn.State == System.Data.ConnectionState.Open)
            {
                Conn.Close();
                Conn = null;
            }
        }

        PeopleDal()
        {
            try
            {
                OpenConnection();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"MySQL Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error: {ex.Message}");
            }
        }

        public static PeopleDal GetInstance()
        {
            if ( instance == null )
            { instance = new PeopleDal();}
            return instance;
        }


        /// <summary>
        /// Takes care of the givin reporter.
        /// Checks if he already exists in the table, if so increments the num_reports by 1,
        /// If he does not exist add him to the table.
        /// </summary>
        /// <param name="fullName"></param>
        public void HandleReporterName(FullName fullName)
        {
            int id;
            if (IsARegisterdPerson(fullName))
            {
                id = Convert.ToInt32(GetColomnByName(fullName, "id"));
                IncrementNumReports(id);
            }
            else
            {
                AddPerson(new Person(null, fullName.FirstName, fullName.LastName));
                id = Convert.ToInt32(GetColomnByName(fullName, "id"));
                IncrementNumReports(id);
            }
        }

        /// <summary>
        /// Takes care of the givin target.
        /// Checks if he already exists in the table, if so increments the num_mentions by 1, and check that the type is accurate.
        /// If he does not exist add him to the table with type 'Target'.
        /// </summary>
        /// <param name="fullName"></param>
        public void HandleTargetName(FullName fullName)
        {
            int id;
            if (IsARegisterdPerson(fullName))
            {
                // if the target already exist, and his type is 'reporter' change the type to 'both'.
                id = Convert.ToInt32(GetColomnByName(fullName, "id"));
                if (GetColomnById(id, "type").ToString() == "reporter")
                {
                    ChangeType(id, "both");
                }

                IncrementNumMentions(id);
                if (CheckNumMentions(fullName))
                {
                    Alerts alert = new Alerts(null, id, null, $"{fullName.fullName} has more than 20 mentions.");
                    SendAlert(alert);
                }
                if (CheackRapidReports(id))
                {
                     Alerts rapidAlert = new Alerts(null, id, null, $"{fullName.fullName} has more than 3 Mentions in the last 15 minutes.");
                     SendAlert(rapidAlert);
                }
            }
            else
            {
                AddPerson(new Person(null , fullName.FirstName, fullName.LastName, "Target"));
                id = Convert.ToInt32(GetColomnByName(fullName, "id"));
                IncrementNumMentions(id);
            }
        }

        
        void AddPerson(Person person)
        {
            try
            {
                using (var conn = new MySqlConnection(ConnStr))
                {
                    conn.Open();
                    var query = @"INSERT INTO People (first_name, last_name , type, num_reports, num_mentions  )
                      VALUES (@FirstName, @lastName, @type, @numReports, @numMensions)";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@FirstName", person.FirstName);
                        cmd.Parameters.AddWithValue("@lastName", person.LastName);
                        cmd.Parameters.AddWithValue("@type", person.Type);
                        cmd.Parameters.AddWithValue("@numReports", person.NumReports);
                        cmd.Parameters.AddWithValue("@numMensions", person.NumMentions);
                        cmd.ExecuteNonQuery();

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"Added {person.FirstName}  {person.LastName} Sucssesfully.\n");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"MySQL Error: {ex.Message}, At People.AddPerson");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error: {ex.Message}, , At People.AddPerson");
            }
        }

        public bool IsARegisterdPerson(FullName fullName)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConnStr))
                {
                    conn.Open();

                    string query = @" SELECT first_name, last_name
                                        FROM People
                                        WHERE last_name = @last_name AND first_name = @first_name";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@first_name", fullName.FirstName);
                        cmd.Parameters.AddWithValue("@last_name", fullName.LastName);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            { return true; }
                            else
                            { return false; }
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"MySQL Error: {ex.Message}, At People.IsARegisterdPerson");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error: {ex.Message}, At People.IsARegisterdPerson");
            }
            return false;
        }

        public List<Person> GetAllPeople()
        {
            try
            {
                List<Person> peopleList = new List<Person>();
                using (var conn = new MySqlConnection(ConnStr))
                {
                    conn.Open();
                    var query = $@"
                                 SELECT *
                                 FROM people";


                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                peopleList.Add(new Person(
                                   reader.GetInt32("id"),
                                   reader.GetString("first_name"),
                                   reader.GetString("last_name"),
                                   reader.GetString("secret_code"),
                                   reader.GetString("type"),
                                   reader.GetInt32("num_reports"),
                                   reader.GetInt32("num_mentions")
                                   ));
                            }
                            return peopleList;
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"MySQL Error: {ex.Message} , At People.GetAllPeople");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error: {ex.Message} , At People.GetAllPeople");
            }
            return null;
        }
        
        public List<Person> GetAllReporters()
        {
            try
            {
                List<Person> peopleList = new List<Person>();
                using (var conn = new MySqlConnection(ConnStr))
                {
                    conn.Open();
                    var query = $@"
                                 SELECT *
                                 FROM people
                                 WHERE type = 'Reporter'";


                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                while (reader.Read())
                                {
                                    peopleList.Add(new Person(
                                        reader.GetInt32("id"),
                                       reader.GetString("first_name"),
                                       reader.GetString("last_name"),
                                       reader.GetString("secret_code"),
                                       reader.GetString("type"),
                                       reader.GetInt32("num_reports"),
                                       reader.GetInt32("num_mentions")
                                       ));
                                }
                            }
                            else
                            {
                                Console.WriteLine("No people of type -reporter- found in the table.");
                            }
                            return peopleList;
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"MySQL Error: {ex.Message} , At People.GetAllReporters");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error: {ex.Message} , At People.GetAllReporters");
            }
            return null;
        }

        public List<Person> GetAllTargets()
        {
            try
            {
                List<Person> peopleList = new List<Person>();
                using (var conn = new MySqlConnection(ConnStr))
                {
                    conn.Open();
                    var query = $@"
                                 SELECT *
                                 FROM people
                                 WHERE type = 'Target'";


                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                while (reader.Read())
                                {
                                    peopleList.Add(new Person(
                                       reader.GetInt32("id"),
                                       reader.GetString("first_name"),
                                       reader.GetString("last_name"),
                                       reader.GetString("secret_code"),
                                       reader.GetString("type"),
                                       reader.GetInt32("num_reports"),
                                       reader.GetInt32("num_mentions")
                                       ));
                                }
                            }
                            else
                            {
                                Console.WriteLine("No people of type -target- apears in the table.");
                            }
                            return peopleList;
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"MySQL Error: {ex.Message} , At People.GetAllTargetss");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error: {ex.Message} , At People.GetAllTargetss");
            }
            return null;
        }

        public List<Person> GetAllPotentialAgents()
        {
            try
            {
                List<Person> peopleList = new List<Person>();
                using (var conn = new MySqlConnection(ConnStr))
                {
                    conn.Open();
                    var query = $@"
                                 SELECT *
                                 FROM people
                                 WHERE type = 'Potential agent'";


                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                while (reader.Read())
                                {
                                    peopleList.Add(new Person(
                                       reader.GetInt32("id"),
                                       reader.GetString("first_name"),
                                       reader.GetString("last_name"),
                                       reader.GetString("secret_code"),
                                       reader.GetString("type"),
                                       reader.GetInt32("num_reports"),
                                       reader.GetInt32("num_mentions")
                                       ));
                                }
                            }
                            else
                            {
                                Console.WriteLine("No potential agents exist.");
                            }
                            return peopleList;
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"MySQL Error: {ex.Message} , At People.GetAllTargetss");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error: {ex.Message} , At People.GetAllTargetss");
            }
            return null;
        }


        public List<Person> GetAllDangerousTargets()
        {
            try
            {
                List<Person> peopleList = new List<Person>();
                using (var conn = new MySqlConnection(ConnStr))
                {
                    conn.Open();
                    var query = $@"
                                 SELECT *
                                 FROM people
                                 WHERE type = 'Dangerous Target'";


                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                while (reader.Read())
                                {
                                    peopleList.Add(new Person(
                                        reader.GetInt32("id"),
                                       reader.GetString("first_name"),
                                       reader.GetString("last_name"),
                                       reader.GetString("secret_code"),
                                       reader.GetString("type"),
                                       reader.GetInt32("num_reports"),
                                       reader.GetInt32("num_mentions")
                                       ));
                                }
                            }
                            else
                            {
                                Console.WriteLine("No dangerous targets.");
                            }
                            return peopleList;
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"MySQL Error: {ex.Message} , At People.GetAllDangeresTargets");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error: {ex.Message} , At People.GetAllDangeresTargets");
            }
            return null;
        }


        public Person GetPersonByName(FullName fullName)
        {
            try
            {
                using (var conn = new MySqlConnection(ConnStr))
                {
                    conn.Open();
                    var query = $@"
                                 SELECT *
                                 FROM people
                                 WHERE @firstName = first_name AND @lastName = last_name";
                                 

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@firstName", fullName.FirstName);
                        cmd.Parameters.AddWithValue("@lastName", fullName.LastName);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                 return new Person(
                                     reader.GetInt32("id"),
                                    reader.GetString("first_name"),
                                    reader.GetString("last_name"),
                                    reader.GetString("secret_code"),
                                    reader.GetString("type"),
                                    reader.GetInt32("num_reports"),
                                    reader.GetInt32("num_mentions")
                                    );
                            }
                            else
                            {
                                Console.WriteLine($"No match for {fullName.fullName} found in people.\n");
                            }
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"MySQL Error: {ex.Message} , At People.GetPersonByName");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error: {ex.Message} , At People.GetPersonByName");
            }
            return null;
        }

        public Person GetPersonById(int id)
        {
            try
            {
                using (var conn = new MySqlConnection(ConnStr))
                {
                    conn.Open();
                    var query = $@"
                                 SELECT *
                                 FROM people
                                 WHERE @id = id";


                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Person(
                                    reader.GetInt32("id"),
                                   reader.GetString("first_name"),
                                   reader.GetString("last_name"),
                                   reader.GetString("secret_code"),
                                   reader.GetString("type"),
                                   reader.GetInt32("num_reports"),
                                   reader.GetInt32("num_mentions")
                                   );
                            }
                            else
                            {
                                Console.WriteLine($"No match for id: {id} found in people.\n");
                            }
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"MySQL Error: {ex.Message} , At People.GetPersonById");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error: {ex.Message} , At People.GetPersonById");
            }
            return null;
        }


        public object GetColomnById(int id, string colomn)
        {
            try
            {
                using (var conn = new MySqlConnection(ConnStr))
                {
                    conn.Open();
                    var query = $@"
                                 SELECT {colomn}
                                 FROM people
                                 WHERE @id = id";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                if (reader.GetFieldType(colomn) == typeof(int))
                                {
                                    return reader.GetInt32(colomn);
                                }
                                else
                                {
                                    return reader.GetString(colomn);
                                }
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.DarkRed;

                                Console.WriteLine($"No {colomn} match for id: {id} found in people.\n");
                                Console.ForegroundColor = ConsoleColor.White;

                            }
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"MySQL Error: {ex.Message} , At People.GetColomnById");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error: {ex.Message} , At People.GetColomnById");
            }
            return null;
        }

        public object GetColomnByName(FullName fullName, string colomn)
        {
            try
            {
                using (var conn = new MySqlConnection(ConnStr))
                {
                    conn.Open();
                    var query = $@"
                                 SELECT {colomn}
                                 FROM people
                                 WHERE @firstName = first_name AND @lastName = last_name";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@firstName", fullName.FirstName);
                        cmd.Parameters.AddWithValue("@lastName", fullName.LastName);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                if (reader.GetFieldType(colomn) == typeof(int))
                                {
                                    return reader.GetInt32(colomn);
                                }
                                else
                                {
                                    return reader.GetString(colomn);
                                }
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.DarkRed;

                                Console.WriteLine($"No {colomn} match for id- {fullName.fullName} found in people.\n");
                                Console.ForegroundColor = ConsoleColor.White;

                            }
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"MySQL Error: {ex.Message} , At People.GetColomnByName");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error: {ex.Message} , At People.GetColomnByName");
            }
            return null;
        }

        void IncrementNumReports(int id)
        {
            try
            {
                using (var conn = new MySqlConnection(ConnStr))
                {
                    conn.Open();
                    var query = @"
                                UPDATE people 
                                SET num_reports = num_reports + 1
                                WHERE @id = id";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@count", 1);
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"Incremented reports number of id: {id} by one.\n");
                        Console.ForegroundColor = ConsoleColor.White;

                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"MySQL Error: {ex.Message} , At People.IncrementNumReports");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error: {ex.Message} , At People.IncrementNumReports");
            }
        }

        void IncrementNumMentions(int id)
        {
            try
            {
                using (var conn = new MySqlConnection(ConnStr))
                {
                    conn.Open();
                    var query = $@"
                                UPDATE `people` 
                                SET num_mentions = num_mentions + 1
                                WHERE id = @id";


                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"Incremented mentions number of id: {id} by one.\n");
                        Console.ForegroundColor = ConsoleColor.White;

                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"MySQL Error: {ex.Message}, At People.IncrementNumMentions");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error: {ex.Message}, At People.IncrementNumMentions");
            }
        }


        public void CheckNumReports(FullName fullName)
        {
            try
            {
                int id = Convert.ToInt32(GetColomnByName(fullName, "num_reports"));
                int val;
                int numOfReports = Convert.ToInt32(GetColomnByName(fullName, "num_reports"));
                if (numOfReports > 10 && IntelReportsDal.AverageReportersText.TryGetValue(id, out val) && val > 100)
                {
                    ChangeType(id, "potential agent");
                }
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Changed {fullName.fullName} to a Potential Agent.");
                Console.ForegroundColor= ConsoleColor.White;
                
            }
            catch (MySqlException ex)
            { 
                Console.WriteLine($"MySQL Error: {ex.Message}, At PeopleDal.CheckNumReports");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error: {ex.Message}, At PeopleDal.CheckNumReports");
            }
        }

        public bool CheckNumMentions(FullName fullName)
        {
            try
            {
                int id = Convert.ToInt32(GetColomnByName(fullName, "num_reports"));

                int numOfMentions = Convert.ToInt32(GetColomnByName(fullName, "num_mentions"));
                if (numOfMentions >= 20)
                {
                    ChangeType(id, "Dangerous Target");
                    Console.ForegroundColor = ConsoleColor.DarkRed;

                    Console.WriteLine("Caution ! this target has 20+ mentions. ");
                    Console.ForegroundColor = ConsoleColor.White;
                    return true;
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"MySQL Error: {ex.Message}, at PeopleDal.CheckNumMentions");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error: {ex.Message}, at PeopleDal.CheckNumMentions");
            }
            return false;
        }

        void ChangeType(int id, string newType)
        {
            try
            {
                using (var conn = new MySqlConnection(ConnStr))
                {
                    conn.Open();
                    var query = @"UPDATE people
                                  SET type = @newtype
                                  WHERE @id = id";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.Parameters.AddWithValue("@newType", newType);
                        cmd.ExecuteNonQuery();

                        Console.WriteLine($"Changed type of id- {id} to type '{newType}'\n");
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"MySQL Error: {ex.Message}, At PeopleDal.ChangeType");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error: {ex.Message}, At PeopleDal.ChangeType");
            }
        }

        bool CheackRapidReports(int id)
        {
            try
            {
                using (var conn = new MySqlConnection(ConnStr))
                {
                    conn.Open();
                    var query = @"SELECT COUNT(*) AS report_count
                                  FROM intelreports i
                                  JOIN alerts a ON i.target_id = a.target_id
                                    AND created_at >= NOW() - INTERVAL 15 MINUTE;
                                     ";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);

                        var reader = cmd.ExecuteScalar();
                        if (reader != null)
                        {
                            int count = Convert.ToInt32(reader);
                            if (count > 3)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"MySQL Error: {ex.Message}, At PeopleDal.CheackRaidReports");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error: {ex.Message}, At PeopleDal.CheackRaidReports");
            }
            return false;
        }

        void SendAlert(Alerts alert)
        {
            AlertDal alertDal = AlertDal.GetInstance();
            alertDal.AddAlert(alert);
        }
    }
}
