# SystemMonitor CA assignment 
## Task: To Test the system monitor process dumps method
------------------------
### Observations made on the current system monitor code:
* The code contains lots of dependencies to the Service Managers class making it difficult to test the code.
* The Processdumps() method requires reading from a file in order to function,this is something that needs to be addressed when testing as it is not good practice to read from a file when testing.
------------------------ 
# Proposed Solution
### Dependency Injection
*  Dependency injection is a software design pattern that implements inversion of control for resolving dependencies. A dependency is an object that can be used (a service). An injection is the passing of a dependency to a dependent object (a client) that would use it.

### Proposed changes to be made in order to test:
1. To implement Dependency injection, The first change needed is to create an interface for each of the 4 Service Manager classes:    
 * FileExtensionManager, 
 * CrashLoggingService, 
 * EmailService and 
 * CorruptFileLoggingService.  
2. From within the Unit Test class a fake version of each of the service manager classes is made. 
3. When writing the tests the fake version of the classes can be called this includes a what has been named the testableSystemMonitor class. So the test code is using the same methods but is able to test each one in isolation without calling the dependencys.
### Test Cases:
1. Test for Valid dump file with expected call to the crashlogger.
2. Test for invalid dump file which expects a call to the corruptfileLogger.
3. Test an exception which is thrown in the crash logger this expects a call to the sendemailService.
4. Test for an exception thrown in the corruptfileLogger which expects a call to the sendemailService.
5. A test was made for the business logic which involves two checks, check if the dump file has the correct file extension e.g. .dmp and the second is to check if the length of the dump file spec is less than 1600.
### Other things to note:
* In following best practice there was minimal change to the production code, the only change being the creation of interfaces for each service class for dependancy injection.
* As mentioned above the production code requires interaction with files, both creation and deletion of files, In testing this is not best practice as it is not required onless specifically testing whether the fie has been created or not. To prevent interacting with a file or folder in the Test code, the suggestion is to extract and override the createdumpfolderifnecessary method and leave it blank which simulates calling the method without the need of FileIO.


