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
                scriptContents.AppendLine("param(");
                scriptContents.AppendLine("    [string]$LocalAdminPass,");
                scriptContents.AppendLine("    [string]$ComputerName");
                scriptContents.AppendLine(")");
                scriptContents.AppendLine("");
                scriptContents.AppendLine("set-executionpolicy unrestricted -confirm -force");
                scriptContents.AppendLine("");
                scriptContents.AppendLine("$password = ConvertTo-SecureString $LocalAdminPass -AsPlainText -Force");
                scriptContents.AppendLine("$admin = get-localuser -name \"Administrator\"");
                scriptContents.AppendLine("$admin | enable-localuser");
                scriptContents.AppendLine("$admin | set-localuser -password $password -accountneverexpires -passwordneverexpires $True");
                scriptContents.AppendLine("");
                scriptContents.AppendLine("# set Time zone to central");
                scriptContents.AppendLine("");
                scriptContents.AppendLine("set-timezone \"Central Standard Time\"");
                scriptContents.AppendLine("");
                scriptContents.AppendLine("# set time zone automatically");
                scriptContents.AppendLine("");
                scriptContents.AppendLine("set-itemproperty -path \"HKLM:\\SYSTEM\\CurrentControlSet\\Services\\tzautoupdate\" -name \"Start\" -Value 3 -type DWORD");
                scriptContents.AppendLine("");
                scriptContents.AppendLine("# install dotnet framework 3.5");
                scriptContents.AppendLine("");
                scriptContents.AppendLine("if (!$(test-path C:\\Temp))");
                scriptContents.AppendLine("{");
                scriptContents.AppendLine("	new-item -itemtype Directory -path C:\\Temp");
                scriptContents.AppendLine("	$(get-item -path C:\\Temp -Force).attributes = \"Hidden\"");
                scriptContents.AppendLine("}");
                scriptContents.AppendLine("");
                scriptContents.AppendLine("copy-item -Path \"$PSScriptRoot\\sxs\\\" -Destination \"C:\\Temp\\\" -Recurse -Force");
                scriptContents.AppendLine("");
                scriptContents.AppendLine("start-process -filepath \"C:\\windows\\system32\\Dism.exe\" -argumentlist '/online /enable-feature /featurename:NetFX3 /All /source:\"C:\\Temp\\sxs\" /LimitAccess' -wait ");
                scriptContents.AppendLine("");
                scriptContents.AppendLine("# changing the computer name");
                scriptContents.AppendLine("");
                scriptContents.AppendLine("rename-computer -newname $ComputerName -restart");
            #endif

            var pwshSession = new PSCore();
            pwshSession.InitializeRunspaces(2, 10, modulesToLoad);

            await pwshSession.RunScript(scriptContents.ToString(), scriptParameters);

            return "Done";
        }
    }
}