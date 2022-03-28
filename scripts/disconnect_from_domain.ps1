param(
    [System.Management.Automation.PSCredential]$Credential = $(Get-Credential)
)

remove-computer -unjoindomaincredential $Credential -restart -force -WorkGroupName "Home" -Confirm