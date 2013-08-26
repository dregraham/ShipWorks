using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.WebServices.OpenAccount;

namespace ShipWorks.Shipping.Carriers.UPS.OpenAccount
{
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
        }

        /// <summary>
        ///     Called when [time changed].
        /// </summary>
        private void OnTimeChanged(object sender, EventArgs e)
        {
            var dateTimePicker = (DateTimePicker)sender;

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
            pickupDateTimePanel.Visible = IsPickupOptionRequired();

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

                request.PickupInformation.PickupStartDate = latestPickup.Value.ToString("yyyyMMdd");
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
    }
}