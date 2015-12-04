using System;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Interapptive.Shared;
using System.Runtime.InteropServices.ComTypes;
using Interapptive.Shared.Win32;

namespace ShipWorks.UI.Controls.Html.Core
{
    [NDependIgnore]
    public static class OleApi
    {
        #region Constants

        public const int DISPID_UNKNOWN = -1;

        public const int DISPID_XOBJ_MIN = -2147418111;

        public const int DISPID_XOBJ_MAX = -2147352577;
        public const int DISPID_XOBJ_BASE = DISPID_XOBJ_MIN;
        public const int DISPID_HTMLOBJECT = (DISPID_XOBJ_BASE + 500);
        public const int DISPID_ELEMENT = (DISPID_HTMLOBJECT + 500);
        public const int DISPID_SITE = (DISPID_ELEMENT + 1000);
        public const int DISPID_OBJECT = (DISPID_SITE + 1000);
        public const int DISPID_STYLE = (DISPID_OBJECT + 1000);
        public const int DISPID_ATTRS = (DISPID_STYLE + 1000);
        public const int DISPID_EVENTS = (DISPID_ATTRS + 1000);
        public const int DISPID_XOBJ_EXPANDO = (DISPID_EVENTS + 1000);
        public const int DISPID_XOBJ_ORDINAL = (DISPID_XOBJ_EXPANDO + 1000);


        public const int STDDISPID_XOBJ_ONBLUR = (DISPID_XOBJ_BASE);
        public const int STDDISPID_XOBJ_ONFOCUS = (DISPID_XOBJ_BASE + 1);
        public const int STDDISPID_XOBJ_BEFOREUPDATE = (DISPID_XOBJ_BASE + 4);
        public const int STDDISPID_XOBJ_AFTERUPDATE = (DISPID_XOBJ_BASE + 5);
        public const int STDDISPID_XOBJ_ONROWEXIT = (DISPID_XOBJ_BASE + 6);
        public const int STDDISPID_XOBJ_ONROWENTER = (DISPID_XOBJ_BASE + 7);
        public const int STDDISPID_XOBJ_ONMOUSEOVER = (DISPID_XOBJ_BASE + 8);
        public const int STDDISPID_XOBJ_ONMOUSEOUT = (DISPID_XOBJ_BASE + 9);
        public const int STDDISPID_XOBJ_ONHELP = (DISPID_XOBJ_BASE + 10);
        public const int STDDISPID_XOBJ_ONDRAGSTART = (DISPID_XOBJ_BASE + 11);
        public const int STDDISPID_XOBJ_ONSELECTSTART = (DISPID_XOBJ_BASE + 12);
        public const int STDDISPID_XOBJ_ERRORUPDATE = (DISPID_XOBJ_BASE + 13);
        public const int STDDISPID_XOBJ_ONDATASETCHANGED = (DISPID_XOBJ_BASE + 14);
        public const int STDDISPID_XOBJ_ONDATAAVAILABLE = (DISPID_XOBJ_BASE + 15);
        public const int STDDISPID_XOBJ_ONDATASETCOMPLETE = (DISPID_XOBJ_BASE + 16);
        public const int STDDISPID_XOBJ_ONFILTER = (DISPID_XOBJ_BASE + 17);
        public const int STDDISPID_XOBJ_ONLOSECAPTURE = (DISPID_XOBJ_BASE + 18);
        public const int STDDISPID_XOBJ_ONPROPERTYCHANGE = (DISPID_XOBJ_BASE + 19);
        public const int STDDISPID_XOBJ_ONDRAG = (DISPID_XOBJ_BASE + 20);
        public const int STDDISPID_XOBJ_ONDRAGEND = (DISPID_XOBJ_BASE + 21);
        public const int STDDISPID_XOBJ_ONDRAGENTER = (DISPID_XOBJ_BASE + 22);
        public const int STDDISPID_XOBJ_ONDRAGOVER = (DISPID_XOBJ_BASE + 23);
        public const int STDDISPID_XOBJ_ONDRAGLEAVE = (DISPID_XOBJ_BASE + 24);
        public const int STDDISPID_XOBJ_ONDROP = (DISPID_XOBJ_BASE + 25);
        public const int STDDISPID_XOBJ_ONCUT = (DISPID_XOBJ_BASE + 26);
        public const int STDDISPID_XOBJ_ONCOPY = (DISPID_XOBJ_BASE + 27);
        public const int STDDISPID_XOBJ_ONPASTE = (DISPID_XOBJ_BASE + 28);
        public const int STDDISPID_XOBJ_ONBEFORECUT = (DISPID_XOBJ_BASE + 29);
        public const int STDDISPID_XOBJ_ONBEFORECOPY = (DISPID_XOBJ_BASE + 30);
        public const int STDDISPID_XOBJ_ONBEFOREPASTE = (DISPID_XOBJ_BASE + 31);
        public const int STDDISPID_XOBJ_ONROWSDELETE = (DISPID_XOBJ_BASE + 32);
        public const int STDDISPID_XOBJ_ONROWSINSERTED = (DISPID_XOBJ_BASE + 33);
        public const int STDDISPID_XOBJ_ONCELLCHANGE = (DISPID_XOBJ_BASE + 34);

        public const int DISPID_CLICK = (-600);
        public const int DISPID_DBLCLICK = (-601);
        public const int DISPID_KEYDOWN = (-602);
        public const int DISPID_KEYPRESS = (-603);
        public const int DISPID_KEYUP = (-604);
        public const int DISPID_MOUSEDOWN = (-605);
        public const int DISPID_MOUSEMOVE = (-606);
        public const int DISPID_MOUSEUP = (-607);
        public const int DISPID_ERROREVENT = (-608);
        public const int DISPID_READYSTATECHANGE = (-609);
        public const int DISPID_CLICK_VALUE = (-610);
        public const int DISPID_RIGHTTOLEFT = (-611);
        public const int DISPID_TOPTOBOTTOM = (-612);
        public const int DISPID_THIS = (-613);

        //  Standard dispatch ID constants

        public const int DISPID_AUTOSIZE = (-500);
        public const int DISPID_BACKCOLOR = (-501);
        public const int DISPID_BACKSTYLE = (-502);
        public const int DISPID_BORDERCOLOR = (-503);
        public const int DISPID_BORDERSTYLE = (-504);
        public const int DISPID_BORDERWIDTH = (-505);
        public const int DISPID_DRAWMODE = (-507);
        public const int DISPID_DRAWSTYLE = (-508);
        public const int DISPID_DRAWWIDTH = (-509);
        public const int DISPID_FILLCOLOR = (-510);
        public const int DISPID_FILLSTYLE = (-511);
        public const int DISPID_FONT = (-512);
        public const int DISPID_FORECOLOR = (-513);
        public const int DISPID_ENABLED = (-514);
        public const int DISPID_HWND = (-515);
        public const int DISPID_TABSTOP = (-516);
        public const int DISPID_TEXT = (-517);
        public const int DISPID_CAPTION = (-518);
        public const int DISPID_BORDERVISIBLE = (-519);
        public const int DISPID_APPEARANCE = (-520);
        public const int DISPID_MOUSEPOINTER = (-521);
        public const int DISPID_MOUSEICON = (-522);
        public const int DISPID_PICTURE = (-523);
        public const int DISPID_VALID = (-524);
        public const int DISPID_READYSTATE = (-525);
        public const int DISPID_LISTINDEX = (-526);
        public const int DISPID_SELECTED = (-527);
        public const int DISPID_LIST = (-528);
        public const int DISPID_COLUMN = (-529);
        public const int DISPID_LISTCOUNT = (-531);
        public const int DISPID_MULTISELECT = (-532);
        public const int DISPID_MAXLENGTH = (-533);
        public const int DISPID_PASSWORDCHAR = (-534);
        public const int DISPID_SCROLLBARS = (-535);
        public const int DISPID_WORDWRAP = (-536);
        public const int DISPID_MULTILINE = (-537);
        public const int DISPID_NUMBEROFROWS = (-538);
        public const int DISPID_NUMBEROFCOLUMNS = (-539);
        public const int DISPID_DISPLAYSTYLE = (-540);
        public const int DISPID_GROUPNAME = (-541);
        public const int DISPID_IMEMODE = (-542);
        public const int DISPID_ACCELERATOR = (-543);
        public const int DISPID_ENTERKEYBEHAVIOR = (-544);
        public const int DISPID_TABKEYBEHAVIOR = (-545);
        public const int DISPID_SELTEXT = (-546);
        public const int DISPID_SELSTART = (-547);
        public const int DISPID_SELLENGTH = (-548);

        public const int DISPID_REFRESH = (-550);
        public const int DISPID_DOCLICK = (-551);
        public const int DISPID_ABOUTBOX = (-552);
        public const int DISPID_ADDITEM = (-553);
        public const int DISPID_CLEAR = (-554);
        public const int DISPID_REMOVEITEM = (-555);
        public const int DISPID_NORMAL_FIRST = 1000;

        public const int DISPID_ONABORT = (DISPID_NORMAL_FIRST);
        public const int DISPID_ONCHANGE = (DISPID_NORMAL_FIRST + 1);
        public const int DISPID_ONERROR = (DISPID_NORMAL_FIRST + 2);
        public const int DISPID_ONLOAD = (DISPID_NORMAL_FIRST + 3);
        public const int DISPID_ONSELECT = (DISPID_NORMAL_FIRST + 6);
        public const int DISPID_ONSUBMIT = (DISPID_NORMAL_FIRST + 7);
        public const int DISPID_ONUNLOAD = (DISPID_NORMAL_FIRST + 8);
        public const int DISPID_ONBOUNCE = (DISPID_NORMAL_FIRST + 9);
        public const int DISPID_ONFINISH = (DISPID_NORMAL_FIRST + 10);
        public const int DISPID_ONSTART = (DISPID_NORMAL_FIRST + 11);
        public const int DISPID_ONLAYOUT = (DISPID_NORMAL_FIRST + 13);
        public const int DISPID_ONSCROLL = (DISPID_NORMAL_FIRST + 14);
        public const int DISPID_ONRESET = (DISPID_NORMAL_FIRST + 15);
        public const int DISPID_ONRESIZE = (DISPID_NORMAL_FIRST + 16);
        public const int DISPID_ONBEFOREUNLOAD = (DISPID_NORMAL_FIRST + 17);
        public const int DISPID_ONCHANGEFOCUS = (DISPID_NORMAL_FIRST + 18);
        public const int DISPID_ONCHANGEBLUR = (DISPID_NORMAL_FIRST + 19);
        public const int DISPID_ONPERSIST = (DISPID_NORMAL_FIRST + 20);
        public const int DISPID_ONPERSISTSAVE = (DISPID_NORMAL_FIRST + 21);
        public const int DISPID_ONPERSISTLOAD = (DISPID_NORMAL_FIRST + 22);
        public const int DISPID_ONCONTEXTMENU = (DISPID_NORMAL_FIRST + 23);
        public const int DISPID_ONBEFOREPRINT = (DISPID_NORMAL_FIRST + 24);
        public const int DISPID_ONAFTERPRINT = (DISPID_NORMAL_FIRST + 25);
        public const int DISPID_ONSTOP = (DISPID_NORMAL_FIRST + 26);
        public const int DISPID_ONBEFOREEDITFOCUS = (DISPID_NORMAL_FIRST + 27);
        public const int DISPID_ONMOUSEHOVER = (DISPID_NORMAL_FIRST + 28);
        public const int DISPID_ONCONTENTREADY = (DISPID_NORMAL_FIRST + 29);
        public const int DISPID_ONLAYOUTCOMPLETE = (DISPID_NORMAL_FIRST + 30);
        public const int DISPID_ONPAGE = (DISPID_NORMAL_FIRST + 31);
        public const int DISPID_ONLINKEDOVERFLOW = (DISPID_NORMAL_FIRST + 32);
        public const int DISPID_ONMOUSEWHEEL = (DISPID_NORMAL_FIRST + 33);
        public const int DISPID_ONBEFOREDEACTIVATE = (DISPID_NORMAL_FIRST + 34);
        public const int DISPID_ONMOVE = (DISPID_NORMAL_FIRST + 35);
        public const int DISPID_ONCONTROLSELECT = (DISPID_NORMAL_FIRST + 36);
        public const int DISPID_ONSELECTIONCHANGE = (DISPID_NORMAL_FIRST + 37);
        public const int DISPID_ONMOVESTART = (DISPID_NORMAL_FIRST + 38);
        public const int DISPID_ONMOVEEND = (DISPID_NORMAL_FIRST + 39);
        public const int DISPID_ONRESIZESTART = (DISPID_NORMAL_FIRST + 40);
        public const int DISPID_ONRESIZEEND = (DISPID_NORMAL_FIRST + 41);
        public const int DISPID_ONMOUSEENTER = (DISPID_NORMAL_FIRST + 42);
        public const int DISPID_ONMOUSELEAVE = (DISPID_NORMAL_FIRST + 43);
        public const int DISPID_ONACTIVATE = (DISPID_NORMAL_FIRST + 44);
        public const int DISPID_ONDEACTIVATE = (DISPID_NORMAL_FIRST + 45);
        public const int DISPID_ONMULTILAYOUTCLEANUP = (DISPID_NORMAL_FIRST + 46);
        public const int DISPID_ONBEFOREACTIVATE = (DISPID_NORMAL_FIRST + 47);
        public const int DISPID_ONFOCUSIN = (DISPID_NORMAL_FIRST + 48);
        public const int DISPID_ONFOCUSOUT = (DISPID_NORMAL_FIRST + 49);



