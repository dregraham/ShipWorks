// File Gaurds
#ifndef VCREDISTDOWNLOAD_ISS
#define VCREDISTDOWNLOAD_ISS

[Code]
#define DwinsHs_Data_Buffer_Length 16384

#include "dwinshs.iss"
#include "VCRedist.iss"
#include "Utilities.iss"

var
  VS_DownloadIndicator: TNewProgressBar;
  VS_BackClicked: Boolean;
  VS_SizeLabel: TLabel;
  VS_Megabyte: extended;

//----------------------------------------------------------------
// Called when each block of the download is read
//----------------------------------------------------------------
function VS_OnRead(URL, Agent: String; Method: TReadMethod; Index, TotalSize, ReadSize,
  CurrentSize: {#BIG_INT}; var ReadStr: String): Boolean;
begin
  if Index = 0 then VS_DownloadIndicator.Max := TotalSize;
  VS_DownloadIndicator.Position := ReadSize; // Update the download progress indicator
  VS_SizeLabel.Caption := Format('%3.1f of %3.1f MB', [ReadSize / VS_Megabyte, TotalSize / VS_Megabyte]);
  Result := not VS_BackClicked; // Determine whether download was cancelled
end;

//----------------------------------------------------------------
// Download the .NET installer
//----------------------------------------------------------------
function DownloadVCRedist(CurPageID: Integer): Boolean;
var
  Response: String;
  Size: {#BIG_INT};
  DownloadResult: Integer;
  DownloadSuccessful: Boolean;
  DownloadCanceled: Boolean;
  DownloadFileName: String;
  returnCode: Integer;
  execSuccess: Boolean;
  failReason: String;
  installFile: String;
begin
    DownloadFileName := ExpandConstant('{tmp}') + '\' + GetVCRedistFileName();

    // Disbale to continue before download completes
    WizardForm.NextButton.Enabled := False;

    DownloadResult := DwinsHs_ReadRemoteURL(GetVCRedistDownloadURL(), 'ShipWorks_Installer', rmGet,
      Response, Size, DownloadFileName, @VS_OnRead);

    DownloadSuccessful := DownloadResult = READ_OK;
    DownloadCanceled := DownloadResult = READ_ERROR_CANCELED;

    if (not DownloadSuccessful) then
    	DeleteFile(DownloadFileName);

    if (DownloadSuccessful)
    then begin
      DownloadSuccessful := False;

      execSuccess := ShellExec('open', DownloadFileName,
        '/norestart /quiet', '', SW_HIDE, ewWaitUntilTerminated, returnCode);

      // Assume failure, until we found out otherwise
      failReason := 'A problem was encountered while installing additional Microsoft run-time components.' + #13 + #13 + 'Error: ';

      // The application was executed (doesnt mean it installed ok)
      if (execSuccess) then
      begin

        // If it is installed OK
        if ((returnCode = 0   ) or  // Success
            (returnCode = 1614) or  // Success, need reboot
            (returnCode = 3010))  // Success, need reboot
        then begin

          DownloadSuccessful := True;
          VCRedistNeedsReboot := (returnCode = 3010) or (returnCode = 1614);

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
    end else begin
      failReason := 'Could not download additional Microsoft run-time components.  Please check that you have an open ' +
          'connection to the internet, and run Setup again.  Error: ' + IntToStr(DownloadResult);
    end;

    // If we didnt get it, show an error
    if (not (DownloadSuccessful or DownloadCanceled)) then
        ShowErrorPage(
            CurPageID,
            'Setup Error',
            'Could not download additional Microsoft run-time components.',
            failReason + #13 + #13 + 'You can download it directly from the following link.',
            GetVCRedistDownloadURL());

    WizardForm.NextButton.Enabled := True;
    Result := not DownloadCanceled;
end;

//----------------------------------------------------------------
// Called when the back button is clicked
//----------------------------------------------------------------
function OnDownloadVCRedistBackButtonClick(Page: TWizardPage): Boolean;
begin
  // Stop to download
  VS_BackClicked := True;
  Result := True;
  WizardForm.NextButton.Enabled := True;
end;

//----------------------------------------------------------------
// Called to start downloading .net
//----------------------------------------------------------------
function OnDownloadVCRedistNextButtonClick(Page: TWizardPage): Boolean;
begin
  Result := DownloadVCRedist(Page.ID);
end;

//----------------------------------------------------------------
// Called to see if we should skip downloading .NET
//----------------------------------------------------------------
function OnDownloadVCRedistShouldSkipPage(Page: TWizardPage): Boolean;
begin
  Result := not (IsVCRedistInstallRequired() and not FileExists(ExpandConstant('{tmp}\' + GetVCRedistFileName())));
end;

//----------------------------------------------------------------
// Called when the wizard page becomes active
//----------------------------------------------------------------
procedure OnDownloadVCRedistActivate(Page: TWizardPage);
begin
    // Allow to download
    DwinsHs_CancelDownload := cdNone;
    VS_BackClicked := False;
    VS_DownloadIndicator.Position := 0;
    VS_SizeLabel.Caption := '';
end;

//----------------------------------------------------------------
// Creates the custom .net download page
//----------------------------------------------------------------
function CreateVCRedistDownloadPage(PreviousPageId: Integer): Integer;
var
  infoLabel: TLabel;
  Page: TWizardPage;
begin
  VS_Megabyte := 1024.0 * 1024.0;

  Page := CreateCustomPage(
    PreviousPageId,
    'Install additional Microsoft run-time components',
    'ShipWorks requires additional Microsoft run-time components'
  );

  infoLabel := TLabel.Create(Page);
  with infoLabel do
  begin
    Parent := Page.Surface;
    Caption :=
      'ShipWorks requires additional Microsoft run-time components, which is not installed on your computer.' + #13 +
      '' + #13 +
      'Click Next to download it now (15 MB).';
    Left := ScaleX(0);
    Top := ScaleY(0);
    Width := ScaleX(415);
    Height := ScaleY(52);
    WordWrap := True;
  end;

  VS_SizeLabel := TLabel.Create(Page);
  with VS_SizeLabel do
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
  VS_DownloadIndicator := TNewProgressBar.Create(Page);
  VS_DownloadIndicator.Left := ScaleX(0);
  VS_DownloadIndicator.Top := ScaleY(67);
  VS_DownloadIndicator.Width := Page.SurfaceWidth;
  VS_DownloadIndicator.Height:= ScaleY(20);
  VS_DownloadIndicator.Min := 0;
  VS_DownloadIndicator.Parent := Page.Surface;

  with Page do
  begin
    OnActivate := @OnDownloadVCRedistActivate;
    OnShouldSkipPage := @OnDownloadVCRedistShouldSkipPage;
    OnNextButtonClick := @OnDownloadVCRedistNextButtonClick;
    OnBackButtonClick := @OnDownloadVCRedistBackButtonClick;
  end;

  Result := Page.ID;
end;

#endif
