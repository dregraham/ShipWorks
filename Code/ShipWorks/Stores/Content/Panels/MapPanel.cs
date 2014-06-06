using System;
using System.Collections.Concurrent;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Interapptive.Shared.Business;
using Interapptive.Shared.Collections;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Data;
using ShipWorks.Data.Grid;
using ShipWorks.Data.Model;
using ShipWorks.Filters;
using ShipWorks.Stores.Platforms.Amazon.WebServices.Associates;
using Image = System.Drawing.Image;

namespace ShipWorks.Stores.Content.Panels
{
    /// <summary>
    /// Map Panel - holds a google map or streetview image
    /// </summary>
    public partial class MapPanel : UserControl, IDockingPanelContent
    {
        EntityBase2 selectedEntity;
        LruCache<string, Image> imageCache = new LruCache<string, Image>(100);
        readonly ConcurrentQueue<long> requestQueue = new ConcurrentQueue<long>();
        readonly object lockObj = new object();

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
            requestQueue.Enqueue(selection.Keys.FirstOrDefault());

            LoadSelection();
        }

        private void LoadSelection()
        {
            if (!googleImage.IsHandleCreated)
            {
                return;
            }

            Task.Factory.StartNew(() =>
            {
                // If we can't get the lock, don't worry
                if (!Monitor.TryEnter(lockObj, 0))
                {
                    return;
                }
                
                try
                {
                    long requestId = 0;

                    while (requestQueue.TryDequeue(out requestId))
                    {
                        long selectionID = requestId;

                        // Get the last item in the queue, since we don't really care about any requests that were queued up in between
                        while (requestQueue.TryDequeue(out requestId))
                        {
                            selectionID = requestId;
                        }

                        selectedEntity = selectionID == 0 ? null : DataProvider.GetEntity(selectionID);
                        GetImage();
                    }
                }
                finally
                {
                    Monitor.Exit(lockObj);
                }
            });
        }

        /// <summary>
        /// Gets the image from Google.
        /// </summary>
        private void GetImage()
        {
            errorLabel.Text = string.Empty;

            if (selectedEntity != null)
            {
                AddressAdapter addressAdapter = new AddressAdapter();
                    
                AddressAdapter.Copy(selectedEntity, "Ship", addressAdapter);

                Size size = GetPictureSize(Size);
                try
                {
                    WebClient imageDownloader = new WebClient();
                    byte[] image = imageDownloader.DownloadData(string.Format(GetUrl(),
                        addressAdapter.Street1,
                        addressAdapter.City,
                        addressAdapter.StateProvCode,
                        size.Width,
                        size.Height));

                    googleImage.Invoke(new MethodInvoker(delegate
                    {
                        using (MemoryStream stream = new MemoryStream(image))
                        {
                            googleImage.Image = Image.FromStream(stream);
                        }

                        googleImage.Visible = true;
                    }));
                }
                catch (Exception)
                {
                    googleImage.Invoke(new MethodInvoker(delegate
                    {
                        googleImage.Visible = false;
                        errorLabel.Text = "Cannot get image from Google.";
                    }));
                }
            }
            else
            {
                googleImage.Invoke(new MethodInvoker(delegate
                {
                    googleImage.Visible = false;
                }));
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
            googleImage.Size = new Size(Size.Width, Size.Height);
            googleImage.Location = new Point(0, 0);


            requestQueue.Enqueue(selectedEntity != null ? EntityUtility.GetEntityId(selectedEntity) : 0);

            LoadSelection();
            //GetImage();
        }
    }
}
