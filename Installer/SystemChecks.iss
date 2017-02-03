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
#ifndef SYSTEMCHECKS_ISS
#define SYSTEMCHECKS_ISS

//----------------------------------------------------------------
// Includes
//----------------------------------------------------------------
#include "ErrorPage.iss"

//----------------------------------------------------------------
// Checks to see if the installer is running under the correct version
//----------------------------------------------------------------
procedure CheckForWinVersion(CurPage: Integer);
var
  version: TWindowsVersion;
begin
    GetWindowsVersionEx(version);

	if (((not UsingWinNT) or (version.Major < 6)) or
		((version.Major = 6) and (version.Minor = 0)) ) then
		ShowErrorPage(
		    CurPage,
		    'Setup Error',
		    'Unsupported Windows Version.',
		    'ShipWorks can only be installed on a computer running Windows 7 or later.',
            '');

end;

//----------------------------------------------------------------
// Checks to see if the current user has administrative rights
//----------------------------------------------------------------
procedure CheckForAdmin(CurPage: Integer);
begin
	// Need to have admin rights to install the stuff
	if (not IsAdminLoggedOn {#if FORCE_NEED_ADMIN}or True{#endif}) then
		ShowErrorPage(
		    CurPage,
		    'Setup Error',
		    'Administrator privileges are required.',
		    'You must be logged on as an administrator to install ShipWorks.' + #13 +
                '' + #13 +
                'Logon to Windows using an administrator account and run setup again.',
            '');
end;

//----------------------------------------------------------------
// Verfies that IE 6.0 or greater is installed (for .NET and Print Templates)
//----------------------------------------------------------------
procedure CheckForIEVersion(CurPage: Integer);
var
	VersionMS: Cardinal;
	VersionLS: Cardinal;
	VersionMSHigh: Integer;
	VersionMSLow: Integer;
	IEVersionOK: Boolean;
begin
	// Assume fals until we find otherwise
	IEVersionOK := False;

	// Check for IE 6.0 and above
	if (GetVersionNumbers(GetSystemDir() + '\wininet.dll', VersionMS, VersionLS))
	then begin
	    VersionMSHigh := (VersionMS shr 16) and $ffff;
	    VersionMSLow := VersionMS and $ffff;

		// Anything 6 and above is ok
		if (VersionMSHigh >= 6) then
			IEVersionOK := True
	end;

	// If IE is not what we need, show the message
	if (not IEVersionOK {#if FORCE_NEED_IE}or True{#endif}) then
		ShowErrorPage(
		    CurPage,
		    'Setup Error',
		    'Internet Explorer is out of date.',
			'ShipWorks requires Internet Explorer 6.0 or higher.  ' +
			    'You currently have Internet Explorer ' + IntToStr(VersionMSHigh) + '.' + IntToStr(VersionMSLow) + '.' + #13 + #13 +
			    'Install the lastest version of Internet Explorer from the following address and run setup again.',
            'http://www.microsoft.com/ie');

end;

// File Gaurds
#endif
