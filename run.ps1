# Usage ./run.ps1 -testType "Regression"
param(
  [string]$testType
)
try
{
	[string]$PROJECTNAME = 'Hctra.Automation'
	[string]$TESTSELECT = $testType
		
	$DateTime = get-date -format ddd_dd.MM.yyyy_HH.mm.ss		
	Set-Location $PSScriptRoot
	
	# File Names
	$PROJECT="$PSScriptRoot\$PROJECTNAME\$PROJECTNAME.csproj"
	
	# Execute Tests
	Write-host "***************************************" -ForegroundColor DarkGray
	$TextInfo = (Get-Culture).TextInfo
	Write-host "Hi!", $TextInfo.ToTitleCase($env:UserName) -ForegroundColor Green
	Write-host "***************************************" -ForegroundColor DarkGray
    dotnet test $PROJECT  --filter TestCategory=$TESTSELECT
}
Catch
{
    write-host $_.Exception.Message;
}