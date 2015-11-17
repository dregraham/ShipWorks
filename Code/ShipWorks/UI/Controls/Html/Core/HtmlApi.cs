using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.CustomMarshalers;
using System.Collections;
using System.Windows.Forms;
using Interapptive.Shared;
using System.Runtime.CompilerServices;
using Interapptive.Shared.Win32;

namespace ShipWorks.UI.Controls.Html.Core
{
	/// <summary>
	/// Interfaces and constants for dealing with the HTML COM API for the html control
	/// </summary>
	public class HtmlApi 
    {
        #region Enums

        #region CARET_DIRECTION
        public enum CARET_DIRECTION 
        {
            CARET_DIRECTION_INDETERMINATE	= 0,
            CARET_DIRECTION_SAME	= 1,
            CARET_DIRECTION_BACKWARD	= 2,
            CARET_DIRECTION_FORWARD	= 3,
            CARET_DIRECTION_Max	= 2147483647
        }
        #endregion

        #region COORD_SYSTEM
        public enum COORD_SYSTEM 
        {
            COORD_SYSTEM_GLOBAL	= 0,
            COORD_SYSTEM_PARENT	= 1,
            COORD_SYSTEM_CONTAINER	= 2,
            COORD_SYSTEM_CONTENT	= 3,
            COORD_SYSTEM_FRAME	= 4,
            COORD_SYSTEM_Max	= 2147483647
        }
        #endregion

        #region HTML_PAINTER_INFO
        [ComVisible(true), StructLayout(LayoutKind.Sequential)]
            public class HTML_PAINTER_INFO 
        {
            [MarshalAs(UnmanagedType.I4)]
            public int lFlags = 0;

            [MarshalAs(UnmanagedType.I4)]
            public int lZOrder = 0;

            [MarshalAs(UnmanagedType.Struct)]
            public Guid iidDrawObject = Guid.Empty;

            [MarshalAs(UnmanagedType.Struct)]
            public NativeMethods.RECT rcBounds = new NativeMethods.RECT();
        }
        #endregion

        #region ELEMENT_CORNER
        public enum ELEMENT_CORNER 
        {
            ELEMENT_CORNER_NONE	= 0,
            ELEMENT_CORNER_TOP	= 1,
            ELEMENT_CORNER_LEFT	= 2,
            ELEMENT_CORNER_BOTTOM	= 3,
            ELEMENT_CORNER_RIGHT	= 4,
            ELEMENT_CORNER_TOPLEFT	= 5,
            ELEMENT_CORNER_TOPRIGHT	= 6,
            ELEMENT_CORNER_BOTTOMLEFT	= 7,
            ELEMENT_CORNER_BOTTOMRIGHT	= 8,
            ELEMENT_CORNER_Max	= 2147483647
        }
        #endregion

        #region DISPLAY_GRAVITY
        public enum DISPLAY_GRAVITY 
        {
            DISPLAY_GRAVITY_PreviousLine	= 1,
            DISPLAY_GRAVITY_NextLine	= 2,
            DISPLAY_GRAVITY_Max	= 2147483647
        }
        #endregion

        #region Display_MoveUnit
        public enum DISPLAY_MOVEUNIT 
        {
            DISPLAY_MOVEUNIT_PreviousLine	= 1,
            DISPLAY_MOVEUNIT_NextLine	= 2,
            DISPLAY_MOVEUNIT_CurrentLineStart	= 3,
            DISPLAY_MOVEUNIT_CurrentLineEnd	= 4,
            DISPLAY_MOVEUNIT_TopOfWindow	= 5,
            DISPLAY_MOVEUNIT_BottomOfWindow	= 6,
            DISPLAY_MOVEUNIT_Max = 2147483647
        }
        #endregion

        #region Element_tag_id
        public enum ELEMENT_TAG_ID 
        {
            TAGID_NULL	= 0,
            TAGID_UNKNOWN	= 1,
            TAGID_A	= 2,
            TAGID_ACRONYM	= 3,
            TAGID_ADDRESS	= 4,
            TAGID_APPLET	= 5,
            TAGID_AREA	= 6,
            TAGID_B	= 7,
            TAGID_BASE	= 8,
            TAGID_BASEFONT	= 9,
            TAGID_BDO	= 10,
            TAGID_BGSOUND	= 11,
            TAGID_BIG	= 12,
            TAGID_BLINK	= 13,
            TAGID_BLOCKQUOTE	= 14,
            TAGID_BODY	= 15,
            TAGID_BR	= 16,
            TAGID_BUTTON	= 17,
            TAGID_CAPTION	= 18,
            TAGID_CENTER	= 19,
            TAGID_CITE	= 20,
            TAGID_CODE	= 21,
            TAGID_COL	= 22,
            TAGID_COLGROUP	= 23,
            TAGID_COMMENT	= 24,
            TAGID_COMMENT_RAW	= 25,
            TAGID_DD	= 26,
            TAGID_DEL	= 27,
            TAGID_DFN	= 28,
            TAGID_DIR	= 29,
            TAGID_DIV	= 30,
            TAGID_DL	= 31,
            TAGID_DT	= 32,
            TAGID_EM	= 33,
            TAGID_EMBED	= 34,
            TAGID_FIELDSET	= 35,
            TAGID_FONT	= 36,
            TAGID_FORM	= 37,
            TAGID_FRAME	= 38,
            TAGID_FRAMESET	= 39,
            TAGID_GENERIC	= 40,
            TAGID_H1	= 41,
            TAGID_H2	= 42,
            TAGID_H3	= 43,
            TAGID_H4	= 44,
            TAGID_H5	= 45,
            TAGID_H6	= 46,
            TAGID_HEAD	= 47,
            TAGID_HR	= 48,
            TAGID_HTML	= 49,
            TAGID_I	= 50,
            TAGID_IFRAME	= 51,
            TAGID_IMG	= 52,
            TAGID_INPUT	= 53,
            TAGID_INS	= 54,
            TAGID_KBD	= 55,
            TAGID_LABEL	= 56,
            TAGID_LEGEND	= 57,
            TAGID_LI	= 58,
            TAGID_LINK	= 59,
            TAGID_LISTING	= 60,
            TAGID_MAP	= 61,
            TAGID_MARQUEE	= 62,
            TAGID_MENU	= 63,
            TAGID_META	= 64,
            TAGID_NEXTID	= 65,
            TAGID_NOBR	= 66,
            TAGID_NOEMBED	= 67,
            TAGID_NOFRAMES	= 68,
            TAGID_NOSCRIPT	= 69,
            TAGID_OBJECT	= 70,
            TAGID_OL	= 71,
            TAGID_OPTION	= 72,
            TAGID_P	= 73,
            TAGID_PARAM	= 74,
            TAGID_PLAINTEXT	= 75,
            TAGID_PRE	= 76,
            TAGID_Q	= 77,
            TAGID_RP	= 78,
            TAGID_RT	= 79,
            TAGID_RUBY	= 80,
            TAGID_S	= 81,
            TAGID_SAMP	= 82,
            TAGID_SCRIPT	= 83,
            TAGID_SELECT	= 84,
            TAGID_SMALL	= 85,
            TAGID_SPAN	= 86,
            TAGID_STRIKE	= 87,
            TAGID_STRONG	= 88,
            TAGID_STYLE	= 89,
            TAGID_SUB	= 90,
            TAGID_SUP	= 91,
            TAGID_TABLE	= 92,
            TAGID_TBODY	= 93,
            TAGID_TC	= 94,
            TAGID_TD	= 95,
            TAGID_TEXTAREA	= 96,
            TAGID_TFOOT	= 97,
            TAGID_TH	= 98,
            TAGID_THEAD	= 99,
            TAGID_TITLE	= 100,
            TAGID_TR	= 101,
            TAGID_TT	= 102,
            TAGID_U	= 103,
            TAGID_UL	= 104,
            TAGID_VAR	= 105,
            TAGID_WBR	= 106,
            TAGID_XMP	= 107,
            TAGID_ROOT	= 108,
            TAGID_OPTGROUP	= 109,
            TAGID_COUNT	= 110,
            TAGID_LAST_PREDEFINED	= 10000,
            ELEMENT_TAG_ID_Max	= 2147483647
        }
        #endregion

        #region Element_adjacency
        public enum ELEMENT_ADJACENCY 
        {
            ELEM_ADJ_BeforeBegin	= 0,
            ELEM_ADJ_AfterBegin	= 1,
            ELEM_ADJ_BeforeEnd	= 2,
            ELEM_ADJ_AfterEnd	= 3,
            ELEMENT_ADJACENCY_Max	= 2147483647
        }
        #endregion

        #region Pointer_Gravity
        public enum POINTER_GRAVITY 
        {
            POINTER_GRAVITY_Left	= 0,
            POINTER_GRAVITY_Right	= 1,
            POINTER_GRAVITY_Max	= 2147483647
        }
        #endregion

        #region Markup_Context_Type
        public enum MARKUP_CONTEXT_TYPE 
        {
            CONTEXT_TYPE_None	= 0,
            CONTEXT_TYPE_Text	= 1,
            CONTEXT_TYPE_EnterScope	= 2,
            CONTEXT_TYPE_ExitScope	= 3,
            CONTEXT_TYPE_NoScope	= 4,
            MARKUP_CONTEXT_TYPE_Max	= 2147483647
        }
        #endregion

        #region Move_Unit_Action
        public enum MOVEUNIT_ACTION 
        {
            MOVEUNIT_PREVCHAR	= 0,
            MOVEUNIT_NEXTCHAR	= 1,
            MOVEUNIT_PREVCLUSTERBEGIN	= 2,
            MOVEUNIT_NEXTCLUSTERBEGIN	= 3,
            MOVEUNIT_PREVCLUSTEREND	= 4,
            MOVEUNIT_NEXTCLUSTEREND	= 5,
            MOVEUNIT_PREVWORDBEGIN	= 6,
            MOVEUNIT_NEXTWORDBEGIN	= 7,
            MOVEUNIT_PREVWORDEND	= 8,
            MOVEUNIT_NEXTWORDEND	= 9,
            MOVEUNIT_PREVPROOFWORD	= 10,
            MOVEUNIT_NEXTPROOFWORD	= 11,
            MOVEUNIT_NEXTURLBEGIN	= 12,
            MOVEUNIT_PREVURLBEGIN	= 13,
            MOVEUNIT_NEXTURLEND	= 14,
            MOVEUNIT_PREVURLEND	= 15,
            MOVEUNIT_PREVSENTENCE	= 16,
            MOVEUNIT_NEXTSENTENCE	= 17,
            MOVEUNIT_PREVBLOCK	= 18,
            MOVEUNIT_NEXTBLOCK	= 19,
            MOVEUNIT_ACTION_Max	= 2147483647
        }
        #endregion

        #region SELECTION_TYPE
        public enum SELECTION_TYPE 
        {
            SELECTION_TYPE_None = 0,
            SELECTION_TYPE_Caret = 1,
            SELECTION_TYPE_Text = 2,
            SELECTION_TYPE_Control = 3,
            SELECTION_TYPE_Max = 2147483647
        } 
        #endregion

        #region HtmlPainter

        public enum HtmlPainter
        {
            ALPHA = 4,
            COMPLEX = 8,
            HITTEST = 0x20,
            NOBAND = 0x400,
            NODC = 0x1000,
            NOPHYSICALCLIP = 0x2000,
            NOSAVEDC = 0x4000,
            OPAQUE = 1,
            OVERLAY = 0x10,
            SUPPORTS_XFORM = 0x8000,
            SURFACE = 0x100,
            THREEDSURFACE = 0x200,
            TRANSPARENT = 2
        }
 
        #endregion

        #region HtmlZOrder

        public enum HtmlZOrder
        {
            NONE,
            REPLACE_ALL,
            REPLACE_CONTENT,
            REPLACE_BACKGROUND,
            BELOW_CONTENT,
            BELOW_FLOW,
            ABOVE_FLOW,
            ABOVE_CONTENT,
            WINDOW_TOP
        }
 
        #endregion

        #endregion

        #region Constants

        #region Base Constants
        public const int DISPID_UNKNOWN = -1;
        public const int DISPID_AUTOSIZE  = (-500);
        public const int DISPID_BACKCOLOR = (-501);
        public const int DISPID_BACKSTYLE = (-502);
        public const int DISPID_BORDERCOLOR = (-503);
        public const int DISPID_BORDERSTYLE = (-504);
        public const int DISPID_BORDERWIDTH = (-505);
        public const int DISPID_DRAWMODE  = (-507);
        public const int DISPID_DRAWSTYLE = (-508);
        public const int DISPID_DRAWWIDTH = (-509);
        public const int DISPID_FILLCOLOR = (-510);
        public const int DISPID_FILLSTYLE = (-511);
        public const int DISPID_FONT  = (-512);
        public const int DISPID_FORECOLOR = (-513);
        public const int DISPID_ENABLED = (-514);
        public const int DISPID_HWND  = (-515);
        public const int DISPID_TABSTOP = (-516);
        public const int DISPID_TEXT  = (-517);
        public const int DISPID_CAPTION = (-518);
        public const int DISPID_BORDERVISIBLE = (-519);
        public const int DISPID_APPEARANCE  = (-520);
        public const int DISPID_MOUSEPOINTER  = (-521);
        public const int DISPID_MOUSEICON = (-522);
        public const int DISPID_PICTURE = (-523);
        public const int DISPID_VALID = (-524);
        public const int DISPID_READYSTATE  = (-525);
        public const int DISPID_LISTINDEX = (-526);
        public const int DISPID_SELECTED  = (-527);
        public const int DISPID_LIST  = (-528);
        public const int DISPID_COLUMN  = (-529);
        public const int DISPID_LISTCOUNT = (-531);
        public const int DISPID_MULTISELECT = (-532);
        public const int DISPID_MAXLENGTH = (-533);
        public const int DISPID_PASSWORDCHAR  = (-534);
        public const int DISPID_SCROLLBARS  = (-535);
        public const int DISPID_WORDWRAP  = (-536);
        public const int DISPID_MULTILINE = (-537);
        public const int DISPID_NUMBEROFROWS  = (-538);
        public const int DISPID_NUMBEROFCOLUMNS = (-539);
        public const int DISPID_DISPLAYSTYLE  = (-540);
        public const int DISPID_GROUPNAME = (-541);
        public const int DISPID_IMEMODE = (-542);
        public const int DISPID_ACCELERATOR  =  (-543);
        public const int DISPID_ENTERKEYBEHAVIOR =  (-544);
        public const int DISPID_TABKEYBEHAVIOR  = (-545);
        public const int DISPID_SELTEXT = (-546);
        public const int DISPID_SELSTART  = (-547);
        public const int DISPID_SELLENGTH = (-548);

        public const int DISPID_REFRESH = (-550);
        public const int DISPID_DOCLICK  =  (-551);
        public const int DISPID_ABOUTBOX  = (-552);
        public const int DISPID_ADDITEM  =  (-553);
        public const int DISPID_CLEAR = (-554);
        public const int DISPID_REMOVEITEM =  (-555);
        public const int DISPID_NORMAL_FIRST =  1000;

        public const int DISPID_PROPERTYPUT	=					-3; //Indicate the Param
        public const int DISPID_NEWENUM	=						-4; //New Enum
        public const int DISPID_XOBJ_MIN    =					unchecked ((int) 0x80010000);
        public const int DISPID_XOBJ_MAX    =					unchecked ((int) 0x8001FFFF);
        public const int DISPID_XOBJ_BASE    =					DISPID_XOBJ_MIN;
        public const int STDPROPID_XOBJ_BLOCKALIGN  =			DISPID_XOBJ_BASE + 0x48;
        public const int DISPID_HTMLOBJECT   =					DISPID_XOBJ_BASE + 500;
        public const int DISPID_ELEMENT      =					DISPID_HTMLOBJECT + 500;
        public const int DISPID_SITE         =					DISPID_ELEMENT + 1000;
        public const int DISPID_OBJECT       =					DISPID_SITE + 1000;
        public const int DISPID_STYLE         =					DISPID_OBJECT + 1000;
        public const int DISPID_ATTRS         =					DISPID_STYLE + 1000;
        public const int DISPID_EVENTS        =					DISPID_ATTRS + 1000;
        public const int DISPID_XOBJ_EXPANDO  =					DISPID_EVENTS + 1000;
        public const int DISPID_XOBJ_ORDINAL  =					DISPID_XOBJ_EXPANDO + 1000;
        public const int STDPROPID_XOBJ_WIDTH   =				DISPID_XOBJ_BASE + 0x5;
        public const int STDPROPID_XOBJ_HEIGHT   =				DISPID_XOBJ_BASE + 0x6;
        public const int STDPROPID_XOBJ_CONTROLALIGN  =			DISPID_XOBJ_BASE + 0x49;
        public const int STDPROPID_XOBJ_STYLE     =				DISPID_XOBJ_BASE + 0x4A;
        public const int STDPROPID_XOBJ_PARENT        =			DISPID_XOBJ_BASE + 0x8;
        public const int STDPROPID_XOBJ_CONTROLTIPTEXT   =		DISPID_XOBJ_BASE + 0x45;
        public const int STDPROPID_XOBJ_TABINDEX =				(DISPID_XOBJ_BASE + 0xF);
        public const int DISPID_A_COLOR  =						DISPID_A_FIRST+2;
        public const int DISPID_OMDOCUMENT =                    DISPID_NORMAL_FIRST;
        public const int DISPID_COLLECTION_MIN      =			1000000;
        public const int DISPID_COLLECTION_MAX    =				2999999;
        public const int DISPID_COLLECTION          =           DISPID_NORMAL_FIRST+500;
        public const int DISPID_VALUE	=						0;
        public const int DISPID_A_LANG  =						(DISPID_A_FIRST+9);
        public const int DISPID_A_LANGUAGE  =					(DISPID_A_FIRST+100);
        public const int DISPID_A_BACKGROUNDATTACHMENT      =   (DISPID_A_FIRST+45);
        public const int DISPID_A_MARGINTOP           =         (DISPID_A_FIRST+37);
        public const int DISPID_A_MARGINRIGHT         =         (DISPID_A_FIRST+38);
        public const int DISPID_A_MARGINBOTTOM        =         (DISPID_A_FIRST+39);
        public const int DISPID_A_MARGINLEFT         =          (DISPID_A_FIRST+40);
        public const int DISPID_A_NOWRAP             =          (DISPID_A_FIRST+5);
        public const int DISPID_TEXTSITE           =			DISPID_NORMAL_FIRST;
        public const int DISPID_BODY                =           (DISPID_TEXTSITE + 1000);
        public const int DISPID_A_SCROLL            =           (DISPID_A_FIRST+79);

        public const int STDDISPID_XOBJ_ONBLUR =  (DISPID_XOBJ_BASE);
        public const int STDDISPID_XOBJ_ONFOCUS = (DISPID_XOBJ_BASE + 1);
        public const int STDDISPID_XOBJ_BEFOREUPDATE =  (DISPID_XOBJ_BASE + 4);
        public const int STDDISPID_XOBJ_AFTERUPDATE = (DISPID_XOBJ_BASE + 5);
        public const int STDDISPID_XOBJ_ONROWEXIT = (DISPID_XOBJ_BASE + 6);
        public const int STDDISPID_XOBJ_ONROWENTER  = (DISPID_XOBJ_BASE + 7);
        public const int STDDISPID_XOBJ_ONMOUSEOVER = (DISPID_XOBJ_BASE + 8);
        public const int STDDISPID_XOBJ_ONMOUSEOUT  = (DISPID_XOBJ_BASE + 9);
        public const int STDDISPID_XOBJ_ONHELP  = (DISPID_XOBJ_BASE + 10);
        public const int STDDISPID_XOBJ_ONDRAGSTART = (DISPID_XOBJ_BASE + 11);
        public const int STDDISPID_XOBJ_ONSELECTSTART = (DISPID_XOBJ_BASE + 12);
        public const int STDDISPID_XOBJ_ERRORUPDATE = (DISPID_XOBJ_BASE + 13);
        public const int STDDISPID_XOBJ_ONDATASETCHANGED =  (DISPID_XOBJ_BASE + 14);
        public const int STDDISPID_XOBJ_ONDATAAVAILABLE  =  (DISPID_XOBJ_BASE + 15);
        public const int STDDISPID_XOBJ_ONDATASETCOMPLETE = (DISPID_XOBJ_BASE + 16);
        public const int STDDISPID_XOBJ_ONFILTER =  (DISPID_XOBJ_BASE + 17);
        public const int STDDISPID_XOBJ_ONLOSECAPTURE  =  (DISPID_XOBJ_BASE + 18);
        public const int STDDISPID_XOBJ_ONPROPERTYCHANGE =  (DISPID_XOBJ_BASE + 19);
        public const int STDDISPID_XOBJ_ONDRAG =  (DISPID_XOBJ_BASE + 20);
        public const int STDDISPID_XOBJ_ONDRAGEND  =  (DISPID_XOBJ_BASE + 21);
        public const int STDDISPID_XOBJ_ONDRAGENTER  =  (DISPID_XOBJ_BASE + 22);
        public const int STDDISPID_XOBJ_ONDRAGOVER =  (DISPID_XOBJ_BASE + 23);
        public const int STDDISPID_XOBJ_ONDRAGLEAVE  =  (DISPID_XOBJ_BASE + 24);
        public const int STDDISPID_XOBJ_ONDROP =  (DISPID_XOBJ_BASE + 25);
        public const int STDDISPID_XOBJ_ONCUT  =  (DISPID_XOBJ_BASE + 26);
        public const int STDDISPID_XOBJ_ONCOPY =  (DISPID_XOBJ_BASE + 27);
        public const int STDDISPID_XOBJ_ONPASTE  =  (DISPID_XOBJ_BASE + 28);
        public const int STDDISPID_XOBJ_ONBEFORECUT  =  (DISPID_XOBJ_BASE + 29);
        public const int STDDISPID_XOBJ_ONBEFORECOPY =  (DISPID_XOBJ_BASE + 30);
        public const int STDDISPID_XOBJ_ONBEFOREPASTE  =  (DISPID_XOBJ_BASE + 31);
        public const int STDDISPID_XOBJ_ONROWSDELETE =  (DISPID_XOBJ_BASE + 32);
        public const int STDDISPID_XOBJ_ONROWSINSERTED =  (DISPID_XOBJ_BASE + 33);
        public const int STDDISPID_XOBJ_ONCELLCHANGE =  (DISPID_XOBJ_BASE + 34);
        public const int DISPID_A_DIR =									(DISPID_A_FIRST+117); // Complex Text support for bidi
        public const int DISPID_A_EDITABLE       =                (DISPID_A_FIRST+162);
        public const int DISPID_A_HIDEFOCUS        =              (DISPID_A_FIRST+163);
        public const int STDPROPID_XOBJ_DISABLED     =        (DISPID_XOBJ_BASE + 0x4C);
        #endregion

        #region DISPID's for classes
        public const int DISPID_ANCHOR                           = DISPID_NORMAL_FIRST;
        public const int DISPID_BLOCK                            = DISPID_NORMAL_FIRST;
        public const int DISPID_BR                               = DISPID_NORMAL_FIRST;
        public const int DISPID_BGSOUND                          = DISPID_NORMAL_FIRST;
        public const int DISPID_DD                               = DISPID_NORMAL_FIRST;
        public const int DISPID_DIR                              = DISPID_NORMAL_FIRST;
        public const int DISPID_DIV                              = DISPID_NORMAL_FIRST;
        public const int DISPID_DL                               = DISPID_NORMAL_FIRST;
        public const int DISPID_DT                               = DISPID_NORMAL_FIRST;
        public const int DISPID_EFONT                            = DISPID_NORMAL_FIRST;
        public const int DISPID_FORM                             = DISPID_NORMAL_FIRST;
        public const int DISPID_HEADER                           = DISPID_NORMAL_FIRST;
        public const int DISPID_HEDELEMS                         = DISPID_NORMAL_FIRST;
        public const int DISPID_HR                               = DISPID_NORMAL_FIRST;
        public const int DISPID_LABEL                           =  DISPID_NORMAL_FIRST;
        public const int DISPID_LI                              =  DISPID_NORMAL_FIRST;
        public const int DISPID_IMGBASE                          = DISPID_NORMAL_FIRST;
        public const int DISPID_IMG                              = (DISPID_IMGBASE + 1000);
        public const int DISPID_INPUTIMAGE                       = (DISPID_IMGBASE + 1000);
        public const int DISPID_INPUT                            = (DISPID_TEXTSITE + 1000);
        public const int DISPID_INPUTTEXTBASE                    = (DISPID_INPUT+1000);
        public const int DISPID_INPUTTEXT                       = (DISPID_INPUTTEXTBASE+1000);
        public const int DISPID_MENU                             = DISPID_NORMAL_FIRST;
        public const int DISPID_OL                               = DISPID_NORMAL_FIRST;
        public const int DISPID_PARA                             = DISPID_NORMAL_FIRST;
        public const int DISPID_SELECT                           = DISPID_NORMAL_FIRST;
        public const int DISPID_SELECTOBJ                        = DISPID_NORMAL_FIRST;
        public const int DISPID_TEXTAREA                         = (DISPID_INPUTTEXT + 1000);
        public const int DISPID_MARQUEE                          = (DISPID_TEXTAREA + 1000);
        public const int DISPID_RICHTEXT                         = (DISPID_MARQUEE + 1000);
        public const int DISPID_BUTTON                           = (DISPID_RICHTEXT + 1000);
        public const int DISPID_UL                               = DISPID_NORMAL_FIRST;
        public const int DISPID_PHRASE                           = DISPID_NORMAL_FIRST;
        public const int DISPID_UNKNOWNPDL                       = DISPID_NORMAL_FIRST;
        public const int DISPID_COMMENTPDL                       = DISPID_NORMAL_FIRST;
        public const int DISPID_TABLECELL                        = (DISPID_TEXTSITE + 1000);
        public const int DISPID_SELECTION                        = DISPID_NORMAL_FIRST;
        public const int DISPID_OPTION                           = DISPID_NORMAL_FIRST;
        public const int DISPID_1D                               = (DISPID_TEXTSITE + 1000);
        public const int DISPID_MAP                              = DISPID_NORMAL_FIRST;
        public const int DISPID_AREA                             = DISPID_NORMAL_FIRST;
        public const int DISPID_PARAM                            = DISPID_NORMAL_FIRST;
        public const int DISPID_TABLESECTION                     = DISPID_NORMAL_FIRST;
        public const int DISPID_TABLECOL                         = DISPID_NORMAL_FIRST;
        public const int DISPID_SCRIPT                           = DISPID_NORMAL_FIRST;
        public const int DISPID_STYLESHEET                       = DISPID_NORMAL_FIRST;
        public const int DISPID_STYLERULE                        = DISPID_NORMAL_FIRST;
        public const int DISPID_STYLEPAGE                        = DISPID_NORMAL_FIRST;
        public const int DISPID_STYLESHEETS_COL                  = DISPID_NORMAL_FIRST;
        public const int DISPID_STYLERULES_COL                   = DISPID_NORMAL_FIRST;
        public const int DISPID_STYLEPAGES_COL                   = DISPID_NORMAL_FIRST;
        public const int DISPID_MIMETYPES_COL                    = DISPID_NORMAL_FIRST;
        public const int DISPID_PLUGINS_COL                      = DISPID_NORMAL_FIRST;
        public const int DISPID_2D                               = DISPID_NORMAL_FIRST;
        public const int DISPID_OMWINDOW                         = DISPID_NORMAL_FIRST;
        public const int DISPID_EVENTOBJ                         = DISPID_NORMAL_FIRST;
        public const int DISPID_PERSISTDATA                      = DISPID_NORMAL_FIRST;
        public const int DISPID_OLESITE                          = DISPID_NORMAL_FIRST;
        public const int DISPID_FRAMESET                         = DISPID_NORMAL_FIRST;
        public const int DISPID_LINK                             = DISPID_NORMAL_FIRST;
        public const int DISPID_STYLEELEMENT                     = DISPID_NORMAL_FIRST;
        public const int DISPID_FILTERS                          = DISPID_NORMAL_FIRST;
        public const int DISPID_OMRECT                           = DISPID_NORMAL_FIRST;
        public const int DISPID_DOMATTRIBUTE                     = DISPID_NORMAL_FIRST;
        public const int DISPID_DOMTEXTNODE                      = DISPID_NORMAL_FIRST;
        public const int DISPID_GENERIC                          = DISPID_NORMAL_FIRST;
        public const int DISPID_URN_COLL                         = DISPID_NORMAL_FIRST;
        public const int DISPID_NAMESPACE_COLLECTION             = DISPID_NORMAL_FIRST;
        public const int DISPID_NAMESPACE                        = DISPID_NORMAL_FIRST;
        public const int DISPID_TAGNAMES_COLLECTION              = DISPID_NORMAL_FIRST;

        public const int DISPID_HTMLDOCUMENT                     = DISPID_NORMAL_FIRST;
        public const int DISPID_DATATRANSFER                     = DISPID_NORMAL_FIRST;
        public const int DISPID_XMLDECL                          = DISPID_NORMAL_FIRST;
        public const int DISPID_DOCFRAG                          = DISPID_NORMAL_FIRST;
        public const int DISPID_ILINEINFO                        = DISPID_NORMAL_FIRST;

        #endregion

        #region Base Event Constants
        public const int DISPID_CLICK = (-600);
        public const int DISPID_DBLCLICK  = (-601);
        public const int DISPID_KEYDOWN = (-602);
        public const int DISPID_KEYPRESS  = (-603);
        public const int DISPID_KEYUP = (-604);
        public const int DISPID_MOUSEDOWN  =  (-605);
        public const int DISPID_MOUSEMOVE  =  (-606);
        public const int DISPID_MOUSEUP  =  (-607);
        public const int DISPID_ERROREVENT =  (-608);
        public const int DISPID_READYSTATECHANGE =  (-609);
        public const int DISPID_CLICK_VALUE = (-610);
        public const int DISPID_RIGHTTOLEFT = (-611);
        public const int DISPID_TOPTOBOTTOM = (-612);
        public const int DISPID_THIS  = (-613);

