using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Interapptive.Shared;
using ShipWorks.UI.Controls.Html.Core;
using Interapptive.Shared.Win32;

namespace ShipWorks.UI.Controls.Html.UndoRedo
{
	/// <summary>
	/// Provides undo\redo helper operations on the given html document
	/// </summary>
	public class UndoManager
	{
        // The owning HtmlControl
        HtmlControl htmlControl;

        // The underlying OleUndoManager we use for our operations
        IOleUndoManager oleUndoManager;

        /// <summary>
        /// Constructor
        /// </summary>
		public UndoManager(HtmlControl htmlControl)
		{
            if (htmlControl == null)
            {
                throw new ArgumentNullException("htmlControl");
            }

            this.htmlControl = htmlControl;

            htmlControl.ReadyStateChanged += new ReadyStateChangedEventHandler(OnReadyStateChanged);
        }

        /// <summary>
        /// The name of an undo unit that should be grouped with the previous undo unit.
        /// </summary>
        public static string UnitNameGroupWithPrevious
        {
            get
            {
                return "__prev";
            }
        }

        /// <summary>
        /// Determines if Undo is available
        /// </summary>
        public bool CanUndo
        {
            get
            {
                return htmlControl.IsCommandEnabled(HtmlApi.IDM_UNDO);
            }
        }

        /// <summary>
        /// Determines if Redo is available
        /// </summary>
        public bool CanRedo
        {
            get
            {
                return htmlControl.IsCommandEnabled(HtmlApi.IDM_REDO);
            }
        }

        /// <summary>
        /// Undoes the last operation.
        /// </summary>
        public bool Undo() 
        {
            if (!CanUndo) 
            {
                return false;
            }

            if (UndoDescription == UnitNameGroupWithPrevious)
            {
                htmlControl.ExecCommand(HtmlApi.IDM_UNDO,null,false,true);
            }

            return htmlControl.ExecCommand(HtmlApi.IDM_UNDO,null,false,true);
        }

        /// <summary>
        /// Redoes the last action.
        /// </summary>
        public bool Redo() 
        {
            if (!CanRedo)
            {
                return false;
            }

            bool result = htmlControl.ExecCommand(HtmlApi.IDM_REDO,null,false,true);

            if (RedoDescription == UnitNameGroupWithPrevious)
            {
                htmlControl.ExecCommand(HtmlApi.IDM_REDO,null,false,true);
            }

            return result;
        }

        /// <summary>
        /// Get a description of the next thing to be redone
        /// </summary>
        public string RedoDescription
        {
            get 
            { 
               return CanRedo ? oleUndoManager.GetLastRedoDescription() : string.Empty; 
            }
        }

        /// <summary>
        /// Get a description of the next thing to be undone
        /// </summary>
        public string UndoDescription
        {
            get 
            { 
                return CanUndo ? oleUndoManager.GetLastUndoDescription() : string.Empty; 
            }
        }

        /// <summary>
        /// The ready state of the html document has changed
        /// </summary>
        private void OnReadyStateChanged(object sender, ReadyStateChangedEventArgs e)
        {
            if (e.ReadyState == HtmlReadyState.Complete)
            {
                if (oleUndoManager != null)
                {
                    // Release the old oleundomanager
                    Marshal.ReleaseComObject(oleUndoManager);
                }

                oleUndoManager = GetOleUndoManager(htmlControl.HtmlDocument);
            }
        }

        /// <summary>
        /// Get the IOleUndoManager for the given document
        /// </summary>
        private IOleUndoManager GetOleUndoManager(HtmlApi.IHTMLDocument2 htmlDocument)
        {
            IComServiceProvider serviceProvider = (IComServiceProvider) htmlDocument;

            Guid undoManagerGuid1 = typeof(IOleUndoManager).GUID;
            Guid undoManagerGuid2 = typeof(IOleUndoManager).GUID;

            IntPtr undoManagerPtr = IntPtr.Zero;

            int hr = serviceProvider.QueryService(ref undoManagerGuid2, ref undoManagerGuid1, out undoManagerPtr);
            if ((hr == NativeMethods.S_OK) && (undoManagerPtr != IntPtr.Zero))
            {
                oleUndoManager = (IOleUndoManager) Marshal.GetObjectForIUnknown(undoManagerPtr);
                Marshal.Release(undoManagerPtr);
            }
               
            return oleUndoManager;
        }
	}
}
