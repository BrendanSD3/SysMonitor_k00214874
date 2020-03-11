using ServiceManagers;
using System;
using System.IO;
using System.Windows.Forms;


namespace CrossCutting
{
    public class SystemMonitor
    {
        private IFileExtensionManager fileManager;
        private ICrashLoggingService crashLogger;
        private ICorruptFileLoggingService corruptFileLogger;
        //private EmailService emailLogger;
        private IEmailService emailLogger;
        //private CorruptFileLoggingService corruptFileLogger;
        string path = Application.StartupPath + @"/Dumps";
        string dumpFileStatus;
        public SystemMonitor(IFileExtensionManager theFileManager, ICrashLoggingService theCrashLogger, ICorruptFileLoggingService thecorruptfilelogger, IEmailService theemailservicelogger)//this constructor could get longer
        {
            fileManager = theFileManager;
            crashLogger = theCrashLogger;
            emailLogger = theemailservicelogger;
            //etc  2 more needed here Email logger and corruptfilelogging service
            //emailLogger = new EmailService();
            corruptFileLogger = thecorruptfilelogger;
        }

        public SystemMonitor()
        {
            fileManager = new FileExtensionManager();
            crashLogger = new CrashLoggingService();
            emailLogger = new EmailService();
            corruptFileLogger = new CorruptFileLoggingService();
  
        }
       
        public void ProcessDumps()
        {
            string[] dumpFileNames;

            createDumpFolderIfNecessary();

            dumpFileNames = fileManager.scanAndReadDumpfileNames();   // Off to disk to read what dump files are there
                if (dumpFileNames.Length != 0)
                {
                    foreach (string filespec in dumpFileNames)  // Stay in loop processing each dump file. 
                    {
                        if (filespec.Contains(".dmp") && filespec.Length < 1600)  // business logic to check the dump file is valid
                        {
                            /* Dump file valid so log details with the crashLoggingService Web service. */
                             dumpFileStatus = fileManager.readAndDeleteDumpfile(filespec);
                            try
                            {
                                crashLogger.LogError("Dump file is valid ",filespec+ dumpFileStatus);  // Call the crashLogger to record the crash
                                // called crashLoggingService
                             
                            }
                            catch (Exception e)
                            {
                                // The CrashLogger threw an exception so use the emailLogger to email the helpdesk
                                emailLogger.SendEmail("HelpDesk@lit.ie", "crashLoggingService Web service threw exception", e.Message);
                            }
                        }
                        else
                        {
                            /* Dump file is invalid so log details with the corruptFileLoggingService Web service. */
                            dumpFileStatus = fileManager.readAndDeleteDumpfile(filespec);
                            try
                            {
                                corruptFileLogger.LogCorruptionDetails("Dump file is corrupt: ", filespec + dumpFileStatus); // Call the coppuptFileLogger as the crash resulted in a corrupt dump file. 
                            }
                            catch (Exception e)
                            {
                                // The corruptFileLogger threw an exception so use the emailLogger to email the helpdesk
                                emailLogger.SendEmail("HelpDesk@lit.ie", "corruptFileLoggingService Web service threw exception", e.Message);
                            }
                        }
                    }
            }

        }
       
            protected virtual void createDumpFolderIfNecessary() // made this virtual
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
       
            }
    }
    }