        public const int DISPID_EVPROP_ONMOUSEOVER = (DISPID_EVENTS + 0);
        public const int DISPID_EVMETH_ONMOUSEOVER = STDDISPID_XOBJ_ONMOUSEOVER;
        public const int DISPID_EVPROP_ONMOUSEOUT = (DISPID_EVENTS + 1);
        public const int DISPID_EVMETH_ONMOUSEOUT = STDDISPID_XOBJ_ONMOUSEOUT;
        public const int DISPID_EVPROP_ONMOUSEDOWN = (DISPID_EVENTS + 2);
        public const int DISPID_EVMETH_ONMOUSEDOWN = DISPID_MOUSEDOWN;
        public const int DISPID_EVPROP_ONMOUSEUP = (DISPID_EVENTS + 3);
        public const int DISPID_EVMETH_ONMOUSEUP = DISPID_MOUSEUP;
        public const int DISPID_EVPROP_ONMOUSEMOVE = (DISPID_EVENTS + 4);
        public const int DISPID_EVMETH_ONMOUSEMOVE = DISPID_MOUSEMOVE;
        public const int DISPID_EVPROP_ONKEYDOWN = (DISPID_EVENTS + 5);
        public const int DISPID_EVMETH_ONKEYDOWN = DISPID_KEYDOWN;
        public const int DISPID_EVPROP_ONKEYUP = (DISPID_EVENTS + 6);
        public const int DISPID_EVMETH_ONKEYUP = DISPID_KEYUP;
        public const int DISPID_EVPROP_ONKEYPRESS = (DISPID_EVENTS + 7);
        public const int DISPID_EVMETH_ONKEYPRESS = DISPID_KEYPRESS;
        public const int DISPID_EVPROP_ONCLICK = (DISPID_EVENTS + 8);
        public const int DISPID_EVMETH_ONCLICK = DISPID_CLICK;
        public const int DISPID_EVPROP_ONDBLCLICK = (DISPID_EVENTS + 9);
        public const int DISPID_EVMETH_ONDBLCLICK = DISPID_DBLCLICK;
        public const int DISPID_EVPROP_ONSELECT = (DISPID_EVENTS + 10);
        public const int DISPID_EVMETH_ONSELECT = DISPID_ONSELECT;
        public const int DISPID_EVPROP_ONSUBMIT = (DISPID_EVENTS + 11);
        public const int DISPID_EVMETH_ONSUBMIT = DISPID_ONSUBMIT;
        public const int DISPID_EVPROP_ONRESET = (DISPID_EVENTS + 12);
        public const int DISPID_EVMETH_ONRESET = DISPID_ONRESET;
        public const int DISPID_EVPROP_ONHELP = (DISPID_EVENTS + 13);
        public const int DISPID_EVMETH_ONHELP = STDDISPID_XOBJ_ONHELP;
        public const int DISPID_EVPROP_ONFOCUS = (DISPID_EVENTS + 14);
        public const int DISPID_EVMETH_ONFOCUS = STDDISPID_XOBJ_ONFOCUS;
        public const int DISPID_EVPROP_ONBLUR = (DISPID_EVENTS + 15);
        public const int DISPID_EVMETH_ONBLUR = STDDISPID_XOBJ_ONBLUR;
        public const int DISPID_EVPROP_ONROWEXIT = (DISPID_EVENTS + 18);
        public const int DISPID_EVMETH_ONROWEXIT = STDDISPID_XOBJ_ONROWEXIT;
        public const int DISPID_EVPROP_ONROWENTER = (DISPID_EVENTS + 19);
        public const int DISPID_EVMETH_ONROWENTER = STDDISPID_XOBJ_ONROWENTER;
        public const int DISPID_EVPROP_ONBOUNCE = (DISPID_EVENTS + 20);
        public const int DISPID_EVMETH_ONBOUNCE = DISPID_ONBOUNCE;
        public const int DISPID_EVPROP_ONBEFOREUPDATE = (DISPID_EVENTS + 21);
        public const int DISPID_EVMETH_ONBEFOREUPDATE = STDDISPID_XOBJ_BEFOREUPDATE;
        public const int DISPID_EVPROP_ONAFTERUPDATE = (DISPID_EVENTS + 22);
        public const int DISPID_EVMETH_ONAFTERUPDATE = STDDISPID_XOBJ_AFTERUPDATE;
        public const int DISPID_EVPROP_ONBEFOREDRAGOVER = (DISPID_EVENTS + 23);
        public const int DISPID_EVPROP_ONBEFOREDROPORPASTE = (DISPID_EVENTS + 24);
        public const int DISPID_EVPROP_ONREADYSTATECHANGE = (DISPID_EVENTS + 25);
        public const int DISPID_EVMETH_ONREADYSTATECHANGE = DISPID_READYSTATECHANGE;
        public const int DISPID_EVPROP_ONFINISH = (DISPID_EVENTS + 26);
        public const int DISPID_EVMETH_ONFINISH = DISPID_ONFINISH;
        public const int DISPID_EVPROP_ONSTART = (DISPID_EVENTS + 27);
        public const int DISPID_EVMETH_ONSTART = DISPID_ONSTART;
        public const int DISPID_EVPROP_ONABORT = (DISPID_EVENTS + 28);
        public const int DISPID_EVMETH_ONABORT = DISPID_ONABORT;
        public const int DISPID_EVPROP_ONERROR = (DISPID_EVENTS + 29);
        public const int DISPID_EVMETH_ONERROR = DISPID_ONERROR;
        public const int DISPID_EVPROP_ONCHANGE = (DISPID_EVENTS + 30);
        public const int DISPID_EVMETH_ONCHANGE = DISPID_ONCHANGE;
        public const int DISPID_EVPROP_ONSCROLL = (DISPID_EVENTS + 31);
        public const int DISPID_EVMETH_ONSCROLL = DISPID_ONSCROLL;
        public const int DISPID_EVPROP_ONLOAD = (DISPID_EVENTS + 32);
        public const int DISPID_EVMETH_ONLOAD = DISPID_ONLOAD;
        public const int DISPID_EVPROP_ONUNLOAD = (DISPID_EVENTS + 33);
        public const int DISPID_EVMETH_ONUNLOAD = DISPID_ONUNLOAD;
        public const int DISPID_EVPROP_ONLAYOUT = (DISPID_EVENTS + 34);
        public const int DISPID_EVMETH_ONLAYOUT = DISPID_ONLAYOUT;
        public const int DISPID_EVPROP_ONDRAGSTART = (DISPID_EVENTS + 35);
        public const int DISPID_EVMETH_ONDRAGSTART = STDDISPID_XOBJ_ONDRAGSTART;
        public const int DISPID_EVPROP_ONRESIZE = (DISPID_EVENTS + 36);
        public const int DISPID_EVMETH_ONRESIZE = DISPID_ONRESIZE;
        public const int DISPID_EVPROP_ONSELECTSTART = (DISPID_EVENTS + 37);
        public const int DISPID_EVMETH_ONSELECTSTART = STDDISPID_XOBJ_ONSELECTSTART;
        public const int DISPID_EVPROP_ONERRORUPDATE = (DISPID_EVENTS + 38);
        public const int DISPID_EVMETH_ONERRORUPDATE = STDDISPID_XOBJ_ERRORUPDATE;
        public const int DISPID_EVPROP_ONBEFOREUNLOAD = (DISPID_EVENTS + 39);
        public const int DISPID_EVPROP_ONDATASETCHANGED = (DISPID_EVENTS + 40);
        public const int DISPID_EVMETH_ONDATASETCHANGED = STDDISPID_XOBJ_ONDATASETCHANGED;
        public const int DISPID_EVPROP_ONDATAAVAILABLE = (DISPID_EVENTS + 41);
        public const int DISPID_EVPROP_ONDATASETCOMPLETE = (DISPID_EVENTS + 42);
        public const int DISPID_EVPROP_ONFILTER = (DISPID_EVENTS + 43);
        public const int DISPID_EVPROP_ONCHANGEFOCUS = (DISPID_EVENTS + 44);
        public const int DISPID_EVPROP_ONCHANGEBLUR = (DISPID_EVENTS + 45);
        public const int DISPID_EVPROP_ONLOSECAPTURE = (DISPID_EVENTS + 46);
        public const int DISPID_EVPROP_ONPROPERTYCHANGE = (DISPID_EVENTS + 47);
        public const int DISPID_EVPROP_ONPERSISTSAVE = (DISPID_EVENTS + 48);
        public const int DISPID_EVPROP_ONDRAG = (DISPID_EVENTS + 49);
        public const int DISPID_EVPROP_ONDRAGEND = (DISPID_EVENTS + 50);
        public const int DISPID_EVPROP_ONDRAGENTER = (DISPID_EVENTS + 51);
        public const int DISPID_EVPROP_ONDRAGOVER = (DISPID_EVENTS + 52);
        public const int DISPID_EVPROP_ONDRAGLEAVE = (DISPID_EVENTS + 53);
        public const int DISPID_EVPROP_ONDROP = (DISPID_EVENTS + 54);
        public const int DISPID_EVPROP_ONCUT = (DISPID_EVENTS + 55);
        public const int DISPID_EVPROP_ONCOPY = (DISPID_EVENTS + 56);
        public const int DISPID_EVPROP_ONPASTE = (DISPID_EVENTS + 57);
        public const int DISPID_EVPROP_ONBEFORECUT = (DISPID_EVENTS + 58);
        public const int DISPID_EVPROP_ONBEFORECOPY = (DISPID_EVENTS + 59);
        public const int DISPID_EVPROP_ONBEFOREPASTE = (DISPID_EVENTS + 60);
        public const int DISPID_EVPROP_ONPERSISTLOAD = (DISPID_EVENTS + 61);
        public const int DISPID_EVPROP_ONROWSDELETE = (DISPID_EVENTS + 62);
        public const int DISPID_EVPROP_ONROWSINSERTED = (DISPID_EVENTS + 63);
        public const int DISPID_EVPROP_ONCELLCHANGE = (DISPID_EVENTS + 64);
        public const int DISPID_EVPROP_ONCONTEXTMENU = (DISPID_EVENTS + 65);
        public const int DISPID_EVPROP_ONBEFOREPRINT = (DISPID_EVENTS + 66);
        public const int DISPID_EVPROP_ONAFTERPRINT = (DISPID_EVENTS + 67);
        public const int DISPID_EVPROP_ONSTOP = (DISPID_EVENTS + 68);
        public const int DISPID_EVPROP_ONBEFOREEDITFOCUS = (DISPID_EVENTS + 69);
        public const int DISPID_EVMETH_ONBEFOREEDITFOCUS = DISPID_ONBEFOREEDITFOCUS;
        public const int DISPID_EVPROP_ONATTACHEVENT = (DISPID_EVENTS + 70);
        public const int DISPID_EVPROP_ONMOUSEHOVER = (DISPID_EVENTS + 71);
        public const int DISPID_EVMETH_ONMOUSEHOVER = DISPID_ONMOUSEHOVER;
        public const int DISPID_EVPROP_ONCONTENTREADY = (DISPID_EVENTS + 72);
        public const int DISPID_EVMETH_ONCONTENTREADY = DISPID_ONCONTENTREADY;
        public const int DISPID_EVPROP_ONLAYOUTCOMPLETE = (DISPID_EVENTS + 73);
        public const int DISPID_EVMETH_ONLAYOUTCOMPLETE = DISPID_ONLAYOUTCOMPLETE;
        public const int DISPID_EVPROP_ONPAGE = (DISPID_EVENTS + 74);
        public const int DISPID_EVMETH_ONPAGE = DISPID_ONPAGE;
        public const int DISPID_EVPROP_ONLINKEDOVERFLOW = (DISPID_EVENTS + 75);
        public const int DISPID_EVMETH_ONLINKEDOVERFLOW = DISPID_ONLINKEDOVERFLOW;
        public const int DISPID_EVPROP_ONMOUSEWHEEL = (DISPID_EVENTS + 76);
        public const int DISPID_EVMETH_ONMOUSEWHEEL = DISPID_ONMOUSEWHEEL;
        public const int DISPID_EVPROP_ONBEFOREDEACTIVATE = (DISPID_EVENTS + 77);
        public const int DISPID_EVMETH_ONBEFOREDEACTIVATE = DISPID_ONBEFOREDEACTIVATE;
        public const int DISPID_EVPROP_ONMOVE = (DISPID_EVENTS + 78);
        public const int DISPID_EVMETH_ONMOVE = DISPID_ONMOVE;
        public const int DISPID_EVPROP_ONCONTROLSELECT = (DISPID_EVENTS + 79);
        public const int DISPID_EVMETH_ONCONTROLSELECT = DISPID_ONCONTROLSELECT;
        public const int DISPID_EVPROP_ONSELECTIONCHANGE = (DISPID_EVENTS + 80);
        public const int DISPID_EVMETH_ONSELECTIONCHANGE = DISPID_ONSELECTIONCHANGE;
        public const int DISPID_EVPROP_ONMOVESTART = (DISPID_EVENTS + 81);
        public const int DISPID_EVMETH_ONMOVESTART = DISPID_ONMOVESTART;
        public const int DISPID_EVPROP_ONMOVEEND = (DISPID_EVENTS + 82);
        public const int DISPID_EVMETH_ONMOVEEND = DISPID_ONMOVEEND;
        public const int DISPID_EVPROP_ONRESIZESTART = (DISPID_EVENTS + 83);
        public const int DISPID_EVMETH_ONRESIZESTART = DISPID_ONRESIZESTART;
        public const int DISPID_EVPROP_ONRESIZEEND = (DISPID_EVENTS + 84);
        public const int DISPID_EVMETH_ONRESIZEEND = DISPID_ONRESIZEEND;
        public const int DISPID_EVPROP_ONMOUSEENTER = (DISPID_EVENTS + 85);
        public const int DISPID_EVMETH_ONMOUSEENTER = DISPID_ONMOUSEENTER;
        public const int DISPID_EVPROP_ONMOUSELEAVE = (DISPID_EVENTS + 86);
        public const int DISPID_EVMETH_ONMOUSELEAVE = DISPID_ONMOUSELEAVE;
        public const int DISPID_EVPROP_ONACTIVATE = (DISPID_EVENTS + 87);
        public const int DISPID_EVMETH_ONACTIVATE = DISPID_ONACTIVATE;
        public const int DISPID_EVPROP_ONDEACTIVATE = (DISPID_EVENTS + 88);
        public const int DISPID_EVMETH_ONDEACTIVATE = DISPID_ONDEACTIVATE;
        public const int DISPID_EVPROP_ONMULTILAYOUTCLEANUP = (DISPID_EVENTS + 89);
        public const int DISPID_EVMETH_ONMULTILAYOUTCLEANUP = DISPID_ONMULTILAYOUTCLEANUP;
        public const int DISPID_EVPROP_ONBEFOREACTIVATE = (DISPID_EVENTS + 90);
        public const int DISPID_EVMETH_ONBEFOREACTIVATE = DISPID_ONBEFOREACTIVATE;
        public const int DISPID_EVPROP_ONFOCUSIN = (DISPID_EVENTS + 91);
        public const int DISPID_EVMETH_ONFOCUSIN = DISPID_ONFOCUSIN;
        public const int DISPID_EVPROP_ONFOCUSOUT = (DISPID_EVENTS + 92);
        public const int DISPID_EVMETH_ONFOCUSOUT = DISPID_ONFOCUSOUT;

