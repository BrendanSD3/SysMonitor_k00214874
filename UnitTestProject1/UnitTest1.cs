using System;
using CrossCutting;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceManagers;
using NUnit.Framework;

namespace UnitTestProject1
{
    [TestFixture]
    public class UnitTest1
    {
        [Test]
        public void TestProcessDumps_ValidDumpFile_ExpectCallToCrashLoggerLogError()//Test for invalid file needed this only looks for valid file
        {
            //Arrange
            fakeEmailService fakeEmailService = new fakeEmailService();
            FakeFileExtensionManager theFakeFileExtensionMgr = new FakeFileExtensionManager();
            FakeCrashLoggingService theFakeCrashLogger = new FakeCrashLoggingService();
            FakeCorruptFileLoggingService thecorruptFile = new FakeCorruptFileLoggingService();
            testableSystemMonitor theSysMon = new testableSystemMonitor(theFakeFileExtensionMgr, theFakeCrashLogger,thecorruptFile, fakeEmailService);
            String expectedResult = "Dump file is valid ";
            //Act
            theSysMon.ProcessDumps();
            //Assert
            Assert.AreEqual(theFakeCrashLogger.lastCalledWithMessage, expectedResult);
            // assert against the FakeCrashLogger to check it was called properly.
        }
        [Test]
        public void TestProcessDumps_notDMPandlessthan1600_ExpectcalltoCorruptlogger()
        {   //Arrange
            fakeEmailService fakeEmailService = new fakeEmailService();
            FakeFileExtensionManager theFakeFileExtensionMgr = new FakeFileExtensionManager();
            FakeCrashLoggingService theFakeCrashLogger = new FakeCrashLoggingService();
            FakeCorruptFileLoggingService thecorruptFile = new FakeCorruptFileLoggingService();
            testableSystemMonitor theSysMon = new testableSystemMonitor(theFakeFileExtensionMgr, theFakeCrashLogger, thecorruptFile, fakeEmailService);
            String expectedResult = "Dump file is corrupt: ";
            theFakeFileExtensionMgr.notdotdmp = true;//Setting to true means no dot dumpfile extension
            //Act
            theSysMon.ProcessDumps();
            //Assert
            Assert.AreEqual(thecorruptFile.lastCalledWithMessage, expectedResult);
            // assert against the FakeCrashLogger to check it was called properly.
        }
        [Test]
        public void TestProcessDumps_Invalidfile_ExpectcallToCorruptfileloggerLogCorruptionDetails()
        {
            //Arrange
            fakeEmailService fakeEmailService = new fakeEmailService();
            FakeCorruptFileLoggingService thecorruptFile = new FakeCorruptFileLoggingService();
            FakeFileExtensionManager theFakeFileExtensionMgr = new FakeFileExtensionManager();
            FakeCrashLoggingService theFakeCrashLogger = new FakeCrashLoggingService();
            testableSystemMonitor theSysMon = new testableSystemMonitor(theFakeFileExtensionMgr, theFakeCrashLogger,thecorruptFile, fakeEmailService);
            String expectedResult = "Dump file is corrupt: ";
            //Act
            //string[] result = theFakeFileExtensionMgr.scanAndReadDumpfileNames();
            theFakeFileExtensionMgr.notdotdmp = true;
            theFakeFileExtensionMgr.notless1600 = true;
            theSysMon.ProcessDumps();
            //Assert
            Assert.AreEqual(thecorruptFile.lastCalledWithMessage, expectedResult);
            // assert against the FakeCrashLogger to check it was called properly.
        }
        [Test]
        public void TestProcessDumps_ExceptionInCrashLoggerCalls_ExpectSendEmail()
        {
            //Arrange
            fakeEmailService fakeEmailService = new fakeEmailService();
            FakeCorruptFileLoggingService thecorruptFile = new FakeCorruptFileLoggingService();
            FakeFileExtensionManager theFakeFileExtensionMgr = new FakeFileExtensionManager();
            FakeCrashLoggingService theFakeCrashLogger = new FakeCrashLoggingService();
            testableSystemMonitor theSysMon = new testableSystemMonitor(theFakeFileExtensionMgr, theFakeCrashLogger, thecorruptFile, fakeEmailService);
           
            Boolean expectedResult = true;

            theFakeCrashLogger.primeExeption = true;


            //Act
            theSysMon.ProcessDumps();
            //Assert
            Assert.AreEqual(fakeEmailService.emailsent, expectedResult);
            // assert against the emailservice to check it was called properly.
        }

