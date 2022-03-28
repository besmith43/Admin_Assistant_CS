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
    public class GenerateUsmtXmlModel
    {
        public async Task<string> runScript()
        {
            Debug.Writeline("starting generate USMT xml runscript");

            await Task.Delay(TimeSpan.FromMilliseconds(500));

            Debug.Writeline("declaring variables for script");

            var scriptContents = new StringBuilder();

            var modulesToLoad = new string[] { "Microsoft.PowerShell.Utility" };

            Debug.Writeline("declaring script");

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

            Debug.Writeline("starting PSCore");

            var pwshSession = new PSCore();

            Debug.Writeline("intializing runspaces");

            pwshSession.InitializeRunspaces(2, 10, modulesToLoad);

            Debug.Writeline("starting pwsh script");

            await pwshSession.RunScript(scriptContents.ToString());

            Debug.Writeline("done running script and exiting function");

            return "Done";
        }
    }
}