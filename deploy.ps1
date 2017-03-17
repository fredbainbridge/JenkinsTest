#connect to site
[CmdletBinding()]
param(
    [string] $ENV = "PROD",
    [string] $originBranch = "origin",
    [string] $masterBranch = "master"
)

$hashEnvironments = @{};

$hashEnvironments.add("PROD", "\\CM01\Source\JenkinsTest")
$hashEnvironments.add("PROD_SITECODE", "LAB")
$hashEnvironments.add("PROD_SITESERVER", "cm01.cm.lab")
$hashEnvironments.add("PROD_PACKAGEID", "LAB00055")


$destination = $hashEnvironments[$ENV]
$siteServer = $hashEnvironments["$($ENV)_SITESERVER"]
$siteCode = $hashEnvironments["$($ENV)_SITECODE"]
$toolkitPackageID = $hashEnvironments["$($ENV)_PACKAGEID"]
#$destination = "c:\temp"

#get latest version from git
#git pull $originBranch $masterBranch

#copy unchanged files to destination
Get-ChildItem -Path . -File | ForEach-Object{
    Copy-Item $PSItem $destination  -Force -Verbose
} 

Import-Module 'C:\Program Files (x86)\Microsoft Configuration Manager\AdminConsole\bin\ConfigurationManager.psd1' -force
if ((Get-PSDrive $siteCode -erroraction SilentlyContinue | Measure-Object).Count -ne 1) {
    New-PSDrive -Name $siteCode -PSProvider "AdminUI.PS.Provider\CMSite" -Root $siteServer
}
Set-Location $siteCode`:

Update-CMDistributionPoint -PackageId $toolkitPackageID -Verbose

Set-Location $env:SystemDrive