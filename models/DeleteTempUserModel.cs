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

            Debug.Writeline("Setting up script variables");

            var scriptContents = new StringBuilder();

            var scriptParameters = new Dictionary<string, object>()
            {
                { "TempUserName", tempUserAccount }
            };

            var modulesToLoad = new string[] { "Microsoft.PowerShell.Utility" };

            #if DEBUG

                Debug.Writeline("Start Defining Debug Script");

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

                Debug.Writeline("Done Defining Debug Script");
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

            Debug.Writeline("Starting PSCore");

            var pwshSession = new PSCore();

            Debug.Writeline("Initializing Runspaces");

            pwshSession.InitializeRunspaces(2, 10, modulesToLoad);

            Debug.Writeline("calling script in PSCore");

            await pwshSession.RunScript(scriptContents.ToString(), scriptParameters);

            Debug.Writeline("Done running script \nabout to exit");

            return "Done";
        }
    }
}