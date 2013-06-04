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
#ifndef WIN32_ISS
#define WIN32_ISS

//----------------------------------------------------------------
// Windows Commands
//----------------------------------------------------------------
const
	SC_CLOSE  	= $F060;

	MF_BYCOMMAND  = $0000;
	MF_BYPOSITION = $0400;
	MF_ENABLED    = $0000;
	MF_GRAYED     = $0001;
	MF_DISABLED   = $0002;

//----------------------------------------------------------------
// User32: PostQuitMessage
//----------------------------------------------------------------
procedure PostQuitMessage;
external 'PostQuitMessage@user32.dll stdcall';

//----------------------------------------------------------------
// User32: ShowWindow
//----------------------------------------------------------------
function ShowWindow(hWnd, nCmdShow: Integer): Integer;
external 'ShowWindow@user32.dll stdcall';

//----------------------------------------------------------------
// User32: Menu Functions
//----------------------------------------------------------------
function GetSystemMenu(hWnd: Integer; bRevert: Boolean): Integer;
external 'GetSystemMenu@User32.dll stdcall';

function EnableMenuItem(hMenu: Integer; uIDEnableItem: Integer; uEnable: Integer): Boolean;
external 'EnableMenuItem@User32.dll stdcall';

function DeleteMenu(hMenu: Integer; uIDEnableItem: Integer; uEnable: Integer): Boolean;
external 'DeleteMenu@User32.dll stdcall';

// File Gaurds
#endif
