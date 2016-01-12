using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices.ComTypes;
using Microsoft.Win32.SafeHandles;

namespace Interapptive.Shared.Win32
{
    /// <summary>
    /// Contains Win32 Interop wrappers
    /// </summary>
    [NDependIgnore]
    public static class NativeMethods
    {
        #region Methods

        [DllImport("gdi32.dll")]
        public static extern int CombineRgn(IntPtr hrgnDest, IntPtr hrgnSrc1, IntPtr hrgnSrc2, int fnCombineMode);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect);

        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        [DllImport("gdi32.dll")]
        public static extern bool RestoreDC(IntPtr hdc, int nSavedDC);

        [DllImport("gdi32.dll")]
        public static extern int SaveDC(IntPtr hdc);

        [DllImport("kernel32.dll")]
        public static extern int GetCurrentThreadId();

        /// <summary>
        /// We know this function will always exist b\c we only runs on operating systems that support it.
        /// </summary>
        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWow64Process(
             [In] IntPtr hProcess,
             [Out] out bool wow64Process);

        [DllImport("shlwapi.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool PathCompactPath(IntPtr hDC, [In, Out] StringBuilder pszPath, int dx);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool AnimateWindow(IntPtr hwnd, int time, int flags);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool BlockInput(bool fBlockIt);
        
        [DllImport("user32.dll")]
        public static extern IntPtr DispatchMessage(ref MSG msg);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnableWindow(IntPtr hWnd, [MarshalAs(UnmanagedType.Bool)] bool bEnable);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int EnumChildWindows(IntPtr hWndParent, EnumWindowsCallback pCallback, ref object lParam);

        [DllImport("user32.dll", SetLastError=true)]
        public static extern int EnumWindows(EnumWindowsCallback pCallback, ref object lParam);

        public delegate bool EnumWindowsCallback(IntPtr hWnd, ref object lParam);

        [DllImport("user32.dll")]
        public static extern IntPtr GetActiveWindow();

        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(Keys key);

        [DllImport("user32.dll")]
        public static extern bool GetClientRect(IntPtr hWnd, out RECT rect);

        /// <summary>
        /// - Win32 GDI objects (pens, brushes, fonts, palettes, regions, device contexts, bitmap headers)
        /// - Win32 USER objects:
        ///      - WIN32 resources (accelerator tables, bitmap resources, dialog box templates, font resources, menu resources, raw data resources, string table entries, message table entries, cursors/icons)
        /// - Other USER objects (windows, menus)
        /// </summary>
        [DllImport("user32.dll")]
        public static extern int GetGuiResources(IntPtr hProcess, int uiFlags);

        [DllImport("user32.dll")]
        public static extern int GetMessage(ref MSG msg, IntPtr hWnd, int wMsgFilterMin, int wMsgFilterMax);

        [DllImport("user32.dll")]
        public static extern bool GetUpdateRect(IntPtr hWnd, ref RECT lpRect, bool bErase);

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern long GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT rect);

        [DllImport("user32.dll")]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll")]
        public static extern int GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsChild(IntPtr hWndParent, IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsDialogMessage(IntPtr hDlg, ref MSG lpMsg);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsIconic(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWindowEnabled(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern int MapWindowPoints(IntPtr hWndFrom, IntPtr hWndTo, ref NativeMethods.POINT pt, int cPoints);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool PeekMessage(ref MSG msg, IntPtr hWnd, int wMsgFilterMin, int wMsgFilterMax, int wRemoveMsg);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool PostMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool RedrawWindow(IntPtr hWnd, IntPtr lprcUpdate, IntPtr hrgnUpdate, int flags); 

        [DllImport("user32.dll")]
        public static extern int RegisterWindowMessage(string sString);

        [DllImport("user32.dll")]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessageTimeout(
            IntPtr hWnd,
            int msg,
            IntPtr wParam,
            IntPtr lParam,
            int nFlags,
            int nTimeout,
            ref IntPtr pResult);

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, ref RECT lpRect);

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, ref POINT lpPoint);  

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, long dwNewLong);

        [DllImport("user32.dll")]
        public static extern bool SetWindowText(IntPtr hWnd, string lpString);

        [DllImport("gdi32.dll")]
        public static extern bool SetViewportOrgEx(IntPtr hdc, int X, int Y, ref POINT point);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool SetWindowPos(IntPtr hwnd, IntPtr hwnd2, int x, int y, int cx, int cy, int uFlags);

        [DllImport("user32.dll")]
        public static extern int ShowWindow(IntPtr hWnd, int cmdShow);

        [DllImport("user32.dll")]
        public static extern int TranslateMessage(ref MSG msg);

        [DllImport("user32.dll")]
        public static extern IntPtr WindowFromPoint(POINT pt);

        [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError=true)]
        public static extern int WritePrivateProfileString(string section, string key, string value, string filePath);