        [Test]
        public void TestProcessDumps_ExceptionInCorruptFileLogger_Expectcalltosendemail()
        {
            //Arrange
            fakeEmailService fakeEmailService = new fakeEmailService();
            FakeCorruptFileLoggingService thecorruptFile = new FakeCorruptFileLoggingService();
            FakeFileExtensionManager theFakeFileExtensionMgr = new FakeFileExtensionManager();
            FakeCrashLoggingService theFakeCrashLogger = new FakeCrashLoggingService();
            testableSystemMonitor theSysMon = new testableSystemMonitor(theFakeFileExtensionMgr, theFakeCrashLogger, thecorruptFile, fakeEmailService);
            Boolean expectedResult = true;
            //Act
            //string[] result = theFakeFileExtensionMgr.scanAndReadDumpfileNames();
            theFakeFileExtensionMgr.notdotdmp = true;
            theFakeFileExtensionMgr.notless1600 = true;
            thecorruptFile.FilestatusNotNormalException = true;
            theSysMon.ProcessDumps();
            //Assert
            Assert.AreEqual(fakeEmailService.emailsent, expectedResult);
            // assert against the FakeCrashLogger to check it was called properly.



        }



        public class FakeCorruptFileLoggingService : ICorruptFileLoggingService
        {
            public string lastCalledWithMessage = null;
            public string lastCalledWithdumpFilespecAndStatus = null;
            public Boolean FilestatusNotNormalException = false;
            public void LogCorruptionDetails(string message, string dumpFilespecAndStatus)
            {
                lastCalledWithMessage = message;
                lastCalledWithdumpFilespecAndStatus = dumpFilespecAndStatus;

                if (FilestatusNotNormalException)
                {
                    throw new Exception();
                }
            }
        }
        public class FakeFileExtensionManager : IFileExtensionManager
        {
            string path = null;
            public Boolean notdotdmp = false;
            public Boolean notless1600 = false;
            public string[] scanAndReadDumpfileNames()
            {

                if (notdotdmp || notless1600)//simulates case when file is notdotdump or not less1600
                {
                    string[] theResult = { "LogFile" };
                    return theResult;
                }
                else if (!notdotdmp && !notless1600)//When both are false this is valid e.g. file is (.dmp and <1600)
                {
                    string[] theRexsult = { "LogFile.dmp" };
                    return theRexsult;
                }
                else 
                {
                    string[] theRes = {""};//this wont be reached as the case will always hit either if or else if 
                    return theRes;


                }
            }

            public string readAndDeleteDumpfile(string dumpFilespec)
            {
                return "something";

            }
        }

        public class FakeCrashLoggingService : ICrashLoggingService
        {
            public string lastCalledWithMessage = null;
            public string lastCalledWithdumpFilespecAndStatus = null;
            // remembers how it was called

            public Boolean primeExeption = false;

            public void LogError(string message, string dumpFilespecAndStatus)
            {
                lastCalledWithMessage = message;
                lastCalledWithdumpFilespecAndStatus = dumpFilespecAndStatus;

                if (primeExeption)
                {
                    throw new Exception();
                }
            }

        }
        public class fakeEmailService : IEmailService
        {
            public string lastCalledWithMessage = null;
            public Boolean emailsent = false;
            public void SendEmail(string to, string subject, string body)
            {
                emailsent = true;
                //lastCalledWithMessage = to + subject + body;
            }
        }








        public class testableSystemMonitor : SystemMonitor
        {  //run tests against the subclass overriding the dependency
            public testableSystemMonitor(IFileExtensionManager theFileManager, ICrashLoggingService theCrashLogger,ICorruptFileLoggingService thecorruptFile,IEmailService theemailservicelogger)
        : base( theFileManager,  theCrashLogger,thecorruptFile,theemailservicelogger)
              {

            }
            protected override void createDumpFolderIfNecessary() //this is a fake 
            {
                // just do nothing, that's all the test needs 
            }
        }
    }
}
