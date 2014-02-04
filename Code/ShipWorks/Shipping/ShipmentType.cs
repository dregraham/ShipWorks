using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Editing;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.UI.Wizard;
using System.Windows.Forms;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Settings;
using ShipWorks.Data.Connection;
using ShipWorks.Shipping.Settings.Defaults;
using ShipWorks.Filters;
using ShipWorks.Shipping.Settings.Origin;
using ShipWorks.Stores;
using ShipWorks.Data;
using ShipWorks.Data.Model.HelperClasses;
using System.Reflection;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model;
using ShipWorks.Shipping.Tracking;
using Interapptive.Shared.Business;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Templates.Processing;
using ShipWorks.Templates.Processing.TemplateXml;
using ShipWorks.Templates.Processing.TemplateXml.ElementOutlines;
using ShipWorks.Shipping.Carriers.BestRate;

namespace ShipWorks.Shipping
{
	/// <summary>
	/// Base class that all shipment types\carriers must derive from
	/// </summary>
	public abstract class ShipmentType
	{
		/// <summary>
		/// The ShipmentTypeCode represented by this ShipmentType
		/// </summary>
		public abstract ShipmentTypeCode ShipmentTypeCode { get; }

		/// <summary>
		/// The user-displayable name of the shipment type
		/// </summary>
		public virtual string ShipmentTypeName
		{
			get
			{
				return EnumHelper.GetDescription(ShipmentTypeCode);
			}
		}

		/// <summary>
		/// Overridden to provide the name of the shipment type
		/// </summary>
		public override string ToString()
		{
			return ShipmentTypeName;
		}

		/// <summary>
		/// Indicates if the shipment service type supports getting rates
		/// </summary>
		public virtual bool SupportsGetRates
		{
			get { return false; }
		}

		/// <summary>
		/// Indicates if the shiopment service type supports return shipments
		/// </summary>
		public virtual bool SupportsReturns
		{
			get { return false; }
		}

		/// <summary>
		/// Supports using an origin address from a shipping account
		/// </summary>
		public virtual bool SupportsAccountAsOrigin
		{
			get { return false; }
		}

        /// <summary>
        /// Supports getting counter rates.
        /// </summary>
	    public virtual bool SupportsCounterRates
	    {
            get { return false; }
	    }

		/// <summary>
		/// Indicates if the ShipmentType needs the ResidentialResult field determined for the given shipment.
		/// </summary>
		public virtual bool IsResidentialStatusRequired(ShipmentEntity shipment)
		{
			return false;
		}

		/// <summary>
		/// Created specifically for WorldShip.  A WorldShip shipment is processed in two phases - first it's processed 
		/// in ShipWorks, then once its processed in WorldShip its completed.  Opted instead of hardcoding WorldShip if statements
		/// to use this instead so its easier to track down all the usgages by doing Find References on this property.
		/// </summary>
		public virtual bool ProcessingCompletesExternally
		{
			get { return false; }
		}

		/// <summary>
		/// Create the setup wizard form that will walk the user through setting up the shipment type.  Can return
		/// null if the shipment type does not require setup
		/// </summary>
		public virtual WizardForm CreateSetupWizard()
		{
			return null;
		}

		/// <summary>
		/// Creates the UserControl that is used to edit service options for the shipment type
		/// </summary>
		public abstract ServiceControlBase CreateServiceControl();

		/// <summary>
		/// Creates the UserControl taht is used to edit customs options
		/// </summary>
		public virtual CustomsControlBase CreateCustomsControl()
		{
			return new CustomsControlBase();
		}

		/// <summary>
		/// Creates the UserControl that is used to edit return shipments
		/// </summary>
		public virtual ReturnsControlBase CreateReturnsControl()
		{
			return new ReturnsControlBase();
		}

		/// <summary>
		/// Creates the UserControl that is used to edit the defaults\settings for the service
		/// </summary>
		public virtual SettingsControlBase CreateSettingsControl()
		{
			return null;
		}

		/// <summary>
		/// Create the UserControl that is used to edit a profile for the service
		/// </summary>
		public virtual ShippingProfileControlBase CreateProfileControl()
		{
			return null;
		}

