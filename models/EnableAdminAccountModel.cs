using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Qml.Net;
using BSStandard.Utilities.Scripting;

namespace Admin_Assistant_CS
{
    public class EnableAdminAccountModel
    {
        public async Task<string> runScript(string password, string hostname)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(500));

            var scriptContents = new StringBuilder();

            var scriptParameters = new Dictionary<string, object>()
            {
                { "LocalAdminPass", password },
                { "ComputerName", hostname }
            };

            var modulesToLoad = new string[] { "Microsoft.PowerShell.Utility" };

            #if DEBUG
                scriptContents.AppendLine("param(");
                scriptContents.AppendLine("    [string]$LocalAdminPass,");
                scriptContents.AppendLine("    [string]$ComputerName");
                scriptContents.AppendLine(")");
                scriptContents.AppendLine("");
                scriptContents.AppendLine("if ($LocalAdminPass -eq \"testing\" -and $ComputerName -eq \"testing\")");
                scriptContents.AppendLine("{");
                scriptContents.AppendLine("    Write-Information \"both fields are testing\"");
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
                scriptContents.AppendLine("write-info \"not implemented yet\"");
            #endif

            var pwshSession = new PSCore();
            pwshSession.InitializeRunspaces(2, 10, modulesToLoad);

            await pwshSession.RunScript(scriptContents.ToString());

            return "Done";
        }
    }
}