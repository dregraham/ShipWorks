using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.UI.Controls.Html.Core;
using ShipWorks.UI.Controls.Html;

namespace ShipWorks.Shipping.Tracking
{
    /// <summary>
    /// UserControl for tracking a shipment
    /// </summary>
    public partial class ShipmentTrackingControl : UserControl
    {
        static readonly ILog log = LogManager.GetLogger(typeof(ShipmentTrackingControl));

        static string resourceMaster = ResourceUtility.ReadString("ShipWorks.Shipping.Tracking.HtmlSnippets.Master.txt");
        static string resourceTracking = ResourceUtility.ReadString("ShipWorks.Shipping.Tracking.HtmlSnippets.Tracking.txt");
        static string resourceDetail = ResourceUtility.ReadString("ShipWorks.Shipping.Tracking.HtmlSnippets.TrackingDetail.txt");
        static string resourceDetailActivityOnly = ResourceUtility.ReadString("ShipWorks.Shipping.Tracking.HtmlSnippets.TrackingDetailActivityOnly.txt");

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentTrackingControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Clear the content of the control
        /// </summary>
        public void Clear()
        {
            htmlControl.Html = "";
        }

        /// <summary>
        /// Track the given shipment and display the results in the control
        /// </summary>
        public void TrackShipment(ShipmentEntity shipment)
        {
            if (!shipment.Processed)
            {
                htmlControl.Html = WrapInHtmlMaster("The shipment has not been processed.");
            }

            if (shipment.Voided)
            {
                htmlControl.Html = WrapInHtmlMaster("The shipment has been voided.");
            }

            try
            {
                TrackingResult result = ShippingManager.TrackShipment(shipment);

                StringBuilder trackingText = new StringBuilder(resourceTracking);
                
                trackingText.Replace("{SwTrackingSummary}", result.Summary);
                trackingText.Replace("{SwDetailRows}", GenerateDetailHtmlRows(result.Details));

                htmlControl.Html = WrapInHtmlMaster(trackingText.ToString());
            }
            catch (ShippingException ex)
            {
                log.Error("Tracking", ex);

                htmlControl.Html = WrapInHtmlMaster("<span style='color:red'>" + ex.Message + "</span>");
            }

            HtmlApi.IHTMLBodyElement body = (HtmlApi.IHTMLBodyElement) htmlControl.HtmlDocument.Body;
            body.SetScroll("no");

            OnResize(this, EventArgs.Empty);
        }

        /// <summary>
        /// Generate the html for the detail rows
        /// </summary>
        private string GenerateDetailHtmlRows(List<TrackingResultDetail> details)
        {
            if (details.Count == 0)
            {
                details.Add(new TrackingResultDetail { Activity = "No details available." });
            }

            StringBuilder sb = new StringBuilder();

            string lastDate = null;
            int detailRowIndex = 0;

            foreach (TrackingResultDetail detail in details)
            {
                StringBuilder detailText;

                if (detail.Date != null)
                {
                    detailText = new StringBuilder(resourceDetail);

                    detailText.Replace("{SwDate}", detail.Date == lastDate ? "" : detail.Date);
                    detailText.Replace("{SwTime}", detail.Time);
                    detailText.Replace("{SwActivity}", detail.Activity);
                    detailText.Replace("{SwLocation}", detail.Location);

                    lastDate = detail.Date;
                }
                else
                {
                    detailText = new StringBuilder(resourceDetailActivityOnly);
                    detailText.Replace("{SwActivity}", detail.Activity);
                }

                detailText.Replace("{SwDetailRowClass}", ((detailRowIndex++ % 2 == 0) ? " class=grayrow" : ""));

                sb.Append(detailText.ToString());
            }

            return sb.ToString();
        }

        /// <summary>
        /// Wrap the given body content with the "Master" html
        /// </summary>
        private string WrapInHtmlMaster(string bodyContent)
        {
            StringBuilder masterHtml = new StringBuilder(resourceMaster);

            masterHtml.Replace("{BODY}", HtmlControl.ZoomDivStartTag + bodyContent + "</div>");

            return masterHtml.ToString();
        }

        /// <summary>
        /// Control is being resized
        /// </summary>
        private void OnResize(object sender, EventArgs e)
        {
            HtmlApi.IHTMLElement zoomElement = htmlControl.HtmlDocument.getElementById("zoomElement") as HtmlApi.IHTMLElement;
            if (zoomElement == null)
            {
                return;
            }

            htmlControl.Height = htmlControl.DetermineIdealRenderedBitmapHeight();
            themeBorderPanel.Height = Math.Min(htmlControl.Height, Height - 30);

            copy.Top = themeBorderPanel.Bottom + 2;
        }

        /// <summary>
        /// Copy the current text
        /// </summary>
        private void OnCopy(object sender, EventArgs e)
        {
            if (!htmlControl.CanCopy)
            {
                htmlControl.SelectAll();
                htmlControl.Copy();
                htmlControl.ClearSelection();
            }
            else
            {
                htmlControl.Copy();
            }
        }
    }
}
