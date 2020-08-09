using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Qml.Net;

namespace Admin_Assistant_CS
{
    public class ConnectToDomainModel
    {
        public async Task<string> runScript()
        {
            await Task.Delay(TimeSpan.FromMilliseconds(500));

            #if !DEBUG
                string pwshParameters = $"-ExecutionPolicy Bypass -NoProfile -WindowStyle Minimized -file { Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) }\\resources\\scripts\\connect_to_domain.ps1\"";
                var pwshProcess = System.Diagnostics.Process.Start("powershell.exe", pwshParameters);
                pwshProcess.Start();
                pwshProcess.WaitForExit();
            #endif

            return "Done";
        }
    }
}