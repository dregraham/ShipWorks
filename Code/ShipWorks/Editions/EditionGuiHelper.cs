using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Drawing;
using ShipWorks.Properties;
using System.Windows.Forms;
using System.Reflection;
using Autofac;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.UI.Controls;
using IContainer = System.ComponentModel.IContainer;

namespace ShipWorks.Editions
{
    /// <summary>
    /// Component that can be added to Forms and Controls to help with managing UI changes related to Editions
    /// </summary>
    public partial class EditionGuiHelper : Component
    {
        List<ManagedElement> managedElements = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public EditionGuiHelper()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public EditionGuiHelper(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        /// <summary>
        /// Initialization \ preparation
        /// </summary>
        private void Initialize()
        {
            managedElements = new List<ManagedElement>();

            EditionManager.RestrictionsChanged += new EventHandler(OnEditionRestrictionsChanged);
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();

                EditionManager.RestrictionsChanged -= new EventHandler(OnEditionRestrictionsChanged);
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// The active edition restrictions have changed
        /// </summary>
        void OnEditionRestrictionsChanged(object sender, EventArgs e)
        {
            if (Program.MainForm.InvokeRequired)
            {
                Program.MainForm.BeginInvoke(new MethodInvoker(UpdateUI));
            }
            else
            {
                UpdateUI();
            }
        }

        /// <summary>
        /// Register the given component to reflect the state of the given feature
        /// </summary>
        public void RegisterElement(Component component, EditionFeature feature)
        {
            RegisterElement(component, feature, null);
        }

        /// <summary>
        /// Register the given component to reflect the state of the given feature
        /// </summary>
        public void RegisterElement(Component component, EditionFeature feature, Func<object> dataProvider)
        {
            // Initial setup
            if (managedElements == null)
            {
                Initialize();
            }

            if (managedElements.Any(e => e.Component == component))
            {
                throw new InvalidOperationException("The component is already being managed.");
            }

            var element = new ManagedElement(component, feature, dataProvider);
            element.UpdateUI();

            managedElements.Add(element);
        }

        /// <summary>
        /// Update all registered UI elements to reflect the current version
        /// </summary>
        public void UpdateUI()
        {
            // initial setup
            if (managedElements == null)
            {
                Initialize();
            }

            foreach (var element in managedElements)
            {
                element.UpdateUI();
            }
        }

        /// <summary>
        /// Get the lock image that is closest to the given ideal size
        /// </summary>
        public static Image GetLockImage(int idealSize)
        {
            return idealSize <= 16 ? Resources.lock16 : Resources.lock32;
        }

        class ManagedElement
        {
            static object formsButtonClickEventID;
            static object toolStripClickEventID;

            static PropertyInfo componentEventsProperty;
            static FieldInfo sandWidgetActivatedField;

            Component component;
            EditionFeature feature;
            Func<object> dataProvider;

            // Indicates if its currently in a restricted state
            bool isRestricted = false;

            // Original data that will be restored
            Image originalImage;
            EventHandler originalClick;
            Divelements.SandRibbon.Popup originalSandPopup;

            /// <summary>
            /// Static constructor
            /// </summary>
            static ManagedElement()
            {
                formsButtonClickEventID = typeof(Control).GetField("EventClick", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);
                toolStripClickEventID = typeof(ToolStripItem).GetField("EventClick", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);

                componentEventsProperty = typeof(Component).GetProperty("Events", BindingFlags.NonPublic | BindingFlags.Instance);
                sandWidgetActivatedField = typeof(Divelements.SandRibbon.WidgetBase).GetField("x5b7f6ddd07ded8cd", BindingFlags.NonPublic | BindingFlags.Instance);

                if ((formsButtonClickEventID ?? toolStripClickEventID ?? componentEventsProperty ?? (object) sandWidgetActivatedField) == null)
                {
                    throw new InvalidOperationException("Some of the reflected stuff in here, the internals have changed");
                }
            }

            /// <summary>
            /// Constructor
            /// </summary>
            public ManagedElement(Component component, EditionFeature feature, Func<object> dataProvider)
            {
                this.component = component;
                this.feature = feature;
                this.dataProvider = dataProvider;
            }

            /// <summary>
            /// The component being managed
            /// </summary>
            public Component Component
            {
                get { return component; }
            }

            /// <summary>
            /// Update the UI of the element based on the current conditions of the active edition
            /// </summary>
            public void UpdateUI()
            {
                using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
                {
                    ILicenseService service = lifetimeScope.Resolve<ILicenseService>();
                    EditionRestrictionLevel restriction = service?.CheckRestriction(feature, dataProvider?.Invoke()) ?? EditionRestrictionLevel.None;

                    if (restriction == EditionRestrictionLevel.Hidden)
                    {
                        throw new InvalidOperationException("Completely hiding GUI elements is not currently supported.");
                    }

                    bool needsRestricted = restriction != EditionRestrictionLevel.None;
                    if ((needsRestricted && !isRestricted) || (!needsRestricted && isRestricted))
                    {
                        ApplyRestriction(component as Button, restriction);
                        ApplyRestriction(component as LinkControl, restriction);
                        ApplyRestriction(component as ToolStripMenuItem, restriction);
                        ApplyRestriction(component as Divelements.SandRibbon.Button, restriction);
                    }
                }
            }

            /// <summary>
            /// Handler that gets called when we intercept the original click
            /// </summary>
            private void InterceptOriginalClick()
            {
                IWin32Window owner = GetWindow(component);

                using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
                {
                    ILicenseService licenseService = lifetimeScope.Resolve<ILicenseService>();

                    if (licenseService?.HandleRestriction(feature, dataProvider?.Invoke(), owner) ?? false)
                    {
                        // Could be null in Ribbon popup case
                        originalClick?.Invoke(component, EventArgs.Empty);
                    }
                }
            }

            /// <summary>
            /// Setup the restriction
            /// </summary>
            private void ApplyRestriction(Button button, EditionRestrictionLevel restriction)
            {
                if (button == null)
                {
                    return;
                }

                EventHandlerList handlerList = (EventHandlerList) componentEventsProperty.GetValue(button, null);
                bool restrict = restriction != EditionRestrictionLevel.None;

                if (restrict)
                {
                    originalImage = button.Image;
                    originalClick = (EventHandler) handlerList[formsButtonClickEventID];

                    button.Image = EditionGuiHelper.GetLockImage(originalImage.Size.Width);
                    handlerList[formsButtonClickEventID] = new EventHandler((object sender, EventArgs e) => InterceptOriginalClick());
                }
                else
                {
                    button.Image = originalImage;
                    handlerList[formsButtonClickEventID] = originalClick;
                }

                isRestricted = restrict;
            }

            /// <summary>
            /// Setup the restriction
            /// </summary>
            private void ApplyRestriction(LinkControl link, EditionRestrictionLevel restriction)
            {
                if (link == null)
                {
                    return;
                }

                EventHandlerList handlerList = (EventHandlerList) componentEventsProperty.GetValue(link, null);
                bool restrict = restriction != EditionRestrictionLevel.None;

                if (restrict)
                {
                    originalImage = link.Image;
                    originalClick = (EventHandler) handlerList[formsButtonClickEventID];

                    link.Image = EditionGuiHelper.GetLockImage(originalImage?.Size.Width ?? link.Height);
                    handlerList[formsButtonClickEventID] = new EventHandler((object sender, EventArgs e) => InterceptOriginalClick());

                    if (originalImage == null)
                    {
                        link.AutoSize = false;
                        link.Width = link.Width += link.Image.Width + 2;
                        link.ImageAlign = ContentAlignment.MiddleLeft;
                        link.TextAlign = ContentAlignment.MiddleRight;
                    }
                }
                else
                {
                    link.Image = originalImage;
                    handlerList[formsButtonClickEventID] = originalClick;

                    if (originalImage == null)
                    {
                        link.AutoSize = true;
                    }
                }

                isRestricted = restrict;
            }

            /// <summary>
            /// Setup the restriction
            /// </summary>
            private void ApplyRestriction(ToolStripMenuItem menuItem, EditionRestrictionLevel restriction)
            {
                if (menuItem == null)
                {
                    return;
                }

                EventHandlerList handlerList = (EventHandlerList) componentEventsProperty.GetValue(menuItem, null);
                bool restrict = restriction != EditionRestrictionLevel.None;

                if (restrict)
                {
                    originalImage = menuItem.Image;
                    originalClick = (EventHandler) handlerList[toolStripClickEventID];

                    menuItem.Image = EditionGuiHelper.GetLockImage(originalImage.Size.Width);
                    handlerList[toolStripClickEventID] = new EventHandler((object sender, EventArgs e) => InterceptOriginalClick());
                }
                else
                {
                    menuItem.Image = originalImage;
                    handlerList[toolStripClickEventID] = originalClick;
                }

                isRestricted = restrict;
            }

            /// <summary>
            /// Setup the restriction
            /// </summary>
            private void ApplyRestriction(Divelements.SandRibbon.Button button, EditionRestrictionLevel restriction)
            {
                if (button == null)
                {
                    return;
                }

                bool restrict = restriction != EditionRestrictionLevel.None;

                if (restrict)
                {
                    originalImage = button.Image;
                    originalClick = (EventHandler) sandWidgetActivatedField.GetValue(button);
                    originalSandPopup = button.PopupWidget;

                    button.Image = EditionGuiHelper.GetLockImage(originalImage.Size.Width);
                    sandWidgetActivatedField.SetValue(button, new EventHandler((object sender, EventArgs e) => InterceptOriginalClick()));
                    button.PopupWidget = null;
                }
                else
                {
                    button.Image = originalImage;
                    sandWidgetActivatedField.SetValue(button, originalClick);
                    button.PopupWidget = originalSandPopup;
                }

                isRestricted = restrict;
            }

            /// <summary>
            /// Get a window object to use as a Form owner
            /// </summary>
            private static IWin32Window GetWindow(object sender)
            {
                if (sender is Control)
                {
                    return (Control) sender;
                }

                if (sender is Divelements.SandRibbon.Button)
                {
                    return ((Divelements.SandRibbon.Button) sender).HostControl;
                }

                return null;
            }
        }
    }
}
