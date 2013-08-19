"C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\Bin\sn.exe" -q -R "..\Artifacts\Application\Interapptive.Shared.dll" "D:\Development\Deployment\Signing\interapptive_private.snk"
"C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\Bin\sn.exe" -q -R "..\Artifacts\Application\ShipWorks.Data.Adapter.dll" "D:\Development\Deployment\Signing\interapptive_private.snk"
"C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\Bin\sn.exe" -q -R "..\Artifacts\Application\ShipWorks.Data.Model.dll" "D:\Development\Deployment\Signing\interapptive_private.snk"
"C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\Bin\sn.exe" -q -R "..\Artifacts\Application\ShipWorks.Shared.dll" "D:\Development\Deployment\Signing\interapptive_private.snk"
"C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\Bin\sn.exe" -q -R "..\Artifacts\Application\ShipWorks.SqlServer.dll" "D:\Development\Deployment\Signing\interapptive_private.snk"
"C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\Bin\sn.exe" -q -R "..\Artifacts\Application\ShipWorks.exe" "D:\Development\Deployment\Signing\interapptive_private.snk"

"C:\Program Files (x86)\Inno Setup 5\ISCC.EXE" ..\Installer\ShipWorks.iss /O"..\Artifacts\Distribute" /F"ShipWorksSetup_Test" /DIncludeSymbols=True /DEditionType=Standard /DVersion=0.0.0.0 /DAppArtifacts="..\Artifacts\Application" /DRequiredSchemaID=12345