        //  Standard dispatch ID constants
        public const int DISPID_ONABORT  =  (DISPID_NORMAL_FIRST);
        public const int DISPID_ONCHANGE  = (DISPID_NORMAL_FIRST + 1);
        public const int DISPID_ONERROR  =  (DISPID_NORMAL_FIRST + 2);
        public const int DISPID_ONLOAD  = (DISPID_NORMAL_FIRST + 3);
        public const int DISPID_ONSELECT =  (DISPID_NORMAL_FIRST + 6);
        public const int DISPID_ONSUBMIT  = (DISPID_NORMAL_FIRST + 7);
        public const int DISPID_ONUNLOAD =  (DISPID_NORMAL_FIRST + 8);
        public const int DISPID_ONBOUNCE  = (DISPID_NORMAL_FIRST + 9);
        public const int DISPID_ONFINISH  = (DISPID_NORMAL_FIRST + 10);
        public const int DISPID_ONSTART = (DISPID_NORMAL_FIRST + 11);
        public const int DISPID_ONLAYOUT  = (DISPID_NORMAL_FIRST + 13);
        public const int DISPID_ONSCROLL  = (DISPID_NORMAL_FIRST + 14);
        public const int DISPID_ONRESET = (DISPID_NORMAL_FIRST + 15);
        public const int DISPID_ONRESIZE  = (DISPID_NORMAL_FIRST + 16);
        public const int DISPID_ONBEFOREUNLOAD =  (DISPID_NORMAL_FIRST + 17);
        public const int DISPID_ONCHANGEFOCUS  =  (DISPID_NORMAL_FIRST + 18);
        public const int DISPID_ONCHANGEBLUR =  (DISPID_NORMAL_FIRST + 19);
        public const int DISPID_ONPERSIST  =  (DISPID_NORMAL_FIRST + 20);
        public const int DISPID_ONPERSISTSAVE  =  (DISPID_NORMAL_FIRST + 21);
        public const int DISPID_ONPERSISTLOAD  =  (DISPID_NORMAL_FIRST + 22);
        public const int DISPID_ONCONTEXTMENU  =  (DISPID_NORMAL_FIRST + 23);
        public const int DISPID_ONBEFOREPRINT  =  (DISPID_NORMAL_FIRST + 24);
        public const int DISPID_ONAFTERPRINT =  (DISPID_NORMAL_FIRST + 25);
        public const int DISPID_ONSTOP =  (DISPID_NORMAL_FIRST + 26);
        public const int DISPID_ONBEFOREEDITFOCUS = (DISPID_NORMAL_FIRST + 27);
        public const int DISPID_ONMOUSEHOVER =  (DISPID_NORMAL_FIRST + 28);
        public const int DISPID_ONCONTENTREADY =  (DISPID_NORMAL_FIRST + 29);
        public const int DISPID_ONLAYOUTCOMPLETE  = (DISPID_NORMAL_FIRST + 30);
        public const int DISPID_ONPAGE  = (DISPID_NORMAL_FIRST + 31);
        public const int DISPID_ONLINKEDOVERFLOW  = (DISPID_NORMAL_FIRST + 32);
        public const int DISPID_ONMOUSEWHEEL  = (DISPID_NORMAL_FIRST + 33);
        public const int DISPID_ONBEFOREDEACTIVATE  = (DISPID_NORMAL_FIRST + 34);
        public const int DISPID_ONMOVE  = (DISPID_NORMAL_FIRST + 35);
        public const int DISPID_ONCONTROLSELECT = (DISPID_NORMAL_FIRST + 36);
        public const int DISPID_ONSELECTIONCHANGE = (DISPID_NORMAL_FIRST + 37);
        public const int DISPID_ONMOVESTART = (DISPID_NORMAL_FIRST + 38);
        public const int DISPID_ONMOVEEND = (DISPID_NORMAL_FIRST + 39);
        public const int DISPID_ONRESIZESTART = (DISPID_NORMAL_FIRST + 40);
        public const int DISPID_ONRESIZEEND = (DISPID_NORMAL_FIRST + 41);
        public const int DISPID_ONMOUSEENTER  = (DISPID_NORMAL_FIRST + 42);
        public const int DISPID_ONMOUSELEAVE  = (DISPID_NORMAL_FIRST + 43);
        public const int DISPID_ONACTIVATE  = (DISPID_NORMAL_FIRST + 44);
        public const int DISPID_ONDEACTIVATE  = (DISPID_NORMAL_FIRST + 45);
        public const int DISPID_ONMULTILAYOUTCLEANUP =  (DISPID_NORMAL_FIRST + 46);
        public const int DISPID_ONBEFOREACTIVATE  = (DISPID_NORMAL_FIRST + 47);
        public const int DISPID_ONFOCUSIN = (DISPID_NORMAL_FIRST + 48);
        public const int DISPID_ONFOCUSOUT  = (DISPID_NORMAL_FIRST + 49);
        public const int  DISPID_EVPROP_ONMOUSEOVER =  (DISPID_EVENTS +  0);
        public const int  DISPID_EVMETH_ONMOUSEOVER = STDDISPID_XOBJ_ONMOUSEOVER;
        public const int  DISPID_EVPROP_ONMOUSEOUT  =  (DISPID_EVENTS +  1);
        public const int  DISPID_EVMETH_ONMOUSEOUT  = STDDISPID_XOBJ_ONMOUSEOUT;
        public const int  DISPID_EVPROP_ONMOUSEDOWN =  (DISPID_EVENTS +  2);
        public const int  DISPID_EVMETH_ONMOUSEDOWN = DISPID_MOUSEDOWN;
        public const int  DISPID_EVPROP_ONMOUSEUP =  (DISPID_EVENTS +  3);
        public const int  DISPID_EVMETH_ONMOUSEUP = DISPID_MOUSEUP;
        public const int  DISPID_EVPROP_ONMOUSEMOVE =  (DISPID_EVENTS +  4);
        public const int  DISPID_EVMETH_ONMOUSEMOVE = DISPID_MOUSEMOVE;
        public const int  DISPID_EVPROP_ONKEYDOWN =  (DISPID_EVENTS +  5);
        public const int  DISPID_EVMETH_ONKEYDOWN = DISPID_KEYDOWN;
        public const int  DISPID_EVPROP_ONKEYUP =  (DISPID_EVENTS +  6);
        public const int  DISPID_EVMETH_ONKEYUP = DISPID_KEYUP;
        public const int  DISPID_EVPROP_ONKEYPRESS  =  (DISPID_EVENTS +  7);
        public const int  DISPID_EVMETH_ONKEYPRESS  = DISPID_KEYPRESS;
        public const int  DISPID_EVPROP_ONCLICK =  (DISPID_EVENTS +  8);
        public const int  DISPID_EVMETH_ONCLICK = DISPID_CLICK;
        public const int  DISPID_EVPROP_ONDBLCLICK = (DISPID_EVENTS +  9);
        public const int  DISPID_EVMETH_ONDBLCLICK =  DISPID_DBLCLICK;
        public const int  DISPID_EVPROP_ONSELECT = (DISPID_EVENTS + 10);
        public const int  DISPID_EVMETH_ONSELECT =  DISPID_ONSELECT;
        public const int  DISPID_EVPROP_ONSUBMIT = (DISPID_EVENTS + 11);
        public const int  DISPID_EVMETH_ONSUBMIT =  DISPID_ONSUBMIT;
        public const int  DISPID_EVPROP_ONRESET  = (DISPID_EVENTS + 12);
        public const int  DISPID_EVMETH_ONRESET  =  DISPID_ONRESET;
        public const int  DISPID_EVPROP_ONHELP = (DISPID_EVENTS + 13);
        public const int  DISPID_EVMETH_ONHELP =  STDDISPID_XOBJ_ONHELP;
        public const int  DISPID_EVPROP_ONFOCUS  = (DISPID_EVENTS + 14);
        public const int  DISPID_EVMETH_ONFOCUS  =  STDDISPID_XOBJ_ONFOCUS;
        public const int  DISPID_EVPROP_ONBLUR = (DISPID_EVENTS + 15);
        public const int  DISPID_EVMETH_ONBLUR =  STDDISPID_XOBJ_ONBLUR;
        public const int  DISPID_EVPROP_ONROWEXIT  = (DISPID_EVENTS + 18);
        public const int  DISPID_EVMETH_ONROWEXIT  =  STDDISPID_XOBJ_ONROWEXIT;
        public const int  DISPID_EVPROP_ONROWENTER = (DISPID_EVENTS + 19);
        public const int  DISPID_EVMETH_ONROWENTER =  STDDISPID_XOBJ_ONROWENTER;
        public const int  DISPID_EVPROP_ONBOUNCE = (DISPID_EVENTS + 20);
        public const int  DISPID_EVMETH_ONBOUNCE =  DISPID_ONBOUNCE;
        public const int  DISPID_EVPROP_ONBEFOREUPDATE = (DISPID_EVENTS + 21);
        public const int  DISPID_EVMETH_ONBEFOREUPDATE =  STDDISPID_XOBJ_BEFOREUPDATE;
        public const int  DISPID_EVPROP_ONAFTERUPDATE  = (DISPID_EVENTS + 22);
        public const int  DISPID_EVMETH_ONAFTERUPDATE  =  STDDISPID_XOBJ_AFTERUPDATE;
        public const int  DISPID_EVPROP_ONBEFOREDRAGOVER = (DISPID_EVENTS + 23);
        public const int  DISPID_EVPROP_ONBEFOREDROPORPASTE =  (DISPID_EVENTS + 24);
        public const int  DISPID_EVPROP_ONREADYSTATECHANGE = (DISPID_EVENTS + 25);
        public const int  DISPID_EVMETH_ONREADYSTATECHANGE =  DISPID_READYSTATECHANGE;
        public const int  DISPID_EVPROP_ONFINISH = (DISPID_EVENTS + 26);
        public const int  DISPID_EVMETH_ONFINISH =  DISPID_ONFINISH;
        public const int  DISPID_EVPROP_ONSTART  = (DISPID_EVENTS + 27);
        public const int  DISPID_EVMETH_ONSTART  =  DISPID_ONSTART;
        public const int  DISPID_EVPROP_ONABORT  = (DISPID_EVENTS + 28);
        public const int  DISPID_EVMETH_ONABORT  =  DISPID_ONABORT;
        public const int  DISPID_EVPROP_ONERROR  = (DISPID_EVENTS + 29);
        public const int  DISPID_EVMETH_ONERROR  =  DISPID_ONERROR;
        public const int  DISPID_EVPROP_ONCHANGE = (DISPID_EVENTS + 30);
        public const int  DISPID_EVMETH_ONCHANGE =  DISPID_ONCHANGE;
        public const int  DISPID_EVPROP_ONSCROLL = (DISPID_EVENTS + 31);
        public const int  DISPID_EVMETH_ONSCROLL =  DISPID_ONSCROLL;
        public const int  DISPID_EVPROP_ONLOAD = (DISPID_EVENTS + 32);
        public const int  DISPID_EVMETH_ONLOAD =  DISPID_ONLOAD;
        public const int  DISPID_EVPROP_ONUNLOAD = (DISPID_EVENTS + 33);
        public const int  DISPID_EVMETH_ONUNLOAD =  DISPID_ONUNLOAD;
        public const int  DISPID_EVPROP_ONLAYOUT = (DISPID_EVENTS + 34);
        public const int  DISPID_EVMETH_ONLAYOUT =  DISPID_ONLAYOUT;
        public const int  DISPID_EVPROP_ONDRAGSTART =  (DISPID_EVENTS + 35);
        public const int  DISPID_EVMETH_ONDRAGSTART = STDDISPID_XOBJ_ONDRAGSTART;
        public const int  DISPID_EVPROP_ONRESIZE = (DISPID_EVENTS + 36);
        public const int  DISPID_EVMETH_ONRESIZE =  DISPID_ONRESIZE;
        public const int  DISPID_EVPROP_ONSELECTSTART =  (DISPID_EVENTS + 37);
        public const int  DISPID_EVMETH_ONSELECTSTART = STDDISPID_XOBJ_ONSELECTSTART;
        public const int  DISPID_EVPROP_ONERRORUPDATE =  (DISPID_EVENTS + 38);
        public const int  DISPID_EVMETH_ONERRORUPDATE = STDDISPID_XOBJ_ERRORUPDATE;
        public const int  DISPID_EVPROP_ONBEFOREUNLOAD = (DISPID_EVENTS + 39);
        public const int  DISPID_EVMETH_ONCONTEXTMENU   =       DISPID_ONCONTEXTMENU;
        public const int DISPID_EVMETH_ONSTOP    =            DISPID_ONSTOP;
        public const int DISPID_EVMETH_ONROWSDELETE      =     STDDISPID_XOBJ_ONROWSDELETE;
        public const int DISPID_EVMETH_ONROWSINSERTED      =   STDDISPID_XOBJ_ONROWSINSERTED;
        public const int DISPID_EVMETH_ONCELLCHANGE      =     STDDISPID_XOBJ_ONCELLCHANGE;
        public const int DISPID_EVMETH_ONPROPERTYCHANGE   =    STDDISPID_XOBJ_ONPROPERTYCHANGE;
        public const int DISPID_EVMETH_ONDATAAVAILABLE    =    STDDISPID_XOBJ_ONDATAAVAILABLE;
        public const int DISPID_EVMETH_ONDATASETCOMPLETE   =   STDDISPID_XOBJ_ONDATASETCOMPLETE;
        public const int  DISPID_EVMETH_ONBEFOREUNLOAD  = DISPID_ONBEFOREUNLOAD;
        public const int  DISPID_EVPROP_ONDATASETCHANGED = (DISPID_EVENTS + 40);
        public const int  DISPID_EVMETH_ONDATASETCHANGED =  STDDISPID_XOBJ_ONDATASETCHANGED;
        public const int  DISPID_EVPROP_ONDATAAVAILABLE  = (DISPID_EVENTS + 41);
        public const int  DISPID_EVPROP_ONDATASETCOMPLETE = (DISPID_EVENTS + 42);
        public const int  DISPID_EVPROP_ONFILTER = (DISPID_EVENTS + 43);
        public const int  DISPID_EVPROP_ONCHANGEFOCUS =  (DISPID_EVENTS + 44);
        public const int  DISPID_EVPROP_ONCHANGEBLUR  =  (DISPID_EVENTS + 45);
        public const int  DISPID_EVPROP_ONLOSECAPTURE =  (DISPID_EVENTS + 46);
        public const int  DISPID_EVPROP_ONPROPERTYCHANGE = (DISPID_EVENTS + 47);
        public const int  DISPID_EVPROP_ONPERSISTSAVE =  (DISPID_EVENTS + 48);
        public const int  DISPID_EVPROP_ONDRAG  =  (DISPID_EVENTS + 49);
        public const int  DISPID_EVPROP_ONDRAGEND =  (DISPID_EVENTS + 50);
        public const int  DISPID_EVPROP_ONDRAGENTER =  (DISPID_EVENTS + 51);
        public const int  DISPID_EVPROP_ONDRAGOVER  =  (DISPID_EVENTS + 52);
        public const int  DISPID_EVPROP_ONDRAGLEAVE =  (DISPID_EVENTS + 53);
        public const int  DISPID_EVPROP_ONDROP  =  (DISPID_EVENTS + 54);
        public const int  DISPID_EVPROP_ONCUT =  (DISPID_EVENTS + 55);
        public const int  DISPID_EVPROP_ONCOPY  =  (DISPID_EVENTS + 56);
        public const int  DISPID_EVPROP_ONPASTE =  (DISPID_EVENTS + 57);
        public const int  DISPID_EVPROP_ONBEFORECUT =  (DISPID_EVENTS + 58);
        public const int  DISPID_EVPROP_ONBEFORECOPY  =  (DISPID_EVENTS + 59);
        public const int  DISPID_EVPROP_ONBEFOREPASTE =  (DISPID_EVENTS + 60);
        public const int  DISPID_EVPROP_ONPERSISTLOAD  = (DISPID_EVENTS + 61);
        public const int  DISPID_EVPROP_ONROWSDELETE = (DISPID_EVENTS + 62);
        public const int  DISPID_EVPROP_ONROWSINSERTED = (DISPID_EVENTS + 63);
        public const int  DISPID_EVPROP_ONCELLCHANGE = (DISPID_EVENTS + 64);
        public const int  DISPID_EVPROP_ONCONTEXTMENU  = (DISPID_EVENTS + 65);
        public const int  DISPID_EVPROP_ONBEFOREPRINT  = (DISPID_EVENTS + 66);
        public const int  DISPID_EVPROP_ONAFTERPRINT = (DISPID_EVENTS + 67);
        public const int  DISPID_EVPROP_ONSTOP = (DISPID_EVENTS + 68);
        public const int  DISPID_EVPROP_ONBEFOREEDITFOCUS =  (DISPID_EVENTS + 69);
        public const int  DISPID_EVMETH_ONBEFOREEDITFOCUS = DISPID_ONBEFOREEDITFOCUS;
        public const int  DISPID_EVPROP_ONATTACHEVENT =  (DISPID_EVENTS + 70);
        public const int  DISPID_EVPROP_ONMOUSEHOVER  =  (DISPID_EVENTS + 71);
        public const int  DISPID_EVMETH_ONMOUSEHOVER  = DISPID_ONMOUSEHOVER;
        public const int  DISPID_EVPROP_ONCONTENTREADY  =  (DISPID_EVENTS + 72);
        public const int  DISPID_EVMETH_ONCONTENTREADY  = DISPID_ONCONTENTREADY;
        public const int  DISPID_EVPROP_ONLAYOUTCOMPLETE = (DISPID_EVENTS + 73);
        public const int  DISPID_EVMETH_ONLAYOUTCOMPLETE =  DISPID_ONLAYOUTCOMPLETE;
        public const int  DISPID_EVPROP_ONPAGE = (DISPID_EVENTS + 74);
        public const int  DISPID_EVMETH_ONPAGE =  DISPID_ONPAGE;
        public const int  DISPID_EVPROP_ONLINKEDOVERFLOW = (DISPID_EVENTS + 75);
        public const int  DISPID_EVMETH_ONLINKEDOVERFLOW =  DISPID_ONLINKEDOVERFLOW;
        public const int  DISPID_EVPROP_ONMOUSEWHEEL = (DISPID_EVENTS + 76);
        public const int  DISPID_EVMETH_ONMOUSEWHEEL =  DISPID_ONMOUSEWHEEL;
        public const int  DISPID_EVPROP_ONBEFOREDEACTIVATE = (DISPID_EVENTS + 77);
        public const int  DISPID_EVMETH_ONBEFOREDEACTIVATE =  DISPID_ONBEFOREDEACTIVATE;
        public const int  DISPID_EVPROP_ONMOVE = (DISPID_EVENTS + 78);
        public const int  DISPID_EVMETH_ONMOVE =  DISPID_ONMOVE;
        public const int  DISPID_EVPROP_ONCONTROLSELECT  = (DISPID_EVENTS + 79);
        public const int  DISPID_EVMETH_ONCONTROLSELECT  =  DISPID_ONCONTROLSELECT;
        public const int  DISPID_EVPROP_ONSELECTIONCHANGE  = (DISPID_EVENTS + 80);
        public const int  DISPID_EVMETH_ONSELECTIONCHANGE  =  DISPID_ONSELECTIONCHANGE;
        public const int  DISPID_EVPROP_ONMOVESTART  = (DISPID_EVENTS + 81);
        public const int  DISPID_EVMETH_ONMOVESTART  =  DISPID_ONMOVESTART;
        public const int  DISPID_EVPROP_ONMOVEEND  = (DISPID_EVENTS + 82);
        public const int  DISPID_EVMETH_ONMOVEEND  =  DISPID_ONMOVEEND;
        public const int  DISPID_EVPROP_ONRESIZESTART  = (DISPID_EVENTS + 83);
        public const int  DISPID_EVMETH_ONRESIZESTART  =  DISPID_ONRESIZESTART;
        public const int  DISPID_EVPROP_ONRESIZEEND  = (DISPID_EVENTS + 84);
        public const int  DISPID_EVMETH_ONRESIZEEND  =  DISPID_ONRESIZEEND;
        public const int  DISPID_EVPROP_ONMOUSEENTER = (DISPID_EVENTS + 85);
        public const int  DISPID_EVMETH_ONMOUSEENTER =  DISPID_ONMOUSEENTER;
        public const int  DISPID_EVPROP_ONMOUSELEAVE = (DISPID_EVENTS + 86);
        public const int  DISPID_EVMETH_ONMOUSELEAVE =  DISPID_ONMOUSELEAVE;
        public const int  DISPID_EVPROP_ONACTIVATE = (DISPID_EVENTS + 87);
        public const int  DISPID_EVMETH_ONACTIVATE =  DISPID_ONACTIVATE;
        public const int  DISPID_EVPROP_ONDEACTIVATE = (DISPID_EVENTS + 88);
        public const int  DISPID_EVMETH_ONDEACTIVATE =  DISPID_ONDEACTIVATE;
        public const int  DISPID_EVPROP_ONMULTILAYOUTCLEANUP = (DISPID_EVENTS + 89);
        public const int  DISPID_EVMETH_ONMULTILAYOUTCLEANUP =  DISPID_ONMULTILAYOUTCLEANUP;
        public const int  DISPID_EVPROP_ONBEFOREACTIVATE  =  (DISPID_EVENTS + 90);
        public const int  DISPID_EVMETH_ONBEFOREACTIVATE  = DISPID_ONBEFOREACTIVATE;
        public const int  DISPID_EVPROP_ONFOCUSIN =  (DISPID_EVENTS + 91);
        public const int  DISPID_EVMETH_ONFOCUSIN = DISPID_ONFOCUSIN;
        public const int  DISPID_EVPROP_ONFOCUSOUT  =  (DISPID_EVENTS + 92);
        public const int  DISPID_EVMETH_ONFOCUSOUT  = DISPID_ONFOCUSOUT;
        public const int DISPID_EVMETH_ONFILTER        =       STDDISPID_XOBJ_ONFILTER;
        public const int DISPID_EVMETH_ONLOSECAPTURE     =     STDDISPID_XOBJ_ONLOSECAPTURE;
        public const int DISPID_EVMETH_ONDRAG         =        STDDISPID_XOBJ_ONDRAG;
        public const int DISPID_EVMETH_ONDRAGEND      =        STDDISPID_XOBJ_ONDRAGEND;
        public const int DISPID_EVMETH_ONDRAGENTER     =       STDDISPID_XOBJ_ONDRAGENTER;
        public const int DISPID_EVMETH_ONDRAGLEAVE    =        STDDISPID_XOBJ_ONDRAGLEAVE;
        public const int DISPID_EVMETH_ONDRAGOVER      =       STDDISPID_XOBJ_ONDRAGOVER;
        public const int DISPID_EVMETH_ONDROP         =        STDDISPID_XOBJ_ONDROP;
        public const int DISPID_EVMETH_ONCUT          =        STDDISPID_XOBJ_ONCUT;
        public const int DISPID_EVMETH_ONBEFORECUT    =        STDDISPID_XOBJ_ONBEFORECUT;
        public const int DISPID_EVMETH_ONBEFORECOPY    =       STDDISPID_XOBJ_ONBEFORECOPY;
        public const int DISPID_EVMETH_ONCOPY           =      STDDISPID_XOBJ_ONCOPY;
        public const int DISPID_EVMETH_ONBEFOREPASTE    =      STDDISPID_XOBJ_ONBEFOREPASTE;
        public const int DISPID_EVMETH_ONPASTE         =       STDDISPID_XOBJ_ONPASTE;
        #endregion

        #region DISPIDs for AMBIENT_DLCONTROL constants
        [CLSCompliant(false)] 
        public const int DLCTL_DLIMAGES   =                       0x00000010;
        [CLSCompliant(false)] 
        public const int DLCTL_VIDEOS      =                      0x00000020;
        [CLSCompliant(false)] 
        public const int DLCTL_BGSOUNDS    =                      0x00000040;
        [CLSCompliant(false)] 
        public const int DLCTL_NO_SCRIPTS   =                     0x00000080;
        [CLSCompliant(false)] 
        public const int DLCTL_NO_JAVA     =                      0x00000100;
        [CLSCompliant(false)] 
        public const int DLCTL_NO_RUNACTIVEXCTLS   =              0x00000200;
        [CLSCompliant(false)] 
        public const int DLCTL_NO_DLACTIVEXCTLS   =               0x00000400;
        [CLSCompliant(false)] 
        public const int DLCTL_DOWNLOADONLY  =                    0x00000800;
        [CLSCompliant(false)] 
        public const int DLCTL_NO_FRAMEDOWNLOAD  =                0x00001000;
        [CLSCompliant(false)] 
        public const int DLCTL_RESYNCHRONIZE   =                  0x00002000;
        [CLSCompliant(false)] 
        public const int DLCTL_PRAGMA_NO_CACHE    =               0x00004000;
        [CLSCompliant(false)] 
        public const int DLCTL_NO_BEHAVIORS  =                    0x00008000;
        [CLSCompliant(false)] 
        public const int DLCTL_NO_METACHARSET   =                 0x00010000;
        [CLSCompliant(false)] 
        public const int DLCTL_URL_ENCODING_DISABLE_UTF8  =       0x00020000;
        [CLSCompliant(false)] 
        public const int DLCTL_URL_ENCODING_ENABLE_UTF8  =        0x00040000;
        [CLSCompliant(false)] 
        public const int DLCTL_NOFRAMES     =                     0x00080000;
        [CLSCompliant(false)] 
        public const int DLCTL_FORCEOFFLINE      =                0x10000000;
        [CLSCompliant(false)] 
        public const int DLCTL_NO_CLIENTPULL    =                 0x20000000;
        [CLSCompliant(false)] 
        public const int DLCTL_SILENT  =                          0x40000000;
        [CLSCompliant(false)] 
        public const uint DLCTL_OFFLINEIFNOTCONNECTED     =        0x80000000;
        [CLSCompliant(false)] 
        public const uint DLCTL_OFFLINE    =                       0x80000000;
        #endregion

        #region DISPIDs for interface IHTMLBodyElement
        public const int DISPID_IHTMLBODYELEMENT_BACKGROUND          =              DISPID_A_BACKGROUNDIMAGE;
        public const int DISPID_IHTMLBODYELEMENT_BGPROPERTIES        =              DISPID_A_BACKGROUNDATTACHMENT;
        public const int DISPID_IHTMLBODYELEMENT_LEFTMARGIN          =              DISPID_A_MARGINLEFT;
        public const int DISPID_IHTMLBODYELEMENT_TOPMARGIN           =              DISPID_A_MARGINTOP;
        public const int DISPID_IHTMLBODYELEMENT_RIGHTMARGIN         =              DISPID_A_MARGINRIGHT;
        public const int DISPID_IHTMLBODYELEMENT_BOTTOMMARGIN        =              DISPID_A_MARGINBOTTOM;
        public const int DISPID_IHTMLBODYELEMENT_NOWRAP              =              DISPID_A_NOWRAP;
        public const int DISPID_IHTMLBODYELEMENT_BGCOLOR             =              DISPID_BACKCOLOR;
        public const int DISPID_IHTMLBODYELEMENT_TEXT                =              DISPID_A_COLOR;
        public const int DISPID_IHTMLBODYELEMENT_LINK               =               DISPID_BODY+10;
        public const int DISPID_IHTMLBODYELEMENT_VLINK              =               DISPID_BODY+12;
        public const int DISPID_IHTMLBODYELEMENT_ALINK              =               DISPID_BODY+11;
        public const int DISPID_IHTMLBODYELEMENT_ONLOAD             =               DISPID_EVPROP_ONLOAD;
        public const int DISPID_IHTMLBODYELEMENT_ONUNLOAD           =               DISPID_EVPROP_ONUNLOAD;
        public const int DISPID_IHTMLBODYELEMENT_SCROLL             =               DISPID_A_SCROLL;
        public const int DISPID_IHTMLBODYELEMENT_ONSELECT            =              DISPID_EVPROP_ONSELECT;
        public const int DISPID_IHTMLBODYELEMENT_ONBEFOREUNLOAD        =            DISPID_EVPROP_ONBEFOREUNLOAD;
        public const int DISPID_IHTMLBODYELEMENT_CREATETEXTRANGE       =            DISPID_BODY+13;
        #endregion

        #region DISPIDs for interface IHTMLComputedStyle
        public const int DISPID_IHTMLCOMPUTEDSTYLE =								DISPID_NORMAL_FIRST;
        public const int DISPID_IHTMLCOMPUTEDSTYLE_BOLD              =              DISPID_IHTMLCOMPUTEDSTYLE+1;
        public const int DISPID_IHTMLCOMPUTEDSTYLE_ITALIC             =             DISPID_IHTMLCOMPUTEDSTYLE+2;
        public const int DISPID_IHTMLCOMPUTEDSTYLE_UNDERLINE          =             DISPID_IHTMLCOMPUTEDSTYLE+3;
        public const int DISPID_IHTMLCOMPUTEDSTYLE_OVERLINE           =             DISPID_IHTMLCOMPUTEDSTYLE+4;
        public const int DISPID_IHTMLCOMPUTEDSTYLE_STRIKEOUT          =             DISPID_IHTMLCOMPUTEDSTYLE+5;
        public const int DISPID_IHTMLCOMPUTEDSTYLE_SUBSCRIPT         =              DISPID_IHTMLCOMPUTEDSTYLE+6;
        public const int DISPID_IHTMLCOMPUTEDSTYLE_SUPERSCRIPT       =              DISPID_IHTMLCOMPUTEDSTYLE+7;
        public const int DISPID_IHTMLCOMPUTEDSTYLE_EXPLICITFACE     =               DISPID_IHTMLCOMPUTEDSTYLE+8;
        public const int DISPID_IHTMLCOMPUTEDSTYLE_FONTWEIGHT       =               DISPID_IHTMLCOMPUTEDSTYLE+9;
        public const int DISPID_IHTMLCOMPUTEDSTYLE_FONTSIZE         =               DISPID_IHTMLCOMPUTEDSTYLE+10;
        public const int DISPID_IHTMLCOMPUTEDSTYLE_FONTNAME         =               DISPID_IHTMLCOMPUTEDSTYLE+11;
        public const int DISPID_IHTMLCOMPUTEDSTYLE_HASBGCOLOR         =             DISPID_IHTMLCOMPUTEDSTYLE+12;
        public const int DISPID_IHTMLCOMPUTEDSTYLE_TEXTCOLOR         =              DISPID_IHTMLCOMPUTEDSTYLE+13;
        public const int DISPID_IHTMLCOMPUTEDSTYLE_BACKGROUNDCOLOR    =             DISPID_IHTMLCOMPUTEDSTYLE+14;
        public const int DISPID_IHTMLCOMPUTEDSTYLE_PREFORMATTED       =             DISPID_IHTMLCOMPUTEDSTYLE+15;
        public const int DISPID_IHTMLCOMPUTEDSTYLE_DIRECTION          =             DISPID_IHTMLCOMPUTEDSTYLE+16;
        public const int DISPID_IHTMLCOMPUTEDSTYLE_BLOCKDIRECTION     =             DISPID_IHTMLCOMPUTEDSTYLE+17;
        public const int DISPID_IHTMLCOMPUTEDSTYLE_OL                 =             DISPID_IHTMLCOMPUTEDSTYLE+18;
        #endregion

        #region DISPIDs for interface IHTMLDocument
        public const int DISPID_IHTMLDOCUMENT_SCRIPT =						DISPID_OMDOCUMENT+1;
        #endregion

        #region DISPIDs for interface IHTMLDocument2
        public const int DISPID_IHTMLDOCUMENT2_ALL   =                              DISPID_OMDOCUMENT+3;
        public const int DISPID_IHTMLDOCUMENT2_BODY         =                       DISPID_OMDOCUMENT+4;
        public const int DISPID_IHTMLDOCUMENT2_ACTIVEELEMENT       =                DISPID_OMDOCUMENT+5;
        public const int DISPID_IHTMLDOCUMENT2_IMAGES            =                  DISPID_OMDOCUMENT+11;
        public const int DISPID_IHTMLDOCUMENT2_APPLETS           =                  DISPID_OMDOCUMENT+8;
        public const int DISPID_IHTMLDOCUMENT2_LINKS             =                  DISPID_OMDOCUMENT+9;
        public const int DISPID_IHTMLDOCUMENT2_FORMS              =                 DISPID_OMDOCUMENT+10;
        public const int DISPID_IHTMLDOCUMENT2_ANCHORS            =                 DISPID_OMDOCUMENT+7;
        public const int DISPID_IHTMLDOCUMENT2_TITLE               =                DISPID_OMDOCUMENT+12;
        public const int DISPID_IHTMLDOCUMENT2_SCRIPTS             =                DISPID_OMDOCUMENT+13;
        public const int DISPID_IHTMLDOCUMENT2_DESIGNMODE          =                DISPID_OMDOCUMENT+14;
        public const int DISPID_IHTMLDOCUMENT2_SELECTION           =                DISPID_OMDOCUMENT+17;
        public const int DISPID_IHTMLDOCUMENT2_READYSTATE         =                 DISPID_OMDOCUMENT+18;
        public const int DISPID_IHTMLDOCUMENT2_FRAMES             =                 DISPID_OMDOCUMENT+19;
        public const int DISPID_IHTMLDOCUMENT2_EMBEDS             =                 DISPID_OMDOCUMENT+15;
        public const int DISPID_IHTMLDOCUMENT2_PLUGINS            =                 DISPID_OMDOCUMENT+21;
        public const int DISPID_IHTMLDOCUMENT2_ALINKCOLOR         =                 DISPID_OMDOCUMENT+22;
        public const int DISPID_IHTMLDOCUMENT2_BGCOLOR             =                DISPID_BACKCOLOR;
        public const int DISPID_IHTMLDOCUMENT2_FGCOLOR              =               DISPID_A_COLOR;
        public const int DISPID_IHTMLDOCUMENT2_LINKCOLOR          =                 DISPID_OMDOCUMENT+24;
        public const int DISPID_IHTMLDOCUMENT2_VLINKCOLOR          =                DISPID_OMDOCUMENT+23;
        public const int DISPID_IHTMLDOCUMENT2_REFERRER            =                DISPID_OMDOCUMENT+27;
        public const int DISPID_IHTMLDOCUMENT2_LOCATION            =                DISPID_OMDOCUMENT+26;
        public const int DISPID_IHTMLDOCUMENT2_LASTMODIFIED        =                DISPID_OMDOCUMENT+28;
        public const int DISPID_IHTMLDOCUMENT2_URL                 =                DISPID_OMDOCUMENT+25;
        public const int DISPID_IHTMLDOCUMENT2_DOMAIN              =                DISPID_OMDOCUMENT+29;
        public const int DISPID_IHTMLDOCUMENT2_COOKIE              =                DISPID_OMDOCUMENT+30;
        public const int DISPID_IHTMLDOCUMENT2_EXPANDO             =                DISPID_OMDOCUMENT+31;
        public const int DISPID_IHTMLDOCUMENT2_CHARSET               =              DISPID_OMDOCUMENT+32;
        public const int DISPID_IHTMLDOCUMENT2_DEFAULTCHARSET         =             DISPID_OMDOCUMENT+33;
        public const int DISPID_IHTMLDOCUMENT2_MIMETYPE               =             DISPID_OMDOCUMENT+41;
        public const int DISPID_IHTMLDOCUMENT2_FILESIZE               =             DISPID_OMDOCUMENT+42;
        public const int DISPID_IHTMLDOCUMENT2_FILECREATEDDATE        =             DISPID_OMDOCUMENT+43;
        public const int DISPID_IHTMLDOCUMENT2_FILEMODIFIEDDATE       =             DISPID_OMDOCUMENT+44;
        public const int DISPID_IHTMLDOCUMENT2_FILEUPDATEDDATE        =             DISPID_OMDOCUMENT+45;
        public const int DISPID_IHTMLDOCUMENT2_SECURITY               =             DISPID_OMDOCUMENT+46;
        public const int DISPID_IHTMLDOCUMENT2_PROTOCOL               =             DISPID_OMDOCUMENT+47;
        public const int DISPID_IHTMLDOCUMENT2_NAMEPROP               =             DISPID_OMDOCUMENT+48;
        public const int DISPID_IHTMLDOCUMENT2_WRITE                  =             DISPID_OMDOCUMENT+54;
        public const int DISPID_IHTMLDOCUMENT2_WRITELN                =             DISPID_OMDOCUMENT+55;
        public const int DISPID_IHTMLDOCUMENT2_OPEN                   =             DISPID_OMDOCUMENT+56;
        public const int DISPID_IHTMLDOCUMENT2_CLOSE                  =             DISPID_OMDOCUMENT+57;
        public const int DISPID_IHTMLDOCUMENT2_CLEAR                  =             DISPID_OMDOCUMENT+58;
        public const int DISPID_IHTMLDOCUMENT2_QUERYCOMMANDSUPPORTED       =        DISPID_OMDOCUMENT+59;
        public const int DISPID_IHTMLDOCUMENT2_QUERYCOMMANDENABLED      =           DISPID_OMDOCUMENT+60;
        public const int DISPID_IHTMLDOCUMENT2_QUERYCOMMANDSTATE        =           DISPID_OMDOCUMENT+61;
        public const int DISPID_IHTMLDOCUMENT2_QUERYCOMMANDINDETERM     =           DISPID_OMDOCUMENT+62;
        public const int DISPID_IHTMLDOCUMENT2_QUERYCOMMANDTEXT         =           DISPID_OMDOCUMENT+63;
        public const int DISPID_IHTMLDOCUMENT2_QUERYCOMMANDVALUE       =            DISPID_OMDOCUMENT+64;
        public const int DISPID_IHTMLDOCUMENT2_EXECCOMMAND             =            DISPID_OMDOCUMENT+65;
        public const int DISPID_IHTMLDOCUMENT2_EXECCOMMANDSHOWHELP     =            DISPID_OMDOCUMENT+66;
        public const int DISPID_IHTMLDOCUMENT2_CREATEELEMENT           =            DISPID_OMDOCUMENT+67;
        public const int DISPID_IHTMLDOCUMENT2_ONHELP                  =            DISPID_EVPROP_ONHELP;
        public const int DISPID_IHTMLDOCUMENT2_ONCLICK                =             DISPID_EVPROP_ONCLICK;
        public const int DISPID_IHTMLDOCUMENT2_ONDBLCLICK            =              DISPID_EVPROP_ONDBLCLICK;
        public const int DISPID_IHTMLDOCUMENT2_ONKEYUP               =              DISPID_EVPROP_ONKEYUP;
        public const int DISPID_IHTMLDOCUMENT2_ONKEYDOWN             =              DISPID_EVPROP_ONKEYDOWN;
        public const int DISPID_IHTMLDOCUMENT2_ONKEYPRESS            =              DISPID_EVPROP_ONKEYPRESS;
        public const int DISPID_IHTMLDOCUMENT2_ONMOUSEUP             =              DISPID_EVPROP_ONMOUSEUP;
        public const int DISPID_IHTMLDOCUMENT2_ONMOUSEDOWN           =              DISPID_EVPROP_ONMOUSEDOWN;
        public const int DISPID_IHTMLDOCUMENT2_ONMOUSEMOVE            =             DISPID_EVPROP_ONMOUSEMOVE;
        public const int DISPID_IHTMLDOCUMENT2_ONMOUSEOUT            =              DISPID_EVPROP_ONMOUSEOUT;
        public const int DISPID_IHTMLDOCUMENT2_ONMOUSEOVER            =             DISPID_EVPROP_ONMOUSEOVER;
        public const int DISPID_IHTMLDOCUMENT2_ONREADYSTATECHANGE    =              DISPID_EVPROP_ONREADYSTATECHANGE;
        public const int DISPID_IHTMLDOCUMENT2_ONAFTERUPDATE        =               DISPID_EVPROP_ONAFTERUPDATE;
        public const int DISPID_IHTMLDOCUMENT2_ONROWEXIT             =              DISPID_EVPROP_ONROWEXIT;
        public const int DISPID_IHTMLDOCUMENT2_ONROWENTER           =               DISPID_EVPROP_ONROWENTER;
        public const int DISPID_IHTMLDOCUMENT2_ONDRAGSTART           =              DISPID_EVPROP_ONDRAGSTART;
        public const int DISPID_IHTMLDOCUMENT2_ONSELECTSTART         =              DISPID_EVPROP_ONSELECTSTART;
        public const int DISPID_IHTMLDOCUMENT2_ELEMENTFROMPOINT      =              DISPID_OMDOCUMENT+68;
        public const int DISPID_IHTMLDOCUMENT2_PARENTWINDOW          =              DISPID_OMDOCUMENT+34;
        public const int DISPID_IHTMLDOCUMENT2_STYLESHEETS          =               DISPID_OMDOCUMENT+69;
        public const int DISPID_IHTMLDOCUMENT2_ONBEFOREUPDATE       =               DISPID_EVPROP_ONBEFOREUPDATE;
        public const int DISPID_IHTMLDOCUMENT2_ONERRORUPDATE        =               DISPID_EVPROP_ONERRORUPDATE;
        public const int DISPID_IHTMLDOCUMENT2_TOSTRING             =               DISPID_OMDOCUMENT+70;
        public const int DISPID_IHTMLDOCUMENT2_CREATESTYLESHEET     =               DISPID_OMDOCUMENT+71;
        #endregion

        #region DISPIDs for event set HTMLDocumentEvents2
        public const int DISPID_HTMLDOCUMENTEVENTS2_ONHELP     =                    DISPID_EVMETH_ONHELP;
        public const int DISPID_HTMLDOCUMENTEVENTS2_ONCLICK       =                 DISPID_EVMETH_ONCLICK;
        public const int DISPID_HTMLDOCUMENTEVENTS2_ONDBLCLICK        =             DISPID_EVMETH_ONDBLCLICK;
        public const int DISPID_HTMLDOCUMENTEVENTS2_ONKEYDOWN          =            DISPID_EVMETH_ONKEYDOWN;
        public const int DISPID_HTMLDOCUMENTEVENTS2_ONKEYUP           =             DISPID_EVMETH_ONKEYUP;
        public const int DISPID_HTMLDOCUMENTEVENTS2_ONKEYPRESS        =             DISPID_EVMETH_ONKEYPRESS;
        public const int DISPID_HTMLDOCUMENTEVENTS2_ONMOUSEDOWN       =             DISPID_EVMETH_ONMOUSEDOWN;
        public const int DISPID_HTMLDOCUMENTEVENTS2_ONMOUSEMOVE       =             DISPID_EVMETH_ONMOUSEMOVE;
        public const int DISPID_HTMLDOCUMENTEVENTS2_ONMOUSEUP         =             DISPID_EVMETH_ONMOUSEUP;
        public const int DISPID_HTMLDOCUMENTEVENTS2_ONMOUSEOUT        =             DISPID_EVMETH_ONMOUSEOUT;
        public const int DISPID_HTMLDOCUMENTEVENTS2_ONMOUSEOVER        =           DISPID_EVMETH_ONMOUSEOVER;
        public const int DISPID_HTMLDOCUMENTEVENTS2_ONREADYSTATECHANGE    =         DISPID_EVMETH_ONREADYSTATECHANGE;
        public const int DISPID_HTMLDOCUMENTEVENTS2_ONBEFOREUPDATE     =            DISPID_EVMETH_ONBEFOREUPDATE;
        public const int DISPID_HTMLDOCUMENTEVENTS2_ONAFTERUPDATE       =           DISPID_EVMETH_ONAFTERUPDATE;
        public const int DISPID_HTMLDOCUMENTEVENTS2_ONROWEXIT          =            DISPID_EVMETH_ONROWEXIT;
        public const int DISPID_HTMLDOCUMENTEVENTS2_ONROWENTER         =            DISPID_EVMETH_ONROWENTER;
        public const int DISPID_HTMLDOCUMENTEVENTS2_ONDRAGSTART        =            DISPID_EVMETH_ONDRAGSTART;
        public const int DISPID_HTMLDOCUMENTEVENTS2_ONSELECTSTART       =           DISPID_EVMETH_ONSELECTSTART;
        public const int DISPID_HTMLDOCUMENTEVENTS2_ONERRORUPDATE       =           DISPID_EVMETH_ONERRORUPDATE;
        public const int DISPID_HTMLDOCUMENTEVENTS2_ONCONTEXTMENU       =           DISPID_EVMETH_ONCONTEXTMENU;
        public const int DISPID_HTMLDOCUMENTEVENTS2_ONSTOP              =           DISPID_EVMETH_ONSTOP;
        public const int DISPID_HTMLDOCUMENTEVENTS2_ONROWSDELETE       =            DISPID_EVMETH_ONROWSDELETE;
        public const int DISPID_HTMLDOCUMENTEVENTS2_ONROWSINSERTED      =           DISPID_EVMETH_ONROWSINSERTED;
        public const int DISPID_HTMLDOCUMENTEVENTS2_ONCELLCHANGE        =           DISPID_EVMETH_ONCELLCHANGE;
        public const int DISPID_HTMLDOCUMENTEVENTS2_ONPROPERTYCHANGE      =         DISPID_EVMETH_ONPROPERTYCHANGE;
        public const int DISPID_HTMLDOCUMENTEVENTS2_ONDATASETCHANGED      =         DISPID_EVMETH_ONDATASETCHANGED;
        public const int DISPID_HTMLDOCUMENTEVENTS2_ONDATAAVAILABLE       =         DISPID_EVMETH_ONDATAAVAILABLE;
        public const int DISPID_HTMLDOCUMENTEVENTS2_ONDATASETCOMPLETE     =         DISPID_EVMETH_ONDATASETCOMPLETE;
        public const int DISPID_HTMLDOCUMENTEVENTS2_ONBEFOREEDITFOCUS     =         DISPID_EVMETH_ONBEFOREEDITFOCUS;
        public const int DISPID_HTMLDOCUMENTEVENTS2_ONSELECTIONCHANGE     =         DISPID_EVMETH_ONSELECTIONCHANGE;
        public const int DISPID_HTMLDOCUMENTEVENTS2_ONCONTROLSELECT      =          DISPID_EVMETH_ONCONTROLSELECT;
        public const int DISPID_HTMLDOCUMENTEVENTS2_ONMOUSEWHEEL         =          DISPID_EVMETH_ONMOUSEWHEEL;
        public const int DISPID_HTMLDOCUMENTEVENTS2_ONFOCUSIN            =          DISPID_EVMETH_ONFOCUSIN;
        public const int DISPID_HTMLDOCUMENTEVENTS2_ONFOCUSOUT           =          DISPID_EVMETH_ONFOCUSOUT;
        public const int DISPID_HTMLDOCUMENTEVENTS2_ONACTIVATE           =          DISPID_EVMETH_ONACTIVATE;
        public const int DISPID_HTMLDOCUMENTEVENTS2_ONDEACTIVATE         =          DISPID_EVMETH_ONDEACTIVATE;
        public const int DISPID_HTMLDOCUMENTEVENTS2_ONBEFOREACTIVATE     =          DISPID_EVMETH_ONBEFOREACTIVATE;
        public const int DISPID_HTMLDOCUMENTEVENTS2_ONBEFOREDEACTIVATE    =         DISPID_EVMETH_ONBEFOREDEACTIVATE;
        #endregion

