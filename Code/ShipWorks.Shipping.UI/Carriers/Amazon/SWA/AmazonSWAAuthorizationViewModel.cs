using System;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Net;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Core.UI;
using ShipWorks.Shipping.ShipEngine;
using ShipWorks.Stores.Platforms.Platform;

namespace ShipWorks.Shipping.UI.Amazon.SWA
{
	/// <summary>
	/// Amazon SWA Authorization Control
	/// </summary>
	[Component]
	public class AmazonSWAAuthorizationViewModel : INotifyPropertyChanged, IAmazonSWAAuthorizationViewModel
	{
		private readonly Func<IWin32Window> windowFactory;
		private readonly IShipEngineWebClient shipEngineClient;

		public event PropertyChangedEventHandler PropertyChanged;
		private readonly PropertyChangedHandler handler;
		private string accessCode;
		private ILog log;
		private readonly IMessageHelper messageHelper;
		private readonly IHubOrderSourceClient hubOrderSourceClient;
		private readonly IWebHelper webHelper;

		/// <summary>
		/// Constructor
		/// </summary>
		public AmazonSWAAuthorizationViewModel(
			Func<IWin32Window> windowFactory,
			IShipEngineWebClient shipEngineClient,
			IMessageHelper messageHelper, Func<Type, ILog> logFactory, IHubOrderSourceClient hubOrderSourceClient, IWebHelper webHelper)
		{
			this.windowFactory = windowFactory;
			this.shipEngineClient = shipEngineClient;
			handler = new PropertyChangedHandler(this, () => PropertyChanged);

			GetAccessCodeCommand = new RelayCommand(async () => await GetAccessCode().ConfigureAwait(true));

			this.messageHelper = messageHelper;
			this.hubOrderSourceClient = hubOrderSourceClient;
			this.webHelper = webHelper;
		}

		/// <summary>
		/// Gets or sets the access code.
		/// </summary>
		[Obfuscation(Exclude = true)]
		public string AccessCode
		{
			get { return accessCode; }
			set { handler.Set(nameof(AccessCode), ref accessCode, value); }
		}

		/// <summary>
		/// Command to GetAccessCode
		/// </summary>
		[Obfuscation(Exclude = true)]
		public ICommand GetAccessCodeCommand { get; }

		/// <summary>
		/// Use the access code to connect to amazon shipping
		/// </summary>
		/// <returns></returns>
		public GenericResult<string> ConnectToAmazonShipping()
		{
			try
			{
				var data = Convert.FromBase64String(accessCode);
				var decodedString = Encoding.UTF8.GetString(data);
				var splitString = decodedString.Split('_');

				if (splitString.Length == 3)
				{
					return GenericResult.FromSuccess(splitString[2]);
				}

				log.Error("The provided Base64 string did not have 3 sections separated by the '_' character.");
				return GenericResult.FromError<string>("Token is not valid.");

			}
			catch (Exception e)
			{
				log.Error(e);
				return GenericResult.FromError<string>("Unknown error occurred");
			}
			//return await shipEngineClient.ConnectAmazonShippingAccount(accessCode);
		}

		/// <summary>
		/// Opens a browser window which will lead the user to the access token
		/// </summary>
		private Task GetAccessCode()
		{
			return InitiateMonoauth(async () => await hubOrderSourceClient.GetCreateCarrierInitiateUrl("amazon_shipping_us", "US").ConfigureAwait(true));

			//string authorizationUrl = $"https://www.interapptive.com/amazon/shipping.subscribe.html";
			//         WebHelper.OpenUrl(authorizationUrl, windowFactory());
		}


		/// <summary>
		/// Given a function to get the URL that kicks off the Monoauth process, open the URL in the browser
		/// </summary>
		private async Task InitiateMonoauth(Func<Task<String>> getUrl)
		{
			try
			{
				webHelper.OpenUrl(await getUrl().ConfigureAwait(false));
			}
			catch (ObjectDisposedException ex)
			{
				//User cancelled the dialog before we could open the browser
				//Do nothing
			}
			catch (Exception ex)
			{
				messageHelper.ShowError(ex.GetBaseException().Message);
			}
		}
	}
}
