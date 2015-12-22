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
#ifndef KBDOWNLOAD_ISS
#define KBDOWNLOAD_ISS

//----------------------------------------------------------------
// Includes
//----------------------------------------------------------------
#include "DotNet.iss"
#include "KB2468871.iss"

//----------------------------------------------------------------
// Attempts to download KB2468871 from the internet
//----------------------------------------------------------------
function DownloadKB2468871(CurPage: Integer): Boolean;
var
	hWnd: Integer;
	Success: Integer;
begin
	// They do, so  setup the download
	isxdl_ClearFiles();
	isxdl_SetOption('title','Setup - Downloading update for Microsoft .NET Framework ' + GetSupportedDotNetVersion() + '...');
	isxdl_SetOption('noftpsize','false');
	isxdl_SetOption('aborttimeout','8');
	hWnd := StrToInt(ExpandConstant('{wizardhwnd}'));

	// Hide this window
	ShowWindow(hWnd, SW_HIDE);

	isxdl_AddFileSize(
		GetKB2468871DownloadURL(),
		ExpandConstant('{tmp}\' + GetKB2468871FileName()),
		GetKB2468871FileSize);

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
		    'Could not download the .NET Framework update.',
			'Setup was unable to download the Microsoft .NET Framework update.  Please check that you have an open ' +
			    'connection to the internet, and run Setup again.' + #13 + #13 +
			    'You can also download the .NET Framework update directly from the following link.',
			'http://www.microsoft.com/en-us/download/details.aspx?id=3556');

	Result := True;

end;

//----------------------------------------------------------------
// Called to see if we should skip downloading .NET
//----------------------------------------------------------------
function OnDownloadKB2468871ShouldSkipPage(Page: TWizardPage): Boolean;
begin

  Result := not (IsKB2468871InstallRequired() and not FileExists(ExpandConstant('{tmp}\' + GetKB2468871FileName())));

end;

//----------------------------------------------------------------
// Called to start downloading .net
//----------------------------------------------------------------
function OnDownloadKB2468871NextButtonClick(Page: TWizardPage): Boolean;
begin

  Result := DownloadKB2468871(Page.ID);

end;

//----------------------------------------------------------------
// Creates the custom .net download page
//----------------------------------------------------------------
function CreateKB2468871DownloadPage(PreviousPageId: Integer): Integer;
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
  with infoLabel do
  begin
    Parent := Page.Surface;
    Caption :=
      'ShipWorks requires an update to the Microsoft .NET Framework ' + GetSupportedDotNetVersion() + ', which is not installed on your computer.' + #13 +
      '' + #13 +
      'Click Next to download it now (19.1 MB).';
    Left := ScaleX(0);
    Top := ScaleY(0);
    Width := ScaleX(415);
    Height := ScaleY(52);
    WordWrap := True;
  end;

  with Page do
  begin
    OnShouldSkipPage := @OnDownloadKB2468871ShouldSkipPage;
    OnNextButtonClick := @OnDownloadKB2468871NextButtonClick;
  end;

  Result := Page.ID;
end;


// File Gaurds
#endif