        #region DISPIDs for interface IHTMLDocument3
        public const int DISPID_IHTMLDOCUMENT3_RELEASECAPTURE     =                 DISPID_OMDOCUMENT+72;
        public const int DISPID_IHTMLDOCUMENT3_RECALC             =                 DISPID_OMDOCUMENT+73;
        public const int DISPID_IHTMLDOCUMENT3_CREATETEXTNODE      =                DISPID_OMDOCUMENT+74;
        public const int DISPID_IHTMLDOCUMENT3_DOCUMENTELEMENT     =                DISPID_OMDOCUMENT+75;
        public const int DISPID_IHTMLDOCUMENT3_UNIQUEID            =                DISPID_OMDOCUMENT+77;
        public const int DISPID_IHTMLDOCUMENT3_ATTACHEVENT          =               DISPID_HTMLOBJECT+7;
        public const int DISPID_IHTMLDOCUMENT3_DETACHEVENT           =              DISPID_HTMLOBJECT+8;
        public const int DISPID_IHTMLDOCUMENT3_ONROWSDELETE          =              DISPID_EVPROP_ONROWSDELETE;
        public const int DISPID_IHTMLDOCUMENT3_ONROWSINSERTED         =             DISPID_EVPROP_ONROWSINSERTED;
        public const int DISPID_IHTMLDOCUMENT3_ONCELLCHANGE           =             DISPID_EVPROP_ONCELLCHANGE;
        public const int DISPID_IHTMLDOCUMENT3_ONDATASETCHANGED        =            DISPID_EVPROP_ONDATASETCHANGED;
        public const int DISPID_IHTMLDOCUMENT3_ONDATAAVAILABLE         =            DISPID_EVPROP_ONDATAAVAILABLE;
        public const int DISPID_IHTMLDOCUMENT3_ONDATASETCOMPLETE       =            DISPID_EVPROP_ONDATASETCOMPLETE;
        public const int DISPID_IHTMLDOCUMENT3_ONPROPERTYCHANGE        =            DISPID_EVPROP_ONPROPERTYCHANGE;
        public const int DISPID_IHTMLDOCUMENT3_DIR                     =            DISPID_A_DIR;
        public const int DISPID_IHTMLDOCUMENT3_ONCONTEXTMENU           =            DISPID_EVPROP_ONCONTEXTMENU;
        public const int DISPID_IHTMLDOCUMENT3_ONSTOP                  =            DISPID_EVPROP_ONSTOP;
        public const int DISPID_IHTMLDOCUMENT3_CREATEDOCUMENTFRAGMENT  =            DISPID_OMDOCUMENT+76;
        public const int DISPID_IHTMLDOCUMENT3_PARENTDOCUMENT          =            DISPID_OMDOCUMENT+78;
        public const int DISPID_IHTMLDOCUMENT3_ENABLEDOWNLOAD          =            DISPID_OMDOCUMENT+79;
        public const int DISPID_IHTMLDOCUMENT3_BASEURL                 =            DISPID_OMDOCUMENT+80;
        public const int DISPID_IHTMLDOCUMENT3_CHILDNODES              =            DISPID_ELEMENT+49;
        public const int DISPID_IHTMLDOCUMENT3_INHERITSTYLESHEETS      =            DISPID_OMDOCUMENT+82;
        public const int DISPID_IHTMLDOCUMENT3_ONBEFOREEDITFOCUS       =            DISPID_EVPROP_ONBEFOREEDITFOCUS;
        public const int DISPID_IHTMLDOCUMENT3_GETELEMENTSBYNAME       =            DISPID_OMDOCUMENT+86;
        public const int DISPID_IHTMLDOCUMENT3_GETELEMENTBYID          =            DISPID_OMDOCUMENT+88;
        public const int DISPID_IHTMLDOCUMENT3_GETELEMENTSBYTAGNAME     =           DISPID_OMDOCUMENT+87;
        #endregion

        #region DISPIDs for interface IHTMLDocument4
        public const int  DISPID_IHTMLDOCUMENT4_FOCUS       =                        DISPID_OMDOCUMENT+89;
        public const int  DISPID_IHTMLDOCUMENT4_HASFOCUS        =                    DISPID_OMDOCUMENT+90;
        public const int  DISPID_IHTMLDOCUMENT4_ONSELECTIONCHANGE  =                 DISPID_EVPROP_ONSELECTIONCHANGE;
        public const int  DISPID_IHTMLDOCUMENT4_NAMESPACES         =                 DISPID_OMDOCUMENT+91;
        public const int  DISPID_IHTMLDOCUMENT4_CREATEDOCUMENTFROMURL    =           DISPID_OMDOCUMENT+92;
        public const int DISPID_IHTMLDOCUMENT4_MEDIA     =                          DISPID_OMDOCUMENT+93;
        public const int DISPID_IHTMLDOCUMENT4_CREATEEVENTOBJECT    =               DISPID_OMDOCUMENT+94;
        public const int  DISPID_IHTMLDOCUMENT4_FIREEVENT    =                       DISPID_OMDOCUMENT+95;
        public const int  DISPID_IHTMLDOCUMENT4_CREATERENDERSTYLE   =                DISPID_OMDOCUMENT+96;
        public const int DISPID_IHTMLDOCUMENT4_ONCONTROLSELECT    =                 DISPID_EVPROP_ONCONTROLSELECT;
        public const int DISPID_IHTMLDOCUMENT4_URLUNENCODED        =                DISPID_OMDOCUMENT+97;
        #endregion

        #region DISPIDs for interface IHTMLDocument5
        public const int  DISPID_IHTMLDOCUMENT5_ONMOUSEWHEEL      =                  DISPID_EVPROP_ONMOUSEWHEEL;
        public const int  DISPID_IHTMLDOCUMENT5_DOCTYPE  =                           DISPID_OMDOCUMENT+98;
        public const int DISPID_IHTMLDOCUMENT5_IMPLEMENTATION  =                    DISPID_OMDOCUMENT+99;
        public const int  DISPID_IHTMLDOCUMENT5_CREATEATTRIBUTE     =                DISPID_OMDOCUMENT+100;
        public const int  DISPID_IHTMLDOCUMENT5_CREATECOMMENT   =                    DISPID_OMDOCUMENT+101;
        public const int  DISPID_IHTMLDOCUMENT5_ONFOCUSIN      =                     DISPID_EVPROP_ONFOCUSIN;
        public const int  DISPID_IHTMLDOCUMENT5_ONFOCUSOUT       =                   DISPID_EVPROP_ONFOCUSOUT;
        public const int  DISPID_IHTMLDOCUMENT5_ONACTIVATE     =                     DISPID_EVPROP_ONACTIVATE;
        public const int DISPID_IHTMLDOCUMENT5_ONDEACTIVATE     =                   DISPID_EVPROP_ONDEACTIVATE;
        public const int DISPID_IHTMLDOCUMENT5_ONBEFOREACTIVATE    =                DISPID_EVPROP_ONBEFOREACTIVATE;
        public const int  DISPID_IHTMLDOCUMENT5_ONBEFOREDEACTIVATE  =                DISPID_EVPROP_ONBEFOREDEACTIVATE;
        public const int  DISPID_IHTMLDOCUMENT5_COMPATMODE      =                    DISPID_OMDOCUMENT+102;
        #endregion

        #region DISPIDs for interface IHTMLDOMChildrenCollection
        public const int  DISPID_IHTMLDOMCHILDRENCOLLECTION_LENGTH           =       DISPID_COLLECTION;
        public const int  DISPID_IHTMLDOMCHILDRENCOLLECTION__NEWENUM         =       DISPID_NEWENUM;
        public const int DISPID_IHTMLDOMCHILDRENCOLLECTION_ITEM             =       DISPID_VALUE;
        #endregion

        #region DISPIDs for interface IHTMLDOMNode
        public const int  DISPID_IHTMLDOMNODE_NODETYPE    =                         DISPID_ELEMENT+46;
        public const int  DISPID_IHTMLDOMNODE_PARENTNODE       =                    DISPID_ELEMENT+47;
        public const int  DISPID_IHTMLDOMNODE_HASCHILDNODES   =                     DISPID_ELEMENT+48;
        public const int  DISPID_IHTMLDOMNODE_CHILDNODES    =                       DISPID_ELEMENT+49;
        public const int  DISPID_IHTMLDOMNODE_ATTRIBUTES  =                         DISPID_ELEMENT+50;
        public const int  DISPID_IHTMLDOMNODE_INSERTBEFORE  =                       DISPID_ELEMENT+51;
        public const int  DISPID_IHTMLDOMNODE_REMOVECHILD =                         DISPID_ELEMENT+52;
        public const int DISPID_IHTMLDOMNODE_REPLACECHILD  =                        DISPID_ELEMENT+53;
        public const int  DISPID_IHTMLDOMNODE_CLONENODE =                           DISPID_ELEMENT+61;
        public const int  DISPID_IHTMLDOMNODE_REMOVENODE  =                         DISPID_ELEMENT+66;
        public const int  DISPID_IHTMLDOMNODE_SWAPNODE  =                           DISPID_ELEMENT+68;
        public const int  DISPID_IHTMLDOMNODE_REPLACENODE   =                       DISPID_ELEMENT+67;
        public const int  DISPID_IHTMLDOMNODE_APPENDCHILD  =                        DISPID_ELEMENT+73;
        public const int  DISPID_IHTMLDOMNODE_NODENAME     =                        DISPID_ELEMENT+74;
        public const int  DISPID_IHTMLDOMNODE_NODEVALUE    =                        DISPID_ELEMENT+75;
        public const int  DISPID_IHTMLDOMNODE_FIRSTCHILD   =                        DISPID_ELEMENT+76;
        public const int  DISPID_IHTMLDOMNODE_LASTCHILD    =                        DISPID_ELEMENT+77;
        public const int  DISPID_IHTMLDOMNODE_PREVIOUSSIBLING  =                    DISPID_ELEMENT+78;
        public const int DISPID_IHTMLDOMNODE_NEXTSIBLING       =					DISPID_ELEMENT+79;
        #endregion

        #region DISPIDs for interface IHTMLElement
        public const int DISPID_IHTMLELEMENT_SETATTRIBUTE         =                 DISPID_HTMLOBJECT+1;
        public const int DISPID_IHTMLELEMENT_GETATTRIBUTE         =                 DISPID_HTMLOBJECT+2;
        public const int DISPID_IHTMLELEMENT_REMOVEATTRIBUTE       =                DISPID_HTMLOBJECT+3;
        public const int DISPID_IHTMLELEMENT_CLASSNAME            =                 DISPID_ELEMENT+1;
        public const int DISPID_IHTMLELEMENT_ID                   =                 DISPID_ELEMENT+2;
        public const int DISPID_IHTMLELEMENT_TAGNAME             =                  DISPID_ELEMENT+4;
        public const int DISPID_IHTMLELEMENT_PARENTELEMENT       =                  STDPROPID_XOBJ_PARENT;
        public const int DISPID_IHTMLELEMENT_STYLE               =                  STDPROPID_XOBJ_STYLE;
        public const int DISPID_IHTMLELEMENT_ONHELP              =                  DISPID_EVPROP_ONHELP;
        public const int DISPID_IHTMLELEMENT_ONCLICK             =                  DISPID_EVPROP_ONCLICK;
        public const int DISPID_IHTMLELEMENT_ONDBLCLICK          =                  DISPID_EVPROP_ONDBLCLICK;
        public const int DISPID_IHTMLELEMENT_ONKEYDOWN           =                  DISPID_EVPROP_ONKEYDOWN;
        public const int DISPID_IHTMLELEMENT_ONKEYUP             =                  DISPID_EVPROP_ONKEYUP;
        public const int DISPID_IHTMLELEMENT_ONKEYPRESS          =                  DISPID_EVPROP_ONKEYPRESS;
        public const int DISPID_IHTMLELEMENT_ONMOUSEOUT           =                 DISPID_EVPROP_ONMOUSEOUT;
        public const int DISPID_IHTMLELEMENT_ONMOUSEOVER          =                 DISPID_EVPROP_ONMOUSEOVER;
        public const int DISPID_IHTMLELEMENT_ONMOUSEMOVE          =                 DISPID_EVPROP_ONMOUSEMOVE;
        public const int DISPID_IHTMLELEMENT_ONMOUSEDOWN          =                 DISPID_EVPROP_ONMOUSEDOWN;
        public const int DISPID_IHTMLELEMENT_ONMOUSEUP            =                 DISPID_EVPROP_ONMOUSEUP;
        public const int DISPID_IHTMLELEMENT_DOCUMENT             =                 DISPID_ELEMENT+18;
        public const int DISPID_IHTMLELEMENT_TITLE                =                 STDPROPID_XOBJ_CONTROLTIPTEXT;
        public const int DISPID_IHTMLELEMENT_LANGUAGE             =                 DISPID_A_LANGUAGE;
        public const int DISPID_IHTMLELEMENT_ONSELECTSTART        =                 DISPID_EVPROP_ONSELECTSTART;
        public const int DISPID_IHTMLELEMENT_SCROLLINTOVIEW        =                DISPID_ELEMENT+19;
        public const int DISPID_IHTMLELEMENT_CONTAINS              =                DISPID_ELEMENT+20;
        public const int DISPID_IHTMLELEMENT_SOURCEINDEX           =                DISPID_ELEMENT+24;
        public const int DISPID_IHTMLELEMENT_RECORDNUMBER          =                DISPID_ELEMENT+25;
        public const int DISPID_IHTMLELEMENT_LANG                  =                DISPID_A_LANG;
        public const int DISPID_IHTMLELEMENT_OFFSETLEFT            =                DISPID_ELEMENT+8;
        public const int DISPID_IHTMLELEMENT_OFFSETTOP             =                DISPID_ELEMENT+9;
        public const int DISPID_IHTMLELEMENT_OFFSETWIDTH           =                DISPID_ELEMENT+10;
        public const int DISPID_IHTMLELEMENT_OFFSETHEIGHT          =                DISPID_ELEMENT+11;
        public const int DISPID_IHTMLELEMENT_OFFSETPARENT          =                DISPID_ELEMENT+12;
        public const int DISPID_IHTMLELEMENT_INNERHTML             =                DISPID_ELEMENT+26;
        public const int DISPID_IHTMLELEMENT_INNERTEXT              =               DISPID_ELEMENT+27;
        public const int DISPID_IHTMLELEMENT_OUTERHTML             =                DISPID_ELEMENT+28;
        public const int DISPID_IHTMLELEMENT_OUTERTEXT              =               DISPID_ELEMENT+29;
        public const int DISPID_IHTMLELEMENT_INSERTADJACENTHTML     =               DISPID_ELEMENT+30;
        public const int DISPID_IHTMLELEMENT_INSERTADJACENTTEXT      =              DISPID_ELEMENT+31;
        public const int DISPID_IHTMLELEMENT_PARENTTEXTEDIT         =               DISPID_ELEMENT+32;
        public const int DISPID_IHTMLELEMENT_ISTEXTEDIT             =               DISPID_ELEMENT+34;
        public const int DISPID_IHTMLELEMENT_CLICK                  =               DISPID_ELEMENT+33;
        public const int DISPID_IHTMLELEMENT_FILTERS                =               DISPID_ELEMENT+35;
        public const int DISPID_IHTMLELEMENT_ONDRAGSTART            =               DISPID_EVPROP_ONDRAGSTART;
        public const int DISPID_IHTMLELEMENT_TOSTRING               =               DISPID_ELEMENT+36;
        public const int DISPID_IHTMLELEMENT_ONBEFOREUPDATE         =               DISPID_EVPROP_ONBEFOREUPDATE;
        public const int DISPID_IHTMLELEMENT_ONAFTERUPDATE          =               DISPID_EVPROP_ONAFTERUPDATE;
        public const int DISPID_IHTMLELEMENT_ONERRORUPDATE          =               DISPID_EVPROP_ONERRORUPDATE;
        public const int DISPID_IHTMLELEMENT_ONROWEXIT              =               DISPID_EVPROP_ONROWEXIT;
        public const int DISPID_IHTMLELEMENT_ONROWENTER             =               DISPID_EVPROP_ONROWENTER;
        public const int DISPID_IHTMLELEMENT_ONDATASETCHANGED       =               DISPID_EVPROP_ONDATASETCHANGED;
        public const int DISPID_IHTMLELEMENT_ONDATAAVAILABLE        =               DISPID_EVPROP_ONDATAAVAILABLE;
        public const int DISPID_IHTMLELEMENT_ONDATASETCOMPLETE       =              DISPID_EVPROP_ONDATASETCOMPLETE;
        public const int DISPID_IHTMLELEMENT_ONFILTERCHANGE          =              DISPID_EVPROP_ONFILTER;
        public const int DISPID_IHTMLELEMENT_CHILDREN               =               DISPID_ELEMENT+37;
        public const int DISPID_IHTMLELEMENT_ALL                    =               DISPID_ELEMENT+38;
        #endregion

        #region DISPIDs for interface IHTMLElement2
        public const int  DISPID_IHTMLELEMENT2_SCOPENAME  =  DISPID_ELEMENT+39;
        public const int  DISPID_IHTMLELEMENT2_SETCAPTURE =  DISPID_ELEMENT+40;
        public const int  DISPID_IHTMLELEMENT2_RELEASECAPTURE =  DISPID_ELEMENT+41;
        public const int  DISPID_IHTMLELEMENT2_ONLOSECAPTURE  =  DISPID_EVPROP_ONLOSECAPTURE;
        public const int  DISPID_IHTMLELEMENT2_COMPONENTFROMPOINT =  DISPID_ELEMENT+42;
        public const int  DISPID_IHTMLELEMENT2_DOSCROLL =  DISPID_ELEMENT+43;
        public const int  DISPID_IHTMLELEMENT2_ONSCROLL =  DISPID_EVPROP_ONSCROLL;
        public const int  DISPID_IHTMLELEMENT2_ONDRAG =  DISPID_EVPROP_ONDRAG;
        public const int  DISPID_IHTMLELEMENT2_ONDRAGEND  =  DISPID_EVPROP_ONDRAGEND;
        public const int  DISPID_IHTMLELEMENT2_ONDRAGENTER  =  DISPID_EVPROP_ONDRAGENTER;
        public const int  DISPID_IHTMLELEMENT2_ONDRAGOVER =  DISPID_EVPROP_ONDRAGOVER;
        public const int  DISPID_IHTMLELEMENT2_ONDRAGLEAVE  =  DISPID_EVPROP_ONDRAGLEAVE;
        public const int  DISPID_IHTMLELEMENT2_ONDROP =  DISPID_EVPROP_ONDROP;
        public const int  DISPID_IHTMLELEMENT2_ONBEFORECUT  =  DISPID_EVPROP_ONBEFORECUT;
        public const int  DISPID_IHTMLELEMENT2_ONCUT  =  DISPID_EVPROP_ONCUT;
        public const int  DISPID_IHTMLELEMENT2_ONBEFORECOPY =  DISPID_EVPROP_ONBEFORECOPY;
        public const int  DISPID_IHTMLELEMENT2_ONCOPY =  DISPID_EVPROP_ONCOPY;
        public const int  DISPID_IHTMLELEMENT2_ONBEFOREPASTE  =  DISPID_EVPROP_ONBEFOREPASTE;
        public const int  DISPID_IHTMLELEMENT2_ONPASTE  =  DISPID_EVPROP_ONPASTE;
        public const int  DISPID_IHTMLELEMENT2_CURRENTSTYLE =  DISPID_ELEMENT+7;
        public const int  DISPID_IHTMLELEMENT2_ONPROPERTYCHANGE =  DISPID_EVPROP_ONPROPERTYCHANGE;
        public const int  DISPID_IHTMLELEMENT2_GETCLIENTRECTS =  DISPID_ELEMENT+44;
        public const int  DISPID_IHTMLELEMENT2_GETBOUNDINGCLIENTRECT = DISPID_ELEMENT+45;
        public const int  DISPID_IHTMLELEMENT2_SETEXPRESSION = DISPID_HTMLOBJECT+4;
        public const int  DISPID_IHTMLELEMENT2_GETEXPRESSION = DISPID_HTMLOBJECT+5;
        public const int  DISPID_IHTMLELEMENT2_REMOVEEXPRESSION  = DISPID_HTMLOBJECT+6;
        public const int  DISPID_IHTMLELEMENT2_FOCUS = DISPID_SITE+0;
        public const int  DISPID_IHTMLELEMENT2_ACCESSKEY = DISPID_SITE+5;
        public const int  DISPID_IHTMLELEMENT2_ONBLUR  = DISPID_EVPROP_ONBLUR;
        public const int  DISPID_IHTMLELEMENT2_ONFOCUS = DISPID_EVPROP_ONFOCUS;
        public const int  DISPID_IHTMLELEMENT2_ONRESIZE  = DISPID_EVPROP_ONRESIZE;
        public const int  DISPID_IHTMLELEMENT2_BLUR  = DISPID_SITE+2;
        public const int  DISPID_IHTMLELEMENT2_ADDFILTER = DISPID_SITE+17;
        public const int  DISPID_IHTMLELEMENT2_REMOVEFILTER  = DISPID_SITE+18;
        public const int  DISPID_IHTMLELEMENT2_CLIENTHEIGHT  = DISPID_SITE+19;
        public const int  DISPID_IHTMLELEMENT2_CLIENTWIDTH = DISPID_SITE+20;
        public const int  DISPID_IHTMLELEMENT2_CLIENTTOP = DISPID_SITE+21;
        public const int  DISPID_IHTMLELEMENT2_CLIENTLEFT  = DISPID_SITE+22;
        public const int  DISPID_IHTMLELEMENT2_ATTACHEVENT = DISPID_HTMLOBJECT+7;
        public const int  DISPID_IHTMLELEMENT2_DETACHEVENT = DISPID_HTMLOBJECT+8;
        public const int  DISPID_IHTMLELEMENT2_ONREADYSTATECHANGE  = DISPID_EVPROP_ONREADYSTATECHANGE;
        public const int  DISPID_IHTMLELEMENT2_ONROWSDELETE  = DISPID_EVPROP_ONROWSDELETE;
        public const int  DISPID_IHTMLELEMENT2_ONROWSINSERTED  = DISPID_EVPROP_ONROWSINSERTED;
        public const int  DISPID_IHTMLELEMENT2_ONCELLCHANGE  = DISPID_EVPROP_ONCELLCHANGE;
        public const int  DISPID_IHTMLELEMENT2_CREATECONTROLRANGE  = DISPID_ELEMENT+56;
        public const int  DISPID_IHTMLELEMENT2_SCROLLHEIGHT  = DISPID_ELEMENT+57;
        public const int  DISPID_IHTMLELEMENT2_SCROLLWIDTH = DISPID_ELEMENT+58;
        public const int  DISPID_IHTMLELEMENT2_SCROLLTOP = DISPID_ELEMENT+59;
        public const int  DISPID_IHTMLELEMENT2_SCROLLLEFT  = DISPID_ELEMENT+60;
        public const int  DISPID_IHTMLELEMENT2_CLEARATTRIBUTES = DISPID_ELEMENT+62;
        public const int  DISPID_IHTMLELEMENT2_MERGEATTRIBUTES = DISPID_ELEMENT+63;
        public const int  DISPID_IHTMLELEMENT2_ONCONTEXTMENU = DISPID_EVPROP_ONCONTEXTMENU;
        public const int  DISPID_IHTMLELEMENT2_INSERTADJACENTELEMENT = DISPID_ELEMENT+69;
        public const int  DISPID_IHTMLELEMENT2_APPLYELEMENT  = DISPID_ELEMENT+65;
        public const int  DISPID_IHTMLELEMENT2_GETADJACENTTEXT = DISPID_ELEMENT+70;
        public const int  DISPID_IHTMLELEMENT2_REPLACEADJACENTTEXT = DISPID_ELEMENT+71;
        public const int  DISPID_IHTMLELEMENT2_CANHAVECHILDREN = DISPID_ELEMENT+72;
        public const int  DISPID_IHTMLELEMENT2_ADDBEHAVIOR = DISPID_ELEMENT+80;
        public const int  DISPID_IHTMLELEMENT2_REMOVEBEHAVIOR  = DISPID_ELEMENT+81;
        public const int  DISPID_IHTMLELEMENT2_RUNTIMESTYLE  = DISPID_ELEMENT+64;
        public const int  DISPID_IHTMLELEMENT2_BEHAVIORURNS  = DISPID_ELEMENT+82;
        public const int  DISPID_IHTMLELEMENT2_TAGURN  = DISPID_ELEMENT+83;
        public const int  DISPID_IHTMLELEMENT2_ONBEFOREEDITFOCUS = DISPID_EVPROP_ONBEFOREEDITFOCUS;
        public const int  DISPID_IHTMLELEMENT2_READYSTATEVALUE = DISPID_ELEMENT+84;
        public const int  DISPID_IHTMLELEMENT2_GETELEMENTSBYTAGNAME  = DISPID_ELEMENT+85;
        public const int  DISPID_IHTMLELEMENT2_DIR =								DISPID_A_DIR;
        public const int  DISPID_IHTMLELEMENT2_TABINDEX  =							STDPROPID_XOBJ_TABINDEX;
        #endregion

        #region DISPIDs for interface IHTMLElement3
        public const int DISPID_IHTMLELEMENT3_MERGEATTRIBUTES     =                 DISPID_ELEMENT+96;
        public const int DISPID_IHTMLELEMENT3_ISMULTILINE          =                DISPID_ELEMENT+97;
        public const int DISPID_IHTMLELEMENT3_CANHAVEHTML          =                DISPID_ELEMENT+98;
        public const int DISPID_IHTMLELEMENT3_ONLAYOUTCOMPLETE     =                DISPID_EVPROP_ONLAYOUTCOMPLETE;
        public const int DISPID_IHTMLELEMENT3_ONPAGE              =                 DISPID_EVPROP_ONPAGE;
        public const int DISPID_IHTMLELEMENT3_INFLATEBLOCK         =                DISPID_ELEMENT+100;
        public const int DISPID_IHTMLELEMENT3_ONBEFOREDEACTIVATE     =              DISPID_EVPROP_ONBEFOREDEACTIVATE;
        public const int DISPID_IHTMLELEMENT3_SETACTIVE              =              DISPID_ELEMENT+101;
        public const int DISPID_IHTMLELEMENT3_CONTENTEDITABLE        =              DISPID_A_EDITABLE;
        public const int DISPID_IHTMLELEMENT3_ISCONTENTEDITABLE       =             DISPID_ELEMENT+102;
        public const int DISPID_IHTMLELEMENT3_HIDEFOCUS              =              DISPID_A_HIDEFOCUS;
        public const int DISPID_IHTMLELEMENT3_DISABLED              =               STDPROPID_XOBJ_DISABLED;
        public const int DISPID_IHTMLELEMENT3_ISDISABLED            =               DISPID_ELEMENT+105;
        public const int DISPID_IHTMLELEMENT3_ONMOVE               =                DISPID_EVPROP_ONMOVE;
        public const int DISPID_IHTMLELEMENT3_ONCONTROLSELECT       =               DISPID_EVPROP_ONCONTROLSELECT;
        public const int DISPID_IHTMLELEMENT3_FIREEVENT            =                DISPID_ELEMENT+106;
        public const int DISPID_IHTMLELEMENT3_ONRESIZESTART       =                 DISPID_EVPROP_ONRESIZESTART;
        public const int DISPID_IHTMLELEMENT3_ONRESIZEEND          =                DISPID_EVPROP_ONRESIZEEND;
        public const int DISPID_IHTMLELEMENT3_ONMOVESTART           =               DISPID_EVPROP_ONMOVESTART;
        public const int DISPID_IHTMLELEMENT3_ONMOVEEND            =                DISPID_EVPROP_ONMOVEEND;
        public const int DISPID_IHTMLELEMENT3_ONMOUSEENTER         =                DISPID_EVPROP_ONMOUSEENTER;
        public const int DISPID_IHTMLELEMENT3_ONMOUSELEAVE         =                DISPID_EVPROP_ONMOUSELEAVE;
        public const int DISPID_IHTMLELEMENT3_ONACTIVATE           =                DISPID_EVPROP_ONACTIVATE;
        public const int DISPID_IHTMLELEMENT3_ONDEACTIVATE         =                DISPID_EVPROP_ONDEACTIVATE;
        public const int DISPID_IHTMLELEMENT3_DRAGDROP             =                DISPID_ELEMENT+107;
        public const int DISPID_IHTMLELEMENT3_GLYPHMODE           =                 DISPID_ELEMENT+108;
        #endregion

        #region DISPIDs for event set HTMLElementEvents
        public const int DISPID_HTMLELEMENTEVENTS_ONHELP       =                    DISPID_EVMETH_ONHELP;
        public const int DISPID_HTMLELEMENTEVENTS_ONCLICK        =                  DISPID_EVMETH_ONCLICK;
        public const int DISPID_HTMLELEMENTEVENTS_ONDBLCLICK     =                  DISPID_EVMETH_ONDBLCLICK;
        public const int DISPID_HTMLELEMENTEVENTS_ONKEYPRESS     =                  DISPID_EVMETH_ONKEYPRESS;
        public const int DISPID_HTMLELEMENTEVENTS_ONKEYDOWN      =                  DISPID_EVMETH_ONKEYDOWN;
        public const int DISPID_HTMLELEMENTEVENTS_ONKEYUP         =                 DISPID_EVMETH_ONKEYUP;
        public const int DISPID_HTMLELEMENTEVENTS_ONMOUSEOUT      =                 DISPID_EVMETH_ONMOUSEOUT;
        public const int DISPID_HTMLELEMENTEVENTS_ONMOUSEOVER     =                 DISPID_EVMETH_ONMOUSEOVER;
        public const int DISPID_HTMLELEMENTEVENTS_ONMOUSEMOVE     =                 DISPID_EVMETH_ONMOUSEMOVE;
        public const int DISPID_HTMLELEMENTEVENTS_ONMOUSEDOWN     =                 DISPID_EVMETH_ONMOUSEDOWN;
        public const int DISPID_HTMLELEMENTEVENTS_ONMOUSEUP       =                 DISPID_EVMETH_ONMOUSEUP;
        public const int DISPID_HTMLELEMENTEVENTS_ONSELECTSTART    =                DISPID_EVMETH_ONSELECTSTART;
        public const int DISPID_HTMLELEMENTEVENTS_ONFILTERCHANGE    =               DISPID_EVMETH_ONFILTER;
        public const int DISPID_HTMLELEMENTEVENTS_ONDRAGSTART        =              DISPID_EVMETH_ONDRAGSTART;
        public const int DISPID_HTMLELEMENTEVENTS_ONBEFOREUPDATE     =              DISPID_EVMETH_ONBEFOREUPDATE;
        public const int DISPID_HTMLELEMENTEVENTS_ONAFTERUPDATE      =              DISPID_EVMETH_ONAFTERUPDATE;
        public const int DISPID_HTMLELEMENTEVENTS_ONERRORUPDATE      =              DISPID_EVMETH_ONERRORUPDATE;
        public const int DISPID_HTMLELEMENTEVENTS_ONROWEXIT          =              DISPID_EVMETH_ONROWEXIT;
        public const int DISPID_HTMLELEMENTEVENTS_ONROWENTER         =              DISPID_EVMETH_ONROWENTER;
        public const int DISPID_HTMLELEMENTEVENTS_ONDATASETCHANGED     =            DISPID_EVMETH_ONDATASETCHANGED;
        public const int DISPID_HTMLELEMENTEVENTS_ONDATAAVAILABLE      =            DISPID_EVMETH_ONDATAAVAILABLE;
        public const int DISPID_HTMLELEMENTEVENTS_ONDATASETCOMPLETE    =            DISPID_EVMETH_ONDATASETCOMPLETE;
        public const int DISPID_HTMLELEMENTEVENTS_ONLOSECAPTURE        =            DISPID_EVMETH_ONLOSECAPTURE;
        public const int DISPID_HTMLELEMENTEVENTS_ONPROPERTYCHANGE     =            DISPID_EVMETH_ONPROPERTYCHANGE;
        public const int DISPID_HTMLELEMENTEVENTS_ONSCROLL      =                   DISPID_EVMETH_ONSCROLL;
        public const int DISPID_HTMLELEMENTEVENTS_ONFOCUS         =                 DISPID_EVMETH_ONFOCUS;
        public const int DISPID_HTMLELEMENTEVENTS_ONBLUR            =               DISPID_EVMETH_ONBLUR;
        public const int DISPID_HTMLELEMENTEVENTS_ONRESIZE          =               DISPID_EVMETH_ONRESIZE;
        public const int DISPID_HTMLELEMENTEVENTS_ONDRAG            =               DISPID_EVMETH_ONDRAG;
        public const int DISPID_HTMLELEMENTEVENTS_ONDRAGEND          =              DISPID_EVMETH_ONDRAGEND;
        public const int DISPID_HTMLELEMENTEVENTS_ONDRAGENTER        =              DISPID_EVMETH_ONDRAGENTER;
        public const int DISPID_HTMLELEMENTEVENTS_ONDRAGOVER         =              DISPID_EVMETH_ONDRAGOVER;
        public const int DISPID_HTMLELEMENTEVENTS_ONDRAGLEAVE        =              DISPID_EVMETH_ONDRAGLEAVE;
        public const int DISPID_HTMLELEMENTEVENTS_ONDROP             =              DISPID_EVMETH_ONDROP;
        public const int DISPID_HTMLELEMENTEVENTS_ONBEFORECUT         =             DISPID_EVMETH_ONBEFORECUT;
        public const int DISPID_HTMLELEMENTEVENTS_ONCUT               =             DISPID_EVMETH_ONCUT;
        public const int DISPID_HTMLELEMENTEVENTS_ONBEFORECOPY        =             DISPID_EVMETH_ONBEFORECOPY;
        public const int DISPID_HTMLELEMENTEVENTS_ONCOPY              =             DISPID_EVMETH_ONCOPY;
        public const int DISPID_HTMLELEMENTEVENTS_ONBEFOREPASTE       =             DISPID_EVMETH_ONBEFOREPASTE;
        public const int DISPID_HTMLELEMENTEVENTS_ONPASTE             =             DISPID_EVMETH_ONPASTE;
        public const int DISPID_HTMLELEMENTEVENTS_ONCONTEXTMENU       =             DISPID_EVMETH_ONCONTEXTMENU;
        public const int DISPID_HTMLELEMENTEVENTS_ONROWSDELETE         =            DISPID_EVMETH_ONROWSDELETE;
        public const int DISPID_HTMLELEMENTEVENTS_ONROWSINSERTED        =           DISPID_EVMETH_ONROWSINSERTED;
        public const int DISPID_HTMLELEMENTEVENTS_ONCELLCHANGE          =           DISPID_EVMETH_ONCELLCHANGE;
        public const int DISPID_HTMLELEMENTEVENTS_ONREADYSTATECHANGE     =          DISPID_EVMETH_ONREADYSTATECHANGE;
        public const int DISPID_HTMLELEMENTEVENTS_ONBEFOREEDITFOCUS      =          DISPID_EVMETH_ONBEFOREEDITFOCUS;
        public const int DISPID_HTMLELEMENTEVENTS_ONLAYOUTCOMPLETE       =          DISPID_EVMETH_ONLAYOUTCOMPLETE;
        public const int DISPID_HTMLELEMENTEVENTS_ONPAGE                 =          DISPID_EVMETH_ONPAGE;
        public const int DISPID_HTMLELEMENTEVENTS_ONBEFOREDEACTIVATE     =          DISPID_EVMETH_ONBEFOREDEACTIVATE;
        public const int DISPID_HTMLELEMENTEVENTS_ONBEFOREACTIVATE       =          DISPID_EVMETH_ONBEFOREACTIVATE;
        public const int DISPID_HTMLELEMENTEVENTS_ONMOVE                =           DISPID_EVMETH_ONMOVE;
        public const int DISPID_HTMLELEMENTEVENTS_ONCONTROLSELECT       =           DISPID_EVMETH_ONCONTROLSELECT;
        public const int DISPID_HTMLELEMENTEVENTS_ONMOVESTART           =           DISPID_EVMETH_ONMOVESTART;
        public const int DISPID_HTMLELEMENTEVENTS_ONMOVEEND            =            DISPID_EVMETH_ONMOVEEND;
        public const int DISPID_HTMLELEMENTEVENTS_ONRESIZESTART        =            DISPID_EVMETH_ONRESIZESTART;
        public const int DISPID_HTMLELEMENTEVENTS_ONRESIZEEND          =            DISPID_EVMETH_ONRESIZEEND;
        public const int DISPID_HTMLELEMENTEVENTS_ONMOUSEENTER         =            DISPID_EVMETH_ONMOUSEENTER;
        public const int DISPID_HTMLELEMENTEVENTS_ONMOUSELEAVE         =            DISPID_EVMETH_ONMOUSELEAVE;
        public const int DISPID_HTMLELEMENTEVENTS_ONMOUSEWHEEL        =             DISPID_EVMETH_ONMOUSEWHEEL;
        public const int DISPID_HTMLELEMENTEVENTS_ONACTIVATE          =             DISPID_EVMETH_ONACTIVATE;
        public const int DISPID_HTMLELEMENTEVENTS_ONDEACTIVATE        =             DISPID_EVMETH_ONDEACTIVATE;
        public const int DISPID_HTMLELEMENTEVENTS_ONFOCUSIN            =            DISPID_EVMETH_ONFOCUSIN;
        public const int DISPID_HTMLELEMENTEVENTS_ONFOCUSOUT          =             DISPID_EVMETH_ONFOCUSOUT;
        #endregion

        #region DISPIDs for interface IHTMLElementCollection
        public const int DISPID_IHTMLELEMENTCOLLECTION_TOSTRING      =              DISPID_COLLECTION+1;
        public const int DISPID_IHTMLELEMENTCOLLECTION_LENGTH        =              DISPID_COLLECTION;
        public const int DISPID_IHTMLELEMENTCOLLECTION__NEWENUM      =              DISPID_NEWENUM;
        public const int DISPID_IHTMLELEMENTCOLLECTION_ITEM          =              DISPID_VALUE;
        public const int DISPID_IHTMLELEMENTCOLLECTION_TAGS          =              DISPID_COLLECTION+2;
        #endregion

        #region DISPIDs for interface IHTMLEventObj

