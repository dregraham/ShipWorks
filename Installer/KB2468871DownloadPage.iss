// File Gaurds
#ifndef KB2468871DOWNLOAD_ISS
#define KB2468871DOWNLOAD_ISS

[Code]
#define DwinsHs_Data_Buffer_Length 16384

#include "KB2468871.iss"
#include "dwinshs.iss"

var
  KB_DownloadIndicator: TNewProgressBar;
  KB_BackClicked: Boolean;
  KB_SizeLabel: TLabel;
  KB_Megabyte: extended;

//----------------------------------------------------------------
// Called when each block of the download is read
//----------------------------------------------------------------
function OnKBRead(URL, Agent: String; Method: TReadMethod; Index, TotalSize, ReadSize,
  CurrentSize: {#BIG_INT}; var ReadStr: String): Boolean;
begin
  if Index = 0 then KB_DownloadIndicator.Max := TotalSize;
  KB_DownloadIndicator.Position := ReadSize; // Update the download progress indicator
  KB_SizeLabel.Caption := Format('%3.1f of %3.1f MB', [ReadSize / KB_Megabyte, TotalSize / KB_Megabyte]);
  Result := not KB_BackClicked; // Determine whether download was cancelled
end;

//----------------------------------------------------------------
// Download the .NET installer
//----------------------------------------------------------------
function DownloadKB2468871(CurPageID: Integer): Boolean;
var
  Response: String;
  Size: {#BIG_INT};
  DownloadResult: Integer;
  DownloadSuccessful: Boolean;
  DownloadCanceled: Boolean;
begin
    // Disbale to continue before download completes
    WizardForm.NextButton.Enabled := False;

    DownloadResult := DwinsHs_ReadRemoteURL(GetKB2468871DownloadURL(), 'ShipWorks_Installer', rmGet,
      Response, Size, ExpandConstant('{tmp}') + '\' + GetKB2468871FileName(), @OnKBRead);

    DownloadSuccessful := DownloadResult = READ_OK;
    DownloadCanceled := DownloadResult = READ_ERROR_CANCELED;

    if (not DownloadSuccessful) then
    	DeleteFile(ExpandConstant('{tmp}') + '\' + GetKB2468871FileName());

    // If we didnt get it, show an error
    if (not (DownloadSuccessful or DownloadCanceled)) then
        ShowErrorPage(
            CurPageID,
		    'Setup Error',
		    'Could not download the .NET Framework update.',
			'Setup was unable to download the Microsoft .NET Framework update.  Please check that you have an open ' +
			    'connection to the internet, and run Setup again.' + #13 + #13 +
			    'You can also download the .NET Framework update directly from the following link.',
			'http://www.microsoft.com/en-us/download/details.aspx?id=3556');

    WizardForm.NextButton.Enabled := True;
    Result := not DownloadCanceled;
end;

//----------------------------------------------------------------
// Called when the back button is clicked
//----------------------------------------------------------------
function OnDownloadKB2468871BackButtonClick(Page: TWizardPage): Boolean;
begin
  // Stop to download
  KB_BackClicked := True;
  Result := True;
  WizardForm.NextButton.Enabled := True;
end;

//----------------------------------------------------------------
// Called to start downloading .net
//----------------------------------------------------------------
function OnDownloadKB2468871NextButtonClick(Page: TWizardPage): Boolean;
begin
  Result := DownloadKB2468871(Page.ID);
end;

//----------------------------------------------------------------
// Called to see if we should skip downloading .NET
//----------------------------------------------------------------
function OnDownloadKB2468871ShouldSkipPage(Page: TWizardPage): Boolean;
begin
  Result := not (IsKB2468871InstallRequired() and not FileExists(ExpandConstant('{tmp}\' + GetKB2468871FileName())));
end;

//----------------------------------------------------------------
// Called when the wizard page becomes active
//----------------------------------------------------------------
procedure OnDownloadKB2468871Activate(Page: TWizardPage);
begin
    // Allow to download
    DwinsHs_CancelDownload := cdNone;
    KB_BackClicked := False;
    KB_DownloadIndicator.Position := 0;
    KB_SizeLabel.Caption := '';
end;

//----------------------------------------------------------------
// Creates the custom .net download page
//----------------------------------------------------------------
function CreateKB2468871DownloadPage(PreviousPageId: Integer): Integer;
var
  infoLabel: TLabel;
  Page: TWizardPage;
begin
  KB_Megabyte := 1024.0 * 1024.0;

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
      'Click Next to download it now (18.7 MB).';
    Left := ScaleX(0);
    Top := ScaleY(0);
    Width := ScaleX(415);
    Height := ScaleY(52);
    WordWrap := True;
  end;

  KB_SizeLabel := TLabel.Create(Page);
  with KB_SizeLabel do
  begin
    Parent := Page.Surface;
    Caption := '';
    Left := ScaleX(0);
    Top := ScaleY(90);
    Width := Page.SurfaceWidth;
    Height := ScaleY(52);
    Alignment := taRightJustify;
  end;

  // Create the download progress indicator
  KB_DownloadIndicator := TNewProgressBar.Create(Page);
  KB_DownloadIndicator.Left := ScaleX(0);
  KB_DownloadIndicator.Top := ScaleY(67);
  KB_DownloadIndicator.Width := Page.SurfaceWidth;
  KB_DownloadIndicator.Height:= ScaleY(20);
  KB_DownloadIndicator.Min := 0;
  KB_DownloadIndicator.Parent := Page.Surface;

  with Page do
  begin
    OnActivate := @OnDownloadKB2468871Activate;
    OnShouldSkipPage := @OnDownloadKB2468871ShouldSkipPage;
    OnNextButtonClick := @OnDownloadKB2468871NextButtonClick;
    OnBackButtonClick := @OnDownloadKB2468871BackButtonClick;
  end;

  Result := Page.ID;
end;

#endif
