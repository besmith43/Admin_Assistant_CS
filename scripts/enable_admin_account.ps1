param(
    [string]$LocalAdminPass,
    [string]$ComputerName
)

set-executionpolicy unrestricted -confirm -force

$password = ConvertTo-SecureString $LocalAdminPass -AsPlainText -Force
$admin = get-localuser -name "Administrator"
$admin | enable-localuser
$admin | set-localuser -password $password -accountneverexpires -passwordneverexpires $True

# set Time zone to central

set-timezone "Central Standard Time"

# set time zone automatically

set-itemproperty -path "HKLM:\SYSTEM\CurrentControlSet\Services\tzautoupdate" -name "Start" -Value 3 -type DWORD

# install dotnet framework 3.5

if (!$(test-path C:\Temp))
{
	new-item -itemtype Directory -path C:\Temp
	$(get-item -path C:\Temp -Force).attributes = "Hidden"
}

copy-item -Path "$PSScriptRoot\sxs\ -Destination "C:\Temp\ -Recurse -Force

start-process -filepath "C:\windows\system32\Dism.exe" -argumentlist '/online /enable-feature /featurename:NetFX3 /All /source:"C:\Temp\sxs" /LimitAccess' -wait

# changing the computer name

rename-computer -newname $ComputerName -restart
