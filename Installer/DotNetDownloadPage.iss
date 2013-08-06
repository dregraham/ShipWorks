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
#ifndef DOTNETDOWNLOAD_ISS
#define DOTNETDOWNLOAD_ISS

//----------------------------------------------------------------
// Includes
//----------------------------------------------------------------
#include "DotNet.iss"

//----------------------------------------------------------------
// Attempts to download .NET from the internet
//----------------------------------------------------------------
function DownloadDotNet(CurPage: Integer): Boolean;
var
	hWnd: Integer;
	Success: Integer;
begin
	// They do, so  setup the download
	isxdl_ClearFiles();
	isxdl_SetOption('title','Setup - Downloading Microsoft .NET Framework ' + GetSupportedDotNetVersion() + '...');
	isxdl_SetOption('noftpsize','false');
	isxdl_SetOption('aborttimeout','8');
	hWnd := StrToInt(ExpandConstant('{wizardhwnd}'));

	// Hide this window
	ShowWindow(hWnd, SW_HIDE);

	isxdl_AddFileSize(
		GetDotNetDownloadURL(),
		ExpandConstant('{tmp}\' + GetDotNetFileName()),
		GetDotNetFileSize);

	// Try to download
	Success := isxdl_DownloadFiles(hWnd);

	// Hide this window
	ShowWindow(hWnd, SW_SHOWNORMAL);
	BringToFrontAndRestore();

	// If we didnt get it, show an error
	if (Success = 0) then
		ShowErrorPage(
		    CurPage,
		    'Setup Error',
		    'Could not download the .NET Framework.',
			'Setup was unable to download the Microsoft .NET Framework.  Please check that you have an open ' +
			    'connection to the internet, and run Setup again.' + #13 + #13 +
			    'You can also download the .NET Framework directly from the following link.',
			'http://www.microsoft.com/downloads/details.aspx?FamilyID=9cfb2d51-5ff4-4491-b0e5-b386f32c0992');

	Result := True;

end;

//----------------------------------------------------------------
// Called to see if we should skip downloading .NET
//----------------------------------------------------------------
function OnDownloadDotNetShouldSkipPage(Page: TWizardPage): Boolean;
begin
  Result := not (IsDotNetInstallRequired() and not FileExists(ExpandConstant('{tmp}\' + GetDotNetFileName())));
end;

//----------------------------------------------------------------
// Called to start downloading .net
//----------------------------------------------------------------
function OnDownloadDotNetNextButtonClick(Page: TWizardPage): Boolean;
begin
  Result := DownloadDotNet(Page.ID);
end;

//----------------------------------------------------------------
// Creates the custom .net download page
//----------------------------------------------------------------
function CreateDotNetDownloadPage(PreviousPageId: Integer): Integer;
var
  infoLabel: TLabel;
  Page: TWizardPage;
begin
  Page := CreateCustomPage(
    PreviousPageId,
    ExpandConstant('Install Microsoft .NET Framework ' + GetSupportedDotNetVersion()),
    ExpandConstant('ShipWorks requires the Microsoft .NET Framework ' + GetSupportedDotNetVersion() + '.')
  );

  infoLabel := TLabel.Create(Page);
  with infoLabel do
  begin
    Parent := Page.Surface;
    Caption :=
      'ShipWorks requires the Microsoft .NET Framework ' + GetSupportedDotNetVersion() + ', which is not installed on your computer.' + #13 +
      '' + #13 +
      'Click Next to download it now (0.9 MB).';
    Left := ScaleX(0);
    Top := ScaleY(0);
    Width := ScaleX(415);
    Height := ScaleY(52);
    WordWrap := True;
  end;

  with Page do
  begin
    OnShouldSkipPage := @OnDownloadDotNetShouldSkipPage;
    OnNextButtonClick := @OnDownloadDotNetNextButtonClick;
  end;

  Result := Page.ID;
end;


// File Gaurds
#endif
