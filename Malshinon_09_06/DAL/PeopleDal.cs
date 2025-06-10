using Google.Protobuf.Compiler;
using Malshinon_09_06.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Malshinon_09_06.DAL
{
    internal class PeopleDal 
    {
        protected string ConnStr = "server=localhost;user=root;password=;database=Malshinon";
        protected MySqlConnection Conn;

        MySqlConnection OpenConnection()
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

        void CloseConnection()
        {
            if (Conn != null && Conn.State == System.Data.ConnectionState.Open)
            {
                Conn.Close();
                Conn = null;
            }
        }

        public PeopleDal()
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


        /// <summary>
        /// Takes care of the givin reporter.
        /// Checks if he already exists in the table, if so increments the num_reports by 1,
        /// If he does not exist add him to the table.
        /// </summary>
        /// <param name="fullName"></param>
        public void HandleReporterName(FullName fullName)
        {
            int id = GetPersonsId(fullName);
            if (IsARegisterdPerson(fullName))
            {
                IncrementNumReports(id);
            }
            else
            {
                AddPerson(new Person(fullName.FirstName, fullName.LastName));
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
            int id = GetPersonsId(fullName);
            if (IsARegisterdPerson(fullName))
            {
                // if the target already exist, and his type is 'reporter' change the type to 'both'.
                if (GetStringTypeFromPeopleTable(id, "type") == "reporter")
                {
                    ChangeType(id, "both");
                }

                IncrementNumMentions(id);
            }
            else
            {
                AddPerson(new Person(fullName.FirstName, fullName.LastName, "Target"));
                IncrementNumMentions(id);
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


        void AddPerson(Person person)
        {
            try
            {
                using (var conn = new MySqlConnection(ConnStr))
                {
                    conn.Open();
                    var query = @"INSERT INTO People (first_name, last_name, secret_code , type, num_reports, num_mentions  )
                      VALUES (@FirstName, @lastName, @SecretCode, @type, @numReports, @numMensions)";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@FirstName", person.FirstName);
                        cmd.Parameters.AddWithValue("@lastName", person.LastName);
                        cmd.Parameters.AddWithValue("@SecretCode", person.SecretCode);
                        cmd.Parameters.AddWithValue("@type", person.Type);
                        cmd.Parameters.AddWithValue("@numReports", person.Num_reports);
                        cmd.Parameters.AddWithValue("@numMensions", person.Num_mentions);
                        cmd.ExecuteNonQuery();

                        Console.WriteLine($"Added {person.FirstName} {person.LastName} Sucssesfully.\n");
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

        public string GetStringTypeFromPeopleTable(int id, string colomn)
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
                                return reader.GetString(colomn);
                            }
                            else
                            {
                                Console.WriteLine($"No {colomn} match found in people.");
                            }
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"MySQL Error: {ex.Message} , At People.GetNumReports");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error: {ex.Message} , At People.GetNumReportss");
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
                                SET num_reports = num_reports + @count
                                WHERE @id = id";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@count", 1);
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                        Console.WriteLine($"Incremented reports number by one.\n");
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
                        Console.WriteLine("Incremented mentions number by one.\n");
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

        public int GetPersonsId(FullName fullName)
        {
            try
            {
                using (var conn = new MySqlConnection(ConnStr))
                {
                    conn.Open();
                    var query = $@"
                                SELECT id 
                                FROM people
                                WHERE first_name = @firstName AND last_name = @lastName";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@firstName", fullName.FirstName);
                        cmd.Parameters.AddWithValue("@lastName", fullName.LastName);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return reader.GetInt32("id");
                            }
                            else
                            {
                                Console.WriteLine("No matches found.");
                                return -1;
                            }
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"MySQL Error: {ex.Message}.  at PeopleDal.GetId");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error: {ex.Message}");
            }
            return -1;
        }

        int GetNumReports(int id)
        {
            try
            {
                using (var conn = new MySqlConnection(ConnStr))
                {
                    conn.Open();
                    var query = $@"
                                 SELECT num_reports
                                 FROM people
                                 WHERE @id = id";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return reader.GetInt32("num_reports");
                            }
                            else
                            {
                                Console.WriteLine("No people found.");
                            }
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"MySQL Error: {ex.Message} , At People.GetNumReports");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error: {ex.Message} , At People.GetNumReportss");
            }
            return -1;
        }
        int GetNumMentions(int id)
        {
            try
            {
                using (var conn = new MySqlConnection(ConnStr))
                {
                    conn.Open();
                    var query = $@"
                                 SELECT num_mentions
                                 FROM people
                                 WHERE @id = id";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return reader.GetInt32("num_mentions");
                            }
                            else
                            {
                                Console.WriteLine("No people found.");
                            }
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"MySQL Error: {ex.Message}, At People.GetNumMentions");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error: {ex.Message}, At People.GetNumMentions");
            }
            return -1;
        }


        public void CheckNumReports(FullName fullName)
        {
            try
            {
                int id = GetPersonsId(fullName);
                int val;
                int numOfReports = GetNumReports(id);
                if (numOfReports > 10 && IntelReportsDal.AverageReportersText.TryGetValue(id, out val) && val > 100)
                {
                    ChangeType(id, "potential_agent");
                }
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

        public void CheckNumMentions(FullName fullName)
        {
            try
            {
                int id = GetPersonsId(fullName);

                int numOfMentions = GetNumMentions(id);
                if (numOfMentions >= 20)
                {
                    Console.WriteLine("Caution ! this target has 20+ mentions. "); ;
                }
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


    }
}
