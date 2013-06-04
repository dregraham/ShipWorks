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
#include "Download.iss"
#include "Win32.iss"
#include "ErrorPage.iss"

//----------------------------------------------------------------
// Globals
//----------------------------------------------------------------
var
	DotNetWasInstalled: Boolean;
	DotNetNeedsReboot: Boolean;

//----------------------------------------------------------------
// Returns true if .net is packaged in the install
//----------------------------------------------------------------
function IsDotNetIncluded(): Boolean;
var
   dotNetIncluded: Boolean;
begin

  dotNetIncluded := False;

  Result := dotNetIncluded;
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
			'Install',
			DotNetRegValue))
	then begin

		// If it exists, no need to install
		if (DotNetRegValue = 1) then begin
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
