using System;
using System.IO;
using Qml.Net;

namespace Admin_Assistant_CS
{
    public class EnableAdminAccountModel
    {
        private string scriptOutput = "";

        public void runScript(string password, string hostname)
        {
            string pwshArgs = "-ExecutionPolicy Bypass -NoProfile -WindowStyle Minimized -file " + Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + @"\scripts\enable_admin_account.ps1 -LocalAdminPass " + password + " -ComputerName " + hostname;

            string pwshArgsTest = string.Format(@"-ExecutionPolicy Bypass -NoProfile -WindowStyle Minimized -file ""{0}\scripts\test_pwsh.ps1"" -LocalAdminPass {1} -ComputerName {2}", Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), password, hostname);

            Console.WriteLine(pwshArgsTest);

            var pwshProcess = System.Diagnostics.Process.Start("powershell.exe", pwshArgsTest);
            pwshProcess.StartInfo.RedirectStandardOutput = true;
            pwshProcess.Start();
            
            StreamReader reader = pwshProcess.StandardOutput;
            string pwshOutput = reader.ReadToEnd();
            pwshProcess.WaitForExit();

            scriptOutput = pwshOutput;
        }

        [NotifySignal]
        public string getPwshOutput
        {
            get
            {
                return scriptOutput;
            }
            set
            {
                if (scriptOutput == value)
                {
                    return;
                }

                scriptOutput = value;
                this.ActivateSignal("scriptOutputChanged");
            }
        }
    }
}