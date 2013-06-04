using System;
using System.Collections.Generic;
using System.Text;

namespace ShipWorks.Data.Model.EntityClasses
{
    public partial class LabelSheetEntity
    {
        string brandName;

        /// <summary>
        /// The brand name of the LabelSheet
        /// </summary>
        public string BrandName
        {
            get 
            {
                if (!IsBuiltin)
                {
                    return "Custom";
                }

                return brandName; 
            }
            set 
            {
                if (!IsBuiltin && value != "Custom")
                {
                    throw new InvalidOperationException("Cannot set custom BrandName.");
                }

                brandName = value; 
            }
        }

        /// <summary>
        /// Indicates if the label sheet is builtin
        /// </summary>
        public bool IsBuiltin
        {
            get { return LabelSheetID < 0; }
        }

        /// <summary>
        /// Don't allow saving of our built in sheets.
        /// </summary>
        protected override void OnBeforeEntitySave()
        {
            if (IsBuiltin)
            {
                throw new InvalidOperationException("Cannot save builtin label sheet.");
            }

            base.OnBeforeEntitySave();
        }
    }
}