		/// <summary>
		/// Ensures that the carrier specific data for the shipment, such as the FedEx data, are loaded for the shipment.  If the data
		/// already exists, nothing is done: it is not refreshed.  This method can throw SqlForeignKeyException if the root shipment
		/// or order has been deleted, ORMConcurrencyException if the shipment had been edited elsewhere, and ObjectDeletedException if the shipment
		/// had been deleted.
		/// </summary>
		public abstract void LoadShipmentData(ShipmentEntity shipment, bool refreshIfPresent);

		/// <summary>
		/// Apply the configured defaults and profile rule settings to the given shipment
		/// </summary>
		public virtual void ConfigureNewShipment(ShipmentEntity shipment)
		{
			shipment.ThermalType = null;

			// First apply the base profile
			ApplyProfile(shipment, GetPrimaryProfile());

			// Go through each additional profile and apply it as well
			foreach (ShippingDefaultsRuleEntity rule in ShippingDefaultsRuleManager.GetRules(ShipmentTypeCode))
			{
				ShippingProfileEntity profile = ShippingProfileManager.GetProfile(rule.ShippingProfileID);
				if (profile != null)
				{
					long? filterContentID = FilterHelper.GetFilterNodeContentID(rule.FilterNodeID);
					if (filterContentID != null)
					{
						if (FilterHelper.IsObjectInFilterContent(shipment.OrderID, filterContentID.Value))
						{
							ApplyProfile(shipment, profile);
						}
					}
				}
			}
		}

		/// <summary>
		/// Ensures that the carrier specific data for the given profile exists and is loaded
		/// </summary>
		public virtual void LoadProfileData(ShippingProfileEntity profile, bool refreshIfPresent)
		{

		}

		/// <summary>
		/// Save carrier specific profile data to the database.  Return true if anything was dirty and saved, or was deleted.
		/// </summary>
		public virtual bool SaveProfileData(ShippingProfileEntity profile, SqlAdapter adapter)
		{
			return false;
		}

		/// <summary>
		/// Create a profile with the default settings for the shipment type
		/// </summary>
		public ShippingProfileEntity GetPrimaryProfile()
		{
			ShippingProfileEntity profile = ShippingProfileManager.Profiles.FirstOrDefault(p =>
				p.ShipmentType == (int) ShipmentTypeCode && p.ShipmentTypePrimary);

			if (profile == null)
			{
				profile = new ShippingProfileEntity();
				profile.Name = string.Format("Defaults - {0}", ShipmentTypeName);
				profile.ShipmentType = (int) ShipmentTypeCode;
				profile.ShipmentTypePrimary = true;

				// Load the shipmentType specific profile data
				LoadProfileData(profile, true);

				// Configure it as a primary profile
				ConfigurePrimaryProfile(profile);

				// Save the profile
				ShippingProfileManager.SaveProfile(profile);
			}

			return profile;
		}

		/// <summary>
		/// Allows bases classes to apply the default settings to the given profile
		/// </summary>
		protected virtual void ConfigurePrimaryProfile(ShippingProfileEntity profile)
		{
			profile.OriginID = (int) ShipmentOriginSource.Store;

			profile.Insurance = false;
			profile.InsuranceInitialValueSource = (int) InsuranceInitialValueSource.ItemSubtotal;
			profile.InsuranceInitialValueAmount = 0;

			profile.ReturnShipment = false;
		}

		/// <summary>
		/// Ensures the ShipDate on an unprocessed shipment is Up-To-Date
		/// </summary>
		protected virtual void UpdateShipmentShipDate(ShipmentEntity shipment)
		{
			if (shipment == null)
			{
				throw new ArgumentNullException("shipment");
			}

			if (!shipment.Processed && shipment.ShipDate.Date < DateTime.Now.Date)
			{
				shipment.ShipDate = DateTime.Now.Date.AddHours(12);
			}
		}

		/// <summary>
		/// Update any data that could have changed dynamically or externally
		/// </summary>
		public virtual void UpdateDynamicShipmentData(ShipmentEntity shipment)
		{
			if (shipment.Processed)
			{
				InvalidOperationException ex = new InvalidOperationException("Cannot update dynamic data on a processed shipment.");
				ex.Data["UpdateDynamicData"] = true;

				throw ex;
			}

			// ensure the shipdate is up-to-date
			UpdateShipmentShipDate(shipment);

			// Ensure the from address is up-to-date
			if (!UpdateOriginAddress(shipment, shipment.OriginOriginID))
			{
				shipment.OriginOriginID = (long) ShipmentOriginSource.Other;
			}
		}

