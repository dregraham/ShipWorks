using System;
using Interapptive.Shared.UI;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.Postal;

namespace ShipWorks.Shipping.UI.Carriers.Postal.Usps
{
    /// <summary>
    /// Details for the Global Post dialog
    /// </summary>
    public class GlobalPostDialogDetails
    {
        // Global Post stuff 
        private const string GlobalPostDisplayUrl = "https://stamps.custhelp.com/app/answers/detail/a_id/3782";
        private const string GlobalPostMoreInfoUrl = "https://stamps.custhelp.com/app/answers/detail/a_id/3802";
        private const string GlobalPostBrowserDlgTitle = "Your GlobalPost Label";

        // Global Post Advantage Program stuff
        private const string StampsGlobalPostAdvantageProgramDisplayUrl = "https://stamps.custhelp.com/app/answers/detail/a_id/5174";
        private const string EndiciaGlobalPostAdvantageProgramDisplayUrl = "https://stamps.custhelp.com/app/answers/detail/a_id/5175";
        private const string GlobalPostAdvantageProgramMoreInfoUrl = "https://shipworks.zendesk.com/hc/en-us/articles/360022649931";
        private const string GlobalPostAdvantageProgramBrowserDlgTitle = "Your First-Class International Envelope Label";

        // Presort info
        private const string PresortDisplayUrl = "https://stamps.custhelp.com/app/answers/detail/a_id/5229";
        private const string PresortMoreInfoUrl = "https://shipworks.zendesk.com/hc/en-us/articles/360022468332";
        private const string PresortDialogTitle = "Your International Label";

        /// <summary>
        /// Constructor
        /// </summary>
        public GlobalPostDialogDetails(IShipmentEntity shipment)
        {
            bool gapShipment = !PostalUtility.IsGlobalPost((PostalServiceType) shipment.Postal.Service);
            bool presortShipment = PostalUtility.IsPresort(shipment.Postal);

            DisplayUrl = new Uri(GetDisplayUrl(shipment, gapShipment, presortShipment));
            Title = GetTitleToUse(gapShipment, presortShipment);
            MoreInfoLink = GetMorInfoLink(gapShipment, presortShipment);
            NotificationType = GetNotificationType(gapShipment, presortShipment);
        }

        /// <summary>
        /// Main url for the details of the dialog
        /// </summary>
        public Uri DisplayUrl { get; }

        /// <summary>
        /// Title of the dialog
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// More info link
        /// </summary>
        public string MoreInfoLink { get; }

        /// <summary>
        /// Type of notification for dismissal
        /// </summary>
        public UserConditionalNotificationType NotificationType { get; }

        /// <summary>
        /// Get the type of notification for dismissal
        /// </summary>
        private static UserConditionalNotificationType GetNotificationType(bool gapShipment, bool presortShipment)
        {
            if (presortShipment)
            {
                return UserConditionalNotificationType.PresortLabelChange;
            }

            return gapShipment ? UserConditionalNotificationType.GlobalPostAdvantageChange : UserConditionalNotificationType.GlobalPostChange;
        }

        /// <summary>
        /// Get the more info link
        /// </summary>
        private static string GetMorInfoLink(bool gapShipment, bool presortShipment)
        {
            if (presortShipment)
            {
                return PresortMoreInfoUrl;
            }

            return gapShipment ? GlobalPostAdvantageProgramMoreInfoUrl : GlobalPostMoreInfoUrl;
        }

        /// <summary>
        /// Get the title for the dialog
        /// </summary>
        private static string GetTitleToUse(bool gapShipment, bool presortShipment)
        {
            if (presortShipment)
            {
                return PresortDialogTitle;
            }

            return gapShipment ? GlobalPostAdvantageProgramBrowserDlgTitle : GlobalPostBrowserDlgTitle;
        }

        /// <summary>
        /// Get the display url for the dialog
        /// </summary>
        private static string GetDisplayUrl(IShipmentEntity shipment, bool gapShipment, bool presortShipment)
        {
            if (presortShipment)
            {
                return PresortDisplayUrl;
            }

            if (gapShipment)
            {
                if (shipment.ShipmentTypeCode == ShipmentTypeCode.Endicia)
                {
                    return EndiciaGlobalPostAdvantageProgramDisplayUrl;
                }

                if (shipment.ShipmentTypeCode == ShipmentTypeCode.Usps)
                {
                    return StampsGlobalPostAdvantageProgramDisplayUrl;
                }
            }

            return GlobalPostDisplayUrl;
        }
    }
}