        public const int DISPID_IHTMLELEMENT_SETATTRIBUTE = DISPID_HTMLOBJECT + 1;
        public const int DISPID_IHTMLELEMENT_GETATTRIBUTE = DISPID_HTMLOBJECT + 2;
        public const int DISPID_IHTMLELEMENT_REMOVEATTRIBUTE = DISPID_HTMLOBJECT + 3;
        public const int DISPID_IHTMLELEMENT_CLASSNAME = DISPID_ELEMENT + 1;
        public const int DISPID_IHTMLELEMENT_ID = DISPID_ELEMENT + 2;
        public const int DISPID_IHTMLELEMENT_TAGNAME = DISPID_ELEMENT + 4;
        public const int DISPID_IHTMLELEMENT_ONHELP = DISPID_EVPROP_ONHELP; //-2147412098
        public const int DISPID_IHTMLELEMENT_ONCLICK = DISPID_EVPROP_ONCLICK; //-2147412103
        public const int DISPID_IHTMLELEMENT_ONDBLCLICK = DISPID_EVPROP_ONDBLCLICK;//-2147412102
        public const int DISPID_IHTMLELEMENT_ONKEYDOWN = DISPID_EVPROP_ONKEYDOWN; //-2147412106
        public const int DISPID_IHTMLELEMENT_ONKEYUP = DISPID_EVPROP_ONKEYUP;
        public const int DISPID_IHTMLELEMENT_ONKEYPRESS = DISPID_EVPROP_ONKEYPRESS; //-2147412104
        public const int DISPID_IHTMLELEMENT_ONMOUSEOUT = DISPID_EVPROP_ONMOUSEOUT; //-2147412110
        public const int DISPID_IHTMLELEMENT_ONMOUSEOVER = DISPID_EVPROP_ONMOUSEOVER; //-2147412111
        public const int DISPID_IHTMLELEMENT_ONMOUSEMOVE = DISPID_EVPROP_ONMOUSEMOVE; // -2147412107
        public const int DISPID_IHTMLELEMENT_ONMOUSEDOWN = DISPID_EVPROP_ONMOUSEDOWN;
        public const int DISPID_IHTMLELEMENT_ONMOUSEUP = DISPID_EVPROP_ONMOUSEUP;
        public const int DISPID_IHTMLELEMENT_DOCUMENT = DISPID_ELEMENT + 18;
        public const int DISPID_IHTMLELEMENT_ONSELECTSTART = DISPID_EVPROP_ONSELECTSTART;
        public const int DISPID_IHTMLELEMENT_SCROLLINTOVIEW = DISPID_ELEMENT + 19;
        public const int DISPID_IHTMLELEMENT_CONTAINS = DISPID_ELEMENT + 20;
        public const int DISPID_IHTMLELEMENT_SOURCEINDEX = DISPID_ELEMENT + 24;
        public const int DISPID_IHTMLELEMENT_RECORDNUMBER = DISPID_ELEMENT + 25;
        public const int DISPID_IHTMLELEMENT_OFFSETLEFT = DISPID_ELEMENT + 8;
        public const int DISPID_IHTMLELEMENT_OFFSETTOP = DISPID_ELEMENT + 9;
        public const int DISPID_IHTMLELEMENT_OFFSETWIDTH = DISPID_ELEMENT + 10;
        public const int DISPID_IHTMLELEMENT_OFFSETHEIGHT = DISPID_ELEMENT + 11;
        public const int DISPID_IHTMLELEMENT_OFFSETPARENT = DISPID_ELEMENT + 12;
        public const int DISPID_IHTMLELEMENT_INNERHTML = DISPID_ELEMENT + 26;
        public const int DISPID_IHTMLELEMENT_INNERTEXT = DISPID_ELEMENT + 27;
        public const int DISPID_IHTMLELEMENT_OUTERHTML = DISPID_ELEMENT + 28;
        public const int DISPID_IHTMLELEMENT_OUTERTEXT = DISPID_ELEMENT + 29;
        public const int DISPID_IHTMLELEMENT_INSERTADJACENTHTML = DISPID_ELEMENT + 30;
        public const int DISPID_IHTMLELEMENT_INSERTADJACENTTEXT = DISPID_ELEMENT + 31;
        public const int DISPID_IHTMLELEMENT_PARENTTEXTEDIT = DISPID_ELEMENT + 32;
        public const int DISPID_IHTMLELEMENT_ISTEXTEDIT = DISPID_ELEMENT + 34;
        public const int DISPID_IHTMLELEMENT_CLICK = DISPID_ELEMENT + 33;
        public const int DISPID_IHTMLELEMENT_FILTERS = DISPID_ELEMENT + 35;
        public const int DISPID_IHTMLELEMENT_ONDRAGSTART = DISPID_EVPROP_ONDRAGSTART;
        public const int DISPID_IHTMLELEMENT_TOSTRING = DISPID_ELEMENT + 36;
        public const int DISPID_IHTMLELEMENT_ONBEFOREUPDATE = DISPID_EVPROP_ONBEFOREUPDATE;
        public const int DISPID_IHTMLELEMENT_ONAFTERUPDATE = DISPID_EVPROP_ONAFTERUPDATE;
        public const int DISPID_IHTMLELEMENT_ONERRORUPDATE = DISPID_EVPROP_ONERRORUPDATE;
        public const int DISPID_IHTMLELEMENT_ONROWEXIT = DISPID_EVPROP_ONROWEXIT;
        public const int DISPID_IHTMLELEMENT_ONROWENTER = DISPID_EVPROP_ONROWENTER;
        public const int DISPID_IHTMLELEMENT_ONDATASETCHANGED = DISPID_EVPROP_ONDATASETCHANGED;
        public const int DISPID_IHTMLELEMENT_ONDATAAVAILABLE = DISPID_EVPROP_ONDATAAVAILABLE;
        public const int DISPID_IHTMLELEMENT_ONDATASETCOMPLETE = DISPID_EVPROP_ONDATASETCOMPLETE;
        public const int DISPID_IHTMLELEMENT_ONFILTERCHANGE = DISPID_EVPROP_ONFILTER;
        public const int DISPID_IHTMLELEMENT_CHILDREN = DISPID_ELEMENT + 37;
        public const int DISPID_IHTMLELEMENT_ALL = DISPID_ELEMENT + 38;

        //  DISPIDs for interface IHTMLElement2

        public const int DISPID_IHTMLELEMENT2_SCOPENAME = DISPID_ELEMENT + 39;
        public const int DISPID_IHTMLELEMENT2_SETCAPTURE = DISPID_ELEMENT + 40;
        public const int DISPID_IHTMLELEMENT2_RELEASECAPTURE = DISPID_ELEMENT + 41;
        public const int DISPID_IHTMLELEMENT2_ONLOSECAPTURE = DISPID_EVPROP_ONLOSECAPTURE;
        public const int DISPID_IHTMLELEMENT2_COMPONENTFROMPOINT = DISPID_ELEMENT + 42;
        public const int DISPID_IHTMLELEMENT2_DOSCROLL = DISPID_ELEMENT + 43;
        public const int DISPID_IHTMLELEMENT2_ONSCROLL = DISPID_EVPROP_ONSCROLL;
        public const int DISPID_IHTMLELEMENT2_ONDRAG = DISPID_EVPROP_ONDRAG;
        public const int DISPID_IHTMLELEMENT2_ONDRAGEND = DISPID_EVPROP_ONDRAGEND;
        public const int DISPID_IHTMLELEMENT2_ONDRAGENTER = DISPID_EVPROP_ONDRAGENTER;
        public const int DISPID_IHTMLELEMENT2_ONDRAGOVER = DISPID_EVPROP_ONDRAGOVER;
        public const int DISPID_IHTMLELEMENT2_ONDRAGLEAVE = DISPID_EVPROP_ONDRAGLEAVE;
        public const int DISPID_IHTMLELEMENT2_ONDROP = DISPID_EVPROP_ONDROP;
        public const int DISPID_IHTMLELEMENT2_ONBEFORECUT = DISPID_EVPROP_ONBEFORECUT;
        public const int DISPID_IHTMLELEMENT2_ONCUT = DISPID_EVPROP_ONCUT;
        public const int DISPID_IHTMLELEMENT2_ONBEFORECOPY = DISPID_EVPROP_ONBEFORECOPY;
        public const int DISPID_IHTMLELEMENT2_ONCOPY = DISPID_EVPROP_ONCOPY;
        public const int DISPID_IHTMLELEMENT2_ONBEFOREPASTE = DISPID_EVPROP_ONBEFOREPASTE;
        public const int DISPID_IHTMLELEMENT2_ONPASTE = DISPID_EVPROP_ONPASTE;
        public const int DISPID_IHTMLELEMENT2_CURRENTSTYLE = DISPID_ELEMENT + 7;
        public const int DISPID_IHTMLELEMENT2_ONPROPERTYCHANGE = DISPID_EVPROP_ONPROPERTYCHANGE;
        public const int DISPID_IHTMLELEMENT2_GETCLIENTRECTS = DISPID_ELEMENT + 44;
        public const int DISPID_IHTMLELEMENT2_GETBOUNDINGCLIENTRECT = DISPID_ELEMENT + 45;
        public const int DISPID_IHTMLELEMENT2_SETEXPRESSION = DISPID_HTMLOBJECT + 4;
        public const int DISPID_IHTMLELEMENT2_GETEXPRESSION = DISPID_HTMLOBJECT + 5;
        public const int DISPID_IHTMLELEMENT2_REMOVEEXPRESSION = DISPID_HTMLOBJECT + 6;
        public const int DISPID_IHTMLELEMENT2_FOCUS = DISPID_SITE + 0;
        public const int DISPID_IHTMLELEMENT2_ACCESSKEY = DISPID_SITE + 5;
        public const int DISPID_IHTMLELEMENT2_ONBLUR = DISPID_EVPROP_ONBLUR;
        public const int DISPID_IHTMLELEMENT2_ONFOCUS = DISPID_EVPROP_ONFOCUS;
        public const int DISPID_IHTMLELEMENT2_ONRESIZE = DISPID_EVPROP_ONRESIZE;
        public const int DISPID_IHTMLELEMENT2_BLUR = DISPID_SITE + 2;
        public const int DISPID_IHTMLELEMENT2_ADDFILTER = DISPID_SITE + 17;
        public const int DISPID_IHTMLELEMENT2_REMOVEFILTER = DISPID_SITE + 18;
        public const int DISPID_IHTMLELEMENT2_CLIENTHEIGHT = DISPID_SITE + 19;
        public const int DISPID_IHTMLELEMENT2_CLIENTWIDTH = DISPID_SITE + 20;
        public const int DISPID_IHTMLELEMENT2_CLIENTTOP = DISPID_SITE + 21;
        public const int DISPID_IHTMLELEMENT2_CLIENTLEFT = DISPID_SITE + 22;
        public const int DISPID_IHTMLELEMENT2_ATTACHEVENT = DISPID_HTMLOBJECT + 7;
        public const int DISPID_IHTMLELEMENT2_DETACHEVENT = DISPID_HTMLOBJECT + 8;
        public const int DISPID_IHTMLELEMENT2_ONREADYSTATECHANGE = DISPID_EVPROP_ONREADYSTATECHANGE;
        public const int DISPID_IHTMLELEMENT2_ONROWSDELETE = DISPID_EVPROP_ONROWSDELETE;
        public const int DISPID_IHTMLELEMENT2_ONROWSINSERTED = DISPID_EVPROP_ONROWSINSERTED;
        public const int DISPID_IHTMLELEMENT2_ONCELLCHANGE = DISPID_EVPROP_ONCELLCHANGE;
        public const int DISPID_IHTMLELEMENT2_CREATECONTROLRANGE = DISPID_ELEMENT + 56;
        public const int DISPID_IHTMLELEMENT2_SCROLLHEIGHT = DISPID_ELEMENT + 57;
        public const int DISPID_IHTMLELEMENT2_SCROLLWIDTH = DISPID_ELEMENT + 58;
        public const int DISPID_IHTMLELEMENT2_SCROLLTOP = DISPID_ELEMENT + 59;
        public const int DISPID_IHTMLELEMENT2_SCROLLLEFT = DISPID_ELEMENT + 60;
        public const int DISPID_IHTMLELEMENT2_CLEARATTRIBUTES = DISPID_ELEMENT + 62;
        public const int DISPID_IHTMLELEMENT2_MERGEATTRIBUTES = DISPID_ELEMENT + 63;
        public const int DISPID_IHTMLELEMENT2_ONCONTEXTMENU = DISPID_EVPROP_ONCONTEXTMENU;
        public const int DISPID_IHTMLELEMENT2_INSERTADJACENTELEMENT = DISPID_ELEMENT + 69;
        public const int DISPID_IHTMLELEMENT2_APPLYELEMENT = DISPID_ELEMENT + 65;
        public const int DISPID_IHTMLELEMENT2_GETADJACENTTEXT = DISPID_ELEMENT + 70;
        public const int DISPID_IHTMLELEMENT2_REPLACEADJACENTTEXT = DISPID_ELEMENT + 71;
        public const int DISPID_IHTMLELEMENT2_CANHAVECHILDREN = DISPID_ELEMENT + 72;
        public const int DISPID_IHTMLELEMENT2_ADDBEHAVIOR = DISPID_ELEMENT + 80;
        public const int DISPID_IHTMLELEMENT2_REMOVEBEHAVIOR = DISPID_ELEMENT + 81;
        public const int DISPID_IHTMLELEMENT2_RUNTIMESTYLE = DISPID_ELEMENT + 64;
        public const int DISPID_IHTMLELEMENT2_BEHAVIORURNS = DISPID_ELEMENT + 82;
        public const int DISPID_IHTMLELEMENT2_TAGURN = DISPID_ELEMENT + 83;
        public const int DISPID_IHTMLELEMENT2_ONBEFOREEDITFOCUS = DISPID_EVPROP_ONBEFOREEDITFOCUS;
        public const int DISPID_IHTMLELEMENT2_READYSTATEVALUE = DISPID_ELEMENT + 84;
        public const int DISPID_IHTMLELEMENT2_GETELEMENTSBYTAGNAME = DISPID_ELEMENT + 85;



        //MSHTML Command IDs
        //----------------------------------------------------------------------------
        //
        // MSHTML Command IDs
        //
        //----------------------------------------------------------------------------

