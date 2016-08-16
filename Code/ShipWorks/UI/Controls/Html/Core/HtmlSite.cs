using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using Interapptive.Shared;
using System.Runtime.InteropServices.ComTypes;
using Interapptive.Shared.Win32;

namespace ShipWorks.UI.Controls.Html.Core
{
    /// <summary>
    /// Implements the site on which mshtml is hosted
    /// </summary>
    [NDependIgnore]
    [ComVisible(true), System.CLSCompliant(false)]
	public sealed class HtmlSite : IDisposable, IOleClientSite, IOleContainer, IDocHostUIHandler,
			IOleInPlaceFrame, IOleInPlaceSite, IOleInPlaceSiteEx, IOleDocumentSite, IAdviseSink,
            HtmlApi.IHTMLEditDesigner, IComServiceProvider, IDocHostShowUI, IOleInPlaceUIWindow, IPropertyNotifySink 
    {
		HtmlControl htmlControl;

        HtmlEditHost htmlEditHost = null;

		IOleObject htmlDocumentClass;
		IOleDocumentView view;
		IOleInPlaceActiveObject activeObject;

		IntPtr documentHwnd = IntPtr.Zero;

		bool fullyActive = false;

		ConnectionPointCookie propNotifyCookie;
		int adviseSinkCookie;
		int adviseCookie = 0;

        public event ElementSnapRectEventHandler ElementMoving;
        public event ElementSnapRectEventHandler ElementSizing;

        #region Document \ Activation

		/// <summary>
		/// Creates the document.
		/// </summary>
		public HtmlSite(HtmlControl htmlControl) 
        {
            if ((htmlControl == null) || (htmlControl.IsHandleCreated == false)) 
            {
                throw new ArgumentException();
            }

			this.htmlControl = htmlControl;
		}

		/// <summary>
		/// Disposes the document.
		/// </summary>
		public void Dispose() 
        {
			CloseDocument();
		}

		/// <summary>
		/// This determines which features are enabled in the editor
		/// </summary>
		/// <returns>The common flags that are implimented</returns>
		[DispId(-5512)]
		public int setFlags() 
        {
			int flags = 0;

            if (htmlControl.ShowImages)
            {
                flags |= HtmlApi.DLCTL_DLIMAGES;
            }

		    if (htmlControl.AllowActiveContent) 
            {
				return flags |
                    HtmlApi.DLCTL_VIDEOS | 
                    HtmlApi.DLCTL_BGSOUNDS;
			} 
            else 
            {
				return flags | 
                    HtmlApi.DLCTL_NO_SCRIPTS | 
                    HtmlApi.DLCTL_NO_JAVA | 
                    HtmlApi.DLCTL_NO_DLACTIVEXCTLS |
                    HtmlApi.DLCTL_NO_RUNACTIVEXCTLS | 
                    HtmlApi.DLCTL_SILENT;
			}
		}


		/// <summary>
		/// Returns the document so that other calls can be made.
		/// </summary>
		public HtmlApi.HTMLDocument Document 
        {
			get 
            { 
                return (HtmlApi.HTMLDocument) htmlDocumentClass;
            }
		}

		/// <summary>
		/// Returns the Handle to the Document
		/// </summary>
		internal IntPtr DocumentHandle 
        {
			get 
            {
                return documentHwnd; 
            }
			set 
            {
                documentHwnd = value;
            }
		}

		/// <summary>
		/// Creates the document one control creation
		/// </summary>
		public void CreateDocument() 
        {
			Debug.Assert(htmlDocumentClass == null, "Must call Close before recreating.");

			bool created = false;

			try 
            {
				htmlDocumentClass = (IOleObject) new HtmlApi.HTMLDocument();

                OleApi.OleRun(htmlDocumentClass);

                // Set the client site
				htmlDocumentClass.SetClientSite(this);
		
				// Lock the object in memory
                OleApi.OleLockRunning(htmlDocumentClass, true, false);
		
				htmlDocumentClass.SetHostNames("HtmlControl", "HtmlControl");
				htmlDocumentClass.Advise(this, out adviseCookie);

				propNotifyCookie = new ConnectionPointCookie(htmlDocumentClass, this, typeof(IPropertyNotifySink), false);
				htmlDocumentClass.Advise((IAdviseSink) this, out adviseSinkCookie);
				
				created = true;
			} 
            finally 
            {
                if (!created)
                {
                    htmlDocumentClass = null;
                }
			}
		}

        /// <summary>
        /// Show the document in the window
        /// </summary>
		public void ShowDocument() 
        {
			if (htmlDocumentClass == null) return;
			if (!htmlControl.Visible) return;
			if (DocumentHandle != IntPtr.Zero) return;
			
			try 
            {
				htmlDocumentClass.DoVerb(
                    OleApi.OLEIVERB_SHOW, 
                    IntPtr.Zero, 
                    this, 
                    0,
					htmlControl.Handle, 
                    new NativeMethods.RECT(ReturnSiteRect()));
			} 
            catch {}
		}

        /// <summary>
        /// Hide the document
        /// </summary>
		public void HideDocument() 
        {
			if (htmlDocumentClass == null) return;
			if (!htmlControl.Visible) return;
			if (DocumentHandle != IntPtr.Zero) return;
		}

		/// <summary>
		/// Activates the document when the user clicks in the control etc.
		/// </summary>
		public void ActivateDocument() 
        {
            if (view == null) 
            {
                return;
            }

			htmlDocumentClass.DoVerb(
                OleApi.OLEIVERB_UIACTIVATE, 
                IntPtr.Zero, 
                this, 
                0,
				htmlControl.Handle, 
                new NativeMethods.RECT(ReturnSiteRect()));
		}

        /// <summary>
        /// Deactivate the document view
        /// </summary>
		public void DeactivateDocument() 
        {
			if (htmlDocumentClass == null) return;
			if (!htmlControl.Visible) return;
			
			try 
            {
                if (view != null)
                {
                    view.UIActivate(false);
                }
			} 
            catch {}
		}

        /// <summary>
        /// this is a workaround for a problem calling Caret.SetLocation before it
        /// is ready, in UpdateUI
        /// </summary>
        public bool FullyActive
        {
            get { return fullyActive; }
            set { fullyActive = value; }
        }

        /// <summary>
        /// Closes the document. Use this to clean up the control.
        /// </summary>
        [NDependIgnoreLongMethod]
        public void CloseDocument() 
        {
			try	
            {
				//ReleaseWndProc always called in the SetActiveObject function don't need to call it here.

                if (htmlDocumentClass == null) 
                {
                    return;
                }
		
				if (propNotifyCookie != null) 
                {
					propNotifyCookie.Disconnect();
					propNotifyCookie = null;
				}

				if (adviseSinkCookie != 0) 
                {
					htmlDocumentClass.Unadvise(adviseSinkCookie);
					adviseSinkCookie = 0;
				}

				try	
                {
					//this may raise an exception, however it does work and must
					//be called
					if (view != null) 
                    {
						view.Show(false);
						view.UIActivate(false);
						view.SetInPlaceSite(null);
						view.Close(0);
					}
				} 
                catch (Exception e) 
                {
					Debug.WriteLine("CloseView raised exception: " + e.Message);
				}
	
                // Clear the client site
				htmlDocumentClass.SetClientSite(null);
	
                // Unlock
                OleApi.OleLockRunning(htmlDocumentClass, false, false);

                if (adviseCookie != 0)
                {
                    htmlDocumentClass.Unadvise(adviseCookie);
                }


				//release COM objects
				int refCount = 0;

				if (htmlDocumentClass != null) 
                {
					//this could raise an exception too, but it must be called
					try	
                    {
						this.DeactivateAndUndo();
						htmlDocumentClass.Close(OleApi.OLECLOSE_NOSAVE);
					} 
                    catch (Exception e) 
                    {
						Debug.WriteLine("Close document raised exception: " + e.Message);
					}

					do 
                    {
						refCount = Marshal.ReleaseComObject(htmlDocumentClass);
					} while (refCount > 0);
				}

                if (view != null)
                {
                    do 
                    {
                        refCount = Marshal.ReleaseComObject(view);
                    } while (refCount > 0);
                }

                if (activeObject != null) 
                {
                    do 
                    {
                        refCount = Marshal.ReleaseComObject(activeObject);
                    } while (refCount > 0);
                }
	
				htmlDocumentClass = null;
				view = null;
				activeObject = null;
				htmlControl.htmlDocumentClass = null;
	
			} 
            catch (Exception e) 
            {
				Debug.WriteLine("Close document raised exception: " + e.Message);
			}
		}

        /// <summary>
        /// Return the area that the document should be placed
        /// </summary>
		internal NativeMethods.RECT ReturnSiteRect() 
        {
			NativeMethods.RECT rect = new NativeMethods.RECT(htmlControl.ClientRectangle);

			if (htmlControl.BorderStyle == BorderStyle.FixedSingle) {
				rect.left += 1;
				rect.right -= 2;
				rect.top += 2;
				rect.bottom -= 1;				
			} else if (htmlControl.BorderStyle == BorderStyle.Fixed3D) {
				rect.left += SystemInformation.Border3DSize.Width;
				rect.right -= SystemInformation.Border3DSize.Width;
				rect.top += SystemInformation.Border3DSize.Height;
				rect.bottom -= SystemInformation.Border3DSize.Height;
			}

			if (rect.left < 0)
				rect.left = 0;
			if (rect.left > htmlControl.ClientRectangle.Right)
				rect.left = htmlControl.ClientRectangle.Right;

			if (rect.right < rect.left)
				rect.right = rect.left;
			if (rect.right > htmlControl.ClientRectangle.Right)
				rect.right = htmlControl.ClientRectangle.Right;

			if (rect.top < 0)
				rect.top = 0;
			if (rect.top > htmlControl.ClientRectangle.Bottom)
				rect.top = htmlControl.ClientRectangle.Bottom;
			if (rect.bottom < rect.top)
				rect.bottom = rect.top;
			if (rect.bottom > htmlControl.ClientRectangle.Bottom)
				rect.bottom = htmlControl.ClientRectangle.Bottom;

			return rect;
		}

        /// <summary>
        /// Our containing control is being resized
        /// </summary>
		public void ResizeSite()
        {
			try 
            {
                if (view == null || ((HtmlApi.HTMLDocument) view) == null) 
                {
                    return;
                }

				NativeMethods.RECT rect = ReturnSiteRect();
                view.SetRect(ref rect);
			} 
            catch (Exception ex) 
            {
				Debug.WriteLine(ex.Message);
			}
		}

        /// <summary>
        /// TranslateAccelarator implementation
        /// </summary>
		public int TranslateAccelarator(NativeMethods.MSG msg) 
        {
            if (activeObject != null)
            {
                return activeObject.TranslateAccelerator(msg);
            }

			return NativeMethods.S_FALSE;
		}

        #endregion

        #region HtmlEditHost Event Handlers

        /// <summary>
        /// Create a new instance of the HtmlEditHost
        /// </summary>
        /// <returns></returns>
        private HtmlEditHost CreateHtmlEditHost()
        {
            if (htmlEditHost != null)
            {
                htmlEditHost.ElementMoving -= new ElementSnapRectEventHandler(OnElementMoving);
                htmlEditHost.ElementSizing -= new ElementSnapRectEventHandler(OnElementSizing);
            }

            htmlEditHost = new HtmlEditHost();

            htmlEditHost.ElementMoving += new ElementSnapRectEventHandler(OnElementMoving);
            htmlEditHost.ElementSizing += new ElementSnapRectEventHandler(OnElementSizing);

            return htmlEditHost;
        }

        /// <summary>
        /// An element is being moved at design time
        /// </summary>
        private void OnElementMoving(object sender, ElementSnapRectEventArgs e)
        {
            if (ElementMoving != null)
            {
                ElementMoving(this, e);
            }
        }

        /// <summary>
        /// An element is being sized at design time
        /// </summary>
        private void OnElementSizing(object sender, ElementSnapRectEventArgs e)
        {
            if (ElementSizing != null)
            {
                ElementSizing(this, e);
            }
        }

        #endregion

		#region Implimented functions for all of the IOle stuff.

		public int SaveObject() 
        {
			return NativeMethods.S_OK;
		}

		public int GetMoniker(uint dwAssign, uint dwWhichMoniker, out IMoniker ppmk) {
			ppmk = null;
			return NativeMethods.E_NOTIMPL;
		}

		public int GetContainer(out IOleContainer pphtmlControl) 
        {
			pphtmlControl = (IOleContainer) this;
			return NativeMethods.S_OK;
		}

		public int ShowObject() 
        {
			return NativeMethods.S_OK;
		}

		public int OnShowWindow(bool fShow) 
        {
			return NativeMethods.S_OK;
		}

		public int RequestNewObjectLayout() 
        {
			return NativeMethods.S_OK;
		}

		public int ParseDisplayName(Object pbc, String pszDisplayName, int[] pchEaten, Object[] ppmkOut) 
        {
			return NativeMethods.S_OK;
		}

		public int EnumObjects(uint grfFlags, Object[] ppenum) 
        {
			return NativeMethods.S_OK;	
		}

		public int LockContainer(bool fLock) 
        {
			return NativeMethods.S_OK;	
		}

        /// <summary>
        /// Activate the document in the given view
        /// </summary>
		public int ActivateMe(IOleDocumentView pViewToActivate)	
        {
			if (pViewToActivate == null) 
				return NativeMethods.E_INVALIDARG;

            if (this == null || htmlControl == null) 
            {
                return NativeMethods.S_OK;
            }

			if (view == pViewToActivate) 
            {
				pViewToActivate.UIActivate(true);
			} 
            else 
            {
                NativeMethods.RECT siteRect = ReturnSiteRect();

				view = pViewToActivate;
				pViewToActivate.SetInPlaceSite((IOleInPlaceSite)this);
                pViewToActivate.SetRect(ref siteRect);
				pViewToActivate.Show(true);
			}

			return NativeMethods.S_OK;
		}

        /// <summary>
        /// Return the handle the container's window
        /// </summary>
		public int GetWindow(out IntPtr hWindow) 
        {
			if (this.htmlControl != null) 
            {
				hWindow = htmlControl.Handle;
			} 
            else 
            {
				hWindow = IntPtr.Zero;
			}

			return NativeMethods.S_OK;
		}

		public int ContextSensitiveHelp(bool fEnterMode) 
        {
			return NativeMethods.S_OK;	
		}

		public int EnableModeless(bool fEnableMode) 
        {
			return NativeMethods.S_OK;
		}

		public int GetBorder(out NativeMethods.RECT lprectBorder) 
        {
			lprectBorder = new NativeMethods.RECT(ReturnSiteRect());
			return NativeMethods.S_OK;	
		}

		public int RequestBorderSpace(NativeMethods.RECT pborderwidths) 
        {
			return NativeMethods.S_OK;	
		}

		public int SetBorderSpace(NativeMethods.RECT pborderwidths) 
        {
			return NativeMethods.S_OK;
		}

        /// <summary>
        /// Set the active object for the container
        /// </summary>
		public int SetActiveObject(IOleInPlaceActiveObject pActiveObject, String pszObjName) 
        {
			try 
            {
                // Cleanup
				if (pActiveObject == null) 
                {
					htmlControl.ReleaseWndProc();
                    if (activeObject != null) 
                    {
                        Marshal.ReleaseComObject(this.activeObject);
                    }
					this.activeObject  = null;
					this.documentHwnd = IntPtr.Zero;
					this.fullyActive = false;
				} 
                // Set the active object
                else 
                {
					this.activeObject = pActiveObject;
					this.documentHwnd = new IntPtr();
					pActiveObject.GetWindow(out this.documentHwnd);
					pActiveObject.EnableModeless(true);
					this.fullyActive = true;

					//we have the handle to the doc so set up WndProc override
					htmlControl.SetupWndProc(documentHwnd);
				}

				return NativeMethods.S_OK;
			} 
            catch (Exception e) 
            {
				Debug.WriteLine("Exception in SetActiveObject: " + e.Message + e.StackTrace);
				return NativeMethods.S_FALSE;
			}
		}

		public int InsertMenus(IntPtr hmenuShared, OleApi.OleMenuGroupWidths lpMenuWidths) 
        {
			return NativeMethods.S_OK;	
		}

		public int SetMenu(IntPtr hmenuShared, IntPtr holemenu, IntPtr hwndActiveObject) 
        {
			return NativeMethods.S_OK;	
		}

		public int RemoveMenus(IntPtr hmenuShared) 
        {
			return NativeMethods.S_OK;	
		}

		public int SetStatusText(String pszStatusText) 
        {
			return NativeMethods.S_OK;	
		}

		public int TranslateAccelerator(NativeMethods.MSG lpmsg, short wID) 
        {
			return NativeMethods.S_FALSE;
		}

		public int CanInPlaceActivate() 
        {
			return NativeMethods.S_OK;
		}

		public int OnInPlaceActivate() 
        {
			return NativeMethods.S_OK;	
		}

		public int OnUIActivate() {
			return NativeMethods.S_OK;
		}

		public int GetWindowContext(out IOleInPlaceFrame ppFrame, out IOleInPlaceUIWindow
            ppDoc, out NativeMethods.RECT lprcPosRect, out NativeMethods.RECT lprcClipRect, OleApi.OIFI lpFrameInfo) 
        {
	
			ppDoc = null; //set to null because same as Frame window
			ppFrame = (IOleInPlaceFrame) this;
			NativeMethods.GetClientRect(htmlControl.Handle, out lprcPosRect);

			NativeMethods.GetClientRect(htmlControl.Handle, out lprcClipRect);
			
			//lpFrameInfo.cb = Marshal.SizeOf(typeof(tagOIFI));
			//This value is set by the caller
			if (lpFrameInfo != null) 
            {
				lpFrameInfo.fMDIApp = 0;
				lpFrameInfo.hwndFrame = htmlControl.Handle;
				lpFrameInfo.hAccel = IntPtr.Zero;
				lpFrameInfo.cAccelEntries = 0;
			}

			return NativeMethods.S_OK;
		}

		public int Scroll(NativeMethods.SIZE scrollExtant) 
        {
			return NativeMethods.E_NOTIMPL;
		}

		public int OnUIDeactivate(bool fUndoable) 
        {
			return NativeMethods.S_OK;		
		}

		public int OnInPlaceDeactivate() 
        {
			activeObject = null;
			return NativeMethods.S_OK;
		}

		public int DiscardUndoState() 
        {
			return NativeMethods.S_OK;	
		}

		public int DeactivateAndUndo() 
        {
			return NativeMethods.S_OK;	
		}

		public int OnPosRectChange(NativeMethods.RECT lprcPosRect) 
        {
			return NativeMethods.S_OK;
		}

		public int OnInPlaceActivateEx(out bool pfNoRedraw, uint dwFlags) 
        {
			pfNoRedraw = true; //false means object needs to redraw

			return NativeMethods.S_OK;
		}

		public int OnInPlaceDeactivateEx(bool fNoRedraw) 
        {
			try 
            {
                if (!fNoRedraw && htmlControl != null)
                {
                    htmlControl.Invalidate();
                }
			} 
            catch {}

			return NativeMethods.S_OK;
		}

		public int RequestUIActivate() 
        {
            if (htmlControl.AllowActivation)
            {
                return NativeMethods.S_OK;
            }
            else
            {
                return NativeMethods.S_FALSE;
            }
        }

		#endregion

		#region IDocHostUIHandler

		public int ShowContextMenu(uint dwID, ref NativeMethods.POINT ppt, object pcmdtReserved, object pdispReserved) {

            if (htmlControl == null)
            {
                return NativeMethods.S_FALSE;
            }

			if (htmlControl.AllowContextMenu) 
            {
				if (htmlControl.ContextMenu != null || htmlControl.ContextMenuStrip != null) 
                {
					return NativeMethods.S_OK;
				} 
                else 
                {
					//show the default IE ContextMenu
					return NativeMethods.S_FALSE;
				}
			} else 
            {
				return NativeMethods.S_OK;
			}
		}

        public int GetHostInfo(OleApi.DOCHOSTUIINFO info)
        {
			if (info != null) 
            {
                info.cbSize = Marshal.SizeOf(typeof(OleApi.DOCHOSTUIINFO));
				info.dwDoubleClick = OleApi.DOCHOSTUIDBLCLK_DEFAULT;
				info.dwFlags = 
                    (int)(
                    OleApi.DOCHOSTUIFLAG.NO3DBORDER |
                    OleApi.DOCHOSTUIFLAG.DISABLE_SCRIPT_INACTIVE |
                    OleApi.DOCHOSTUIFLAG.OPENNEWWIN |
                    OleApi.DOCHOSTUIFLAG.FLAT_SCROLLBAR |
                    OleApi.DOCHOSTUIFLAG.THEME);

                if (!htmlControl.AllowTextSelection)
                {
                    info.dwFlags |= (int) OleApi.DOCHOSTUIFLAG.DIALOG;
                }

				info.dwReserved1 = 0;
				info.dwReserved2 = 0;

				return NativeMethods.S_OK;
			} 
            else 
            {
				return NativeMethods.S_FALSE;
			}
		}

		public int ShowUI(int dwID, IOleInPlaceActiveObject activeObject, IOleCommandTarget commandTarget, IOleInPlaceFrame frame, IOleInPlaceUIWindow doc) {
			return NativeMethods.S_OK;
		}

		public int HideUI() {
			return NativeMethods.S_OK;
		}

		public int UpdateUI() 
        {
			//Note the UpdateUI code was moved to it's own function.
			//This function is called in the OnSelectionChange event handler for the HTMLDocumentEvents2 interface.
			//The reason for this is to work around a bug in IE 6 that will cause a crash using UpdateUI that is very hard to find 
			//and debug because it's actually a bug in MSHTML.dll in the system32 directory not anything to do with .net or the interop module.

			if (this.htmlControl != null && this.fullyActive && 
				this.htmlDocumentClass != null && this.htmlControl.EditMode) {
				try {

                    htmlControl.OnUpdateUI();

				} catch (Exception e) {
					Debug.WriteLine(e.Message + e.StackTrace);
					return NativeMethods.S_FALSE;
				}
			}

			//should always return S_OK unless error
			return NativeMethods.S_OK;
		}

		public int OnDocWindowActivate(Boolean fActivate) {
			return NativeMethods.E_NOTIMPL;
		}

		public int OnFrameWindowActivate(Boolean fActivate) {
			return NativeMethods.E_NOTIMPL;
		}

		public int ResizeBorder(NativeMethods.RECT rect, IntPtr doc, Boolean FrameWindow) {
			return NativeMethods.E_NOTIMPL;
		}

		public int GetOptionKeyPath(out IntPtr pbstrKey, uint dw) {
			//use this to set your own app-specific preferences

			//eg pbstrKey = Marshal.StringToBSTR("Software\\myapp\\mysettings\\mshtml");
			pbstrKey = IntPtr.Zero;
			return NativeMethods.S_OK;
		}

		public int GetDropTarget(IOleDropTarget pDropTarget, out IOleDropTarget ppDropTarget) {
			ppDropTarget = null;
			return NativeMethods.E_NOTIMPL;
		}


		public int GetExternal(out object ppDispatch) 
        {
			ppDispatch = null;

			//Note from Jamie on this function:
			//Gotta handle the cases where the htmlControl is dead and the htmlControl's parent is null.
			//Wasn't handled before and was causing an error.
			if (htmlControl == null) 
            {
				return NativeMethods.S_FALSE;
			}

            ppDispatch = htmlControl.ExternalObject;

			return NativeMethods.S_OK;
		}

		public int TranslateAccelerator(NativeMethods.MSG msg, ref Guid group, int nCmdID) {
			return NativeMethods.S_FALSE;
		}

		public int TranslateUrl(int dwTranslate, String strURLIn, out String pstrURLOut) 
        {
            pstrURLOut = null;

            if (!htmlControl.AllowNavigation)
            {
                return NativeMethods.S_OK;
            }

            NavigatingEventArgs e = new NavigatingEventArgs(strURLIn);
            htmlControl.OnBeforeNavigate(e);

            bool translated = false;

            if (e.Cancel)
            {
                if (e.Target.StartsWith("javascript"))
                {
                    pstrURLOut = null;
                }
                else
                {
                    pstrURLOut = " ";
                }

                translated = true;
            }
            else if (e.NewTarget != e.Target)
            {
                pstrURLOut = e.NewTarget;
                translated = true;
            }

            //if scripts are disabled we can't navigate to javascript links
            else if ((e.Target.StartsWith("javascript")) & (!htmlControl.AllowActiveContent))
            {
                pstrURLOut = null;
                translated = true;
            }
            else if ((htmlControl.OpenLinksInNewWindow))
            {
                pstrURLOut = " ";

                //Load the link in an external window.
                System.Diagnostics.Process p = new System.Diagnostics.Process();
                p.StartInfo.FileName = e.NewTarget;
                p.Start();

                translated = true;
            }


            if (e.Target.StartsWith("res://shdoclc"))
            {
                //error condition - redirect to about blank
                pstrURLOut = "about:blank";
                translated = true;
            }

            if (translated)
            {
                return NativeMethods.S_OK;
            }
            else
            {
                return NativeMethods.S_FALSE; //False = not translated.
            }
		}

		public int FilterDataObject(IOleDataObject pDO, out IOleDataObject ppDORet) {
			ppDORet = null;
			return NativeMethods.E_NOTIMPL;
		}
		#endregion

		#region IDocHostShowUI
		/*
		* A host can supply mechanisms that will 
		* show message boxes and Help 
		* by implementing the IDocHostShowUI interface
		*/
		int IDocHostShowUI.ShowMessage(IntPtr hwnd,String lpStrText,
				String lpstrCaption,uint dwType,String lpHelpFile,
				uint dwHelpContext,IntPtr lpresult) {
			return NativeMethods.E_NOTIMPL; 
		}

		int IDocHostShowUI.ShowHelp(IntPtr hwnd,String lpHelpFile,
				uint uCommand,uint dwData, NativeMethods.POINT ptMouse,
				Object pDispatchObjectHit) {
			return NativeMethods.E_NOTIMPL;
		}
		#endregion

		#region IOleServiceProvider

		/*
		* Defines a mechanism for retrieving a service object; 
		* that is, an object that provides custom support to other objects
		*/
		public int QueryService(ref Guid sid, ref Guid iid, out IntPtr ppvObject) {
			int hr = NativeMethods.E_NOINTERFACE;
			System.Guid iid_htmledithost = new System.Guid("3050f6a0-98b5-11cf-bb82-00aa00bdce0b");
			System.Guid sid_shtmledithost = new System.Guid("3050F6A0-98B5-11CF-BB82-00AA00BDCE0B");
				

			if ((sid == sid_shtmledithost) & (iid == iid_htmledithost)) 
            {
				ppvObject = Marshal.GetComInterfaceForObject(CreateHtmlEditHost(), typeof(HtmlApi.IHTMLEditHost));
				if (ppvObject != IntPtr.Zero ) {
					hr = NativeMethods.S_OK;
				}

			}
			else {
				ppvObject = IntPtr.Zero;
			}

			return hr;


//			int hr = NativeMethods.E_NOINTERFACE;
//			ppvObject = IntPtr.Zero;

//			object service = htmlControl.GetService(ref sid);
//			if (service != null) {
//			    if (iid.Equals(Interop.IID_IUnknown)) {
//			        ppvObject = Marshal.GetIUnknownForObject(service);
//			    }
//			    else {
//			        IntPtr pUnk = Marshal.GetIUnknownForObject(service);
//
//			        hr = Marshal.QueryInterface(pUnk, ref iid, out ppvObject);
//			        Marshal.Release(pUnk);
//			    }
//			}

//			return hr;
		}
		#endregion

		#region IHTMLEditDesigner change the IE editor's default behavior
		// IHTMLEditDesigner
		/*
			 * This custom interface provides methods that enable clients using the editor 
			 * to intercept Microsoft® Internet Explorer events 
			 * so that they can change the editor's default behavior
			 * */
		int HtmlApi.IHTMLEditDesigner.PreHandleEvent(int inEvtDispID, HtmlApi.IHTMLEventObj pIEventObj) {			
            if (Document != null) {
				HtmlApi.IHTMLDocument2 doc = (HtmlApi.IHTMLDocument2) Document;


				switch (inEvtDispID) {
					case HtmlApi.DISPID_KEYDOWN:
						//Need to trap Del here
						if (pIEventObj.KeyCode == 9) {
							htmlControl.InvokeTab();
							return NativeMethods.S_OK;
						} else if (pIEventObj.KeyCode == 46) {
							doc.ExecCommand("Delete", false, null);
							return NativeMethods.S_OK;
						} else if (pIEventObj.CtrlKey) {
							System.Diagnostics.Debug.WriteLine(pIEventObj.KeyCode);
							if (pIEventObj.KeyCode >= 48 && pIEventObj.KeyCode <=57)
								htmlControl.DoShortCut((Keys) pIEventObj.KeyCode);
						}
						break;
				}
			}
				
			return NativeMethods.S_FALSE;
		}

		int HtmlApi.IHTMLEditDesigner.PostHandleEvent(int inEvtDispID, HtmlApi.IHTMLEventObj pIEventObj) {
			return NativeMethods.S_FALSE;
		}

		int HtmlApi.IHTMLEditDesigner.TranslateAccelerator(int inEvtDispID, HtmlApi.IHTMLEventObj pIEventObj) {		
			return NativeMethods.S_FALSE;
		}

		int HtmlApi.IHTMLEditDesigner.PostEditorEventNotify(int inEvtDispID, HtmlApi.IHTMLEventObj pIEventObj) {
			return NativeMethods.S_FALSE;
		}
		#endregion IHTMLEditDesigner ================

		#region IAdviseSink Members
        public void OnDataChange(OleApi.FORMATETC pFormatetc, OleApi.STGMEDIUM pStgmed)
        {
			// TODO:  Add HtmlSite.OnDataChange implementation
		}

		public void OnViewChange(int dwAspect, int lindex) {
			// TODO:  Add HtmlSite.OnViewChange implementation
		}

		public void OnRename(IMoniker pmk) {
			// TODO:  Add HtmlSite.OnRename implementation
		}

		public void OnSave() {
			// TODO:  Add HtmlSite.OnSave implementation
		}

		public void OnClose() {
			// TODO:  Add HtmlSite.OnClose implementation
		}
		#endregion

		#region IPropertyNotifySink Members

		public void OnChanged(int dispID) {
			if (dispID == HtmlApi.DISPID_READYSTATE) {
				htmlControl.OnReadyStateChanged();
			}
        }

		public void OnRequestEdit(int dispID) {

        }

        #endregion
    }
}
