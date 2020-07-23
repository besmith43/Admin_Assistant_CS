if (!$(test-path C:\Temp))
{
    new-item -itemtype Directory -path C:\Temp
    $(get-item -path C:\Temp -Force).attributes = "Hidden"
}

if (test-path -path "C:\Temp\USMT\")
{ 
    remove-item -path "C:\Temp\USMT\" -Recurse -force
}

copy-item -Path "$PSScriptRoot\USMT\" -Destination "C:\Temp\" -Recurse -Force
        
$scan_job = start-job -scriptblock {set-location -path "C:\Temp\USMT\"; start-process -FilePath "C:\Temp\USMT\scanstate" -ArgumentList "/i:migapp.xml /i:miguser.xml /genconfig:config.xml /c" -wait}

$scan_job | Wait-Job

if ($scan_job.state -match "Completed")
{
    $exit_code = 0

    exit $exit_code
}
else
{
    $exit_code = 1

    exit $exit_code
}