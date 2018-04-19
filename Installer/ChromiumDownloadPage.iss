// File Gaurds
#ifndef CHROMIUMDOWNLOAD_ISS
#define CHROMIUMDOWNLOAD_ISS

[Code]
#define DwinsHs_Data_Buffer_Length 16384

#include "dwinshs.iss"
#include "Chromium.iss"
#include "Utilities.iss"

var
  C_DownloadIndicator: TNewProgressBar;
  C_BackClicked: Boolean;
  C_SizeLabel: TLabel;
  C_Megabyte: extended;

//----------------------------------------------------------------
// Called when each block of the download is read
//----------------------------------------------------------------
function C_OnRead(URL, Agent: String; Method: TReadMethod; Index, TotalSize, ReadSize,
  CurrentSize: {#BIG_INT}; var ReadStr: String): Boolean;
begin
  if Index = 0 then C_DownloadIndicator.Max := TotalSize;
  C_DownloadIndicator.Position := ReadSize; // Update the download progress indicator
  C_SizeLabel.Caption := Format('%3.1f of %3.1f MB', [ReadSize / C_Megabyte, TotalSize / C_Megabyte]);
  Result := not C_BackClicked; // Determine whether download was cancelled
end;

//----------------------------------------------------------------
// Download the .NET installer
//----------------------------------------------------------------
function DownloadChromium(CurPageID: Integer): Boolean;
var
  Response: String;
  Size: {#BIG_INT};
  DownloadResult: Integer;
  DownloadSuccessful: Boolean;
  DownloadCanceled: Boolean;
  DownloadFileName: String;
  failReason: String;
begin
    DownloadFileName := ExpandConstant('{tmp}') + '\' + GetChromiumFileName();

    // Disbale to continue before download completes
    WizardForm.NextButton.Enabled := False;

    DownloadResult := DwinsHs_ReadRemoteURL(GetChromiumDownloadURL(), 'ShipWorks_Installer', rmGet,
      Response, Size, DownloadFileName, @C_OnRead);

    DownloadSuccessful := DownloadResult = READ_OK;
    DownloadCanceled := DownloadResult = READ_ERROR_CANCELED;

    if (not DownloadSuccessful) then
    	DeleteFile(DownloadFileName);

    if (DownloadSuccessful)
    then begin
      if (not DirExists(GetChromiumDestination())) then ForceDirectories(GetChromiumDestination());
      UnZip(DownloadFileName, GetChromiumDestination() + '\');
    end else begin
      failReason := 'Setup was unable to download the template renderer.  Please check that you have an open ' +
          'connection to the internet, and run Setup again.  Error: ' + IntToStr(DownloadResult) + ' ' + DownloadFileName;
    end;

    // If we didnt get it, show an error
    if (not (DownloadSuccessful or DownloadCanceled)) then
        ShowErrorPage(
            CurPageID,
            'Setup Error',
            'Could not download the template renderer.',
            failReason + #13 + #13 + 'You can download it directly from the following link.',
            GetChromiumDownloadURL());

    WizardForm.NextButton.Enabled := True;
    Result := not DownloadCanceled;
end;

//----------------------------------------------------------------
// Called when the back button is clicked
//----------------------------------------------------------------
function OnDownloadChromiumBackButtonClick(Page: TWizardPage): Boolean;
begin
  // Stop to download
  C_BackClicked := True;
  Result := True;
  WizardForm.NextButton.Enabled := True;
end;

//----------------------------------------------------------------
// Called to start downloading .net
//----------------------------------------------------------------
function OnDownloadChromiumNextButtonClick(Page: TWizardPage): Boolean;
begin
  Result := DownloadChromium(Page.ID);
end;

//----------------------------------------------------------------
// Called to see if we should skip downloading .NET
//----------------------------------------------------------------
function OnDownloadChromiumShouldSkipPage(Page: TWizardPage): Boolean;
begin
  Result := not (IsChromiumInstallRequired() and not FileExists(ExpandConstant('{tmp}\' + GetChromiumFileName())));
end;

//----------------------------------------------------------------
// Called when the wizard page becomes active
//----------------------------------------------------------------
procedure OnDownloadChromiumActivate(Page: TWizardPage);
begin
    // Allow to download
    DwinsHs_CancelDownload := cdNone;
    C_BackClicked := False;
    C_DownloadIndicator.Position := 0;
    C_SizeLabel.Caption := '';
end;

//----------------------------------------------------------------
// Creates the custom .net download page
//----------------------------------------------------------------
function CreateChromiumDownloadPage(PreviousPageId: Integer): Integer;
var
  infoLabel: TLabel;
  Page: TWizardPage;
begin
  C_Megabyte := 1024.0 * 1024.0;

  Page := CreateCustomPage(
    PreviousPageId,
    'Install template renderer',
    'ShipWorks requires a renderer for templates'
  );

  infoLabel := TLabel.Create(Page);
  with infoLabel do
  begin
    Parent := Page.Surface;
    Caption :=
      'ShipWorks requires a template renderer, which is not installed on your computer.' + #13 +
      '' + #13 +
      'Click Next to download it now (60 MB).';
    Left := ScaleX(0);
    Top := ScaleY(0);
    Width := ScaleX(415);
    Height := ScaleY(52);
    WordWrap := True;
  end;

  C_SizeLabel := TLabel.Create(Page);
  with C_SizeLabel do
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
  C_DownloadIndicator := TNewProgressBar.Create(Page);
  C_DownloadIndicator.Left := ScaleX(0);
  C_DownloadIndicator.Top := ScaleY(67);
  C_DownloadIndicator.Width := Page.SurfaceWidth;
  C_DownloadIndicator.Height:= ScaleY(20);
  C_DownloadIndicator.Min := 0;
  C_DownloadIndicator.Parent := Page.Surface;

  with Page do
  begin
    OnActivate := @OnDownloadChromiumActivate;
    OnShouldSkipPage := @OnDownloadChromiumShouldSkipPage;
    OnNextButtonClick := @OnDownloadChromiumNextButtonClick;
    OnBackButtonClick := @OnDownloadChromiumBackButtonClick;
  end;

  Result := Page.ID;
end;

#endif
