using Malshinon_09_06.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Malshinon_09_06
{
    internal class IntelReportsDal : Dal
    {
       
        private static IntelReportsDal Instance = null;

        public static Dictionary<int, int> AverageReportersText = new Dictionary<int, int>();

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

        IntelReportsDal()
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

        public static IntelReportsDal GetInstance()
        {
            if ( Instance == null )
            { 
                Instance = new IntelReportsDal();
                
            }
            return Instance; 
        }


        /// <summary>
        /// Adds the Report to the reports table, 
        /// and updates the reporters text average.
        /// </summary>
        /// <param name="report"></param>
        public void HandleReports(IntelReports report)
        {
            AddReport(report);
            Console.WriteLine("Report added.");

            UpdateAverageText(report.Text, report.ReporterId);
        }


        public void AddReport(IntelReports report)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConnStr))
                {
                    conn.Open();
                    string query = @"
                                    INSERT INTO intelReports (reporter_id, target_id, text)
                                    VALUES (@reporterId, @targetId, @text)";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@reporterId", report.ReporterId);
                        cmd.Parameters.AddWithValue("@targetId", report.TargetId);
                        cmd.Parameters.AddWithValue("@text", report.Text);

                        cmd.ExecuteNonQuery();


                    }
                }

            }
            catch (MySqlException ex) 
            {
                Console.WriteLine($"MySQL Error: {ex.Message}. At IntelRepoortDal.AddReport");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error: {ex.Message}.  At IntelRepoortDal.AddReport");
            }

            
        }


        public void UpdateAverageText(string text, int reporterId)
        {
            try
            {
                int len = text.Length;
                if (AverageReportersText.ContainsKey(reporterId))
                {
                    AverageReportersText[reporterId] = (AverageReportersText[reporterId] + len) / 2;
                }
                else
                {
                    AverageReportersText.Add(reporterId, len);
                }
            }
            catch (Exception ex)  
            { Console.WriteLine($"ERROR: {ex}, At IntelReportsDal.UpdateAverageText"); }
        }
    }
}
