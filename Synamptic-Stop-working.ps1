
clear
# Fix issue with synamptic stop working after sleeping
$syns = Get-Process syn*

if($syns.Count -eq 0)
{
    exit
    #Exit-PSHostProcess
}

foreach ($name in $syns) {
    $Host.UI.WriteVerboseLine("Stoping: $name")
    Stop-Process($name) -Force
}

$synpPath = 'C:\Program Files\Synaptics\SynTP\SynTPEnh.exe'
#$Host.UI.WriteLine('Restarting process')

if(Test-Path -LiteralPath $synpPath)
{
    Start-Process $synpPath -Verb runAs # start process and run as admin
}

try
{
    Restart-Service SynTPEnhService
}
catch # Todo: write out catched expection
{
    #$host.UI.WriteLine('something bad happenned')
}

<#
foreach($name in $syns)
{
    $Host.UI.WriteVerboseLine("Starting: $name")
    Start-Process($name)
}#>

# (Get-FileHash .\at.exe -Algorithm SHA1)