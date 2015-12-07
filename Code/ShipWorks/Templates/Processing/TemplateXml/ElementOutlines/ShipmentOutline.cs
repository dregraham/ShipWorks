using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data;
using ShipWorks.Templates.Processing.TemplateXml.NodeFactories;
using log4net;
using ShipWorks.Shipping;
using Interapptive.Shared.Business;
using SD.LLBLGen.Pro.ORMSupportClasses;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Connection;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Editing.Enums;
using ShipWorks.Users;
using ShipWorks.Shipping.ShipSense;

namespace ShipWorks.Templates.Processing.TemplateXml.ElementOutlines
{
    /// <summary>
    /// Outline for the 'Shipment' node
    /// </summary>
    public class ShipmentOutline : ElementOutline
    {
        static readonly ILog log = LogManager.GetLogger(typeof(ShipmentOutline));

        long shipmentID;
        ShipmentEntity shipment;

        bool fullyLoaded = false;

        /// <summary>
        /// Constructor
        /// </summary>
        [NDependIgnoreLongMethod]
        [NDependIgnoreComplexMethodAttribute]
        public ShipmentOutline(TemplateTranslationContext context)
            : base(context)
        {
            AddAttribute("ID", () => shipmentID);

            AddElement("ShipmentType", CreateShipmentTypeOutline(context));
            AddElement("Status", () => GetShipmentStatus(Shipment));
            AddElement("Processed", () => Shipment.Processed);
            AddElementLegacy2x("IsProcessed", () => Shipment.Processed);
            AddElement("ProcessedDate", () => Shipment.ProcessedDate);
            AddElement("ProcessedUser", new UserOutline(context), () => Shipment.ProcessedUserID == null ? (UserEntity) null : UserManager.GetUser(Shipment.ProcessedUserID.Value));
            AddElement("Voided", () => Shipment.Voided);
            AddElement("VoidedDate", () => Shipment.VoidedDate);
            AddElement("VoidedUser", new UserOutline(context), () => Shipment.VoidedUserID == null ? (UserEntity) null : UserManager.GetUser(Shipment.VoidedUserID.Value));
            AddElement("ShippedDate", () => Shipment.ShipDate);
            AddElement("ServiceUsed", () => ShippingManager.GetServiceUsed(Shipment));
            AddElement("ReturnShipment", () => Shipment.ReturnShipment);
            AddElement("TrackingNumber", () => Shipment.TrackingNumber);
            AddElement("TotalCharges", () => Shipment.ShipmentCost);
            AddElement("TotalWeight", () => Shipment.TotalWeight);

            AddElement("Address", new AddressOutline(context, "ship", true), () => new PersonAdapter(Shipment, "Ship"));
            AddElement("Address", new AddressOutline(context, "from", true), () => new PersonAdapter(Shipment, "Origin"));

            AddElement("CustomsItem", new CustomsItemOutline(context), () => { if (CustomsManager.IsCustomsRequired(Shipment)) { CustomsManager.LoadCustomsItems(Shipment, false); return Shipment.CustomsItems; } else return null; });
            
            // Add an outline entry for the last/terminating best rate event that occurred on the shipment
            AddElement("BestRateEvent", () => GetLatestBestRateEventDescription(Shipment));

            // Add the ShipSense outline with package and customs info
            AddElement("ShipSense", new ShipSenseOutline(context), () => Shipment);

            // Add the ShipSense outline with package and customs info
            AddElement("BilledType", () => EnumHelper.GetDescription((BilledType)Shipment.BilledType));
            AddElement("BilledWeight", () => Shipment.BilledWeight);

            // Add an outline entry for each unique shipment type that could potentially be used
            foreach (ShipmentType shipmentType in ShipmentTypeManager.ShipmentTypes)
            {
                // We need to "hoist" this as its own variable - otherwise the same typeCode variable instance would get captured for each iteration.
                ShipmentTypeCode typeCode = shipmentType.ShipmentTypeCode;

                // Add a package node for each package in the shipment.  For shipment types that don't support true "Packages" there will always be a single Package node that represents
                // the entire shipment.
                AddElement("Package", new PackageOutline(context),
                    () => Enumerable.Range(0, shipmentType.GetParcelCount(LoadedShipment)).Select(index => Tuple.Create(LoadedShipment, index)),
                    If(() => Shipment.ShipmentType == (int) typeCode));

                // Let the ShipmentType generate its elements into a stand-in container
                ElementOutline container = new ElementOutline(context);
                shipmentType.GenerateTemplateElements(container, () => Shipment, () => LoadedShipment);

                // Copy the elements from the stand-in to ourself, adding on the ShipmentType specific condition
                AddElements(container, If(() => Shipment.ShipmentType == (int) typeCode));
            }

            AddElement("RequestedLabelFormat", () => EnumHelper.GetDescription((ThermalLanguage)Shipment.RequestedLabelFormat));
        }
        
