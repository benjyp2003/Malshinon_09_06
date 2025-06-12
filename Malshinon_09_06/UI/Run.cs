using Malshinon_09_06.DAL;
using Malshinon_09_06.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Malshinon_09_06
{
    internal class RunningFunctions : UserInterface
    {
        static RunningFunctions Instance = null;
        static PeopleDal Dal = PeopleDal.GetInstance();
        static IntelReportsDal ReportDal = IntelReportsDal.GetInstance(); 

        RunningFunctions() { }

        public static RunningFunctions GetInstance()
        {
            if (Instance == null)
            {
                Instance = new RunningFunctions();
            }
            return Instance;
        }


        public void HandleMenuChoice()
        {
            while (true)
            {

                ShowAnalysisMenu();

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        CreateReport();
                        break;

                    case "2":
                        Console.WriteLine("Enter Persons full name.");
                        string name = Console.ReadLine();
                        string[] arrName = name.Trim().Split(' ');
                        FullName fullName = new FullName(arrName[0], arrName[1]);
                        Console.WriteLine("\n*** Persons INFO: ***");
                        Console.WriteLine(Dal.GetPersonByName(fullName));
                        break;

                    case "3":
                        Console.WriteLine("Enter Persons ID.");
                        int id = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("\n*** Persons INFO: ***");
                        Console.WriteLine(Dal.GetPersonById(id));
                        break;

                    case "4":
                        Dal.GetAllTargets().ForEach(person => Console.WriteLine(person));
                        break;
                    
                    case "5":
                        Dal.GetAllReporters().ForEach(person => Console.WriteLine(person));
                        break;

                    case "6":
                        Dal.GetAllPotentialAgents().ForEach(person => Console.WriteLine(person));
                        break;

                    case "7":
                        Dal.GetAllDangerousTargets().ForEach(person => Console.WriteLine(person));
                        break;

                    case "8":
                        Console.WriteLine("not available yet.");
                        break;

                    case "0":
                        return;

                    default:
                        Console.WriteLine("Not a valid input.");
                        break;

                }

                char breakChar = ExitOption();
                if (breakChar == 'e' || breakChar == 'E')
                {
                    break;
                }
                Console.Clear();
            }
        }

        public void CreateReport()
        {
            FullName reporterFullName = GetReportersFullName();
            string text = GetReport();
            FullName targetFullName = FilterNameFromText.FilterAndGetName(text);

            Dal.HandleReporterName(reporterFullName);
            Dal.HandleTargetName(targetFullName);

            int reporterId = Convert.ToInt32(Dal.GetColomnByName(reporterFullName, "id"));
            int targetId = Convert.ToInt32(Dal.GetColomnByName(targetFullName, "id"));

            IntelReports report = new IntelReports(reporterId, targetId, text);
            ReportDal.HandleReports(report);
        }

        public void Start()
        {
            HandleMenuChoice();
        }

        
        char ExitOption()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine();
            Console.WriteLine("┌──────────────────────────────────────────────┐");
            Console.WriteLine("│       Press any key  (or 'E' to exit):       |");
            Console.WriteLine("└──────────────────────────────────────────────┘");
            char key = Console.ReadKey().KeyChar;
            Console.WriteLine(); // Add a new line after the key press
            Console.ForegroundColor= ConsoleColor.White;
            return key;
        }
    }
}
