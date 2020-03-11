using CrossCutting;
using System;
using System.IO;
using System.Windows.Forms;

namespace SystemMonitorV2
{
    class Program
    {
        static void Main(string[] args)
        {
            string choice = string.Empty;
        
            string path = Application.StartupPath+@"\Dumps";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }


            SystemMonitor monitorAll = new SystemMonitor();
            int userInput = 0;
            
            do
            {
             userInput = DisplayMenu();
                string dateAndTime;
                string filespec;
                switch (userInput)
                {
                    
                    case 1:
                       
                        monitorAll.ProcessDumps();
                        break;
                    case 2:
                      
                        break;

                    case 3:
                         dateAndTime = DateTime.Now.ToString("MMM-dd-yyyy-hh-mm-ss"); 
                         filespec= path+ @"\"+ dateAndTime+".dmp";
                        File.Create(filespec).Close();
                        using (StreamWriter writetext = new StreamWriter(filespec))
                        {
                            writetext.WriteLine("Normal dump");
                        }
                        break;
                    case 4:
                         dateAndTime = DateTime.Now.ToString("MMM-dd-yyyy-hh-mm-ss");
                         filespec = path + @"\" + dateAndTime + ".d£mp";
                        File.Create(filespec).Close();
                        using (StreamWriter writetext = new StreamWriter(filespec))
                        {
                            writetext.WriteLine("Corrupt dump");
                        }
                        break;
                    case 5:
                        dateAndTime = DateTime.Now.ToString("MMM-dd-yyyy-hh-mm-ss");
                        filespec = path + @"\" + dateAndTime + ".dmp";
                        File.Create(filespec).Close();
                        using (StreamWriter writetext = new StreamWriter(filespec))
                        {
                            writetext.WriteLine("Exception thrown the Crash File Logger");
                        }
                        break;
                    case 6:
                        dateAndTime = DateTime.Now.ToString("MMM-dd-yyyy-hh-mm-ss");
                        filespec = path + @"\" + dateAndTime + ".d£mp";
                        File.Create(filespec).Close();
                        using (StreamWriter writetext = new StreamWriter(filespec))
                        {
                            writetext.WriteLine("Exception thrown the Corrupt File Logger");
                        }
                        break;
                    case 7:
                       
                        break;
                    default:
                        break;

                }
            } while (userInput != 7);
          


            
           
          

         



            //monitorAll.ProcessDump("DumpFile.dmp");



        }
        private static int DisplayMenu()
        {
            Console.WriteLine("This system mimics a production system creating dump files");
            Console.WriteLine();
            Console.WriteLine("Enter 1. to start the System Monitor Process");
            Console.WriteLine("Enter 2. to request the System Monitor Process to stop");
            Console.WriteLine("Enter 3 to simulate a Production system crash producing a dump file");
            Console.WriteLine("Enter 4 to simulate a Production system crash producing a corrupt dump file");
            Console.WriteLine("Enter 5 to simulate the Crash File Logger throwing an exception");
            Console.WriteLine("Enter 6 to simulate the Corrupt File Logger throwing an exception");
            Console.WriteLine("Enter 7 to Exit");
            var result = Console.ReadLine();
            return Convert.ToInt32(result);
        }
    }
}
