; For testing these may be altered
#define FORCE_DOTNET_REQUIRED FALSE
#define FORCE_NEED_ADMIN	  FALSE
#define FORCE_NEED_IE		  FALSE

#ifndef EditionType
	#define EditionType 'Standard'
#endif

#if (EditionType == 'Standard')
	#define EditionName ''
	#define EditionAppConfig 'ShipWorks.exe.config'
#endif

#if (EditionType == 'Endicia')
	#define EditionName ' (Free for eBay)'
	#define EditionAppConfig 'App.Endicia.config'
#endif

#if (EditionType == 'UPS')
	#define EditionName ' (UPS Only)'
	#define EditionAppConfig 'App.Ups.config'
#endif

[Setup]
AppName=ShipWorks®
AppVersion={#= Version} {#= EditionName}
AppVerName=ShipWorks® {#= Version} {#= EditionName}
AppPublisher=Interapptive®, Inc.
AppPublisherURL=http://www.shipworks.com
AppSupportURL=http://www.shipworks.com
AppUpdatesURL=http://www.shipworks.com
AppMutex={{AX70DA71-2A39-4f8c-8F97-7F5348493F57}
DefaultDirName={pf}\ShipWorks
DefaultGroupName=ShipWorks
LicenseFile=License.rtf
MinVersion=4.0.950,4.0.1381
PrivilegesRequired=none
DisableStartupPrompt=true
AllowRootDirectory=false
UserInfoPage=false
ShowComponentSizes=false
WizardImageFile=WizardLarge.bmp
WizardImageBackColor=clWhite
UninstallDisplayIcon={app}\ShipWorks.exe
AppID={code:GetAppID}
UninstallFilesDir={app}\Uninstall
WizardSmallImageFile=WizardSmall.bmp
DirExistsWarning=no
RestartIfNeededByRun=true
UsePreviousAppDir=true
UsePreviousGroup=true
AlwaysRestart=false
ShowLanguageDialog=no
AllowUNCPath=false
VersionInfoVersion={#= Version}
VersionInfoCompany=Interapptive®, Inc.
VersionInfoDescription=Interapptive® ShipWorks®
VersionInfoTextVersion=ShipWorks® {#= Version}
VersionInfoCopyright=Copyright © Interapptive®, Inc. 2003-2013
ArchitecturesInstallIn64BitMode=x64
AppendDefaultDirName=false

[InstallDelete]
Type: files; Name: {app}\ShipWorks.chm
Type: files; Name: {app}\Interapptive.Common.dll
Type: files; Name: {app}\Interapptive.UI.dll
Type: files; Name: {app}\AxInterop.SHDocVw.dll
Type: files; Name: {app}\Interop.SHDocVw.dll
Type: files; Name: {app}\Interop.Smui.dll
Type: files; Name: {app}\Microsoft.mshtml.dll
Type: files; Name: {app}\NetToolWorks.dll
Type: files; Name: {app}\NetToolWorks.Mail.dll
Type: files; Name: {app}\MailBee.NET.dll
Type: files; Name: {app}\SandBar.dll
Type: files; Name: {app}\Skybound.VisualStyles.dll
Type: files; Name: {app}\Skybound.VisualTips.dll
Type: files; Name: {app}\ActiproSoftware.Shared.dll
Type: files; Name: {app}\ActiproSoftware.SyntaxEditor.dll
Type: files; Name: {app}\ActiproSoftware.WinUICore.dll
Type: files; Name: {app}\Microsoft.Web.Services2.dll
Type: files; Name: {app}\eBay.SDK.dll

[Files]
Source: isxdl.dll; DestDir: {tmp}; Flags: dontcopy
Source: License.rtf; DestDir: {app}; Flags: overwritereadonly ignoreversion
Source: {#AppArtifacts}\ShipWorks.exe; DestDir: {app}; Flags: overwritereadonly ignoreversion
Source: {#AppArtifacts}\{#= EditionAppConfig}; DestDir: {app}; DestName: "ShipWorks.exe.config"; Flags: overwritereadonly ignoreversion
Source: {#AppArtifacts}\ActiproSoftware.Shared.Net20.dll; DestDir: {app}; Flags: overwritereadonly ignoreversion
Source: {#AppArtifacts}\ActiproSoftware.SyntaxEditor.Addons.Web.Net20.dll; DestDir: {app}; Flags: overwritereadonly ignoreversion
Source: {#AppArtifacts}\ActiproSoftware.SyntaxEditor.Net20.dll; DestDir: {app}; Flags: overwritereadonly ignoreversion
Source: {#AppArtifacts}\ActiproSoftware.WinUICore.Net20.dll; DestDir: {app}; Flags: overwritereadonly ignoreversion
Source: {#AppArtifacts}\Apitron.PDF.Rasterizer.dll; DestDir: {app}; Flags: overwritereadonly ignoreversion
Source: {#AppArtifacts}\Autofac.dll; DestDir: {app}; Flags: overwritereadonly ignoreversion
Source: {#AppArtifacts}\Autofac.Extras.Attributed.dll; DestDir: {app}; Flags: overwritereadonly ignoreversion
Source: {#AppArtifacts}\Autofac.Integration.Mef.dll; DestDir: {app}; Flags: overwritereadonly ignoreversion
Source: {#AppArtifacts}\Common.Logging.Core.dll; DestDir: {app}; Flags: overwritereadonly ignoreversion
Source: {#AppArtifacts}\Common.Logging.dll; DestDir: {app}; Flags: overwritereadonly ignoreversion
Source: {#AppArtifacts}\Common.Logging.Log4Net1213.dll; DestDir: {app}; Flags: overwritereadonly ignoreversion
Source: {#AppArtifacts}\ComponentFactory.Krypton.Toolkit.dll; DestDir: {app}; Flags: overwritereadonly ignoreversion
Source: {#AppArtifacts}\Divelements.SandGrid.dll; DestDir: {app}; Flags: overwritereadonly ignoreversion
Source: {#AppArtifacts}\Divelements.SandRibbon.dll; DestDir: {app}; Flags: overwritereadonly ignoreversion
Source: {#AppArtifacts}\Interapptive.Shared.dll; DestDir: {app}; Flags: overwritereadonly ignoreversion
Source: {#AppArtifacts}\log4net.dll; DestDir: {app}; Flags: overwritereadonly ignoreversion
Source: {#AppArtifacts}\Microsoft.Threading.Tasks.dll; DestDir: {app}; Flags: overwritereadonly ignoreversion
Source: {#AppArtifacts}\Microsoft.Threading.Tasks.Extensions.Desktop.dll; DestDir: {app}; Flags: overwritereadonly ignoreversion
Source: {#AppArtifacts}\Microsoft.Threading.Tasks.Extensions.dll; DestDir: {app}; Flags: overwritereadonly ignoreversion
Source: {#AppArtifacts}\Microsoft.Web.Services3.dll; DestDir: {app}; Flags: overwritereadonly ignoreversion
Source: {#AppArtifacts}\NAudio.dll; DestDir: {app}; Flags: overwritereadonly ignoreversion
Source: {#AppArtifacts}\Newtonsoft.Json.dll; DestDir: {app}; Flags: overwritereadonly ignoreversion
Source: {#AppArtifacts}\Quartz.dll; DestDir: {app}; Flags: overwritereadonly ignoreversion
Source: {#AppArtifacts}\Rebex.Common.dll; DestDir: {app}; Flags: overwritereadonly ignoreversion
Source: {#AppArtifacts}\Rebex.Ftp.dll; DestDir: {app}; Flags: overwritereadonly ignoreversion
Source: {#AppArtifacts}\Rebex.Mail.dll; DestDir: {app}; Flags: overwritereadonly ignoreversion
Source: {#AppArtifacts}\Rebex.Net.Imap.dll; DestDir: {app}; Flags: overwritereadonly ignoreversion
Source: {#AppArtifacts}\Rebex.Net.Pop3.dll; DestDir: {app}; Flags: overwritereadonly ignoreversion
Source: {#AppArtifacts}\Rebex.Net.ProxySocket.dll; DestDir: {app}; Flags: overwritereadonly ignoreversion
Source: {#AppArtifacts}\Rebex.Net.SecureSocket.dll; DestDir: {app}; Flags: overwritereadonly ignoreversion
Source: {#AppArtifacts}\Rebex.Net.Smtp.dll; DestDir: {app}; Flags: overwritereadonly ignoreversion
Source: {#AppArtifacts}\Rebex.Networking.dll; DestDir: {app}; Flags: overwritereadonly ignoreversion
Source: {#AppArtifacts}\Rebex.Security.dll; DestDir: {app}; Flags: overwritereadonly ignoreversion
Source: {#AppArtifacts}\Rebex.Sftp.dll; DestDir: {app}; Flags: overwritereadonly ignoreversion
Source: {#AppArtifacts}\RestSharp.dll; DestDir: {app}; Flags: overwritereadonly ignoreversion
Source: {#AppArtifacts}\SandDock.dll; DestDir: {app}; Flags: overwritereadonly ignoreversion
Source: {#AppArtifacts}\SD.LLBLGen.Pro.DQE.SqlServer.NET20.dll; DestDir: {app}; Flags: overwritereadonly ignoreversion
Source: {#AppArtifacts}\SD.LLBLGen.Pro.LinqSupportClasses.NET35.dll; DestDir: {app}; Flags: overwritereadonly ignoreversion
Source: {#AppArtifacts}\SD.LLBLGen.Pro.ORMSupportClasses.NET20.dll; DestDir: {app}; Flags: overwritereadonly ignoreversion
Source: {#AppArtifacts}\ShipWorks.Core.dll; DestDir: {app}; Flags: overwritereadonly ignoreversion
Source: {#AppArtifacts}\ShipWorks.Data.Adapter.dll; DestDir: {app}; Flags: overwritereadonly ignoreversion
Source: {#AppArtifacts}\ShipWorks.Data.Model.dll; DestDir: {app}; Flags: overwritereadonly ignoreversion
Source: {#AppArtifacts}\ShipWorks.Shared.dll; DestDir: {app}; Flags: overwritereadonly ignoreversion
Source: {#AppArtifacts}\ShipWorks.Shipping.dll; DestDir: {app}; Flags: overwritereadonly ignoreversion
Source: {#AppArtifacts}\ShipWorks.Shipping.UI.dll; DestDir: {app}; Flags: overwritereadonly ignoreversion
Source: {#AppArtifacts}\ShipWorks.SqlServer.dll; DestDir: {app}; Flags: overwritereadonly ignoreversion
Source: {#AppArtifacts}\ShipWorks.Stores.dll; DestDir: {app}; Flags: overwritereadonly ignoreversion
Source: {#AppArtifacts}\ShipWorks.Stores.UI.dll; DestDir: {app}; Flags: overwritereadonly ignoreversion
Source: {#AppArtifacts}\ShipWorks.ThirdPartyApi.dll; DestDir: {app}; Flags: overwritereadonly ignoreversion
Source: {#AppArtifacts}\ShipWorks.UI.dll; DestDir: {app}; Flags: overwritereadonly ignoreversion
Source: {#AppArtifacts}\SpreadsheetGear.dll; DestDir: {app}; Flags: overwritereadonly ignoreversion
Source: {#AppArtifacts}\System.IO.dll; DestDir: {app}; Flags: overwritereadonly ignoreversion
Source: {#AppArtifacts}\System.Reactive.Core.dll; DestDir: {app}; Flags: overwritereadonly ignoreversion
Source: {#AppArtifacts}\System.Reactive.Interfaces.dll; DestDir: {app}; Flags: overwritereadonly ignoreversion
Source: {#AppArtifacts}\System.Reactive.Linq.dll; DestDir: {app}; Flags: overwritereadonly ignoreversion
Source: {#AppArtifacts}\System.Reactive.PlatformServices.dll; DestDir: {app}; Flags: overwritereadonly ignoreversion
Source: {#AppArtifacts}\System.Reactive.Windows.Threading.dll; DestDir: {app}; Flags: overwritereadonly ignoreversion
Source: {#AppArtifacts}\System.Runtime.dll; DestDir: {app}; Flags: overwritereadonly ignoreversion
Source: {#AppArtifacts}\System.Threading.Tasks.dll; DestDir: {app}; Flags: overwritereadonly ignoreversion
Source: {#AppArtifacts}\System.Windows.Interactivity.dll; DestDir: {app}; Flags: overwritereadonly ignoreversion
Source: {#AppArtifacts}\x64\ShipWorks.Native.dll; DestDir: {app}; Flags: overwritereadonly ignoreversion; Check: Is64BitInstallMode
Source: {#AppArtifacts}\Win32\ShipWorks.Native.dll; DestDir: {app}; Flags: overwritereadonly ignoreversion; Check: not Is64BitInstallMode

#ifdef IncludeSymbols
    Source: {#AppArtifacts}\ShipWorks.pdb; DestDir: {app}; Flags: overwritereadonly ignoreversion
    Source: {#AppArtifacts}\ShipWorks.Shared.pdb; DestDir: {app}; Flags: overwritereadonly ignoreversion
    Source: {#AppArtifacts}\ShipWorks.Data.Model.pdb; DestDir: {app}; Flags: overwritereadonly ignoreversion
    Source: {#AppArtifacts}\ShipWorks.Data.Adapter.pdb; DestDir: {app}; Flags: overwritereadonly ignoreversion
    Source: {#AppArtifacts}\ShipWorks.SqlServer.pdb; DestDir: {app}; Flags: overwritereadonly ignoreversion
    Source: {#AppArtifacts}\Interapptive.Shared.pdb; DestDir: {app}; Flags: overwritereadonly ignoreversion
#endif


[Tasks]
Name: desktopicon; Description: Create a &Desktop icon; GroupDescription: Additional shortcuts:

[Icons]
Name: {group}\ShipWorks; Filename: {app}\ShipWorks.exe; IconIndex: 0
Name: {userdesktop}\ShipWorks; Filename: {app}\ShipWorks.exe; Tasks: desktopicon

[_ISTool]
EnableISX=true

[Registry]
Root: HKLM; Subkey: Software\Interapptive\ShipWorks; ValueType: string; ValueName: ComputerID; ValueData: {code:GetGuid}; Flags: createvalueifdoesntexist
Root: HKLM; Subkey: Software\Interapptive\ShipWorks\Instances; ValueType: string; ValueName: {app}; ValueData: {code:GetAppID}; Flags: createvalueifdoesntexist
Root: HKLM; Subkey: Software\Interapptive\ShipWorks; ValueType: string; ValueName: LastInstalledInstanceID; ValueData: {code:GetAppID};
Root: HKLM; Subkey: Software\Microsoft\Windows\CurrentVersion\Run; ValueType: string; ValueName: {code:GetBackgroundProcessName}; ValueData: {app}\ShipWorks.exe /s=Scheduler;

[Run]
Filename: {app}\ShipWorks.exe; Description: Launch ShipWorks; Flags: nowait postinstall skipifsilent
Filename: {app}\ShipWorks.exe; Parameters: "/s=scheduler"; Flags: nowait; Check: not NeedRestart

[UninstallRun]
Filename: {app}\ShipWorks.exe; Parameters: "/command:uninstall";

[Dirs]
Name: {app}
Name: {commonappdata}\Interapptive; Permissions: everyone-modify; Check: not CommonAppDataExists;

[Code]
//----------------------------------------------------------------
// Includes
//----------------------------------------------------------------
#include "DotNetDownloadPage.iss"
#include "DotNetInstallPage.iss";
#include "KB2468871DownloadPage.iss";
#include "KB2468871InstallPage.iss";
#include "SystemChecks.iss"
#include "Guid.iss"

var
   newAppID: string;

//----------------------------------------------------------------
// Get the AppID - which is the unique string that identifies the uninstall key
//----------------------------------------------------------------
function GetAppID(Param: String): String;
var
  appPath: string;
  instanceID: string;
begin
	if (Length(newAppID) = 0)
	then begin
		newAppID := GetGuid('');
	end;

	try
	    appPath := ExpandConstant('{app}');

		if RegQueryStringValue(HKEY_LOCAL_MACHINE, 'Software\Interapptive\ShipWorks\Instances' , appPath, instanceID)
		then begin
			Result := instanceID;
		end
		else
		begin
			Result := newAppID;
		end;

    except

		if RegQueryStringValue(HKEY_LOCAL_MACHINE, 'Software\Interapptive\ShipWorks', 'LastInstalledInstanceID', instanceID)
		then begin
			Result := instanceID;
		end
		else
		begin
			Result := newAppID;
		end;
    end;
end;

//----------------------------------------------------------------
// Returns the background/service name (ShipWorksScheduler$GuidWithNonAlphaCharactersStripped)
//----------------------------------------------------------------
function GetBackgroundProcessName(Param: String): String;
var
    processName:string;
begin
    processName := GetAppID('');

    delete(processName, 38, 1);
    delete(processName, 25, 1);
    delete(processName, 20, 1);
    delete(processName, 15, 1);
    delete(processName, 10, 1);
    delete(processName, 1, 1);

    Result := 'ShipWorksScheduler$' + processName;
end;


//----------------------------------------------------------------
// Was the scheduler included in installed version of ShipWorks
//----------------------------------------------------------------
function ShipWorksVersionHasScheduler(): Boolean;
var
	TargetExe: string;
	VersionMS: Cardinal;
	VersionLS: Cardinal;
	VersionMajor: Integer;
	VersionMinor: Integer;
begin

	TargetExe := ExpandConstant('{app}') + '\ShipWorks.exe';
	if (FileExists(TargetExe))
    then begin

	    if (GetVersionNumbers(TargetExe, VersionMS, VersionLS))
	    then begin
	        VersionMajor := (VersionMS shr 16) and $ffff;
	        VersionMinor := VersionMS and $ffff;

           // MsgBox(IntToStr(VersionMajor) + ' ' + IntToStr(VersionMinor),
		   //				    mbConfirmation,
		   //				    MB_OKCANCEL)

            if (
                (VersionMajor > 3) or
                ((VersionMajor = 3) and (VersionMinor > 4)) or

                // Special case for internal builds
                ((VersionMajor = 0) and (VersionMinor = 0))
               )

            then begin
                Result := true;
            end;
        end;
	end
	else
	begin
		Result := false;
	end;

end;

//----------------------------------------------------------------
// Indicates if the ShipWorks common app data folder already exists
//----------------------------------------------------------------
function CommonAppDataExists(): Boolean;
begin

	if (DirExists(ExpandConstant('{commonappdata}') + '\Interapptive'))
    then begin
        Result := true;
    end
	else
	begin
		Result := false;
	end;

end;

//----------------------------------------------------------------
// Add all our custom pages
//----------------------------------------------------------------
procedure InitializeWizard();
var
  LastPageID: Integer;
begin
  LastPageID := wpLicense;
  LastPageID := CreateDotNetDownloadPage(LastPageID);
  LastPageID := CreateDotNetInstallPage(LastPageID);
  LastPageID := CreateKB2468871DownloadPage(LastPageID);
  LastPageID := CreateKB2468871InstallPage(LastPageID);
end;

//----------------------------------------------------------------
// Verfies that all necessary conditions are met before continuing
// with the install.
//----------------------------------------------------------------
procedure CheckInstallConditions(CurPage: Integer);
begin
	// Has to be XPSP2+
	CheckForWinVersion(CurPage);

	// Has to be logged on as administrator
	CheckForAdmin(CurPage);

	// Needs to be IE 5.5
	CheckForIEVersion(CurPage);
end;

//----------------------------------------------------------------
// Check the selected installation directory and warn about upgrade issues.
//----------------------------------------------------------------
function CheckUpgradeIssues() : Boolean;
var
	TargetExe: string;
	VersionFound: string;
	SchemaFound: Integer;
begin

	Result := True;

	TargetExe := ExpandConstant('{app}') + '\ShipWorks.exe';

	if (FileExists(TargetExe))
	then begin
		GetVersionNumbersString(TargetExe, VersionFound);

		// Upgrading from 2x
		if (Pos('2.', VersionFound) = 1)
		then begin

			 // During beta we found it best to not even allow them to overwrite sw2, or they end up regretting it.
			 MsgBox(
				'The installation folder you selected contains ShipWorks 2. We have found that it is best to leave ShipWorks 2 around while getting up to speed with ShipWorks 3.' + #13 +
                   '' + #13 +
                   'Please select a different installation directory.  The first time you run ShipWorks 3 it will walk you through upgrading your data from ShipWorks 2.',
                 mbCriticalError,
                 MB_OK);
             Result := False;
		end

		// Existing 3x version
		else if (Pos('3.', VersionFound) = 1)
		then begin

			// See if a DB upgrade will be required.
		    if Exec(ExpandConstant(TargetExe), '/command:getdbschemaversion -type:database', '', SW_SHOW, ewWaitUntilTerminated, SchemaFound)
		    then begin

				if ((SchemaFound > 0) and ({#RequiredSchemaID} > SchemaFound))
				then begin

					if (MsgBox('The version of ShipWorks being installed will require your database to be updated.' + #13 +
							   '' + #13 +
							   'Continue with installation?',
						mbConfirmation,
						MB_OKCANCEL) = IDCANCEL)
					then begin
						Result := False;
					end;

				end;

			end;

		end;
	end;

end;

//----------------------------------------------------------------
// Called when the user clicks Next.  Forwads the call to the main
// script handler.
//----------------------------------------------------------------
function NextButtonClick(CurPage: Integer): Boolean;
var
	serviceWasStopped: Integer;
begin

	Result := True;

	if (CurPage = wpWelcome)
	then begin
		CheckInstallConditions(CurPage);
	end;

	if (CurPage = wpSelectDir)
	then begin
		Result := CheckUpgradeIssues();
	end;

    if (CurPage = wpReady)
    then begin

		if (ShipWorksVersionHasScheduler())
		then begin
			Exec(ExpandConstant(ExpandConstant('{app}') + '\ShipWorks.exe'), '/s=scheduler /stop', '', SW_SHOW, ewWaitUntilTerminated, serviceWasStopped)
		end;

        if IsTaskSelected('desktopicon')
        then begin
            // We now call it just ShipWorks instead of ShipWorks 3
            DeleteFile(ExpandConstant('{userdesktop}\ShipWorks 3.lnk'));
        end;

  end;

end;

//----------------------------------------------------------------
// Called by Inno to see if it should restart.  We will determine
// this depending on what the dotnet and mdac installs said.
//----------------------------------------------------------------
function NeedRestart(): Boolean;
begin
	Result := DotNetNeedsReboot;
end;

// In case we need to see if anything went wrong
#call SaveToFile('InnoSetup.iss')
