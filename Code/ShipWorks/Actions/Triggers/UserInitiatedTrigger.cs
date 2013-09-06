using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using ShipWorks.Actions.Triggers.Editors;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Properties;
using ShipWorks.UI;

namespace ShipWorks.Actions.Triggers
{
    /// <summary>
    /// A trigger that allows a user to click a custom button or menu item to initiate a sequence of actions
    /// </summary>
    public class UserInitiatedTrigger : ActionTrigger
    {
        bool showOnRibbon = true;
        bool showOnOrdersMenu;
        bool showOnCustomersMenu;

        UserInitiatedSelectionRequirement selectionRequirement = UserInitiatedSelectionRequirement.Orders;

        long resourceReferenceID = 0;
        Image pendingImage = null;

        Guid guid = Guid.NewGuid();

        /// <summary>
        /// Constructor for default settings
        /// </summary>
        public UserInitiatedTrigger()
            : base(null)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public UserInitiatedTrigger(string xmlSettings)
            : base(xmlSettings)
        {

        }

        /// <summary>
        /// Save validation
        /// </summary>
        public override void Validate()
        {
            if (!showOnRibbon && !showOnOrdersMenu && !showOnCustomersMenu)
            {
                throw new ActionTriggerException("You must select to either add a button or add a menu item.");
            }
        }

        /// <summary>
        /// Return the enumerate trigger type value represented by the type
        /// </summary>
        public override ActionTriggerType TriggerType
        {
            get { return ActionTriggerType.UserInitiated; }
        }

        /// <summary>
        /// Create the editor for editing the settings of the trigger
        /// </summary>
        public override ActionTriggerEditor CreateEditor()
        {
            return new UserInititatedTriggerEditor(this);
        }

        /// <summary>
        /// The entity type that triggers this trigger
        /// </summary>
        public override EntityType? TriggeringEntityType
        {
            get 
            {
                switch (SelectionRequirement)
                {
                    case UserInitiatedSelectionRequirement.Orders:
                        return EntityType.OrderEntity;
                    case UserInitiatedSelectionRequirement.Customers:
                        return EntityType.CustomerEntity;
                    default:
                        return null;
                }
            }
        }

        /// <summary>
        /// Guid that remains the same for the specifc trigger instance for the lifetime of the trigger.  Needed to make the Quick Access Toolbar work between sessions.
        /// </summary>
        public Guid Guid
        {
            get { return guid; }
            set { guid = value; }
        }

        /// <summary>
        /// Indicates if the trigger adds a button to the UI
        /// </summary>
        public bool ShowOnRibbon
        {
            get { return showOnRibbon; }
            set { showOnRibbon = value; }
        }

        /// <summary>
        /// Indicates if the trigger adds a menu to the UI
        /// </summary>
        public bool ShowOnOrdersMenu
        {
            get { return showOnOrdersMenu; }
            set { showOnOrdersMenu = value; }
        }

        /// <summary>
        /// Indicates if the trigger adds a menu to the UI
        /// </summary>
        public bool ShowOnCustomersMenu
        {
            get { return showOnCustomersMenu; }
            set { showOnCustomersMenu = value; }
        }

        /// <summary>
        /// What has to be selected, if anything
        /// </summary>
        public UserInitiatedSelectionRequirement SelectionRequirement
        {
            get 
            { 
                return selectionRequirement; 
            }
            set 
            {
                if (value == selectionRequirement)
                {
                    return;
                }

                selectionRequirement = value;

                RaiseTriggeringEntityTypeChanged();
            }
        }

        /// <summary>
        /// The ID of the image resource in the database
        /// </summary>
        public long ResourceReferenceID
        {
            get { return resourceReferenceID; }
            set { resourceReferenceID = value; }
        }

        /// <summary>
        /// The Image that will be used the next time the trigger is saved
        /// </summary>
        [XmlIgnore]
        public Image PendingImage
        {
            get 
            { 
                return pendingImage; 
            }
            set 
            {
                if (value != null)
                {
                    value = DisplayHelper.ResizeImage(value, new Size(32, 32));
                }

                pendingImage = value; 
            }
        }

        /// <summary>
        /// Load the image to display for the custom buttons
        /// </summary>
        public Image LoadImage()
        {
            if (pendingImage != null)
            {
                return (Image) pendingImage.Clone();
            }

            DataResourceReference resource = DataResourceManager.LoadResourceReference(resourceReferenceID);
            if (resource != null)
            {
                return Image.FromFile(resource.GetCachedFilename());
            }

            return Resources.gear_run32;
        }

        /// <summary>
        /// Save the image file resource to the database
        /// </summary>
        public override void SaveExtraState(ActionEntity action, SqlAdapter adapter)
        {
            if (pendingImage == null)
            {
                return;
            }

            // Cleanup the old resource
            DeleteExtraState(action, adapter);

            using (MemoryStream imageStream = new MemoryStream())
            {
                pendingImage.Save(imageStream, ImageFormat.Png);

                resourceReferenceID = DataResourceManager.CreateFromBytes(imageStream.ToArray(), action.ActionID, "Image").ReferenceID;
            }

            // Clear the pending now that it's saved
            pendingImage = null;
        }

        /// <summary>
        /// Delete any extra state
        /// </summary>
        public override void DeleteExtraState(ActionEntity action, SqlAdapter adapter)
        {
            if (resourceReferenceID != 0)
            {
                DataResourceManager.ReleaseResourceReference(resourceReferenceID);
            }
        }
    }
}