		/// <summary>
		/// Update the origin address of the shipment based on the given originID value.  If the shipment has already been processed, nothing is done.  If
		/// the originID is no longer valid and the address could not be updated, false is returned.
		/// </summary>
		public bool UpdateOriginAddress(ShipmentEntity shipment, long originID)
		{
			return UpdatePersonAddress(shipment, new PersonAdapter(shipment, "Origin"), originID);
		}

		/// <summary>
		/// Update the person address based on the given originID value.  If the shipment has already been processed, nothing is done.  If
		/// the originID is no longer valid and the address could not be updated, false is returned.
		/// </summary>
		public virtual bool UpdatePersonAddress(ShipmentEntity shipment, PersonAdapter person, long originID)
		{
			if (shipment.Processed)
			{
				return true;
			}

			// Copy from the store
			if (originID == (int) ShipmentOriginSource.Store)
			{
				StoreEntity store = StoreManager.GetStore(shipment.Order.StoreID);

				// Create an intermediate person to setup the source information, so we can copy it all at one time. If we dot it in stages, it can
				// look edited when it really shouldn't and cause problems with concurrency.
				PersonAdapter source = new PersonAdapter();
				PersonAdapter.Copy(store, "", source);

				// Store doesn't maintain a first\last name - so we need to create it from the StoreName
				PersonName name = PersonName.Parse(store.StoreName);

				// Apply the name to the source
				source.FirstName = name.First;
				source.MiddleName = name.Middle;
				source.LastName = name.LastWithSuffix;

				PersonAdapter.Copy(source, person);

				return true;
			}

			// Other - no change.
			if (originID == (int) ShipmentOriginSource.Other)
			{
				return true;
			}

			// Try looking it up as ShippingOriginID
			ShippingOriginEntity origin = ShippingOriginManager.GetOrigin(originID);
			if (origin != null)
			{
				PersonAdapter.Copy(origin, "", person);

				return true;
			}

			return false;
		}

		/// <summary>
		/// Update the total weight of the shipment based on its ContentWeight and any packaging weight.  The default
		/// implementation is just to set the TotalWeight equal to the ContentWeight.
		/// </summary>
		public virtual void UpdateTotalWeight(ShipmentEntity shipment)
		{
			if (shipment.Processed)
			{
				throw new InvalidOperationException("Cannot update weight on a processed shipment.");
			}

			shipment.TotalWeight = shipment.ContentWeight;
		}

		/// <summary>
		/// Get the available origins for the ShipmentType.  This is used to display the origin address UI.
		/// </summary>
		public List<KeyValuePair<string, long>> GetOrigins()
		{
			List<KeyValuePair<string, long>> origins = new List<KeyValuePair<string, long>>();

			if (SupportsAccountAsOrigin)
			{
				origins.Add(new KeyValuePair<string, long>("Account Address", (long) ShipmentOriginSource.Account));
			}

			// Add all the shippers
			foreach (ShippingOriginEntity origin in ShippingOriginManager.Origins)
			{
				origins.Add(new KeyValuePair<string, long>(origin.Description, origin.ShippingOriginID));
			}

			// Add store and manual always
			origins.Add(new KeyValuePair<string, long>("Store Address", (long) ShipmentOriginSource.Store));
			origins.Add(new KeyValuePair<string, long>("Other Address", (long) ShipmentOriginSource.Other));

			return origins;
		}

		/// <summary>
		/// Get the carrier specific description of the shipping service used. The carrier specific data must already exist
		/// when this method is called.
		/// </summary>
		public abstract string GetServiceDescription(ShipmentEntity shipment);

		/// <summary>
		/// Can be overridden by derived classes to provide common carrier details of shipments
		/// </summary>
		public virtual ShipmentCommonDetail GetShipmentCommonDetail(ShipmentEntity shipment)
		{
			return new ShipmentCommonDetail();
		}

		/// <summary>
		/// Get the total number of parcels contained in the shipment.  For shipments without "packages", the shipment itself counts as 1.
		/// </summary>
		public virtual int GetParcelCount(ShipmentEntity shipment)
		{
			return 1;
		}

