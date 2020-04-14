param ($Port)

# Check for existing cert for ShipWorksAPI
$SwApiCert = Get-ChildItem -path Cert:\LocalMachine\Root -Recurse | Where-Object {$_.Subject -like "*ShipWorksAPI*"}

# If no cert exists, create one
if($SwApiCert -eq $nulL)
{
	echo 'No certificate found for ShipWorksAPI, creating a new one'
	$SwApiCert = New-SelfSignedCertificate -FriendlyName ShipWorksAPI -NotAfter (Get-Date -Year 2038 -Month 1 -Day 19) -Subject ShipWorksAPI -CertStoreLocation Cert:\LocalMachine\My

	Move-Item -Path $SwApiCert.PSPath -Destination Cert:\LocalMachine\Root
}
$CertHash = $SwApiCert.thumbprint

# Register the port with netsh
echo "Registering port $Port"
netsh http add urlacl url="https://+:$Port/" user=Everyone
$RegisterPortResult = $?
if(-not $RegisterPortResult)
{
	echo "Failed to register port $Port"
}

# If port registered successfully, add the sslcert to it
if($RegisterPortResult)
{
	echo "adding sslcert to port $Port"
	netsh http add sslcert ipport="0.0.0.0:$Port" certhash=$CertHash appid='{214124cd-d05b-4309-9af9-9caa44b2b74a}' certstorename=Root
	if(-not $?)
	{
		echo "Failed to add sslcert to port $Port"
	}
}

exit $LASTEXITCODE
