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

#define CurrentYear GetDateTimeString('yyyy', '', '');

[Setup]
AppName=ShipWorks�
AppVersion={#= Version} {#= EditionName}
AppVerName=ShipWorks� {#= Version} {#= EditionName}
AppPublisher=Interapptive�, Inc.
AppPublisherURL=http://www.shipworks.com
AppSupportURL=http://www.shipworks.com
AppUpdatesURL=http://www.shipworks.com
AppMutex={{AX70DA71-2A39-4f8c-8F97-7F5348493F57}
DefaultDirName={pf}\ShipWorks
DefaultGroupName=ShipWorks
LicenseFile=License.rtf
MinVersion=6.1
PrivilegesRequired=none
DisableStartupPrompt=true
AllowRootDirectory=false
UserInfoPage=false
ShowComponentSizes=false
WizardImageFile=WizardLarge.bmp
UninstallDisplayIcon={app}\ShipWorks.exe
UsePreviousLanguage=no
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
VersionInfoCompany=Interapptive�, Inc.
VersionInfoDescription=Interapptive� ShipWorks�
VersionInfoTextVersion=ShipWorks� {#= Version}
VersionInfoCopyright=Copyright � Interapptive�, Inc. 2003-{#= CurrentYear}
ArchitecturesInstallIn64BitMode=x64
AppendDefaultDirName=false
DisableDirPage=no
DisableProgramGroupPage=no


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
Source: License.rtf; DestDir: {app}; Flags: overwritereadonly ignoreversion; BeforeInstall: BackupInstallingFile
Source: {#AppArtifacts}\*.exe; DestDir: {app}; Flags: overwritereadonly ignoreversion; BeforeInstall: BackupInstallingFile
Source: {#AppArtifacts}\{#= EditionAppConfig}; DestDir: {app}; DestName: "ShipWorks.exe.config"; Flags: overwritereadonly ignoreversion; BeforeInstall: BackupInstallingFile
Source: {#AppArtifacts}\swc.exe.config; DestDir: {app}; Flags: overwritereadonly ignoreversion; BeforeInstall: BackupInstallingFile
Source: {#AppArtifacts}\ShipWorks.Escalator.exe.config; DestDir: {app}; Flags: overwritereadonly ignoreversion; BeforeInstall: BackupInstallingFile
Source: {#AppArtifacts}\*.dll; DestDir: {app}; Excludes: "ShipWorks.Native.dll"; Flags: overwritereadonly ignoreversion; BeforeInstall: BackupInstallingFile
Source: {#AppArtifacts}\x64\ShipWorks.Native.dll; DestDir: {app}; Flags: overwritereadonly ignoreversion; Check: Is64BitInstallMode; BeforeInstall: BackupInstallingFile
Source: {#AppArtifacts}\Win32\ShipWorks.Native.dll; DestDir: {app}; Flags: overwritereadonly ignoreversion; Check: not Is64BitInstallMode; BeforeInstall: BackupInstallingFile
Source: {#AppArtifacts}\fre3of9x.ttf; DestDir: {fonts}; FontInstall: Free 3 of 9 Extended; Flags: onlyifdoesntexist uninsneveruninstall
Source: {#AppArtifacts}\FontLicense.txt; DestDir: {app}; Flags: overwritereadonly ignoreversion; BeforeInstall: BackupInstallingFile
Source: {#AppArtifacts}\SingleScanPanels.swe; DestDir: {app}; Flags: overwritereadonly ignoreversion; BeforeInstall: BackupInstallingFile

#ifdef IncludeSymbols
    Source: {#AppArtifacts}\ShipWorks.pdb; DestDir: {app}; Flags: overwritereadonly ignoreversion
    Source: {#AppArtifacts}\ShipWorks.Shared.pdb; DestDir: {app}; Flags: overwritereadonly ignoreversion
    Source: {#AppArtifacts}\ShipWorks.Data.Model.pdb; DestDir: {app}; Flags: overwritereadonly ignoreversion
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
Root: HKLM; Subkey: Software\Interapptive\ShipWorks; ValueType: string; ValueName: LastInstalledInstanceID; ValueData: {code:GetAppID}
Root: HKLM; Subkey: Software\Microsoft\Windows\CurrentVersion\Run; ValueType: string; ValueName: {code:GetBackgroundProcessName}; ValueData: {app}\ShipWorks.exe /s=Scheduler

[Run]
Filename: {app}\ShipWorks.exe; Description: Launch ShipWorks; Flags: nowait postinstall skipifsilent
Filename: {app}\ShipWorks.exe; Parameters: "/s=scheduler"; Flags: nowait skipifsilent; Check: not NeedRestart

[UninstallRun]
Filename: {app}\ShipWorks.exe; Parameters: "/command:uninstall"; Flags: runhidden
Filename: {app}\ShipWorks.Escalator.exe; Parameters: "--uninstall"; Flags: runhidden

[Dirs]
Name: {app}
Name: {commonappdata}\Interapptive; Permissions: everyone-modify; Check: not CommonAppDataExists

[Code]
//----------------------------------------------------------------
// Includes
//----------------------------------------------------------------
#include "DotNetDownloadPage.iss"
#include "DotNetInstallPage.iss";
#include "SystemChecks.iss"
#include "Guid.iss"

var
  newAppID: string;
  DatabaseUpgradeFailed: Boolean;
  CopyFilesSucceeded: Boolean;
  ShouldUpgradeDatabase: Boolean;
  InstallStarted: Boolean;
  ShouldLaunchShipWorks: Boolean;

//----------------------------------------------------------------
// Convert a bool to a string
//----------------------------------------------------------------

function BoolToStr(B: Boolean): string;
var 
	retVal: string;
begin
	if B then begin
		retVal := 'True';
	end
	else
	begin
		retVal := 'False';
	end
	Result := retVal;
end;

//----------------------------------------------------------------
// Initialize setup
//----------------------------------------------------------------
function InitializeSetup(): Boolean;
var
  j: Integer;
begin	
  InstallStarted := False;
  ShouldUpgradeDatabase := False;
  ShouldLaunchShipWorks := False;
  
  for j := 1 to ParamCount do
  begin
    if CompareText(ParamStr(j), '/upgradedb') = 0 then
    begin
      ShouldUpgradeDatabase := True;
    end;
    
    if CompareText(ParamStr(j), '/launchafterinstall') = 0 then
    begin
      ShouldLaunchShipWorks := True;
    end;
  end
  
  Log('ShouldUpgradeDatabase= ' + BoolToStr(ShouldUpgradeDatabase));
  Log('ShouldLaunchShipWorks= ' + BoolToStr(ShouldLaunchShipWorks));

  Result := True;
end;

//----------------------------------------------------------------
// Copy the contents of one directory to another
//----------------------------------------------------------------
procedure DirectoryCopy(SourcePath, DestPath: string);
var
  FindRec: TFindRec;
  SourceFilePath: string;
  DestFilePath: string;
begin
  if FindFirst(SourcePath + '\*', FindRec) then
  begin
    try
      repeat
        if (FindRec.Name <> '.') and (FindRec.Name <> '..') then
        begin
          SourceFilePath := SourcePath + '\' + FindRec.Name;
          DestFilePath := DestPath + '\' + FindRec.Name;
          if FindRec.Attributes and FILE_ATTRIBUTE_DIRECTORY = 0 then
          begin
            if FileCopy(SourceFilePath, DestFilePath, False) then
            begin
              Log(Format('Copied %s to %s', [SourceFilePath, DestFilePath]));
            end
              else
            begin
              Log(Format('Failed to copy %s to %s', [SourceFilePath, DestFilePath]));
            end;
          end
            else
          begin
            if DirExists(DestFilePath) or CreateDir(DestFilePath) then
            begin
              Log(Format('Created %s', [DestFilePath]));
              DirectoryCopy(SourceFilePath, DestFilePath);
            end
              else
            begin
              Log(Format('Failed to create %s', [DestFilePath]));
            end;
          end;
        end;
      until not FindNext(FindRec);
    finally
      FindClose(FindRec);
    end;
  end
    else
  begin
    Log(Format('Failed to list %s', [SourcePath]));
  end;
end;

//----------------------------------------------------------------
// Get the exit code
// -----------------
// This is NOT called when the setup has failed, which is how we
// can tell if setup succeeded or failed
//----------------------------------------------------------------
function GetCustomSetupExitCode: Integer;
begin
  CopyFilesSucceeded := True;

  if DatabaseUpgradeFailed then
  begin
  	Result := 0;
  end
  else
  begin
  	Result := 1;
  end;
end;

//----------------------------------------------------------------
// Backup a file before installing the new version
//----------------------------------------------------------------
procedure BackupInstallingFile();
var
	tempDir: string;
	existingFile: string;
	backupFile: string;
begin
	existingFile := ExpandConstant(CurrentFileName);

	if FileExists(existingFile)
	then begin
		tempDir := GetTempDir + '\InstallBackup';

		backupFile := CurrentFileName;
		StringChangeEx(backupFile, '{app}', tempDir, True);

		Log('BACKUP: Copying ' + existingFile + ' to ' + backupFile);

		CreateDir(tempDir);
		FileCopy(existingFile, backupFile, False);
	end
	else
	begin
		Log('BACKUP: File ' + existingFile + ' does not exist');
	end;
end;

//----------------------------------------------------------------
// Attempt to upgrade the database
//----------------------------------------------------------------
procedure UpgradeDatabase();
var
	ExecResult: Boolean;
	ResultCode: Integer;
	TargetExe: String;
begin
	Log('Running DB upgrade...')
	TargetExe := ExpandConstant('{app}') + '\swc.exe';

	ExecResult := Exec(TargetExe, '/command=upgradedatabaseschema', '', SW_HIDE, ewWaitUntilTerminated, ResultCode);

	if ExecResult and (ResultCode = 0) then
	begin
		Log('DB upgrade succeeded with exit code ' + IntToStr(ResultCode))
		DatabaseUpgradeFailed := False;
	end
	else
	begin
		Log('DB upgrade failed with exit code ' + IntToStr(ResultCode))
		DatabaseUpgradeFailed := True;
	end;
end;

//----------------------------------------------------------------
// The current wizard step has changed
//----------------------------------------------------------------
procedure CurStepChanged(CurStep: TSetupStep);
begin
  if CurStep = ssPostInstall then begin
  	if ShouldUpgradeDatabase then
  	begin
    	UpgradeDatabase();
    end;    
  end;
end;

//----------------------------------------------------------------
// Roll back the install at the end of setup, if necessary
//----------------------------------------------------------------
procedure DeinitializeSetup();
var
	tempDir: string;
	installStatus: string;
	serviceWasStarted: Integer;
begin
	tempDir := GetTempDir + '\InstallBackup';
	installStatus := 'success';

	if DirExists(tempDir)
	then begin
		if DatabaseUpgradeFailed or not CopyFilesSucceeded then
		begin
			Log('DatabaseUpgradeFailed = ' + BoolToStr(DatabaseUpgradeFailed));
			Log('CopyFilesSucceeded = ' + BoolToStr(CopyFilesSucceeded));
			Log('BACKUP: Install failed, rolling back...');
			DirectoryCopy(tempDir, ExpandConstant('{app}'));
			installStatus := 'failure';
		end;

		DelTree(tempDir, True, True, True);
	end
	else
	begin
		Log('BACKUP: Install failed, nothing to roll back');
		installStatus := 'failure';
	end;

	Log('Install Status = ' + installStatus);

	if InstallStarted then
	begin
		if WizardSilent AND ShouldLaunchShipWorks then
  		begin
			Exec(ExpandConstant(ExpandConstant('{app}') + '\ShipWorks.Escalator.exe'), '--launchshipworks', '', SW_HIDE, ewWaitUntilTerminated, serviceWasStarted)
		end;   

		Exec(ExpandConstant(WizardDirValue + '\ShipWorks.Escalator.exe'), '--install', '', SW_HIDE, ewWaitUntilTerminated, serviceWasStarted)
	end;
end;

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

//-------------------------------------------------------------------------
// Does Escelator Service Exists
//-------------------------------------------------------------------------
function ShipWorksEscalatorServiceExists(): Boolean;
var 
	TargetExe: string;
begin
	TargetExe := ExpandConstant('{app}') + '\ShipWorks.Escalator.exe';
	Result := (FileExists(TargetExe));
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
end;

//----------------------------------------------------------------
// Verfies that all necessary conditions are met before continuing
// with the install.
//----------------------------------------------------------------
procedure CheckInstallConditions(CurPage: Integer);
begin
    // Has to be Win7+
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

    if ((CurPage = wpSelectDir) and (not ShouldUpgradeDatabase))
    then begin
        Result := CheckUpgradeIssues();
    end;

    if (CurPage = wpReady)
    then begin
		Log('Setting InstallStarted to True');
		InstallStarted := True;

        if (ShipWorksVersionHasScheduler())
        then begin
            Exec(ExpandConstant(ExpandConstant('{app}') + '\ShipWorks.exe'), '/s=scheduler /stop', '', SW_HIDE, ewWaitUntilTerminated, serviceWasStopped)
        end;

        if IsTaskSelected('desktopicon')
        then begin
            // We now call it just ShipWorks instead of ShipWorks 3
            DeleteFile(ExpandConstant('{userdesktop}\ShipWorks 3.lnk'));
        end;

        if(ShipWorksEscalatorServiceExists())
        then begin
            Exec(ExpandConstant(ExpandConstant('{app}') + '\ShipWorks.Escalator.exe'), '--stop', '', SW_HIDE, ewWaitUntilTerminated, serviceWasStopped)
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