        [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern int GetPrivateProfileString(string section, string key, string defaultValue, StringBuilder returnValue, int size, string filePath);

        [DllImport("kernel32.dll")]
        public static extern int GetErrorMode();

        [DllImport("kernel32.dll")]
        public static extern int SetErrorMode(int uMode);

        [DllImport("kernel32.dll")]
        public static extern bool AttachConsole(int dwProcessId);

        [DllImport("kernel32.dll")]
        public static extern bool GetFileInformationByHandle(SafeFileHandle hFile, out BY_HANDLE_FILE_INFORMATION lpFileInformation);

        [DllImport("kernel32.dll")]
        public static extern SafeFileHandle GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll")]
        public static extern bool SetStdHandle(int nStdHandle, SafeFileHandle hHandle);

        [DllImport("kernel32.dll")]
        public static extern bool DuplicateHandle(IntPtr hSourceProcessHandle, SafeFileHandle hSourceHandle, IntPtr hTargetProcessHandle, out SafeFileHandle lpTargetHandle, int dwDesiredAccess, Boolean bInheritHandle, int dwOptions);

        [DllImport("Kernel32.dll")]
        public static extern bool FreeConsole();

        [DllImport("Kernel32.dll")]
        public static extern bool AllocConsole();

        [DllImport("advapi32.dll", EntryPoint = "ChangeServiceConfig2")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ChangeServiceFailureActions(IntPtr hService, int dwInfoLevel, [MarshalAs(UnmanagedType.Struct)] ref ServiceFailureActions lpInfo);

        [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern Boolean ChangeServiceConfig(IntPtr hService, int nServiceType, int nStartType, int nErrorControl, String lpBinaryPathName, String lpLoadOrderGroup, IntPtr lpdwTagId, [In] char[] lpDependencies, String lpServiceStartName, String lpPassword, String lpDisplayName);

        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr OpenService(IntPtr hSCManager, [MarshalAs(UnmanagedType.LPWStr)] string lpServiceName, int dwDesiredAccess);

        [DllImport("advapi32.dll", EntryPoint = "OpenSCManagerW", ExactSpelling = true, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern IntPtr OpenSCManager(string machineName, string databaseName, int dwAccess);

        [DllImport("advapi32.dll", PreserveSig = true)]
        internal static extern Int32 LsaOpenPolicy(ref LsaUnicodeString SystemName, ref LsaObjectAttributes ObjectAttributes, Int32 DesiredAccess, out IntPtr PolicyHandle);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true, PreserveSig = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool LookupAccountName([MarshalAs(UnmanagedType.LPTStr)] string lpSystemName, [MarshalAs(UnmanagedType.LPTStr)] string lpAccountName, IntPtr psid, ref int cbsid, StringBuilder domainName, ref int cbdomainLength, ref int use);

        [DllImport("advapi32.dll", SetLastError = true, PreserveSig = true)]
        [CLSCompliant(false)]
        public static extern Int32 LsaAddAccountRights(IntPtr PolicyHandle, IntPtr AccountSid, LsaUnicodeString[] UserRights, int CountOfRights);

        [DllImport("advapi32")]
        public static extern IntPtr FreeSid(IntPtr pSid);

        [DllImport("advapi32.dll")]
        public static extern Int32 LsaClose(IntPtr ObjectHandle);

        [DllImport("advapi32.dll")]
        public static extern Int32 LsaNtStatusToWinError(Int32 status);

        #endregion

        #region Constants


        public const int AW_HOR_POSITIVE = 0x00000001;
        public const int AW_HOR_NEGATIVE = 0x00000002;
        public const int AW_VER_POSITIVE = 0x00000004;
        public const int AW_VER_NEGATIVE = 0x00000008;
        public const int AW_CENTER = 0x00000010;
        public const int AW_HIDE = 0x00010000;
        public const int AW_ACTIVATE = 0x00020000;
        public const int AW_SLIDE = 0x00040000;
        public const int AW_BLEND = 0x00080000;

        public const int BCM_SETSHIELD = 0x0000160C;

        public const int CB_SETCURSEL = 0x014E;

        public const int CDDS_PREPAINT = 0x00000001;
        public const int CDDS_POSTPAINT = 0x00000002;
        public const int CDDS_PREERASE = 0x00000003;
        public const int CDDS_POSTERASE = 0x00000004;
        public const int CDDS_ITEM = 0x00010000;
        public const int CDDS_ITEMPREPAINT = (CDDS_ITEM | CDDS_PREPAINT);
        public const int CDDS_ITEMPOSTPAINT = (CDDS_ITEM | CDDS_POSTPAINT);
        public const int CDDS_ITEMPREERASE = (CDDS_ITEM | CDDS_PREERASE);
        public const int CDDS_ITEMPOSTERASE = (CDDS_ITEM | CDDS_POSTERASE);
        public const int CDDS_SUBITEM = 0x00020000;

        public const int CDRF_DODEFAULT         = 0x00000000;
        public const int CDRF_NEWFONT           = 0x00000002;
        public const int CDRF_SKIPDEFAULT       = 0x00000004;
        public const int CDRF_DOERASE           = 0x00000008;
        public const int CDRF_NOTIFYPOSTPAINT   = 0x00000010;
        public const int CDRF_NOTIFYITEMDRAW    = 0x00000020;
        public const int CDRF_NOTIFYSUBITEMDRAW = 0x00000020;
        public const int CDRF_NOTIFYPOSTERASE   = 0x00000040;

        public const int DLGC_WANTARROWS = 0x0001;
        public const int DLGC_WANTTAB = 0x0002;
        public const int DLGC_WANTALLKEYS = 0x0004;
        public const int DLGC_WANTMESSAGE = 0x0004;
        public const int DLGC_HASSETSEL = 0x0008;
        public const int DLGC_DEFPUSHBUTTON = 0x0010; 
        public const int DLGC_UNDEFPUSHBUTTON = 0x0020;
        public const int DLGC_RADIOBUTTON = 0x0040;
        public const int DLGC_WANTCHARS = 0x0080; 
        public const int DLGC_STATIC = 0x0100;
        public const int DLGC_BUTTON = 0x2000;

        public const int EM_GETSCROLLPOS = (WM_USER + 221);
        public const int EM_SETSCROLLPOS = (WM_USER + 222);

        public const int GR_GDIOBJECTS = 0;
        public const int GR_USEROBJECTS = 1;

        public const int GWL_WNDPROC         = (-4);
        public const int GWL_HINSTANCE       = (-6);
        public const int GWL_HWNDPARENT      = (-8);
        public const int GWL_STYLE           = (-16);
        public const int GWL_EXSTYLE         = (-20);
        public const int GWL_USERDATA        = (-21);
        public const int GWL_ID              = (-12);

        public const int HTTRANSPARENT = (-1);
        public const int HTNOWHERE = 0;
        public const int HTCLIENT = 1;
        public const int HTCAPTION = 2;
        public const int HTSYSMENU = 3;
        public const int HTGROWBOX = 4;
        public const int HTSIZE = HTGROWBOX;
        public const int HTMENU = 5;
        public const int HTHSCROLL = 6;
        public const int HTVSCROLL = 7;
        public const int HTMINBUTTON = 8;
        public const int HTMAXBUTTON = 9;
        public const int HTLEFT = 10;
        public const int HTRIGHT = 11;
        public const int HTTOP = 12;
        public const int HTTOPLEFT = 13;
        public const int HTTOPRIGHT = 14;
        public const int HTBOTTOM = 15;
        public const int HTBOTTOMLEFT = 16;
        public const int HTBOTTOMRIGHT = 17;
        public const int HTBORDER = 18;
        public const int HTREDUCE = HTMINBUTTON;
        public const int HTZOOM = HTMAXBUTTON;
        public const int HTSIZEFIRST = HTLEFT;
        public const int HTSIZELAST = HTBOTTOMRIGHT;
        public const int HTOBJECT = 19;
        public const int HTCLOSE = 20;
        public const int HTHELP = 21;

        public const int MA_ACTIVATE = 1;
        public const int MA_ACTIVATEANDEAT = 2;
        public const int MA_NOACTIVATE = 3;
        public const int MA_NOACTIVATEANDEAT = 4;

        public const int NM_FIRST = unchecked(0 - 0);
        public const int NM_LAST = unchecked(0 - 99);

        public const int NM_OUTOFMEMORY = (NM_FIRST - 1);
        public const int NM_CLICK = (NM_FIRST - 2);    // uses NMCLICK struct
        public const int NM_DBLCLK = (NM_FIRST - 3);
        public const int NM_RETURN = (NM_FIRST - 4);
        public const int NM_RCLICK = (NM_FIRST - 5);    // uses NMCLICK struct
        public const int NM_RDBLCLK = (NM_FIRST - 6);
        public const int NM_SETFOCUS = (NM_FIRST - 7);
        public const int NM_KILLFOCUS = (NM_FIRST - 8);
        public const int NM_CUSTOMDRAW = (NM_FIRST - 12);
        public const int NM_HOVER = (NM_FIRST - 13);
        public const int NM_NCHITTEST = (NM_FIRST - 14);   // uses NMMOUSE struct
        public const int NM_KEYDOWN = (NM_FIRST - 15);   // uses NMKEY struct
        public const int NM_RELEASEDCAPTURE = (NM_FIRST - 16);
        public const int NM_SETCURSOR = (NM_FIRST - 17);   // uses NMMOUSE struct
        public const int NM_CHAR = (NM_FIRST - 18);   // uses NMCHAR struct
        public const int NM_TOOLTIPSCREATED = (NM_FIRST - 19);   // notify of when the tooltips window is create
        public const int NM_LDOWN = (NM_FIRST - 20);
        public const int NM_RDOWN = (NM_FIRST - 21);
        public const int NM_THEMECHANGED = (NM_FIRST - 22);

        public const int PM_NOREMOVE = 0x0000;
        public const int PM_REMOVE = 0x0001;
        public const int PM_NOYIELD = 0x0002;

		public const int PRF_CHECKVISIBLE = 1;
		public const int PRF_NONCLIENT = 2;
		public const int PRF_CLIENT = 4;
		public const int PRF_ERASEBKGND = 8;
		public const int PRF_CHILDREN = 16;
        public const int PRF_OWNED = 32;

        public const int RGN_AND         = 1;
        public const int RGN_OR          = 2;
        public const int RGN_XOR         = 3;
        public const int RGN_DIFF        = 4;
        public const int RGN_COPY        = 5;

        public const int SC_MANAGER_ALL_ACCESS = 0x000F003F;
        public const int SERVICE_NO_CHANGE = unchecked((int) 0xffffffff); //this value is found in winsvc.h
        public const int SERVICE_QUERY_CONFIG = 0x00000001;
        public const int SERVICE_CHANGE_CONFIG = 0x00000002;

        public const int SMTO_NORMAL = 0x0000;
        public const int SMTO_BLOCK = 0x0001;
        public const int SMTO_ABORTIFHUNG = 0x0002;
        public const int SMTO_NOTIMEOUTIFNOTHUNG = 0x0008;

        public const int SWP_ASYNCWINDOWPOS = 0x4000;
        public const int SWP_DEFERERASE = 0x2000;
        public const int SWP_DRAWFRAME = 0x0020;
        public const int SWP_FRAMECHANGED = 0x0020;
        public const int SWP_HIDEWINDOW = 0x0080;
        public const int SWP_NOACTIVATE = 0x0010;
        public const int SWP_NOCOPYBITS = 0x0100;
        public const int SWP_NOMOVE = 0x0002;
        public const int SWP_NOOWNERZORDER = 0x0200;
        public const int SWP_NOREDRAW = 0x0008;
        public const int SWP_NOREPOSITION = 0x0200;
        public const int SWP_NOSENDCHANGING = 0x0400;
        public const int SWP_NOSIZE = 0x0001;
        public const int SWP_NOZORDER = 0x0004;
        public const int SWP_SHOWWINDOW = 0x0040;
        public const int SW_HIDE = 0;
        public const int SW_SHOWNORMAL = 1;
        public const int SW_NORMAL = 1;
        public const int SW_SHOWMINIMIZED = 2;
        public const int SW_SHOWMAXIMIZED = 3;
        public const int SW_MAXIMIZE = 3;
        public const int SW_SHOWNOACTIVATE = 4;
        public const int SW_SHOW = 5;
        public const int SW_MINIMIZE = 6;
        public const int SW_SHOWMINNOACTIVE = 7;
        public const int SW_SHOWNA = 8;
        public const int SW_RESTORE = 9;
        public const int SW_SHOWDEFAULT = 10;
        public const int SW_FORCEMINIMIZE = 11;

        public const int S_OK = 0;
        public const int S_FALSE = 1;
        public const int E_NOTIMPL = unchecked((int) 0x80004001);
        public const int E_INVALIDARG = unchecked((int) 0x80070057);
        public const int E_NOINTERFACE = unchecked((int) 0x80004002);
        public const int E_POINTER = unchecked((int) 0x80004003);

        public const int TBCD_TICS    = 0x0001;
        public const int TBCD_THUMB   = 0x0002;
        public const int TBCD_CHANNEL = 0x0003;

        public const int  TBM_GETPOS              = (WM_USER);
        public const int  TBM_GETRANGEMIN         = (WM_USER+1);
        public const int  TBM_GETRANGEMAX         = (WM_USER+2);
        public const int  TBM_GETTIC              = (WM_USER+3);
        public const int  TBM_SETTIC              = (WM_USER+4);
        public const int  TBM_SETPOS              = (WM_USER+5);
        public const int  TBM_SETRANGE            = (WM_USER+6);
        public const int  TBM_SETRANGEMIN         = (WM_USER+7);
        public const int  TBM_SETRANGEMAX         = (WM_USER+8);
        public const int  TBM_CLEARTICS           = (WM_USER+9);
        public const int  TBM_SETSEL              = (WM_USER+10);
        public const int  TBM_SETSELSTART         = (WM_USER+11);
        public const int  TBM_SETSELEND           = (WM_USER+12);
        public const int  TBM_GETPTICS            = (WM_USER+14);
        public const int  TBM_GETTICPOS           = (WM_USER+15);
        public const int  TBM_GETNUMTICS          = (WM_USER+16);
        public const int  TBM_GETSELSTART         = (WM_USER+17);
        public const int  TBM_GETSELEND           = (WM_USER+18);
        public const int  TBM_CLEARSEL            = (WM_USER+19);
        public const int  TBM_SETTICFREQ          = (WM_USER+20);
        public const int  TBM_SETPAGESIZE         = (WM_USER+21);
        public const int  TBM_GETPAGESIZE         = (WM_USER+22);
        public const int  TBM_SETLINESIZE         = (WM_USER+23);
        public const int  TBM_GETLINESIZE         = (WM_USER+24);
        public const int  TBM_GETTHUMBRECT        = (WM_USER+25);
        public const int  TBM_GETCHANNELRECT      = (WM_USER+26);
        public const int  TBM_SETTHUMBLENGTH      = (WM_USER+27);
        public const int  TBM_GETTHUMBLENGTH      = (WM_USER+28);

        public const int WM_NULL = 0x0000;
        public const int WM_CREATE = 0x0001;
        public const int WM_DESTROY = 0x0002;
        public const int WM_MOVE = 0x0003;
        public const int WM_SIZE = 0x0005;
        public const int WM_ACTIVATE = 0x0006;
        public const int WM_SETFOCUS = 0x0007;
        public const int WM_KILLFOCUS = 0x0008;
        public const int WM_ENABLE = 0x000A;
        public const int WM_SETREDRAW = 0x000B;
        public const int WM_SETTEXT = 0x000C;
        public const int WM_GETTEXT = 0x000D;
        public const int WM_GETTEXTLENGTH = 0x000E;
        public const int WM_PAINT = 0x000F;
        public const int WM_CLOSE = 0x0010;
        public const int WM_QUERYENDSESSION = 0x0011;
        public const int WM_QUIT = 0x0012;
        public const int WM_QUERYOPEN = 0x0013;
        public const int WM_ERASEBKGND = 0x0014;
        public const int WM_SYSCOLORCHANGE = 0x0015;
        public const int WM_ENDSESSION = 0x0016;
        public const int WM_SHOWWINDOW = 0x0018;
        public const int WM_WININICHANGE = 0x001A;
        public const int WM_SETTINGCHANGE = 0x001A;
        public const int WM_DEVMODECHANGE = 0x001B;
        public const int WM_ACTIVATEAPP = 0x001C;
        public const int WM_FONTCHANGE = 0x001D;
        public const int WM_TIMECHANGE = 0x001E;
        public const int WM_CANCELMODE = 0x001F;
        public const int WM_SETCURSOR = 0x0020;
        public const int WM_MOUSEACTIVATE = 0x0021;
        public const int WM_CHILDACTIVATE = 0x0022;
        public const int WM_QUEUESYNC = 0x0023;
        public const int WM_GETMINMAXINFO = 0x0024;
        public const int WM_PAINTICON = 0x0026;
        public const int WM_ICONERASEBKGND = 0x0027;
        public const int WM_NEXTDLGCTL = 0x0028;
        public const int WM_SPOOLERSTATUS = 0x002A;
        public const int WM_DRAWITEM = 0x002B;
        public const int WM_MEASUREITEM = 0x002C;
        public const int WM_DELETEITEM = 0x002D;
        public const int WM_VKEYTOITEM = 0x002E;
        public const int WM_CHARTOITEM = 0x002F;
        public const int WM_SETFONT = 0x0030;
        public const int WM_GETFONT = 0x0031;
        public const int WM_SETHOTKEY = 0x0032;
        public const int WM_GETHOTKEY = 0x0033;
        public const int WM_QUERYDRAGICON = 0x0037;
        public const int WM_COMPAREITEM = 0x0039;
        public const int WM_GETOBJECT = 0x003D;
        public const int WM_COMPACTING = 0x0041;
        public const int WM_COMMNOTIFY = 0x0044;
        public const int WM_WINDOWPOSCHANGING = 0x0046;
        public const int WM_WINDOWPOSCHANGED = 0x0047;
        public const int WM_POWER = 0x0048;
        public const int WM_COPYDATA = 0x004A;
        public const int WM_CANCELJOURNAL = 0x004B;
        public const int WM_NOTIFY = 0x004E;
        public const int WM_INPUTLANGCHANGEREQUEST = 0x0050;
        public const int WM_INPUTLANGCHANGE = 0x0051;
        public const int WM_TCARD = 0x0052;
        public const int WM_HELP = 0x0053;
        public const int WM_USERCHANGED = 0x0054;
        public const int WM_NOTIFYFORMAT = 0x0055;
        public const int WM_CONTEXTMENU = 0x007B;
        public const int WM_STYLECHANGING = 0x007C;
        public const int WM_STYLECHANGED = 0x007D;
        public const int WM_DISPLAYCHANGE = 0x007E;
        public const int WM_GETICON = 0x007F;
        public const int WM_SETICON = 0x0080;
        public const int WM_NCCREATE = 0x0081;
        public const int WM_NCDESTROY = 0x0082;
        public const int WM_NCCALCSIZE = 0x0083;
        public const int WM_NCHITTEST = 0x0084;
        public const int WM_NCPAINT = 0x0085;
        public const int WM_NCACTIVATE = 0x0086;
        public const int WM_GETDLGCODE = 0x0087;
        public const int WM_SYNCPAINT = 0x0088;
        public const int WM_NCMOUSEMOVE = 0x00A0;
        public const int WM_NCLBUTTONDOWN = 0x00A1;
        public const int WM_NCLBUTTONUP = 0x00A2;
        public const int WM_NCLBUTTONDBLCLK = 0x00A3;
        public const int WM_NCRBUTTONDOWN = 0x00A4;
        public const int WM_NCRBUTTONUP = 0x00A5;
        public const int WM_NCRBUTTONDBLCLK = 0x00A6;
        public const int WM_NCMBUTTONDOWN = 0x00A7;
        public const int WM_NCMBUTTONUP = 0x00A8;
        public const int WM_NCMBUTTONDBLCLK = 0x00A9;
        public const int WM_KEYFIRST = 0x0100;
        public const int WM_KEYDOWN = 0x0100;
        public const int WM_KEYUP = 0x0101;
        public const int WM_CHAR = 0x0102;
        public const int WM_DEADCHAR = 0x0103;
        public const int WM_SYSKEYDOWN = 0x0104;
        public const int WM_SYSKEYUP = 0x0105;
        public const int WM_SYSCHAR = 0x0106;
        public const int WM_SYSDEADCHAR = 0x0107;
        public const int WM_KEYLAST = 0x0108;
        public const int WM_IME_STARTCOMPOSITION = 0x010D;
        public const int WM_IME_ENDCOMPOSITION = 0x010E;
        public const int WM_IME_COMPOSITION = 0x010F;
        public const int WM_IME_KEYLAST = 0x010F;
        public const int WM_INITDIALOG = 0x0110;
        public const int WM_COMMAND = 0x0111;
        public const int WM_SYSCOMMAND = 0x0112;
        public const int WM_TIMER = 0x0113;
        public const int WM_HSCROLL = 0x0114;
        public const int WM_VSCROLL = 0x0115;
        public const int WM_INITMENU = 0x0116;
        public const int WM_INITMENUPOPUP = 0x0117;
        public const int WM_MENUSELECT = 0x011F;
        public const int WM_MENUCHAR = 0x0120;
        public const int WM_ENTERIDLE = 0x0121;
        public const int WM_MENURBUTTONUP = 0x0122;
        public const int WM_MENUDRAG = 0x0123;
        public const int WM_MENUGETOBJECT = 0x0124;
        public const int WM_UNINITMENUPOPUP = 0x0125;
        public const int WM_MENUCOMMAND = 0x0126;
        public const int WM_CTLCOLORMSGBOX = 0x0132;
        public const int WM_CTLCOLOREDIT = 0x0133;
        public const int WM_CTLCOLORLISTBOX = 0x0134;
        public const int WM_CTLCOLORBTN = 0x0135;
        public const int WM_CTLCOLORDLG = 0x0136;
        public const int WM_CTLCOLORSCROLLBAR = 0x0137;
        public const int WM_CTLCOLORSTATIC = 0x0138;
        public const int WM_MOUSEFIRST = 0x0200;
        public const int WM_MOUSEMOVE = 0x0200;
        public const int WM_LBUTTONDOWN = 0x0201;
        public const int WM_LBUTTONUP = 0x0202;
        public const int WM_LBUTTONDBLCLK = 0x0203;
        public const int WM_RBUTTONDOWN = 0x0204;
        public const int WM_RBUTTONUP = 0x0205;
        public const int WM_RBUTTONDBLCLK = 0x0206;
        public const int WM_MBUTTONDOWN = 0x0207;
        public const int WM_MBUTTONUP = 0x0208;
        public const int WM_MBUTTONDBLCLK = 0x0209;
        public const int WM_MOUSEWHEEL = 0x020A;
        public const int WM_PARENTNOTIFY = 0x0210;
        public const int WM_ENTERMENULOOP = 0x0211;
        public const int WM_EXITMENULOOP = 0x0212;
        public const int WM_NEXTMENU = 0x0213;
        public const int WM_SIZING = 0x0214;
        public const int WM_CAPTURECHANGED = 0x0215;
        public const int WM_MOVING = 0x0216;
        public const int WM_DEVICECHANGE = 0x0219;
        public const int WM_MDICREATE = 0x0220;
        public const int WM_MDIDESTROY = 0x0221;
        public const int WM_MDIACTIVATE = 0x0222;
        public const int WM_MDIRESTORE = 0x0223;
        public const int WM_MDINEXT = 0x0224;
        public const int WM_MDIMAXIMIZE = 0x0225;
        public const int WM_MDITILE = 0x0226;
        public const int WM_MDICASCADE = 0x0227;
        public const int WM_MDIICONARRANGE = 0x0228;
        public const int WM_MDIGETACTIVE = 0x0229;
        public const int WM_MDISETMENU = 0x0230;
        public const int WM_ENTERSIZEMOVE = 0x0231;
        public const int WM_EXITSIZEMOVE = 0x0232;
        public const int WM_DROPFILES = 0x0233;
        public const int WM_MDIREFRESHMENU = 0x0234;
        public const int WM_IME_SETCONTEXT = 0x0281;
        public const int WM_IME_NOTIFY = 0x0282;
        public const int WM_IME_CONTROL = 0x0283;
        public const int WM_IME_COMPOSITIONFULL = 0x0284;
        public const int WM_IME_SELECT = 0x0285;
        public const int WM_IME_CHAR = 0x0286;
        public const int WM_IME_REQUEST = 0x0288;
        public const int WM_IME_KEYDOWN = 0x0290;
        public const int WM_IME_KEYUP = 0x0291;
        public const int WM_MOUSEHOVER = 0x02A1;
        public const int WM_MOUSELEAVE = 0x02A3;
        public const int WM_CUT = 0x0300;
        public const int WM_COPY = 0x0301;
        public const int WM_PASTE = 0x0302;
        public const int WM_CLEAR = 0x0303;
        public const int WM_UNDO = 0x0304;
        public const int WM_RENDERFORMAT = 0x0305;
        public const int WM_RENDERALLFORMATS = 0x0306;
        public const int WM_DESTROYCLIPBOARD = 0x0307;
        public const int WM_DRAWCLIPBOARD = 0x0308;
        public const int WM_PAINTCLIPBOARD = 0x0309;
        public const int WM_VSCROLLCLIPBOARD = 0x030A;
        public const int WM_SIZECLIPBOARD = 0x030B;
        public const int WM_ASKCBFORMATNAME = 0x030C;
        public const int WM_CHANGECBCHAIN = 0x030D;
        public const int WM_HSCROLLCLIPBOARD = 0x030E;
        public const int WM_QUERYNEWPALETTE = 0x030F;
        public const int WM_PALETTEISCHANGING = 0x0310;
        public const int WM_PALETTECHANGED = 0x0311;
        public const int WM_HOTKEY = 0x0312;
        public const int WM_PRINT = 0x0317;
        public const int WM_PRINTCLIENT = 0x0318;
        public const int WM_HANDHELDFIRST = 0x0358;
        public const int WM_HANDHELDLAST = 0x035F;
        public const int WM_AFXFIRST = 0x0360;
        public const int WM_AFXLAST = 0x037F;
        public const int WM_PENWINFIRST = 0x0380;
        public const int WM_PENWINLAST = 0x038F;
        public const int WM_APP = 0x8000;
        public const int WM_MOUSELAST = 0x20A;
        public const int WM_USER = 0x0400;
        public const int WM_REFLECT = WM_USER + 0x1C00;

        public const int WS_OVERLAPPED = 0x00000000;
        public const int WS_POPUP = unchecked((int) 0x80000000);
        public const int WS_CHILD = 0x40000000;
        public const int WS_MINIMIZE = 0x20000000;
        public const int WS_VISIBLE = 0x10000000;
        public const int WS_DISABLED = 0x08000000;
        public const int WS_CLIPSIBLINGS = 0x04000000;
        public const int WS_CLIPCHILDREN = 0x02000000;
        public const int WS_MAXIMIZE = 0x01000000;
        public const int WS_CAPTION = 0x00C00000;
        public const int WS_BORDER = 0x00800000;
        public const int WS_DLGFRAME = 0x00400000;
        public const int WS_VSCROLL = 0x00200000;
        public const int WS_HSCROLL = 0x00100000;
        public const int WS_SYSMENU = 0x00080000;
        public const int WS_THICKFRAME = 0x00040000;
        public const int WS_GROUP = 0x00020000;
        public const int WS_TABSTOP = 0x00010000;
        public const int WS_MINIMIZEBOX = 0x00020000;
        public const int WS_MAXIMIZEBOX = 0x00010000;
        public const int WS_TILED = 0x00000000;
        public const int WS_ICONIC = 0x20000000;
        public const int WS_SIZEBOX = 0x00040000;
        public const int WS_POPUPWINDOW = unchecked((int) 0x80880000);
        public const int WS_OVERLAPPEDWINDOW = 0x00CF0000;
        public const int WS_TILEDWINDOW = 0x00CF0000;
        public const int WS_CHILDWINDOW = 0x40000000;

        public const int WS_EX_DLGMODALFRAME = 0x00000001;
        public const int WS_EX_NOPARENTNOTIFY = 0x00000004;
        public const int WS_EX_TOPMOST = 0x00000008;
        public const int WS_EX_ACCEPTFILES = 0x00000010;
        public const int WS_EX_TRANSPARENT = 0x00000020;
        public const int WS_EX_MDICHILD = 0x00000040;
        public const int WS_EX_TOOLWINDOW = 0x00000080;
        public const int WS_EX_WINDOWEDGE = 0x00000100;
        public const int WS_EX_CLIENTEDGE = 0x00000200;
        public const int WS_EX_CONTEXTHELP = 0x00000400;
        public const int WS_EX_RIGHT = 0x00001000;
        public const int WS_EX_LEFT = 0x00000000;
        public const int WS_EX_RTLREADING = 0x00002000;
        public const int WS_EX_LTRREADING = 0x00000000;
        public const int WS_EX_LEFTSCROLLBAR = 0x00004000;
        public const int WS_EX_RIGHTSCROLLBAR = 0x00000000;
        public const int WS_EX_CONTROLPARENT = 0x00010000;
        public const int WS_EX_STATICEDGE = 0x00020000;
        public const int WS_EX_APPWINDOW = 0x00040000;
        public const int WS_EX_OVERLAPPEDWINDOW = (WS_EX_WINDOWEDGE | WS_EX_CLIENTEDGE);
        public const int WS_EX_PALETTEWINDOW = (WS_EX_WINDOWEDGE | WS_EX_TOOLWINDOW | WS_EX_TOPMOST);

        public const int SEM_FAILCRITICALERRORS = 0x0001;
        public const int SEM_NOALIGNMENTFAULTEXCEPT = 0x0004;
        public const int SEM_NOGPFAULTERRORBOX = 0x0002;
        public const int SEM_NOOPENFILEERRORBOX = 0x8000;

        public const int ATTACH_PARENT_PROCESS = unchecked((int) 0xFFFFFFFF);
        public const int STD_OUTPUT_HANDLE = unchecked((int) 0xFFFFFFF5);
        public const int STD_ERROR_HANDLE = unchecked((int) 0xFFFFFFF4);
        public const int DUPLICATE_SAME_ACCESS = 2;

        public const int WHEEL_DELTA = 120;

        /// <summary>
        /// Policy Enumeration
        /// </summary>
        [Flags]
        public enum LsaAccessPolicy : long
        {
            PolicyViewLocalInformation = 0x00000001L,
            PolicyViewAuditInformation = 0x00000002L,
            PolicyGetPrivateInformation = 0x00000004L,
            PolicyTrustAdmin = 0x00000008L,
            PolicyCreateAccount = 0x00000010L,
            PolicyCreateSecret = 0x00000020L,
            PolicyCreatePrivilege = 0x00000040L,
            PolicySetDefaultQuotaLimits = 0x00000080L,
            PolicySetAuditRequirements = 0x00000100L,
            PolicyAuditLogAdmin = 0x00000200L,
            PolicyServerAdmin = 0x00000400L,
            PolicyLookupNames = 0x00000800L,
            PolicyNotification = 0x00001000L
        }

        #endregion

        #region Structs

        [StructLayout(LayoutKind.Sequential)]
        public struct MSG
        {
            public IntPtr hwnd;
            public int message;
            public IntPtr wParam;
            public IntPtr lParam;
            public int time;
            public int pt_x;
            public int pt_y;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int x;
            public int y;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SIZE
        {
            public int dw;
            public int dy;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;

            public RECT(int left, int top, int right, int bottom)
            {
                this.left = left;
                this.top = top;
                this.right = right;
                this.bottom = bottom;
            }

            public RECT(Rectangle rect)
            {
                left = rect.Left;
                top = rect.Top;
                right = rect.Right;
                bottom = rect.Bottom;
            }

            public RECT(RECT rect)
            {
                left = rect.left;
                top = rect.top;
                right = rect.right;
                bottom = rect.bottom;
            }

            public override string ToString()
            {
                return string.Format("({0}, {1}) ({2}, {3})", left, top, right, bottom);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct NMHDR
        {
            public IntPtr hwndFrom;
            public int idFrom;
            public int code;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct NMCUSTOMDRAW
        {
            public NMHDR hdr;
            public int dwDrawStage;
            public IntPtr hdc;
            public RECT rc;
            public int dwItemSpec;
            public int uItemState;
            public int lItemlParam;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct BY_HANDLE_FILE_INFORMATION
        {
            public int FileAttributes;
            public System.Runtime.InteropServices.ComTypes.FILETIME CreationTime;
            public System.Runtime.InteropServices.ComTypes.FILETIME LastAccessTime;
            public System.Runtime.InteropServices.ComTypes.FILETIME LastWriteTime;
            public int VolumeSerialNumber;
            public int FileSizeHigh;
            public int FileSizeLow;
            public int NumberOfLinks;
            public int FileIndexHigh;
            public int FileIndexLow;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct ServiceFailureActions
        {
            public int dwResetPeriod;

            [MarshalAs(UnmanagedType.LPWStr)]
            public string lpRebootMsg;

            [MarshalAs(UnmanagedType.LPWStr)]
            public string lpCommand;

            public int cActions;

            public IntPtr lpsaActions;
        }

        /// <summary>
        /// Passed to LsaOpenPolicy. 
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct LsaObjectAttributes
        {
            private readonly int Attributes;
            private readonly int Length;
            private readonly IntPtr RootDirectory;
            private readonly IntPtr SecurityDescriptor;
            private readonly IntPtr SecurityQualityOfService;
            private readonly LsaUnicodeString ObjectName;
        }

        /// <summary>
        /// Used in LsaObjectAttributes.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        [CLSCompliant(false)]
        public struct LsaUnicodeString : IDisposable
        {
            public UInt16 Length;
            public UInt16 MaximumLength;

            /// <summary>
            /// NOTE: Buffer has to be declared after Length and MaximumLength;
            /// otherwise, you will get winErrorCode: 1734 (The array bounds are invalid.)
            /// and waste lots of time trying to track down what causes the error!
            /// </summary>
            public IntPtr Buffer;

            public void Dispose()
            {
                Marshal.FreeHGlobal(Buffer);
            }
        }


        #endregion

        #region Utility Functions

        public static int HIWORD(IntPtr n)
        {
            return HIWORD((int) ((long) n));
        }

        public static int HIWORD(int n)
        {
            return ((n >> 0x10) & 0xffff);
        }

        public static int LOWORD(int n)
        {
            return (n & 0xffff);
        }

        public static int LOWORD(IntPtr n)
        {
            return LOWORD((int) ((long) n));
        }

        public static int SignedHIWORD(int n)
        {
            return (short) ((n >> 0x10) & 0xffff);
        }

        public static int SignedHIWORD(IntPtr n)
        {
            return SignedHIWORD((int) ((long) n));
        }

        public static int SignedLOWORD(int n)
        {
            return (short) (n & 0xffff);
        }

        public static int SignedLOWORD(IntPtr n)
        {
            return SignedLOWORD((int) ((long) n));
        }

        #endregion
    }
}
