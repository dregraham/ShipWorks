using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Interapptive.Shared.Business;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Data;
using ShipWorks.Data.Grid;
using ShipWorks.Data.Model;
using ShipWorks.Filters;
using ShipWorks.Shipping.ShipSense.Hashing;
using Image = System.Drawing.Image;
using ShipWorks.Core.Common.Threading;

namespace ShipWorks.Stores.Content.Panels
{
    /// <summary>
    /// Map Panel - holds a google map or streetview image
    /// </summary>
    public partial class MapPanel : UserControl, IDockingPanelContent
    {
        private EntityBase2 selectedEntity;
        private LruCache<string, Image> imageCache = new LruCache<string, Image>(100);
        private readonly ConcurrentQueue<long> requestQueue = new ConcurrentQueue<long>();
        private readonly object lockObj = new object();

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
        public MapPanelType MapType { get; set; }

        /// <summary>
        /// Load the state of the panel.
        /// </summary>
        public void LoadState()
        {}

        /// <summary>
        /// Save the state of the panel.
        /// </summary>
        public void SaveState()
        {}

        /// <summary>
        /// Indicates if the panel can handle multiple selected items at one time.
        /// </summary>
        public bool SupportsMultiSelect => false;

        /// <summary>
        /// Change the content of the panel based on the given keys.
        /// </summary>
        /// <param name="selection"></param>
        public Task ChangeContent(IGridSelection selection)
        {
            requestQueue.Enqueue(selection.Keys.FirstOrDefault());

            return LoadSelection();
        }

        private Task LoadSelection()
        {
            if (!googleImage.IsHandleCreated)
            {
                return TaskUtility.CompletedTask;
            }

            return Task.Factory.StartNew(async () =>
            {
                // If we can't get the lock, don't worry
                if (!Monitor.TryEnter(lockObj, 0))
                {
                    return;
                }

                try
                {
                    long requestId = 0;
                    GoogleResponse googleResponse = null;

                    googleImage.Invoke(new MethodInvoker(delegate
                    {
                        googleImage.Visible = false;
                        errorLabel.Text = "Loading";
                    }));

                    while (requestQueue.TryDequeue(out requestId))
                    {
                        long selectionID = requestId;

                        // Get the last item in the queue, since we don't really care about any requests that were queued up in between
                        while (requestQueue.TryDequeue(out requestId))
                        {
                            selectionID = requestId;
                        }

                        selectedEntity = selectionID == 0 ? null : DataProvider.GetEntity(selectionID);
                        googleResponse = await GetImage();
                    }

                    googleImage.Invoke(new MethodInvoker(delegate
                    {
                        if (selectedEntity == null)
                        {
                            googleImage.Visible = false;
                            errorLabel.Text = string.Empty;
                        }
                        else if (googleResponse.ReturnedImage != null)
                        {
                            googleImage.Image = googleResponse.ReturnedImage;
                            googleImage.Visible = true;
                            errorLabel.Text = string.Empty;
                        }
                        else if (googleResponse.IsThrottled)
                        {
                            googleImage.Visible = false;
                            errorLabel.Text = @"Too many requests. Please try again later.";
                        }
                        else
                        {
                            googleImage.Visible = false;
                            errorLabel.Text = @"Cannot contact map server.";
                        }
                    }));
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
        private async Task<GoogleResponse> GetImage()
        {
            AddressAdapter addressAdapter = SelectedAddress();
            if (addressAdapter == null)
            {
                return null;
            }

            Size size = GetPictureSize(Size);

            StringHash hasher = new StringHash();
            string hashValue = string.Join("|", addressAdapter.Street1, addressAdapter.City, addressAdapter.StateProvCode, size.Width.ToString(), size.Height.ToString());
            string hash = hasher.Hash(hashValue, "somesalt");

            GoogleResponse response;

            if (imageCache.Contains(hash))
            {
                response = new GoogleResponse() { ReturnedImage = imageCache[hash] };
            }
            else
            {
                response = await LoadImageFromGoogle(addressAdapter, size);

                if (!response.IsThrottled)
                {
                    imageCache[hash] = response.ReturnedImage;
                }
            }

            return response;
        }

        /// <summary>
        /// Selects the address.
        /// </summary>
        /// <returns>Selected Address - null if non selected</returns>
        private AddressAdapter SelectedAddress()
        {
            if (selectedEntity == null)
            {
                return null;
            }

            AddressAdapter addressAdapter = new AddressAdapter();

            AddressAdapter.Copy(selectedEntity, "Ship", addressAdapter);
            return addressAdapter;
        }

        private async Task<GoogleResponse> LoadImageFromGoogle(AddressAdapter addressAdapter, Size size)
        {
            try
            {
                byte[] image;

                using (WebClient imageDownloader = new WebClient())
                {
                    image = await imageDownloader.DownloadDataTaskAsync(string.Format(GetImageUrl(),
                        addressAdapter.Street1,
                        addressAdapter.City,
                        addressAdapter.StateProvCode,
                        size.Width,
                        size.Height));
                }

                using (MemoryStream stream = new MemoryStream(image))
                {
                    return new GoogleResponse() { ReturnedImage = Image.FromStream(stream) };
                }
            }
            catch (WebException ex)
            {
                GoogleResponse googleResponse = new GoogleResponse();

                if (((HttpWebResponse)ex.Response).StatusCode == HttpStatusCode.Forbidden)
                {
                    googleResponse.IsThrottled = true;
                }

                return googleResponse;
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
        private string GetImageUrl()
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
        {}


        /// <summary>
        /// The EntityType displayed by the panel grid
        /// </summary>
        public EntityType EntityType
        {
            get { return EntityType.OrderEntity; }
        }

        /// <summary>
        /// The supported filter targets that the panel can display for.
        /// </summary>
        public FilterTarget[] SupportedTargets
        {
            get { return new[] { FilterTarget.Orders, FilterTarget.Customers }; }
        }

        /// <summary>
        /// Refresh the existing selected content by requerying for the relevant keys to ensure an up-to-date related row 
        /// list with up-to-date displayed entity content.
        /// </summary>
        public Task ReloadContent()
        {
            return TaskUtility.CompletedTask;
        }

        /// <summary>
        /// Refresh the existing displayed content.  Does not try to reset or look for new\deleted rows - just refreshes
        /// the known existing rows and their known corresponding entities.
        /// </summary>
        public Task UpdateContent()
        {
            return TaskUtility.CompletedTask;
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
        }

        /// <summary>
        /// The resposne we get back from Google.
        /// </summary>
        private class GoogleResponse
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="GoogleResponse"/> class.
            /// </summary>
            public GoogleResponse()
            {
                ReturnedImage = null;
                IsThrottled = false;
            }

            /// <summary>
            /// Gets or sets the returned image.
            /// </summary>
            public Image ReturnedImage { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether this instance is throttled.
            /// </summary>
            public bool IsThrottled { get; set; }
        }

        /// <summary>
        /// Called when [google image click].
        /// </summary>
        private void OnGoogleImageClick(object sender, EventArgs e)
        {
            var addressAdapter = SelectedAddress();
            if (addressAdapter == null)
            {
                return;
            }
            
            string url = string.Format("https://www.google.com/maps/place/{0}+{1}+{2}+{3}+{4}",
                addressAdapter.Street1,
                addressAdapter.City,
                addressAdapter.StateProvCode,
                addressAdapter.PostalCode,
                addressAdapter.CountryCode);

            Process.Start(url);
        }
    }
}

