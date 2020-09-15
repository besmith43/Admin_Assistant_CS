using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Qml.Net;
using BSStandard.Utilities;

namespace Admin_Assistant_CS
{
    public class ConnectToDomainModel
    {
        public async Task<string> runScript()
        {
            Debug.Writeline("Started Connect to Domain runScript Function");

            await Task.Delay(TimeSpan.FromMilliseconds(500));

            #if !DEBUG
                string pwshParameters = $"-ExecutionPolicy Bypass -NoProfile -file { Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) }\\scripts\\connect_to_domain.ps1\"";
                var pwshProcess = System.Diagnostics.Process.Start("powershell.exe", pwshParameters);
                pwshProcess.Start();
                pwshProcess.WaitForExit();
            #endif

            Debug.Writeline("Done running script \nabout to exit");

            return "Done";
        }
    }
}