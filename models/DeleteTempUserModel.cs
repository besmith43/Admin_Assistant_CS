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
    public class DeleteTempUserModel
    {
        public async Task<string> runScript(string tempUserAccount)
        {
            Debug.Writeline("Started Delete Temp User runScript Function");

            await Task.Delay(TimeSpan.FromMilliseconds(500));

            var scriptContents = new StringBuilder();

            var scriptParameters = new Dictionary<string, object>()
            {
                { "TempUserName", tempUserAccount }
            };

            var modulesToLoad = new string[] { "Microsoft.PowerShell.Utility" };

            #if DEBUG
                scriptContents.AppendLine("param(");
                scriptContents.AppendLine("    [string]$TempUserName");
                scriptContents.AppendLine(")");
                scriptContents.AppendLine("");
                scriptContents.AppendLine("if ($TempUserName -eq \"testing\")");
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
                scriptContents.AppendLine("    [string]$TempUserName");
                scriptContents.AppendLine(")");
                scriptContents.AppendLine("");
                scriptContents.AppendLine("$temp = get-localuser $TempUserName");
                scriptContents.AppendLine("");
                scriptContents.AppendLine("if ($temp)");
                scriptContents.AppendLine("{");
                scriptContents.AppendLine("	remove-localuser $temp");
                scriptContents.AppendLine("}");
            #endif

            var pwshSession = new PSCore();
            pwshSession.InitializeRunspaces(2, 10, modulesToLoad);

            await pwshSession.RunScript(scriptContents.ToString(), scriptParameters);

            return "Done";
        }
    }
}