// File Gaurds
#ifndef VCRedist_ISS
#define VCRedist_ISS

//----------------------------------------------------------------
// Includes
//----------------------------------------------------------------
#include "Utilities.iss"

//----------------------------------------------------------------
// Globals
//----------------------------------------------------------------
var
	VCRedistNeedsReboot: Boolean;

//----------------------------------------------------------------
// Returns filename of the VCRedist exe
//----------------------------------------------------------------
function GetVCRedistFileName(): String;
begin
	Result := 'vcredist_' + GetArchName() + '.exe';
end;

//----------------------------------------------------------------
// Returns the URL to the VCRedist exe
//----------------------------------------------------------------
function GetVCRedistDownloadURL(): String;
begin
	Result := 'https://www.interapptive.com/download/components/chromium/VC12/' + GetVCRedistFileName();
end;

//----------------------------------------------------------------
// Returns true if we need to install the VC++ 12 Redist package
//----------------------------------------------------------------
function IsVCRedistInstallRequired(): Boolean;
var
	FileVersion: string;
	Installed: Cardinal;
begin
	// We will set this to fals if we find out otherwise
	Result := True;

	// Try to read the registry to see if this version of .NET is installed
	if (RegQueryDWordValue(
		HKEY_LOCAL_MACHINE,
		'SOFTWARE\Microsoft\VisualStudio\12.0\VC\Runtimes\' + GetArchName(),
		'Installed',
		Installed))
	then begin
		if (Installed = 1)
		then begin
			Result := False;
		end;
	end;
end;

// File Gaurds
#endif
