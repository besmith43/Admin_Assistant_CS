param (
[string]$TempUserName
)

$temp = get-localuser $TempUserName

if ($temp)
{
	remove-localuser $temp
}
