using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Interapptive.Shared.Business;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Data;
using ShipWorks.Data.Grid;
using ShipWorks.Data.Model;
using ShipWorks.Filters;

namespace ShipWorks.Stores.Content.Panels
{
    /// <summary>
    /// Map Panel - holds a google map or streetview image
    /// </summary>
    public partial class MapPanel : UserControl, IDockingPanelContent
    {
        private EntityBase2 selectedEntity;
       
        /// <summary>
        /// Initializes a new instance of the <see cref="MapPanel"/> class.
        /// </summary>
        public MapPanel()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the type of the map.
        /// </summary>
        public MapPanelType MapType 
        { 
            get; 
            set; 
        }

        /// <summary>
        /// Load the state of the panel.
        /// </summary>
        public void LoadState()
        {
        }

        /// <summary>
        /// Save the state of the panel.
        /// </summary>
        public void SaveState()
        {
        }

        /// <summary>
        /// Indicates if the panel can handle multiple selected items at one time.
        /// </summary>
        public bool SupportsMultiSelect
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Change the content of the panel based on the given keys.
        /// </summary>
        /// <param name="selection"></param>
        public void ChangeContent(IGridSelection selection)
        {
            long selectedID = selection.Keys.FirstOrDefault();
            selectedEntity = selectedID == 0 ? null : DataProvider.GetEntity(selection.Keys.FirstOrDefault());
            GetMap();
        }

        /// <summary>
        /// Gets the map.
        /// </summary>
        private void GetMap()
        {
            if (selectedEntity != null)
            {
                AddressAdapter addressAdapter = new AddressAdapter();
                    
                AddressAdapter.Copy(selectedEntity, "Ship", addressAdapter);

                Size size = GetPictureSize(Size);

                pictureBox1.Load(
                    string.Format(GetUrl(),
                        addressAdapter.Street1,
                        addressAdapter.City,
                        addressAdapter.StateProvCode,
                        size.Width,
                        size.Height));

                pictureBox1.Visible = true;
            }
            else
            {
                pictureBox1.Visible = false;
            }
        }

        /// <summary>
        /// Gets the size of the picture - Since max size is 640, panelSize is over 640, the largest possible size of the same aspect ratio is returned.
        /// </summary>
        public static Size GetPictureSize(Size panelSize)
        {
            if (panelSize.Width <= 640 && panelSize.Height <= 640)
            {
                return panelSize;
            }
            else
            {
                Size returnSize = new Size(640, 640);
                if (panelSize.Width > panelSize.Height)
                {
                    returnSize.Height = (int)(640*((double)panelSize.Height/panelSize.Width));
                }
                else
                {
                    returnSize.Width = (int)(640*((double)panelSize.Width/panelSize.Height));
                }
                return returnSize;
            }
        }

        /// <summary>
        /// Gets the URL.
        /// </summary>
        private string GetUrl()
        {
            if (MapType == MapPanelType.Satellite)
            {
                return "http://maps.google.com/maps/api/staticmap?center={0}+{1}+{2}&zoom=18&size={3}x{4}&maptype=hybrid&sensor=false&markers=size:medium%7Ccolor:blue%7C{0}+{1}+{2}";
            }
            else
            {
                return "http://maps.googleapis.com/maps/api/streetview?size={3}x{4}&location={0}+{1}+{2}&fov=120&heading=235&pitch=10&sensor=false";
            }
        }

        /// <summary>
        /// Update the content to reflect changes to the loaded stores
        /// </summary>
        public void UpdateStoreDependentUI()
        {
        }


        /// <summary>
        /// The EntityType displayed by the panel grid
        /// </summary>
        public EntityType EntityType
        {
            get
            {
                return EntityType.OrderEntity;
            }
        }

        /// <summary>
        /// The supported filter targets that the panel can display for.
        /// </summary>
        public FilterTarget[] SupportedTargets
        {
            get
            {
                return new[] { FilterTarget.Orders, FilterTarget.Customers };
            }
        }

        /// <summary>
        /// Refresh the existing selected content by requerying for the relevant keys to ensure an up-to-date related row 
        /// list with up-to-date displayed entity content.
        /// </summary>
        public void ReloadContent()
        {
        }

        /// <summary>
        /// Refresh the existing displayed content.  Does not try to reset or look for new\deleted rows - just refreshes
        /// the known existing rows and their known corresponding entities.
        /// </summary>
        public void UpdateContent()
        {
        }

        /// <summary>
        /// Called when [size changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnSizeChanged(object sender, EventArgs e)
        {
            pictureBox1.Size = new Size(Size.Width, Size.Height);
            pictureBox1.Location = new Point(0, 0);
            GetMap();
        }
    }
}
