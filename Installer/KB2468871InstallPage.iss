//----------------------------------------------------------------
// Header section to make this work in ISTool
//----------------------------------------------------------------
#ifdef EXCLUDE
[Setup]

[_ISTool]
EnableISX=true

[Code]
#endif

// File Gaurds
#ifndef KB2468871INSTALL_ISS
#define KB2468871INSTALL_ISS

//----------------------------------------------------------------
// Includes
//----------------------------------------------------------------
#include "DotNet.iss"
#include "KB2468871.iss"

function InstallKB2468871(Page: TWizardPage): Boolean;
var
    installProgress: TOutputProgressWizardPage;
    returnCode: Integer;
    execSuccess: Boolean;
    failReason: String;
    installFile: String;
begin

    installProgress := CreateOutputProgressPage(
        Page.Caption,
        Page.Description);
    installProgress.SetText('Installing the Microsoft .NET Framework update...', '');
    installProgress.SetProgress(0, 0);
    installProgress.Show;

    installFile := GetKB2468871FileName();

    try
		execSuccess := ShellExec('open',
			ExpandConstant('{tmp}') + '\' + installFile,
			'/passive', '', SW_SHOW, ewWaitUntilTerminated, returnCode);

		// Assume failure, until we found out otherwise
		failReason := 'A problem was encountered while installing the Microsoft .NET Framework update.' + #13 + #13 + 'Error: ';

		// The application was executed (doesnt mean it installed ok)
		if (execSuccess) then
		begin

			// If it is installed OK
			if ((returnCode = 0   ) or 	// Success
			    (returnCode = 1614) or  // Success, need reboot
				(returnCode = 3010)) 	// Success, need reboot
			then begin

				KB2468871WasInstalled := True;
				//KB2468871NeedsReboot := (returnCode = 3010) or (returnCode = 1614);

			// Did not install OK
			end else begin
				if (Length(SysErrorMessage(returnCode)) > 0) then
					failReason := failReason + SysErrorMessage(returnCode)
				else
					failReason := failReason + IntToStr(returnCode);
			end;

		// The application did not execute at all
		end else begin
			failReason := failReason + SysErrorMessage(returnCode);
		end;

		// Something bad happend
		if (not KB2468871WasInstalled) then begin
			ShowErrorPage(
		        Page.ID,
		        'Setup Error',
		        'Could not install the .NET Framework update.',
			    failReason + '.' + #13 + #13 + 'You can download the .NET Framework update directly from the following link.',
			    'http://www.microsoft.com/en-us/download/details.aspx?id=3556');
		end;

    finally
        installProgress.Hide;
    end;

	// If the user went to something else in the interim, let them know that this is ready.
	BringToFrontAndRestore();

    Result := True;
end;

//----------------------------------------------------------------
// Called to see if we should skip installing .NET
//----------------------------------------------------------------
function OnInstallKB2468871ShouldSkipPage(Page: TWizardPage): Boolean;
begin
  Result := not (IsKB2468871InstallRequired() and not KB2468871WasInstalled);
end;

//----------------------------------------------------------------
// Called to start installing .net
//----------------------------------------------------------------
function OnInstallKB2468871NextButtonClick(Page: TWizardPage): Boolean;
begin

  // Do the install
  Result := InstallKB2468871(Page);

end;

//----------------------------------------------------------------
// Creates the custom .net install page
//----------------------------------------------------------------
function CreateKB2468871InstallPage(PreviousPageId: Integer): Integer;
var
  infoLabel: TLabel;
  Page: TWizardPage;
begin
  Page := CreateCustomPage(
    PreviousPageId,
    ExpandConstant('Install Microsoft .NET Framework ' + GetSupportedDotNetVersion() + ' update'),
    ExpandConstant('ShipWorks requires an update for the Microsoft .NET Framework ' + GetSupportedDotNetVersion() + '.')
  );

  infoLabel := TLabel.Create(Page);
  infoLabel.Parent := Page.Surface;

  infoLabel.Caption :=
    'Setup has downloaded the .NET Framework update.' + #13 +
    '' + #13 +
    'Click Next to begin the installation.';

  with infoLabel do
  begin
    Left := ScaleX(0);
    Top := ScaleY(0);
    Width := ScaleX(415);
    Height := ScaleY(52);
    WordWrap := True;
  end;

  with Page do
  begin
    OnShouldSkipPage := @OnInstallKB2468871ShouldSkipPage;
    OnNextButtonClick := @OnInstallKB2468871NextButtonClick;
  end;

  Result := Page.ID;
end;


// File Gaurds
#endif