        public const int IDM_UNKNOWN = 0;
        public const int IDM_ALIGNBOTTOM = 1;
        public const int IDM_ALIGNHORIZONTALCENTERS = 2;
        public const int IDM_ALIGNLEFT = 3;
        public const int IDM_ALIGNRIGHT = 4;
        public const int IDM_ALIGNTOGRID = 5;
        public const int IDM_ALIGNTOP = 6;
        public const int IDM_ALIGNVERTICALCENTERS = 7;
        public const int IDM_ARRANGEBOTTOM = 8;
        public const int IDM_ARRANGERIGHT = 9;
        public const int IDM_BRINGFORWARD = 10;
        public const int IDM_BRINGTOFRONT = 11;
        public const int IDM_CENTERHORIZONTALLY = 12;
        public const int IDM_CENTERVERTICALLY = 13;
        public const int IDM_CODE = 14;
        public const int IDM_DELETE = 17;
        public const int IDM_FONTNAME = 18;
        public const int IDM_FONTSIZE = 19;
        public const int IDM_GROUP = 20;
        public const int IDM_HORIZSPACECONCATENATE = 21;
        public const int IDM_HORIZSPACEDECREASE = 22;
        public const int IDM_HORIZSPACEINCREASE = 23;
        public const int IDM_HORIZSPACEMAKEEQUAL = 24;
        public const int IDM_INSERTOBJECT = 25;
        public const int IDM_MULTILEVELREDO = 30;
        public const int IDM_SENDBACKWARD = 32;
        public const int IDM_SENDTOBACK = 33;
        public const int IDM_SHOWTABLE = 34;
        public const int IDM_SIZETOCONTROL = 35;
        public const int IDM_SIZETOCONTROLHEIGHT = 36;
        public const int IDM_SIZETOCONTROLWIDTH = 37;
        public const int IDM_SIZETOFIT = 38;
        public const int IDM_SIZETOGRID = 39;
        public const int IDM_SNAPTOGRID = 40;
        public const int IDM_TABORDER = 41;
        public const int IDM_TOOLBOX = 42;
        public const int IDM_MULTILEVELUNDO = 44;
        public const int IDM_UNGROUP = 45;
        public const int IDM_VERTSPACECONCATENATE = 46;
        public const int IDM_VERTSPACEDECREASE = 47;
        public const int IDM_VERTSPACEINCREASE = 48;
        public const int IDM_VERTSPACEMAKEEQUAL = 49;
        public const int IDM_JUSTIFYFULL = 50;
        public const int IDM_BACKCOLOR = 51;
        public const int IDM_BOLD = 52;
        public const int IDM_BORDERCOLOR = 53;
        public const int IDM_FLAT = 54;
        public const int IDM_FORECOLOR = 55;
        public const int IDM_ITALIC = 56;
        public const int IDM_JUSTIFYCENTER = 57;
        public const int IDM_JUSTIFYGENERAL = 58;
        public const int IDM_JUSTIFYLEFT = 59;
        public const int IDM_JUSTIFYRIGHT = 60;
        public const int IDM_RAISED = 61;
        public const int IDM_SUNKEN = 62;
        public const int IDM_UNDERLINE = 63;
        public const int IDM_CHISELED = 64;
        public const int IDM_ETCHED = 65;
        public const int IDM_SHADOWED = 66;
        public const int IDM_FIND = 67;
        public const int IDM_SHOWGRID = 69;
        public const int IDM_OBJECTVERBLIST0 = 72;
        public const int IDM_OBJECTVERBLIST1 = 73;
        public const int IDM_OBJECTVERBLIST2 = 74;
        public const int IDM_OBJECTVERBLIST3 = 75;
        public const int IDM_OBJECTVERBLIST4 = 76;
        public const int IDM_OBJECTVERBLIST5 = 77;
        public const int IDM_OBJECTVERBLIST6 = 78;
        public const int IDM_OBJECTVERBLIST7 = 79;
        public const int IDM_OBJECTVERBLIST8 = 80;
        public const int IDM_OBJECTVERBLIST9 = 81;
        public const int IDM_OBJECTVERBLISTLAST = IDM_OBJECTVERBLIST9;
        public const int IDM_CONVERTOBJECT = 82;
        public const int IDM_CUSTOMCONTROL = 83;
        public const int IDM_CUSTOMIZEITEM = 84;
        public const int IDM_RENAME = 85;
        public const int IDM_IMPORT = 86;
        public const int IDM_NEWPAGE = 87;
        public const int IDM_MOVE = 88;
        public const int IDM_CANCEL = 89;
        public const int IDM_FONT = 90;
        public const int IDM_STRIKETHROUGH = 91;
        public const int IDM_DELETEWORD = 92;
        public const int IDM_EXECPRINT = 93;
        public const int IDM_JUSTIFYNONE = 94;
        public const int IDM_TRISTATEBOLD = 95;
        public const int IDM_TRISTATEITALIC = 96;
        public const int IDM_TRISTATEUNDERLINE = 97;

        public const int IDM_FOLLOW_ANCHOR = 2008;

        public const int IDM_INSINPUTIMAGE = 2114;
        public const int IDM_INSINPUTBUTTON = 2115;
        public const int IDM_INSINPUTRESET = 2116;
        public const int IDM_INSINPUTSUBMIT = 2117;
        public const int IDM_INSINPUTUPLOAD = 2118;
        public const int IDM_INSFIELDSET = 2119;

        public const int IDM_PASTEINSERT = 2120;
        public const int IDM_REPLACE = 2121;
        public const int IDM_EDITSOURCE = 2122;
        public const int IDM_BOOKMARK = 2123;
        public const int IDM_HYPERLINK = 2124;
        public const int IDM_UNLINK = 2125;
        public const int IDM_BROWSEMODE = 2126;
        public const int IDM_EDITMODE = 2127;
        public const int IDM_UNBOOKMARK = 2128;

        public const int IDM_TOOLBARS = 2130;
        public const int IDM_STATUSBAR = 2131;
        public const int IDM_FORMATMARK = 2132;
        public const int IDM_TEXTONLY = 2133;
        public const int IDM_OPTIONS = 2135;
        public const int IDM_FOLLOWLINKC = 2136;
        public const int IDM_FOLLOWLINKN = 2137;
        public const int IDM_VIEWSOURCE = 2139;
        public const int IDM_ZOOMPOPUP = 2140;

        // IDM_BASELINEFONT1, IDM_BASELINEFONT2, IDM_BASELINEFONT3, IDM_BASELINEFONT4,
        // and IDM_BASELINEFONT5 should be consecutive integers;
        //
        public const int IDM_BASELINEFONT1 = 2141;
        public const int IDM_BASELINEFONT2 = 2142;
        public const int IDM_BASELINEFONT3 = 2143;
        public const int IDM_BASELINEFONT4 = 2144;
        public const int IDM_BASELINEFONT5 = 2145;

        public const int IDM_HORIZONTALLINE = 2150;
        public const int IDM_LINEBREAKNORMAL = 2151;
        public const int IDM_LINEBREAKLEFT = 2152;
        public const int IDM_LINEBREAKRIGHT = 2153;
        public const int IDM_LINEBREAKBOTH = 2154;
        public const int IDM_NONBREAK = 2155;
        public const int IDM_SPECIALCHAR = 2156;
        public const int IDM_HTMLSOURCE = 2157;
        public const int IDM_IFRAME = 2158;
        public const int IDM_HTMLCONTAIN = 2159;
        public const int IDM_TEXTBOX = 2161;
        public const int IDM_TEXTAREA = 2162;
        public const int IDM_CHECKBOX = 2163;
        public const int IDM_RADIOBUTTON = 2164;
        public const int IDM_DROPDOWNBOX = 2165;
        public const int IDM_LISTBOX = 2166;
        public const int IDM_BUTTON = 2167;
        public const int IDM_IMAGE = 2168;
        public const int IDM_OBJECT = 2169;
        public const int IDM_1D = 2170;
        public const int IDM_IMAGEMAP = 2171;
        public const int IDM_FILE = 2172;
        public const int IDM_COMMENT = 2173;
        public const int IDM_SCRIPT = 2174;
        public const int IDM_JAVAAPPLET = 2175;
        public const int IDM_PLUGIN = 2176;
        public const int IDM_PAGEBREAK = 2177;
        public const int IDM_HTMLAREA = 2178;

        public const int IDM_PARAGRAPH = 2180;
        public const int IDM_FORM = 2181;
        public const int IDM_MARQUEE = 2182;
        public const int IDM_LIST = 2183;
        public const int IDM_ORDERLIST = 2184;
        public const int IDM_UNORDERLIST = 2185;
        public const int IDM_INDENT = 2186;
        public const int IDM_OUTDENT = 2187;
        public const int IDM_PREFORMATTED = 2188;
        public const int IDM_ADDRESS = 2189;
        public const int IDM_BLINK = 2190;
        public const int IDM_DIV = 2191;

        public const int IDM_TABLEINSERT = 2200;
        public const int IDM_RCINSERT = 2201;
        public const int IDM_CELLINSERT = 2202;
        public const int IDM_CAPTIONINSERT = 2203;
        public const int IDM_CELLMERGE = 2204;
        public const int IDM_CELLSPLIT = 2205;
        public const int IDM_CELLSELECT = 2206;
        public const int IDM_ROWSELECT = 2207;
        public const int IDM_COLUMNSELECT = 2208;
        public const int IDM_TABLESELECT = 2209;
        public const int IDM_TABLEPROPERTIES = 2210;
        public const int IDM_CELLPROPERTIES = 2211;
        public const int IDM_ROWINSERT = 2212;
        public const int IDM_COLUMNINSERT = 2213;

        public const int IDM_HELP_CONTENT = 2220;
        public const int IDM_HELP_ABOUT = 2221;
        public const int IDM_HELP_README = 2222;

        public const int IDM_REMOVEFORMAT = 2230;
        public const int IDM_PAGEINFO = 2231;
        public const int IDM_TELETYPE = 2232;
        public const int IDM_GETBLOCKFMTS = 2233;
        public const int IDM_BLOCKFMT = 2234;
        public const int IDM_SHOWHIDE_CODE = 2235;
        public const int IDM_TABLE = 2236;

        public const int IDM_COPYFORMAT = 2237;
        public const int IDM_PASTEFORMAT = 2238;
        public const int IDM_GOTO = 2239;

        public const int IDM_CHANGEFONT = 2240;
        public const int IDM_CHANGEFONTSIZE = 2241;
        public const int IDM_CHANGECASE = 2246;
        public const int IDM_SHOWSPECIALCHAR = 2249;

        public const int IDM_SUBSCRIPT = 2247;
        public const int IDM_SUPERSCRIPT = 2248;

        public const int IDM_CENTERALIGNPARA = 2250;
        public const int IDM_LEFTALIGNPARA = 2251;
        public const int IDM_RIGHTALIGNPARA = 2252;
        public const int IDM_REMOVEPARAFORMAT = 2253;
        public const int IDM_APPLYNORMAL = 2254;
        public const int IDM_APPLYHEADING1 = 2255;
        public const int IDM_APPLYHEADING2 = 2256;
        public const int IDM_APPLYHEADING3 = 2257;

        public const int IDM_DOCPROPERTIES = 2260;
        public const int IDM_ADDFAVORITES = 2261;
        public const int IDM_COPYSHORTCUT = 2262;
        public const int IDM_SAVEBACKGROUND = 2263;
        public const int IDM_SETWALLPAPER = 2264;
        public const int IDM_COPYBACKGROUND = 2265;
        public const int IDM_CREATESHORTCUT = 2266;
        public const int IDM_PAGE = 2267;
        public const int IDM_SAVETARGET = 2268;
        public const int IDM_SHOWPICTURE = 2269;
        public const int IDM_SAVEPICTURE = 2270;
        public const int IDM_DYNSRCPLAY = 2271;
        public const int IDM_DYNSRCSTOP = 2272;
        public const int IDM_PRINTTARGET = 2273;
        public const int IDM_IMGARTPLAY = 2274;
        public const int IDM_IMGARTSTOP = 2275;
        public const int IDM_IMGARTREWIND = 2276;
        public const int IDM_PRINTQUERYJOBSPENDING = 2277;
        public const int IDM_SETDESKTOPITEM = 2278;
        public const int IDM_CONTEXTMENU = 2280;
        public const int IDM_GOBACKWARD = 2282;
        public const int IDM_GOFORWARD = 2283;
        public const int IDM_PRESTOP = 2284;

        public const int IDM_MP_MYPICS = 2287;
        public const int IDM_MP_EMAILPICTURE = 2288;
        public const int IDM_MP_PRINTPICTURE = 2289;

        public const int IDM_CREATELINK = 2290;
        public const int IDM_COPYCONTENT = 2291;

        public const int IDM_LANGUAGE = 2292;

        public const int IDM_GETPRINTTEMPLATE = 2295;
        public const int IDM_SETPRINTTEMPLATE = 2296;
        public const int IDM_TEMPLATE_PAGESETUP = 2298;

        public const int IDM_REFRESH = 2300;
        public const int IDM_STOPDOWNLOAD = 2301;

        public const int IDM_ENABLE_INTERACTION = 2302;

        public const int IDM_LAUNCHDEBUGGER = 2310;
        public const int IDM_BREAKATNEXT = 2311;

        public const int IDM_INSINPUTHIDDEN = 2312;
        public const int IDM_INSINPUTPASSWORD = 2313;

        public const int IDM_OVERWRITE = 2314;

        public const int IDM_PARSECOMPLETE = 2315;

        public const int IDM_HTMLEDITMODE = 2316;

        public const int IDM_REGISTRYREFRESH = 2317;
        public const int IDM_COMPOSESETTINGS = 2318;

        public const int IDM_SHOWALLTAGS = 2327;
        public const int IDM_SHOWALIGNEDSITETAGS = 2321;
        public const int IDM_SHOWSCRIPTTAGS = 2322;
        public const int IDM_SHOWSTYLETAGS = 2323;
        public const int IDM_SHOWCOMMENTTAGS = 2324;
        public const int IDM_SHOWAREATAGS = 2325;
        public const int IDM_SHOWUNKNOWNTAGS = 2326;
        public const int IDM_SHOWMISCTAGS = 2320;
        public const int IDM_SHOWZEROBORDERATDESIGNTIME = 2328;

        public const int IDM_AUTODETECT = 2329;

        public const int IDM_SCRIPTDEBUGGER = 2330;

        public const int IDM_GETBYTESDOWNLOADED = 2331;

        public const int IDM_NOACTIVATENORMALOLECONTROLS = 2332;
        public const int IDM_NOACTIVATEDESIGNTIMECONTROLS = 2333;
        public const int IDM_NOACTIVATEJAVAAPPLETS = 2334;
        public const int IDM_NOFIXUPURLSONPASTE = 2335;

        public const int IDM_EMPTYGLYPHTABLE = 2336;
        public const int IDM_ADDTOGLYPHTABLE = 2337;
        public const int IDM_REMOVEFROMGLYPHTABLE = 2338;
        public const int IDM_REPLACEGLYPHCONTENTS = 2339;

        public const int IDM_SHOWWBRTAGS = 2340;

        public const int IDM_PERSISTSTREAMSYNC = 2341;
        public const int IDM_SETDIRTY = 2342;

        public const int IDM_RUNURLSCRIPT = 2343;


        public const int IDM_ZOOMRATIO = 2344;
        public const int IDM_GETZOOMNUMERATOR = 2345;
        public const int IDM_GETZOOMDENOMINATOR = 2346;


