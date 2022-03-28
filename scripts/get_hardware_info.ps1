$user = [Security.Principal.WindowsIdentity]::GetCurrent();

if (!(New-Object Security.Principal.WindowsPrincipal $user).IsInRole([Security.Principal.WindowsBuiltinRole]::Administrator))
{
	$creds = Get-Credential

	$hostname = hostname

	$cpu = get-wmiobject -class win32_processor -Credential $creds | select-object name, currentclockspeed, currentvoltage, maxclockspeed, numberofcores, numberoflogicalprocessors
	
	$hdds = get-wmiobject -class win32_logicaldisk -Credential $creds | select deviceid, volumename, size, freespace | out-string
	
	$encryption = manage-bde -status -Credential $creds | out-string
	
	$ram = get-wmiobject -class win32_physicalmemory -Credential $creds | select devicelocator, capacity, speed | out-string
	
	$bios = get-wmiobject -class win32_bios -Credential $creds | select name, serialnumber | out-string
	
	$os = get-wmiobject -class win32_operatingsystem -Credential $creds | select name, version | out-string
	
	$gpus = get-wmiobject -class win32_videocontroller -Credential $creds | select name, videomodedescription | out-string
	
	$monitors = get-wmiobject -class Win32_DesktopMonitor -Credential $creds | select name | out-string
	
	$model = get-wmiobject -class Win32_ComputerSystemProduct -Credential $creds | select name | out-string
	
	$installed_programs = get-wmiobject -class Win32_InstalledWin32Program -Credential $creds | select name, vendor, version
}
else
{
	$hostname = hostname

	$cpu = get-wmiobject -class win32_processor | select-object name, currentclockspeed, currentvoltage, maxclockspeed, numberofcores, numberoflogicalprocessors
	
	$hdds = get-wmiobject -class win32_logicaldisk | select deviceid, volumename, size, freespace | out-string
	
	$encryption = manage-bde -status | out-string
	
	$ram = get-wmiobject -class win32_physicalmemory | select devicelocator, capacity, speed | out-string
	
	$bios = get-wmiobject -class win32_bios | select name, serialnumber | out-string
	
	$os = get-wmiobject -class win32_operatingsystem | select name, version | out-string
	
	$gpus = get-wmiobject -class win32_videocontroller | select name, videomodedescription | out-string
	
	$monitors = get-wmiobject -class Win32_DesktopMonitor | select name | out-string
	
	$model = get-wmiobject -class Win32_ComputerSystemProduct | select name | out-string
	
	$installed_programs = get-wmiobject -class Win32_InstalledWin32Program | select name, vendor, version		
}

$Word = New-Object -ComObject Word.Application

$Document = $Word.Documents.Add()
$Selection = $Word.Selection

$Selection.TypeText("Computer Name: $hostname")
$Selection.TypeParagraph()
$Selection.TypeParagraph()
$Selection.TypeText("Model of CPU")
$Selection.TypeText("$cpu")
$Selection.TypeParagraph()
$Selection.TypeText("RAM") 
$Selection.TypeParagraph()
$Selection.TypeText("$ram")
$Selection.TypeParagraph()
$Selection.TypeText("Hard Drives")
$Selection.TypeParagraph()
$Selection.TypeText("$hdds")
$Selection.TypeParagraph()
$Selection.TypeText("Bitlocker Status")
$Selection.TypeParagraph()
$Selection.TypeText("$encryption")
$Selection.TypeParagraph()
$Selection.TypeText("Bios Info")
$Selection.TypeParagraph()
$Selection.TypeText("$bios")
$Selection.TypeParagraph()
$Selection.TypeText("GPU Model")
$Selection.TypeParagraph()
$Selection.TypeText("$gpus")
$Selection.TypeParagraph()
$Selection.TypeText("Operating System")
$Selection.TypeParagraph()
$Selection.TypeText("$os")
$Selection.TypeParagraph()
$Selection.TypeText("Monitors")
$Selection.TypeParagraph()
$Selection.TypeText("$monitors")
$Selection.TypeParagraph()
$Selection.TypeText("Model of Computer")
$Selection.TypeParagraph()
$Selection.TypeText("$model")
$Selection.TypeParagraph()
$Selection.TypeText("Installed Programs")
foreach ($program in $installed_programs)
{
	$Selection.TypeParagraph()
	$Selection.TypeText("$program")
}

$date = get-date -UFormat "%m-%d-%Y"

$Report = "C:\Users\$env:UserName\Desktop\$hostname-$date.doc"

$Document.SaveAs([ref]$Report,[ref]$SaveFormat::wdFormatDocument)
$word.Quit()

$null = [System.Runtime.InteropServices.Marshal]::ReleaseComObject([System.__ComObject]$word)
[gc]::Collect()
[gc]::WaitForPendingFinalizers()
Remove-Variable word