		/// <summary>
		/// Get detailed information about the parcel in a generic way that can be used accross shipment types
		/// </summary>
		public abstract ShipmentParcel GetParcelDetail(ShipmentEntity shipment, int parcelIndex);

		/// <summary>
		/// Get the tracking numbers for the shipment.  This can incluce extra text, such as "Package 1: (track#)"
		/// </summary>
		public virtual List<string> GetTrackingNumbers(ShipmentEntity shipment)
		{
			if (!shipment.Processed)
			{
				return new List<string>();
			}

			return new List<string> { shipment.TrackingNumber };
		}

		/// <summary>
		/// Called to get the latest rates for the shipment
		/// </summary>
		public virtual RateGroup GetRates(ShipmentEntity shipment)
		{
			if (SupportsGetRates)
			{
				throw new NotImplementedException();
			}
			else
			{
				throw new InvalidOperationException("Should not be called.");
			}
		}

		/// <summary>
		/// Apply the specified shipment profile to the given shipment.
		/// </summary>
		public virtual void ApplyProfile(ShipmentEntity shipment, ShippingProfileEntity profile)
		{
			ShippingProfileUtility.ApplyProfileValue(profile.OriginID, shipment, ShipmentFields.OriginOriginID);
			ShippingProfileUtility.ApplyProfileValue(profile.ReturnShipment, shipment, ShipmentFields.ReturnShipment);

			// Special case for insurance
			for (int i = 0; i < GetParcelCount(shipment); i++)
			{
				InsuranceChoice insuranceChoice = GetParcelDetail(shipment, i).Insurance;

				if (profile.Insurance != null)
				{
					insuranceChoice.Insured = profile.Insurance.Value;
				}

				if (profile.InsuranceInitialValueSource != null)
				{
					// Don't apply the value to the subsequent parcels - that would probably end up over-ensuring the whole shipment.
					if (i == 0)
					{
						InsuranceInitialValueSource source = (InsuranceInitialValueSource) profile.InsuranceInitialValueSource;
						insuranceChoice.InsuranceValue = InsuranceUtility.GetInsuranceValue(shipment, source, profile.InsuranceInitialValueAmount);
					}
				}
			}
		}

		/// <summary>
		/// Process the shipment
		/// </summary>
		public abstract void ProcessShipment(ShipmentEntity shipment);

		/// <summary>
		/// Must be overridden by derived types to provide tracking details for the given shipment.
		/// </summary>
		public virtual TrackingResult TrackShipment(ShipmentEntity shipment)
		{
			throw new ShippingException(string.Format("Tracking is not supported for {0}.", ShipmentTypeName));
		}

		/// <summary>
		/// Called to do carrier specific shipment voiding.  Not all carriers required voiding.
		/// </summary>
		public virtual void VoidShipment(ShipmentEntity shipment)
		{

		}

		/// <summary>
		/// Create the XML elements that are the input to the XSL engine for the given shipment
		/// </summary>
		public virtual void GenerateTemplateElements(ElementOutline container, Func<ShipmentEntity> shipment, Func<ShipmentEntity> loaded)
		{

		}

		/// <summary>
		/// Returns currency to be used for a country
		/// </summary>
		/// <param name="countryCode"></param>
		public static CurrencyType GetCurrencyForCountryCode(string countryCode)
		{
			if (string.IsNullOrWhiteSpace(countryCode))
			{
				return CurrencyType.USD;
			}

			switch (countryCode.ToUpperInvariant())
			{
				case "CA":
					return CurrencyType.CAD;
				case "US":
					return CurrencyType.USD;
				default:
					return CurrencyType.USD;
			}
		}
		
		/// <summary>
		/// Returns a url to the carrier's website for the specified shipment
		/// </summary>
		public virtual string GetCarrierTrackingUrl(ShipmentEntity shipment)
		{
			return string.Empty;
		}
		
		/// <summary>
		/// Determines if a shipment will be domestic or international
		/// </summary>
		/// <param name="shipmentEntity"></param>
		public static bool IsDomestic(ShipmentEntity shipmentEntity)
		{
			if (shipmentEntity == null)
			{
				throw new ArgumentNullException("shipmentEntity");
			}

			return shipmentEntity.OriginCountryCode.ToUpperInvariant() == shipmentEntity.ShipCountryCode.ToUpperInvariant();
		}

