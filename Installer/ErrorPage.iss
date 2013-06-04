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
#ifndef ERRORPAGE_ISS
#define ERRORPAGE_ISS
	
//----------------------------------------------------------------
// Page is being activated
//----------------------------------------------------------------
procedure OnErrorActivate(Page: TWizardPage);
begin
  WizardForm.BackButton.Visible := False;
  WizardForm.NextButton.Visible := False;
  WizardForm.CancelButton.Caption := 'Close';
end;

//----------------------------------------------------------------
// Suppresses cancel confirmation
//----------------------------------------------------------------
procedure OnErrorCancelButtonClick(Page: TWizardPage; var Cancel, Confirm: Boolean);
begin
  Confirm := False;
end;

//----------------------------------------------------------------
// Help url has been clicked
//----------------------------------------------------------------
procedure OnErrorPageUrlClick(Sender: TObject);
var
  ErrorCode: Integer;
  link: TNewStaticText;
begin
  link := TNewStaticText(Sender);
  ShellExec('open', link.Caption, '', '', SW_SHOWNORMAL, ewNoWait, ErrorCode);
end;

//----------------------------------------------------------------
// Creates the error page
//----------------------------------------------------------------
function ShowErrorPage(PreviousPageId: Integer; Caption: String; Description: String; ErrorMessage: String; HelpURL: String): Integer;
var
  infoLabel: TLabel;
  link: TNewStaticText;
  Page: TWizardPage;
begin
  Page := CreateCustomPage(
    PreviousPageId,
    Caption,
    Description
  );

  infoLabel := TLabel.Create(Page);
  with infoLabel do
  begin
    Parent := Page.Surface;
    Caption := ErrorMessage;
    Left := ScaleX(0);
    Top := ScaleY(0);
    Width := ScaleX(415);
    Height := ScaleY(52);
    WordWrap := True;
  end;
  
  link := TNewStaticText.Create(Page);
  with link do
  begin
    Parent := Page.Surface;
    Caption := HelpURL;
    Cursor := crHand;
    Left := ScaleX(0);
    Top := ScaleY(72);
    Width := ScaleX(15);
    Height := ScaleY(13);
    Font.Color := clBlue;
    Font.Height := ScaleY(-11);
    Font.Style := Font.Style + [fsUnderline];
    OnClick := @OnErrorPageUrlClick;
  end;

  with Page do
  begin
    OnActivate := @OnErrorActivate;
    OnCancelButtonClick := @OnErrorCancelButtonClick;
  end;

  Result := Page.ID;
end;

// File Gaurds
#endif
