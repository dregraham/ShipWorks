// File Gaurds
#ifndef DOTNETDOWNLOAD_ISS
#define DOTNETDOWNLOAD_ISS

[Code]
#define DwinsHs_Data_Buffer_Length 16384

#include "DotNet.iss"
#include "dwinshs.iss"

var
  DownloadIndicator: TNewProgressBar;
  BackClicked: Boolean;
  SizeLabel: TLabel;
  Megabyte: extended;

//----------------------------------------------------------------
// Called when each block of the download is read
//----------------------------------------------------------------
function OnRead(URL, Agent: String; Method: TReadMethod; Index, TotalSize, ReadSize,
  CurrentSize: {#BIG_INT}; var ReadStr: String): Boolean;
begin
  if Index = 0 then DownloadIndicator.Max := TotalSize;
  DownloadIndicator.Position := ReadSize; // Update the download progress indicator
  SizeLabel.Caption := Format('%3.1f of %3.1f MB', [ReadSize / Megabyte, TotalSize / Megabyte]);
  Result := not BackClicked; // Determine whether download was cancelled
end;

//----------------------------------------------------------------
// Download the .NET installer
//----------------------------------------------------------------
function DownloadDotNet(CurPageID: Integer): Boolean;
var
  Response: String;
  Size: {#BIG_INT};
  DownloadResult: Integer;
  DownloadSuccessful: Boolean;
  DownloadCanceled: Boolean;
begin
    // Disbale to continue before download completes
    WizardForm.NextButton.Enabled := False;

    DownloadResult := DwinsHs_ReadRemoteURL(GetDotNetDownloadURL(), 'ShipWorks_Installer', rmGet,
      Response, Size, ExpandConstant('{tmp}') + '\' + GetDotNetFileName(), @OnRead);

    DownloadSuccessful := DownloadResult = READ_OK;
    DownloadCanceled := DownloadResult = READ_ERROR_CANCELED;

    if (not DownloadSuccessful) then
    	DeleteFile(ExpandConstant('{tmp}') + '\' + GetDotNetFileName());

    // If we didnt get it, show an error
    if (not (DownloadSuccessful or DownloadCanceled)) then
        ShowErrorPage(
            CurPageID,
            'Setup Error',
            'Could not download the .NET Framework.',
            'Setup was unable to download the Microsoft .NET Framework.  Please check that you have an open ' +
                'connection to the internet, and run Setup again.' + #13 + #13 +
                'You can also download the .NET Framework directly from the following link.',
            'http://www.microsoft.com/downloads/details.aspx?FamilyID=9cfb2d51-5ff4-4491-b0e5-b386f32c0992');

    WizardForm.NextButton.Enabled := True;
    Result := not DownloadCanceled;
end;

//----------------------------------------------------------------
// Called when the back button is clicked
//----------------------------------------------------------------
function OnDownloadDotNetBackButtonClick(Page: TWizardPage): Boolean;
begin
  // Stop to download
  BackClicked := True;
  Result := True;
  WizardForm.NextButton.Enabled := True;
end;

//----------------------------------------------------------------
// Called to start downloading .net
//----------------------------------------------------------------
function OnDownloadDotNetNextButtonClick(Page: TWizardPage): Boolean;
begin
  Result := DownloadDotNet(Page.ID);
end;

//----------------------------------------------------------------
// Called to see if we should skip downloading .NET
//----------------------------------------------------------------
function OnDownloadDotNetShouldSkipPage(Page: TWizardPage): Boolean;
begin
  Result := not (IsDotNetInstallRequired() and not FileExists(ExpandConstant('{tmp}\' + GetDotNetFileName())));
end;

//----------------------------------------------------------------
// Called when the wizard page becomes active
//----------------------------------------------------------------
procedure OnDownloadDotNetActivate(Page: TWizardPage);
begin
    // Allow to download
    DwinsHs_CancelDownload := cdNone;
    BackClicked := False;
    DownloadIndicator.Position := 0;
    SizeLabel.Caption := '';
end;

//----------------------------------------------------------------
// Creates the custom .net download page
//----------------------------------------------------------------
function CreateDotNetDownloadPage(PreviousPageId: Integer): Integer;
var
  infoLabel: TLabel;
  Page: TWizardPage;
begin
  Megabyte := 1024.0 * 1024.0;

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
      'Click Next to download it now (62.4 MB).';
    Left := ScaleX(0);
    Top := ScaleY(0);
    Width := ScaleX(415);
    Height := ScaleY(52);
    WordWrap := True;
  end;

  SizeLabel := TLabel.Create(Page);
  with SizeLabel do
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
  DownloadIndicator := TNewProgressBar.Create(Page);
  DownloadIndicator.Left := ScaleX(0);
  DownloadIndicator.Top := ScaleY(67);
  DownloadIndicator.Width := Page.SurfaceWidth;
  DownloadIndicator.Height:= ScaleY(20);
  DownloadIndicator.Min := 0;
  DownloadIndicator.Parent := Page.Surface;

  with Page do
  begin
    OnActivate := @OnDownloadDotNetActivate;
    OnShouldSkipPage := @OnDownloadDotNetShouldSkipPage;
    OnNextButtonClick := @OnDownloadDotNetNextButtonClick;
    OnBackButtonClick := @OnDownloadDotNetBackButtonClick;
  end;

  Result := Page.ID;
end;

#endif
