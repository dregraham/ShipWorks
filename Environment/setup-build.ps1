iex ((New-Object System.Net.WebClient).DownloadString('https://chocolatey.org/install.ps1'))

choco feature enable -n allowGlobalConfirmation

choco install ruby --version 2.4.2.2 --forcex86
choco install ruby2.devkit --version 4.7.2.2013022402 --forcex86
choco install dotnet3.5
choco install visualstudio2017buildtools `
    --package-parameters "--add Microsoft.VisualStudio.Workload.NetCoreBuildTools --includeRecommended --noUpdateInstaller" `
    --version 15.2.26430.20170605
choco install msaccess2010-redist-x86 --version 1.2
choco install sqlserverlocaldb --version 11.0.2318.0 --allow-empty-checksums
choco install jdk8 --version 8.0.162
choco install firefox
choco install notepadplusplus

& "${Env:ProgramFiles(x86)}\Microsoft Visual Studio\Installer\vs_installer.exe" modify `
    --installPath "${Env:ProgramFiles(x86)}\Microsoft Visual Studio\2017\BuildTools" `
    --add Microsoft.VisualStudio.Workload.NetCoreBuildTools `
    --add Microsoft.VisualStudio.Workload.MSBuildTools `
    --add Microsoft.VisualStudio.Workload.WebBuildTools `
    --add Microsoft.Net.Component.3.5.DeveloperTools `
    --includeRecommended `
    --passive `
    --noUpdateInstaller `
    --wait

Write-Host "Press any key to continue..."
[void][System.Console]::ReadKey($true)