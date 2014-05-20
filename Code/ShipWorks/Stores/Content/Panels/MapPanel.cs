using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.Business;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Data;
using ShipWorks.Data.Adapter;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Grid.Columns;
using ShipWorks.Data.Grid.Paging;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Filters;

namespace ShipWorks.Stores.Content.Panels
{
    public partial class MapPanel : UserControl, IDockingPanelContent
    {
        static readonly ILog log = LogManager.GetLogger(typeof(MapPanel));
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
       
        public void LoadState()
        {
            //throw new NotImplementedException();
        }

        public void SaveState()
        {
            //throw new NotImplementedException();
        }

        public bool SupportsMultiSelect
        {
            get
            {
                return false;
            }
        }

        public void ChangeContent(Data.Grid.IGridSelection selection)
        {
            long selectedID = selection.Keys.FirstOrDefault();
            selectedEntity = selectedID == 0 ? null : DataProvider.GetEntity(selection.Keys.FirstOrDefault());
            GetMap();
        }

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

        public void UpdateStoreDependentUI()
        {
            //throw new NotImplementedException();
        }


        public EntityType EntityType
        {
            get
            {
                return EntityType.OrderEntity;
            }
        }

        public FilterTarget[] SupportedTargets
        {
            get
            {
                return new FilterTarget[] { FilterTarget.Orders, FilterTarget.Customers };
            }

        }

        public void ReloadContent()
        {
            //throw new NotImplementedException();
        }

        public void UpdateContent()
        {
            //throw new NotImplementedException();
        }

        private void OnSizeChanged(object sender, EventArgs e)
        {
            pictureBox1.Size = new Size(Size.Width, Size.Height);
            pictureBox1.Location = new Point(0, 0);
            GetMap();
        }
    }
}
