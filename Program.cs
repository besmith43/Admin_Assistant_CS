using System;
using System.IO;
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

                    #if DEBUG
                         qmlEngine.Load("pages/main.qml");
                    #else
                        qmlEngine.Load($"{Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location)}/pages/main.qml");
                    #endif
                    
                    return application.Exec();
                }
            }
        }

        public static void GetVersion()
        {
            Console.WriteLine($"CmdLineParsingTest Version: { VersionNumber }");
        }
    }
}