        // COMMANDS FOR COMPLEX TEXT
        public const int IDM_DIRLTR = 2350;
        public const int IDM_DIRRTL = 2351;
        public const int IDM_BLOCKDIRLTR = 2352;
        public const int IDM_BLOCKDIRRTL = 2353;
        public const int IDM_INLINEDIRLTR = 2354;
        public const int IDM_INLINEDIRRTL = 2355;

        // SHDOCVW
        public const int IDM_ISTRUSTEDDLG = 2356;

        // MSHTMLED
        public const int IDM_INSERTSPAN = 2357;
        public const int IDM_LOCALIZEEDITOR = 2358;

        // XML MIMEVIEWER
        public const int IDM_SAVEPRETRANSFORMSOURCE = 2370;
        public const int IDM_VIEWPRETRANSFORMSOURCE = 2371;

        // Scrollbar context menu
        public const int IDM_SCROLL_HERE = 2380;
        public const int IDM_SCROLL_TOP = 2381;
        public const int IDM_SCROLL_BOTTOM = 2382;
        public const int IDM_SCROLL_PAGEUP = 2383;
        public const int IDM_SCROLL_PAGEDOWN = 2384;
        public const int IDM_SCROLL_UP = 2385;
        public const int IDM_SCROLL_DOWN = 2386;
        public const int IDM_SCROLL_LEFTEDGE = 2387;
        public const int IDM_SCROLL_RIGHTEDGE = 2388;
        public const int IDM_SCROLL_PAGELEFT = 2389;
        public const int IDM_SCROLL_PAGERIGHT = 2390;
        public const int IDM_SCROLL_LEFT = 2391;
        public const int IDM_SCROLL_RIGHT = 2392;

        // IE 6 Form Editing Commands
        public const int IDM_MULTIPLESELECTION = 2393;
        public const int IDM_2D_POSITION = 2394;
        public const int IDM_2D_ELEMENT = 2395;
        public const int IDM_1D_ELEMENT = 2396;
        public const int IDM_ABSOLUTE_POSITION = 2397;
        public const int IDM_LIVERESIZE = 2398;
        public const int IDM_ATOMICSELECTION = 2399;

        // Auto URL detection mode
        public const int IDM_AUTOURLDETECT_MODE = 2400;

        // Legacy IE50 compatible paste
        public const int IDM_IE50_PASTE = 2401;

        // ie50 paste mode
        public const int IDM_IE50_PASTE_MODE = 2402;

        //;begin_public
        public const int IDM_GETIPRINT = 2403;
        //;end_public

        // for disabling selection handles
        public const int IDM_DISABLE_EDITFOCUS_UI = 2404;

        // for visibility/display in design
        public const int IDM_RESPECTVISIBILITY_INDESIGN = 2405;

        // set css mode
        public const int IDM_CSSEDITING_LEVEL = 2406;

        // New outdent
        public const int IDM_UI_OUTDENT = 2407;

        // Printing Status
        public const int IDM_UPDATEPAGESTATUS = 2408;

        // IME Reconversion 
        public const int IDM_IME_ENABLE_RECONVERSION = 2409;

        public const int IDM_KEEPSELECTION = 2410;

        public const int IDM_UNLOADDOCUMENT = 2411;

        public const int IDM_OVERRIDE_CURSOR = 2420;

        public const int IDM_PEERHITTESTSAMEINEDIT = 2423;

        public const int IDM_TRUSTAPPCACHE = 2425;

        public const int IDM_BACKGROUNDIMAGECACHE = 2430;

        public const int IDM_DEFAULTBLOCK = 6046;

        public const int IDM_MIMECSET__FIRST__ = 3609;
        public const int IDM_MIMECSET__LAST__ = 3699;

        public const int IDM_MENUEXT_FIRST__ = 3700;
        public const int IDM_MENUEXT_LAST__ = 3732;
        public const int IDM_MENUEXT_COUNT = 3733;

        // Commands mapped from the standard set.  We should
        // consider deleting them from public header files.

        public const int IDM_OPEN = 2000;
        public const int IDM_NEW = 2001;
        public const int IDM_SAVE = 70;
        public const int IDM_SAVEAS = 71;
        public const int IDM_SAVECOPYAS = 2002;
        public const int IDM_PRINTPREVIEW = 2003;
        public const int IDM_SHOWPRINT = 2010;
        public const int IDM_SHOWPAGESETUP = 2011;
        public const int IDM_PRINT = 27;
        public const int IDM_PAGESETUP = 2004;
        public const int IDM_SPELL = 2005;
        public const int IDM_PASTESPECIAL = 2006;
        public const int IDM_CLEARSELECTION = 2007;
        public const int IDM_PROPERTIES = 28;
        public const int IDM_REDO = 29;
        public const int IDM_UNDO = 43;
        public const int IDM_SELECTALL = 31;
        public const int IDM_ZOOMPERCENT = 50;
        public const int IDM_GETZOOM = 68;
        public const int IDM_STOP = 2138;
        public const int IDM_COPY = 15;
        public const int IDM_CUT = 16;
        public const int IDM_PASTE = 26;

        // Defines for IDM_ZOOMPERCENT
        public const int CMD_ZOOM_PAGEWIDTH = -1;
        public const int CMD_ZOOM_ONEPAGE = -2;
        public const int CMD_ZOOM_TWOPAGES = -3;
        public const int CMD_ZOOM_SELECTION = -4;
        public const int CMD_ZOOM_FIT = -5;

        // IDMs for CGID_EditStateCommands group 
        public const int IDM_CONTEXT = 1;
        public const int IDM_HWND = 2;

        // Shdocvw Execs on CGID_DocHostCommandHandler
        public const int IDM_NEW_TOPLEVELWINDOW = 7050;


        //DISPID_AMBIENT_DLCONTROL constants
        [CLSCompliant(false)]
        public const uint DLCTL_DLIMAGES = 0x00000010;
        [CLSCompliant(false)]
        public const uint DLCTL_VIDEOS = 0x00000020;
        [CLSCompliant(false)]
        public const uint DLCTL_BGSOUNDS = 0x00000040;
        [CLSCompliant(false)]
        public const uint DLCTL_NO_SCRIPTS = 0x00000080;
        [CLSCompliant(false)]
        public const uint DLCTL_NO_JAVA = 0x00000100;
        [CLSCompliant(false)]
        public const uint DLCTL_NO_RUNACTIVEXCTLS = 0x00000200;
        [CLSCompliant(false)]
        public const uint DLCTL_NO_DLACTIVEXCTLS = 0x00000400;
        [CLSCompliant(false)]
        public const uint DLCTL_DOWNLOADONLY = 0x00000800;
        [CLSCompliant(false)]
        public const uint DLCTL_NO_FRAMEDOWNLOAD = 0x00001000;
        [CLSCompliant(false)]
        public const uint DLCTL_RESYNCHRONIZE = 0x00002000;
        [CLSCompliant(false)]
        public const uint DLCTL_PRAGMA_NO_CACHE = 0x00004000;
        [CLSCompliant(false)]
        public const uint DLCTL_NO_BEHAVIORS = 0x00008000;
        [CLSCompliant(false)]
        public const uint DLCTL_NO_METACHARSET = 0x00010000;
        [CLSCompliant(false)]
        public const uint DLCTL_URL_ENCODING_DISABLE_UTF8 = 0x00020000;
        [CLSCompliant(false)]
        public const uint DLCTL_URL_ENCODING_ENABLE_UTF8 = 0x00040000;
        [CLSCompliant(false)]
        public const uint DLCTL_NOFRAMES = 0x00080000;
        [CLSCompliant(false)]
        public const uint DLCTL_FORCEOFFLINE = 0x10000000;
        [CLSCompliant(false)]
        public const uint DLCTL_NO_CLIENTPULL = 0x20000000;
        [CLSCompliant(false)]
        public const uint DLCTL_SILENT = 0x40000000;
        [CLSCompliant(false)]
        public const uint DLCTL_OFFLINEIFNOTCONNECTED = 0x80000000;
        [CLSCompliant(false)]
        public const uint DLCTL_OFFLINE = 0x80000000;



        public const int OLECLOSE_NOSAVE = 1;

        public const int OLEIVERB_PRIMARY = (0);
        public const int OLEIVERB_SHOW = (-1);
        public const int OLEIVERB_OPEN = (-2);
        public const int OLEIVERB_HIDE = (-3);
        public const int OLEIVERB_UIACTIVATE = (-4);
        public const int OLEIVERB_INPLACEACTIVATE = (-5);
        public const int OLEIVERB_DISCARDUNDOSTATE = (-6);

        public const int DOCHOSTUIDBLCLK_DEFAULT = 0;
        public const int DOCHOSTUIDBLCLK_SHOWPROPERTIES = 1;
        public const int DOCHOSTUIDBLCLK_SHOWCODE = 2;

        //
        // Undo persistence comands
        //
        public const int IDM_PRESERVEUNDOALWAYS = 6049;
        public const int IDM_PERSISTDEFAULTVALUES = 7100;
        public const int IDM_PROTECTMETATAGS = 7101;

        public const int IDM_GETFRAMEZONE = 6037;

        public const int IDM_FIRE_PRINTTEMPLATEUP = 15000;
        public const int IDM_FIRE_PRINTTEMPLATEDOWN = 15001;
        public const int IDM_SETPRINTHANDLES = 15002;

        public const int STGM_READ = 0x00000000;
        public const int STGM_WRITE = 0x00000001;
        public const int STGM_READWRITE = 0x00000002;

        //error codes
        public const int OLE_E_FIRST = -2147221504;
        public const int OLE_E_LAST = -2147221249;

        public const int OLECMDERR_E_FIRST = (OLE_E_LAST + 1);
        public const int OLECMDERR_E_NOTSUPPORTED = (OLECMDERR_E_FIRST); //-2147221248
        public const int OLECMDERR_E_DISABLED = (OLECMDERR_E_FIRST + 1); //-2147221247
        public const int OLECMDERR_E_NOHELP = (OLECMDERR_E_FIRST + 2); //-2147221246
        public const int OLECMDERR_E_CANCELED = (OLECMDERR_E_FIRST + 3); //-2147221245
        public const int OLECMDERR_E_UNKNOWNGROUP = (OLECMDERR_E_FIRST + 4); //-2147221244

        public const int MSOCMDERR_E_FIRST = OLECMDERR_E_FIRST;
        public const int MSOCMDERR_E_NOTSUPPORTED = OLECMDERR_E_NOTSUPPORTED;
        public const int MSOCMDERR_E_DISABLED = OLECMDERR_E_DISABLED;
        public const int MSOCMDERR_E_NOHELP = OLECMDERR_E_NOHELP;
        public const int MSOCMDERR_E_CANCELED = OLECMDERR_E_CANCELED;
        public const int MSOCMDERR_E_UNKNOWNGROUP = OLECMDERR_E_UNKNOWNGROUP;



        //win32 functions
        internal const int WM_SETFOCUS = 0x7;
        internal const int WM_MOUSEACTIVATE = 0x21;
        internal const int WM_PARENTNOTIFY = 0x210;
        internal const int WM_ACTIVATE = 0x6;
        internal const int WM_KILLFOCUS = 0x8;
        internal const int WM_CLOSE = 0x10;
        internal const int WM_DESTROY = 0x2;
        internal const int WM_KEYDOWN = 0x100;
        internal const int WM_KEYUP = 0x101;
        internal const int WM_LBUTTONDOWN = 0x0201;
        internal const int WM_LBUTTONUP = 0x0202;
        internal const int WM_LBUTTONDBLCLK = 0x0203;
        internal const int WM_RBUTTONDOWN = 0x0204;
        internal const int WM_RBUTTONUP = 0x0205;
        internal const int WM_RBUTTONDBLCLK = 0x0206;
        internal const int WM_MBUTTONDOWN = 0x0207;
        internal const int WM_MBUTTONUP = 0x0208;
        internal const int WM_MBUTTONDBLCLK = 0x0209;
        internal const int WM_XBUTTONDOWN = 0x020B;
        internal const int WM_XBUTTONUP = 0x020C;
        internal const int WM_MOUSEMOVE = 0x0200;
        internal const int WM_MOUSELEAVE = 0x02A3;
        internal const int WM_MOUSEHOVER = 0x02A1;
        internal const int MK_LBUTTON = 0x0001;
        internal const int MK_RBUTTON = 0x0002;
        internal const int MK_SHIFT = 0x0004;
        internal const int MK_CONTROL = 0x0008;
        internal const int MK_MBUTTON = 0x0010;
        internal const int MK_XBUTTON1 = 0x0020;
        internal const int MK_XBUTTON2 = 0x0040;

        internal const int TTM_ADJUSTRECT = 0x400 + 31;
        internal const uint TTF_CENTERTIP = 0x2;
        internal const uint TTF_TRANSPARENT = 0x100;
        internal const uint WS_POPUP = 0x80000000;
        internal const int WS_EX_LAYERED = 0x80000;
        internal const int WS_EX_TOPMOST = 0x00000008;
        internal const int TTS_BALLOON = 0x40;
        internal const int TTS_NOPREFIX = 0x2;
        internal const uint TTM_ADDTOOL = (0x400 + 4);
        internal const int TTS_NOANIMATE = 0x10;
        internal const int TTS_NOFADE = 0x20;
        internal const int GWL_STYLE = -16;
        internal const int LWA_ALPHA = 0x2;
        internal const int HWND_TOPMOST = -1;
        internal const int TTM_SETMAXTIPWIDTH = 0x400 + 24;
        internal const int TTM_ACTIVATE = 0x400 + 1;
        internal const int TTM_SETTIPBKCOLOR = 0x400 + 19;
        internal const int TTM_SETTIPTEXTCOLOR = 0x400 + 20;
        internal const int TTM_SETMARGIN = 0x400 + 26;
        internal const int TTM_SETTITLE = 0x400 + 32;
        internal const int TTS_ALWAYSTIP = 0x1;
        internal const int TTF_SUBCLASS = 0x0010;
        internal const byte SW_SHOWMINNOACTIVE = 7;
        internal const byte SW_SHOW = 5;
        internal const byte SW_HIDE = 0;
        internal const int TTM_WINDOWFROMPOINT = (0x400 + 16);
        #endregion

        #region Structs

        [ComVisible(true), StructLayout(LayoutKind.Sequential)]
        public sealed class FORMATETC
        {
            [MarshalAs(UnmanagedType.I4)]
            public int cfFormat;
            public IntPtr ptd;
            [MarshalAs(UnmanagedType.I4)]
            public int dwAspect;
            [MarshalAs(UnmanagedType.I4)]
            public int lindex;
            [MarshalAs(UnmanagedType.I4)]
            public int tymed;

        }

        [ComVisible(true), StructLayout(LayoutKind.Sequential)]
        public class STGMEDIUM
        {
            [MarshalAs(UnmanagedType.I4)]
            public int tymed;
            public IntPtr unionmember;
            public IntPtr pUnkForRelease;
        }

