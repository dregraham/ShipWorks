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
#ifndef DOTNET_ISS
#define DOTNET_ISS

//----------------------------------------------------------------
// Includes
//----------------------------------------------------------------
#include "Win32.iss"
#include "ErrorPage.iss"

//----------------------------------------------------------------
// Globals
//----------------------------------------------------------------
var
	DotNetWasInstalled: Boolean;
	DotNetNeedsReboot: Boolean;

//----------------------------------------------------------------
// Returns "4.0" or "4.5" depending on the OS
//----------------------------------------------------------------
function GetSupportedDotNetVersion(): String;
begin
	Result := '4.6';
end;

//----------------------------------------------------------------
// Returns filename of the .NET installer
//----------------------------------------------------------------
function GetDotNetFileName(): String;
begin
    Result := 'NDP46-KB3045557-x86-x64-AllOS-ENU.exe';
end;


//----------------------------------------------------------------
// Returns the URL to the .NET installer
//----------------------------------------------------------------
function GetDotNetDownloadURL(): String;
begin
	Result := 'https://download.microsoft.com/download/C/3/A/C3A5200B-D33C-47E9-9D70-2F7C65DAAD94/' + GetDotNetFileName();
end;

//----------------------------------------------------------------
// Returns true if we need to install the .NET Framework
//----------------------------------------------------------------
function IsDotNetInstallRequired(): Boolean;
var
	DotNetRegValue: Cardinal;
begin
	// We will set this to fals if we find out otherwise
	Result := True;

	// Try to read the registry to see if this version of .NET is installed
	if (RegQueryDWordValue(
			HKEY_LOCAL_MACHINE,
			'SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full',
			'Release',
			DotNetRegValue))
	then begin

		// Test version of .NET installed
		// https://msdn.microsoft.com/en-us/library/hh925568(v=vs.110).aspx
		if (DotNetRegValue > 393294)
        then begin

        	Result := False;

	    end;
    end;
// For testing.  This lets us simulate what would happen if .NET were required, even if it is not
#if FORCE_DOTNET_REQUIRED
	Result := True;
#endif

end;

// File Gaurds
#endif
