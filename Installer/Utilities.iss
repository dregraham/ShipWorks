// File Gaurds
#ifndef UTILITIES_ISS
#define UTILITIES_ISS

const
  SHCONTCH_NOPROGRESSBOX = 4;
  SHCONTCH_RESPONDYESTOALL = 16;

//----------------------------------------------------------------
// Returns name of the architecture
//----------------------------------------------------------------
function GetArchName(): String;
begin
  Result := 'x86';

  if (Is64BitInstallMode) then Result := 'x64';
end;

//----------------------------------------------------------------
// Unzip the specified file into the target path
//----------------------------------------------------------------
procedure UnZip(ZipPath, TargetPath: string);
var
  Shell: Variant;
  ZipFile: Variant;
  TargetFolder: Variant;
begin
  Shell := CreateOleObject('Shell.Application');

  ZipFile := Shell.NameSpace(ZipPath);
  if VarIsEmpty(ZipFile) then
    RaiseException(Format('ZIP file "%s" does not exist or cannot be opened', [ZipPath]));

  TargetFolder := Shell.NameSpace(TargetPath);
  if VarIsEmpty(TargetFolder) then
    RaiseException(Format('Target path "%s" does not exist', [TargetPath]));

  TargetFolder.CopyHere(ZipFile.Items, SHCONTCH_NOPROGRESSBOX or SHCONTCH_RESPONDYESTOALL);
end;

// File Gaurds
#endif