        public const int DISPID_IHTMLEVENTOBJ_SRCELEMENT =                          DISPID_EVENTOBJ+1;
        public const int DISPID_IHTMLEVENTOBJ_ALTKEY   =                            DISPID_EVENTOBJ+2;
        public const int DISPID_IHTMLEVENTOBJ_CTRLKEY    =                          DISPID_EVENTOBJ+3;
        public const int DISPID_IHTMLEVENTOBJ_SHIFTKEY     =                        DISPID_EVENTOBJ+4;
        public const int DISPID_IHTMLEVENTOBJ_RETURNVALUE    =                      DISPID_EVENTOBJ+7;
        public const int DISPID_IHTMLEVENTOBJ_CANCELBUBBLE     =                    DISPID_EVENTOBJ+8;
        public const int DISPID_IHTMLEVENTOBJ_FROMELEMENT    =                      DISPID_EVENTOBJ+9;
        public const int DISPID_IHTMLEVENTOBJ_TOELEMENT    =                        DISPID_EVENTOBJ+10;
        public const int DISPID_IHTMLEVENTOBJ_KEYCODE     =                         DISPID_EVENTOBJ+11;
        public const int DISPID_IHTMLEVENTOBJ_BUTTON       =                        DISPID_EVENTOBJ+12;
        public const int DISPID_IHTMLEVENTOBJ_TYPE         =                        DISPID_EVENTOBJ+13;
        public const int DISPID_IHTMLEVENTOBJ_QUALIFIER      =                      DISPID_EVENTOBJ+14;
        public const int DISPID_IHTMLEVENTOBJ_REASON     =                          DISPID_EVENTOBJ+15;
        public const int DISPID_IHTMLEVENTOBJ_X           =                         DISPID_EVENTOBJ+5;
        public const int DISPID_IHTMLEVENTOBJ_Y            =                        DISPID_EVENTOBJ+6;
        public const int DISPID_IHTMLEVENTOBJ_CLIENTX        =                      DISPID_EVENTOBJ+20;
        public const int DISPID_IHTMLEVENTOBJ_CLIENTY      =                        DISPID_EVENTOBJ+21;
        public const int DISPID_IHTMLEVENTOBJ_OFFSETX        =                      DISPID_EVENTOBJ+22;
        public const int DISPID_IHTMLEVENTOBJ_OFFSETY        =                      DISPID_EVENTOBJ+23;
        public const int DISPID_IHTMLEVENTOBJ_SCREENX       =                       DISPID_EVENTOBJ+24;
        public const int DISPID_IHTMLEVENTOBJ_SCREENY        =                      DISPID_EVENTOBJ+25;
        public const int DISPID_IHTMLEVENTOBJ_SRCFILTER       =                     DISPID_EVENTOBJ+26;
        #endregion

        #region DISPIDs for interface IHTMLEventObj4
        public const int DISPID_IHTMLEVENTOBJ4_WHEELDELTA    =                   DISPID_EVENTOBJ+51;
        #endregion

        #region DISPIDs for interface IHTMLTable
        public const int DISPID_A_FIRST =					                        DISPID_ATTRS;
        public const int DISPID_A_READYSTATE   =									(DISPID_A_FIRST+116);// ready state
        public const int DISPID_A_BACKGROUNDIMAGE =					            (DISPID_A_FIRST+1);
        public const int DISPID_A_TABLEBORDERCOLORLIGHT  =					        (DISPID_A_FIRST+29);
        public const int DISPID_A_TABLEBORDERCOLORDARK  =					        (DISPID_A_FIRST+30);
        public const int DISPID_A_TABLEBORDERCOLOR  =					            (DISPID_A_FIRST+28);
        public const int DISPID_TABLE  =					                        DISPID_NORMAL_FIRST;
        public const int DISPID_IHTMLTABLE_COLS      =                              DISPID_TABLE+1;
        public const int DISPID_IHTMLTABLE_BORDER     =                             DISPID_TABLE+2;
        public const int DISPID_IHTMLTABLE_FRAME       =                            DISPID_TABLE+4;
        public const int DISPID_IHTMLTABLE_RULES        =                           DISPID_TABLE+3;
        public const int DISPID_IHTMLTABLE_CELLSPACING   =                          DISPID_TABLE+5;
        public const int DISPID_IHTMLTABLE_CELLPADDING    =                         DISPID_TABLE+6;
        public const int DISPID_IHTMLTABLE_BACKGROUND      =                       DISPID_A_BACKGROUNDIMAGE;
        public const int DISPID_IHTMLTABLE_BGCOLOR          =                       DISPID_BACKCOLOR;
        public const int DISPID_IHTMLTABLE_BORDERCOLOR       =                     DISPID_A_TABLEBORDERCOLOR;
        public const int DISPID_IHTMLTABLE_BORDERCOLORLIGHT   =                    DISPID_A_TABLEBORDERCOLORLIGHT;
        public const int DISPID_IHTMLTABLE_BORDERCOLORDARK     =                   DISPID_A_TABLEBORDERCOLORDARK;
        public const int DISPID_IHTMLTABLE_ALIGN                =                  STDPROPID_XOBJ_CONTROLALIGN;
        public const int DISPID_IHTMLTABLE_REFRESH               =                  DISPID_TABLE+15;
        public const int DISPID_IHTMLTABLE_ROWS                   =                 DISPID_TABLE+16;
        public const int DISPID_IHTMLTABLE_WIDTH                   =                STDPROPID_XOBJ_WIDTH;
        public const int DISPID_IHTMLTABLE_HEIGHT                   =               STDPROPID_XOBJ_HEIGHT;
        public const int DISPID_IHTMLTABLE_DATAPAGESIZE              =              DISPID_TABLE+17;
        public const int DISPID_IHTMLTABLE_NEXTPAGE                   =             DISPID_TABLE+18;
        public const int DISPID_IHTMLTABLE_PREVIOUSPAGE   =                         DISPID_TABLE+19;
        public const int DISPID_IHTMLTABLE_THEAD           =                        DISPID_TABLE+20;
        public const int DISPID_IHTMLTABLE_TFOOT            =                       DISPID_TABLE+21;
        public const int DISPID_IHTMLTABLE_TBODIES           =                      DISPID_TABLE+24;
        public const int DISPID_IHTMLTABLE_CAPTION            =                     DISPID_TABLE+25;
        public const int DISPID_IHTMLTABLE_CREATETHEAD         =                    DISPID_TABLE+26;
        public const int DISPID_IHTMLTABLE_DELETETHEAD          =                   DISPID_TABLE+27;
        public const int DISPID_IHTMLTABLE_CREATETFOOT           =                  DISPID_TABLE+28;
        public const int DISPID_IHTMLTABLE_DELETETFOOT            =                 DISPID_TABLE+29;
        public const int DISPID_IHTMLTABLE_CREATECAPTION           =                DISPID_TABLE+30;
        public const int DISPID_IHTMLTABLE_DELETECAPTION            =               DISPID_TABLE+31;
        public const int DISPID_IHTMLTABLE_INSERTROW                 =              DISPID_TABLE+32;
        public const int DISPID_IHTMLTABLE_DELETEROW                  =             DISPID_TABLE+33;
        public const int DISPID_IHTMLTABLE_READYSTATE                  =            DISPID_A_READYSTATE;
        public const int DISPID_IHTMLTABLE_ONREADYSTATECHANGE           =           DISPID_EVPROP_ONREADYSTATECHANGE;
        #endregion

        #region DISPIDs for interface IHTMLTableRow
        public const int DISPID_A_TABLEVALIGN       =								(DISPID_A_FIRST+31);
        public const int DISPID_TABLEROW          =									DISPID_NORMAL_FIRST;
        public const int DISPID_IHTMLTABLEROW_ALIGN       =                         STDPROPID_XOBJ_BLOCKALIGN;
        public const int DISPID_IHTMLTABLEROW_VALIGN          =                     DISPID_A_TABLEVALIGN;
        public const int DISPID_IHTMLTABLEROW_BGCOLOR           =                   DISPID_BACKCOLOR;
        public const int DISPID_IHTMLTABLEROW_BORDERCOLOR         =                 DISPID_A_TABLEBORDERCOLOR;
        public const int DISPID_IHTMLTABLEROW_BORDERCOLORLIGHT      =               DISPID_A_TABLEBORDERCOLORLIGHT;
        public const int DISPID_IHTMLTABLEROW_BORDERCOLORDARK      =                DISPID_A_TABLEBORDERCOLORDARK;
        public const int DISPID_IHTMLTABLEROW_ROWINDEX             =                DISPID_TABLEROW;
        public const int DISPID_IHTMLTABLEROW_SECTIONROWINDEX      =                DISPID_TABLEROW+1;
        public const int DISPID_IHTMLTABLEROW_CELLS                =                DISPID_TABLEROW+2;
        public const int DISPID_IHTMLTABLEROW_INSERTCELL           =                DISPID_TABLEROW+3;
        public const int DISPID_IHTMLTABLEROW_DELETECELL           =                DISPID_TABLEROW+4;
        #endregion

        #region DISPIDs for interface IHTMLTableCell

        public const int DISPID_IHTMLTABLECELL_ROWSPAN                             = DISPID_TABLECELL+1;
        public const int DISPID_IHTMLTABLECELL_COLSPAN                             = DISPID_TABLECELL+2;
        public const int DISPID_IHTMLTABLECELL_ALIGN                               = STDPROPID_XOBJ_BLOCKALIGN;
        public const int DISPID_IHTMLTABLECELL_VALIGN                              = DISPID_A_TABLEVALIGN;
        public const int DISPID_IHTMLTABLECELL_BGCOLOR                             = DISPID_BACKCOLOR;
        public const int DISPID_IHTMLTABLECELL_NOWRAP                              = DISPID_A_NOWRAP;
        public const int DISPID_IHTMLTABLECELL_BACKGROUND                          = DISPID_A_BACKGROUNDIMAGE;
        public const int DISPID_IHTMLTABLECELL_BORDERCOLOR                         = DISPID_A_TABLEBORDERCOLOR;
        public const int DISPID_IHTMLTABLECELL_BORDERCOLORLIGHT                    = DISPID_A_TABLEBORDERCOLORLIGHT;
        public const int DISPID_IHTMLTABLECELL_BORDERCOLORDARK                     = DISPID_A_TABLEBORDERCOLORDARK;
        public const int DISPID_IHTMLTABLECELL_WIDTH                               = STDPROPID_XOBJ_WIDTH;
        public const int DISPID_IHTMLTABLECELL_HEIGHT                              = STDPROPID_XOBJ_HEIGHT;
        public const int DISPID_IHTMLTABLECELL_CELLINDEX                           = DISPID_TABLECELL+3;

        #endregion

        #region DISPIDs for interface IHTMLTableCell2

        public const int DISPID_IHTMLTABLECELL2_ABBR                               = DISPID_TABLECELL+4;
        public const int DISPID_IHTMLTABLECELL2_AXIS                               = DISPID_TABLECELL+5;
        public const int DISPID_IHTMLTABLECELL2_CH                                 = DISPID_TABLECELL+6;
        public const int DISPID_IHTMLTABLECELL2_CHOFF                              = DISPID_TABLECELL+7;
        public const int DISPID_IHTMLTABLECELL2_HEADERS                            = DISPID_TABLECELL+8;
        public const int DISPID_IHTMLTABLECELL2_SCOPE                              = DISPID_TABLECELL+9;

        #endregion

        #region DISPIDs for interface IHTMLTableCol

        public const int DISPID_IHTMLTABLECOL_SPAN                                 = DISPID_TABLECOL+1;
        public const int DISPID_IHTMLTABLECOL_WIDTH                                = STDPROPID_XOBJ_WIDTH;
        public const int DISPID_IHTMLTABLECOL_ALIGN                                = STDPROPID_XOBJ_BLOCKALIGN;
        public const int DISPID_IHTMLTABLECOL_VALIGN                               = DISPID_A_TABLEVALIGN;

        #endregion

        #region DISPIDs for interface IHTMLTableCol2

        public const int DISPID_IHTMLTABLECOL2_CH                                  = DISPID_TABLECOL+2;
        public const int DISPID_IHTMLTABLECOL2_CHOFF                               = DISPID_TABLECOL+3;

        #endregion

        #region DISPIDs for interface IHTMLTableSection

        public const int DISPID_IHTMLTABLESECTION_ALIGN                            = STDPROPID_XOBJ_BLOCKALIGN;
        public const int DISPID_IHTMLTABLESECTION_VALIGN                           = DISPID_A_TABLEVALIGN;
        public const int DISPID_IHTMLTABLESECTION_BGCOLOR                          = DISPID_BACKCOLOR;
        public const int DISPID_IHTMLTABLESECTION_ROWS                             = DISPID_TABLESECTION;
        public const int DISPID_IHTMLTABLESECTION_INSERTROW                        = DISPID_TABLESECTION+1;
        public const int DISPID_IHTMLTABLESECTION_DELETEROW                        = DISPID_TABLESECTION+2;

        #endregion

        #region DISPIDs for interface IHTMLTableSection2

        public const int DISPID_IHTMLTABLESECTION2_MOVEROW                         = DISPID_TABLESECTION+3;

        #endregion

        #region DISPIDs for interface IHTMLTableSection3

        public const int DISPID_IHTMLTABLESECTION3_CH                              = DISPID_TABLESECTION+4;
        public const int DISPID_IHTMLTABLESECTION3_CHOFF                           = DISPID_TABLESECTION+5;

        #endregion

        #region DISPIDs for interface IHTMLTxtRange
        public const int  DISPID_RANGE               =								DISPID_NORMAL_FIRST;
        public const int DISPID_IHTMLTXTRANGE_HTMLTEXT       =                      DISPID_RANGE+3;
        public const int DISPID_IHTMLTXTRANGE_TEXT            =                     DISPID_RANGE+4;
        public const int DISPID_IHTMLTXTRANGE_PARENTELEMENT    =                    DISPID_RANGE+6;
        public const int DISPID_IHTMLTXTRANGE_DUPLICATE         =                   DISPID_RANGE+8;
        public const int DISPID_IHTMLTXTRANGE_INRANGE            =                  DISPID_RANGE+10;
        public const int DISPID_IHTMLTXTRANGE_ISEQUAL            =                  DISPID_RANGE+11;
        public const int DISPID_IHTMLTXTRANGE_SCROLLINTOVIEW      =                 DISPID_RANGE+12;
        public const int DISPID_IHTMLTXTRANGE_COLLAPSE            =                 DISPID_RANGE+13;
        public const int DISPID_IHTMLTXTRANGE_EXPAND              =                 DISPID_RANGE+14;
        public const int DISPID_IHTMLTXTRANGE_MOVE                =                 DISPID_RANGE+15;
        public const int DISPID_IHTMLTXTRANGE_MOVESTART           =                 DISPID_RANGE+16;
        public const int DISPID_IHTMLTXTRANGE_MOVEEND             =                 DISPID_RANGE+17;
        public const int DISPID_IHTMLTXTRANGE_SELECT               =                DISPID_RANGE+24;
        public const int DISPID_IHTMLTXTRANGE_PASTEHTML            =                DISPID_RANGE+26;
        public const int DISPID_IHTMLTXTRANGE_MOVETOELEMENTTEXT     =               DISPID_RANGE+1;
        public const int DISPID_IHTMLTXTRANGE_SETENDPOINT           =               DISPID_RANGE+25;
        public const int DISPID_IHTMLTXTRANGE_COMPAREENDPOINTS      =               DISPID_RANGE+18;
        public const int DISPID_IHTMLTXTRANGE_FINDTEXT              =               DISPID_RANGE+19;
        public const int DISPID_IHTMLTXTRANGE_MOVETOPOINT            =              DISPID_RANGE+20;
        public const int DISPID_IHTMLTXTRANGE_GETBOOKMARK             =             DISPID_RANGE+21;
        public const int DISPID_IHTMLTXTRANGE_MOVETOBOOKMARK           =            DISPID_RANGE+9;
        public const int DISPID_IHTMLTXTRANGE_QUERYCOMMANDSUPPORTED     =           DISPID_RANGE+27;
        public const int DISPID_IHTMLTXTRANGE_QUERYCOMMANDENABLED       =           DISPID_RANGE+28;
        public const int DISPID_IHTMLTXTRANGE_QUERYCOMMANDSTATE          =          DISPID_RANGE+29;
        public const int DISPID_IHTMLTXTRANGE_QUERYCOMMANDINDETERM       =          DISPID_RANGE+30;
        public const int DISPID_IHTMLTXTRANGE_QUERYCOMMANDTEXT           =          DISPID_RANGE+31;
        public const int DISPID_IHTMLTXTRANGE_QUERYCOMMANDVALUE          =          DISPID_RANGE+32;
        public const int DISPID_IHTMLTXTRANGE_EXECCOMMAND                =          DISPID_RANGE+33;
        public const int DISPID_IHTMLTXTRANGE_EXECCOMMANDSHOWHELP         =         DISPID_RANGE+34;
        #endregion

        #region HtmlApi Command Constants
        public const int IDM_UNKNOWN                 =0;
        public const int IDM_ALIGNBOTTOM             =1;
        public const int IDM_ALIGNHORIZONTALCENTERS  =2;
        public const int IDM_ALIGNLEFT               =3;
        public const int IDM_ALIGNRIGHT              =4;
        public const int IDM_ALIGNTOGRID             =5;
        public const int IDM_ALIGNTOP                =6;
        public const int IDM_ALIGNVERTICALCENTERS    =7;
        public const int IDM_ARRANGEBOTTOM           =8;
        public const int IDM_ARRANGERIGHT            =9;
        public const int IDM_BRINGFORWARD            =10;
        public const int IDM_BRINGTOFRONT            =11;
        public const int IDM_CENTERHORIZONTALLY      =12;
        public const int IDM_CENTERVERTICALLY        =13;
        public const int IDM_CODE                    =14;
        public const int IDM_DELETE                  =17;
        public const int IDM_FONTNAME                =18;
        public const int IDM_FONTSIZE                =19;
        public const int IDM_GROUP                   =20;
        public const int IDM_HORIZSPACECONCATENATE   =21;
        public const int IDM_HORIZSPACEDECREASE      =22;
        public const int IDM_HORIZSPACEINCREASE      =23;
        public const int IDM_HORIZSPACEMAKEEQUAL     =24;
        public const int IDM_INSERTOBJECT            =25;
        public const int IDM_MULTILEVELREDO          =30;
        public const int IDM_SENDBACKWARD            =32;
        public const int IDM_SENDTOBACK              =33;
        public const int IDM_SHOWTABLE               =34;
        public const int IDM_SIZETOCONTROL           =35;
        public const int IDM_SIZETOCONTROLHEIGHT     =36;
        public const int IDM_SIZETOCONTROLWIDTH      =37;
        public const int IDM_SIZETOFIT               =38;
        public const int IDM_SIZETOGRID              =39;
        public const int IDM_SNAPTOGRID              =40;
        public const int IDM_TABORDER                =41;
        public const int IDM_TOOLBOX                 =42;
        public const int IDM_MULTILEVELUNDO          =44;
        public const int IDM_UNGROUP                 =45;
        public const int IDM_VERTSPACECONCATENATE    =46;
        public const int IDM_VERTSPACEDECREASE       =47;
        public const int IDM_VERTSPACEINCREASE       =48;
        public const int IDM_VERTSPACEMAKEEQUAL      =49;
        public const int IDM_JUSTIFYFULL             =50;
        public const int IDM_BACKCOLOR               =51;
        public const int IDM_BOLD                    =52;
        public const int IDM_BORDERCOLOR             =53;
        public const int IDM_FLAT                    =54;
        public const int IDM_FORECOLOR               =55;
        public const int IDM_ITALIC                  =56;
        public const int IDM_JUSTIFYCENTER           =57;
        public const int IDM_JUSTIFYGENERAL          =58;
        public const int IDM_JUSTIFYLEFT             =59;
        public const int IDM_JUSTIFYRIGHT            =60;
        public const int IDM_RAISED                  =61;
        public const int IDM_SUNKEN                  =62;
        public const int IDM_UNDERLINE               =63;
        public const int IDM_CHISELED                =64;
        public const int IDM_ETCHED                  =65;
        public const int IDM_SHADOWED                =66;
        public const int IDM_FIND                    =67;
        public const int IDM_SHOWGRID                =69;
        public const int IDM_OBJECTVERBLIST0         =72;
        public const int IDM_OBJECTVERBLIST1         =73;
        public const int IDM_OBJECTVERBLIST2         =74;
        public const int IDM_OBJECTVERBLIST3         =75;
        public const int IDM_OBJECTVERBLIST4         =76;
        public const int IDM_OBJECTVERBLIST5         =77;
        public const int IDM_OBJECTVERBLIST6         =78;
        public const int IDM_OBJECTVERBLIST7         =79;
        public const int IDM_OBJECTVERBLIST8         =80;
        public const int IDM_OBJECTVERBLIST9         =81;
        public const int IDM_OBJECTVERBLISTLAST = IDM_OBJECTVERBLIST9;
        public const int IDM_CONVERTOBJECT       =    82;
        public const int IDM_CUSTOMCONTROL       =    83;
        public const int IDM_CUSTOMIZEITEM       =    84;
        public const int IDM_RENAME              =    85;
        public const int IDM_IMPORT              =    86;
        public const int IDM_NEWPAGE             =    87;
        public const int IDM_MOVE                =    88;
        public const int IDM_CANCEL              =    89;
        public const int IDM_FONT                =    90;
        public const int IDM_STRIKETHROUGH       =    91;
        public const int IDM_DELETEWORD          =    92;
        public const int IDM_EXECPRINT           =    93;
        public const int IDM_JUSTIFYNONE         =    94;
        public const int IDM_TRISTATEBOLD        =    95;
        public const int IDM_TRISTATEITALIC      =    96;
        public const int IDM_TRISTATEUNDERLINE   =    97;
        public const int IDM_FOLLOW_ANCHOR        =   2008;
        public const int IDM_INSINPUTIMAGE         =  2114;
        public const int IDM_INSINPUTBUTTON        =  2115;
        public const int IDM_INSINPUTRESET         =  2116;
        public const int IDM_INSINPUTSUBMIT        =  2117;
        public const int IDM_INSINPUTUPLOAD        =  2118;
        public const int IDM_INSFIELDSET           =  2119;
        public const int IDM_PASTEINSERT          =   2120;
        public const int IDM_REPLACE              =   2121;
        public const int IDM_EDITSOURCE           =   2122;
        public const int IDM_BOOKMARK             =   2123;
        public const int IDM_HYPERLINK            =   2124;
        public const int IDM_UNLINK               =   2125;
        public const int IDM_BROWSEMODE           =   2126;
        public const int IDM_EDITMODE             =   2127;
        public const int IDM_UNBOOKMARK           =   2128;
        public const int IDM_TOOLBARS             =   2130;
        public const int IDM_STATUSBAR            =   2131;
        public const int IDM_FORMATMARK           =   2132;
        public const int IDM_TEXTONLY             =   2133;
        public const int IDM_OPTIONS              =   2135;
        public const int IDM_FOLLOWLINKC          =   2136;
        public const int IDM_FOLLOWLINKN          =   2137;
        public const int IDM_VIEWSOURCE           =   2139;
        public const int IDM_ZOOMPOPUP            =   2140;
        public const int IDM_BASELINEFONT1       =    2141;
        public const int IDM_BASELINEFONT2       =    2142;
        public const int IDM_BASELINEFONT3       =    2143;
        public const int IDM_BASELINEFONT4       =    2144;
        public const int IDM_BASELINEFONT5       =    2145;
        public const int IDM_HORIZONTALLINE      =    2150;
        public const int IDM_LINEBREAKNORMAL     =    2151;
        public const int IDM_LINEBREAKLEFT       =    2152;
        public const int IDM_LINEBREAKRIGHT      =    2153;
        public const int IDM_LINEBREAKBOTH       =    2154;
        public const int IDM_NONBREAK            =    2155;
        public const int IDM_SPECIALCHAR         =    2156;
        public const int IDM_HTMLSOURCE          =    2157;
        public const int IDM_IFRAME              =    2158;
        public const int IDM_HTMLCONTAIN         =    2159;
        public const int IDM_TEXTBOX             =    2161;
        public const int IDM_TEXTAREA            =    2162;
        public const int IDM_CHECKBOX            =    2163;
        public const int IDM_RADIOBUTTON         =    2164;
        public const int IDM_DROPDOWNBOX         =    2165;
        public const int IDM_LISTBOX             =    2166;
        public const int IDM_BUTTON              =    2167;
        public const int IDM_IMAGE               =    2168;
        public const int IDM_OBJECT              =    2169;
        public const int IDM_1D                  =    2170;
        public const int IDM_IMAGEMAP            =    2171;
        public const int IDM_FILE                =    2172;
        public const int IDM_COMMENT             =    2173;
        public const int IDM_SCRIPT              =    2174;
        public const int IDM_JAVAAPPLET          =    2175;
        public const int IDM_PLUGIN              =    2176;
        public const int IDM_PAGEBREAK           =    2177;
        public const int IDM_HTMLAREA            =    2178;
        public const int IDM_PARAGRAPH           =    2180;
        public const int IDM_FORM                =    2181;
        public const int IDM_MARQUEE             =    2182;
        public const int IDM_LIST                =    2183;
        public const int IDM_ORDERLIST           =    2184;
        public const int IDM_UNORDERLIST         =    2185;
        public const int IDM_INDENT              =    2186;
        public const int IDM_OUTDENT             =    2187;
        public const int IDM_PREFORMATTED        =    2188;
        public const int IDM_ADDRESS             =    2189;
        public const int IDM_BLINK               =    2190;
        public const int IDM_DIV                 =    2191;
        public const int IDM_TABLEINSERT         =    2200;
        public const int IDM_RCINSERT            =    2201;
        public const int IDM_CELLINSERT          =    2202;
        public const int IDM_CAPTIONINSERT       =    2203;
        public const int IDM_CELLMERGE           =    2204;
        public const int IDM_CELLSPLIT           =    2205;
        public const int IDM_CELLSELECT          =    2206;
        public const int IDM_ROWSELECT           =    2207;
        public const int IDM_COLUMNSELECT        =    2208;
        public const int IDM_TABLESELECT         =    2209;
        public const int IDM_TABLEPROPERTIES     =    2210;
        public const int IDM_CELLPROPERTIES      =    2211;
        public const int IDM_ROWINSERT           =    2212;
        public const int IDM_COLUMNINSERT        =    2213;
        public const int IDM_HELP_CONTENT         =   2220;
        public const int IDM_HELP_ABOUT           =   2221;
        public const int IDM_HELP_README          =   2222;
        public const int IDM_REMOVEFORMAT          =  2230;
        public const int IDM_PAGEINFO             =   2231;
        public const int IDM_TELETYPE             =   2232;
        public const int IDM_GETBLOCKFMTS          =  2233;
        public const int IDM_BLOCKFMT              =  2234;
        public const int IDM_SHOWHIDE_CODE         =  2235;
        public const int IDM_TABLE                 =  2236;
        public const int IDM_COPYFORMAT             = 2237;
        public const int IDM_PASTEFORMAT         =    2238;
        public const int IDM_GOTO                 =   2239;
        public const int IDM_CHANGEFONT            =  2240;
        public const int IDM_CHANGEFONTSIZE        =  2241;
        public const int IDM_CHANGECASE            =  2246;
        public const int IDM_SHOWSPECIALCHAR       =  2249;
        public const int IDM_SUBSCRIPT             =  2247;
        public const int IDM_SUPERSCRIPT           =  2248;
        public const int IDM_CENTERALIGNPARA       =  2250;
        public const int IDM_LEFTALIGNPARA         =  2251;
        public const int IDM_RIGHTALIGNPARA        =  2252;
        public const int IDM_REMOVEPARAFORMAT      =  2253;
        public const int IDM_APPLYNORMAL           =  2254;
        public const int IDM_APPLYHEADING1         =  2255;
        public const int IDM_APPLYHEADING2         =  2256;
        public const int IDM_APPLYHEADING3         =  2257;
        public const int IDM_DOCPROPERTIES         =  2260;
        public const int IDM_ADDFAVORITES          =  2261;
        public const int IDM_COPYSHORTCUT          =  2262;
        public const int IDM_SAVEBACKGROUND        =  2263;
        public const int IDM_SETWALLPAPER          =  2264;
        public const int IDM_COPYBACKGROUND        =  2265;
        public const int IDM_CREATESHORTCUT        =  2266;
        public const int IDM_PAGE                  =  2267;
        public const int IDM_SAVETARGET            =  2268;
        public const int IDM_SHOWPICTURE           =  2269;
        public const int IDM_SAVEPICTURE           =  2270;
        public const int IDM_DYNSRCPLAY            =  2271;
        public const int IDM_DYNSRCSTOP            =  2272;
        public const int IDM_PRINTTARGET           =  2273;
        public const int IDM_IMGARTPLAY            =  2274;
        public const int IDM_IMGARTSTOP            =  2275;
        public const int IDM_IMGARTREWIND          =  2276;
        public const int IDM_PRINTQUERYJOBSPENDING =  2277;
        public const int IDM_SETDESKTOPITEM        =  2278;
        public const int IDM_CONTEXTMENU           =  2280;
        public const int IDM_GOBACKWARD            =  2282;
        public const int IDM_GOFORWARD             =  2283;
        public const int IDM_PRESTOP               =  2284;
        public const int IDM_MP_MYPICS             =  2287;
        public const int IDM_MP_EMAILPICTURE       =  2288;
        public const int IDM_MP_PRINTPICTURE       =  2289;
        public const int IDM_CREATELINK           =   2290;
        public const int IDM_COPYCONTENT          =   2291;
        public const int IDM_LANGUAGE             =   2292;
        public const int IDM_GETPRINTTEMPLATE    =    2295;
        public const int IDM_SETPRINTTEMPLATE    =    2296;
        public const int IDM_TEMPLATE_PAGESETUP  =    2298;
        public const int IDM_REFRESH              =   2300;
        public const int IDM_STOPDOWNLOAD         =   2301;
        public const int IDM_ENABLE_INTERACTION    =  2302;
        public const int IDM_LAUNCHDEBUGGER         = 2310;
        public const int IDM_BREAKATNEXT            = 2311;
        public const int IDM_INSINPUTHIDDEN        =  2312;
        public const int IDM_INSINPUTPASSWORD      =  2313;
        public const int IDM_OVERWRITE          =     2314;
        public const int IDM_PARSECOMPLETE       =    2315;
        public const int IDM_HTMLEDITMODE       =     2316;
        public const int IDM_REGISTRYREFRESH      =   2317;
        public const int IDM_COMPOSESETTINGS      =   2318;
        public const int IDM_SHOWALLTAGS           =  2327;
        public const int IDM_SHOWALIGNEDSITETAGS   =  2321;
        public const int IDM_SHOWSCRIPTTAGS        =  2322;
        public const int IDM_SHOWSTYLETAGS         =  2323;
        public const int IDM_SHOWCOMMENTTAGS       =  2324;
        public const int IDM_SHOWAREATAGS          =  2325;
        public const int IDM_SHOWUNKNOWNTAGS       =  2326;
        public const int IDM_SHOWMISCTAGS          =  2320;
        public const int IDM_SHOWZEROBORDERATDESIGNTIME  =       2328;
        public const int IDM_AUTODETECT         =     2329;
        public const int IDM_SCRIPTDEBUGGER     =     2330;
        public const int IDM_GETBYTESDOWNLOADED  =    2331;
        public const int IDM_NOACTIVATENORMALOLECONTROLS   =     2332;
        public const int IDM_NOACTIVATEDESIGNTIMECONTROLS  =     2333;
        public const int IDM_NOACTIVATEJAVAAPPLETS          =    2334;
        public const int IDM_NOFIXUPURLSONPASTE              =   2335;
        public const int IDM_EMPTYGLYPHTABLE   =      2336;
        public const int IDM_ADDTOGLYPHTABLE   =      2337;
        public const int IDM_REMOVEFROMGLYPHTABLE =   2338;
        public const int IDM_REPLACEGLYPHCONTENTS  =  2339;
        public const int IDM_SHOWWBRTAGS            = 2340;
        public const int IDM_PERSISTSTREAMSYNC      = 2341;
        public const int IDM_SETDIRTY              =  2342;
        public const int IDM_RUNURLSCRIPT       =     2343;
        public const int IDM_ZOOMRATIO          =     2344;
        public const int IDM_GETZOOMNUMERATOR    =    2345;
        public const int IDM_GETZOOMDENOMINATOR   =   2346;


        // COMMANDS FOR COMPLEX TEXT
        public const int IDM_DIRLTR                =  2350;
        public const int IDM_DIRRTL               =   2351;
        public const int IDM_BLOCKDIRLTR          =   2352;
        public const int IDM_BLOCKDIRRTL          =   2353;
        public const int IDM_INLINEDIRLTR         =   2354;
        public const int IDM_INLINEDIRRTL         =   2355;

        // SHDOCVW
        public const int IDM_ISTRUSTEDDLG     =       2356;

        // MSHTMLED
        public const int IDM_INSERTSPAN        =      2357;
        public const int IDM_LOCALIZEEDITOR     =     2358;

        // XML MIMEVIEWER
        public const int IDM_SAVEPRETRANSFORMSOURCE = 2370;
        public const int IDM_VIEWPRETRANSFORMSOURCE = 2371;

        // Scrollbar context menu
        public const int IDM_SCROLL_HERE            = 2380;
        public const int IDM_SCROLL_TOP             = 2381;
        public const int IDM_SCROLL_BOTTOM          = 2382;
        public const int IDM_SCROLL_PAGEUP          = 2383;
        public const int IDM_SCROLL_PAGEDOWN        = 2384;
        public const int IDM_SCROLL_UP              = 2385;
        public const int IDM_SCROLL_DOWN            = 2386;
        public const int IDM_SCROLL_LEFTEDGE        = 2387;
        public const int IDM_SCROLL_RIGHTEDGE       = 2388;
        public const int IDM_SCROLL_PAGELEFT        = 2389;
        public const int IDM_SCROLL_PAGERIGHT       = 2390;
        public const int IDM_SCROLL_LEFT            = 2391;
        public const int IDM_SCROLL_RIGHT           = 2392;

        // IE 6 Form Editing Commands
        public const int IDM_MULTIPLESELECTION      = 2393;
        public const int IDM_2D_POSITION            = 2394;
        public const int IDM_2D_ELEMENT             = 2395;
        public const int IDM_1D_ELEMENT             = 2396;
        public const int IDM_ABSOLUTE_POSITION      = 2397;
        public const int IDM_LIVERESIZE             = 2398;
        public const int IDM_ATOMICSELECTION	=		2399;

        // Auto URL detection mode
        public const int IDM_AUTOURLDETECT_MODE  =    2400;

        // Legacy IE50 compatible paste
        public const int IDM_IE50_PASTE          =    2401;

        // ie50 paste mode
        public const int IDM_IE50_PASTE_MODE      =   2402;

        //;begin_public
        public const int IDM_GETIPRINT             =  2403;
        //;end_public

        // for disabling selection handles
        public const int IDM_DISABLE_EDITFOCUS_UI   = 2404;

        // for visibility/display in design
        public const int IDM_RESPECTVISIBILITY_INDESIGN = 2405;

        // set css mode
        public const int IDM_CSSEDITING_LEVEL         =   2406;

        // New outdent
        public const int IDM_UI_OUTDENT                =  2407;

        // Printing Status
        public const int IDM_UPDATEPAGESTATUS           = 2408;

        // IME Reconversion 
        public const int IDM_IME_ENABLE_RECONVERSION	=	2409;
        public const int	IDM_KEEPSELECTION			=	2410;
        public const int IDM_UNLOADDOCUMENT             = 2411;
        public const int IDM_OVERRIDE_CURSOR            = 2420;
        public const int IDM_PEERHITTESTSAMEINEDIT      = 2423;
        public const int IDM_TRUSTAPPCACHE              = 2425;
        public const int IDM_BACKGROUNDIMAGECACHE       = 2430;
        public const int IDM_DEFAULTBLOCK           =     6046;
        public const int IDM_MIMECSET__FIRST__       =    3609;
        public const int IDM_MIMECSET__LAST__         =   3699;
        public const int IDM_MENUEXT_FIRST__   =    3700;
        public const int IDM_MENUEXT_LAST__     =   3732;
        public const int IDM_MENUEXT_COUNT       =  3733;

        // Commands mapped from the standard set.  We should
        // consider deleting them from public header files.

        public const int IDM_OPEN                =    2000;
        public const int IDM_NEW                 =    2001;
        public const int IDM_SAVE                =    70;
        public const int IDM_SAVEAS              =    71;
        public const int IDM_SAVECOPYAS          =    2002;
        public const int IDM_PRINTPREVIEW        =    2003;
        public const int IDM_SHOWPRINT           =    2010;
        public const int IDM_SHOWPAGESETUP       =    2011;
        public const int IDM_PRINT               =    27;
        public const int IDM_PAGESETUP           =    2004;
        public const int IDM_SPELL               =    2005;
        public const int IDM_PASTESPECIAL        =    2006;
        public const int IDM_CLEARSELECTION      =    2007;
        public const int IDM_PROPERTIES          =    28;
        public const int IDM_REDO                =    29;
        public const int IDM_UNDO                =    43;
        public const int IDM_SELECTALL           =    31;
        public const int IDM_ZOOMPERCENT         =    50;
        public const int IDM_GETZOOM             =    68;
        public const int IDM_STOP                =    2138;
        public const int IDM_COPY                =    15;
        public const int IDM_CUT                 =    16;
        public const int IDM_PASTE               =    26;

        // Defines for IDM_ZOOMPERCENT
        public const int CMD_ZOOM_PAGEWIDTH = -1;
        public const int CMD_ZOOM_ONEPAGE = -2;
        public const int CMD_ZOOM_TWOPAGES = -3;
        public const int CMD_ZOOM_SELECTION = -4;
        public const int CMD_ZOOM_FIT=  -5;

        // IDMs for CGID_EditStateCommands group 
        public const int IDM_CONTEXT   =              1; 
        public const int IDM_HWND      =              2;

        // Shdocvw Execs on CGID_DocHostCommandHandler
        public const int IDM_NEW_TOPLEVELWINDOW    =  7050;

        #endregion

        #endregion


        #region Interfaces

        #region HTMLDocument
        [ComVisible(true), ComImport(), Guid("25336920-03F9-11CF-8FD0-00AA00686F13")]
		public class HTMLDocument {
			
		}

		[ComVisible(true), ComImport(), Guid("3050F613-98B5-11CF-BB82-00AA00BDCE0B")]
		public class HTMLTable {
		}

		[ComVisible(true), ComImport(), Guid("3050f26d-98b5-11cf-bb82-00aa00bdce0b")]
		public class HTMLTableRow {
			
		}
		[ComVisible(true), ComImport(), Guid("3050f246-98b5-11cf-bb82-00aa00bdce0b")]
		public class HTMLTableCell {
			
		}
		#endregion

		#region DispHTMLDocument
		[InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIDispatch)]
			[GuidAttribute("3050f55f-98b5-11cf-bb82-00aa00bdce0b")]
			[ComImportAttribute()]
		public interface DispHTMLDocument {

			object onstop {
				[DispIdAttribute(-2147412044)]
				get;

				[DispIdAttribute(-2147412044)]
				set;
			}

			string defaultCharset {
				[DispIdAttribute(1033)]
				get;

				[DispIdAttribute(1033)]
				set;
			}

			object onselectstart {
				[DispIdAttribute(-2147412075)]
				get;

				[DispIdAttribute(-2147412075)]
				set;
			}

			string URL {
				[DispIdAttribute(1025)]
				get;

				[DispIdAttribute(1025)]
				set;
			}

			object namespaces {
				[DispIdAttribute(1091)]
				get;
			}

			object onrowenter {
				[DispIdAttribute(-2147412093)]
				get;

				[DispIdAttribute(-2147412093)]
				set;
			}

			IHTMLElement activeElement {
				[DispIdAttribute(1005)]
				get;
			}

			object oncontextmenu {
				[DispIdAttribute(-2147412047)]
				get;

				[DispIdAttribute(-2147412047)]
				set;
			}

			IHTMLDocument2 parentDocument {
				[DispIdAttribute(1078)]
				get;
			}

