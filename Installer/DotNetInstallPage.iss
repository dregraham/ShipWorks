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
#ifndef DOTNETINSTALL_ISS
#define DOTNETINSTALL_ISS

//----------------------------------------------------------------
// Includes
//----------------------------------------------------------------
#include "DotNet.iss"

function InstallDotNetFramework(Page: TWizardPage): Boolean;
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
    installProgress.SetText('Installing the Microsoft .NET Framework...', '');
    installProgress.SetProgress(0, 0);
    installProgress.Show;

    if (IsDotNetIncluded()) then begin
		installFile := 'dotNetFx40_Full_x86_x64.exe';
	end else begin
		installFile := 'dotNetFx40_Full_setup.exe';
	end;

    try
		execSuccess := ShellExec('open',
			ExpandConstant('{tmp}') + '\' + installFile,
			'/norestart /passive', '', SW_SHOW, ewWaitUntilTerminated, returnCode);

		// Assume failure, until we found out otherwise
		failReason := 'A problem was encountered while installing the Microsoft .NET Framework.' + #13 + #13 + 'Error: ';

		// The application was executed (doesnt mean it installed ok)
		if (execSuccess) then
		begin

			// If it is installed OK
			if ((returnCode = 0   ) or 	// Success
			    (returnCode = 1614) or  // Success, need reboot
				(returnCode = 3010)) 	// Success, need reboot
			then begin

				DotNetWasInstalled := True;
				DotNetNeedsReboot := (returnCode = 3010) or (returnCode = 1614);

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
		if (not DotNetWasInstalled) then begin
			ShowErrorPage(
		        Page.ID,
		        'Setup Error',
		        'Could not install the .NET Framework.',
			    failReason + '.' + #13 + #13 + 'You can download the .NET Framework directly from the following link.',
			    'http://www.microsoft.com/downloads/details.aspx?FamilyID=9cfb2d51-5ff4-4491-b0e5-b386f32c0992');
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
function OnInstallDotNetShouldSkipPage(Page: TWizardPage): Boolean;
begin
  Result := not (IsDotNetInstallRequired() and not DotNetWasInstalled);
end;

//----------------------------------------------------------------
// Called to start installing .net
//----------------------------------------------------------------
function OnInstallDotNetNextButtonClick(Page: TWizardPage): Boolean;
var
    extractProgress: TOutputProgressWizardPage;
begin

  // See if we first have to extract the file from setup
  if (IsDotNetIncluded() and not FileExists(ExpandConstant('{tmp}\dotNetFx40_Full_x86_x64.exe'))) then
	begin

        extractProgress := CreateOutputProgressPage(
            Page.Caption,
            Page.Description);
        extractProgress.SetText('Preparing installation files...', '');
        extractProgress.SetProgress(0, 0);
        extractProgress.Show;

        try
		    // Try to extract the file ourselves
		    ExtractTemporaryFile('dotNetFx40_Full_x86_x64.exe');
        finally
            extractProgress.Hide;
        end;
  end;

  // Do the install
  Result := InstallDotNetFramework(Page);

end;

//----------------------------------------------------------------
// Creates the custom .net install page
//----------------------------------------------------------------
function CreateDotNetInstallPage(PreviousPageId: Integer): Integer;
var
  infoLabel: TLabel;
  Page: TWizardPage;
begin
  Page := CreateCustomPage(
    PreviousPageId,
    ExpandConstant('Install Microsoft .NET Framework 4.0'),
    ExpandConstant('ShipWorks requires the Microsoft .NET Framework 4.0.')
  );

  infoLabel := TLabel.Create(Page);
  infoLabel.Parent := Page.Surface;

  if (IsDotNetIncluded()) then
  begin
      infoLabel.Caption :=
        'ShipWorks requires the Microsoft .NET Framework 4.0, which is not installed on your computer.' + #13 +
        '' + #13 +
        'Click Next to install the .NET Framework.';
  end
  else
  begin
      infoLabel.Caption :=
        'Setup has downloaded the .NET Framework.' + #13 +
        '' + #13 +
        'Click Next to begin the installation.';
  end

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
    OnShouldSkipPage := @OnInstallDotNetShouldSkipPage;
    OnNextButtonClick := @OnInstallDotNetNextButtonClick;
  end;

  Result := Page.ID;
end;


// File Gaurds
#endif
