using System;
using System.Windows.Forms;
using System.IO;
using ServiceManagers;

namespace CrossCutting
{

    public class FileExtensionManager : IFileExtensionManager
    {
        string path = Application.StartupPath + @"\Dumps";
        public string[] scanAndReadDumpfileNames()
        {
            /*Production code to open and process the dump file goes here. Returning string array containing the file specifications for the dump file for all the system failures. 
            */
            string[] dumpFileNames = System.IO.Directory.GetFiles(path,"*.dmp", System.IO.SearchOption.TopDirectoryOnly);
            return dumpFileNames;
        }

        public string readAndDeleteDumpfile(string dumpFilespec)
        {
            string readMeText;
            using (StreamReader readtext = new StreamReader(dumpFilespec))
            {
                readMeText = readtext.ReadLine();
            }
            System.IO.File.Delete(dumpFilespec); // delete the dump file as I don't want to process it again. 
            if (readMeText.Contains("Exception thrown"))  // This is how exception is primed
            {
                return "Exception";
            }
            else
            {
                return "Normal";
            }
        }
      
    }

    public class CrashLoggingService : ICrashLoggingService
    {
        public void LogError(string message, string dumpFilespecAndStatus)
        {
           
            if (dumpFilespecAndStatus.Contains("Normal"))
            { 
                // Production code here to process the dump file
                //  and will throw an exception if there is a problem. This is still under construction.
                // etc.
              System.Windows.Forms.MessageBox.Show("System Monitor - Crash Logger Service Called" + "\n" + "You should not be seeing this if you are executing unit tests!!!!");
                // Don't use windows forms from a DLL - UI is for the client layer! The form here so to ensure you know you can't call this from the test as it is a dependency!!

            }
            else // contains exception - primed from the dump file contents
            {
                throw new Exception("CrashLoggingService threw an Exception");
            }
        } 
    }

    public class EmailService:IEmailService
    {
        public void SendEmail(string to, string subject, string body)
        {
            // Production code here which invokes email 
            // service and passes the details.
            // etc.
           System.Windows.Forms.MessageBox.Show("System Monitor - Email service called to Email Administrator as a Logger Service crashed. Emailing admin: " + to+" subject: "+subject + "\n" + "You should not be seeing this if you are executing unit tests!!!!");
            // Don't use windows forms from a DLL - UI is for the client layer! The form here so to ensure you know you can't call this from the test as it is a dependency!!
        }
    }

    public class CorruptFileLoggingService:ICorruptFileLoggingService
    {
        public void LogCorruptionDetails(string message, string dumpFilespecAndStatus)
        {
          
            if (dumpFilespecAndStatus.Contains("Normal"))
            {
                System.Windows.Forms.MessageBox.Show("System Monitor - Corrupt File Logger Service Called " + "\n" + "You should not be seeing this if you are executing unit tests!!!!");
                // Don't use windows forms from a DLL - UI is for the client layer! The form here so to ensure you know you can't call this from the test as it is a dependency!!!
            }
            else // contains exception - primed from the dump file contents
            {
                throw new Exception("CorruptLoggingService threw an Exception");
            }
        } 
    }
}



