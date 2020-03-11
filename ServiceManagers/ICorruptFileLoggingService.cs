namespace CrossCutting
{
    public interface ICorruptFileLoggingService
    {
       void LogCorruptionDetails(string message, string dumpFilespecAndStatus);

    }
}