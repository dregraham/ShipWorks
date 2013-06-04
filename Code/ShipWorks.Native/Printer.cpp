#include <windows.h>

// You are explicitly linking to SetDefaultPrinter because implicitly
// linking on Windows 95/98 or NT4 results in a runtime error.
// This block specifies which text version you explicitly link to.
#define SETDEFAULTPRINTER "SetDefaultPrinterA"
typedef BOOL (*SetDefaultPrinterType)(LPCTSTR);

extern "C" __declspec(dllexport) BOOL PrintSetDefaultPrinter(LPSTR pPrinterName, BOOL notify);
extern "C" __declspec(dllexport) BOOL PrintSetPaperSettings(
    LPSTR pPrinterName, 
    short dmPaperSource,
    short dmSize,
    short dmLength, 
    short dmWidth,
    short dmOrientation,
    short* pOldSize,
    short* pOldLength, 
    short* pOldWidth,
    BOOL notify);

/*-----------------------------------------------------------------*/ 
/* PrintSetDefaultPrinter                                          */ 
/*                                                                 */ 
// Taken from:
//    Microsoft Knowledge Base Article - 246772 
//    HOWTO: Retrieve and Set the Default Printer in Windows
//
/* Parameters:                                                     */ 
/*   pPrinterName: Valid name of existing printer to make default. */ 
/*                                                                 */ 
/* Returns: TRUE for success, FALSE for failure.                   */ 
/*-----------------------------------------------------------------*/ 
extern "C" BOOL PrintSetDefaultPrinter(LPSTR pPrinterName, BOOL notify)
{
  BOOL bFlag;
  OSVERSIONINFO osv;
  DWORD dwNeeded = 0;
  HANDLE hPrinter = NULL;
  PRINTER_INFO_2 *ppi2 = NULL;
  LPTSTR pBuffer = NULL;
  LONG lResult;
  HMODULE hWinSpool = NULL;
  SetDefaultPrinterType fnSetDefaultPrinter;
  
  // What version of Windows are you running?
  osv.dwOSVersionInfoSize = sizeof(OSVERSIONINFO);
  GetVersionEx(&osv);
  
  if (!pPrinterName)
    return FALSE;
  
  // If Windows 95 or 98, use SetPrinter.
  if (osv.dwPlatformId == VER_PLATFORM_WIN32_WINDOWS)
  {
    // Open this printer so you can get information about it.
    bFlag = OpenPrinter(pPrinterName, &hPrinter, NULL);
    if (!bFlag || !hPrinter)
      return FALSE;
    
    // The first GetPrinter() tells you how big our buffer must
    // be to hold ALL of PRINTER_INFO_2. Note that this will
    // typically return FALSE. This only means that the buffer (the 3rd
    // parameter) was not filled in. You do not want it filled in here.
    SetLastError(0);
    bFlag = GetPrinter(hPrinter, 2, 0, 0, &dwNeeded);
    if (!bFlag)
    {
      if ((GetLastError() != ERROR_INSUFFICIENT_BUFFER) || (dwNeeded == 0))
      {
        ClosePrinter(hPrinter);
        return FALSE;
      }
    }
    
    // Allocate enough space for PRINTER_INFO_2.
    ppi2 = (PRINTER_INFO_2 *)GlobalAlloc(GPTR, dwNeeded);
    if (!ppi2)
    {
      ClosePrinter(hPrinter);
      return FALSE;
    }
    
    // The second GetPrinter() will fill in all the current information
    // so that all you have to do is modify what you are interested in.
    bFlag = GetPrinter(hPrinter, 2, (LPBYTE)ppi2, dwNeeded, &dwNeeded);
    if (!bFlag)
    {
      ClosePrinter(hPrinter);
      GlobalFree(ppi2);
      return FALSE;
    }
    
    // Set default printer attribute for this printer.
    ppi2->Attributes |= PRINTER_ATTRIBUTE_DEFAULT;
    bFlag = SetPrinter(hPrinter, 2, (LPBYTE)ppi2, 0);
    if (!bFlag)
    {
      ClosePrinter(hPrinter);
      GlobalFree(ppi2);
      return FALSE;
    }
    
    // Tell all open programs that this change occurred. 
    // Allow each program 1 second to handle this message.
    if (notify)
    {
        lResult = (LONG) SendMessageTimeout(HWND_BROADCAST, WM_SETTINGCHANGE, 0L,
        (LPARAM)(LPCTSTR)"windows", SMTO_NORMAL, 1000, NULL);
    }
  }
  
  // If Windows NT, use the SetDefaultPrinter API for Windows 2000,
  // or WriteProfileString for version 4.0 and earlier.
  else if (osv.dwPlatformId == VER_PLATFORM_WIN32_NT)
  {
    if (osv.dwMajorVersion >= 5) // Windows 2000 or later (use explicit call)
    {
      hWinSpool = LoadLibrary("winspool.drv");
      if (!hWinSpool)
        return FALSE;
      fnSetDefaultPrinter = (SetDefaultPrinterType) GetProcAddress(hWinSpool, SETDEFAULTPRINTER);
      if (!fnSetDefaultPrinter)
      {
        FreeLibrary(hWinSpool);
        return FALSE;
      }

      //MessageBox(NULL, "Calling SetDefaultPrinter with: ", "hi", 0);
      //MessageBox(NULL, pPrinterName, " ", 0);
      bFlag = fnSetDefaultPrinter(pPrinterName);
      FreeLibrary(hWinSpool);
      if (!bFlag)
        return FALSE;
    }

    else // NT4.0 or earlier
    {
      // Open this printer so you can get information about it.
      bFlag = OpenPrinter(pPrinterName, &hPrinter, NULL);
      if (!bFlag || !hPrinter)
        return FALSE;
      
      // The first GetPrinter() tells you how big our buffer must
      // be to hold ALL of PRINTER_INFO_2. Note that this will
      // typically return FALSE. This only means that the buffer (the 3rd
      // parameter) was not filled in. You do not want it filled in here.
      SetLastError(0);
      bFlag = GetPrinter(hPrinter, 2, 0, 0, &dwNeeded);
      if (!bFlag)
      {
        if ((GetLastError() != ERROR_INSUFFICIENT_BUFFER) || (dwNeeded == 0))
        {
          ClosePrinter(hPrinter);
          return FALSE;
        }
      }
      
      // Allocate enough space for PRINTER_INFO_2.
      ppi2 = (PRINTER_INFO_2 *)GlobalAlloc(GPTR, dwNeeded);
      if (!ppi2)
      {
        ClosePrinter(hPrinter);
        return FALSE;
      }
      
      // The second GetPrinter() fills in all the current<BR/>
      // information.
      bFlag = GetPrinter(hPrinter, 2, (LPBYTE)ppi2, dwNeeded, &dwNeeded);
      if ((!bFlag) || (!ppi2->pDriverName) || (!ppi2->pPortName))
      {
        ClosePrinter(hPrinter);
        GlobalFree(ppi2);
        return FALSE;
      }
      
      // Allocate buffer big enough for concatenated string.
      // String will be in form "printername,drivername,portname".
      pBuffer = (LPTSTR)GlobalAlloc(GPTR,
        lstrlen(pPrinterName) +
        lstrlen(ppi2->pDriverName) +
        lstrlen(ppi2->pPortName) + 3);
      if (!pBuffer)
      {
        ClosePrinter(hPrinter);
        GlobalFree(ppi2);
        return FALSE;
      }
      
      // Build string in form "printername,drivername,portname".
      lstrcpy(pBuffer, pPrinterName);  lstrcat(pBuffer, ",");
      lstrcat(pBuffer, ppi2->pDriverName);  lstrcat(pBuffer, ",");
      lstrcat(pBuffer, ppi2->pPortName);
      
      // Set the default printer in Win.ini and registry.
      bFlag = WriteProfileString("windows", "device", pBuffer);
      if (!bFlag)
      {
        ClosePrinter(hPrinter);
        GlobalFree(ppi2);
        GlobalFree(pBuffer);
        return FALSE;
      }
    }
    
    // Tell all open programs that this change occurred. 
    // Allow each app 1 second to handle this message.
    if (notify)
    {
        lResult = (LONG) SendMessageTimeout(HWND_BROADCAST, WM_SETTINGCHANGE, 0L, 0L,
        SMTO_NORMAL, 1000, NULL);
    }
  }
  
  // Clean up.
  if (hPrinter)
    ClosePrinter(hPrinter);
  if (ppi2)
    GlobalFree(ppi2);
  if (pBuffer)
    GlobalFree(pBuffer);
  
  return TRUE;
}