        [ComVisible(false), StructLayout(LayoutKind.Sequential)]
        public sealed class LOGPALETTE
        {
            [MarshalAs(UnmanagedType.U2)/*leftover(offset=0, palVersion)*/]
            public short palVersion;

            [MarshalAs(UnmanagedType.U2)/*leftover(offset=2, palNumEntries)*/]
            public short palNumEntries;

            // UNMAPPABLE: palPalEntry: Cannot be used as a structure field.
            //   /** @com.structmap(UNMAPPABLE palPalEntry) */
            //  public UNMAPPABLE palPalEntry;
        }

        [ComVisible(false), StructLayout(LayoutKind.Sequential)]
        public class OIFI
        {
            [MarshalAs(UnmanagedType.U4)]
            public int cb;
            [MarshalAs(UnmanagedType.I4)]
            public int fMDIApp;
            public IntPtr hwndFrame;
            public IntPtr hAccel;
            [MarshalAs(UnmanagedType.U4)]
            public int cAccelEntries;
        }

        [ComVisible(false), StructLayout(LayoutKind.Sequential)]
        public class OleMenuGroupWidths
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            public int[] widths = new int[6];
        }

        [ComVisible(true), StructLayout(LayoutKind.Sequential)]
        public struct OLECMD
        {
            [MarshalAs(UnmanagedType.U4)]
            public int cmdID;
            [MarshalAs(UnmanagedType.U4)]
            public int cmdf;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct OLECMDTEXT
        {
            [CLSCompliant(false)]
            public UInt32 cmdtextf;
            [CLSCompliant(false)]
            public UInt32 cwActual;
            [CLSCompliant(false)]
            public UInt32 cwBuf;
            public Char rgwz;
        }

        [ComVisible(true), StructLayout(LayoutKind.Sequential)]
        public class DOCHOSTUIINFO
        {
            [MarshalAs(UnmanagedType.U4)]
            public int cbSize = 0;
            [MarshalAs(UnmanagedType.I4)]
            public int dwFlags = 0;
            [MarshalAs(UnmanagedType.I4)]
            public int dwDoubleClick = 0;
            [MarshalAs(UnmanagedType.I4)]
            public int dwReserved1 = 0;
            [MarshalAs(UnmanagedType.I4)]
            public int dwReserved2 = 0;
        }

        [ComVisible(false), StructLayout(LayoutKind.Sequential)]
        public sealed class STATDATA
        {

            [MarshalAs(UnmanagedType.U4)]
            public int advf = 0;
            [MarshalAs(UnmanagedType.U4)]
            public int dwConnection = 0;

        }

        [ComVisible(false), StructLayout(LayoutKind.Sequential)]
        public sealed class tagOLEVERB
        {
            [MarshalAs(UnmanagedType.I4)]
            public int lVerb = 0;

            [MarshalAs(UnmanagedType.LPWStr)]
            public String lpszVerbName = string.Empty;

            [MarshalAs(UnmanagedType.U4)]
            public int fuFlags = 0;

            [MarshalAs(UnmanagedType.U4)]
            public int grfAttribs = 0;

        }

        #endregion

        #region Functions

