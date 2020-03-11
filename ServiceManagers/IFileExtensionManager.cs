using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossCutting
{
    public interface IFileExtensionManager
    {
        string readAndDeleteDumpfile(string dumpFilespec);
        string[] scanAndReadDumpfileNames();
    }
}
