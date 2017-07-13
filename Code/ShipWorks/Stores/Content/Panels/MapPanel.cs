using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Interapptive.Shared.Business;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Core.Common.Threading;
using ShipWorks.Core.Messaging;
using ShipWorks.Core.Messaging.Messages.Shipping;
using ShipWorks.Data.Grid;
using ShipWorks.Data.Model;
using ShipWorks.Filters;
using ShipWorks.Messaging.Messages;
using ShipWorks.Messaging.Messages.Panels;
using TD.SandDock;
using Image = System.Drawing.Image;

namespace ShipWorks.Stores.Content.Panels
{
    /// <summary>
    /// Map Panel - holds a Google map or streetview image
    /// </summary>
    public partial class MapPanel : UserControl, IDockingPanelContent
    {
        private readonly static FilterTarget[] supportedTargets = { FilterTarget.Orders, FilterTarget.Customers };
        private LruCache<string, GoogleResponse> imageCache = new LruCache<string, GoogleResponse>(100);
        private IObserver<PersonAdapter> imageLoadingObserver;
        private bool isPanelShown = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="MapPanel"/> class.
        /// </summary>
        public MapPanel()
        {
            InitializeComponent();
            Load += OnMapPanelLoad;

            Messenger.Current.OfType<PanelShownMessage>()
                .Where(x => IsOwnedBy(x.Panel))
                .Where(_ => !isPanelShown)
                .Do(_ => isPanelShown = true)
                .Subscribe(_ => LoadImage(googleImage.Tag as PersonAdapter));

            Messenger.Current.OfType<PanelHiddenMessage>()
                .Where(x => IsOwnedBy(x.Panel))
                .Subscribe(_ => isPanelShown = false);

            GotFocus += OnGotFocus;
            LostFocus += OnLostFocus;
        }

        /// <summary>
        /// Handles lost focus event.  Used to set the panel as not shown.
        /// </summary>
        private void OnLostFocus(object sender, EventArgs e)
        {
            isPanelShown = false;
        }

        /// <summary>
        /// Handles lost got event.  Used to set the panel as shown and fetch the image.
        /// </summary>
        private void OnGotFocus(object sender, EventArgs e)
        {
            errorLabel.Text = "Loading...";
            isPanelShown = true;
            LoadImage(googleImage.Tag as PersonAdapter);
        }

        /// <summary>
        /// Gets or sets the type of the map.
        /// </summary>
        public MapPanelType MapType { get; set; }

        /// <summary>
        /// Load the state of the panel.
        /// </summary>
        public void LoadState()
        { }

        /// <summary>
        /// Save the state of the panel.
        /// </summary>
        public void SaveState()
        { }

        /// <summary>
        /// Indicates if the panel can handle multiple selected items at one time.
        /// </summary>
        public bool SupportsMultiSelect => false;

        /// <summary>
        /// Change the content of the panel based on the given keys.
        /// </summary>
        /// <param name="selection"></param>
        public Task ChangeContent(IGridSelection selection) => Task.FromResult(true);