        [DllImport("ole32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern int CreateBindCtx(int dwReserved, [Out] out IBindCtx ppbc);

        [DllImport("urlmon.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
        public static extern int CreateURLMoniker(IMoniker pmkContext, string szURL, [Out] out IMoniker ppmk);

        [DllImport("ole32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern int OleRun([In, MarshalAs(UnmanagedType.IUnknown)] object pUnknown);

        [DllImport("ole32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern int OleLockRunning(
            [In, MarshalAs(UnmanagedType.IUnknown)] object pUnknown,
            [In, MarshalAs(UnmanagedType.Bool)] bool flock,
            [In, MarshalAs(UnmanagedType.Bool)] bool fLastUnlockCloses
            );

        #endregion

        #region Enums

        public enum DOCHOSTUIFLAG 
        {
            DIALOG                    = 0x00000001,
            DISABLE_HELP_MENU         = 0x00000002,
            NO3DBORDER                = 0x00000004,
            SCROLL_NO                 = 0x00000008,
            DISABLE_SCRIPT_INACTIVE   = 0x00000010,
            OPENNEWWIN                = 0x00000020,
            DISABLE_OFFSCREEN         = 0x00000040,
            FLAT_SCROLLBAR            = 0x00000080,
            DIV_BLOCKDEFAULT          = 0x00000100,
            ACTIVATE_CLIENTHIT_ONLY   = 0x00000200,
            OVERRIDEBEHAVIORFACTORY   = 0x00000400,
            CODEPAGELINKEDFONTS       = 0x00000800,
            URL_ENCODING_DISABLE_UTF8 = 0x00001000,
            URL_ENCODING_ENABLE_UTF8  = 0x00002000,
            ENABLE_FORMS_AUTOCOMPLETE = 0x00004000,
            ENABLE_INPLACE_NAVIGATION = 0x00010000,
            IME_ENABLE_RECONVERSION   = 0x00020000,
            THEME                     = 0x00040000,
            NOTHEME                   = 0x00080000,
            NOPICS                    = 0x00100000,
            NO3DOUTERBORDER           = 0x00200000,
            DELEGATESIDOFDISPATCH     = 0x00400000
        }

        public enum SELECTION_TYPE 
        {
            None = 0,
            Caret = 1,
            Text = 2,
            Control = 3,
            Max = 2147483647
        }

        public enum OLECMDF
        { 
            SUPPORTED    = 1, 
            ENABLED      = 2, 
            LATCHED      = 4, 
            NINCHED      = 8 
        }

        public enum OLECMDID
        {
            OPEN                           = 1,
            NEW                            = 2,
            SAVE                           = 3,
            SAVEAS                         = 4,
            SAVECOPYAS                     = 5,
            PRINT                          = 6,
            PRINTPREVIEW                   = 7,
            PAGESETUP                      = 8,
            SPELL                          = 9,
            PROPERTIES                     = 10,
            CUT                            = 11,
            COPY                           = 12,
            PASTE                          = 13,
            PASTESPECIAL                   = 14,
            UNDO                           = 15,
            REDO                           = 16,
            SELECTALL                      = 17,
            CLEARSELECTION                 = 18,
            ZOOM                           = 19,
            GETZOOMRANGE                   = 20,
            UPDATECOMMANDS                 = 21,
            REFRESH                        = 22,
            STOP                           = 23,
            HIDETOOLBARS                   = 24,
            SETPROGRESSMAX                 = 25,
            SETPROGRESSPOS                 = 26,
            SETPROGRESSTEXT                = 27,
            SETTITLE                       = 28,
            SETDOWNLOADSTATE               = 29,
            STOPDOWNLOAD                   = 30,
            ONTOOLBARACTIVATED             = 31,
            FIND                           = 32,
            DELETE                         = 33,
            HTTPEQUIV                      = 34,
            HTTPEQUIV_DONE                 = 35,
            ENABLE_INTERACTION             = 36,
            ONUNLOAD                       = 37,
            PROPERTYBAG2                   = 38,
            PREREFRESH                     = 39,
            SHOWSCRIPTERROR                = 40,
            SHOWMESSAGE	                = 41,
            SHOWFIND   	                = 42,
            SHOWPAGESETUP                  = 43,
            SHOWPRINT                      = 44,
            CLOSE                          = 45,
            ALLOWUILESSSAVEAS              = 46,
            DONTDOWNLOADCSS                = 47,
            UPDATEPAGESTATUS               = 48,
            PRINT2                         = 49,
            PRINTPREVIEW2                  = 50,
            SETPRINTTEMPLATE               = 51,
            GETPRINTTEMPLATE               = 52,
        }

        public enum OLECMDEXECOPT 
        {
            DODEFAULT         = 0,
            PROMPTUSER        = 1,
            DONTPROMPTUSER    = 2,
            SHOWHELP          = 3
        } 

        public enum OLECLOSE 
        {
            SAVEIFDIRTY = 0, 
            NOSAVE = 1, 
            PROMPTSAVE = 2 
        }
        #endregion
    }


    #region WinAPI Interfaces

    #region ConnectionPointCookie
    [ComVisible(false)]
        public class ConnectionPointCookie 
    {
        private IConnectionPoint connectionPoint;
        private int cookie;

        // UNDONE: Should cookie be IntPtr?

        /// <summary>
        /// Creates a connection point to of the given interface type.
        /// which will call on a managed code sink that implements that interface.
        /// </summary>
        /// <param name='source'>
        /// The object that exposes the events.  This object must implement IConnectionPointContainer or an InvalidCastException will be thrown.
        /// </param>
        /// <param name='sink'>
        /// The object to sink the events.  This object must implement the interface eventInterface, or an InvalidCastException is thrown.
        /// </param>
        /// <param name='eventInterface'>
        /// The event interface to sink.  The sink object must support this interface and the source object must expose it through it's ConnectionPointContainer.
        /// </param>
        public ConnectionPointCookie(object source, object sink, Type eventInterface) :
            this(source, sink, eventInterface, true) 
        {
        }


        /// <summary>
        /// Creates a connection point to of the given interface type.
        /// which will call on a managed code sink that implements that interface.
        /// </summary>
        /// <param name='source'>
        /// The object that exposes the events.  This object must implement IConnectionPointContainer or an InvalidCastException will be thrown.
        /// </param>
        /// <param name='sink'>
        /// The object to sink the events.  This object must implement the interface eventInterface, or an InvalidCastException is thrown.
        /// </param>
        /// <param name='eventInterface'>
        /// The event interface to sink.  The sink object must support this interface and the source object must expose it through it's ConnectionPointContainer.
        /// </param>
        /// <param name='throwException'>
        /// If true, exceptions described will be thrown, otherwise object will silently fail to connect.
        /// </param>
        public ConnectionPointCookie(object source, object sink, Type eventInterface, bool throwException) 
        {
            Exception ex = null;
            if (source is IConnectionPointContainer) 
            {
                IConnectionPointContainer cpc = (IConnectionPointContainer)source;

                try 
                {
                    Guid tmp = eventInterface.GUID;
                    cpc.FindConnectionPoint(ref tmp, out connectionPoint);
                } 
                catch(Exception) 
                {
                    connectionPoint = null;
                }

                if (connectionPoint == null) 
                {
                    ex = new ArgumentException("The source object does not expose the " + eventInterface.Name + " event inteface");
                }
                else if (!eventInterface.IsInstanceOfType(sink)) 
                {
                    ex = new InvalidCastException("The sink object does not implement the eventInterface");
                }
                else 
                {
                    try 
                    {
                        connectionPoint.Advise(sink, out cookie);
                    }
                    catch 
                    {
                        cookie = 0;
                        connectionPoint = null;
                        ex = new Exception("IConnectionPoint::Advise failed for event interface '" + eventInterface.Name + "'");
                    }
                }
            }
            else 
            {
                ex = new InvalidCastException("The source object does not expost IConnectionPointContainer");
            }

            if (throwException && (connectionPoint == null || cookie == 0)) 
            {
                if (ex == null) 
                {
                    throw new ArgumentException("Could not create connection point for event interface '" + eventInterface.Name + "'");
                }
                else 
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Disconnect the current connection point.  If the object is not connected,
        /// this method will do nothing.
        /// </summary>
        public void Disconnect() 
        {
            if (connectionPoint != null && cookie != 0) 
            {
                connectionPoint.Unadvise(cookie);
                cookie = 0;
                connectionPoint = null;
            }
        }

        ~ConnectionPointCookie() 
        {
            Disconnect();
        }
    }
    #endregion

    #region IPropertyNotifySink
    [ComVisible(true), Guid("9BFBBC02-EFF1-101A-84ED-00AA00341D07"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IPropertyNotifySink 
    {
        void OnChanged(int dispID);

        void OnRequestEdit(int dispID);
    }
    #endregion

    #region IDocHostShowUI
    [NDependIgnore]
    [ComVisible(true), ComImport(),
        Guid("C4D244B0-D43E-11CF-893B-00AA00BDCE1A"),
        InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
        internal interface IDocHostShowUI
    {
        [NDependIgnoreLongMethod]
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int ShowMessage(
            [In] IntPtr hwnd,[In][MarshalAs(UnmanagedType.LPWStr)] String lpStrText,
            [In][MarshalAs(UnmanagedType.LPWStr)] String lpstrCaption,
            [In] uint dwType,[In][MarshalAs(UnmanagedType.LPWStr)] String
            lpStrHelpFile,[In] uint dwHelpContext,
            [Out] IntPtr lpresult);

        [NDependIgnoreLongMethod]
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int ShowHelp(
            [In] IntPtr hwnd,
            [In][MarshalAs(UnmanagedType.LPWStr)] String lpHelpFile,
            [In] uint uCommand,
            [In] uint dwData,
            [In] NativeMethods.POINT ptMouse,
            [Out][MarshalAs(UnmanagedType.IDispatch)] Object pDispatchObjectHit);
    }
    #endregion

    #region IOleCommandTarget
    [ComVisible(true), ComImport(),
        Guid("b722bccb-4e68-101b-a2bc-00aa00404770"),
        InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IOleCommandTarget 
    {
        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int QueryStatus(
            ref Guid pguidCmdGroup,
            int cCmds,
            [In, Out, MarshalAs(UnmanagedType.LPArray)] OleApi.OLECMD[] prgCmds,
            [In, Out] OleApi.OLECMDTEXT pCmdText);

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int Exec(
            ref Guid pguidCmdGroup,
            int nCmdID,
            int nCmdexecopt,
            [In, MarshalAs(UnmanagedType.LPArray)] object[] pvaIn,
            [Out, MarshalAs(UnmanagedType.LPArray)] object[] pvaOut);
    }
    #endregion

    #region IPersisMoniker
    [ComVisible(true), Guid("79eac9c9-baf9-11ce-8c82-00aa004ba90b"),
        InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IPersistMoniker 
    {
        void GetClassID([In,Out] ref Guid pClassID);
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int IsDirty();
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int Load([In] int fFullyAvailable, [In,
            MarshalAs(UnmanagedType.Interface)] IMoniker pmk, 
            [In,MarshalAs(UnmanagedType.Interface)] IBindCtx pbc, 
            [In] int grfMode);
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int Save([In, MarshalAs(UnmanagedType.Interface)] IMoniker pmk,
            [In, MarshalAs(UnmanagedType.Interface)] IBindCtx pbc,
            [In] int fRemember);
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int SaveCompleted([In, MarshalAs(UnmanagedType.Interface)] IMoniker pmk,
            [In, MarshalAs(UnmanagedType.Interface)] Object pbc);
        [return: MarshalAs(UnmanagedType.Interface)] IMoniker GetCurMoniker();
    }
    #endregion

    #region IAdviseSink
    [ComVisible(true), Guid("0000010f-0000-0000-C000-000000000046"),
        InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IAdviseSink 
    {
        void OnDataChange(
            [In] OleApi.FORMATETC pFormatetc,
           [In] OleApi.STGMEDIUM pStgmed
            );
        void OnViewChange(
            [In,MarshalAs(UnmanagedType.U4)] int dwAspect,
            [In,MarshalAs(UnmanagedType.I4)] int lindex
            );
        void OnRename(
            [In, MarshalAs(UnmanagedType.Interface)] IMoniker pmk 
            );
        void OnSave();
        void OnClose();
    }
    #endregion

    #region IPersistStreamInit
    [ComVisible(true), ComImport(),
        Guid("7FD52380-4E07-101B-AE2D-08002B2EC713"),
        InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IPersistStreamInit 
    {
        void GetClassID([In, Out] ref Guid pClassID);
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int IsDirty();
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int Load([In] IStream pstm);
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int Save([In] IStream pstm, [In,
            MarshalAs(UnmanagedType.Bool)] bool ClearDirty);
        void GetSizeMax([Out] long pcbSize);
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int InitNew();
    }
    #endregion

    #region IEnumOLEVERB
    [ComVisible(true), ComImport(), Guid("00000104-0000-0000-C000-000000000046"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
        internal interface IEnumOLEVERB 
    {

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int Next(
            [MarshalAs(UnmanagedType.U4)]
            int celt,
            [Out]
            OleApi.tagOLEVERB rgelt,
            [Out, MarshalAs(UnmanagedType.LPArray)]
            int[] pceltFetched);

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int Skip(
            [In, MarshalAs(UnmanagedType.U4)]
            int celt);


        void Reset();


        void Clone(
            out IEnumOLEVERB ppenum);
    }
    #endregion

    #region IEnumSTATDATA
    [ComVisible(true), Guid("00000105-0000-0000-C000-000000000046"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IEnumSTATDATA 
    {
        void Next(
            [In, MarshalAs(UnmanagedType.U4)]
            int celt,
            [Out]
            OleApi.STATDATA rgelt,
            [Out, MarshalAs(UnmanagedType.LPArray)]
            int[] pceltFetched);

        void Skip(
            [In, MarshalAs(UnmanagedType.U4)]
            int celt);

        void Reset();

        void Clone(
            [Out, MarshalAs(UnmanagedType.LPArray)]
            IEnumSTATDATA[] ppenum);
    }
    #endregion

    #region IOleClientSite
    [ComVisible(true), Guid("00000118-0000-0000-C000-000000000046"),
        InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
        internal interface IOleClientSite 
    {
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int SaveObject();
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int GetMoniker([In, MarshalAs(UnmanagedType.U4)] uint dwAssign, 
            [In,MarshalAs(UnmanagedType.U4)] uint dwWhichMoniker, 
            [Out,MarshalAs(UnmanagedType.Interface)] out IMoniker ppmk);
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int GetContainer([Out] out IOleContainer ppContainer);
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int ShowObject();
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int OnShowWindow([In, MarshalAs(UnmanagedType.Bool)] bool fShow);
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int RequestNewObjectLayout();
    }
    #endregion

    #region IOleContainer
    [ComVisible(true), Guid("0000011B-0000-0000-C000-000000000046"), System.CLSCompliant(false),
        InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IOleContainer 
    {
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int ParseDisplayName([In, MarshalAs(UnmanagedType.Interface)] Object pbc,
            [In, MarshalAs(UnmanagedType.LPWStr)] String pszDisplayName, [Out,
            MarshalAs(UnmanagedType.LPArray)] int[] pchEaten, [Out,
            MarshalAs(UnmanagedType.LPArray)] Object[] ppmkOut);
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int EnumObjects([In, MarshalAs(UnmanagedType.U4)] uint grfFlags, [Out,
            MarshalAs(UnmanagedType.LPArray)] Object[] ppenum);
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int LockContainer([In, MarshalAs(UnmanagedType.Bool)] Boolean fLock);
    }
    #endregion

    #region IDocHostUIHandler
    [ComVisible(true), ComImport(),
        Guid("BD3F23C0-D43E-11CF-893B-00AA00BDCE1A"),
        InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
        internal interface IDocHostUIHandler 
    {
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int ShowContextMenu(uint dwID, ref NativeMethods.POINT ppt,
            [MarshalAs(UnmanagedType.IUnknown)] object pcmdtReserved,
            [MarshalAs(UnmanagedType.IDispatch)]object pdispReserved);

        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
            int GetHostInfo([In, Out] OleApi.DOCHOSTUIINFO info);

        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int ShowUI([In, MarshalAs(UnmanagedType.I4)] int dwID, [In,
            MarshalAs(UnmanagedType.Interface)] IOleInPlaceActiveObject activeObject,
            [In, MarshalAs(UnmanagedType.Interface)] IOleCommandTarget
            commandTarget, [In, MarshalAs(UnmanagedType.Interface)] IOleInPlaceFrame
            frame, [In, MarshalAs(UnmanagedType.Interface)] IOleInPlaceUIWindow doc);

        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int HideUI();

        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int UpdateUI();

        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int EnableModeless([In, MarshalAs(UnmanagedType.Bool)] Boolean fEnable);

        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int OnDocWindowActivate([In, MarshalAs(UnmanagedType.Bool)] bool fActivate);

        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int OnFrameWindowActivate([In, MarshalAs(UnmanagedType.Bool)] bool
            fActivate);

        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int ResizeBorder([In] NativeMethods.RECT prcBorder, [In, MarshalAs(UnmanagedType.Interface)] IntPtr pUIWindow, [In,
            MarshalAs(UnmanagedType.Bool)] Boolean fFrameWindow);

        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int TranslateAccelerator([In] NativeMethods.MSG msg, [In] ref Guid group, [In,
            MarshalAs(UnmanagedType.I4)] int nCmdID);
		
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int GetOptionKeyPath(out IntPtr pbstrKey, 
            [In, MarshalAs(UnmanagedType.U4)] uint dw);
			
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int GetDropTarget([In, MarshalAs(UnmanagedType.Interface)] IOleDropTarget
            pDropTarget, [Out, MarshalAs(UnmanagedType.Interface)] out IOleDropTarget ppDropTarget);

        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int GetExternal([Out, MarshalAs(UnmanagedType.Interface)] out Object
            ppDispatch);

        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int TranslateUrl([In, MarshalAs(UnmanagedType.U4)] int dwTranslate, [In,
            MarshalAs(UnmanagedType.LPWStr)] string strURLIn,
            [Out, MarshalAs(UnmanagedType.LPWStr)] out String pstrURLOut);

        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int FilterDataObject([In, MarshalAs(UnmanagedType.Interface)] IOleDataObject pDO,
            [Out, MarshalAs(UnmanagedType.Interface)] out IOleDataObject ppDORet);
    }
    #endregion

    #region IOleDropTarget
    [ComVisible(true), ComImport(), Guid("00000122-0000-0000-C000-000000000046"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IOleDropTarget 
    {
        [PreserveSig]
        int OleDragEnter(
            //[In, MarshalAs(UnmanagedType.Interface)]
            IntPtr pDataObj,
            [In, MarshalAs(UnmanagedType.U4)]
            int grfKeyState,
            [In, MarshalAs(UnmanagedType.U8)]
            long pt,
            [In, Out]
            ref int pdwEffect);

        [PreserveSig]
        int OleDragOver(
            [In, MarshalAs(UnmanagedType.U4)]
            int grfKeyState,
            [In, MarshalAs(UnmanagedType.U8)]
            long pt,
            [In, Out]
            ref int pdwEffect);

        [PreserveSig]
        int OleDragLeave();

        [PreserveSig]
        int OleDrop(
            //[In, MarshalAs(UnmanagedType.Interface)]
            IntPtr pDataObj,
            [In, MarshalAs(UnmanagedType.U4)]
            int grfKeyState,
            [In, MarshalAs(UnmanagedType.U8)]
            long pt,
            [In, Out]
            ref int pdwEffect);
    }
    #endregion

    #region IOleDataObject
    [ComVisible(true), ComImport(), Guid("0000010E-0000-0000-C000-000000000046"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IOleDataObject 
    {

        int OleGetData(
            OleApi.FORMATETC pFormatetc,
            [Out]
            OleApi.STGMEDIUM pMedium);


        int OleGetDataHere(
            OleApi.FORMATETC pFormatetc,
            [In, Out]
            OleApi.STGMEDIUM pMedium);


        int OleQueryGetData(
            OleApi.FORMATETC pFormatetc);


        int OleGetCanonicalFormatEtc(
            OleApi.FORMATETC pformatectIn,
            [Out]
            OleApi.FORMATETC pformatetcOut);


        int OleSetData(
            OleApi.FORMATETC pFormatectIn,
            OleApi.STGMEDIUM pmedium,

            int fRelease);

        [return: MarshalAs(UnmanagedType.Interface)]
        IEnumFORMATETC OleEnumFormatEtc(
            [In, MarshalAs(UnmanagedType.U4)]
            int dwDirection);

        int OleDAdvise(
            OleApi.FORMATETC pFormatetc,
            [In, MarshalAs(UnmanagedType.U4)]
            int advf,
            [In, MarshalAs(UnmanagedType.Interface)]
            object pAdvSink,
            [Out, MarshalAs(UnmanagedType.LPArray)]
            int[] pdwConnection);

        int OleDUnadvise(
            [In, MarshalAs(UnmanagedType.U4)]
            int dwConnection);

        int OleEnumDAdvise(
            [Out, MarshalAs(UnmanagedType.LPArray)]
            object[] ppenumAdvise);
    }
    #endregion

    #region IEnumFORMATETC
    [ComVisible(true), ComImport(), Guid("00000103-0000-0000-C000-000000000046"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IEnumFORMATETC 
    {

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int Next(
            [In, MarshalAs(UnmanagedType.U4)]
            int celt,
            [Out]
            OleApi.FORMATETC rgelt,
            [In, Out, MarshalAs(UnmanagedType.LPArray)]
            int[] pceltFetched);

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int Skip(
            [In, MarshalAs(UnmanagedType.U4)]
            int celt);

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int Reset();

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int Clone(
            [Out, MarshalAs(UnmanagedType.LPArray)]
            IEnumFORMATETC[] ppenum);
    }
    #endregion

    #region IOleInPlaceUIWindow
    [ComVisible(true), ComImport(),
        Guid("00000115-0000-0000-C000-000000000046"),
        InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IOleInPlaceUIWindow 
    {
        //IOleWindow
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int GetWindow([Out] out IntPtr phwnd);
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int ContextSensitiveHelp([In, MarshalAs(UnmanagedType.Bool)] bool
            fEnterMode);

        //IOleInPlaceUIWindow
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int GetBorder([Out] out NativeMethods.RECT lprectBorder);
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int RequestBorderSpace([In] NativeMethods.RECT pborderwidths);
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int SetBorderSpace([In] NativeMethods.RECT pborderwidths);
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int SetActiveObject([In, MarshalAs(UnmanagedType.Interface)]
            IOleInPlaceActiveObject pActiveObject, [In, MarshalAs(UnmanagedType.LPWStr)]
            String pszObjName);
    }
    #endregion

    #region IOleInPlaceFrame
    [ComVisible(true), ComImport(),
        Guid("00000116-0000-0000-C000-000000000046"),
        InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IOleInPlaceFrame 
    {
        //IOleWindow
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int GetWindow([Out] out IntPtr phwnd);
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int ContextSensitiveHelp([In, MarshalAs(UnmanagedType.Bool)] bool
            fEnterMode);

        //IOleInPlaceUIWindow
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int GetBorder([Out] out NativeMethods.RECT lprectBorder);
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int RequestBorderSpace([In] NativeMethods.RECT pborderwidths);
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int SetBorderSpace([In] NativeMethods.RECT pborderwidths);
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int SetActiveObject([In, MarshalAs(UnmanagedType.Interface)]
            IOleInPlaceActiveObject pActiveObject, [In, MarshalAs(UnmanagedType.LPWStr)]
            String pszObjName);

        //IOleInPlaceFrame
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
            int InsertMenus([In] IntPtr hmenuShared, [In, Out] OleApi.OleMenuGroupWidths
            lpMenuWidths);
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int SetMenu([In] IntPtr hmenuShared, [In] IntPtr holemenu, [In] IntPtr
            hwndActiveObject);
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int RemoveMenus([In] IntPtr hmenuShared);
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int SetStatusText([In, MarshalAs(UnmanagedType.LPWStr)] string
            pszStatusText);
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int EnableModeless([In, MarshalAs(UnmanagedType.Bool)] Boolean fEnable);
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int TranslateAccelerator([In, MarshalAs(UnmanagedType.LPStruct)] NativeMethods.MSG
            lpmsg, [In, MarshalAs(UnmanagedType.U2)] short wID);
    }
    #endregion

    #region IOleInPlaceSite
    [ComVisible(true), ComImport(),
        Guid("00000119-0000-0000-C000-000000000046"),
        InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IOleInPlaceSite 
    {
        //IOleWindow
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int GetWindow([Out] out IntPtr phwnd);
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int ContextSensitiveHelp([In, MarshalAs(UnmanagedType.Bool)] bool
            fEnterMode);

        [return: MarshalAs(UnmanagedType.I4)][PreserveSig] 
        int	CanInPlaceActivate();
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int	 OnInPlaceActivate();
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig] 
        int OnUIActivate();
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig] 
        int GetWindowContext([Out,MarshalAs(UnmanagedType.Interface)] out IOleInPlaceFrame ppFrame,
            [Out,MarshalAs(UnmanagedType.Interface)] out IOleInPlaceUIWindow
           ppDoc, [Out, MarshalAs(UnmanagedType.Struct)] out NativeMethods.RECT lprcPosRect, [Out, MarshalAs(UnmanagedType.Struct)] out NativeMethods.RECT lprcClipRect, [In, Out] OleApi.OIFI
            lpFrameInfo);
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int Scroll([In, MarshalAs(UnmanagedType.U4)] NativeMethods.SIZE scrollExtent);
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig] 
        int OnUIDeactivate([In, MarshalAs(UnmanagedType.Bool)] bool fUndoable);
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig] 
        int OnInPlaceDeactivate();
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig] 
        int DiscardUndoState();
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig] 
        int DeactivateAndUndo();
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int OnPosRectChange([In] NativeMethods.RECT lprcPosRect);
    }
    #endregion

    #region IOleInPlaceSiteEx
    [ComVisible(true), ComImport(), System.CLSCompliant(false),
        Guid("9C2CAD80-3424-11CF-B670-00AA004CD6D8"),
        InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IOleInPlaceSiteEx 
    {
        //IOleWindow
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int GetWindow([Out] out IntPtr phwnd);
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int ContextSensitiveHelp([In, MarshalAs(UnmanagedType.Bool)] bool
            fEnterMode);

        [return: MarshalAs(UnmanagedType.I4)][PreserveSig] 
        int	CanInPlaceActivate();
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int	 OnInPlaceActivate();
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig] 
        int OnUIActivate();
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig] 
        int GetWindowContext([Out,MarshalAs(UnmanagedType.Interface)] out IOleInPlaceFrame ppFrame,
            [Out,MarshalAs(UnmanagedType.Interface)] out IOleInPlaceUIWindow
           ppDoc, [Out, MarshalAs(UnmanagedType.Struct)] out NativeMethods.RECT lprcPosRect, [Out, MarshalAs(UnmanagedType.Struct)] out NativeMethods.RECT lprcClipRect, [In, Out] OleApi.OIFI
            lpFrameInfo);
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int Scroll([In, MarshalAs(UnmanagedType.Struct)] NativeMethods.SIZE scrollExtent);
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig] 
        int OnUIDeactivate([In, MarshalAs(UnmanagedType.Bool)] bool fUndoable);
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig] 
        int OnInPlaceDeactivate();
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig] 
        int DiscardUndoState();
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig] 
        int DeactivateAndUndo();
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int OnPosRectChange([In] NativeMethods.RECT lprcPosRect);

        //IOleInPlaceSiteEx
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int	 OnInPlaceActivateEx(
            [Out,MarshalAs(UnmanagedType.Bool)] out bool pfNoRedraw,
            [In,MarshalAs(UnmanagedType.U4)]  uint dwFlags);

        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int	 OnInPlaceDeactivateEx(
            [In,MarshalAs(UnmanagedType.Bool)] bool fNoRedraw);

        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int	 RequestUIActivate();
    }
    #endregion

    #region IOleDocumentSite
    [ComVisible(true), ComImport(), Guid("B722BCC7-4E68-101B-A2BC-00AA00404770"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IOleDocumentSite 
    {
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int ActivateMe(
            [In, MarshalAs(UnmanagedType.Interface)]
            IOleDocumentView pViewToActivate);
    }
    #endregion

    #region IOleDocumentView
    [ComVisible(true), Guid("B722BCC6-4E68-101B-A2BC-00AA00404770"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IOleDocumentView 
    {
        void SetInPlaceSite(
            [In, MarshalAs(UnmanagedType.Interface)]
            IOleInPlaceSite pIPSite);

        [return: MarshalAs(UnmanagedType.Interface)]
        IOleInPlaceSite GetInPlaceSite();

        [return: MarshalAs(UnmanagedType.Interface)]
        object GetDocument();

        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int SetRect([In] ref NativeMethods.RECT prcView);

        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int GetRect(
            [Out]
            out NativeMethods.RECT prcView);

        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int SetRectComplex(
            [In]
            NativeMethods.RECT prcView,
            [In]
            NativeMethods.RECT prcHScroll,
            [In]
            NativeMethods.RECT prcVScroll,
            [In]
            NativeMethods.RECT prcSizeBox);

        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int Show(
            [In, MarshalAs(UnmanagedType.Bool)]
            bool fShow);

        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int UIActivate(
            [In, MarshalAs(UnmanagedType.Bool)]
            bool fUIActivate);

        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int Open();

        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int Close(
            [In, MarshalAs(UnmanagedType.U4)]
            int dwReserved);

        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int SaveViewState(
            [In, MarshalAs(UnmanagedType.Interface)]
            IStream pstm);

        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int ApplyViewState(
            [In, MarshalAs(UnmanagedType.Interface)]
            IStream pstm);

        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int Clone(
            [In, MarshalAs(UnmanagedType.Interface)]
            IOleInPlaceSite pIPSiteNew,
            [Out, MarshalAs(UnmanagedType.LPArray)]
            IOleDocumentView[] ppViewNew);
    }
    #endregion

    #region IOleInPlaceObject
    [ComVisible(true), ComImport,
        Guid("00000113-0000-0000-C000-000000000046")]
        public interface IOleInPlaceObject 
    {
        //IOleWindow
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int GetWindow([Out] out IntPtr phwnd);
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int ContextSensitiveHelp([In, MarshalAs(UnmanagedType.Bool)] bool
            fEnterMode);

        void InPlaceDeactivate();
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int UIDeactivate();
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int SetObjectRects( [In] NativeMethods.RECT lprcPosRect,
            [In] NativeMethods.RECT lprcClipRect);
    
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int ReactivateAndUndo();
    }
    #endregion

    #region IOleInPlaceActiveObject
    [ComVisible(true), ComImport(),
        Guid("00000117-0000-0000-C000-000000000046"),
        InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IOleInPlaceActiveObject 
    {

        //IOleWindow
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int GetWindow([Out] out IntPtr phwnd);
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int ContextSensitiveHelp([In, MarshalAs(UnmanagedType.Bool)] bool
            fEnterMode);

        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int TranslateAccelerator([In, MarshalAs(UnmanagedType.LPStruct)] NativeMethods.MSG
            lpmsg);
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int OnFrameWindowActivate([In, MarshalAs(UnmanagedType.Bool)] bool
            fActivate);
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int OnDocWindowActivate([In, MarshalAs(UnmanagedType.Bool)] bool fActivate);
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int ResizeBorder([In] NativeMethods.RECT prcBorder, [In, MarshalAs(UnmanagedType.Interface)] IntPtr pUIWindow, [In,
            MarshalAs(UnmanagedType.Bool)] Boolean fFrameWindow);
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int EnableModeless([In, MarshalAs(UnmanagedType.Bool)] Boolean fEnable);
    }
    #endregion

    #region IOleObject
    [NDependIgnore]
    [ComVisible(true), ComImport(),
        Guid("00000112-0000-0000-C000-000000000046"),
        InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
        internal interface IOleObject 
    {
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int SetClientSite([In, MarshalAs(UnmanagedType.Interface)] IOleClientSite
            pClientSite);
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int GetClientSite([Out, MarshalAs(UnmanagedType.Interface)] out IOleClientSite site);
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int SetHostNames([In, MarshalAs(UnmanagedType.LPWStr)] String
            szContainerApp, [In, MarshalAs(UnmanagedType.LPWStr)] String
            szContainerObj);
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int Close([In,MarshalAs(UnmanagedType.U4)] uint dwSaveOption);
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int SetMoniker([In, MarshalAs(UnmanagedType.U4)] uint dwWhichMoniker, [In,
            MarshalAs(UnmanagedType.Interface)] Object pmk);
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int GetMoniker([In, MarshalAs(UnmanagedType.U4)] uint dwAssign, [In,
            MarshalAs(UnmanagedType.U4)] uint dwWhichMoniker,[Out,MarshalAs(UnmanagedType.Interface)] out Object moniker);
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int InitFromData([In, MarshalAs(UnmanagedType.Interface)] Object
            pDataObject, [In, MarshalAs(UnmanagedType.Bool)] Boolean fCreation, [In,
            MarshalAs(UnmanagedType.U4)] uint dwReserved);
        int GetClipboardData([In, MarshalAs(UnmanagedType.U4)] uint dwReserved, out
            Object data);
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int DoVerb([In, MarshalAs(UnmanagedType.I4)] int iVerb, [In] IntPtr lpmsg,
            [In, MarshalAs(UnmanagedType.Interface)] IOleClientSite pActiveSite, [In,
            MarshalAs(UnmanagedType.I4)] int lindex, [In] IntPtr hwndParent, [In] NativeMethods.RECT
            lprcPosRect);
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int EnumVerbs(out IEnumOLEVERB e);
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int Update();
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int IsUpToDate();
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int GetUserClassID([In, Out] ref Guid pClsid);
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int GetUserType([In, MarshalAs(UnmanagedType.U4)] uint dwFormOfType, [Out,
            MarshalAs(UnmanagedType.LPWStr)] out String userType);
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int SetExtent([In, MarshalAs(UnmanagedType.U4)] uint dwDrawAspect, [In]
            NativeMethods.SIZE pSizel);
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int GetExtent([In, MarshalAs(UnmanagedType.U4)] uint dwDrawAspect, [Out]
            NativeMethods.SIZE pSizel);
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int Advise([In, MarshalAs(UnmanagedType.Interface)] IAdviseSink pAdvSink, out
            int cookie);
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int Unadvise([In, MarshalAs(UnmanagedType.U4)] int dwConnection);
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int EnumAdvise(out Object e);
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
        int GetMiscStatus([In, MarshalAs(UnmanagedType.U4)] uint dwAspect, out int
            misc);
        [return: MarshalAs(UnmanagedType.I4)][PreserveSig]
            int SetColorScheme([In] OleApi.LOGPALETTE pLogpal);
    }
    #endregion

    [ComVisible(true),  Guid("B3E7C340-EF97-11CE-9BC9-00AA00608E01"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IEnumOleUndoUnits  
    {

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int Next([In, MarshalAs(UnmanagedType.U4)] int numDesired, [Out] out IntPtr unit, [Out, MarshalAs(UnmanagedType.U4)] out int numReceived);

        void Bogus();

        [PreserveSig]
        int Skip([In, MarshalAs(UnmanagedType.I4)] int numToSkip);

        [PreserveSig]
        int Reset();

        [PreserveSig]
        int Clone([Out, MarshalAs(UnmanagedType.Interface)] IEnumOleUndoUnits enumerator);
    };

    [ComVisible(true), Guid("A1FAF330-EF97-11CE-9BC9-00AA00608E01")]
    public interface IOleParentUndoUnit : IOleUndoUnit
    {
        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int Open([In, MarshalAs(UnmanagedType.Interface)] IOleParentUndoUnit parentUnit);

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int Close([In, MarshalAs(UnmanagedType.Interface)] IOleParentUndoUnit parentUnit, [In, MarshalAs(UnmanagedType.Bool)] bool fCommit);

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int Add([In, MarshalAs(UnmanagedType.Interface)] IOleUndoUnit undoUnit);

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int FindUnit([In, MarshalAs(UnmanagedType.Interface)] IOleUndoUnit undoUnit);

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int GetParentState([Out, MarshalAs(UnmanagedType.I4)] out int state);
    }

    [ComVisible(true), Guid("894AD3B0-EF97-11CE-9BC9-00AA00608E01"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IOleUndoUnit  
    {
        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int Do([In, MarshalAs(UnmanagedType.Interface)] IOleUndoManager undoManager);

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int GetDescription([Out, MarshalAs(UnmanagedType.BStr)] out string bStr);

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int GetUnitType([Out, MarshalAs(UnmanagedType.I4)] out int clsid, [Out, MarshalAs(UnmanagedType.I4)] out int plID);

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int OnNextAdd();
    }

    [ComVisible(true), Guid("D001F200-EF97-11CE-9BC9-00AA00608E01"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IOleUndoManager 
    {
        void Open([In, MarshalAs(UnmanagedType.Interface)] IOleParentUndoUnit parentUndo);

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int Close([In, MarshalAs(UnmanagedType.Interface)] IOleParentUndoUnit parentUndo, [In, MarshalAs(UnmanagedType.Bool)] bool fCommit);

        void Add([In, MarshalAs(UnmanagedType.Interface)] IOleUndoUnit undoUnit);

        [return: MarshalAs(UnmanagedType.I4)]
        int GetOpenParentState();

        void DiscardFrom([In, MarshalAs(UnmanagedType.Interface)] IOleUndoUnit undoUnit);
        void UndoTo([In, MarshalAs(UnmanagedType.Interface)] IOleUndoUnit undoUnit);
        void RedoTo([In, MarshalAs(UnmanagedType.Interface)] IOleUndoUnit undoUnit);
        [return: MarshalAs(UnmanagedType.Interface)]
        IEnumOleUndoUnits EnumUndoable();
        [return: MarshalAs(UnmanagedType.Interface)]
        IEnumOleUndoUnits EnumRedoable();
        [return: MarshalAs(UnmanagedType.BStr)]
        string GetLastUndoDescription();
        [return: MarshalAs(UnmanagedType.BStr)]
        string GetLastRedoDescription();


        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int Enable([In, MarshalAs(UnmanagedType.Bool)] bool fEnable);
    }

    [ComVisible(true), Guid("6D5140C1-7436-11CE-8034-00AA006009FA"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IComServiceProvider 
    {
        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int QueryService([In] ref Guid sid, [In] ref Guid iid, out IntPtr service);
    }

    [GuidAttribute("0000010d-0000-0000-C000-000000000046")]
    [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    [ComImportAttribute()]
    [NDependIgnore]
    public interface IViewObject
    {
        void Draw(
            [MarshalAs(UnmanagedType.U4)] int dwDrawAspect,
            int lindex,
            IntPtr pvAspect,
            IntPtr asdf,
            IntPtr hdcTargetDev,
            IntPtr hdcDraw,
            NativeMethods.RECT lprcBounds,
            ref NativeMethods.RECT lprcWBounds,
            IntPtr pfnContinue,
            int dwContinue);

        int GetColorSet([MarshalAs(UnmanagedType.U4)] 
                int dwDrawAspect,
            int lindex,
            IntPtr pvAspect,
            DVTARGETDEVICE ptd,
            IntPtr hicTargetDev,
            out object ppColorSet);

        int Freeze([MarshalAs(UnmanagedType.U4)] 
                int dwDrawAspect,
            int lindex,
            IntPtr pvAspect,
            out IntPtr pdwFreeze);

        int Unfreeze([MarshalAs(UnmanagedType.U4)] int dwFreeze);

        int SetAdvise([MarshalAs(UnmanagedType.U4)] 
                int aspects,
            [MarshalAs(UnmanagedType.U4)] int advf,
            [MarshalAs(UnmanagedType.Interface)] IAdviseSink pAdvSink);

        void GetAdvise(
            [MarshalAs(UnmanagedType.LPArray)] out int[] paspects,
            [MarshalAs(UnmanagedType.LPArray)] out int[] advf,
            [MarshalAs(UnmanagedType.LPArray)] out IAdviseSink[] pAdvSink);
    }

    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct DVTARGETDEVICE
    {
        [MarshalAs(UnmanagedType.U4)]
        public int tdSize;
        [MarshalAs(UnmanagedType.U2)]
        public short tdDriverNameOffset;
        [MarshalAs(UnmanagedType.U2)]
        public short tdDeviceNameOffset;
        [MarshalAs(UnmanagedType.U2)]
        public short tdPortNameOffset;
        [MarshalAs(UnmanagedType.U2)]
        public short tdExtDevmodeOffset;
        [MarshalAs(UnmanagedType.LPArray, SizeConst = 1)]
        public byte[] tdData;
    }

    #endregion
}