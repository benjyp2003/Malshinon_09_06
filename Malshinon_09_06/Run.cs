using Malshinon_09_06.DAL;
using Malshinon_09_06.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Malshinon_09_06
{
    internal class RunningFunctions
    {
        static RunningFunctions Instance = null;
        static PeopleDal Dal = new PeopleDal();
        static IntelReportsDal reportDal = IntelReportsDal.GetInstance(); 

        RunningFunctions() { }

        public static RunningFunctions GetInstance()
        {
            if (Instance == null)
            {
                Instance = new RunningFunctions();
            }
            return Instance;
        }

        public void Start()
        {
            while (true)
            {
                // get full name of the reporter.
                FullName reporterFullName = GetUserName();
                Dal.HandleReporterName(reporterFullName);

                // check the amount of reprots the reporter has, and update his type if needed. 
                Dal.CheckNumReports(reporterFullName);

                // get the report.
                string reportTxt = UserInterface.GetReport();

                // extract name of the target out of the report.
                FullName TargetFullName = FilterNameFromText.FilterAndGetName(reportTxt);

                // send targets name for handeling.
                Dal.HandleTargetName(TargetFullName);

                // // check the amount of mentions the target has, and give an alert if needed. 
                Dal.CheckNumMentions(TargetFullName);

                // get reporter and targets id.
                int reporterId = Dal.GetPersonsId(reporterFullName);
                int targetId = Dal.GetPersonsId(TargetFullName);

                // create a report.
                IntelReports report = new IntelReports(reporterId, targetId, reportTxt);

                // send the report for handling. (sending to dataBase etc.)
                reportDal.HandleReports(report);

                // If the user presses 0 the program will finish.              
                if (ExitOption() == '0')
                { break; }
            }
        }

        FullName GetUserName()
        {
            try
            {
                string[] a = UserInterface.GetFullName();

                string firstName = a[0];
                string lastName = a[1];
                FullName fullName = new FullName(firstName, lastName);

                return fullName;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error: {ex.Message}, At Run.GetUserInfo");
            }
            return null;
        }

        char ExitOption()
        {
            Console.WriteLine("┌──────────────────────────────────────────────┐");
            Console.WriteLine("│       Press any key  (or '0' to exit):       |");
            Console.WriteLine("└──────────────────────────────────────────────┘");
            char key = Console.ReadKey().KeyChar;
            Console.WriteLine(); // Add a new line after the key press
            return key;
        }
    }
}
