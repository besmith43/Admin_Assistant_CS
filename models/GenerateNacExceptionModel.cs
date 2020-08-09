using System;
using System.IO;
using System.Threading.Tasks;
using Qml.Net;
using Generate_NACException;

namespace Admin_Assistant_CS
{
    public class GenerateNacExceptionModel
    {
        public async Task<string> runScript()
        {
            await Task.Delay(TimeSpan.FromMilliseconds(500));

            return "Done";
        }
    }
}