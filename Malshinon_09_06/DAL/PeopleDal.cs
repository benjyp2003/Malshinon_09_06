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

        public MySqlConnection OpenConnection()
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

        public void CloseConnection()
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


        public void AddPerson(Person person)
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
                    }
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


        public void IncrementNumReports()
        {
            try
            {
                using (var conn = new MySqlConnection(ConnStr))
                {
                    conn.Open();
                    var query = $@"
                                UPDATE `people` 
                                SET num_reports = num_reports + {1}";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.ExecuteNonQuery();
                        Console.WriteLine("Incremented reports number by one.");
                    }
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

        public void IncrementNumMentions()
        {
            try
            {
                using (var conn = new MySqlConnection(ConnStr))
                {
                    conn.Open();
                    var query = $@"
                                UPDATE `people` 
                                SET num_reports = num_mensions + {1}";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.ExecuteNonQuery();
                        Console.WriteLine("Incremented mentions number by one.");
                    }
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

        public string GetPersonsId(FullName fullName)
        {
            try
            {
                using (var conn = new MySqlConnection(ConnStr))
                {
                    conn.Open();
                    var query = $@"
                                SELECT secret_code 
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
                                return reader.GetString("secret_code");
                            }
                            else
                            {
                                Console.WriteLine("No matches found.");
                                return null;
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
            return null;
        }

        
    }
}