        /// <summary>
        /// Create the outline for the ShipmentType element
        /// </summary>
        private ElementOutline CreateShipmentTypeOutline(TemplateTranslationContext context)
        {
            ElementOutline outline = new ElementOutline(context);
            outline.AddAttribute("code", () => (int) ShipmentTypeManager.GetType(Shipment).ShipmentTypeCode);
            outline.AddTextContent(() => ShipmentTypeManager.GetType(Shipment).ShipmentTypeName);

            return outline;
        }

        /// <summary>
        /// Get text describing the current status of the shipment
        /// </summary>
        private static string GetShipmentStatus(ShipmentEntity shipment)
        {
            if (shipment.Processed)
            {
                return shipment.Voided ? "Voided" : "Processed";
            }

            return "None";
        }

        /// <summary>
        /// Get text describing the latest best rate event that occurred on the shipment.
        /// </summary>
        private static string GetLatestBestRateEventDescription(ShipmentEntity shipment)
        {
            // Obtain the latest event that occurred
            BestRateEventTypes eventTypes = (BestRateEventTypes)shipment.BestRateEvents;
            BestRateEventTypes latestEvent = eventTypes.GetLatestBestRateEvent();

            // Now that we have the latest event, we can just return the description for it
            return new BestRateEventsDescription(latestEvent).ToString().Trim();
        }

        /// <summary>
        /// Get text describing the ShipSense status of the shipment.
        /// </summary>
        private static string GetShipSenseStatusDescription(ShipmentEntity shipment)
        {
            ShipSenseStatus status = (ShipSenseStatus)shipment.ShipSenseStatus;
            return EnumHelper.GetDescription(status);
        }

        /// <summary>
        /// Create a clone of the outline, bound to the given data
        /// </summary>
        public override ElementOutline CreateDataBoundClone(object data)
        {
            return new ShipmentOutline(Context) { shipmentID = (long) data };
        }

        /// <summary>
        /// The ShipmentEntity represented by the bound outline
        /// </summary>
        private ShipmentEntity Shipment
        {
            get
            {
                if (shipment == null)
                {
                    shipment = (ShipmentEntity) DataProvider.GetEntity(shipmentID);
                    if (shipment == null)
                    {
                        log.WarnFormat("Order {0} was deleted and cannot be processed by template.", shipmentID);
                        throw new TemplateProcessException("An order has been deleted.");
                    }
                }

                return shipment;
            }
        }

        /// <summary>
        /// The same shipment as 'Shipment', except with EnsureShipmentLoaded called
        /// </summary>
        private ShipmentEntity LoadedShipment
        {
            get
            {
                if (fullyLoaded)
                {
                    return shipment;
                }

                try
                {
                    ShippingManager.EnsureShipmentLoaded(Shipment);

                    fullyLoaded = true;
                }
                catch (ORMConcurrencyException ex)
                {
                    log.WarnFormat("Shipment {0} had a concurrency violation and could not be processed.", shipmentID);
                    throw new TemplateProcessException("A shipment has been edited outside of ShipWorks and could not be loaded.", ex);
                }
                catch (ObjectDeletedException ex)
                {
                    log.WarnFormat("Shipment {0} was deleted and cannot be processed by template.", shipmentID);
                    throw new TemplateProcessException("A shipment has been deleted.", ex);
                }
                catch (SqlForeignKeyException ex)
                {
                    log.WarnFormat("Shipment {0} was deleted and cannot be processed by template.", shipmentID);
                    throw new TemplateProcessException("A shipment has been deleted.", ex);
                }

                return Shipment;
            }
        }
    }
}
