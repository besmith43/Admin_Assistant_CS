param (
    [switch]$DifferentUser
)

$DriveAddress = '\\eagle\users'
$DriveLetter = 'N:'

if ($DifferentUser)
{
    $creds = Get-Credential
}

# see link for where this method came from
# https://stackoverflow.com/questions/1530733/powershell-how-to-map-a-network-drive-with-a-different-username-password
$net = $(New-Object -ComObject WScript.Network)

if ($DifferentUser)
{
    #New-SmbMapping -LocalPath $DriveLetter -RemotePath $DriveAddress -Username $creds.Username -Password $creds.GetNetworkCredential().Password -Persistent $true
    $net.MapNetworkDrive($DriveLetter,$DriveAddress, $false, $creds.UserName, $creds.GetNetworkCredential().Password)
    explorer.exe $DriveLetter
}
else
{
    #New-SmbMapping -LocalPath $DriveLetter -RemotePath $DriveAddress -Persistent $true
    $net.MapNetworkDrive($DriveLetter,$DriveAddress)
    explorer.exe $DriveLetter
}