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
// Returns "4.0" or "4.5" depending on the OS
//----------------------------------------------------------------
function GetSupportedDotNetVersion(): String;
var
    version: TWindowsVersion;
begin

    GetWindowsVersionEx(version);

    if (
        ((version.Major < 6)) or // Anything less than Vista
        ((version.Major = 6) and (version.Minor = 0) and (version.ServicePackMajor < 2)) or // Vista\Server 2008 need SP2
        ((version.Major = 6) and (version.Minor = 1) and (version.ServicePackMajor < 1))    // Windows 7 \ Server 2008 R2 need SP1
        )
    then begin

		Result := '4.0';

	end
	else
	begin
        
        Result := '4.5';

    end;

end;

//----------------------------------------------------------------
// Returns filename of the .NET installer
//----------------------------------------------------------------
function GetDotNetFileName(): String;
begin

    if (GetSupportedDotNetVersion() = '4.0')
    then begin

		Result := 'dotNetFx40_Full_setup.exe';

	end
	else
	begin
        
        Result := 'dotNetFx45_Full_setup.exe';

    end;

end;

//----------------------------------------------------------------
// Returns the URL to the .NET installer
//----------------------------------------------------------------
function GetDotNetDownloadURL(): String;
begin

    if (GetSupportedDotNetVersion() = '4.0')
    then begin

		Result := 'http://www.interapptive.com/download/components/dotnet40/' + GetDotNetFileName();

	end
	else
	begin
        
        Result := 'http://www.interapptive.com/download/components/dotnet45/' + GetDotNetFileName();

    end;

end;

//----------------------------------------------------------------
// Returns the filesize of the .net installer
//----------------------------------------------------------------
function GetDotNetFileSize(): Integer;
begin

    if (GetSupportedDotNetVersion() = '4.0')
    then begin

		Result := (889) * 1024;

	end
	else
	begin
        
        Result := (1005) * 1024;

    end;

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
		if (DotNetRegValue = 1) 
        then begin

            // Now we need to know if we can install .NET 4.5, or have to stick with .NET 4.0
            if (GetSupportedDotNetVersion() = '4.0')
            then begin

                // .NET 4.5 isn't supported, and .NET 4.0 is installed - so we're good
			    Result := False;

		    end
		    else
		    begin

                // .NET 4.5 is supported - so we require it
                if (RegValueExists(
                	HKEY_LOCAL_MACHINE,
			        'SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full',
			        'Release'))
                then begin

                    // The presense of 'Release' as documented as meaning .net 4.5 or higher is installed
			        Result := False;

                end;
        
            end;
	   end;
    end;
// For testing.  This lets us simulate what would happen if .NET were required, even if it is not
#if FORCE_DOTNET_REQUIRED
	Result := True;
#endif

end;

// File Gaurds
#endif