		/// <summary>
		/// Gets whether the specified settings tab should be hidden in the UI
		/// </summary>
		public virtual bool IsSettingsTabHidden(ShipmentTypeSettingsControl.Page tab)
		{
			return false;
		}

		/// <summary>
		/// Gets an instance to the best rate shipping broker for a provider based on the shipment configuration.
		/// </summary>
		/// <param name="shipment">The shipment.</param>
		/// <returns>An instance of an IBestRateShippingBroker.</returns>
		public abstract IBestRateShippingBroker GetShippingBroker(ShipmentEntity shipment);

		/// <summary>
		/// Preferences the process.
		/// </summary>
		/// <returns>
		/// Most shipment types don't do any pre-processing and will return themselves.  
		/// This will return a different shipping type for BestRate
		/// </returns>
		public virtual ShipmentType PreProcess(ShipmentEntity shipment)
		{
			return this;
		}

		/// <summary>
		/// Indicates if customs forms may be required to ship the shipment based on the
		/// shipping address and any store specific logic that may impact whether customs
		/// is required (i.e. eBay GSP).
		/// </summary>
		public virtual bool IsCustomsRequired(ShipmentEntity shipment)
		{
			// Some carts have an international shipping program in place that allow
			// sellers to ship international orders to a domestic facility meaning 
			// customs is not required despite the international shipping address, so 
			// let the store take a look at the shipment as well to determine if customs
			// are required in addition to the just looking at the shipping address.

			bool requiresCustoms = IsCustomsRequiredByShipment(shipment);

			if (requiresCustoms)
			{
				// This shipment requires customs based on the shipping address
				// but allow the store to have the final say
				OrderHeader orderHeader = DataProvider.GetOrderHeader(shipment.OrderID);
				StoreType storeType = StoreTypeManager.GetType(StoreManager.GetStore(orderHeader.StoreID));

				// Pass a true value indicating customs is required based on the shipping address
				requiresCustoms = storeType.IsCustomsRequired(shipment, true);
			}

			return requiresCustoms;
		}

		/// <summary>
		/// Indicates if customs forms may be required to ship the shipment based on the
		/// shipping address.
		/// </summary>
		protected virtual bool IsCustomsRequiredByShipment(ShipmentEntity shipment)
		{
			bool requiresCustoms = !ShipmentType.IsDomestic(shipment);

			if (shipment.ShipCountryCode == "US")
			{
				if (PostalUtility.IsMilitaryState(shipment.ShipStateProvCode))
				{
					requiresCustoms = true;
				}

				// Foreign US territories requiring customs forms - http://pe.usps.com/text/dmm300/608.htm
				if (shipment.ShipPostalCode.StartsWith("96910") ||
					shipment.ShipPostalCode.StartsWith("96912") ||
					shipment.ShipPostalCode.StartsWith("96913") ||
					shipment.ShipPostalCode.StartsWith("96915") ||
					shipment.ShipPostalCode.StartsWith("96916") ||
					shipment.ShipPostalCode.StartsWith("96917") ||
					shipment.ShipPostalCode.StartsWith("96919") ||
					shipment.ShipPostalCode.StartsWith("96921") ||
					shipment.ShipPostalCode.StartsWith("96923") ||
					shipment.ShipPostalCode.StartsWith("96928") ||
					shipment.ShipPostalCode.StartsWith("96929") ||
					shipment.ShipPostalCode.StartsWith("96931") ||
					shipment.ShipPostalCode.StartsWith("96932") ||
					shipment.ShipPostalCode.StartsWith("96939") ||
					shipment.ShipPostalCode.StartsWith("96940") ||
					shipment.ShipPostalCode.StartsWith("96941") ||
					shipment.ShipPostalCode.StartsWith("96942") ||
					shipment.ShipPostalCode.StartsWith("96943") ||
					shipment.ShipPostalCode.StartsWith("96944") ||
					shipment.ShipPostalCode.StartsWith("96950") ||
					shipment.ShipPostalCode.StartsWith("96951") ||
					shipment.ShipPostalCode.StartsWith("96952") ||
					shipment.ShipPostalCode.StartsWith("96960") ||
					shipment.ShipPostalCode.StartsWith("96970") ||
					shipment.ShipPostalCode.StartsWith("96799"))
				{
					requiresCustoms = true;
				}
			}

			return requiresCustoms;
		}
	}
}
