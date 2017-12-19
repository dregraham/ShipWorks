Param(
    [string]$applicationPath = $(Resolve-Path "../Artifacts/Application")
)

function Ensure-RegistryKey($key)
{
    if (!(Test-Path $key))
    {
        New-Item -Path $key -Force | Out-Null
    }
}

function Ensure-RegistryValue($keyName, $name, $value, $type)
{
    $key = Get-Item -Path $keyName;
    if ($key.GetValue($name) -eq $null) {
        New-ItemProperty -Path $keyName -Name $name -Value $value -PropertyType $type
    }
}

function New-Guid
{
    ("{" + [guid]::NewGuid().ToString() + "}")
}

# Skip verification of 32-bit ShipWorks signing
Ensure-RegistryKey "HKLM:\SOFTWARE\Microsoft\StrongName\Verification\*,6a464ecbd35cd6df";

# Skip verification of 64-bit ShipWorks signing
Ensure-RegistryKey "HKLM:\SOFTWARE\Wow6432Node\Microsoft\StrongName\Verification\*,6a464ecbd35cd6df";

# Ensure that the ShipWorks key is in the registry
Ensure-RegistryKey "HKLM:\SOFTWARE\Interapptive\ShipWorks\Instances";

# Ensure there is a computer ID
Ensure-RegistryValue "HKLM:\SOFTWARE\Interapptive\ShipWorks" "ComputerID" $(New-Guid) String

Ensure-RegistryValue "HKLM:\SOFTWARE\Interapptive\ShipWorks\Instances" $applicationPath $(New-Guid) String

Write-Host "Press any key to continue..."
[void][System.Console]::ReadKey($true)