using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Qml.Net;
using BSStandard.Utilities.Work;
using BSStandard.Utilities;

namespace Admin_Assistant_CS
{
    public class GenerateNacExceptionModel
    {
        public async Task<string> runScript()
        {
            Debug.Writeline("starting generate NAC exception RunScript");

            await Task.Delay(TimeSpan.FromMilliseconds(500));

            Debug.Writeline("declaring variables");

            string csvPath;

            #if DEBUG
                csvPath = Environment.CurrentDirectory;
            #else
                csvPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            #endif

            Debug.Writeline("starting NACException class instance");

            GenerateNACException NACException = new GenerateNACException();

            Debug.Writeline("generating csv and exiting function");
            
            return NACException.ComputerRun(csvPath);
        }
    }
}