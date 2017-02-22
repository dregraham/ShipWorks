# Delete any database that the integration tests might have created for this branch

$databasePrefix = $env:FEATURE_NAME.Split('-') | Select-Object -Last 1

[Reflection.Assembly]::LoadWithPartialName("Microsoft.SqlServer.Smo")
$databaseServer = New-Object 'Microsoft.SqlServer.Management.SMO.Server' "(localdb)\v11.0"
$databasesToDrop = $databaseServer.Databases | Where-Object {$_.name -like "SW_" + $databasePrefix + "*" }
foreach ($database in $databasesToDrop) { $database.Drop() }
