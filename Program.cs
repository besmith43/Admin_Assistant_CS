using System;
using System.IO;
using System.Net;
using System.Diagnostics;
using System.Security.Principal;
using System.Windows.Forms;
using Qml.Net;
using Qml.Net.Runtimes;
using McMaster.Extensions.CommandLineUtils;

namespace Admin_Assistant_CS
{
    class Program
    {
        public static int Main(string[] args) => CommandLineApplication.Execute<Program>(args);

        [Option(Description = "Version", ShortName = "V")]
        public bool Version { get; }

        public static string VersionNumber = "0.1.0";

        private int OnExecute()
        {
            if (Version)
            {
                GetVersion();
                return 0;
            }

            if (IsWindows())
            {
                if (!IsAdministrator())
                {
                    RuntimeError("Please make sure to run this application as Administrator", "Not Admin");
                    return 1;
                }

                if (!TestVCRuntime())
                {
                    InstallVCRuntime();
                }
            }

            RuntimeManager.DiscoverOrDownloadSuitableQtRuntime();
            
            QQuickStyle.SetStyle("Material");

            using (var application = new QGuiApplication())
            {
                using (var qmlEngine = new QQmlApplicationEngine())
                {
                    // register Models
                    // don't forget that models must start with a capital letter and they can't have special characters in the import library name
                    //Qml.Net.Qml.RegisterType<Model>("AdminAssistant");

                    // New Computer Setup
                    Qml.Net.Qml.RegisterType<EnableAdminAccountModel>("AdminAssistant");
                    Qml.Net.Qml.RegisterType<DeleteTempUserModel>("AdminAssistant");
                    Qml.Net.Qml.RegisterType<ConnectToDomainModel>("AdminAssistant");
                    Qml.Net.Qml.RegisterType<GenerateNacExceptionModel>("AdminAssistant");

                    // USMT Backup
                    Qml.Net.Qml.RegisterType<GenerateUsmtXmlModel>("AdminAssistant");
                    Qml.Net.Qml.RegisterType<BackupUserProfileModel>("AdminAssistant");
                    Qml.Net.Qml.RegisterType<LoadUserProfileModel>("AdminAssistant");

                    qmlEngine.Load($"{Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location)}/pages/main.qml");
                    
                    return application.Exec();
                }
            }
        }

        public static void GetVersion()
        {
            Console.WriteLine($"CmdLineParsingTest Version: { VersionNumber }");
        }

        public static bool IsWindows() =>
            System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows);

        public static bool IsAdministrator()
        {
            var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        public static void RuntimeError(string message, string title)
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static bool TestVCRuntime()
        {
            return File.Exists(@"C:\Windows\System32\msvcp140.dll");
        }

        public static void InstallVCRuntime()
        {
            string installer = $"{Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location)}/VC_redist.x64.exe";

            if (!File.Exists(installer))
            {
                DownloadVCRuntime();
            }

            if (File.Exists(installer) && IsAdministrator())
            {
                var installerProcess = Process.Start(installer, "/install /quiet /noreboot");

                installerProcess.Start();
                installerProcess.WaitForExit();
            }
            else
            {
                RuntimeError("You need to run the VC++ installer","VC++ Runtime Missing");
            }
        }

        public static void DownloadVCRuntime()
        {
            using (var client = new WebClient())
            {
                client.DownloadFile("https://aka.ms/vs/16/release/vc_redist.x64.exe", $"{Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location)}/vc_redist.x64.exe");
            }
        }
    }
}
