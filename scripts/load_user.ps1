if ($(test-path -path C:\Temp\USMT))
{
    $load_job = start-job -ScriptBlock {set-location -path "C:\Temp\USMT"; start-process -FilePath "C:\Temp\USMT\loadstate" -argumentlist "C:\Temp\USMT\ /i:migapp.xml /i:miguser.xml /config:config.xml /c /v:13" -wait}

    $load_job | Wait-Job

    if ($load_job.State -match "Completed")
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
    $exit_code = 2

    exit $exit_code
}