			IHTMLDOMNode doctype {
				[DispIdAttribute(1098)]
				get;
			}

			object onmousewheel {
				[DispIdAttribute(-2147412036)]
				get;

				[DispIdAttribute(-2147412036)]
				set;
			}

			object linkColor {
				[DispIdAttribute(1024)]
				get;

				[DispIdAttribute(1024)]
				set;
			}

			object alinkColor {
				[DispIdAttribute(1022)]
				get;

				[DispIdAttribute(1022)]
				set;
			}

			object onrowsinserted {
				[DispIdAttribute(-2147412049)]
				get;

				[DispIdAttribute(-2147412049)]
				set;
			}

			object Script {
				[DispIdAttribute(1001)]
				get;
			}

			object onmousedown {
				[DispIdAttribute(-2147412110)]
				get;

				[DispIdAttribute(-2147412110)]
				set;
			}

			IHTMLElementCollection frames {
				[DispIdAttribute(1019)]
				get;
			}

			object ondblclick {
				[DispIdAttribute(-2147412103)]
				get;

				[DispIdAttribute(-2147412103)]
				set;
			}

			IHTMLDOMNode nextSibling {
				[DispIdAttribute(-2147417033)]
				get;
			}

			IHTMLDOMNode firstChild {
				[DispIdAttribute(-2147417036)]
				get;
			}

			object onbeforeupdate {
				[DispIdAttribute(-2147412091)]
				get;

				[DispIdAttribute(-2147412091)]
				set;
			}

			IHTMLElementCollection images {
				[DispIdAttribute(1011)]
				get;
			}

			string domain {
				[DispIdAttribute(1029)]
				get;

				[DispIdAttribute(1029)]
				set;
			}

			object onbeforeactivate {
				[DispIdAttribute(-2147412022)]
				get;

				[DispIdAttribute(-2147412022)]
				set;
			}

			string compatMode {
				[DispIdAttribute(1102)]
				get;
			}

			object nodeValue {
				[DispIdAttribute(-2147417037)]
				get;

				[DispIdAttribute(-2147417037)]
				set;
			}

			object onmouseup {
				[DispIdAttribute(-2147412109)]
				get;

				[DispIdAttribute(-2147412109)]
				set;
			}

			object onhelp {
				[DispIdAttribute(-2147412099)]
				get;

				[DispIdAttribute(-2147412099)]
				set;
			}

			object onbeforedeactivate {
				[DispIdAttribute(-2147412035)]
				get;

				[DispIdAttribute(-2147412035)]
				set;
			}

			object onmouseout {
				[DispIdAttribute(-2147412111)]
				get;

				[DispIdAttribute(-2147412111)]
				set;
			}

			object onrowexit {
				[DispIdAttribute(-2147412094)]
				get;

				[DispIdAttribute(-2147412094)]
				set;
			}

			IHTMLSelectionObject selection {
				[DispIdAttribute(1017)]
				get;
			}

			object vlinkColor {
				[DispIdAttribute(1023)]
				get;

				[DispIdAttribute(1023)]
				set;
			}

			object onfocusout {
				[DispIdAttribute(-2147412020)]
				get;

				[DispIdAttribute(-2147412020)]
				set;
			}

			object onfocusin {
				[DispIdAttribute(-2147412021)]
				get;

				[DispIdAttribute(-2147412021)]
				set;
			}

			object childNodes {
				[DispIdAttribute(-2147417063)]
				get;
			}

			object onclick {
				[DispIdAttribute(-2147412104)]
				get;

				[DispIdAttribute(-2147412104)]
				set;
			}

			object onpropertychange {
				[DispIdAttribute(-2147412065)]
				get;

				[DispIdAttribute(-2147412065)]
				set;
			}

			object attributes {
				[DispIdAttribute(-2147417062)]
				get;
			}

			string charset {
				[DispIdAttribute(1032)]
				get;

				[DispIdAttribute(1032)]
				set;
			}

			string mimeType {
				[DispIdAttribute(1041)]
				get;
			}

			string readyState {
				[DispIdAttribute(1018)]
				get;
			}

			object onafterupdate {
				[DispIdAttribute(-2147412090)]
				get;

				[DispIdAttribute(-2147412090)]
				set;
			}

			IHTMLDOMNode parentNode {
				[DispIdAttribute(-2147417065)]
				get;
			}

			string uniqueID {
				[DispIdAttribute(1077)]
				get;
			}

			IHTMLElement body {
				[DispIdAttribute(1004)]
				get;
			}

			bool expando {
				[DispIdAttribute(1031)]
				get;

				[DispIdAttribute(1031)]
				set;
			}

			object bgColor {
				[DispIdAttribute(-501)]
				get;

				[DispIdAttribute(-501)]
				set;
			}

			string fileUpdatedDate {
				[DispIdAttribute(1045)]
				get;
			}

			string designMode {
				[DispIdAttribute(1014)]
				get;

				[DispIdAttribute(1014)]
				set;
			}

			object onmousemove {
				[DispIdAttribute(-2147412108)]
				get;

				[DispIdAttribute(-2147412108)]
				set;
			}

			object onbeforeeditfocus {
				[DispIdAttribute(-2147412043)]
				get;

				[DispIdAttribute(-2147412043)]
				set;
			}

			object onkeydown {
				[DispIdAttribute(-2147412107)]
				get;

				[DispIdAttribute(-2147412107)]
				set;
			}

			bool enableDownload {
				[DispIdAttribute(1079)]
				get;

				[DispIdAttribute(1079)]
				set;
			}

			IHTMLDOMImplementation implementation {
				[DispIdAttribute(1099)]
				get;
			}

			string fileCreatedDate {
				[DispIdAttribute(1043)]
				get;
			}

			IHTMLElementCollection all {
				[DispIdAttribute(1003)]
				get;
			}

			string cookie {
				[DispIdAttribute(1030)]
				get;

				[DispIdAttribute(1030)]
				set;
			}

			object onactivate {
				[DispIdAttribute(-2147412025)]
				get;

				[DispIdAttribute(-2147412025)]
				set;
			}

			object onselectionchange {
				[DispIdAttribute(-2147412032)]
				get;

				[DispIdAttribute(-2147412032)]
				set;
			}

			string referrer {
				[DispIdAttribute(1027)]
				get;
			}

//			IHTMLLocation location {
//				[DispIdAttribute(1026)]
//				get;
//			}

			object ondeactivate {
				[DispIdAttribute(-2147412024)]
				get;

				[DispIdAttribute(-2147412024)]
				set;
			}

			string title {
				[DispIdAttribute(1012)]
				get;

				[DispIdAttribute(1012)]
				set;
			}

			object onerrorupdate {
				[DispIdAttribute(-2147412074)]
				get;

				[DispIdAttribute(-2147412074)]
				set;
			}

			string dir {
				[DispIdAttribute(-2147412995)]
				get;

				[DispIdAttribute(-2147412995)]
				set;
			}

			string baseUrl {
				[DispIdAttribute(1080)]
				get;

				[DispIdAttribute(1080)]
				set;
			}

			object onreadystatechange {
				[DispIdAttribute(-2147412087)]
				get;

				[DispIdAttribute(-2147412087)]
				set;
			}

			object oncontrolselect {
				[DispIdAttribute(-2147412033)]
				get;

				[DispIdAttribute(-2147412033)]
				set;
			}

			string fileModifiedDate {
				[DispIdAttribute(1044)]
				get;
			}

			object ondataavailable {
				[DispIdAttribute(-2147412071)]
				get;

				[DispIdAttribute(-2147412071)]
				set;
			}

			IHTMLElementCollection plugins {
				[DispIdAttribute(1021)]
				get;
			}

			string nameProp {
				[DispIdAttribute(1048)]
				get;
			}

			IHTMLWindow2 parentWindow {
				[DispIdAttribute(1034)]
				get;
			}

			IHTMLElementCollection scripts {
				[DispIdAttribute(1013)]
				get;
			}

			string protocol {
				[DispIdAttribute(1047)]
				get;
			}

			int nodeType {
				[DispIdAttribute(-2147417066)]
				get;
			}

			object ownerDocument {
				[DispIdAttribute(-2147416999)]
				get;
			}

			string media {
				[DispIdAttribute(1093)]
				get;

				[DispIdAttribute(1093)]
				set;
			}

			IHTMLElementCollection applets {
				[DispIdAttribute(1008)]
				get;
			}

			string nodeName {
				[DispIdAttribute(-2147417038)]
				get;
			}

			object onkeypress {
				[DispIdAttribute(-2147412105)]
				get;

				[DispIdAttribute(-2147412105)]
				set;
			}

			object oncellchange {
				[DispIdAttribute(-2147412048)]
				get;

				[DispIdAttribute(-2147412048)]
				set;
			}

			object ondatasetcomplete {
				[DispIdAttribute(-2147412070)]
				get;

				[DispIdAttribute(-2147412070)]
				set;
			}

			object ondragstart {
				[DispIdAttribute(-2147412077)]
				get;

				[DispIdAttribute(-2147412077)]
				set;
			}

			object onmouseover {
				[DispIdAttribute(-2147412112)]
				get;

				[DispIdAttribute(-2147412112)]
				set;
			}

			IHTMLElementCollection embeds {
				[DispIdAttribute(1015)]
				get;
			}

			IHTMLElementCollection anchors {
				[DispIdAttribute(1007)]
				get;
			}

			IHTMLDOMNode lastChild {
				[DispIdAttribute(-2147417035)]
				get;
			}

			object onrowsdelete {
				[DispIdAttribute(-2147412050)]
				get;

				[DispIdAttribute(-2147412050)]
				set;
			}

			bool inheritStyleSheets {
				[DispIdAttribute(1082)]
				get;

				[DispIdAttribute(1082)]
				set;
			}

			object onkeyup {
				[DispIdAttribute(-2147412106)]
				get;

				[DispIdAttribute(-2147412106)]
				set;
			}

			object fgColor {
				[DispIdAttribute(-2147413110)]
				get;

				[DispIdAttribute(-2147413110)]
				set;
			}

			string URLUnencoded {
				[DispIdAttribute(1097)]
				get;
			}

			object ondatasetchanged {
				[DispIdAttribute(-2147412072)]
				get;

				[DispIdAttribute(-2147412072)]
				set;
			}

			string security {
				[DispIdAttribute(1046)]
				get;
			}

			IHTMLDOMNode previousSibling {
				[DispIdAttribute(-2147417034)]
				get;
			}

			IHTMLElementCollection links {
				[DispIdAttribute(1009)]
				get;
			}

			IHTMLStyleSheetsCollection styleSheets {
				[DispIdAttribute(1069)]
				get;
			}

			IHTMLElement documentElement {
				[DispIdAttribute(1075)]
				get;
			}

			string lastModified {
				[DispIdAttribute(1028)]
				get;
			}

			IHTMLElementCollection forms {
				[DispIdAttribute(1010)]
				get;
			}

			string fileSize {
				[DispIdAttribute(1042)]
				get;
			}

			[DispIdAttribute(1054)]
			void write(object[] psarray);

			[DispIdAttribute(1055)]
			void writeln(object[] psarray);

			[DispIdAttribute(1056)]
			object open(string url, object name, object features, object replace);

			[DispIdAttribute(1057)]
			void close();

			[DispIdAttribute(1058)]
			void clear();

			[DispIdAttribute(1059)]
			bool queryCommandSupported(string cmdID);

			[DispIdAttribute(1060)]
			bool queryCommandEnabled(string cmdID);

			[DispIdAttribute(1061)]
			bool queryCommandState(string cmdID);

			[DispIdAttribute(1062)]
			bool queryCommandIndeterm(string cmdID);

			[DispIdAttribute(1063)]
			string queryCommandText(string cmdID);

			[DispIdAttribute(1064)]
			object queryCommandValue(string cmdID);

			[DispIdAttribute(1065)]
			bool execCommand(string cmdID, bool showUI, object value);

			[DispIdAttribute(1066)]
			bool execCommandShowHelp(string cmdID);

			[DispIdAttribute(1067)]
			IHTMLElement createElement(string eTag);

			[DispIdAttribute(1068)]
			IHTMLElement elementFromPoint(int x, int y);

			[DispIdAttribute(1070)]
			string toString();

			[DispIdAttribute(1071)]
			IHTMLStyleSheet createStyleSheet(string bstrHref, int lIndex);

			[DispIdAttribute(1072)]
			void releaseCapture();

			[DispIdAttribute(1073)]
			void recalc(bool fForce);

			[DispIdAttribute(1074)]
			IHTMLDOMNode createTextNode(string text);

			[DispIdAttribute(-2147417605)]
			bool attachEvent(string Event, object pDisp);

			[DispIdAttribute(-2147417604)]
			void detachEvent(string Event, object pDisp);

			[DispIdAttribute(1076)]
			IHTMLDocument2 createDocumentFragment();

			[DispIdAttribute(1086)]
			IHTMLElementCollection getElementsByName(string v);

			[DispIdAttribute(1088)]
			IHTMLElement getElementById(string v);

			[DispIdAttribute(1087)]
			IHTMLElementCollection getElementsByTagName(string v);

			[DispIdAttribute(1089)]
			void focus();

			[DispIdAttribute(1090)]
			bool hasFocus();

			[DispIdAttribute(1092)]
			IHTMLDocument2 createDocumentFromUrl(string bstrUrl, string bstrOptions);

			[DispIdAttribute(1094)]
			IHTMLEventObj createEventObject(object pvarEventObject);

			[DispIdAttribute(1095)]
			bool fireEvent(string bstrEventName, object pvarEventObject);

//			[DispIdAttribute(1096)]
//			IHTMLRenderStyle createRenderStyle(string v);

			[DispIdAttribute(1100)]
			IHTMLDOMAttribute createAttribute(string bstrattrName);

			[DispIdAttribute(1101)]
			IHTMLDOMNode createComment(string bstrdata);

			[DispIdAttribute(-2147417064)]
			bool hasChildNodes();

			[DispIdAttribute(-2147417061)]
			IHTMLDOMNode insertBefore(IHTMLDOMNode newChild, object refChild);

			[DispIdAttribute(-2147417060)]
			IHTMLDOMNode removeChild(IHTMLDOMNode oldChild);

			[DispIdAttribute(-2147417059)]
			IHTMLDOMNode replaceChild(IHTMLDOMNode newChild, IHTMLDOMNode oldChild);

			[DispIdAttribute(-2147417051)]
			IHTMLDOMNode cloneNode(bool fDeep);

			[DispIdAttribute(-2147417046)]
			IHTMLDOMNode removeNode(bool fDeep);

			[DispIdAttribute(-2147417044)]
			IHTMLDOMNode swapNode(IHTMLDOMNode otherNode);

			[DispIdAttribute(-2147417045)]
			IHTMLDOMNode replaceNode(IHTMLDOMNode replacement);

			[DispIdAttribute(-2147417039)]
			IHTMLDOMNode appendChild(IHTMLDOMNode newChild);
		}
		#endregion
	
