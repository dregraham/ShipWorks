$applicationPath = ".\Artifacts\Application"
if (!(Test-Path $applicationPath))
{
    mkdir $applicationPath
}

start-process -FilePath powershell `
    -verb runas `
    -ArgumentList "& $(Resolve-Path .\Environment\setup-build.ps1)" `
    -Wait

start-process -FilePath powershell `
    -verb runas `
    -ArgumentList "& $(Resolve-Path .\Environment\setup-registry.ps1) -applicationPath $(Resolve-Path $applicationPath)" `
    -Wait

$env:Path = [System.Environment]::GetEnvironmentVariable("Path","Machine") + ";" + [System.Environment]::GetEnvironmentVariable("Path","User")

& $(Resolve-Path .\Environment\setup-ruby.ps1)

bundle exec rake build:debug test:units test:integration[ContinuousIntegration]