/*-----------------------------------------------------------------*/ 
// Taken from:
//      Microsoft Knowledge Base Article - 140285
//      HOWTO: Modify Printer Settings by Using SetPrinter
// 
/*-----------------------------------------------------------------*/ 
extern "C" BOOL PrintSetPaperSettings(
    LPSTR pPrinterName, 
    short dmPaperSource,
    short dmSize,
    short dmLength, 
    short dmWidth,
    short dmOrientation,
    short* pOldSize,
    short* pOldLength, 
    short* pOldWidth,
    BOOL notify)
{
	HANDLE hPrinter = NULL;
	DWORD dwNeeded = 0;
	PRINTER_INFO_2 *pi2 = NULL;
	DEVMODE *pDevMode = NULL;
	PRINTER_DEFAULTS pd;
	BOOL bFlag;
	LONG lFlag;

	// Open printer handle (on Windows NT, you need full-access because you
	// will eventually use SetPrinter)...
	ZeroMemory(&pd, sizeof(pd));
	pd.DesiredAccess = PRINTER_ACCESS_USE;
	bFlag = OpenPrinter(pPrinterName, &hPrinter, &pd);
	if (!bFlag || (hPrinter == NULL))
		return FALSE;

	// The first GetPrinter tells you how big the buffer should be in 
	// order to hold all of PRINTER_INFO_2. Note that this should fail with 
	// ERROR_INSUFFICIENT_BUFFER.  If GetPrinter fails for any other reason 
	// or dwNeeded isn't set for some reason, then there is a problem...
	SetLastError(0);
	bFlag = GetPrinter(hPrinter, 2, 0, 0, &dwNeeded);
         if ((!bFlag) && (GetLastError() != ERROR_INSUFFICIENT_BUFFER) || 
         (dwNeeded == 0))
	{
		ClosePrinter(hPrinter);
		return FALSE;
	}

	// Allocate enough space for PRINTER_INFO_2...
	pi2 = (PRINTER_INFO_2 *)GlobalAlloc(GPTR, dwNeeded);
	if (pi2 == NULL)
	{
		ClosePrinter(hPrinter);
		return FALSE;
	}

	// The second GetPrinter fills in all the current settings, so all you
	// need to do is modify what you're interested in...
	bFlag = GetPrinter(hPrinter, 2, (LPBYTE)pi2, dwNeeded, &dwNeeded);
	if (!bFlag)
	{
		GlobalFree(pi2);
		ClosePrinter(hPrinter);
		return FALSE;
	}

	// If GetPrinter didn't fill in the DEVMODE, try to get it by calling
	// DocumentProperties...
	if (pi2->pDevMode == NULL)
	{
		dwNeeded = DocumentProperties(NULL, hPrinter,
						pPrinterName,
						NULL, NULL, 0);
		if (dwNeeded <= 0)
		{
			GlobalFree(pi2);
			ClosePrinter(hPrinter);
			return FALSE;
		}

		pDevMode = (DEVMODE *)GlobalAlloc(GPTR, dwNeeded);
		if (pDevMode == NULL)
		{
			GlobalFree(pi2);
			ClosePrinter(hPrinter);
			return FALSE;
		}

		lFlag = DocumentProperties(NULL, hPrinter,
						pPrinterName,
						pDevMode, NULL,
						DM_OUT_BUFFER);
		if (lFlag != IDOK || pDevMode == NULL)
		{
			GlobalFree(pDevMode);
			GlobalFree(pi2);
			ClosePrinter(hPrinter);
			return FALSE;
		}

		pi2->pDevMode = pDevMode;
	}

    // We will be changing all of these...
    UINT changeFields = 
        DM_PAPERSIZE | DM_PAPERLENGTH | DM_PAPERWIDTH | DM_ORIENTATION | DM_DEFAULTSOURCE;

	// Driver is reporting that it doesn't support this change...
	if (!(pi2->pDevMode->dmFields & changeFields ))
	{
		GlobalFree(pi2);
		ClosePrinter(hPrinter);
		if (pDevMode)
			GlobalFree(pDevMode);
		return FALSE;
	}

    // Return what the used to be
    *pOldSize = pi2->pDevMode->dmPaperSize;
    *pOldLength = pi2->pDevMode->dmPaperLength;
    *pOldWidth = pi2->pDevMode->dmPaperWidth;

	// Specify exactly what we are attempting to change...
	pi2->pDevMode->dmFields = changeFields;
    pi2->pDevMode->dmDefaultSource = dmPaperSource;
    pi2->pDevMode->dmPaperSize = dmSize;
    pi2->pDevMode->dmPaperLength = dmLength;
    pi2->pDevMode->dmPaperWidth = dmWidth;
    pi2->pDevMode->dmOrientation = dmOrientation;

	// Do not attempt to set security descriptor...
	pi2->pSecurityDescriptor = NULL;

	// Make sure the driver-dependent part of devmode is updated...
	lFlag = DocumentProperties(NULL, hPrinter,
		  pPrinterName,
		  pi2->pDevMode, pi2->pDevMode,
		  DM_IN_BUFFER | DM_OUT_BUFFER);
	if (lFlag != IDOK)
	{
		GlobalFree(pi2);
		ClosePrinter(hPrinter);
		if (pDevMode)
			GlobalFree(pDevMode);
		return FALSE;
	}

	PRINTER_INFO_9 pi9;
	pi9.pDevMode = pi2->pDevMode;

	// Update printer information...
	bFlag = SetPrinter(hPrinter, 9, (LPBYTE)&pi9, 0);
	if (!bFlag)
	// The driver doesn't support, or it is unable to make the change...
	{
		GlobalFree(pi2);
		ClosePrinter(hPrinter);
		if (pDevMode)
			GlobalFree(pDevMode);
		return FALSE;
	}

	// Tell other apps that there was a change...
    if (notify)
    {
	    SendMessageTimeout(HWND_BROADCAST, WM_DEVMODECHANGE, 0L,
			    (LPARAM)(LPCSTR)pPrinterName,
			    SMTO_NORMAL, 1000, NULL);
    }

	// Clean up...
	if (pi2)
		GlobalFree(pi2);
	if (hPrinter)
		ClosePrinter(hPrinter);
	if (pDevMode)
		GlobalFree(pDevMode);

	return TRUE;
}