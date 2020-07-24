param(
    [string]$LocalAdminPass,
    [string]$ComputerName
)

if ($LocalAdminPass -eq "testing" -and $ComputerName -eq "testing")
{
    Write-Host "both fields are testing"
    $code = 0
    exit $code
}
else
{
    Write-Host "bad input"
    $code = 1
    exit $code
}