		#region HTMLDocumentEvents2
		[ComVisible(true), Guid("3050f613-98b5-11cf-bb82-00aa00bdce0b"), 
			TypeLibType(TypeLibTypeFlags.FDispatchable), 
			InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIDispatch)]
		public interface HTMLDocumentEvents2 {

			[DispId(DISPID_HTMLDOCUMENTEVENTS2_ONACTIVATE)][PreserveSig]
			void OnActivate([MarshalAs(UnmanagedType.Interface)] IHTMLEventObj pEvtObj);

			[DispId(DISPID_HTMLDOCUMENTEVENTS2_ONAFTERUPDATE)][PreserveSig]
			void OnAfterUpdate([MarshalAs(UnmanagedType.Interface)] IHTMLEventObj pEvtObj);

			[DispId(DISPID_HTMLDOCUMENTEVENTS2_ONBEFOREACTIVATE)][PreserveSig]
			[return: MarshalAs(UnmanagedType.Bool)]
			bool OnBeforeActivate([MarshalAs(UnmanagedType.Interface)] IHTMLEventObj pEvtObj);

			[DispId(DISPID_HTMLDOCUMENTEVENTS2_ONBEFOREDEACTIVATE)][PreserveSig]
			[return: MarshalAs(UnmanagedType.Bool)]
			bool OnBeforeDeActivate([MarshalAs(UnmanagedType.Interface)] IHTMLEventObj pEvtObj);

			[DispId(DISPID_HTMLDOCUMENTEVENTS2_ONBEFOREEDITFOCUS)][PreserveSig]
			void OnBeforeEditFocus([MarshalAs(UnmanagedType.Interface)] IHTMLEventObj pEvtObj);

			[DispId(DISPID_HTMLDOCUMENTEVENTS2_ONBEFOREUPDATE)][PreserveSig]
			[return: MarshalAs(UnmanagedType.Bool)]
			bool OnBeforeUpdate([MarshalAs(UnmanagedType.Interface)] IHTMLEventObj pEvtObj);

			[DispId(DISPID_HTMLDOCUMENTEVENTS2_ONCELLCHANGE)][PreserveSig]
			void OnCellChange([MarshalAs(UnmanagedType.Interface)] IHTMLEventObj pEvtObj);

			[DispId(DISPID_HTMLDOCUMENTEVENTS2_ONCLICK)][PreserveSig]
			[return: MarshalAs(UnmanagedType.Bool)]
			bool OnClick([MarshalAs(UnmanagedType.Interface)] IHTMLEventObj pEvtObj);

			[DispId(DISPID_HTMLDOCUMENTEVENTS2_ONCONTEXTMENU)][PreserveSig]
			[return: MarshalAs(UnmanagedType.Bool)]
			bool OnContextMenu([MarshalAs(UnmanagedType.Interface)] IHTMLEventObj pEvtObj);

			[DispId(DISPID_HTMLDOCUMENTEVENTS2_ONCONTROLSELECT)][PreserveSig]
			[return: MarshalAs(UnmanagedType.Bool)]
			bool OnControlSelect([MarshalAs(UnmanagedType.Interface)] IHTMLEventObj pEvtObj);

			[DispId(DISPID_HTMLDOCUMENTEVENTS2_ONDATAAVAILABLE)][PreserveSig]
			void OnDataAvailable([MarshalAs(UnmanagedType.Interface)] IHTMLEventObj pEvtObj);

			[DispId(DISPID_HTMLDOCUMENTEVENTS2_ONDATASETCHANGED)][PreserveSig]
			void OnDatasetChanged([MarshalAs(UnmanagedType.Interface)] IHTMLEventObj pEvtObj);

			[DispId(DISPID_HTMLDOCUMENTEVENTS2_ONDATASETCOMPLETE)][PreserveSig]
			void OnDataSetComplete([MarshalAs(UnmanagedType.Interface)] IHTMLEventObj pEvtObj);

			[DispId(DISPID_HTMLDOCUMENTEVENTS2_ONDEACTIVATE)][PreserveSig]
			void OnDeactivate([MarshalAs(UnmanagedType.Interface)] IHTMLEventObj pEvtObj);

			[DispId(DISPID_HTMLDOCUMENTEVENTS2_ONDBLCLICK)][PreserveSig]
			[return: MarshalAs(UnmanagedType.Bool)]
			bool OnDoubleClick([MarshalAs(UnmanagedType.Interface)] IHTMLEventObj pEvtObj);

			[DispId(DISPID_HTMLDOCUMENTEVENTS2_ONDRAGSTART)][PreserveSig]
			[return: MarshalAs(UnmanagedType.Bool)]
			bool OnDragStart([MarshalAs(UnmanagedType.Interface)] IHTMLEventObj pEvtObj);

			[DispId(DISPID_HTMLDOCUMENTEVENTS2_ONERRORUPDATE)][PreserveSig]
			[return: MarshalAs(UnmanagedType.Bool)]
			bool OnErrorUpdate([MarshalAs(UnmanagedType.Interface)] IHTMLEventObj pEvtObj);

			[DispId(DISPID_HTMLDOCUMENTEVENTS2_ONFOCUSIN)][PreserveSig]
			void OnFocusIn([MarshalAs(UnmanagedType.Interface)] IHTMLEventObj pEvtObj);

			[DispId(DISPID_HTMLDOCUMENTEVENTS2_ONFOCUSOUT)][PreserveSig]
			void OnFocusOut([MarshalAs(UnmanagedType.Interface)] IHTMLEventObj pEvtObj);

			[DispId(DISPID_HTMLDOCUMENTEVENTS2_ONHELP)][PreserveSig]
			[return: MarshalAs(UnmanagedType.Bool)]
			bool OnHelp([MarshalAs(UnmanagedType.Interface)] IHTMLEventObj pEvtObj);

			[DispId(DISPID_HTMLDOCUMENTEVENTS2_ONKEYDOWN)][PreserveSig]
			void OnKeyDown([MarshalAs(UnmanagedType.Interface)] IHTMLEventObj pEvtObj);

			[DispId(DISPID_HTMLDOCUMENTEVENTS2_ONKEYPRESS)][PreserveSig]
			[return: MarshalAs(UnmanagedType.Bool)]
			bool OnKeyPress([MarshalAs(UnmanagedType.Interface)] IHTMLEventObj pEvtObj);

			[DispId(DISPID_HTMLDOCUMENTEVENTS2_ONKEYUP)][PreserveSig]
			void OnKeyUp([MarshalAs(UnmanagedType.Interface)] IHTMLEventObj pEvtObj);

			[DispId(DISPID_HTMLDOCUMENTEVENTS2_ONMOUSEDOWN)][PreserveSig]
			void OnMouseDown([MarshalAs(UnmanagedType.Interface)] IHTMLEventObj pEvtObj);

			[DispId(DISPID_HTMLDOCUMENTEVENTS2_ONMOUSEMOVE)][PreserveSig]
			void OnMouseMove([MarshalAs(UnmanagedType.Interface)] IHTMLEventObj pEvtObj);

			[DispId(DISPID_HTMLDOCUMENTEVENTS2_ONMOUSEOUT)][PreserveSig]
			void OnMouseOut([MarshalAs(UnmanagedType.Interface)] IHTMLEventObj pEvtObj);

			[DispId(DISPID_HTMLDOCUMENTEVENTS2_ONMOUSEOVER)][PreserveSig]
			void OnMouseOver([MarshalAs(UnmanagedType.Interface)] IHTMLEventObj pEvtObj);

			[DispId(DISPID_HTMLDOCUMENTEVENTS2_ONMOUSEUP)][PreserveSig]
			void OnMouseUp([MarshalAs(UnmanagedType.Interface)] IHTMLEventObj pEvtObj);

			[DispId(DISPID_HTMLDOCUMENTEVENTS2_ONMOUSEWHEEL)][PreserveSig]
			[return: MarshalAs(UnmanagedType.Bool)]
			bool OnMouseWheel([MarshalAs(UnmanagedType.Interface)] IHTMLEventObj pEvtObj);

			[DispId(DISPID_HTMLDOCUMENTEVENTS2_ONPROPERTYCHANGE)][PreserveSig]
			void OnPropertyChange([MarshalAs(UnmanagedType.Interface)] IHTMLEventObj pEvtObj);

			[DispId(DISPID_HTMLDOCUMENTEVENTS2_ONREADYSTATECHANGE)][PreserveSig]
			void OnReadyStateChange([MarshalAs(UnmanagedType.Interface)] IHTMLEventObj pEvtObj);

			[DispId(DISPID_HTMLDOCUMENTEVENTS2_ONROWENTER)][PreserveSig]
			void OnRowEnter([MarshalAs(UnmanagedType.Interface)] IHTMLEventObj pEvtObj);

			[DispId(DISPID_HTMLDOCUMENTEVENTS2_ONROWEXIT)][PreserveSig]
			[return: MarshalAs(UnmanagedType.Bool)]
			bool OnRowExit([MarshalAs(UnmanagedType.Interface)] IHTMLEventObj pEvtObj);

			[DispId(DISPID_HTMLDOCUMENTEVENTS2_ONROWSDELETE)][PreserveSig]
			void OnRowsDeleted([MarshalAs(UnmanagedType.Interface)] IHTMLEventObj pEvtObj);

			[DispId(DISPID_HTMLDOCUMENTEVENTS2_ONROWSINSERTED)][PreserveSig]
			void OnRowsInserted([MarshalAs(UnmanagedType.Interface)] IHTMLEventObj pEvtObj);

			[DispId(DISPID_HTMLDOCUMENTEVENTS2_ONSELECTIONCHANGE)][PreserveSig]
			void OnSelectionChange([MarshalAs(UnmanagedType.Interface)] IHTMLEventObj pEvtObj);

			[DispId(DISPID_HTMLDOCUMENTEVENTS2_ONSELECTSTART)][PreserveSig]
			[return: MarshalAs(UnmanagedType.Bool)]
			bool OnSelectStart([MarshalAs(UnmanagedType.Interface)] IHTMLEventObj pEvtObj);

			[DispId(DISPID_HTMLDOCUMENTEVENTS2_ONSTOP)][PreserveSig]
			[return: MarshalAs(UnmanagedType.Bool)]
			bool OnStop([MarshalAs(UnmanagedType.Interface)] IHTMLEventObj pEvtObj);
		}
		#endregion

		#region HTMLElementEvents -- Not implimented correctly
		[ComVisible(true), Guid("3050f60f-98b5-11cf-bb82-00aa00bdce0b"), 
			InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIDispatch)]
		public interface HTMLElementEvents2 {

		}
		#endregion

		#region IDisplayPointer
		[ComVisible(true), Guid("3050f69e-98b5-11cf-bb82-00aa00bdce0b"), 
			InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IDisplayPointer {
			void MoveToPoint([In] NativeMethods.POINT ptPoint, [In] COORD_SYSTEM eCoordSystem,[In] [MarshalAs(UnmanagedType.Interface)] IHTMLElement pElementContext, [In] int dwHitTestOptions, [Out] int pdwHitTestResults);
			void MoveUnit([In] DISPLAY_MOVEUNIT eMoveUnit, [In] int lXPos);
			void PositionMarkupPointer([In] [MarshalAs(UnmanagedType.Interface)] IMarkupPointer pMarkupPointer);
			void MoveToPointer([In] [MarshalAs(UnmanagedType.Interface)] IDisplayPointer pDispPointer);
			void SetPointerGravity([In] POINTER_GRAVITY eGravity);
			void GetPointerGravity([Out] POINTER_GRAVITY peGravity);
			void SetDisplayGravity([In] DISPLAY_GRAVITY eGravity);
			void GetDisplayGravity([Out] DISPLAY_GRAVITY peGravity);
			void IsPositioned([Out] bool pfPositioned);
			void Unposition();
			void IsEqualTo([In] [MarshalAs(UnmanagedType.Interface)] IDisplayPointer pDispPointer, [Out] bool pfIsEqual);
			void IsLeftOf([In] [MarshalAs(UnmanagedType.Interface)] IDisplayPointer pDispPointer, [Out] bool pfIsLeftOf);
			void IsRightOf([In] [MarshalAs(UnmanagedType.Interface)] IDisplayPointer pDispPointer, [Out] bool pfIsRightOf);
			void IsAtBOL([Out] bool pfBOL);
			void MoveToMarkupPointer([In] [MarshalAs(UnmanagedType.Interface)] IMarkupPointer pPointer, [In] [MarshalAs(UnmanagedType.Interface)] IDisplayPointer pDispLineContext);
			void ScrollIntoView();
			//void GetLineInfo([Out] ILineInfo ppLineInfo);
			void GetFlowElement([Out] [MarshalAs(UnmanagedType.Interface)] IHTMLElement ppLayoutElement);
			void QueryBreaks([Out] int pdwBreaks);
		}
		#endregion

		#region IDisplayServices
		[ComVisible(true), Guid("3050f69d-98b5-11cf-bb82-00aa00bdce0b"), 
			InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IDisplayServices {
			[return: MarshalAs(UnmanagedType.I4)][PreserveSig]
			int CreateDisplayPointer([Out] out IDisplayPointer ppDispPointer);

			[return: MarshalAs(UnmanagedType.I4)][PreserveSig]
			int TransformRect([In, Out] NativeMethods.RECT pRect,
				[In] COORD_SYSTEM eSource,
				[In] COORD_SYSTEM eDestination,
				[In] [MarshalAs(UnmanagedType.Interface)] IHTMLElement pIElement);
		
			[return: MarshalAs(UnmanagedType.I4)][PreserveSig]
			int TransformPoint([In, Out] NativeMethods.POINT pPoint,[In] COORD_SYSTEM eSource,
				[In] COORD_SYSTEM eDestination,
				[In] [MarshalAs(UnmanagedType.Interface)] IHTMLElement pIElement);
		
			[return: MarshalAs(UnmanagedType.I4)][PreserveSig]
			int  GetCaret([Out, MarshalAs(UnmanagedType.Interface)] out IHTMLCaret ppCaret);

			[return: MarshalAs(UnmanagedType.I4)][PreserveSig]
			int  GetComputedStyle([In] IMarkupPointer pPointer,
				[Out, MarshalAs(UnmanagedType.Interface)] out IHTMLComputedStyle ppComputedStyle);

			[return: MarshalAs(UnmanagedType.I4)][PreserveSig]
			int  ScrollRectIntoView(
				[In] [MarshalAs(UnmanagedType.Interface)] IHTMLElement pIElement,
				[In] NativeMethods.RECT rect);

			[return: MarshalAs(UnmanagedType.I4)][PreserveSig]
			int HasFlowLayout([In] [MarshalAs(UnmanagedType.Interface)] IHTMLElement pIElement,
				[Out] out bool pfHasFlowLayout);
		}
		#endregion

		#region IElementBehavior
		[ComVisible(true), Guid("3050f425-98b5-11cf-bb82-00aa00bdce0b"), 
			InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IElementBehavior {

			void Init(
				[In, MarshalAs(UnmanagedType.Interface)]
				IElementBehaviorSite pBehaviorSite);

			void Notify(
				[In, MarshalAs(UnmanagedType.U4)]
				int dwEvent,
				[In]
				IntPtr pVar);

			void Detach();
		}
		#endregion

		#region IElementBehaviorFactory
		[ComVisible(true), Guid("3050f429-98b5-11cf-bb82-00aa00bdce0b"), 
		InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IElementBehaviorFactory {

			[return: MarshalAs(UnmanagedType.Interface)]
			IElementBehavior FindBehavior(
				[In, MarshalAs(UnmanagedType.BStr)]
				string bstrBehavior,
				[In, MarshalAs(UnmanagedType.BStr)]
				string bstrBehaviorUrl,
				[In, MarshalAs(UnmanagedType.Interface)]
				IElementBehaviorSite pSite);
		}
		#endregion

		#region IElementBehaviorSite
		[ComVisible(true), Guid("3050f427-98b5-11cf-bb82-00aa00bdce0b"), 
			InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IElementBehaviorSite {
			[return: MarshalAs(UnmanagedType.Interface)]
			IHTMLElement GetElement();

			void RegisterNotification(
				[In, MarshalAs(UnmanagedType.I4)]
				int lEvent);
		}
		#endregion

		#region IElementBehaviorSiteIOM2
		[ComVisible(true), Guid("3050f659-98b5-11cf-bb82-00aa00bdce0b"), 
			InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IElementBehaviorSiteOM2 {

			[return: MarshalAs(UnmanagedType.I4)]
			int RegisterEvent(
				[In, MarshalAs(UnmanagedType.BStr)]
				string pchEvent,
				[In, MarshalAs(UnmanagedType.I4)]
				int lFlags);

			[return: MarshalAs(UnmanagedType.I4)]
			int GetEventCookie(
				[In, MarshalAs(UnmanagedType.BStr)]
				string pchEvent);


			void FireEvent(
				[In, MarshalAs(UnmanagedType.I4)]
				int lCookie,
				[In, MarshalAs(UnmanagedType.Interface)]
				IHTMLEventObj pEventObject);

			[return: MarshalAs(UnmanagedType.Interface)]
			IHTMLEventObj CreateEventObject();


			void RegisterName(
				[In, MarshalAs(UnmanagedType.BStr)]
				string pchName);


			void RegisterUrn(
				[In, MarshalAs(UnmanagedType.BStr)]
				string pchUrn);

			[return: MarshalAs(UnmanagedType.Interface)]
			IHTMLElementDefaults GetDefaults();
		}
		#endregion

		#region IElementNamespace
		[ComVisible(true), Guid("3050f671-98b5-11cf-bb82-00aa00bdce0b"), 
			InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IElementNamespace {
			void AddTag(
				[In, MarshalAs(UnmanagedType.BStr)]
				string tagName,
				[In, MarshalAs(UnmanagedType.I4)]
				int lFlags);
		}
		#endregion

		#region IElementNamespaceFactory
		[ComVisible(true), Guid("3050f672-98b5-11cf-bb82-00aa00bdce0b"), 
			InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IElementNamespaceFactory {

			void Create(
				[In, MarshalAs(UnmanagedType.Interface)]
				IElementNamespace pNamespace);
		}
		#endregion

		#region IElementNamespaceFactoryCallback
		[ComVisible(true), Guid("3050f7fd-98b5-11cf-bb82-00aa00bdce0b"), 
			InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IElementNamespaceFactoryCallback {

			void Resolve(
				[In, MarshalAs(UnmanagedType.BStr)]
				string nameSpace,
				[In, MarshalAs(UnmanagedType.BStr)]
				string tagName,
				[In, MarshalAs(UnmanagedType.BStr)]
				string attributes,
				[In, MarshalAs(UnmanagedType.Interface)]
				IElementNamespace pNamespace);
		}
		#endregion

		#region IElementNamespaceTable
		[ComVisible(true), Guid("3050f670-98b5-11cf-bb82-00aa00bdce0b"), 
			InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IElementNamespaceTable {

			void AddNamespace(
				[In, MarshalAs(UnmanagedType.BStr)]
				string nameSpace,
				[In, MarshalAs(UnmanagedType.BStr)]
				string urn,
				[In, MarshalAs(UnmanagedType.I4)]
				int lFlags,
				[In]
				ref Object factory);
		}
		#endregion

		#region IHTMLBodyElement
		[ComVisible(true), Guid("3050f1d8-98b5-11cf-bb82-00aa00bdce0b"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsDual)]
		public interface IHTMLBodyElement {
			void put_background(
				[In, MarshalAs(UnmanagedType.BStr)]
				string v);

			[return: MarshalAs(UnmanagedType.BStr)]
			string get_background();

			void put_bgProperties(
				[In, MarshalAs(UnmanagedType.BStr)]
				string v);

			[return: MarshalAs(UnmanagedType.BStr)]
			string get_bgProperties();

			void put_leftMargin(
				[In, MarshalAs(UnmanagedType.Interface)]
				object o);

			[return: MarshalAs(UnmanagedType.Interface)]
			object get_leftMargin();

			void put_topMargin(
				[In, MarshalAs(UnmanagedType.Interface)]
				object o);

			[return: MarshalAs(UnmanagedType.Interface)]
			object get_topMargin();

			void put_rightMargin(
				[In, MarshalAs(UnmanagedType.Interface)]
				object o);

			[return: MarshalAs(UnmanagedType.Interface)]
			object get_rightMargin();

			void put_bottomMargin(
				[In, MarshalAs(UnmanagedType.Interface)]
				object o);

			[return: MarshalAs(UnmanagedType.Interface)]
			object get_bottomMargin();

			void put_noWrap(
				[In, MarshalAs(UnmanagedType.Interface)]
				object o);

			[return: MarshalAs(UnmanagedType.Interface)]
			object get_noWrap();

			void put_bgColor(
				[In, MarshalAs(UnmanagedType.Interface)]
				object o);

			[return: MarshalAs(UnmanagedType.Interface)]
			object get_bgColor();

			void put_text(
				[In, MarshalAs(UnmanagedType.Interface)]
				object o);
			[return: MarshalAs(UnmanagedType.Interface)]
			object get_text();

			void put_link(
				[In, MarshalAs(UnmanagedType.Interface)]
				object o);

			[return: MarshalAs(UnmanagedType.Interface)]
			object get_link();

			void put_vLink(
				[In, MarshalAs(UnmanagedType.Interface)]
				object o);

			[return: MarshalAs(UnmanagedType.Interface)]
			object get_vLink();

			void put_aLink(
				[In, MarshalAs(UnmanagedType.Interface)]
				object o);

			[return: MarshalAs(UnmanagedType.Interface)]
			object get_aLink();

			void put_onload(
				[In, MarshalAs(UnmanagedType.Interface)]
				object o);

			[return: MarshalAs(UnmanagedType.Interface)]
			object get_onload();

			void put_onunload(
				[In, MarshalAs(UnmanagedType.Interface)]
				object o);

			[return: MarshalAs(UnmanagedType.Interface)]
			object get_onunload();

			void SetScroll(
				[In, MarshalAs(UnmanagedType.BStr)]
				string s);

			[return: MarshalAs(UnmanagedType.BStr)]
			string get_scroll();

			void put_onselect(
				[In, MarshalAs(UnmanagedType.Interface)]
				object o);

			[return: MarshalAs(UnmanagedType.Interface)]
			object get_onselect();

			void put_onbeforeunload(
				[In, MarshalAs(UnmanagedType.Interface)]
				object o);

			[return: MarshalAs(UnmanagedType.Interface)]
			object get_onbeforeunload();

			[DispId(DISPID_IHTMLBODYELEMENT_CREATETEXTRANGE)]		
			[return: MarshalAs(UnmanagedType.Interface)]
			IHTMLTxtRange CreateTextRange();
		}
		#endregion

		#region IHTMLCaret
		[ComVisible(true), Guid("3050f604-98b5-11cf-bb82-00aa00bdce0b"), 
		InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IHTMLCaret {
			[return: MarshalAs(UnmanagedType.I4)][PreserveSig]
			int MoveCaretToPointer( 
				/* [in] */ [In] [MarshalAs(UnmanagedType.Interface)] IDisplayPointer pDispPointer,
				/* [in] */ [In] bool fScrollIntoView,
				/* [in] */ [In] CARET_DIRECTION eDir);
        
			[return: MarshalAs(UnmanagedType.I4)][PreserveSig]
			int MoveCaretToPointerEx( 
				/* [in] */ [In] [MarshalAs(UnmanagedType.Interface)] IDisplayPointer pDispPointer,
				/* [in] */ [In] bool fVisible,
				/* [in] */ [In] bool fScrollIntoView,
				/* [in] */ [In] CARET_DIRECTION eDir);
        
			[return: MarshalAs(UnmanagedType.I4)][PreserveSig]
			int MoveMarkupPointerToCaret( 
				/* [in] */ [In] [MarshalAs(UnmanagedType.Interface)] IMarkupPointer pIMarkupPointer);
		        
			[return: MarshalAs(UnmanagedType.I4)][PreserveSig]
			int MoveDisplayPointerToCaret( 
				/* [in] */ [In] [MarshalAs(UnmanagedType.Interface)] IDisplayPointer pDispPointer);
        
			[return: MarshalAs(UnmanagedType.I4)][PreserveSig]
			int IsVisible( 
				/* [out] */ [Out] out bool pIsVisible);
        
			[return: MarshalAs(UnmanagedType.I4)][PreserveSig]
			int Show( 
				/* [in] */ [In] bool fScrollIntoView);
        
			[return: MarshalAs(UnmanagedType.I4)][PreserveSig]
			int Hide( );
        
			[return: MarshalAs(UnmanagedType.I4)][PreserveSig]
			int InsertText( 
				/* [in] */ [In] [MarshalAs(UnmanagedType.LPWStr)] String pText,
				/* [in] */ [In] int lLen);
        
			[return: MarshalAs(UnmanagedType.I4)][PreserveSig]
			int ScrollIntoView();
        
			[return: MarshalAs(UnmanagedType.I4)][PreserveSig]
			int GetLocation( 
				/* [out] */ [Out] out NativeMethods.POINT pPoint,
				/* [in] */ [In, MarshalAs(UnmanagedType.Bool)] bool fTranslate);
        
			[return: MarshalAs(UnmanagedType.I4)][PreserveSig]
			int GetCaretDirection( 
				/* [out] */ [Out] out CARET_DIRECTION peDir);
        
			[return: MarshalAs(UnmanagedType.I4)][PreserveSig]
			int SetCaretDirection( 
				/* [in] */ [In] CARET_DIRECTION eDir);
		}
		#endregion

		#region IHTMLComputedStyle
		[ComVisible(true), Guid("3050f6c3-98b5-11cf-bb82-00aa00bdce0b"), 
			InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IHTMLComputedStyle {
			[DispId(DISPID_IHTMLCOMPUTEDSTYLE_BOLD)]
			bool Bold{get;} 
			[DispId(DISPID_IHTMLCOMPUTEDSTYLE_ITALIC)]
			bool Italic{get;} 
			[DispId(DISPID_IHTMLCOMPUTEDSTYLE_UNDERLINE)]
			bool Underline{get;}        
			[DispId(DISPID_IHTMLCOMPUTEDSTYLE_OVERLINE)]
			bool Overline{get;} 
			[DispId(DISPID_IHTMLCOMPUTEDSTYLE_STRIKEOUT)]
			bool Strikeout{get;} 
			[DispId(DISPID_IHTMLCOMPUTEDSTYLE_SUBSCRIPT)]
			bool Subscript{get;} 
			[DispId(DISPID_IHTMLCOMPUTEDSTYLE_SUPERSCRIPT)]
			bool Superscript{get;} 
			[DispId(DISPID_IHTMLCOMPUTEDSTYLE_EXPLICITFACE)]
			bool explicitFace{get;} 
			[DispId(DISPID_IHTMLCOMPUTEDSTYLE_FONTWEIGHT)]
			int FontWeight{get;}        
			[DispId(DISPID_IHTMLCOMPUTEDSTYLE_FONTSIZE)]
			int FontSize{get;}        
			[DispId(DISPID_IHTMLCOMPUTEDSTYLE_FONTNAME)]
			string FontName{get;}        
			[DispId(DISPID_IHTMLCOMPUTEDSTYLE_HASBGCOLOR)]
			bool HasBgColor{get;} 
			[DispId(DISPID_IHTMLCOMPUTEDSTYLE_TEXTCOLOR)]
			int TextColor{get;}        
			[DispId(DISPID_IHTMLCOMPUTEDSTYLE_BACKGROUNDCOLOR)]
			int BackgroundColor{get;}        
			[DispId(DISPID_IHTMLCOMPUTEDSTYLE_PREFORMATTED)]
			bool PreFormatted{get;} 
			[DispId(DISPID_IHTMLCOMPUTEDSTYLE_BLOCKDIRECTION)]
			bool BlockDirection{get;} 
			[DispId(DISPID_IHTMLCOMPUTEDSTYLE_OL)]
			bool OL{get;}        
			void IsEqual([In] [MarshalAs(UnmanagedType.Interface)] IHTMLComputedStyle pComputedStyle,
				[Out] bool pfEqual);
		}
		#endregion

		#region IHTMLControlElement
		[ComVisible(true), Guid("3050f4e9-98b5-11cf-bb82-00aa00bdce0b"), 
			InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLControlElement {
			void SetTabIndex(
				[In, MarshalAs(UnmanagedType.I2)]
				short p);

			[return: MarshalAs(UnmanagedType.I2)]
			short GetTabIndex();


			void Focus();


			void SetAccessKey(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetAccessKey();


			void SetOnblur(
				[In, MarshalAs(UnmanagedType.Struct)]
				object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnblur();


			void SetOnfocus(
				[In, MarshalAs(UnmanagedType.Struct)]
				object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnfocus();


			void SetOnresize(
				[In, MarshalAs(UnmanagedType.Struct)]
				object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOnresize();


			void Blur();

			void AddFilter(
				[In, MarshalAs(UnmanagedType.Interface)]
				object pUnk);

			void RemoveFilter(
				[In, MarshalAs(UnmanagedType.Interface)]
				object pUnk);

			[return: MarshalAs(UnmanagedType.I4)]
			int GetClientHeight();

			[return: MarshalAs(UnmanagedType.I4)]
			int GetClientWidth();

			[return: MarshalAs(UnmanagedType.I4)]
			int GetClientTop();

			[return: MarshalAs(UnmanagedType.I4)]
			int GetClientLeft();
		}
		#endregion

		#region IHTMLControlRange
		[ComVisible(true), Guid("3050f29c-98b5-11cf-bb82-00aa00bdce0b"), 
			InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLControlRange {
			void Select();

			void Add(
				[In, MarshalAs(UnmanagedType.Interface)]
				IHTMLControlElement item);

			void Remove(
				[In, MarshalAs(UnmanagedType.I4)]
				int index);

			[return: MarshalAs(UnmanagedType.Interface)]
			IHTMLElement Item(
				[In, MarshalAs(UnmanagedType.I4)]
				int index);

			void ScrollIntoView(
				[In, MarshalAs(UnmanagedType.Struct)]
				object varargStart);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool QueryCommandSupported(
				[In, MarshalAs(UnmanagedType.BStr)]
				string cmdID);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool QueryCommandEnabled(
				[In, MarshalAs(UnmanagedType.BStr)]
				string cmdID);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool QueryCommandState(
				[In, MarshalAs(UnmanagedType.BStr)]
				string cmdID);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool QueryCommandIndeterm(
				[In, MarshalAs(UnmanagedType.BStr)]
				string cmdID);

			[return: MarshalAs(UnmanagedType.BStr)]
			string QueryCommandText(
				[In, MarshalAs(UnmanagedType.BStr)]
				string cmdID);

			[return: MarshalAs(UnmanagedType.Struct)]
			object QueryCommandValue(
				[In, MarshalAs(UnmanagedType.BStr)]
				string cmdID);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool ExecCommand(
				[In, MarshalAs(UnmanagedType.BStr)]
				string cmdID,
				[In, MarshalAs(UnmanagedType.Bool)]
				bool showUI,
				[In, MarshalAs(UnmanagedType.Struct)]
				object value);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool ExecCommandShowHelp(
				[In, MarshalAs(UnmanagedType.BStr)]
				string cmdID);

			[return: MarshalAs(UnmanagedType.Interface)]
			IHTMLElement CommonParentElement();

			[return: MarshalAs(UnmanagedType.I4)]
			int length();
		}
		#endregion

		#region IHTMLControlRange2
		[ComVisible(true), Guid("3050f65e-98b5-11cf-bb82-00aa00bdce0b"), 
			InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHtmlControlRange2 {
			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int addElement(
				[In, MarshalAs(UnmanagedType.Interface)]
				IHTMLElement element);
		}
		#endregion

		#region IHTMLCurrentStyle
		[ComVisible(true), Guid("3050f3db-98b5-11cf-bb82-00aa00bdce0b"), 
			InterfaceTypeAttribute(ComInterfaceType.InterfaceIsDual)]
		public interface IHTMLCurrentStyle {
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetPosition();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetStyleFloat();

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetColor();

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetBackgroundColor();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetFontFamily();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetFontStyle();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetFontObject();

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetFontWeight();

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetFontSize();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBackgroundImage();

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetBackgroundPositionX();

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetBackgroundPositionY();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBackgroundRepeat();

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetBorderLeftColor();

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetBorderTopColor();

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetBorderRightColor();

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetBorderBottomColor();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBorderTopStyle();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBorderRightStyle();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBorderBottomStyle();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBorderLeftStyle();

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetBorderTopWidth();

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetBorderRightWidth();

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetBorderBottomWidth();

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetBorderLeftWidth();

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetLeft();

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetTop();

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetWidth();

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetHeight();

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetPaddingLeft();

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetPaddingTop();

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetPaddingRight();

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetPaddingBottom();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetTextAlign();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetTextDecoration();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetDisplay();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetVisibility();

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetZIndex();

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetLetterSpacing();

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetLineHeight();

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetTextIndent();

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetVerticalAlign();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBackgroundAttachment();

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetMarginTop();

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetMarginRight();

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetMarginBottom();

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetMarginLeft();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetClear();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetListStyleType();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetListStylePosition();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetListStyleImage();

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetClipTop();

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetClipRight();

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetClipBottom();

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetClipLeft();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetOverflow();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetPageBreakBefore();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetPageBreakAfter();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetCursor();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetTableLayout();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBorderCollapse();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetDirection();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBehavior();

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetAttribute(
				[In, MarshalAs(UnmanagedType.BStr)]
				string strAttributeName,
				[In, MarshalAs(UnmanagedType.I4)]
				int lFlags);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetUnicodeBidi();

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetRight();

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetBottom();
		}
		#endregion

		#region IHTMLDocument
		[ComVisible(true), Guid("626FC520-A41E-11cf-A731-00A0C9082637"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsDual),
		TypeLibType(TypeLibTypeFlags.FDual | TypeLibTypeFlags.FDispatchable)]
		public interface IHTMLDocument {
			[DispId(DISPID_IHTMLDOCUMENT_SCRIPT)]
			object Script{get;}
		}
		#endregion

		#region IHTMLDocument2
		[ComVisible(true), Guid("332c4425-26cb-11d0-b483-00c04fd90119"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIDispatch),
		TypeLibType(TypeLibTypeFlags.FDual | TypeLibTypeFlags.FDispatchable)]
		public interface IHTMLDocument2 {
			[return: MarshalAs(UnmanagedType.Interface)]
			object GetScript();

			[DispId(DISPID_IHTMLDOCUMENT2_ALL)]
			IHTMLElementCollection All {[return: MarshalAs(UnmanagedType.Interface)] get;}

			[DispId(DISPID_IHTMLDOCUMENT2_BODY)]
            IHTMLElement Body{ [return: MarshalAs(UnmanagedType.Interface)] get;}

			[DispId(DISPID_IHTMLDOCUMENT2_ACTIVEELEMENT)]
			IHTMLElement ActiveElement{[return: MarshalAs(UnmanagedType.Interface)] get;}

			[DispId(DISPID_IHTMLDOCUMENT2_ANCHORS)]
			IHTMLElementCollection Achors{[return: MarshalAs(UnmanagedType.Interface)] get;}

			[DispId(DISPID_IHTMLDOCUMENT2_APPLETS)]
			IHTMLElementCollection Applets{[return: MarshalAs(UnmanagedType.Interface)] get;}

			[DispId(DISPID_IHTMLDOCUMENT2_EMBEDS)]
			IHTMLElementCollection Embeds{[return: MarshalAs(UnmanagedType.Interface)] get;}

			[DispId(DISPID_IHTMLDOCUMENT2_FORMS)]
			IHTMLElementCollection Forms{[return: MarshalAs(UnmanagedType.Interface)] get;}

			[DispId(DISPID_IHTMLDOCUMENT2_IMAGES)]
			IHTMLElementCollection Images{[return: MarshalAs(UnmanagedType.Interface)] get;}

			[DispId(DISPID_IHTMLDOCUMENT2_LINKS)]
			IHTMLElementCollection Links{[return: MarshalAs(UnmanagedType.Interface)] get;}

			[DispId(DISPID_IHTMLDOCUMENT2_PLUGINS)]
			IHTMLElementCollection Plugins{[return: MarshalAs(UnmanagedType.Interface)] get;}

			[DispId(DISPID_IHTMLDOCUMENT2_TITLE)]
			string Title{[return: MarshalAs(UnmanagedType.BStr)] get; [param: MarshalAs(UnmanagedType.BStr)] set;}

			[DispId(DISPID_IHTMLDOCUMENT2_DESIGNMODE)]
			string DesignMode{[return: MarshalAs(UnmanagedType.BStr)] get; [param: MarshalAs(UnmanagedType.BStr)] set;}

			[DispId(DISPID_IHTMLDOCUMENT2_SELECTION)]
			IHTMLSelectionObject Selection{[return: MarshalAs(UnmanagedType.Interface)] get;}

			[DispId(DISPID_IHTMLDOCUMENT2_READYSTATE)]
			string ReadyState {get;}

			[DispId(DISPID_IHTMLDOCUMENT2_ALINKCOLOR)]
			object ALinkColor {get; set;}

			[DispId(DISPID_IHTMLDOCUMENT2_BGCOLOR)]
			object BGColor {get; set;}

			[DispId(DISPID_IHTMLDOCUMENT2_FGCOLOR)]
			object ForeColor {get; set;}

			[DispId(DISPID_IHTMLDOCUMENT2_LINKCOLOR)]
			object LinkColor {get; set;}

			[DispId(DISPID_IHTMLDOCUMENT2_VLINKCOLOR)]
			object VLinkColor {get; set;}


			[return: MarshalAs(UnmanagedType.BStr)]
			string GetReferrer();

			[return: MarshalAs(UnmanagedType.Interface)]
			object GetLocation();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetLastModified();

			[DispId(DISPID_IHTMLDOCUMENT2_URL)]
			string URL{[return: MarshalAs(UnmanagedType.BStr)] get; [param: MarshalAs(UnmanagedType.BStr)] set;}

			[DispId(DISPID_IHTMLDOCUMENT2_DOMAIN)]
			string Domain{[return: MarshalAs(UnmanagedType.BStr)] get; [param: MarshalAs(UnmanagedType.BStr)] set;}

			[DispId(DISPID_IHTMLDOCUMENT2_COOKIE)]
			string Cookie{[return: MarshalAs(UnmanagedType.BStr)] get; [param: MarshalAs(UnmanagedType.BStr)] set;}

			[DispId(DISPID_IHTMLDOCUMENT2_EXPANDO)]
			bool Expando{[return: MarshalAs(UnmanagedType.BStr)] get; [param: MarshalAs(UnmanagedType.BStr)] set;}

			[DispId(DISPID_IHTMLDOCUMENT2_CHARSET)]
			string Charset{[return: MarshalAs(UnmanagedType.BStr)] get; [param: MarshalAs(UnmanagedType.BStr)] set;}

			[DispId(DISPID_IHTMLDOCUMENT2_DEFAULTCHARSET)]
			string DefaultCharset{[return: MarshalAs(UnmanagedType.BStr)] get; [param: MarshalAs(UnmanagedType.BStr)] set;}

			[DispId(DISPID_IHTMLDOCUMENT2_MIMETYPE)]
			string MimeType {[return: MarshalAs(UnmanagedType.BStr)] get;}

			[DispId(DISPID_IHTMLDOCUMENT2_FILESIZE)]
			string FileSize {[return: MarshalAs(UnmanagedType.BStr)] get;}

			[DispId(DISPID_IHTMLDOCUMENT2_FILECREATEDDATE)]
			string FileCreatedDate {[return: MarshalAs(UnmanagedType.BStr)] get;}

			[DispId(DISPID_IHTMLDOCUMENT2_FILEMODIFIEDDATE)]
			string FileModifiedDate {[return: MarshalAs(UnmanagedType.BStr)] get;}

			[DispId(DISPID_IHTMLDOCUMENT2_FILEUPDATEDDATE)]
			string FileUpdatedDate {[return: MarshalAs(UnmanagedType.BStr)] get;}

			[DispId(DISPID_IHTMLDOCUMENT2_SECURITY)]
			string Security {[return: MarshalAs(UnmanagedType.BStr)] get;}

			[DispId(DISPID_IHTMLDOCUMENT2_PROTOCOL)]
			string Protocol {[return: MarshalAs(UnmanagedType.BStr)] get;}

			//		[return: MarshalAs(UnmanagedType.BStr)]
			//		string GetNameProp();

			void Write(
				[In, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=System.Runtime.InteropServices.VarEnum.VT_BSTR)]
				string psarray);

			void Writeln(
				[In, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=System.Runtime.InteropServices.VarEnum.VT_BSTR)]
				string psarray);

			[return: MarshalAs(UnmanagedType.Interface)]
			object Open(
				[In, MarshalAs(UnmanagedType.BStr)]
				string URL,
				[In, MarshalAs(UnmanagedType.Struct)]
				Object name,
				[In, MarshalAs(UnmanagedType.Struct)]
				Object features,
				[In, MarshalAs(UnmanagedType.Struct)]
				Object replace);

			void Close();

			void Clear();

			[return: MarshalAs(UnmanagedType.Bool)]
			bool QueryCommandSupported(
				[In, MarshalAs(UnmanagedType.BStr)]
				string cmdID);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool QueryCommandEnabled(
				[In, MarshalAs(UnmanagedType.BStr)]
				string cmdID);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool QueryCommandState(
				[In, MarshalAs(UnmanagedType.BStr)]
				string cmdID);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool QueryCommandIndeterm(
				[In, MarshalAs(UnmanagedType.BStr)]
				string cmdID);

			[return: MarshalAs(UnmanagedType.BStr)]
			string QueryCommandText(
				[In, MarshalAs(UnmanagedType.BStr)]
				string cmdID);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object QueryCommandValue(
				[In, MarshalAs(UnmanagedType.BStr)]
				string cmdID);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool ExecCommand(
				[In, MarshalAs(UnmanagedType.BStr)]
				string cmdID,
				[In, MarshalAs(UnmanagedType.Bool)]
				bool showUI,
				[In, MarshalAs(UnmanagedType.Struct)]
				Object value);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool ExecCommandShowHelp(
				[In, MarshalAs(UnmanagedType.BStr)]
				string cmdID);

			[DispId(DISPID_IHTMLDOCUMENT2_CREATEELEMENT)]
			[return: MarshalAs(UnmanagedType.Interface)]
			IHTMLElement CreateElement(
				[In, MarshalAs(UnmanagedType.BStr)]
				string eTag);

//			void SetOnhelp(
//				[In, MarshalAs(UnmanagedType.Struct)]
//				Object p);
//
//			[return: MarshalAs(UnmanagedType.Struct)]
//			Object GetOnhelp();
//
//
//            void SetOnclick(
//				[In, MarshalAs(UnmanagedType.Struct)]
//				Object p);
//
//			[return: MarshalAs(UnmanagedType.Struct)]
//			Object GetOnclick();
//
//			void SetOndblclick(
//				[In, MarshalAs(UnmanagedType.Struct)]
//				Object p);
//
//			[return: MarshalAs(UnmanagedType.Struct)]
//			Object GetOndblclick();
//
//
//			void SetOnkeyup(
//				[In, MarshalAs(UnmanagedType.Struct)]
//				Object p);
//
//			[return: MarshalAs(UnmanagedType.Struct)]
//			Object GetOnkeyup();
//
//			void SetOnkeydown(
//				[In, MarshalAs(UnmanagedType.Struct)]
//				Object p);
//
//			[return: MarshalAs(UnmanagedType.Struct)]
//			Object GetOnkeydown();
//
//			void SetOnkeypress(
//				[In, MarshalAs(UnmanagedType.Struct)]
//				Object p);
//
//			[return: MarshalAs(UnmanagedType.Struct)]
//			Object GetOnkeypress();
//
//			void SetOnmouseup(
//				[In, MarshalAs(UnmanagedType.Struct)]
//				Object p);
//
//			[return: MarshalAs(UnmanagedType.Struct)]
//			Object GetOnmouseup();
//
//			void SetOnmousedown(
//				[In, MarshalAs(UnmanagedType.Struct)]
//				Object p);
//
//			[return: MarshalAs(UnmanagedType.Struct)]
//			Object GetOnmousedown();
//
//			void SetOnmousemove(
//				[In, MarshalAs(UnmanagedType.Struct)]
//				Object p);
//
//			[return: MarshalAs(UnmanagedType.Struct)]
//			Object GetOnmousemove();
//
//			void SetOnmouseout(
//				[In, MarshalAs(UnmanagedType.Struct)]
//				Object p);
//
//			[return: MarshalAs(UnmanagedType.Struct)]
//			Object GetOnmouseout();
//
//			void SetOnmouseover(
//				[In, MarshalAs(UnmanagedType.Struct)]
//				Object p);
//
//			[return: MarshalAs(UnmanagedType.Struct)]
//			Object GetOnmouseover();
//
//			void SetOnreadystatechange(
//				[In, MarshalAs(UnmanagedType.Struct)]
//				Object p);
//
//			[return: MarshalAs(UnmanagedType.Struct)]
//			Object GetOnreadystatechange();
//
//			void SetOnafterupdate(
//				[In, MarshalAs(UnmanagedType.Struct)]
//				Object p);
//
//			[return: MarshalAs(UnmanagedType.Struct)]
//			Object GetOnafterupdate();
//
//			void SetOnrowexit(
//				[In, MarshalAs(UnmanagedType.Struct)]
//				Object p);
//
//			[return: MarshalAs(UnmanagedType.Struct)]
//			Object GetOnrowexit();
//
//			void SetOnrowenter(
//				[In, MarshalAs(UnmanagedType.Struct)]
//				Object p);
//
//			[return: MarshalAs(UnmanagedType.Struct)]
//			Object GetOnrowenter();
//
//			int SetOndragstart(
//				[In, MarshalAs(UnmanagedType.Struct)]
//				Object p);
//
//			[return: MarshalAs(UnmanagedType.Struct)]
//			Object GetOndragstart();
//
//			void SetOnselectstart(
//				[In, MarshalAs(UnmanagedType.Struct)]
//				Object p);
//
//			[return: MarshalAs(UnmanagedType.Struct)]
//			Object GetOnselectstart();

			[return: MarshalAs(UnmanagedType.Interface)]
			IHTMLElement ElementFromPoint(
				[In, MarshalAs(UnmanagedType.I4)]
				int x,
				[In, MarshalAs(UnmanagedType.I4)]
				int y);

			[return: MarshalAs(UnmanagedType.Interface)]
			IHTMLWindow2 GetParentWindow();

			[return: MarshalAs(UnmanagedType.Interface)]
			IHTMLStyleSheetsCollection GetStyleSheets();

//			void SetOnbeforeupdate(
//				[In, MarshalAs(UnmanagedType.Struct)]
//				Object p);
//
//			[return: MarshalAs(UnmanagedType.Struct)]
//			Object GetOnbeforeupdate();
//
//			void SetOnerrorupdate(
//				[In, MarshalAs(UnmanagedType.Struct)]
//				Object p);
//
//			[return: MarshalAs(UnmanagedType.Struct)]
//			Object GetOnerrorupdate();

			[DispId(DISPID_IHTMLDOCUMENT2_TOSTRING)]
			[return: MarshalAs(UnmanagedType.BStr)]
			string ToString();

			[return: MarshalAs(UnmanagedType.Interface)]
			IHTMLStyleSheet CreateStyleSheet(
				[In, MarshalAs(UnmanagedType.BStr)]
				string bstrHref,
				[In, MarshalAs(UnmanagedType.I4)]
				int lIndex);
		}
		#endregion

		#region IHTMLDocument3
		[ComVisible(true), Guid("3050f485-98b5-11cf-bb82-00aa00bdce0b"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIDispatch),
			TypeLibType(TypeLibTypeFlags.FDual | TypeLibTypeFlags.FDispatchable)]
		public interface IHTMLDocument3 : IHTMLDocument2 {
			[DispId(DISPID_IHTMLDOCUMENT3_DOCUMENTELEMENT)]
			IHTMLElement DocumentElement {get;}

            [DispId(DISPID_IHTMLDOCUMENT3_GETELEMENTBYID)]
            IHTMLElement getElementById(string id);

            [DispId(DISPID_IHTMLDOCUMENT3_GETELEMENTSBYTAGNAME)]
            IHTMLElementCollection getElementsByTagName(string tagName);
		}
		#endregion

		#region IHTMLDocument4
		[ComVisible(true), Guid("3050f69a-98b5-11cf-bb82-00aa00bdce0b"), 
		InterfaceTypeAttribute(ComInterfaceType.InterfaceIsDual),
		TypeLibType(TypeLibTypeFlags.FDual | TypeLibTypeFlags.FDispatchable)]
		internal interface IHTMLDocument4 {
			[DispId(DISPID_IHTMLDOCUMENT4_FOCUS)]
			int focus();

			[DispId(DISPID_IHTMLDOCUMENT4_HASFOCUS)]
			int hasFocus([Out] out bool pfFocus);
        		
			[DispId(DISPID_IHTMLDOCUMENT4_ONSELECTIONCHANGE)]
			object onselectionchange{set; get;}
		
			[DispId(DISPID_IHTMLDOCUMENT4_NAMESPACES)]
			object namespaces{get;}
		
			[DispId(DISPID_IHTMLDOCUMENT4_CREATEDOCUMENTFROMURL)]
			IHTMLDocument2 createDocumentFromUrl([In] String bstrUrl,[In] String bstrOptions);
		
			[DispId(DISPID_IHTMLDOCUMENT4_MEDIA)]
			String media{set; get;}
		
			[DispId(DISPID_IHTMLDOCUMENT4_CREATEEVENTOBJECT)]
			IHTMLEventObj createEventObject();
		
			[DispId(DISPID_IHTMLDOCUMENT4_FIREEVENT)]
			int fireEvent([In] String bstrEventName,[In] object pvarEventObject,[Out] out bool pfCancelled);
		
			[DispId(DISPID_IHTMLDOCUMENT4_CREATERENDERSTYLE)]
			int createRenderStyle([In] String v, [Out, MarshalAs(UnmanagedType.IUnknown)] /*IHTMLRenderStyle*/ object ppIHTMLRenderStyle);
		
			[DispId(DISPID_IHTMLDOCUMENT4_ONCONTROLSELECT)]
			object oncontrolselect{set; get;}
		
			[DispId(DISPID_IHTMLDOCUMENT4_URLUNENCODED)]
			String URLUnencoded{get;}
		}
		#endregion

		#region IHTMLDocument5
		[ComImport,ComVisible(true), Guid("3050f80c-98b5-11cf-bb82-00aa00bdce0b"), 
		InterfaceTypeAttribute(ComInterfaceType.InterfaceIsDual),
		TypeLibType(TypeLibTypeFlags.FDual | TypeLibTypeFlags.FDispatchable)]
		internal interface IHTMLDocument5 {
			[DispId(DISPID_IHTMLDOCUMENT5_ONMOUSEWHEEL)]
			object onmousewheel{set; get;}
		
			[DispId(DISPID_IHTMLDOCUMENT5_DOCTYPE)]
			void getDoctype ([Out] out IHTMLDOMNode p);
		
			[DispId(DISPID_IHTMLDOCUMENT5_IMPLEMENTATION)]
			void getImplementation([Out] out IHTMLDOMImplementation p);
		
			[DispId(DISPID_IHTMLDOCUMENT5_CREATEATTRIBUTE)]
			IHTMLDOMAttribute createAttribute([In] String bstrattrName);
		
			[DispId(DISPID_IHTMLDOCUMENT5_CREATECOMMENT)]
			IHTMLDOMNode createComment([In] String bstrdata );
		
			[DispId(DISPID_IHTMLDOCUMENT5_ONFOCUSIN)]
			object  onfocusin{set; get;}

			[DispId(DISPID_IHTMLDOCUMENT5_ONFOCUSOUT)]
			object onfocusout{set; get;}
		
			[DispId(DISPID_IHTMLDOCUMENT5_ONACTIVATE)]
			object onactivate{set; get;}
		
			[DispId(DISPID_IHTMLDOCUMENT5_ONDEACTIVATE)]
			object ondeactivate{set; get;}
		
			[DispId(DISPID_IHTMLDOCUMENT5_ONBEFOREACTIVATE)]
			object onbeforeactivate{set; get;}
		
			[DispId(DISPID_IHTMLDOCUMENT5_ONBEFOREDEACTIVATE)]
			object onbeforedeactivate{set; get;}
		
			[DispId(DISPID_IHTMLDOCUMENT5_COMPATMODE)]
			String compatMode{get;}
		}
		#endregion

		#region IHTMLDOMAttribute --Not Implimented Yet.
		[ComImport, ComVisible(true), Guid("3050f4b0-98b5-11cf-bb82-00aa00bdce0b"), 
			InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIDispatch),
			TypeLibType(TypeLibTypeFlags.FDispatchable)]
		public interface IHTMLDOMAttribute {
			//		virtual /* [id][propget] */ HtmlApi STDMETHODCALLTYPE nodeName(out string p) 
			//																  /* [out][retval] */ BSTR *p) = 0;
			//        
			//		virtual /* [id][propput] */ HtmlApi STDMETHODCALLTYPE nodeValue( 
			//																  /* [in] */ VARIANT v) = 0;
			//        
			//		virtual /* [id][propget] */ HtmlApi STDMETHODCALLTYPE get_nodeValue( 
			//																  /* [out][retval] */ VARIANT *p) = 0;
			//        
			//		virtual /* [id][propget] */ HtmlApi STDMETHODCALLTYPE get_specified( 
			//																  /* [out][retval] */ VARIANT_BOOL *p) = 0;
		}
		#endregion

		#region IHTMLDOMChildrenCollection -- IEnumerator Implimentation
		[ComImport, Guid("3050f5ab-98b5-11cf-bb82-00aa00bdce0b"),
			InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIDispatch),
			TypeLibType(TypeLibTypeFlags.FDual | TypeLibTypeFlags.FDispatchable)]
		public interface IHTMLDOMChildrenCollection : IEnumerable {
			[DispId(DISPID_IHTMLDOMCHILDRENCOLLECTION_LENGTH)]
			int length {get;}
		
			[DispId(DISPID_IHTMLDOMCHILDRENCOLLECTION__NEWENUM)]
			[return: MarshalAs(UnmanagedType.CustomMarshaler,
						 MarshalTypeRef = typeof(EnumeratorToEnumVariantMarshaler))]
			new IEnumerator GetEnumerator();
		
			[DispId(DISPID_IHTMLDOMCHILDRENCOLLECTION_ITEM)]
			[return: MarshalAs(UnmanagedType.IDispatch)]
			Object item([In] int lIndex); 
		}
		#endregion

		#region IHTMLDOMImplimentation -- Not Implimented Yet.
		[ComImport, ComVisible(true), Guid("3050f80d-98b5-11cf-bb82-00aa00bdce0b"), 
			InterfaceTypeAttribute(ComInterfaceType.InterfaceIsDual),
			TypeLibType(TypeLibTypeFlags.FDual | TypeLibTypeFlags.FDispatchable)]
		public interface IHTMLDOMImplementation {
		
		}
		#endregion

		#region IHTMLDOMNode
		[ComImport, ComVisible(true), Guid("3050f5da-98b5-11cf-bb82-00aa00bdce0b"), 
			InterfaceTypeAttribute(ComInterfaceType.InterfaceIsDual),
			TypeLibType(TypeLibTypeFlags.FDual | TypeLibTypeFlags.FDispatchable)]
		public interface IHTMLDOMNode {
			[DispId(DISPID_IHTMLDOMNODE_NODETYPE)]
			long nodeType{get;}
	
			[DispId(DISPID_IHTMLDOMNODE_PARENTNODE)]
			IHTMLDOMNode parentNode{get;}
		
			[DispId(DISPID_IHTMLDOMNODE_HASCHILDNODES)]
			bool hasChildNodes();
		
			[DispId(DISPID_IHTMLDOMNODE_CHILDNODES)]
			IHTMLDOMChildrenCollection childNodes{[return: MarshalAs(UnmanagedType.Interface)] get;}
		
			[DispId(DISPID_IHTMLDOMNODE_ATTRIBUTES)]
			Object attributes{[return: MarshalAs(UnmanagedType.IDispatch)] get;}
		
			[DispId(DISPID_IHTMLDOMNODE_INSERTBEFORE)]
			IHTMLDOMNode insertBefore([In, MarshalAs(UnmanagedType.Interface)] IHTMLDOMNode newChild);
		
			[DispId(DISPID_IHTMLDOMNODE_REMOVECHILD)]
			IHTMLDOMNode removeChild([In, MarshalAs(UnmanagedType.Interface)] IHTMLDOMNode oldChild);
		
			[DispId(DISPID_IHTMLDOMNODE_REPLACECHILD)]
			IHTMLDOMNode replaceChild([In, MarshalAs(UnmanagedType.Interface)] IHTMLDOMNode newChild,[In, MarshalAs(UnmanagedType.Interface)] IHTMLDOMNode oldChild);
		
			[DispId(DISPID_IHTMLDOMNODE_CLONENODE)]
			IHTMLDOMNode cloneNode([In] bool fDeep);
		
			[DispId(DISPID_IHTMLDOMNODE_REMOVENODE)]
			IHTMLDOMNode removeNode([In] bool fDeep);
		
			[DispId(DISPID_IHTMLDOMNODE_SWAPNODE)]
			IHTMLDOMNode swapNode([In, MarshalAs(UnmanagedType.Interface)] IHTMLDOMNode otherNode);
		
			[DispId(DISPID_IHTMLDOMNODE_REPLACENODE)]
			IHTMLDOMNode replaceNode([In] IHTMLDOMNode replacement);
		
			[DispId(DISPID_IHTMLDOMNODE_APPENDCHILD)]
			IHTMLDOMNode appendChild([In,MarshalAs(UnmanagedType.Interface)] IHTMLDOMNode newChild);
		
			[DispId(DISPID_IHTMLDOMNODE_NODENAME)]
			String nodeName{get;}
		
			[DispId(DISPID_IHTMLDOMNODE_NODEVALUE)]
			Object nodeValue{set; get;}
		
			[DispId(DISPID_IHTMLDOMNODE_FIRSTCHILD)]
			IHTMLDOMNode firstChild{get;}
		
			[DispId(DISPID_IHTMLDOMNODE_LASTCHILD)]
			IHTMLDOMNode lastChild{get;}
		
			[DispId(DISPID_IHTMLDOMNODE_PREVIOUSSIBLING)]
			IHTMLDOMNode previousSibling{get;}
		
			[DispId(DISPID_IHTMLDOMNODE_NEXTSIBLING)]
			IHTMLDOMNode nextSibling{get;}
		}
		#endregion

		#region IHTMLEditDesigner
		[ComVisible(true), ComImport(), Guid("3050f662-98b5-11cf-bb82-00aa00bdce0b"), 
			InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IHTMLEditDesigner {
			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int PreHandleEvent(
				[In] int dispId,
				[In, MarshalAs(UnmanagedType.Interface)]
				IHTMLEventObj eventObj
				);

			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int PostHandleEvent(
				[In] int dispId,
				[In, MarshalAs(UnmanagedType.Interface)]
				IHTMLEventObj eventObj
				);

			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int TranslateAccelerator(
				[In] int dispId,
				[In, MarshalAs(UnmanagedType.Interface)]
				IHTMLEventObj eventObj
				);

			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int PostEditorEventNotify(
				[In] int dispId,
				[In, MarshalAs(UnmanagedType.Interface)]
				IHTMLEventObj eventObj
				);
		}
		#endregion

		#region IHTMLEditHost
		[ComVisible(true), Guid("3050f6a0-98b5-11cf-bb82-00aa00bdce0b"), 
			InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IHTMLEditHost {
			void SnapRect(
				[In, MarshalAs(UnmanagedType.Interface)]
				IHTMLElement pElement,
				[In, Out]
				ref NativeMethods.RECT rcNew,
				[In, MarshalAs(UnmanagedType.I4)]
				ELEMENT_CORNER nHandle);
		}
		#endregion

		#region IHTMLEditServices
		[ComVisible(true), Guid("3050f663-98b5-11cf-bb82-00aa00bdce0b"), 
			InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IHTMLEditServices {

			[return: MarshalAs(UnmanagedType.I4)]
			int AddDesigner(
				[In, MarshalAs(UnmanagedType.Interface)]
				IHTMLEditDesigner designer);

			[return: MarshalAs(UnmanagedType.Interface)]
			object /*ISelectionServices*/ GetSelectionServices(
				[In, MarshalAs(UnmanagedType.Interface)]
				object /*IMarkupContainer*/ markupContainer);

			[return: MarshalAs(UnmanagedType.I4)]
			int MoveToSelectionAnchor (
				[In, MarshalAs(UnmanagedType.Interface)]
				object markupPointer);

			[return: MarshalAs(UnmanagedType.I4)]
			int MoveToSelectionEnd (
				[In, MarshalAs(UnmanagedType.Interface)]
				object markupPointer);

			[return: MarshalAs(UnmanagedType.I4)]
			int RemoveDesigner(
				[In, MarshalAs(UnmanagedType.Interface)]
				IHTMLEditDesigner designer);
		}
		#endregion

        #region IHTMLElement
        [ComVisible(true), Guid("3050f1ff-98b5-11cf-bb82-00aa00bdce0b"), 
            InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIDispatch)]
            public interface IHTMLElement 
        {
            void SetAttribute(
                [In, MarshalAs(UnmanagedType.BStr)]
                string strAttributeName,
                [In, MarshalAs(UnmanagedType.Struct)]
                object AttributeValue,
                [In, MarshalAs(UnmanagedType.I4)]
                int lFlags);

            [return: MarshalAs(UnmanagedType.Struct)]
            object GetAttribute(
                [In, MarshalAs(UnmanagedType.BStr)]
                string strAttributeName,
                [In, MarshalAs(UnmanagedType.I4)]
                int lFlags);

            [return: MarshalAs(UnmanagedType.Bool)]
            bool RemoveAttribute(
                [In, MarshalAs(UnmanagedType.BStr)]
                string strAttributeName,
                [In, MarshalAs(UnmanagedType.I4)]
                int lFlags);

            [DispId(DISPID_IHTMLELEMENT_CLASSNAME)]
            string ClassName {[return: MarshalAs(UnmanagedType.BStr)] get; [param: MarshalAs(UnmanagedType.BStr)] set;}

            [DispId(DISPID_IHTMLELEMENT_ID)]
            string ID {[return: MarshalAs(UnmanagedType.BStr)] get; [param: MarshalAs(UnmanagedType.BStr)] set;}

            [DispId(DISPID_IHTMLELEMENT_TAGNAME)]
            string TagName{[return: MarshalAs(UnmanagedType.BStr)]get;}

            [DispId(DISPID_IHTMLELEMENT_PARENTELEMENT)]
            IHTMLElement ParentElement{[return: MarshalAs(UnmanagedType.Interface)] get;}

            [DispId(DISPID_IHTMLELEMENT_STYLE)]
            IHTMLStyle Style{[return: MarshalAs(UnmanagedType.Interface)] get;}

            //			void SetOnhelp(
            //				[In, MarshalAs(UnmanagedType.Struct)]
            //				Object p);
            //
            //			[return: MarshalAs(UnmanagedType.Struct)]
            //			Object GetOnhelp();
            //
            //
            //			void SetOnclick(
            //				[In, MarshalAs(UnmanagedType.Struct)]
            //				Object p);
            //
            //			[return: MarshalAs(UnmanagedType.Struct)]
            //			Object GetOnclick();
            //
            //
            //			void SetOndblclick(
            //				[In, MarshalAs(UnmanagedType.Struct)]
            //				Object p);
            //
            //			[return: MarshalAs(UnmanagedType.Struct)]
            //			Object GetOndblclick();
            //
            //
            //			void SetOnkeydown(
            //				[In, MarshalAs(UnmanagedType.Struct)]
            //				Object p);
            //
            //			[return: MarshalAs(UnmanagedType.Struct)]
            //			Object GetOnkeydown();
            //
            //
            //			void SetOnkeyup(
            //				[In, MarshalAs(UnmanagedType.Struct)]
            //				Object p);
            //
            //			[return: MarshalAs(UnmanagedType.Struct)]
            //			Object GetOnkeyup();
            //
            //
            //			void SetOnkeypress(
            //				[In, MarshalAs(UnmanagedType.Struct)]
            //				Object p);
            //
            //			[return: MarshalAs(UnmanagedType.Struct)]
            //			Object GetOnkeypress();

            //			void SetOnmouseout(
            //				[In, MarshalAs(UnmanagedType.Struct)]
            //				Object p);
            //
            //			[return: MarshalAs(UnmanagedType.Struct)]
            //			Object GetOnmouseout();

            //			void SetOnmouseover(
            //				[In, MarshalAs(UnmanagedType.Struct)]
            //				Object p);
            //
            //			[return: MarshalAs(UnmanagedType.Struct)]
            //			Object GetOnmouseover();

            //			void SetOnmousemove(
            //				[In, MarshalAs(UnmanagedType.Struct)]
            //				Object p);
            //
            //			[return: MarshalAs(UnmanagedType.Struct)]
            //			Object GetOnmousemove();

            //			void SetOnmousedown(
            //				[In, MarshalAs(UnmanagedType.Struct)]
            //				Object p);
            //
            //			[return: MarshalAs(UnmanagedType.Struct)]
            //			Object GetOnmousedown();

            //			void SetOnmouseup(
            //				[In, MarshalAs(UnmanagedType.Struct)]
            //				Object p);
            //
            //			[return: MarshalAs(UnmanagedType.Struct)]
            //			Object GetOnmouseup();

            [DispId(DISPID_IHTMLELEMENT_DOCUMENT)]
            IHTMLDocument2 Document {[return: MarshalAs(UnmanagedType.Interface)] get;}

            [DispId(DISPID_IHTMLELEMENT_TITLE)]
            string Title {[return: MarshalAs(UnmanagedType.BStr)] get; [param: MarshalAs(UnmanagedType.BStr)] set;}

            [DispId(DISPID_IHTMLELEMENT_LANGUAGE)]
            string Language{[return: MarshalAs(UnmanagedType.BStr)] get; [param: MarshalAs(UnmanagedType.BStr)] set;}


            //			void SetOnselectstart(
            //				[In, MarshalAs(UnmanagedType.Struct)]
            //				Object p);
            //
            //			[return: MarshalAs(UnmanagedType.Struct)]
            //			Object GetOnselectstart();


            void ScrollIntoView(
                [In, MarshalAs(UnmanagedType.Struct)]
                Object varargStart);

            [return: MarshalAs(UnmanagedType.Bool)]
            bool Contains(
                [In, MarshalAs(UnmanagedType.Interface)]
                IHTMLElement pChild);

            [DispId(DISPID_IHTMLELEMENT_SOURCEINDEX)]
            int SourceIndex {[return: MarshalAs(UnmanagedType.I4)]get;}

            [DispId(DISPID_IHTMLELEMENT_RECORDNUMBER)]
            Object RecordNumber {get;}

            [DispId(DISPID_IHTMLELEMENT_LANG)]
            string Lang {[return: MarshalAs(UnmanagedType.BStr)] get; [param: MarshalAs(UnmanagedType.BStr)] set;}

            [DispId(DISPID_IHTMLELEMENT_OFFSETLEFT)]
            int OffsetLeft {[return: MarshalAs(UnmanagedType.I4)]get;}

            [DispId(DISPID_IHTMLELEMENT_OFFSETTOP)]
            int OffsetTop {[return: MarshalAs(UnmanagedType.I4)]get;}

            [DispId(DISPID_IHTMLELEMENT_OFFSETWIDTH)]
            int OffsetWidth {[return: MarshalAs(UnmanagedType.I4)]get;}

            [DispId(DISPID_IHTMLELEMENT_OFFSETHEIGHT)]
            int OffsetHeight {[return: MarshalAs(UnmanagedType.I4)]get;}

            [DispId(DISPID_IHTMLELEMENT_OFFSETPARENT)]
            IHTMLElement OffsetParent {[return: MarshalAs(UnmanagedType.Interface)] get;}

            [DispId(DISPID_IHTMLELEMENT_INNERHTML)]
            string InnerHTML {[return: MarshalAs(UnmanagedType.BStr)] get; [param: MarshalAs(UnmanagedType.BStr)] set;}

            [DispId(DISPID_IHTMLELEMENT_INNERTEXT)]
            string InnerText {[return: MarshalAs(UnmanagedType.BStr)] get; [param: MarshalAs(UnmanagedType.BStr)] set;}

            [DispId(DISPID_IHTMLELEMENT_OUTERHTML)]
            string OuterHTML {[return: MarshalAs(UnmanagedType.BStr)] get; [param: MarshalAs(UnmanagedType.BStr)] set;}

            [DispId(DISPID_IHTMLELEMENT_OUTERTEXT)]
            string OuterText {[return: MarshalAs(UnmanagedType.BStr)] get; [param: MarshalAs(UnmanagedType.BStr)] set;}

            void InsertAdjacentHTML(
                [In, MarshalAs(UnmanagedType.BStr)]
                string where,
                [In, MarshalAs(UnmanagedType.BStr)]
                string html);


            void InsertAdjacentText(
                [In, MarshalAs(UnmanagedType.BStr)]
                string where,
                [In, MarshalAs(UnmanagedType.BStr)]
                string text);

            [DispId(DISPID_IHTMLELEMENT_PARENTTEXTEDIT)]
            IHTMLElement ParentTextEdit {[return: MarshalAs(UnmanagedType.Interface)] get;}

            [DispId(DISPID_IHTMLELEMENT_ISTEXTEDIT)]
            bool IsTextEdit {get;}

            [DispId(DISPID_IHTMLELEMENT_CLICK)]
            void Click();

            [DispId(DISPID_IHTMLELEMENT_FILTERS)]
            object Filters {[return: MarshalAs(UnmanagedType.Interface)] get;} //Should be IHTMLFiltersCollection

            //			void SetOndragstart(
            //				[In, MarshalAs(UnmanagedType.Struct)]
            //				Object p);
            //
            //			[return: MarshalAs(UnmanagedType.Struct)]
            //			Object GetOndragstart();


            [DispId(DISPID_IHTMLELEMENT_TOSTRING)]
            [return: MarshalAs(UnmanagedType.BStr)]
            string ToString();


            //			void SetOnbeforeupdate(
            //				[In, MarshalAs(UnmanagedType.Struct)]
            //				Object p);
            //
            //			[return: MarshalAs(UnmanagedType.Struct)]
            //			Object GetOnbeforeupdate();
            //
            //
            //			void SetOnafterupdate(
            //				[In, MarshalAs(UnmanagedType.Struct)]
            //				Object p);
            //
            //			[return: MarshalAs(UnmanagedType.Struct)]
            //			Object GetOnafterupdate();
            //
            //
            //			void SetOnerrorupdate(
            //				[In, MarshalAs(UnmanagedType.Struct)]
            //				Object p);
            //
            //			[return: MarshalAs(UnmanagedType.Struct)]
            //			Object GetOnerrorupdate();
            //
            //
            //			void SetOnrowexit(
            //				[In, MarshalAs(UnmanagedType.Struct)]
            //				Object p);
            //
            //			[return: MarshalAs(UnmanagedType.Struct)]
            //			Object GetOnrowexit();
            //
            //
            //			void SetOnrowenter(
            //				[In, MarshalAs(UnmanagedType.Struct)]
            //				Object p);
            //
            //			[return: MarshalAs(UnmanagedType.Struct)]
            //			Object GetOnrowenter();
            //
            //
            //			void SetOndatasetchanged(
            //				[In, MarshalAs(UnmanagedType.Struct)]
            //				Object p);
            //
            //			[return: MarshalAs(UnmanagedType.Struct)]
            //			Object GetOndatasetchanged();
            //
            //
            //			void SetOndataavailable(
            //				[In, MarshalAs(UnmanagedType.Struct)]
            //				Object p);
            //
            //			[return: MarshalAs(UnmanagedType.Struct)]
            //			Object GetOndataavailable();
            //
            //
            //			void SetOndatasetcomplete(
            //				[In, MarshalAs(UnmanagedType.Struct)]
            //				Object p);
            //
            //			[return: MarshalAs(UnmanagedType.Struct)]
            //			Object GetOndatasetcomplete();
            //
            //
            //			void SetOnfilterchange(
            //				[In, MarshalAs(UnmanagedType.Struct)]
            //				Object p);
            //
            //			[return: MarshalAs(UnmanagedType.Struct)]
            //			Object GetOnfilterchange();


            [DispId(DISPID_IHTMLELEMENT_ALL)]
            IHTMLElementCollection All{[return: MarshalAs(UnmanagedType.Interface)] get;}

            [DispId(DISPID_IHTMLELEMENT_CHILDREN)]
            IHTMLElementCollection Children{[return: MarshalAs(UnmanagedType.Interface)] get;}
        }
        #endregion

        #region IHTMLElement2
        [Guid("3050F434-98B5-11CF-BB82-00AA00BDCE0B"), InterfaceType(ComInterfaceType.InterfaceIsDual), ComVisible(true)]
            public interface IHTMLElement2
        {
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetScopeName();
            void SetCapture([In, MarshalAs(UnmanagedType.Bool)] bool containerCapture);
            void ReleaseCapture();
            void SetOnlosecapture([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOnlosecapture();
            [return: MarshalAs(UnmanagedType.BStr)]
            string ComponentFromPoint([In, MarshalAs(UnmanagedType.I4)] int x, [In, MarshalAs(UnmanagedType.I4)] int y);
            void DoScroll([In, MarshalAs(UnmanagedType.Struct)] object component);
            void SetOnscroll([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOnscroll();
            void SetOndrag([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOndrag();
            void SetOndragend([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOndragend();
            void SetOndragenter([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOndragenter();
            void SetOndragover([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOndragover();
            void SetOndragleave([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOndragleave();
            void SetOndrop([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOndrop();
            void SetOnbeforecut([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOnbeforecut();
            void SetOncut([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOncut();
            void SetOnbeforecopy([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOnbeforecopy();
            void SetOncopy([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOncopy();
            void SetOnbeforepaste([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOnbeforepaste();
            void SetOnpaste([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOnpaste();
            [return: MarshalAs(UnmanagedType.Interface)]
            IHTMLCurrentStyle GetCurrentStyle();
            void SetOnpropertychange([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOnpropertychange();
            [return: MarshalAs(UnmanagedType.Interface)]
            object GetClientRects();
            [return: MarshalAs(UnmanagedType.Interface)]
            object GetBoundingClientRect();
            void SetExpression([In, MarshalAs(UnmanagedType.BStr)] string propname, [In, MarshalAs(UnmanagedType.BStr)] string expression, [In, MarshalAs(UnmanagedType.BStr)] string language);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetExpression([In, MarshalAs(UnmanagedType.BStr)] object propname);
            [return: MarshalAs(UnmanagedType.Bool)]
            bool RemoveExpression([In, MarshalAs(UnmanagedType.BStr)] string propname);
            void SetTabIndex([In, MarshalAs(UnmanagedType.I2)] short p);
            [return: MarshalAs(UnmanagedType.I2)]
            short GetTabIndex();
            void Focus();
            void SetAccessKey([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetAccessKey();
            void SetOnblur([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOnblur();
            void SetOnfocus([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOnfocus();
            void SetOnresize([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOnresize();
            void Blur();
            void AddFilter([In, MarshalAs(UnmanagedType.Interface)] object pUnk);
            void RemoveFilter([In, MarshalAs(UnmanagedType.Interface)] object pUnk);
            [return: MarshalAs(UnmanagedType.I4)]
            int GetClientHeight();
            [return: MarshalAs(UnmanagedType.I4)]
            int GetClientWidth();
            [return: MarshalAs(UnmanagedType.I4)]
            int GetClientTop();
            [return: MarshalAs(UnmanagedType.I4)]
            int GetClientLeft();
            [return: MarshalAs(UnmanagedType.Bool)]
            bool AttachEvent([In, MarshalAs(UnmanagedType.BStr)] string ev, [In, MarshalAs(UnmanagedType.Interface)] object pdisp);
            void DetachEvent([In, MarshalAs(UnmanagedType.BStr)] string ev, [In, MarshalAs(UnmanagedType.Interface)] object pdisp);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetReadyState();
            void SetOnreadystatechange([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOnreadystatechange();
            void SetOnrowsdelete([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOnrowsdelete();
            void SetOnrowsinserted([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOnrowsinserted();
            void SetOncellchange([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOncellchange();
            void SetDir([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetDir();
            [return: MarshalAs(UnmanagedType.Interface)]
            object CreateControlRange();
            [return: MarshalAs(UnmanagedType.I4)]
            int GetScrollHeight();
            [return: MarshalAs(UnmanagedType.I4)]
            int GetScrollWidth();
            void SetScrollTop([In, MarshalAs(UnmanagedType.I4)] int p);
            [return: MarshalAs(UnmanagedType.I4)]
            int GetScrollTop();
            void SetScrollLeft([In, MarshalAs(UnmanagedType.I4)] int p);
            [return: MarshalAs(UnmanagedType.I4)]
            int GetScrollLeft();
            void ClearAttributes();
            void MergeAttributes([In, MarshalAs(UnmanagedType.Interface)] IHTMLElement mergeThis);
            void SetOncontextmenu([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOncontextmenu();
            [return: MarshalAs(UnmanagedType.Interface)]
            IHTMLElement InsertAdjacentElement([In, MarshalAs(UnmanagedType.BStr)] string where, [In, MarshalAs(UnmanagedType.Interface)] IHTMLElement insertedElement);
            [return: MarshalAs(UnmanagedType.Interface)]
            IHTMLElement ApplyElement([In, MarshalAs(UnmanagedType.Interface)] IHTMLElement apply, [In, MarshalAs(UnmanagedType.BStr)] string where);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetAdjacentText([In, MarshalAs(UnmanagedType.BStr)] string where);
            [return: MarshalAs(UnmanagedType.BStr)]
            string ReplaceAdjacentText([In, MarshalAs(UnmanagedType.BStr)] string where, [In, MarshalAs(UnmanagedType.BStr)] string newText);
            [return: MarshalAs(UnmanagedType.Bool)]
            bool GetCanHaveChildren();
            [return: MarshalAs(UnmanagedType.I4)]
            int AddBehavior([In, MarshalAs(UnmanagedType.BStr)] string bstrUrl, [In] ref object pvarFactory);
            [return: MarshalAs(UnmanagedType.Bool)]
            bool RemoveBehavior([In, MarshalAs(UnmanagedType.I4)] int cookie);
            [return: MarshalAs(UnmanagedType.Interface)]
            IHTMLStyle GetRuntimeStyle();
            [return: MarshalAs(UnmanagedType.Interface)]
            object GetBehaviorUrns();
            void SetTagUrn([In, MarshalAs(UnmanagedType.BStr)] string p);
            [return: MarshalAs(UnmanagedType.BStr)]
            string GetTagUrn();
            void SetOnbeforeeditfocus([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOnbeforeeditfocus();
            [return: MarshalAs(UnmanagedType.I4)]
            int GetReadyStateValue();
            [return: MarshalAs(UnmanagedType.Interface)]
            IHTMLElementCollection GetElementsByTagName([In, MarshalAs(UnmanagedType.BStr)] string v);
            [return: MarshalAs(UnmanagedType.Interface)]
            IHTMLStyle GetBaseStyle();
            [return: MarshalAs(UnmanagedType.Interface)]
            IHTMLCurrentStyle GetBaseCurrentStyle();
            [return: MarshalAs(UnmanagedType.Interface)]
            IHTMLStyle GetBaseRuntimeStyle();
            void SetOnmousehover([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOnmousehover();
            void SetOnkeydownpreview([In, MarshalAs(UnmanagedType.Struct)] object p);
            [return: MarshalAs(UnmanagedType.Struct)]
            object GetOnkeydownpreview();
            [return: MarshalAs(UnmanagedType.Interface)]
            object GetBehavior([In, MarshalAs(UnmanagedType.BStr)] string bstrName, [In, MarshalAs(UnmanagedType.BStr)] string bstrUrn);
        }
 

		#endregion

		#region IHTMLElement3
		[ComVisible(true), Guid("3050f673-98b5-11cf-bb82-00aa00bdce0b"), 
			InterfaceTypeAttribute(ComInterfaceType.InterfaceIsDual)]
		public interface IHTMLElement3 {
			[DispId(DISPID_IHTMLELEMENT3_ISCONTENTEDITABLE)]
			string ContentEditable {get; set;}

			[DispId(DISPID_IHTMLELEMENT3_DISABLED)]
			bool Disabled {get; set;}

			[DispId(DISPID_IHTMLELEMENT3_SETACTIVE)]
			void SetActive();
		}
		#endregion

		#region IHTMLElementCollection
		[ComVisible(true), Guid("3050f21f-98b5-11cf-bb82-00aa00bdce0b"), 
			InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIDispatch),
			TypeLibType(TypeLibTypeFlags.FDual | TypeLibTypeFlags.FDispatchable)]
		public interface IHTMLElementCollection : IEnumerable{
			[DispId(DISPID_IHTMLELEMENTCOLLECTION_TOSTRING)]
			[return: MarshalAs(UnmanagedType.BStr)]
			string ToString();

			[DispId(DISPID_IHTMLELEMENTCOLLECTION_LENGTH)]
			int Length {get;}

			[DispId(DISPID_IHTMLELEMENTCOLLECTION__NEWENUM)]
			[return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(EnumeratorToEnumVariantMarshaler))]
			new IEnumerator GetEnumerator();


			[DispId(DISPID_IHTMLELEMENTCOLLECTION_ITEM)]
			[return: MarshalAs(UnmanagedType.IDispatch)]
			object Item([In] int Index); 

			[DispId(DISPID_IHTMLELEMENTCOLLECTION_ITEM)]
			[return: MarshalAs(UnmanagedType.IDispatch)]
			object Item([In, MarshalAs(UnmanagedType.BStr)] string Name); 

			[return: MarshalAs(UnmanagedType.Interface)]
			object Tags([In, MarshalAs(UnmanagedType.BStr)] string TagName);
		}
		#endregion

		#region IHTMLElementDefaults
		[ComVisible(true), Guid("3050f6c9-98b5-11cf-bb82-00aa00bdce0b"), 
			InterfaceTypeAttribute(ComInterfaceType.InterfaceIsDual)]
		public interface IHTMLElementDefaults {

			[return: MarshalAs(UnmanagedType.Interface)]
			IHTMLStyle GetStyle();

			void SetTabStop(
				[In, MarshalAs(UnmanagedType.Bool)]
				bool v);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool GetTabStop();


			void SetViewInheritStyle(
				[In, MarshalAs(UnmanagedType.Bool)]
				bool v);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool GetViewInheritStyle();


			void SetViewMasterTab(
				[In, MarshalAs(UnmanagedType.Bool)]
				bool v);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool GetViewMasterTab();


			void SetScrollSegmentX(
				[In, MarshalAs(UnmanagedType.I4)]
				int v);

			[return: MarshalAs(UnmanagedType.I4)]
			int GetScrollSegmentX();


			void SetScrollSegmentY(
				[In, MarshalAs(UnmanagedType.I4)]
				object p);

			[return: MarshalAs(UnmanagedType.I4)]
			int GetScrollSegmentY();


			void SetIsMultiLine(
				[In, MarshalAs(UnmanagedType.Bool)]
				bool v);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool GetIsMultiLine();


			void SetContentEditable(
				[In, MarshalAs(UnmanagedType.BStr)]
				string v);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetContentEditable();


			void SetCanHaveHTML(
				[In, MarshalAs(UnmanagedType.Bool)]
				bool v);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool GetCanHaveHTML();


			void SetViewLink(
				[In, MarshalAs(UnmanagedType.Interface)]
				IHTMLDocument viewLink);

			[return: MarshalAs(UnmanagedType.Interface)]
			IHTMLDocument GetViewLink();

			void SetFrozen(
				[In, MarshalAs(UnmanagedType.Bool)]
				bool v);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool GetFrozen();
		}
		#endregion

		#region IHTMLElementRender
		[ComVisible(true), ComImport(),
			Guid("3050f669-98b5-11cf-bb82-00aa00bdce0b"),
			InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
			internal interface IHTMLElementRender {
			[return: MarshalAs(UnmanagedType.I4)][PreserveSig]
			int DrawToDC(IntPtr hDC);
        
			[return: MarshalAs(UnmanagedType.I4)][PreserveSig]
			int SetDocumentPrinter([MarshalAs(UnmanagedType.BStr)] string bstrPrinterName,
				IntPtr hDC);
		}
		#endregion

        #region IHTMLImgElement

        [ComImport, Guid("3050f240-98b5-11cf-bb82-00aa00bdce0b"), InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
        public interface IHTMLImgElement
        {
            object onabort { [DispId(-2147412084)] get; [DispId(-2147412084)] set; }
            string lowsrc { [DispId(0x3ef)] get; [DispId(0x3ef)] set; }
            string useMap { [DispId(0x7d8)] get; [DispId(0x7d8)] set; }
            string vrml { [DispId(0x3f0)] get; [DispId(0x3f0)] set; }
            string alt { [DispId(0x3ea)] get; [DispId(0x3ea)] set; }
            string readyState { [DispId(-2147412996)] get; }
            bool complete { [DispId(0x3f2)] get; }
            string name { [DispId(-2147418112)] get; [DispId(-2147418112)] set; }
            string fileUpdatedDate { [DispId(0x7de)] get; }
            int width { [DispId(-2147418107)] get; [DispId(-2147418107)] set; }
            string start { [DispId(0x3f5)] get; [DispId(0x3f5)] set; }
            string nameProp { [DispId(0x7e1)] get; }
            string src { [DispId(0x3eb)] get; [DispId(0x3eb)] set; }
            string fileModifiedDate { [DispId(0x7dd)] get; }
            object loop { [DispId(0x3f3)] get; [DispId(0x3f3)] set; }
            int vspace { [DispId(0x3ed)] get; [DispId(0x3ed)] set; }
            string fileSize { [DispId(0x7db)] get; }
            string align { [DispId(-2147418039)] get; [DispId(-2147418039)] set; }
            string dynsrc { [DispId(0x3f1)] get; [DispId(0x3f1)] set; }
            string fileCreatedDate { [DispId(0x7dc)] get; }
            object onerror { [DispId(-2147412083)] get; [DispId(-2147412083)] set; }
            bool isMap { [DispId(0x7d2)] get; [DispId(0x7d2)] set; }
            object onload { [DispId(-2147412080)] get; [DispId(-2147412080)] set; }
            int height { [DispId(-2147418106)] get; [DispId(-2147418106)] set; }
            string protocol { [DispId(0x7df)] get; }
            string href { [DispId(0x7e0)] get; }
            string mimeType { [DispId(0x7da)] get; }
            int hspace { [DispId(0x3ee)] get; [DispId(0x3ee)] set; }
            object border { [DispId(0x3ec)] get; [DispId(0x3ec)] set; }
        } 

        #endregion

        #region IHTMLEventObj
        [ComVisible(true), Guid("3050f32d-98b5-11cf-bb82-00aa00bdce0b"), 
			InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLEventObj {

			[DispId(DISPID_IHTMLEVENTOBJ_SRCELEMENT)]
			IHTMLElement SourceElement {[return: MarshalAs(UnmanagedType.Interface)] get;}

			[DispId(DISPID_IHTMLEVENTOBJ_ALTKEY)]
			bool AltKey {[return: MarshalAs(UnmanagedType.Bool)] get;}

			[DispId(DISPID_IHTMLEVENTOBJ_CTRLKEY)]
			bool CtrlKey {[return: MarshalAs(UnmanagedType.Bool)] get;}

			[DispId(DISPID_IHTMLEVENTOBJ_SHIFTKEY)]
			bool ShiftKey {[return: MarshalAs(UnmanagedType.Bool)] get;}

			[DispId(DISPID_IHTMLEVENTOBJ_RETURNVALUE)]
			object ReturnValue {[return: MarshalAs(UnmanagedType.VariantBool)] get; [param: MarshalAs(UnmanagedType.VariantBool)] set;}

			[DispId(DISPID_IHTMLEVENTOBJ_CANCELBUBBLE)]
			bool CancelBubble {[return: MarshalAs(UnmanagedType.VariantBool)] get; [param: MarshalAs(UnmanagedType.VariantBool)] set;}

			[DispId(DISPID_IHTMLEVENTOBJ_FROMELEMENT)]
			IHTMLElement FromElement {[return: MarshalAs(UnmanagedType.Interface)] get;}

			[DispId(DISPID_IHTMLEVENTOBJ_TOELEMENT)]
			IHTMLElement ToElement {[return: MarshalAs(UnmanagedType.Interface)] get;}

			[DispId(DISPID_IHTMLEVENTOBJ_KEYCODE)]
			int KeyCode {[return: MarshalAs(UnmanagedType.I4)] get; [param: MarshalAs(UnmanagedType.I4)] set;}

			[DispId(DISPID_IHTMLEVENTOBJ_BUTTON)]
			int Button {[return: MarshalAs(UnmanagedType.I4)] get;}

			[DispId(DISPID_IHTMLEVENTOBJ_TYPE)]
			string Type {[return: MarshalAs(UnmanagedType.BStr)] get;}

			[DispId(DISPID_IHTMLEVENTOBJ_QUALIFIER)]
			string Qualifier {[return: MarshalAs(UnmanagedType.BStr)] get;}

			[DispId(DISPID_IHTMLEVENTOBJ_REASON)]
			string Reason {[return: MarshalAs(UnmanagedType.I4)] get;}

			[DispId(DISPID_IHTMLEVENTOBJ_X)]
			int X {[return: MarshalAs(UnmanagedType.I4)] get;}

			[DispId(DISPID_IHTMLEVENTOBJ_Y)]
			int Y {[return: MarshalAs(UnmanagedType.I4)] get;}

			[DispId(DISPID_IHTMLEVENTOBJ_CLIENTX)]
			int ClientX {[return: MarshalAs(UnmanagedType.I4)] get;}

			[DispId(DISPID_IHTMLEVENTOBJ_CLIENTY)]
			int ClientY {[return: MarshalAs(UnmanagedType.I4)] get;}

			[DispId(DISPID_IHTMLEVENTOBJ_OFFSETX)]
			int OffSetX {[return: MarshalAs(UnmanagedType.I4)] get;}

			[DispId(DISPID_IHTMLEVENTOBJ_OFFSETY)]
			int OffSetY {[return: MarshalAs(UnmanagedType.I4)] get;}

			[DispId(DISPID_IHTMLEVENTOBJ_SCREENX)]
			int ScreenX {[return: MarshalAs(UnmanagedType.I4)] get;}

			[DispId(DISPID_IHTMLEVENTOBJ_SCREENY)]
			int ScreenY {[return: MarshalAs(UnmanagedType.I4)] get;}


			[DispId(DISPID_IHTMLEVENTOBJ_SRCFILTER)]
			object SRCFilter {[return: MarshalAs(UnmanagedType.IDispatch)] get;}
		}
		#endregion

		#region IHTMLEventObj4
		[ComVisible(true), Guid("3050f814-98b5-11cf-bb82-00aa00bdce0b"), 
			InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLEventObj4 {
			[DispId(DISPID_IHTMLEVENTOBJ4_WHEELDELTA)]
			int WheelDelta {[return: MarshalAs(UnmanagedType.I4)] get;}
		}
		#endregion

		#region IHTMLPaintSite
		[ComVisible(true), Guid("3050f6a7-98b5-11cf-bb82-00aa00bdce0b"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IHTMLPaintSite {
			void InvalidatePainterInfo();

			void InvalidateRect(
				[In]
				IntPtr pRect);
		}
        #endregion

        #region IHTMLPainter
        [NDependIgnore]
        [ComVisible(true), Guid("3050f6a6-98b5-11cf-bb82-00aa00bdce0b"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IHTMLPainter {
			void Draw(
				[In, MarshalAs(UnmanagedType.I4)]
				int leftBounds,
				[In, MarshalAs(UnmanagedType.I4)]
				int topBounds,
				[In, MarshalAs(UnmanagedType.I4)]
				int rightBounds,
				[In, MarshalAs(UnmanagedType.I4)]
				int bottomBounds,
				[In, MarshalAs(UnmanagedType.I4)]
				int leftUpdate,
				[In, MarshalAs(UnmanagedType.I4)]
				int topUpdate,
				[In, MarshalAs(UnmanagedType.I4)]
				int rightUpdate,
				[In, MarshalAs(UnmanagedType.I4)]
				int bottomUpdate,
				[In, MarshalAs(UnmanagedType.U4)]
				int lDrawFlags,
				[In]
				IntPtr hdc,
				[In]
				IntPtr pvDrawObject);


			void OnResize(
				[In, MarshalAs(UnmanagedType.I4)]
				int cx,
				[In, MarshalAs(UnmanagedType.I4)]
				int cy);


			void GetPainterInfo(
				[ Out]
				HTML_PAINTER_INFO htmlPainterInfo);

			void HitTestPoint(
				[In, MarshalAs(UnmanagedType.I4)]
				int ptx,
				[In, MarshalAs(UnmanagedType.I4)]
				int pty,
				out int pbHit,
				out int plPartID);
		}
		#endregion

		#region IHTMLRuleStyle -- Not Used
		[ComVisible(true), Guid("3050f3cf-98b5-11cf-bb82-00aa00bdce0b"), 
			InterfaceTypeAttribute(ComInterfaceType.InterfaceIsDual)]
		public interface IHTMLRuleStyle {


			void SetFontFamily(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetFontFamily();


			void SetFontStyle(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetFontStyle();


			void SetFontObject(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetFontObject();


			void SetFontWeight(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetFontWeight();


			void SetFontSize(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetFontSize();


			void SetFont(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetFont();


			void SetColor(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetColor();


			void SetBackground(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBackground();


			void SetBackgroundColor(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetBackgroundColor();


			void SetBackgroundImage(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBackgroundImage();


			void SetBackgroundRepeat(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBackgroundRepeat();


			void SetBackgroundAttachment(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBackgroundAttachment();


			void SetBackgroundPosition(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBackgroundPosition();


			void SetBackgroundPositionX(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetBackgroundPositionX();


			void SetBackgroundPositionY(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetBackgroundPositionY();


			void SetWordSpacing(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetWordSpacing();


			void SetLetterSpacing(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetLetterSpacing();


			void SetTextDecoration(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetTextDecoration();


			void SetTextDecorationNone(
				[In, MarshalAs(UnmanagedType.Bool)]
				bool p);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool GetTextDecorationNone();


			void SetTextDecorationUnderline(
				[In, MarshalAs(UnmanagedType.Bool)]
				bool p);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool GetTextDecorationUnderline();


			void SetTextDecorationOverline(
				[In, MarshalAs(UnmanagedType.Bool)]
				bool p);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool GetTextDecorationOverline();


			void SetTextDecorationLineThrough(
				[In, MarshalAs(UnmanagedType.Bool)]
				bool p);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool GetTextDecorationLineThrough();


			void SetTextDecorationBlink(
				[In, MarshalAs(UnmanagedType.Bool)]
				bool p);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool GetTextDecorationBlink();


			void SetVerticalAlign(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetVerticalAlign();


			void SetTextTransform(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetTextTransform();


			void SetTextAlign(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetTextAlign();


			void SetTextIndent(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetTextIndent();


			void SetLineHeight(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetLineHeight();


			void SetMarginTop(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetMarginTop();


			void SetMarginRight(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetMarginRight();


			void SetMarginBottom(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetMarginBottom();


			void SetMarginLeft(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetMarginLeft();


			void SetMargin(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetMargin();


			void SetPaddingTop(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetPaddingTop();


			void SetPaddingRight(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetPaddingRight();


			void SetPaddingBottom(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetPaddingBottom();


			void SetPaddingLeft(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetPaddingLeft();


			void SetPadding(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetPadding();


			void SetBorder(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBorder();


			void SetBorderTop(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBorderTop();


			void SetBorderRight(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBorderRight();


			void SetBorderBottom(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBorderBottom();


			void SetBorderLeft(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBorderLeft();


			void SetBorderColor(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBorderColor();


			void SetBorderTopColor(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetBorderTopColor();


			void SetBorderRightColor(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetBorderRightColor();


			void SetBorderBottomColor(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetBorderBottomColor();


			void SetBorderLeftColor(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetBorderLeftColor();


			void SetBorderWidth(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBorderWidth();


			void SetBorderTopWidth(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetBorderTopWidth();


			void SetBorderRightWidth(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetBorderRightWidth();


			void SetBorderBottomWidth(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetBorderBottomWidth();


			void SetBorderLeftWidth(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetBorderLeftWidth();


			void SetBorderStyle(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBorderStyle();


			void SetBorderTopStyle(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBorderTopStyle();


			void SetBorderRightStyle(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBorderRightStyle();


			void SetBorderBottomStyle(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBorderBottomStyle();


			void SetBorderLeftStyle(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBorderLeftStyle();


			void SetWidth(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetWidth();


			void SetHeight(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetHeight();


			void SetStyleFloat(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetStyleFloat();


			void SetClear(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetClear();


			void SetDisplay(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetDisplay();


			void SetVisibility(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetVisibility();


			void SetListStyleType(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetListStyleType();


			void SetListStylePosition(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetListStylePosition();


			void SetListStyleImage(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetListStyleImage();


			void SetListStyle(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetListStyle();


			void SetWhiteSpace(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetWhiteSpace();


			void SetTop(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetTop();


			void SetLeft(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetLeft();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetPosition();


			void SetZIndex(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetZIndex();


			void SetOverflow(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetOverflow();


			void SetPageBreakBefore(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetPageBreakBefore();


			void SetPageBreakAfter(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetPageBreakAfter();


			void SetCssText(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetCssText();


			void SetCursor(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetCursor();


			void SetClip(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetClip();


			void SetFilter(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetFilter();


			void SetAttribute(
				[In, MarshalAs(UnmanagedType.BStr)]
				string strAttributeName,
				[In, MarshalAs(UnmanagedType.Struct)]
				Object AttributeValue,
				[In, MarshalAs(UnmanagedType.I4)]
				int lFlags);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetAttribute(
				[In, MarshalAs(UnmanagedType.BStr)]
				string strAttributeName,
				[In, MarshalAs(UnmanagedType.I4)]
				int lFlags);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool RemoveAttribute(
				[In, MarshalAs(UnmanagedType.BStr)]
				string strAttributeName,
				[In, MarshalAs(UnmanagedType.I4)]
				int lFlags);

		}
		#endregion

		#region IHTMLSelectionObject --Needs Updating
		[ComVisible(true), Guid("3050f25A-98b5-11cf-bb82-00aa00bdce0b"), 
			InterfaceTypeAttribute(ComInterfaceType.InterfaceIsDual)]
		public interface IHTMLSelectionObject {

			[return: MarshalAs(UnmanagedType.Interface)]
			object CreateRange();

			void Empty();


			void Clear();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetSelectionType();
		}
		#endregion

		#region IHTMLStyle -- Needs Updatinging (Not Currently Used)
		[ComVisible(true), Guid("3050f25e-98b5-11cf-bb82-00aa00bdce0b"), 
			InterfaceTypeAttribute(ComInterfaceType.InterfaceIsDual)]
		public interface IHTMLStyle {


			void SetFontFamily(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetFontFamily();


			void SetFontStyle(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetFontStyle();


			void SetFontObject(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetFontObject();


			void SetFontWeight(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetFontWeight();


			void SetFontSize(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetFontSize();


			void SetFont(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetFont();


			void SetColor(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetColor();


			void SetBackground(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBackground();


			void SetBackgroundColor(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetBackgroundColor();


			void SetBackgroundImage(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBackgroundImage();


			void SetBackgroundRepeat(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBackgroundRepeat();


			void SetBackgroundAttachment(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBackgroundAttachment();


			void SetBackgroundPosition(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBackgroundPosition();


			void SetBackgroundPositionX(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetBackgroundPositionX();


			void SetBackgroundPositionY(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetBackgroundPositionY();


			void SetWordSpacing(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetWordSpacing();


			void SetLetterSpacing(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetLetterSpacing();


			void SetTextDecoration(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetTextDecoration();


			void SetTextDecorationNone(
				[In, MarshalAs(UnmanagedType.Bool)]
				bool p);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool GetTextDecorationNone();


			void SetTextDecorationUnderline(
				[In, MarshalAs(UnmanagedType.Bool)]
				bool p);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool GetTextDecorationUnderline();


			void SetTextDecorationOverline(
				[In, MarshalAs(UnmanagedType.Bool)]
				bool p);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool GetTextDecorationOverline();


			void SetTextDecorationLineThrough(
				[In, MarshalAs(UnmanagedType.Bool)]
				bool p);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool GetTextDecorationLineThrough();


			void SetTextDecorationBlink(
				[In, MarshalAs(UnmanagedType.Bool)]
				bool p);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool GetTextDecorationBlink();


			void SetVerticalAlign(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetVerticalAlign();


			void SetTextTransform(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetTextTransform();


			void SetTextAlign(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetTextAlign();


			void SetTextIndent(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetTextIndent();


			void SetLineHeight(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetLineHeight();


			void SetMarginTop(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetMarginTop();


			void SetMarginRight(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetMarginRight();


			void SetMarginBottom(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetMarginBottom();


			void SetMarginLeft(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetMarginLeft();


			void SetMargin(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetMargin();


			void SetPaddingTop(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetPaddingTop();


			void SetPaddingRight(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetPaddingRight();


			void SetPaddingBottom(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetPaddingBottom();


			void SetPaddingLeft(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetPaddingLeft();


			void SetPadding(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetPadding();


			void SetBorder(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBorder();


			void SetBorderTop(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBorderTop();


			void SetBorderRight(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBorderRight();


			void SetBorderBottom(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBorderBottom();


			void SetBorderLeft(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBorderLeft();


			void SetBorderColor(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBorderColor();


			void SetBorderTopColor(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetBorderTopColor();


			void SetBorderRightColor(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetBorderRightColor();


			void SetBorderBottomColor(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetBorderBottomColor();


			void SetBorderLeftColor(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetBorderLeftColor();


			void SetBorderWidth(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBorderWidth();


			void SetBorderTopWidth(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetBorderTopWidth();


			void SetBorderRightWidth(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetBorderRightWidth();


			void SetBorderBottomWidth(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetBorderBottomWidth();


			void SetBorderLeftWidth(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetBorderLeftWidth();


			void SetBorderStyle(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBorderStyle();


			void SetBorderTopStyle(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBorderTopStyle();


			void SetBorderRightStyle(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBorderRightStyle();


			void SetBorderBottomStyle(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBorderBottomStyle();


			void SetBorderLeftStyle(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBorderLeftStyle();


			void SetWidth(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetWidth();


			void SetHeight(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetHeight();


			void SetStyleFloat(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetStyleFloat();


			void SetClear(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetClear();


			void SetDisplay(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetDisplay();


			void SetVisibility(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetVisibility();


			void SetListStyleType(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetListStyleType();


			void SetListStylePosition(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetListStylePosition();


			void SetListStyleImage(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetListStyleImage();


			void SetListStyle(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetListStyle();


			void SetWhiteSpace(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetWhiteSpace();


			void SetTop(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetTop();


			void SetLeft(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetLeft();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetPosition();


			void SetZIndex(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetZIndex();


			void SetOverflow(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetOverflow();


			void SetPageBreakBefore(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetPageBreakBefore();


			void SetPageBreakAfter(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetPageBreakAfter();


			void SetCssText(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetCssText();


			void SetPixelTop(
				[In, MarshalAs(UnmanagedType.I4)]
				int p);

			[return: MarshalAs(UnmanagedType.I4)]
			int GetPixelTop();


			void SetPixelLeft(
				[In, MarshalAs(UnmanagedType.I4)]
				int p);

			[return: MarshalAs(UnmanagedType.I4)]
			int GetPixelLeft();


			void SetPixelWidth(
				[In, MarshalAs(UnmanagedType.I4)]
				int p);

			[return: MarshalAs(UnmanagedType.I4)]
			int GetPixelWidth();


			void SetPixelHeight(
				[In, MarshalAs(UnmanagedType.I4)]
				int p);

			[return: MarshalAs(UnmanagedType.I4)]
			int GetPixelHeight();


			void SetPosTop(
				[In, MarshalAs(UnmanagedType.R4)]
				float p);

			[return: MarshalAs(UnmanagedType.R4)]
			float GetPosTop();


			void SetPosLeft(
				[In, MarshalAs(UnmanagedType.R4)]
				float p);

			[return: MarshalAs(UnmanagedType.R4)]
			float GetPosLeft();


			void SetPosWidth(
				[In, MarshalAs(UnmanagedType.R4)]
				float p);

			[return: MarshalAs(UnmanagedType.R4)]
			float GetPosWidth();


			void SetPosHeight(
				[In, MarshalAs(UnmanagedType.R4)]
				float p);

			[return: MarshalAs(UnmanagedType.R4)]
			float GetPosHeight();


			void SetCursor(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetCursor();


			void SetClip(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetClip();


			void SetFilter(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetFilter();


			void SetAttribute(
				[In, MarshalAs(UnmanagedType.BStr)]
				string strAttributeName,
				[In, MarshalAs(UnmanagedType.Struct)]
				Object AttributeValue,
				[In, MarshalAs(UnmanagedType.I4)]
				int lFlags);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetAttribute(
				[In, MarshalAs(UnmanagedType.BStr)]
				string strAttributeName,
				[In, MarshalAs(UnmanagedType.I4)]
				int lFlags);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool RemoveAttribute(
				[In, MarshalAs(UnmanagedType.BStr)]
				string strAttributeName,
				[In, MarshalAs(UnmanagedType.I4)]
				int lFlags);
		}
		#endregion

        [InterfaceType(ComInterfaceType.InterfaceIsDual), Guid("3050F656-98B5-11CF-BB82-00AA00BDCE0B")]
        public interface IHTMLStyle3
        {
            [DispId(-2147412957)]
            string layoutFlow { [return: MarshalAs(UnmanagedType.BStr)] [DispId(-2147412957)] get; [param: In, MarshalAs(UnmanagedType.BStr)] [DispId(-2147412957)] set; }

            [DispId(-2147412959)]
            void SetZoom([In] object zoom);

            [DispId(-2147412959)]
            object GetZoom();

            [DispId(-2147412954)]
            string wordWrap { [return: MarshalAs(UnmanagedType.BStr)] [DispId(-2147412954)] get; [param: In, MarshalAs(UnmanagedType.BStr)] [DispId(-2147412954)] set; }
            [DispId(-2147412953)]
            string textUnderlinePosition { [return: MarshalAs(UnmanagedType.BStr)] [DispId(-2147412953)] get; [param: In, MarshalAs(UnmanagedType.BStr)] [DispId(-2147412953)] set; }
            [DispId(-2147412932)]
            object scrollbarBaseColor { [return: MarshalAs(UnmanagedType.Struct)] [DispId(-2147412932)] get; [param: In, MarshalAs(UnmanagedType.Struct)] [DispId(-2147412932)] set; }
            [DispId(-2147412931)]
            object scrollbarFaceColor { [return: MarshalAs(UnmanagedType.Struct)] [DispId(-2147412931)] get; [param: In, MarshalAs(UnmanagedType.Struct)] [DispId(-2147412931)] set; }
            [DispId(-2147412930)]
            object scrollbar3dLightColor { [return: MarshalAs(UnmanagedType.Struct)] [DispId(-2147412930)] get; [param: In, MarshalAs(UnmanagedType.Struct)] [DispId(-2147412930)] set; }
            [DispId(-2147412929)]
            object scrollbarShadowColor { [return: MarshalAs(UnmanagedType.Struct)] [DispId(-2147412929)] get; [param: In, MarshalAs(UnmanagedType.Struct)] [DispId(-2147412929)] set; }
            [DispId(-2147412928)]
            object scrollbarHighlightColor { [return: MarshalAs(UnmanagedType.Struct)] [DispId(-2147412928)] get; [param: In, MarshalAs(UnmanagedType.Struct)] [DispId(-2147412928)] set; }
            [DispId(-2147412927)]
            object scrollbarDarkShadowColor { [return: MarshalAs(UnmanagedType.Struct)] [DispId(-2147412927)] get; [param: In, MarshalAs(UnmanagedType.Struct)] [DispId(-2147412927)] set; }
            [DispId(-2147412926)]
            object scrollbarArrowColor { [return: MarshalAs(UnmanagedType.Struct)] [DispId(-2147412926)] get; [param: In, MarshalAs(UnmanagedType.Struct)] [DispId(-2147412926)] set; }
            [DispId(-2147412916)]
            object scrollbarTrackColor { [return: MarshalAs(UnmanagedType.Struct)] [DispId(-2147412916)] get; [param: In, MarshalAs(UnmanagedType.Struct)] [DispId(-2147412916)] set; }
            [DispId(-2147412920)]
            string writingMode { [return: MarshalAs(UnmanagedType.BStr)] [DispId(-2147412920)] get; [param: In, MarshalAs(UnmanagedType.BStr)] [DispId(-2147412920)] set; }
            [DispId(-2147412909)]
            string textAlignLast { [return: MarshalAs(UnmanagedType.BStr)] [DispId(-2147412909)] get; [param: In, MarshalAs(UnmanagedType.BStr)] [DispId(-2147412909)] set; }
            [DispId(-2147412908)]
            object textKashidaSpace { [return: MarshalAs(UnmanagedType.Struct)] [DispId(-2147412908)] get; [param: In, MarshalAs(UnmanagedType.Struct)] [DispId(-2147412908)] set; }
        }

		#region IHTMLStyleSheet -- Needs Updating
		[ComVisible(true), Guid("3050f2e3-98b5-11cf-bb82-00aa00bdce0b"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsDual)]
		public interface IHTMLStyleSheet {
			void SetTitle(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetTitle();

			[return: MarshalAs(UnmanagedType.Interface)]
			IHTMLStyleSheet GetParentStyleSheet();

			[return: MarshalAs(UnmanagedType.Interface)]
			IHTMLElement GetOwningElement();


			void SetDisabled(
				[In, MarshalAs(UnmanagedType.Bool)]
				bool p);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool GetDisabled();

			[return: MarshalAs(UnmanagedType.Bool)]
			bool GetReadOnly();

			[return: MarshalAs(UnmanagedType.Interface)]
			IHTMLStyleSheetsCollection GetImports();


			void SetHref(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetHref();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetStyleSheetType();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetId();

			[return: MarshalAs(UnmanagedType.I4)]
			int AddImport(
				[In, MarshalAs(UnmanagedType.BStr)]
				string bstrURL,
				[In, MarshalAs(UnmanagedType.I4)]
				int lIndex);

			[return: MarshalAs(UnmanagedType.I4)]
			int AddRule(
				[In, MarshalAs(UnmanagedType.BStr)]
				string bstrSelector,
				[In, MarshalAs(UnmanagedType.BStr)]
				string bstrStyle,
				[In, MarshalAs(UnmanagedType.I4)]
				int lIndex);


			void RemoveImport(
				[In, MarshalAs(UnmanagedType.I4)]
				int lIndex);


			void RemoveRule(
				[In, MarshalAs(UnmanagedType.I4)]
				int lIndex);


			void SetMedia(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetMedia();


			void SetCssText(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetCssText();

			[return: MarshalAs(UnmanagedType.Interface)]
			IHTMLStyleSheetRulesCollection GetRules();

		}
		#endregion

		#region IHTMLStyleSheetRule -- Needs Updating
		[ComVisible(true), Guid("3050f357-98b5-11cf-bb82-00aa00bdce0b"), 
			InterfaceTypeAttribute(ComInterfaceType.InterfaceIsDual)]
		public interface IHTMLStyleSheetRule {
			void SetSelectorText(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetSelectorText();

			[return: MarshalAs(UnmanagedType.Interface)]
			IHTMLRuleStyle GetStyle();

			[return: MarshalAs(UnmanagedType.Bool)]
			bool GetReadOnly();

		}
		#endregion

		#region IHTMLStyleSheetRulesCollection -- Needs UPdating with Enumerator
		[ComVisible(true), Guid("3050f2e5-98b5-11cf-bb82-00aa00bdce0b"), 
			InterfaceTypeAttribute(ComInterfaceType.InterfaceIsDual)]
		public interface IHTMLStyleSheetRulesCollection {

			[return: MarshalAs(UnmanagedType.I4)]
			int GetLength();

			[return: MarshalAs(UnmanagedType.Interface)]
			IHTMLStyleSheetRule Item(
				[In, MarshalAs(UnmanagedType.I4)]
				int index);

		}
		#endregion

		#region IHTMLStyleSheetsCollection -- Needs updating with Enumerator
		[ComVisible(true), Guid("3050f37e-98b5-11cf-bb82-00aa00bdce0b"), 
			InterfaceTypeAttribute(ComInterfaceType.InterfaceIsDual)]
		public interface IHTMLStyleSheetsCollection {

			[return: MarshalAs(UnmanagedType.I4)]
			int GetLength();

			[return: MarshalAs(UnmanagedType.Interface)]
			object Get_newEnum();

			[return: MarshalAs(UnmanagedType.Struct)]
			Object Item(
				[In]
				ref Object pvarIndex);

		}
		#endregion

		#region IHTMLTable
		[ComVisible(true), Guid("3050f21e-98b5-11cf-bb82-00aa00bdce0b"), 
			InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIDispatch),
			TypeLibType(TypeLibTypeFlags.FDual | TypeLibTypeFlags.FDispatchable)]
		public interface IHTMLTable {
			[DispId(DISPID_IHTMLTABLE_COLS)]
			int Cols{get; set;}        
			[DispId(DISPID_IHTMLTABLE_BORDER)]
			object Border{get; set;} 
			[DispId(DISPID_IHTMLTABLE_FRAME)]
			string Frame {[return: MarshalAs(UnmanagedType.BStr)] get; [param: MarshalAs(UnmanagedType.BStr)] set;}
			[DispId(DISPID_IHTMLTABLE_RULES)]
			string Rules {[return: MarshalAs(UnmanagedType.BStr)] get; [param: MarshalAs(UnmanagedType.BStr)] set;}
			[DispId(DISPID_IHTMLTABLE_CELLSPACING)]
			object CellSpacing{get; set;}
			[DispId(DISPID_IHTMLTABLE_CELLPADDING)]
			object CellPadding{get; set;}
			[DispId(DISPID_IHTMLTABLE_BACKGROUND)]
			string Background {[return: MarshalAs(UnmanagedType.BStr)] get; [param: MarshalAs(UnmanagedType.BStr)] set;}
			[DispId(DISPID_IHTMLTABLE_BGCOLOR)]
			object BGColor{get; set;}
			[DispId(DISPID_IHTMLTABLE_BORDERCOLOR)]
			object BorderColor{get; set;}
			[DispId(DISPID_IHTMLTABLE_BORDERCOLORLIGHT)]
			object BorderColorLight{get; set;}
			[DispId(DISPID_IHTMLTABLE_BORDERCOLORDARK)]
			object BorderColorDark{get; set;}
			[DispId(DISPID_IHTMLTABLE_ALIGN)]
			string Align {[return: MarshalAs(UnmanagedType.BStr)] get; [param: MarshalAs(UnmanagedType.BStr)] set;}
       
			[DispId(DISPID_IHTMLTABLE_CREATECAPTION)]
			object /* IHTMLTableCaption */ CreateCaption();

			[DispId(DISPID_IHTMLTABLE_CREATETHEAD)]
			object createTHead();

			[DispId(DISPID_IHTMLTABLE_CREATETFOOT)]
			object createTFoot();

			[DispId(DISPID_IHTMLTABLE_DELETETHEAD)]
			void deleteTHead();
			[DispId(DISPID_IHTMLTABLE_DELETETFOOT)]
			void deleteTFoot();
			void deleteCaption();
			[DispId(DISPID_IHTMLTABLE_DELETEROW)]
			void deleteRow([In, MarshalAs(UnmanagedType.I4)] int index);
		
			[DispId(DISPID_IHTMLTABLE_INSERTROW)]
			[return: MarshalAs(UnmanagedType.IDispatch)]
			IHTMLElement InsertRow([In, MarshalAs(UnmanagedType.I4)] int index);

			[DispId(DISPID_IHTMLTABLE_NEXTPAGE)]
			void nextPage();
			[DispId(DISPID_IHTMLTABLE_PREVIOUSPAGE)]
			void previousPage();
			[DispId(DISPID_IHTMLTABLE_REFRESH)]
			void refresh();
        
			[DispId(DISPID_IHTMLTABLE_ROWS)]
			IHTMLElementCollection rows{get;}
			[DispId(DISPID_IHTMLTABLE_WIDTH)]
			object Width{get; set;}
			[DispId(DISPID_IHTMLTABLE_HEIGHT)]
			object Height{get; set;}
			[DispId(DISPID_IHTMLTABLE_DATAPAGESIZE)]
			int DataPageSize{get; set;}        
			[DispId(DISPID_IHTMLTABLE_THEAD)]
			object /*IHTMLTableSection*/ tHead{get;}
			[DispId(DISPID_IHTMLTABLE_TFOOT)]
			object /*IHTMLTableSection*/ tFoot{get;}
			[DispId(DISPID_IHTMLTABLE_TBODIES)]
			object /*IHTMLElementCollection*/ tBodies{get;}
			//		[MarshalAs(UnmanagedType.Interface)]			
			//		object /*IHTMLTableCaption*/ tBodies{get;}
			[DispId(DISPID_IHTMLTABLE_READYSTATE)]
			string ReadyState {[return: MarshalAs(UnmanagedType.BStr)] get; [param: MarshalAs(UnmanagedType.BStr)] set;}
        
			[DispId(DISPID_IHTMLTABLE_ONREADYSTATECHANGE)]
			void Setonreadystatechange(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object v);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object Getonreadystatechange();
		}
		#endregion

		#region IHTMLTableRow
		[ComVisible(true), Guid("3050f23c-98b5-11cf-bb82-00aa00bdce0b"), 
			InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLTableRow {
			[DispId(DISPID_IHTMLTABLEROW_ALIGN)]
			string Align {[return: MarshalAs(UnmanagedType.BStr)] get; [param: MarshalAs(UnmanagedType.BStr)] set;}

			[DispId(DISPID_IHTMLTABLEROW_VALIGN)]
			string vAlign {[return: MarshalAs(UnmanagedType.BStr)] get; [param: MarshalAs(UnmanagedType.BStr)] set;}

			[DispId(DISPID_IHTMLTABLEROW_BGCOLOR)]
			object BGColor{get; set;}        
			
			[DispId(DISPID_IHTMLTABLEROW_BORDERCOLOR)]
			object BorderColor{get; set;}        
			
			[DispId(DISPID_IHTMLTABLEROW_BORDERCOLORLIGHT)]
			object BorderColorLight{get; set;}        
			
			[DispId(DISPID_IHTMLTABLEROW_BORDERCOLORDARK)]
			object BorderColorDark{get; set;}        
			
			[DispId(DISPID_IHTMLTABLEROW_ROWINDEX)]
			int RowIndex{[return: MarshalAs(UnmanagedType.I4)] get;}        

			[DispId(DISPID_IHTMLTABLEROW_SECTIONROWINDEX)]
			int SelectionRowIndex{[return: MarshalAs(UnmanagedType.I4)] get;}        

			[DispId(DISPID_IHTMLTABLEROW_CELLS)]
			IHTMLElementCollection Cells{[return: MarshalAs(UnmanagedType.Interface)] get;}        

			[DispId(DISPID_IHTMLTABLEROW_INSERTCELL)]
			[return: MarshalAs(UnmanagedType.IDispatch)]
			object InsertCell([In, MarshalAs(UnmanagedType.I4)] int index);

			[DispId(DISPID_IHTMLTABLEROW_DELETECELL)]
			void DeleteCell([In, MarshalAs(UnmanagedType.I4)] int index);      
		}
		#endregion

        #region IHTMLTableCell
        [ComImport, Guid("3050f23d-98b5-11cf-bb82-00aa00bdce0b"), InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
            public interface IHTMLTableCell
        {
            [DispId(DISPID_IHTMLTABLECELL_VALIGN)]
            string vAlign {  get;  set; }

            [DispId(DISPID_IHTMLTABLECELL_BACKGROUND)]
            string background { get; set; }

            [DispId(DISPID_IHTMLTABLECELL_COLSPAN)]
            int colSpan { get;  set; }

            [DispId(DISPID_IHTMLTABLECELL_CELLINDEX)]
            int cellIndex {  get; }

            [DispId(DISPID_IHTMLTABLECELL_ROWSPAN)]
            int rowSpan {  get;  set; }

            [DispId(DISPID_IHTMLTABLECELL_NOWRAP)]
            bool noWrap {  get;  set; }

            [DispId(DISPID_IHTMLTABLECELL_BGCOLOR)]
            object bgColor {  get;  set; }

            [DispId(DISPID_IHTMLTABLECELL_BORDERCOLOR)]
            object borderColor {  get;  set; }

            [DispId(DISPID_IHTMLTABLECELL_HEIGHT)]
            object height {  get;  set; }

            [DispId(DISPID_IHTMLTABLECELL_BORDERCOLORLIGHT)]
            object borderColorLight {  get;  set; }

            [DispId(DISPID_IHTMLTABLECELL_WIDTH)]
            object width {  get;  set; }

            [DispId(DISPID_IHTMLTABLECELL_ALIGN)]
            string align {  get; set; }

            [DispId(DISPID_IHTMLTABLECELL_BORDERCOLORDARK)]
            object borderColorDark {  get;  set; }
        }
        #endregion

		#region IHTMLTextContainer --Needs Updating
		[ComVisible(true), Guid("3050f230-98b5-11cf-bb82-00aa00bdce0b"), 
			InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLTextContainer {
			[return: MarshalAs(UnmanagedType.IDispatch)]
			object createControlRange();
			int get_ScrollHeight();
			int get_ScrollWidth();
			int get_ScrollTop();
			int get_ScrollLeft();
			void put_ScrollHeight(int i);
			void put_ScrollWidth(int i);
			void put_ScrollTop(int i);
			void put_ScrollLeft(int i);
		}
		#endregion

		#region IHTMLTxtRange
		[ComVisible(true), Guid("3050f220-98b5-11cf-bb82-00aa00bdce0b"), 
			InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLTxtRange {
			[DispId(DISPID_IHTMLTXTRANGE_HTMLTEXT)]
			string HTMLText {get;}

			[DispId(DISPID_IHTMLTXTRANGE_TEXT)]
			string Text {get; set;}

			[return: MarshalAs(UnmanagedType.Interface)]
			IHTMLElement ParentElement();

			[return: MarshalAs(UnmanagedType.Interface)]
			IHTMLTxtRange Duplicate();

			[return: MarshalAs(UnmanagedType.Bool)]
			bool InRange(
				[In, MarshalAs(UnmanagedType.Interface)]
				IHTMLTxtRange range);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool IsEqual(
				[In, MarshalAs(UnmanagedType.Interface)]
				IHTMLTxtRange range);


			void ScrollIntoView(
				[In, MarshalAs(UnmanagedType.Bool)]
				bool fStart);


			void Collapse(
				[In, MarshalAs(UnmanagedType.Bool)]
				bool Start);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool Expand(
				[In, MarshalAs(UnmanagedType.BStr)]
				string Unit);

			[return: MarshalAs(UnmanagedType.I4)]
			int Move(
				[In, MarshalAs(UnmanagedType.BStr)]
				string Unit,
				[In, MarshalAs(UnmanagedType.I4)]
				int Count);

			[return: MarshalAs(UnmanagedType.I4)]
			int MoveStart(
				[In, MarshalAs(UnmanagedType.BStr)]
				string Unit,
				[In, MarshalAs(UnmanagedType.I4)]
				int Count);

			[return: MarshalAs(UnmanagedType.I4)]
			int MoveEnd(
				[In, MarshalAs(UnmanagedType.BStr)]
				string Unit,
				[In, MarshalAs(UnmanagedType.I4)]
				int Count);


			void Select();


			void PasteHTML(
				[In, MarshalAs(UnmanagedType.BStr)]
				string html);


			void MoveToElementText(
				[In, MarshalAs(UnmanagedType.Interface)]
				IHTMLElement element);


			void SetEndPoint(
				[In, MarshalAs(UnmanagedType.BStr)]
				string how,
				[In, MarshalAs(UnmanagedType.Interface)]
				IHTMLTxtRange SourceRange);

			[return: MarshalAs(UnmanagedType.I4)]
			int CompareEndPoints(
				[In, MarshalAs(UnmanagedType.BStr)]
				string how,
				[In, MarshalAs(UnmanagedType.Interface)]
				IHTMLTxtRange SourceRange);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool FindText(
				[In, MarshalAs(UnmanagedType.BStr)]
				string String,
				[In, MarshalAs(UnmanagedType.I4)]
				int Count,
				[In, MarshalAs(UnmanagedType.I4)]
				int Flags);


			void MoveToPoint(
				[In, MarshalAs(UnmanagedType.I4)]
				int x,
				[In, MarshalAs(UnmanagedType.I4)]
				int y);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBookmark();

			[return: MarshalAs(UnmanagedType.Bool)]
			bool MoveToBookmark(
				[In, MarshalAs(UnmanagedType.BStr)]
				string Bookmark);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool QueryCommandSupported(
				[In, MarshalAs(UnmanagedType.BStr)]
				string cmdID);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool QueryCommandEnabled(
				[In, MarshalAs(UnmanagedType.BStr)]
				string cmdID);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool QueryCommandState(
				[In, MarshalAs(UnmanagedType.BStr)]
				string cmdID);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool QueryCommandIndeterm(
				[In, MarshalAs(UnmanagedType.BStr)]
				string cmdID);

			[return: MarshalAs(UnmanagedType.BStr)]
			string QueryCommandText(
				[In, MarshalAs(UnmanagedType.BStr)]
				string cmdID);

			[return: MarshalAs(UnmanagedType.Struct)]
			object QueryCommandValue(
				[In, MarshalAs(UnmanagedType.BStr)]
				string cmdID);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool ExecCommand(
				[In, MarshalAs(UnmanagedType.BStr)]
				string cmdID,
				[In, MarshalAs(UnmanagedType.Bool)]
				bool showUI,
				[In, MarshalAs(UnmanagedType.Struct)]
				object value);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool ExecCommandShowHelp(
				[In, MarshalAs(UnmanagedType.BStr)]
				string cmdID);
		}
		#endregion

		#region IHTMLWindow2 -- Needs Updating
		[ComVisible(true), Guid("332c4427-26cb-11d0-b483-00c04fd90119"), 
			InterfaceTypeAttribute(ComInterfaceType.InterfaceIsDual)]
		public interface IHTMLWindow2 {

			[return: MarshalAs(UnmanagedType.Struct)]
			Object Item(
				[In]
				ref Object pvarIndex);

			[return: MarshalAs(UnmanagedType.I4)]
			int GetLength();

			[return: MarshalAs(UnmanagedType.Interface)]
			object GetFrames();


			void SetDefaultStatus(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetDefaultStatus();


			void SetStatus(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetStatus();

			[return: MarshalAs(UnmanagedType.I4)]
			int SetTimeout(
				[In, MarshalAs(UnmanagedType.BStr)]
				string expression,
				[In, MarshalAs(UnmanagedType.I4)]
				int msec,
				[In]
				ref Object language);


			void ClearTimeout(
				[In, MarshalAs(UnmanagedType.I4)]
				int timerID);


			void Alert(
				[In, MarshalAs(UnmanagedType.BStr)]
				string message);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool Confirm(
				[In, MarshalAs(UnmanagedType.BStr)]
				string message);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object Prompt(
				[In, MarshalAs(UnmanagedType.BStr)]
				string message,
				[In, MarshalAs(UnmanagedType.BStr)]
				string defstr);

			[return: MarshalAs(UnmanagedType.Interface)]
			object GetImage();

			[return: MarshalAs(UnmanagedType.Interface)]
			object GetLocation();

			[return: MarshalAs(UnmanagedType.Interface)]
			object GetHistory();


			void Close();


			void SetOpener(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetOpener();

			[return: MarshalAs(UnmanagedType.Interface)]
			object GetNavigator();


			void SetName(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetName();

			[return: MarshalAs(UnmanagedType.Interface)]
			IHTMLWindow2 GetParent();

			[return: MarshalAs(UnmanagedType.Interface)]
			IHTMLWindow2 Open(
				[In, MarshalAs(UnmanagedType.BStr)]
				string URL,
				[In, MarshalAs(UnmanagedType.BStr)]
				string name,
				[In, MarshalAs(UnmanagedType.BStr)]
				string features,
				[In, MarshalAs(UnmanagedType.Bool)]
				bool replace);

			[return: MarshalAs(UnmanagedType.Interface)]
			IHTMLWindow2 GetSelf();

			[return: MarshalAs(UnmanagedType.Interface)]
			IHTMLWindow2 GetTop();

			[return: MarshalAs(UnmanagedType.Interface)]
			IHTMLWindow2 GetWindow();


			void Navigate(
				[In, MarshalAs(UnmanagedType.BStr)]
				string URL);


			void SetOnfocus(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetOnfocus();


			void SetOnblur(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetOnblur();


			void SetOnload(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetOnload();


			void SetOnbeforeunload(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetOnbeforeunload();


			void SetOnunload(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetOnunload();

			void SetOnhelp(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetOnhelp();

			void SetOnerror(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetOnerror();

			void SetOnresize(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetOnresize();

			void SetOnscroll(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetOnscroll();

			[return: MarshalAs(UnmanagedType.Interface)]
			IHTMLDocument2 GetDocument();

			[return: MarshalAs(UnmanagedType.Interface)]
			IHTMLEventObj GetEvent();

			[return: MarshalAs(UnmanagedType.Interface)]
			object Get_newEnum();

			[return: MarshalAs(UnmanagedType.Struct)]
			Object ShowModalDialog(
				[In, MarshalAs(UnmanagedType.BStr)]
				string dialog,
				[In]
				ref Object varArgIn,
				[In]
				ref Object varOptions);

			void ShowHelp(
				[In, MarshalAs(UnmanagedType.BStr)]
				string helpURL,
				[In, MarshalAs(UnmanagedType.Struct)]
				Object helpArg,
				[In, MarshalAs(UnmanagedType.BStr)]
				string features);

			[return: MarshalAs(UnmanagedType.Interface)]
			object GetScreen();

			[return: MarshalAs(UnmanagedType.Interface)]
			object GetOption();

			void Focus();

			[return: MarshalAs(UnmanagedType.Bool)]
			bool GetClosed();

			void Blur();

			void Scroll(
				[In, MarshalAs(UnmanagedType.I4)]
				int x,
				[In, MarshalAs(UnmanagedType.I4)]
				int y);

			[return: MarshalAs(UnmanagedType.Interface)]
			object GetClientInformation();

			[return: MarshalAs(UnmanagedType.I4)]
			int SetInterval(
				[In, MarshalAs(UnmanagedType.BStr)]
				string expression,
				[In, MarshalAs(UnmanagedType.I4)]
				int msec,
				[In]
				ref Object language);

			void ClearInterval(
				[In, MarshalAs(UnmanagedType.I4)]
				int timerID);

			void SetOffscreenBuffering(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetOffscreenBuffering();

			[return: MarshalAs(UnmanagedType.Struct)]
			Object ExecScript(
				[In, MarshalAs(UnmanagedType.BStr)]
				string code,
				[In, MarshalAs(UnmanagedType.BStr)]
				string language);

			void ScrollBy(
				[In, MarshalAs(UnmanagedType.I4)]
				int x,
				[In, MarshalAs(UnmanagedType.I4)]
				int y);

			void ScrollTo(
				[In, MarshalAs(UnmanagedType.I4)]
				int x,
				[In, MarshalAs(UnmanagedType.I4)]
				int y);

			void MoveTo(
				[In, MarshalAs(UnmanagedType.I4)]
				int x,
				[In, MarshalAs(UnmanagedType.I4)]
				int y);

			void MoveBy(
				[In, MarshalAs(UnmanagedType.I4)]
				int x,
				[In, MarshalAs(UnmanagedType.I4)]
				int y);

			void ResizeTo(
				[In, MarshalAs(UnmanagedType.I4)]
				int x,
				[In, MarshalAs(UnmanagedType.I4)]
				int y);

			void ResizeBy(
				[In, MarshalAs(UnmanagedType.I4)]
				int x,
				[In, MarshalAs(UnmanagedType.I4)]
				int y);

			[return: MarshalAs(UnmanagedType.Interface)]
			object GetExternal();
		}
		#endregion

		#region IMarkupContainer
		[ComVisible(true), Guid("3050f5f9-98b5-11cf-bb82-00aa00bdce0b"), 
			InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IMarkupContainer {
			void OwningDoc([Out] [MarshalAs(UnmanagedType.Interface)] IHTMLDocument2 ppDoc);
		}
		#endregion

		#region IMarkupPointer
		[ComVisible(true), Guid("3050f49f-98b5-11cf-bb82-00aa00bdce0b"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
		public interface  IMarkupPointer {
			void OwningDoc([Out] [MarshalAs(UnmanagedType.Interface)] IHTMLDocument2 ppDoc);
			void Gravity([Out] POINTER_GRAVITY pGravity);
			void SetGravity([In] POINTER_GRAVITY Gravity);
			void Cling([Out] bool pfCling);
			void SetCling([In] bool fCLing);
			void Unposition();
			void IsPositioned([Out] bool pfPositioned);
			void GetContainer([Out] [MarshalAs(UnmanagedType.Interface)] IMarkupContainer ppContainer);
			void MoveAdjacentToElement([In] [MarshalAs(UnmanagedType.Interface)] IHTMLElement pElement, [In] ELEMENT_ADJACENCY eAdj);
			void MoveToPointer([In] [MarshalAs(UnmanagedType.Interface)] IMarkupPointer pPointer);
			void MoveToContainer([In] [MarshalAs(UnmanagedType.Interface)] IMarkupContainer pContainer,[In] bool fAtStart);
			void Left([In] bool fMove, [Out] [MarshalAs(UnmanagedType.Interface)] MARKUP_CONTEXT_TYPE pContext, [Out] [MarshalAs(UnmanagedType.Interface)] IHTMLElement ppElement, [In, Out] int pcch, [Out] [MarshalAs(UnmanagedType.LPWStr)] string pchText);
			void Right([In] bool fMove, [Out] MARKUP_CONTEXT_TYPE pContext, [Out] [MarshalAs(UnmanagedType.Interface)] IHTMLElement ppElement, [In, Out] int pcch, [Out] [MarshalAs(UnmanagedType.LPWStr)] string pchText);
			void CurrentScope([Out] [MarshalAs(UnmanagedType.Interface)] out IHTMLElement ppElemCurrent);
			void IsLeftOf([In] [MarshalAs(UnmanagedType.Interface)] IMarkupPointer pPointerThat, [Out] out bool pfResult);
			void IsLeftOfOrEqualTo([In] [MarshalAs(UnmanagedType.Interface)] IMarkupPointer pPointerThat, [Out] out bool pfResult);
			void IsRightOf([In] [MarshalAs(UnmanagedType.Interface)] IMarkupPointer pPointerThat, [Out] out bool pfResult);
			void IsRightOfOrEqualTo([In] [MarshalAs(UnmanagedType.Interface)] IMarkupPointer pPointerThat, [Out] out bool pfResult);
			void IsEqualTo([In] [MarshalAs(UnmanagedType.Interface)] IMarkupPointer pPointerThat, [Out] out bool pfAreEqual);
			void MoveUnit([In] MOVEUNIT_ACTION muAction);
			void FindText([In] [MarshalAs(UnmanagedType.LPWStr)] string pchFindText, [In] int dwFlags, [In] [MarshalAs(UnmanagedType.Interface)] IMarkupPointer pIEndMatch, [In] [MarshalAs(UnmanagedType.Interface)] IMarkupPointer pIEndSearch);
		}
		#endregion

		#region IMarkupServices
		[ComVisible(true), Guid("3050f4a0-98b5-11cf-bb82-00aa00bdce0b"), 
			InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IMarkupServices {
			void CreateMarkupPointer([Out] [MarshalAs(UnmanagedType.Interface)] out IMarkupPointer ppPointer);
			void CreateMarkupContainer([Out] [MarshalAs(UnmanagedType.Interface)] IMarkupContainer ppMarkupContainer);
			void CreateElement([In] ELEMENT_TAG_ID tagID, 
				[In] [MarshalAs(UnmanagedType.LPWStr)] string pchAttributes,
				[Out] [MarshalAs(UnmanagedType.Interface)] out IHTMLElement ppElement);
			void CloneElement([In] [MarshalAs(UnmanagedType.Interface)] IHTMLElement pElemCloneThis,
				[Out] [MarshalAs(UnmanagedType.Interface)] out IHTMLElement ppElementTheClone);
			void InsertElement([In] [MarshalAs(UnmanagedType.Interface)] IHTMLElement pElementInsert,
				[In] [MarshalAs(UnmanagedType.Interface)] IMarkupPointer pPointerStart,
				[In] [MarshalAs(UnmanagedType.Interface)] IMarkupPointer pPointerFinish);    
			void RemoveElement([In] [MarshalAs(UnmanagedType.Interface)] IHTMLElement pElementRemove);
			void Remove([In] [MarshalAs(UnmanagedType.Interface)] IMarkupPointer pPointerStart,
				[In] [MarshalAs(UnmanagedType.Interface)] IMarkupPointer pPointerFinish);
			void Copy([In] [MarshalAs(UnmanagedType.Interface)] IMarkupPointer pPointerSourceStart,
				[In] [MarshalAs(UnmanagedType.Interface)] IMarkupPointer pPointerSourceFinish,
				[In] [MarshalAs(UnmanagedType.Interface)] IMarkupPointer pPointerTarget);
			void Move([In] [MarshalAs(UnmanagedType.Interface)] IMarkupPointer pPointerSourceStart,
				[In] [MarshalAs(UnmanagedType.Interface)] IMarkupPointer pPointerSourceFinish,
				[In] [MarshalAs(UnmanagedType.Interface)] IMarkupPointer pPointerTarget);        
			void InsertText([In] [MarshalAs(UnmanagedType.LPWStr)] string pchText,
				[In] [MarshalAs(UnmanagedType.I4)] int cch, //This was LONG.
				[In] [MarshalAs(UnmanagedType.Interface)] IMarkupPointer pPointerTarget);
			void ParseString([In] [MarshalAs(UnmanagedType.LPWStr)] string pchHTML,
				[In] int dwFlags,
				[Out] [MarshalAs(UnmanagedType.Interface)] out IMarkupContainer ppContainerResult,
				[In] [MarshalAs(UnmanagedType.Interface)] IMarkupPointer ppPointerStart,
				[In] [MarshalAs(UnmanagedType.Interface)] IMarkupPointer ppPointerFinish);
			void ParseGlobal([In] Int32 hglobalHTML,
				[In] int dwFlags,
				[Out] [MarshalAs(UnmanagedType.Interface)] IMarkupContainer ppContainerResult,
				[In] [MarshalAs(UnmanagedType.Interface)] IMarkupPointer pPointerStart,
				[In] [MarshalAs(UnmanagedType.Interface)] IMarkupPointer pPointerFinish);
			void IsScopedElement([In] [MarshalAs(UnmanagedType.Interface)] IHTMLElement pElement,
				[Out] bool pfScoped);
			void GetElementTagId([In] [MarshalAs(UnmanagedType.Interface)] IHTMLElement pElement,
				[Out] ELEMENT_TAG_ID ptagId);
			void GetTagIDForName([In] [MarshalAs(UnmanagedType.BStr)] string bstrName,
				[Out] ELEMENT_TAG_ID ptagId);
			void GetNameForTagID([In] ELEMENT_TAG_ID tagId,
				[Out] [MarshalAs(UnmanagedType.BStr)] string pbstrName);
			void MovePointersToRange([In] [MarshalAs(UnmanagedType.Interface)] IHTMLTxtRange pIRange,
				[In] [MarshalAs(UnmanagedType.Interface)] IMarkupPointer pPointerStart,
				[In] [MarshalAs(UnmanagedType.Interface)] IMarkupPointer pPointerFinish);
			void MoveRangeToPointers([In] [MarshalAs(UnmanagedType.Interface)] IMarkupPointer pPointerStart,
				[In] [MarshalAs(UnmanagedType.Interface)] IMarkupPointer pPointerFinish,
				[In] [MarshalAs(UnmanagedType.Interface)] IHTMLTxtRange pIRange);    
			void BeginUndoUnit([In] [MarshalAs(UnmanagedType.LPWStr)] string pchTitle);
			void EndUndoUnit();
		}
		#endregion

        #endregion
	}
}