        /// <summary>
        /// Gets the size of the picture - Since max size is 640, panelSize is over 640, the largest possible size of the same aspect ratio is returned.
        /// </summary>
        public static Size GetPictureSize(Size panelSize)
        {
            if (panelSize.Width <= 640 && panelSize.Height <= 640)
            {
                return panelSize;
            }

            Size returnSize = new Size(640, 640);
            if (panelSize.Width > panelSize.Height)
            {
                returnSize.Height = (int) (640 * ((double) panelSize.Height / panelSize.Width));
            }
            else
            {
                returnSize.Width = (int) (640 * ((double) panelSize.Width / panelSize.Height));
            }
            return returnSize;
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
        public EntityType EntityType => EntityType.OrderEntity;

        /// <summary>
        /// The supported filter targets that the panel can display for.
        /// </summary>
        public FilterTarget[] SupportedTargets => supportedTargets;

        /// <summary>
        /// Refresh the existing selected content by requerying for the relevant keys to ensure an up-to-date related row
        /// list with up-to-date displayed entity content.
        /// </summary>
        public Task ReloadContent() => TaskUtility.CompletedTask;

        /// <summary>
        /// Refresh the existing displayed content.  Does not try to reset or look for new\deleted rows - just refreshes
        /// the known existing rows and their known corresponding entities.
        /// </summary>
        public Task UpdateContent() => TaskUtility.CompletedTask;

        /// <summary>
        /// Map panel has loaded
        /// </summary>
        private void OnMapPanelLoad(object sender, EventArgs e)
        {
            BuildOrderSelectionChangingHandler();
            BuildOrderSelectionChangedHandler();
            BuildImageLoadListener();
        }

        /// <summary>
        /// Build the handler for order selection changing messages
        /// </summary>
        private void BuildOrderSelectionChangingHandler()
        {
            Messenger.Current
                .OfType<OrderSelectionChangingMessage>()
                .Subscribe(x =>
                {
                    googleImage.Tag = null;
                    googleImage.Visible = false;
                    errorLabel.Text = string.Empty;
                });
        }

        /// <summary>
        /// Build the handler for order selection changed messages
        /// </summary>
        private void BuildOrderSelectionChangedHandler()
        {
            Messenger.Current
                .OfType<OrderSelectionChangedMessage>()
                .Where(x => x.LoadedOrderSelection.CompareCountTo(1) == ComparisonResult.Equal)
                .Select(x => x.LoadedOrderSelection.Single())
                .OfType<LoadedOrderSelection>()
                .Where(x => x.Order != null)
                .Select(x => new PersonAdapter(x.Order, "Ship").CopyToNew())
                .Subscribe(LoadImage);
        }

        /// <summary>
        /// Build listener for loading images
        /// </summary>
        private void BuildImageLoadListener()
        {
            Observable.Create<PersonAdapter>(x => SaveGetImageObserver(x))
                .Where(x => x != null)
                .Select(x => new { Address = x, Size })
                .Throttle(TimeSpan.FromMilliseconds(200))
                .SelectMany(x => Observable.FromAsync(() => GetImage(x.Address, x.Size)))
                .ObserveOn(new SchedulerProvider(() => Program.MainForm).WindowsFormsEventLoop)
                .Subscribe(x => DisplayResult(x));
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

            LoadImage(googleImage.Tag as PersonAdapter);
        }

        /// <summary>
        /// Called when [Google image click].
        /// </summary>
        private void OnGoogleImageClick(object sender, EventArgs e)
        {
            var address = (sender as PictureBox)?.Tag as PersonAdapter;
            if (address == null)
            {
                return;
            }

            string url = string.Format("https://www.google.com/maps/place/{0}+{1}+{2}+{3}+{4}",
                address.Street1,
                address.City,
                address.StateProvCode,
                address.PostalCode,
                address.CountryCode);

            Process.Start(url);
        }

        /// <summary>
        /// Find the parent dock control of the given control
        /// </summary>
        private DockControl FindParentDockControl(Control control)
        {
            Control parent = control.Parent;
            if (parent == null)
            {
                return null;
            }

            return parent as DockControl ?? FindParentDockControl(parent);
        }

        /// <summary>
        /// Save the getImageObserver
        /// </summary>
        private IDisposable SaveGetImageObserver(IObserver<PersonAdapter> getImageObserver)
        {
            imageLoadingObserver = getImageObserver;
            return Disposable.Create(() => imageLoadingObserver = null);
        }

        /// <summary>
        /// Display the result
        /// </summary>
        private void DisplayResult(GoogleResponse googleResponse)
        {
            googleImage.Tag = googleResponse.Address;

            if (googleResponse.ReturnedImage != null)
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
        }

        /// <summary>
        /// Gets the image from Google.
        /// </summary>
        private async Task<GoogleResponse> GetImage(PersonAdapter person, Size size)
        {
            if (person == null)
            {
                return null;
            }

            // We need to keep the requested person adapter around so that got/lost focus can
            // get the image if it hasn't already been downloaded.
            googleImage.Tag = person;

            Size adjustedSize = GetPictureSize(size);

            string hash = GetAddressHash(person, adjustedSize);

            return imageCache[hash] ??
                await GetImageFromGoogle(person, adjustedSize, hash);
        }

        /// <summary>
        /// Get a hash for the requested address and size
        /// </summary>
        private static string GetAddressHash(PersonAdapter person, Size size)
        {
            string hashValue = string.Join("|", person.Street1, person.City,
                person.StateProvCode, size.Width.ToString(), size.Height.ToString());
            return new StringHash().Hash(hashValue, "somesalt");
        }

        /// <summary>
        /// Get the requested image from Google
        /// </summary>
        private async Task<GoogleResponse> GetImageFromGoogle(PersonAdapter person, Size size, string hash)
        {
            GoogleResponse response;
            response = ShouldLoadImageFromGoogle() ? 
                await LoadImageFromGoogle(person, size) : 
                new GoogleResponse();

            if (!response.IsThrottled && response.ReturnedImage != null)
            {
                imageCache[hash] = response;
            }

            response.Address = person;

            return response;
        }

        /// <summary>
        /// Returns true if we know the panel is visible.
        /// </summary>
        private bool ShouldLoadImageFromGoogle()
        {
            if (isPanelShown)
            {
                return true;
            }

            DockControl dockControl = GetDockControl(this);

            return (dockControl?.DockSituation ?? DockSituation.None) != DockSituation.None;
        }

        /// <summary>
        /// Gets the dock control.
        /// </summary>
        private DockControl GetDockControl(Control control)
        {
            if (control == null)
            {
                return null;
            }

            DockControl dockControl = control as DockControl;
            if (dockControl != null)
            {
                return dockControl;
            }

            return GetDockControl(control.Parent);
        }

        /// <summary>
        /// Load an image from Google for the address and size
        /// </summary>
        private async Task<GoogleResponse> LoadImageFromGoogle(PersonAdapter addressAdapter, Size size)
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
                    return new GoogleResponse { ReturnedImage = Image.FromStream(stream) };
                }
            }
            catch (WebException ex)
            {
                return new GoogleResponse
                {
                    IsThrottled = ((HttpWebResponse) ex.Response).StatusCode == HttpStatusCode.Forbidden
                };
            }
        }

        /// <summary>
        /// Start loading an image for the given address
        /// </summary>
        private void LoadImage(PersonAdapter person) => imageLoadingObserver?.OnNext(person);

        /// <summary>
        /// Is the panel associated with the message a parent of this control?
        /// </summary>
        private bool IsOwnedBy(DockControl panel) => panel == FindParentDockControl(this);

        /// <summary>
        /// Gets the URL.
        /// </summary>
        private string GetImageUrl()
        {
            return MapType == MapPanelType.Satellite ?
                "http://maps.google.com/maps/api/staticmap?center={0}+{1}+{2}&zoom=18&size={3}x{4}&maptype=hybrid&sensor=false&markers=size:medium%7Ccolor:blue%7C{0}+{1}+{2}" :
                "http://maps.googleapis.com/maps/api/streetview?size={3}x{4}&location={0}+{1}+{2}&fov=120&heading=235&pitch=10&sensor=false";
        /// Refresh the existing selected content by requerying for the relevant keys to ensure an up-to-date related row 
        }

        /// <summary>
        /// The response we get back from Google.
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
            /// Gets the address associated with the result
            /// </summary>
            public PersonAdapter Address { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether this instance is throttled.
            /// </summary>
            public bool IsThrottled { get; set; }
        }

    }
}

