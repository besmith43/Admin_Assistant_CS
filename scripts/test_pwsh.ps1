param(
    [string]$LocalAdminPass,
    [string]$ComputerName
)

if ($LocalAdminPass -eq "testing" -and $ComputerName -eq "testing")
{
    Write-Host "both fields are testing"
}
else
{
    Write-Host "bad input"
}