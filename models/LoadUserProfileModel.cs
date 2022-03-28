using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Qml.Net;
using BSStandard.Utilities.Scripting;
using BSStandard.Utilities;

namespace Admin_Assistant_CS
{
    public class LoadUserProfileModel
    {
        public async Task<string> runScript()
        {
            Debug.Writeline("starting Load User Profile RunScript");

            await Task.Delay(TimeSpan.FromMilliseconds(500));

            Debug.Writeline("declaring script variables");

            var scriptContents = new StringBuilder();

            var modulesToLoad = new string[] { "Microsoft.PowerShell.Utility" };

            Debug.Writeline("declaring script");

            #if DEBUG
                scriptContents.AppendLine("write-information \"running pwsh\"");
            #else
                scriptContents.AppendLine("if ($(test-path -path C:\\Temp\\USMT))");
                scriptContents.AppendLine("{");
                scriptContents.AppendLine("    $load_job = start-job -ScriptBlock {set-location -path \"C:\\Temp\\USMT\"; start-process -FilePath \"C:\\Temp\\USMT\\loadstate\" -argumentlist \"C:\\Temp\\USMT\\ /i:migapp.xml /i:miguser.xml /config:config.xml /c /v:13\" -wait} ");
                scriptContents.AppendLine("");
                scriptContents.AppendLine("    $load_job | Wait-Job");
                scriptContents.AppendLine("}");
            #endif

            Debug.Writeline("starting PSCore");

            var pwshSession = new PSCore();

            Debug.Writeline("initializing runspaces");

            pwshSession.InitializeRunspaces(2, 10, modulesToLoad);

            Debug.Writeline("starting pwsh script");

            await pwshSession.RunScript(scriptContents.ToString());

            Debug.Writeline("done running script and exiting function");

            return "Done";
        }
    }
}