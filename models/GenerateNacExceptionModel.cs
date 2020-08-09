using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Qml.Net;
using BSStandard.Utilities.Work;

namespace Admin_Assistant_CS
{
    public class GenerateNacExceptionModel
    {
        public async Task<string> runScript()
        {
            await Task.Delay(TimeSpan.FromMilliseconds(500));

            string csvPath;

            #if DEBUG
                csvPath = Environment.CurrentDirectory;
            #else
                csvPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            #endif

            GenerateNACException NACException = new GenerateNACException();
            return NACException.ComputerRun(csvPath);
        }
    }
}