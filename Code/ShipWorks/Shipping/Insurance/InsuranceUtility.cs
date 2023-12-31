﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using Autofac;
using Interapptive.Shared;
using Interapptive.Shared.Business.Geography;
using Interapptive.Shared.Net;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using log4net;
using RestSharp;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Licensing.Warehouse;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.Endicia;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Insurance.RatesModel;
using ShipWorks.Shipping.Settings;
using ShipWorks.Stores;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.ShipEngine;

namespace ShipWorks.Shipping.Insurance
{
	/// <summary>
	/// Used for helpful insurance methods
	/// </summary>
	public static class InsuranceUtility
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(InsuranceUtility));
		private static readonly string countryExclusionsFile = Path.Combine(DataPath.SharedSettings, @"Insurance\insuranceExcludedCountries.xml");
		private static List<string> excludedCountries = new List<string>();

		// Anytime our ShipWorks rates change and the code to calculate rates is updated, this should be increased along with our version.php
		private const int shipWorksRateCalculateVersion = 1;

		// Anytime any carrier's rates change and the code to calculate them changes, this should be increased along with our version.php
		private const int carrierRateCalculationVersion = 1;

		// The actual rates that are current - may be larger than the ones the code knows about
		private static int shipWorksRateActualVersion = shipWorksRateCalculateVersion;
		private static int carrierRateActualVersion = carrierRateCalculationVersion;

		private static bool areRatesDefault = true;

		/// <summary>
		/// The URL to the customer agreement file online
		/// </summary>
		public static string OnlineCustomerAgreementFile => "https://www.insureship.com/policies/shipworks.txt";

		/// <summary>
		/// The URL toe the excluded country list online
		/// </summary>
		public static string OnlineExcludedCountriesFile =>
			"https://www.interapptive.com/insurance/insuranceExcludedCountries.xml";

		private static InsuranceRates InsuranceRates { get; set; } = new InsuranceRates();

		/// <summary>
		/// Configure anything about our insurance display and interaction based on online data about rate tables, countries, etc.
		/// </summary>
		public static void ConfigureInsurance(int shipWorksRateVersion, int carrierRateVersion, int countryVersion, int bannerVersion)
		{
			shipWorksRateActualVersion = shipWorksRateVersion;
			carrierRateActualVersion = carrierRateVersion;

			LoadExcludedCountries(countryVersion);
		}

		/// <summary>
		/// Get the default insurance value to use based on the shipment contents
		/// </summary>
		public static decimal GetInsuranceValue(ShipmentEntity shipment)
		{
			return GetInsuranceValue(shipment, InsuranceInitialValueSource.ItemSubtotal, 0);
		}

		/// <summary>
		/// Get the default insurance value to use based on the shipment contents and value source
		/// </summary>
		public static decimal GetInsuranceValue(ShipmentEntity shipment, InsuranceInitialValueSource source, decimal? otherAmount)
		{
			switch (source)
			{
				case InsuranceInitialValueSource.OrderTotal:
					return OrderUtility.CalculateTotal(shipment.Order, true);

				case InsuranceInitialValueSource.ItemSubtotal:
					return OrderUtility.CalculateTotal(shipment.Order, false);

				case InsuranceInitialValueSource.OtherAmount:
				default:
					return otherAmount ?? 0;
			}
		}

		/// <summary>
		/// Determines how much the shipment was insured for.
		/// </summary>
		public static decimal GetInsuredValue(ShipmentEntity shipment)
		{
			ShipmentType shipmentType = ShipmentTypeManager.GetType(shipment);

			return
				Enumerable.Range(0, shipmentType.GetParcelCount(shipment))
					.Select(parcelIndex => shipmentType.GetParcelDetail(shipment, parcelIndex).Insurance)
					.Where(
						choice =>
							choice.Insured && choice.InsuranceProvider == InsuranceProvider.ShipWorks &&
							choice.InsuranceValue > 0)
					.Select(insuredPackages => insuredPackages.InsuranceValue)
					.DefaultIfEmpty(0)
					.Sum();
		}

		/// <summary>
		/// Check for any insurance issues in the given list
		/// </summary>
		[NDependIgnoreLongMethodAttribute]
		public static async Task ValidateShipment(ShipmentEntity shipment, IAsyncMessageHelper messageHelper)
		{
			StoreEntity store = StoreManager.GetStore(shipment.Order.StoreID);

			if (store == null)
			{
				throw new ShippingException("The store for the shipment has gone away.");
			}

			ShipmentType shipmentType = ShipmentTypeManager.GetType(shipment);

			// Trying to insure
			if (shipment.Insurance && shipment.InsuranceProvider == (int) InsuranceProvider.ShipWorks)
			{
				List<IInsuranceChoice> insuranceChoices =
					Enumerable.Range(0, shipmentType.GetParcelCount(shipment))
					.Select(parcelIndex => shipmentType.GetParcelDetail(shipment, parcelIndex).Insurance)
					.Where(choice => choice.Insured).ToList();

				using (var lifetimeScope = IoC.BeginLifetimeScope())
				{
					var license = lifetimeScope.Resolve<ILicenseService>().GetLicense(store);
					if (license.TrialDetails.IsInTrial)
					{
						throw new ShippingException(
							"ShipWorks Insurance is not available during the ShipWorks trial period. Please go to https://hub.shipworks.com/account to add a credit card to your account.");
					}
				}

				ShipWorksLicense shipWorksLicense = new ShipWorksLicense(store.License);

				if (!shipWorksLicense.IsMetered)
				{
					Debug.Fail("Should never get here on a non-metered license. The ShippingManager should have already validated that.");
					throw new InvalidOperationException("Cannot insure on a non-metered license.");
				}

				if (IsExcludedCountry(shipment.ShipCountryCode))
				{
					throw new ShippingException(
						"Shipments cannot be insured by ShipWorks Insurance when the destination is " +
						"on the restricted country list.\n\n" +
						"Restricted country: " + Geography.GetCountryName(shipment.ShipCountryCode));
				}

				// Restriction since tango isn't setup to handle it yet
				if (insuranceChoices.Count(choice => choice.InsuranceValue > 0) > 1)
				{
					throw new ShippingException("Multi-package shipments cannot be insured using ShipWorks. " +
				   "To insure multiple packages, create a shipment for each package.");
				}

				if (insuranceChoices.Any(choice => choice.InsuranceValue > 5000))
				{
					throw new ShippingException("ShipWorks Insurance can only insure parcels up to $5000");
				}

				ShippingSettingsEntity settings = ShippingSettings.Fetch();

				// They have to agree to the insurance agreement
				if (string.IsNullOrWhiteSpace(settings.InsurancePolicy))
				{
					bool result = await messageHelper.ShowDialog(() => new InsuranceAgreementDlg()).ConfigureAwait(true) == DialogResult.OK;

					if (result)
					{
						settings.InsurancePolicy = TangoWebClient.GenerateInsurancePolicyNumber(store);
						settings.InsuranceLastAgreed = DateTime.UtcNow;

						ShippingSettings.Save(settings);
					}
					else
					{
						throw new ShippingException("You must agree to the ShipWorks Insurance agreement to continue.");
					}
				}
			}
		}

		public static void GetInsuranceRates()
		{
			Task.Factory.StartNew(() =>
			{
				log.Info($"Start retrieving insurance rates");
				Stopwatch watch = Stopwatch.StartNew();

				using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
				{
					IWarehouseRequestClient warehouseRequestClient = lifetimeScope.Resolve<IWarehouseRequestClient>();

					var request = new RestRequest(WarehouseEndpoints.GetRates, Method.GET);
					var result = warehouseRequestClient.MakeRequest(request, "GetInsuranceRates").Result;

					if (result.Success && (!result.Value?.Content?.IsNullOrWhiteSpace() ?? false))
					{
						InsuranceRates = PlatformHelper.JsonConvertToDto<InsuranceRates>(result.Value.Content);
						areRatesDefault = false;
						log.Info($"Insurance rates retrieved and set correctly");
					}
					else
					{
						log.Error("Could not retrieve insurance rates correctly. Using default values instead.");
					}
				}

				watch.Stop();
				log.Info($"Insurance rates retrieving finished in {watch.ElapsedMilliseconds}ms");
			});
		}

		/// <summary>
		/// Get the cost of insurance for the given shipment given the specified declared value
		/// </summary>
		public static InsuranceCost GetInsuranceCost(ShipmentEntity originalShipment, decimal declaredValue)
		{
			InsuranceCost cost = new InsuranceCost();

			if (areRatesDefault)
			{
				log.Warn("Only default rates are loaded currently, trying to retrieve them again.");
				GetInsuranceRates();
			}

			// Check declared value
			if (declaredValue > 5000)
			{
				cost.AddInfoMessage("ShipWorks Insurance can only cover up to $5000 in declared value.");
				return cost;
			}

			var shipment = GetShipmentWithStoreSpecificAddressIfNecessary(originalShipment);

			// Check country
			try
			{
				if (IsExcludedCountry(shipment.ShipCountryCode))
				{
					cost.AddInfoMessage(
							"Shipments cannot be insured by ShipWorks Insurance when the destination is " +
							"on the restricted country list.\n\n" +
							"Restricted country: " + Geography.GetCountryName(shipment.ShipCountryCode));

					return cost;
				}
			}
			catch (ShippingException ex)
			{
				log.WarnFormat("Couldn't check excluded country", ex);
			}

			// Make sure there aren't new rates we don't know about
			if (shipWorksRateCalculateVersion == shipWorksRateActualVersion)
			{
				FillInShipWorksCost(cost, shipment, declaredValue);

				if (carrierRateCalculationVersion == carrierRateActualVersion)
				{
					FillInCarrierCost(cost, shipment, declaredValue);
				}
				else
				{
					cost.AddInfoMessage("The carrier rates have changed.  Please update your version of ShipWorks\nto see how much you save using ShipWorks insurance.");
				}
			}
			else
			{
				cost.AddInfoMessage("ShipWorks insurance rates have changed.  Please update your version of ShipWorks\nto see the rate for this shipment.");
			}

			return cost;
		}

		/// <summary>
		/// Get the shipment with the address updated if necessary
		/// </summary>
		private static ShipmentEntity GetShipmentWithStoreSpecificAddressIfNecessary(ShipmentEntity shipment)
		{
			using (var lifetimeScope = IoC.BeginLifetimeScope())
			{
				var storeTypeManager = lifetimeScope.Resolve<IStoreTypeManager>();
				var storeType = storeTypeManager.GetType(shipment);

				var shipmentToUse = storeType.WillOverrideShipmentDetailsChangeShipment(shipment) ? EntityUtility.CloneEntity(shipment) : shipment;
				storeType.OverrideShipmentDetails(shipmentToUse);

				return shipmentToUse;
			}
		}

		/// <summary>
		/// Indicates if the given country is in the list of excluded countries
		/// </summary>
		private static bool IsExcludedCountry(string country)
		{
			LoadExcludedCountries(null);

			lock (excludedCountries)
			{
				if (excludedCountries.Count == 0)
				{
					throw new ShippingException("The list of countries excluded from ShipWorks Insurance failed to load.  Please restart ShipWorks and try again.");
				}

				return excludedCountries.Any(excluded => Geography.GetCountryCode(excluded) == Geography.GetCountryCode(country));
			}
		}

		/// <summary>
		/// Get the shipworks cost for the given shipment with the specified declared value
		/// </summary>
		private static void FillInShipWorksCost(InsuranceCost cost, ShipmentEntity shipment, decimal declaredValue)
		{
			decimal adjustedValue = declaredValue;
			decimal rate;

			ShipmentTypeCode shipmentType = (ShipmentTypeCode) shipment.ShipmentType;

			switch (shipmentType)
			{
				case ShipmentTypeCode.AmazonSFP:
					FillInShipWorksCostForAmazon(shipmentType, cost, shipment, declaredValue);
					return;
				case ShipmentTypeCode.UpsWorldShip:
				case ShipmentTypeCode.UpsOnLineTools:
				case ShipmentTypeCode.FedEx:
				case ShipmentTypeCode.DhlEcommerce:
				case ShipmentTypeCode.OnTrac:
				case ShipmentTypeCode.iParcel:
					{
						// We can hardcode to just look at the first parcel in the shipment - all parcels in a shipment will have the same pennyone setting
						bool pennyOne = ShipmentTypeManager.GetType(shipment).GetParcelDetail(shipment, 0).Insurance.InsurancePennyOne.Value;

						if (!pennyOne)
						{
							cost.AdvertisePennyOne = true;

							string carrierName = ShippingManager.GetCarrierName(shipmentType);

							cost.AddInfoMessage(string.Format("The first $100 of coverage is provided by {0}. Learn how to add protection\nfor the first $100 in the Shipping Settings for {0}.", carrierName));

							if (declaredValue > 0 && declaredValue <= 100)
							{
								cost.AddInfoMessage(string.Format("No ShipWorks Insurance coverage will be provided on this shipment\nsince it will be provided by {0}.", carrierName));

								return;
							}

							adjustedValue = Math.Max(declaredValue - 100, 0);
						}

						if (shipmentType == ShipmentTypeCode.iParcel)
						{
							rate = InsuranceRates.ShipworksRates.iParcelCost;
						}
						else
						{
							rate = GetFedExUpsRate(shipment);
						}
					}
					break;
				case ShipmentTypeCode.Express1Endicia:
				case ShipmentTypeCode.Express1Usps:
				case ShipmentTypeCode.PostalWebTools:
				case ShipmentTypeCode.Endicia:
				case ShipmentTypeCode.Usps:
					{
						rate = GetUspsRate(shipment);
					}
					break;

				default:
					rate = GetOthersRate(shipment);
					break;
			}

			if (adjustedValue <= 0)
			{
				cost.ShipWorks = null;
				cost.AddInfoMessage("No ShipWorks Insurance coverage will be provided on this shipment\nsince it is valued at $0.00.");

				return;
			}

			// Get increments of $100
			int quantity = (int) Math.Ceiling(adjustedValue / 100m);

			// Set the shipworks cost
			cost.ShipWorks = quantity * rate;
			cost.ShipWorksRate = rate;
		}

		/// <summary>
		/// Get the shipworks cost for the given shipment with the specified declared value
		/// </summary>
		private static void FillInShipWorksCostForAmazon(ShipmentTypeCode shipmentType, InsuranceCost cost, ShipmentEntity shipment, decimal declaredValue)
		{
			if (shipmentType != ShipmentTypeCode.AmazonSFP)
			{
				throw new ShippingException("The shipment is not an Amazon shipment.");
			}

			decimal adjustedValue = declaredValue;
			decimal rate;

			if (string.IsNullOrEmpty(shipment.AmazonSFP?.CarrierName))
			{
				return;
			}

			cost.AdvertisePennyOne = false;
			if (shipment.AmazonSFP.CarrierName == "STAMPS_DOT_COM" || shipment.AmazonSFP.CarrierName == "USPS")
			{
				rate = GetUspsRate(shipment);
			}
			else
			{
				// FedEx or UPS
				if (declaredValue > 0 && declaredValue <= 100)
				{
					cost.AddInfoMessage($"No ShipWorks Insurance coverage will be provided on this shipment\nsince it will be provided by the carrier.");
					return;
				}

				adjustedValue = Math.Max(declaredValue - 100, 0);
				rate = GetFedExUpsRate(shipment);
			}

			if (adjustedValue <= 0)
			{
				cost.ShipWorks = null;
				cost.AddInfoMessage("No ShipWorks Insurance coverage will be provided on this shipment\nsince it is valued at $0.00.");

				return;
			}

			// Get increments of $100
			int quantity = (int) Math.Ceiling(adjustedValue / 100m);

			// Set the shipworks cost
			cost.ShipWorks = quantity * rate;
		}

		/// <summary>
		/// Gets the usps rate based on country
		/// </summary>
		private static decimal GetUspsRate(ShipmentEntity shipment)
					=> shipment.ShipPerson.IsDomesticCountry() ? InsuranceRates.ShipworksRates.UspsDomesticCost : InsuranceRates.ShipworksRates.UspsInternationalCost;
		
		private static decimal GetFedExUpsRate(ShipmentEntity shipment)
							=> shipment.ShipPerson.IsDomesticCountry() ? InsuranceRates.ShipworksRates.FedExUpsDomesticCost : InsuranceRates.ShipworksRates.FedExUpsInternationalCost;
		
		private static decimal GetOthersRate(ShipmentEntity shipment)
							=> shipment.ShipPerson.IsDomesticCountry() ? InsuranceRates.ShipworksRates.OtherDomesticCost : InsuranceRates.ShipworksRates.OtherInternationalCost;

		/// <summary>
		/// Get the native carrier cost of the give shipment with the specified declared value
		/// </summary>
		private static void FillInCarrierCost(InsuranceCost cost, ShipmentEntity shipment, decimal declaredValue)
		{
			ShipmentTypeCode shipmentType = (ShipmentTypeCode) shipment.ShipmentType;

			switch (shipmentType)
			{
				case ShipmentTypeCode.UpsWorldShip:
				case ShipmentTypeCode.UpsOnLineTools:
				case ShipmentTypeCode.FedEx:
					{
						cost.Carrier = CalculateUpsFedExCost(declaredValue);
					}
					break;
				case ShipmentTypeCode.OnTrac:
					{
						cost.Carrier = CalculateOnTracCost(declaredValue);
					}
					break;

				case ShipmentTypeCode.Express1Endicia:
				case ShipmentTypeCode.Express1Usps:
				case ShipmentTypeCode.PostalWebTools:
					{
						cost.Carrier = CalculatePostalCost(declaredValue, shipment.ShipPerson.IsDomesticCountry(), (PostalServiceType) shipment.Postal.Service);
					}
					break;

				case ShipmentTypeCode.Endicia:
					{
						if (EndiciaUtility.IsEndiciaInsuranceActive)
						{
							// We don't want to compare our rates to theres
							cost.Carrier = null;
						}
						else
						{
							cost.Carrier = CalculatePostalCost(declaredValue, shipment.ShipPerson.IsDomesticCountry(), (PostalServiceType) shipment.Postal.Service);
						}
					}
					break;

				case ShipmentTypeCode.Usps:
					{
						cost.Carrier = UspsUtility.IsStampsInsuranceActive ?
							null :
							CalculatePostalCost(declaredValue, shipment.ShipPerson.IsDomesticCountry(), (PostalServiceType) shipment.Postal.Service);
					}
					break;

				case ShipmentTypeCode.AmazonSFP:
				default:

					// Unknown for other
					cost.Carrier = null;
					break;

			}
		}

		/// <summary>
		/// Determine the cost of insurance for UPS, FedEx, and OnTrac based on the given declared value
		/// </summary>
		private static decimal CalculateUpsFedExCost(decimal declaredValue)
		{
			// Anything less than $100 is free
			if (declaredValue <= 100)
			{
				return 0;
			}
			else
			{
				// Get how many increments of $100
				int quantity = (int) Math.Ceiling(declaredValue / 100m);

				// It's .75 per 100, but a minimum of 2.25
				return (decimal) Math.Max(InsuranceRates.CarrierRates.FedExUpsMinimumCost, quantity * InsuranceRates.CarrierRates.FedExUpsRatePer100);
			}
		}

		/// <summary>
		/// Determine the cost of insurance for OnTrac based on the given declared value
		/// </summary>
		private static decimal CalculateOnTracCost(decimal declaredValue)
		{
			// Anything less than $101 is free
			if (declaredValue < 101)
			{
				return 0;
			}

			// Get how many increments of $100
			// This one is different from the other calculations because the $100 increments
			// start at $101 instead of $100
			int quantity = (int) Math.Floor((declaredValue - 101) / 100m) + 1;

			// It's rate per 100, but a minimum of 0
			return Math.Max(InsuranceRates.CarrierRates.OnTracMinimumCost, quantity * InsuranceRates.CarrierRates.OnTracRatePer100);
		}

		/// <summary>
		/// Calculate the cost of USPS insurance
		/// </summary>
		private static decimal? CalculatePostalCost(decimal declaredValue, bool isDomesticCountry, PostalServiceType postalService)
		{
			if (isDomesticCountry)
			{
				if (declaredValue <= 50)
				{
					return InsuranceRates.CarrierRates.Usps.CostFor50;
				}
				if (declaredValue <= 100)
				{
					return InsuranceRates.CarrierRates.Usps.CostFor100;
				}
				if (declaredValue <= 200)
				{
					return InsuranceRates.CarrierRates.Usps.CostFor200;
				}
				if (declaredValue <= 300)
				{
					return InsuranceRates.CarrierRates.Usps.CostFor300;
				}
				if (declaredValue <= 400)
				{
					return InsuranceRates.CarrierRates.Usps.CostFor400;
				}
				if (declaredValue <= 500)
				{
					return InsuranceRates.CarrierRates.Usps.CostFor500;
				}
				if (declaredValue <= 600)
				{
					return InsuranceRates.CarrierRates.Usps.CostFor600;
				}
				decimal adjustedValue = declaredValue - 600m;

				// Get how many increments of $100
				int quantity = (int) Math.Ceiling(adjustedValue / 100);

				// $1.85 per $100
				return InsuranceRates.CarrierRates.Usps.CostFor600 + (quantity * InsuranceRates.CarrierRates.Usps.RatePer100);
			}

			if (postalService == PostalServiceType.InternationalPriority ||
				postalService == PostalServiceType.GlobalPostStandardIntl ||
				postalService == PostalServiceType.GlobalPostSmartSaverStandardIntl ||
				postalService == PostalServiceType.GlobalPostPlus ||
				postalService == PostalServiceType.GlobalPostPlusSmartSaver)
			{
				if (declaredValue <= 200)
				{
					return 0;
				}

				decimal adjustedValue = declaredValue - 300m;

				// Get how many increments of $100
				int quantity = (int) Math.Ceiling(adjustedValue / 100);
					
				// $3.4 per $100
				return InsuranceRates.CarrierRates.Usps.InternationalCostFor300 + (quantity * InsuranceRates.CarrierRates.Usps.InternationalRatePer100);
			}

			// Too complicated to calculate
			return null;
		}

		/// <summary>
		/// Load the list of ShipWorks excluded countries
		/// </summary>
		private static void LoadExcludedCountries(int? countryVersion)
		{
			lock (excludedCountries)
			{
				try
				{
					if (!CountryExclusionsFileIsValid())
					{
						Directory.CreateDirectory(Path.GetDirectoryName(countryExclusionsFile));

						using (WebClient webClient = new WebClient())
						{
							webClient.DownloadFile(OnlineExcludedCountriesFile, countryExclusionsFile);
						}
					}

					XElement xCountries = XElement.Parse(File.ReadAllText(countryExclusionsFile));

					// If we know the country version, and the version doesn't match we need to download an updated file
					if (countryVersion != null && (int) xCountries.Attribute("version") != countryVersion)
					{
						File.Delete(countryExclusionsFile);

						using (WebClient webClient = new WebClient())
						{
							webClient.DownloadFile(OnlineExcludedCountriesFile, countryExclusionsFile);
						}

						xCountries = XElement.Parse(File.ReadAllText(countryExclusionsFile));
					}

					excludedCountries.AddRange(xCountries.XPathSelectElements("//Country/Code").Select(xElement => (string) xElement));
				}
				catch (IOException ex)
				{
					log.Error("Error loading excluded countries.", ex);
				}
				catch (UnauthorizedAccessException ex)
				{
					log.Error("Error loading excluded countries.", ex);
				}
				catch (Exception ex)
				{
					if (WebHelper.IsWebException(ex))
					{
						log.Error("Error loading excluded countries.", ex);
					}

					throw;
				}
			}
		}

		/// <summary>
		/// Determines if the country exclusions file exists and can be successfully parsed by XElement.
		/// </summary>
		/// <returns>True if the file exists and does not throw an XmlException on XElement.Parse</returns>
		private static bool CountryExclusionsFileIsValid()
		{
			if (!File.Exists(countryExclusionsFile))
			{
				return false;
			}

			try
			{
				XElement xCountries = XElement.Parse(File.ReadAllText(countryExclusionsFile));
			}
			catch (XmlException ex)
			{
				log.Error("The insurance country exclusions file had invalid xml.  Rebuilding.", ex);
				return false;
			}

			return true;
		}
	}
}