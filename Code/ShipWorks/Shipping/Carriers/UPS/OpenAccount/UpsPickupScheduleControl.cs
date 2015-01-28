using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.WebServices.OpenAccount;

namespace ShipWorks.Shipping.Carriers.UPS.OpenAccount
{
    /// <summary>
    /// Control to define UPS Pickup Schedule
    /// </summary>
    public partial class UpsPickupScheduleControl : UserControl
    {
        private const string timeFormat = "HHmmss";

        /// <summary>
        ///     Initializes a new instance of the <see cref="UpsPickupScheduleControl" /> class.
        /// </summary>
        public UpsPickupScheduleControl()
        {
            InitializeComponent();

            EnumHelper.BindComboBox<UpsPickupOption>(pickupOption);
            EnumHelper.BindComboBox<UpsPickupLocation>(pickupLocation);
            BindPickupStartDates();

            UpdatePanelVisibility();
        }

        /// <summary>
        /// Popluate the pickup start dates with valid options.  It seems that UPS only allows 2 business days from now
        /// and a total of 8 business days in the future.  
        /// </summary>
        private void BindPickupStartDates()
        {
            List<DateTime> pickupStartDates = new List<DateTime>();
            
            // Start two days in the future
            DateTime date = DateTime.Now.AddDays(2);

            // Now check the date for being a business day, and if it is add it to the list,
            // otherwise, keep adding days.  Continue until we have 8 business days.
            while (pickupStartDates.Count <= 7)
            {
                if (date.IsBusinessDay())
                {
                    pickupStartDates.Add(date);
                }

                date = date.AddDays(1);
            }

            pickupStartDate.DisplayMember = "Key";
            pickupStartDate.ValueMember = "Value";
            pickupStartDate.DataSource = pickupStartDates.Select(d => new { Key = d.ToString("dddd, MMMM dd, yyyy"), Value = d.ToString("yyyyMMdd") }).ToList();
        }

        /// <summary>
        ///     Called when [time changed].
        /// </summary>
        private void OnTimeChanged(object sender, EventArgs e)
        {
            DateTimePicker dateTimePicker = (DateTimePicker)sender;

            int diff = dateTimePicker.Value.TimeOfDay.Minutes%15;
            if (diff > 7)
            {
                dateTimePicker.Value = dateTimePicker.Value.AddMinutes(-1*diff);
            }
            else if (diff != 0)
            {
                dateTimePicker.Value = dateTimePicker.Value.AddMinutes(15 - diff);
            }
        }

        /// <summary>
        ///     Called when [changed pickup option].
        /// </summary>
        private void OnChangedPickupOption(object sender, EventArgs e)
        {
            UpdatePanelVisibility();
        }

        /// <summary>
        /// Updates the panel visibilities based on pickup selection.
        /// </summary>
        private void UpdatePanelVisibility()
        {
            bool isPickupOptionRequired = IsPickupOptionRequired();
            pickupDateTimePanel.Visible = isPickupOptionRequired;
            feeInfoPanel.Visible = isPickupOptionRequired;

            // Shop if pickup option is day specific pickup (99)
            pickUpDay.Visible = (UpsPickupOption)pickupOption.SelectedValue == UpsPickupOption.DaySpecificPickup;
        }

        /// <summary>
        ///     Pickup Option Required if PickupOption is any daily pickup option (01,07,99,02)
        /// </summary>
        /// <returns></returns>
        private bool IsPickupOptionRequired()
        {
            return (UpsPickupOption)pickupOption.SelectedValue != UpsPickupOption.NoScheduledPickup;
        }

        /// <summary>
        ///     Saves to request.
        /// </summary>
        public void SaveToRequest(OpenAccountRequest request)
        {
            ValidatePickupTime();

            // Clear out existing pickup time info.
            request.PickupInformation = new PickupInformationType();

            AddDays(request);

            request.PickupInformation.PickupOption = new CodeOnlyType
            {
                Code = EnumHelper.GetApiValue((UpsPickupOption)pickupOption.SelectedValue)
            };

            AddDailyPickupOptions(request);
        }

        /// <summary>
        ///     Validates the pickup time.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        private void ValidatePickupTime()
        {
            if (IsPickupOptionRequired())
            {
                if (earliestPickup.Value >= preferredPickup.Value)
                {
                    throw new UpsOpenAccountException("Earliest Pickup time must be before the Preferred Pickup time.");
                }
                if (latestPickup.Value <= preferredPickup.Value)
                {
                    throw new UpsOpenAccountException("Latest Pickup time must be after the Preferred Pickup time.");
                }
            }
        }

        /// <summary>
        ///     Adds the daily pickup options.
        /// </summary>
        private void AddDailyPickupOptions(OpenAccountRequest request)
        {
            if ((UpsPickupOption)pickupOption.SelectedValue != UpsPickupOption.NoScheduledPickup)
            {
                request.PickupInformation.PickupLocation = EnumHelper.GetApiValue((UpsPickupLocation)pickupLocation.SelectedValue);

                request.PickupInformation.EarliestPickupTime = earliestPickup.Value.ToString(timeFormat);
                request.PickupInformation.PreferredPickupTime = preferredPickup.Value.ToString(timeFormat);
                request.PickupInformation.LatestPickupTime = latestPickup.Value.ToString(timeFormat);

                request.PickupInformation.PickupStartDate = pickupStartDate.SelectedValue.ToString();
            }
        }

        /// <summary>
        ///     Adds the days.
        /// </summary>
        /// <exception cref="UpsOpenAccountException">Please select a day.</exception>
        private void AddDays(OpenAccountRequest request)
        {
            if ((UpsPickupOption)pickupOption.SelectedValue == UpsPickupOption.DaySpecificPickup)
            {
                var pickupDays = new List<string>();

                AddDay(monday, "01", pickupDays);
                AddDay(tuesday, "02", pickupDays);
                AddDay(wednesday, "03", pickupDays);
                AddDay(thursday, "03", pickupDays);
                AddDay(friday, "04", pickupDays);

                if (pickupDays.Count == 0)
                {
                    throw new UpsOpenAccountException("Please select a day.");
                }

                request.PickupInformation.PickupSchedule = pickupDays.ToArray();
            }
        }

        /// <summary>
        ///     Adds the day.
        /// </summary>
        private static void AddDay(CheckBox dayComboBox, string dayCode, ICollection<string> pickupDays)
        {
            if (dayComboBox.Checked)
            {
                pickupDays.Add(dayCode);
            }
        }

        /// <summary>
        /// Called when [fee link clicked].
        /// </summary>
        private void OnFeeLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            WebHelper.OpenUrl("http://support.shipworks.com/solution/articles/4000035267-installing-ups-using-the-ups-setup-wizard", this);
        }
    }
}