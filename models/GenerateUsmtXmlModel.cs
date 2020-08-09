using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Qml.Net;
using BSStandard.Utilities.Scripting;

namespace Admin_Assistant_CS
{
    public class GenerateUsmtXmlModel
    {
        public async Task<string> runScript()
        {
            await Task.Delay(TimeSpan.FromMilliseconds(500));

            var scriptContents = new StringBuilder();

            var modulesToLoad = new string[] { "Microsoft.PowerShell.Utility" };

            #if DEBUG
                scriptContents.AppendLine("write-information \"running pwsh\"");
            #else
                scriptContents.AppendLine("if (!$(test-path C:\\Temp))");
                scriptContents.AppendLine("{");
                scriptContents.AppendLine("    new-item -itemtype Directory -path C:\\Temp");
                scriptContents.AppendLine("    $(get-item -path C:\\Temp -Force).attributes = \"Hidden\"");
                scriptContents.AppendLine("}");
                scriptContents.AppendLine("");
                scriptContents.AppendLine("if (test-path -path \"C:\\Temp\\USMT\")");
                scriptContents.AppendLine("{");
                scriptContents.AppendLine("    remove-item -path \"C:\\Temp\\USMT\\\" -Recurse -force");
                scriptContents.AppendLine("}");
                scriptContents.AppendLine("");
                scriptContents.AppendLine("copy-item -Path \"$PSScriptRoot\\USMT\\\" -Destination \"C:\\Temp\\\" -Recurse -Force");
                scriptContents.AppendLine("");
                scriptContents.AppendLine("$scan_job = start-job -scriptblock {set-location -path \"C:\\Temp\\USMT\\\"; start-process -FilePath \"C:\\Temp\\USMT\\scanstate\" -ArgumentList \"/i:migapp.xml /i:miguser.xml /genconfig:config.xml /c\" -wait}");
                scriptContents.AppendLine("");
                scriptContents.AppendLine("$scan_job | Wait-Job");
            #endif

            var pwshSession = new PSCore();
            pwshSession.InitializeRunspaces(2, 10, modulesToLoad);

            await pwshSession.RunScript(scriptContents.ToString());

            return "Done";
        }
    }
}