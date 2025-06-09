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
        protected string connStr = "server=localhost;user=root;password=;database=Malshinon";
        protected MySqlConnection _conn;

        public MySqlConnection openConnection()
        {
            if (_conn == null)
            {
                _conn = new MySqlConnection(connStr);
            }

            if (_conn.State != System.Data.ConnectionState.Open)
            {
                _conn.Open();
                Console.WriteLine("Connection successful.");
            }

            return _conn;
        }

        public void closeConnection()
        {
            if (_conn != null && _conn.State == System.Data.ConnectionState.Open)
            {
                _conn.Close();
                _conn = null;
            }
        }

        public PeopleDal()
        {
            try
            {
                openConnection();
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
                using (var conn = new MySqlConnection(connStr))
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
    }
}
