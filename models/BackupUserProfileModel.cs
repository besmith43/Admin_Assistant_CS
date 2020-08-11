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
    public class BackupUserProfileModel
    {
        public async Task<string> runScript(string UserProfile)
        {
            Debug.Writeline("starting backup user profile RunScript");

            await Task.Delay(TimeSpan.FromMilliseconds(500));

            Debug.Writeline("declaring variables");

            var scriptContents = new StringBuilder();

            var scriptParameters = new Dictionary<string, object>()
            {
                { "user", UserProfile }
            };

            var modulesToLoad = new string[] { "Microsoft.PowerShell.Utility" };

            Debug.Writeline("declaring the script to be run");

            #if DEBUG
                scriptContents.AppendLine("param(");
                scriptContents.AppendLine("    [string]$user");
                scriptContents.AppendLine(")");
                scriptContents.AppendLine("");
                scriptContents.AppendLine("if ($user -eq \"testing\")");
                scriptContents.AppendLine("{");
                scriptContents.AppendLine("    Write-Information \"input field is testing\"");
                scriptContents.AppendLine("    $code = 0");
                scriptContents.AppendLine("    exit $code");
                scriptContents.AppendLine("}");
                scriptContents.AppendLine("else");
                scriptContents.AppendLine("{");
                scriptContents.AppendLine("    Write-Error \"bad input\"");
                scriptContents.AppendLine("    $code = 1");
                scriptContents.AppendLine("    exit $code");
                scriptContents.AppendLine("}");
            #else
                scriptContents.AppendLine("param(");
                scriptContents.AppendLine("    [string]$user");
                scriptContents.AppendLine(")");
                scriptContents.AppendLine("");
                scriptContents.AppendLine("if ($(test-path -path C:\\Temp\\USMT))");
                scriptContents.AppendLine("{");
                scriptContents.AppendLine("    if ($(test-path -path C:\\Users\\$user))");
                scriptContents.AppendLine("    {");
                scriptContents.AppendLine("        $backup_job = start-job -ScriptBlock {set-location -path \"C:\\Temp\\USMT\"; start-process -FilePath \"C:\\Temp\\USMT\\scanstate\" -ArgumentList \"C:\\Temp\\USMT /ue:*\\* /ui:ttu\\$user /i:migapp.xml /i:miguser.xml /config:config.xml /localonly /o /efs:copyraw /c /v:13\" -wait} ");
                scriptContents.AppendLine("");
                scriptContents.AppendLine("        $backup_job | Wait-Job");
                scriptContents.AppendLine("    }");
                scriptContents.AppendLine("}");
            #endif

            Debug.Writeline("declaring PSCore");

            var pwshSession = new PSCore();

            Debug.Writeline("initializing runspaces");

            pwshSession.InitializeRunspaces(2, 10, modulesToLoad);

            Debug.Writeline("triggering pwsh script");

            await pwshSession.RunScript(scriptContents.ToString(), scriptParameters);

            Debug.Writeline("done runnning script and exiting function");

            return "Done";
        }
    }
}