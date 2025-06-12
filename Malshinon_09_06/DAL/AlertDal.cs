using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Malshinon_09_06.DAL
{
    internal class AlertDal : Dal 
    {
        private static AlertDal instance = null;
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

        AlertDal()
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

        public static AlertDal GetInstance()
        {
            if (instance == null)
            { instance = new AlertDal(); }
            return instance;
        }

        public void AddAlert(Alerts alert)
        {
            try
            {
                using (MySqlConnection conn = OpenConnection())
                {
                    conn.Open();
                    string query = @"INSERT INTO alerts (target_id, reason)
                                     VALUES (@targetId, @reason)";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@targetId", alert.TargetId);
                        cmd.Parameters.AddWithValue("@reason", alert.Reason);

                        cmd.ExecuteNonQuery();
                    }
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Added Alert {alert.Reason} successfully");
                    Console.ForegroundColor = ConsoleColor.White;
                }

            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"MySQL Error: {ex.Message}, At AlertDal.AddAlert");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error: {ex.Message}, At  AlertDal.AddAlert");
            }
        }


    }
}
