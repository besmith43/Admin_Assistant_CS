param (
    [string]$user
)

if ($(test-path -path C:\Temp\USMT))
{
    if ($(test-path -path C:\Users\$user))
    {
        $backup_job = start-job -ScriptBlock {set-location -path "C:\Temp\USMT"; start-process -FilePath "C:\Temp\USMT\scanstate" -ArgumentList "C:\Temp\USMT /ue:*\* /ui:ttu\$user /i:migapp.xml /i:miguser.xml /config:config.xml /localonly /o /efs:copyraw /c /v:13" -wait}

        $backup_job | Wait-Job

        if ($backup_job.State -match "Completed")
        {
            $exit_code = 0

            exit $exit_code
        }
        else
        {
            $exit_code = 1

            exit $exit_code
        }
    }
    else
    {
        $exit_code = 3
        
        exit $exit_code
    }
}
else
{
    $exit_code = 2

    exit $exit_code
}