using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Qml.Net;
using BSStandard.Utilities.Scripting;

namespace Admin_Assistant_CS
{
    public class LoadUserProfileModel
    {
        public async Task<string> runScript()
        {
            await Task.Delay(TimeSpan.FromMilliseconds(500));

            var scriptContents = new StringBuilder();

            var modulesToLoad = new string[] { "Microsoft.PowerShell.Utility" };

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

            var pwshSession = new PSCore();
            pwshSession.InitializeRunspaces(2, 10, modulesToLoad);

            await pwshSession.RunScript(scriptContents.ToString());

            return "Done";
        }
    }
}