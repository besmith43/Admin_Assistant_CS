params (
    [System.Management.Automation.PSCredential]$Credential = $(Get-Credential)
)

add-computer -DomainName tntech.edu -Credential $Credential -restart -force
