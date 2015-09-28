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
#ifndef KB2468871_ISS
#define KB2468871_ISS

//----------------------------------------------------------------
// Includes
//----------------------------------------------------------------
#include "Download.iss"
#include "Win32.iss"
#include "ErrorPage.iss"

//----------------------------------------------------------------
// Globals
//----------------------------------------------------------------
var
	KB2468871WasInstalled: Boolean;

//----------------------------------------------------------------
// Returns filename of the KB2468871 installer
//----------------------------------------------------------------
function GetKB2468871FileName(): String;
begin

	Result := 'NDP40-KB2468871-v2-x86.exe';

end;

//----------------------------------------------------------------
// Returns the URL to the KB2468871 installer
//----------------------------------------------------------------
function GetKB2468871DownloadURL(): String;
begin

	Result := 'http://download.microsoft.com/download/2/B/F/2BF4D7D1-E781-4EE0-9E4F-FDD44A2F8934/' + GetKB2468871FileName();

end;

//----------------------------------------------------------------
// Returns the filesize of the .net installer
//----------------------------------------------------------------
function GetKB2468871FileSize(): Integer;
begin

	Result := (19098) * 1024;

end;

//----------------------------------------------------------------
// Returns true if we need to install the .NET Framework
//----------------------------------------------------------------
function IsKB2468871InstallRequired(): Boolean;
var
	DotNetRegValue: String;
begin
	// We will set this to false if we find out otherwise
	Result := True;

	// Try to read the registry to see if this version of .NET is installed
	if (not (GetSupportedDotNetVersion() = '4.0') or RegValueExists(
			HKEY_LOCAL_MACHINE,
			'SOFTWARE\Classes\Installer\Features\5C1093C35543A0E32A41B090A305076A',
			'KB2468871'))
	then begin

		Result := False;

    end;
// For testing.  This lets us simulate what would happen if .NET were required, even if it is not
#if FORCE_DOTNET_REQUIRED
	Result := True;
#endif

end;

// File Gaurds
#endif
