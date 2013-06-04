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
	/// Implements the IOleParentUndoUnit interface
	/// </summary>
	public class ParentUndoUnit : IOleParentUndoUnit
	{
        // Description of the undo\redo action
        string description;

        // The open parent unit that is a child of this one, if any
        IOleParentUndoUnit openSubParentUnit = null;

        // List of all child units we have
        ArrayList childUnits = new ArrayList();

        /// <summary>
        /// Constructor
        /// </summary>
		public ParentUndoUnit(string description)
		{
            this.description = description;
        }

        #region IOleParentUndoUnit Members

        /// <summary>
        /// A new parent unit is being opened, which will become a child of this unit.
        /// </summary>
        public int Open(IOleParentUndoUnit parentUnit)
        {
            if (parentUnit == null)
                return NativeMethods.E_POINTER;

            string name;
            IOleUndoUnit undoUnit = (IOleUndoUnit) parentUnit;
            undoUnit.GetDescription(out name);

            if (openSubParentUnit == null)
            {
                openSubParentUnit = parentUnit;
                return NativeMethods.S_OK;
            }

            else
            {
                return openSubParentUnit.Open(parentUnit);
            }
        }

        /// <summary>
        /// Close the unit
        /// </summary>
        public int Close(IOleParentUndoUnit parentUnit, bool fCommit)
        {
            // No open child parent unit, return S_FALSE
            if (openSubParentUnit == null) 
                return NativeMethods.S_FALSE;

            // Call close on the child
            int hRet = openSubParentUnit.Close(parentUnit, fCommit);

            // Handled by the child parent undo unit
            if (hRet == NativeMethods.S_OK) 
            {
                return NativeMethods.S_OK;
            }

            // Verify that unit is equal to open child
            if (hRet == NativeMethods.S_FALSE) 
            {
                // Dont match, dont change internal state
                if (openSubParentUnit != parentUnit)
                {
                    return NativeMethods.E_INVALIDARG;
                }

                // Add to the collection
                if (fCommit)
                {
                    childUnits.Add(openSubParentUnit);
                }

                openSubParentUnit = null;
                return NativeMethods.S_OK;
            }

            return hRet;
        }

        /// <summary>
        /// Add a new simple unit to the stack
        /// </summary>
        public int Add(IOleUndoUnit undoUnit)
        {
            if (undoUnit == null)
            {
                throw new ArgumentNullException("undoUnit");
            }

            string name;
            undoUnit.GetDescription(out name);

            if (openSubParentUnit == null)
            {
                if (childUnits.Count > 0)
                {
                    ((IOleUndoUnit) childUnits[childUnits.Count - 1]).OnNextAdd();
                }

                childUnits.Add(undoUnit);
                return NativeMethods.S_OK;
            }

            else
            {
                int res = openSubParentUnit.Add(undoUnit);
                return res;
            }
        }

        /// <summary>
        /// Determine if the given UndoUnit is apart of this hierarchy
        /// </summary>
        public int FindUnit(IOleUndoUnit undoUnit)
        {
            // Look in our own collection of units
            if (childUnits.Contains(undoUnit))
            {
                return NativeMethods.S_OK;
            }

            // Look in our child paren't subitems
            if (openSubParentUnit != null)
            {
                return openSubParentUnit.FindUnit(undoUnit);
            }
            else
            {
                return NativeMethods.S_FALSE;
            }
        }

        /// <summary>
        /// Get the state
        /// </summary>
        public int GetParentState(out int state)
        {
            if (openSubParentUnit != null)
            {
                return openSubParentUnit.GetParentState(out state);
            }

            // Return UAS_NORMAL
            state = 0;

            return NativeMethods.S_OK;
        }

        #endregion

        #region IOleUndoUnit Members

        /// <summary>
        /// Do the undo's
        /// </summary>
        public int Do(IOleUndoManager undoManager)
        {
            ParentUndoUnit newParent = null;

            if (undoManager != null)
            {
                // Create a new parent to be put on the other stack
                newParent = new ParentUndoUnit(description);
                undoManager.Open(newParent);
            }

            int hr = NativeMethods.S_OK;
            
            // Undo all of the children
            foreach (IOleUndoUnit unit in childUnits)
            {
                hr = unit.Do(undoManager);

                if (hr != NativeMethods.S_OK)
                {
                    break;
                }
            }

            if (undoManager != null)
            {
                bool commit = (hr == NativeMethods.S_OK);
                int closeRet = undoManager.Close(newParent, commit);

                // Pass along this error if we tried to commit
                if (commit) 
                {
                    hr = closeRet;
                }
            }

            return hr;       
        }

        /// <summary>
        /// Get a description of the undo\redo batch
        /// </summary>
        public int GetDescription(out string bStr)
        {
            bStr = description;

            return NativeMethods.S_OK;
        }

        /// <summary>
        /// Get the CLSDID and type of this unit.  Not used.
        /// </summary>
        public int GetUnitType(out int clsid, out int plID)
        {
            clsid = 0;
            plID = 0;
            return 0;
        }

        /// <summary>
        /// An undo unit has been added after this one
        /// </summary>
        public int OnNextAdd()
        {
            return NativeMethods.S_OK;
        }

        #endregion
    }
}
