// File Gaurds
#ifndef CHROMIUM_ISS
#define CHROMIUM_ISS

//----------------------------------------------------------------
// Includes
//----------------------------------------------------------------
#include "Win32.iss"
#include "ErrorPage.iss"

//----------------------------------------------------------------
// Returns filename of the Chromium zip
//----------------------------------------------------------------
function GetChromiumFileName(): String;
begin
	Result := 'renderer_63_' + GetArchName() + '.zip';
end;

//----------------------------------------------------------------
// Returns destination path of the Chromium binaries
//----------------------------------------------------------------
function GetChromiumDestination(): String;
begin
	Result := ExpandConstant('{app}') + '\' + GetArchName();
end;

//----------------------------------------------------------------
// Returns filename of file to use to test installed Chromium version
//----------------------------------------------------------------
function GetChromiumDestinationTestFile(): String;
begin
	Result := GetChromiumDestination() + '\CefSharp.BrowserSubprocess.exe';
end;

//----------------------------------------------------------------
// Returns the URL to the Chromium zip
//----------------------------------------------------------------
function GetChromiumDownloadURL(): String;
begin
	// Result := 'https://www.interapptive.com/download/components/chromium/' + GetChromiumFileName();
	Result := 'http://devsandbox:8888/' + GetChromiumFileName();
end;

//----------------------------------------------------------------
// Returns true if we need to copy Chromium binaries
//----------------------------------------------------------------
function IsChromiumInstallRequired(): Boolean;
var
	FileVersion: string;
begin
	Result := True;

	if (FileExists(GetChromiumDestinationTestFile()))
	then begin
		if (GetVersionNumbersString(GetChromiumDestinationTestFile(), FileVersion))
		then begin
			Result := not (FileVersion = '63.0.3.0');
		end;
	end;
end;

// File Gaurds
#endif
