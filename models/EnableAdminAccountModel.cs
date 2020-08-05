using System;
using System.IO;
using System.Threading.Tasks;
using Qml.Net;

namespace Admin_Assistant_CS
{
    public class EnableAdminAccountModel
    {
        public async Task<string> runScript(string password, string hostname)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(500));

            string pwshArgs = "-ExecutionPolicy Bypass -NoProfile -WindowStyle Minimized -file " + Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + @"\scripts\enable_admin_account.ps1 -LocalAdminPass " + password + " -ComputerName " + hostname;

            string pwshArgsTest = string.Format(@"-ExecutionPolicy Bypass -NoProfile -WindowStyle Minimized -file ""{0}\scripts\test_pwsh.ps1"" -LocalAdminPass {1} -ComputerName {2}", Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), password, hostname);

            Console.WriteLine(pwshArgsTest);

            var pwshProcess = System.Diagnostics.Process.Start("powershell.exe", pwshArgsTest);
            pwshProcess.StartInfo.RedirectStandardOutput = true;
            pwshProcess.Start();
            
            //StreamReader reader = pwshProcess.StandardOutput;
            //string pwshOutput = reader.ReadToEnd();
            pwshProcess.WaitForExit();

            //ScriptOutput = Convert.ToString(pwshProcess.ExitCode);
            return "Done";
        }
    }
}