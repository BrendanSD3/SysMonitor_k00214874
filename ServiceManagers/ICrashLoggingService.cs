using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceManagers
{
    public interface ICrashLoggingService
    {
        void LogError(string message, string dumpFilespecAndStatus);
    }
}
