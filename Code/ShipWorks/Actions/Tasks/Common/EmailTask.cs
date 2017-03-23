using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using ShipWorks.Actions.Tasks.Common.Editors;
using ShipWorks.Actions.Tasks.Common.Enums;
using ShipWorks.Data;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Email;
using ShipWorks.Templates.Emailing;
using ShipWorks.Templates.Processing;

namespace ShipWorks.Actions.Tasks.Common
{
    /// <summary>
    /// Task for printing with a chosen template
    /// </summary>
    [ActionTask("Email", "Email", ActionTaskCategory.Output)]
    public class EmailTask : TemplateBasedTask
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(EmailTask));

        bool delayDelivery = false;
        EmailDelayType delayType = EmailDelayType.ShipDate;
        int delayQuantity = 1;
        TimeSpan delayTimeOfDay = new TimeSpan(8, 0, 0);

        /// <summary>
        /// Create the editor for the settings
        /// </summary>
        public override ActionTaskEditor CreateEditor()
        {
            return new EmailTaskEditor(this);
        }

        /// <summary>
        /// The label that goes before what the data source for the task should be.
        /// </summary>
        public override string InputLabel
        {
            get
            {
                return "Email using:";
            }
        }

        /// <summary>
        /// Indiciates if the email delivery should be delayed
        /// </summary>
        public bool DelayDelivery
        {
            get { return delayDelivery; }
            set { delayDelivery = value; }
        }

        /// <summary>
        /// How delivery should be delayed
        /// </summary>
        public EmailDelayType DelayType
        {
            get { return delayType; }
            set { delayType = value; }
        }

        /// <summary>
        /// The number of units to delay.  Only has meaning relative to delay type.
        /// </summary>
        public int DelayQuantity
        {
            get { return delayQuantity; }
            set { delayQuantity = value; }
        }

        /// <summary>
        /// The time of day the delay is set for, if enabled
        /// </summary>
        public TimeSpan DelayTimeOfDay
        {
            get { return delayTimeOfDay; }
            set { delayTimeOfDay = value; }
        }

        /// <summary>
        /// Do the email for the given template and input
        /// </summary>
        protected override void ProcessTemplateResults(TemplateEntity template, IList<TemplateResult> templateResults, ActionStepContext context)
        {
            try
            {
                EmailGenerator emailGenerator = EmailGenerator.Create(template, templateResults);

                if (DelayDelivery)
                {
                    emailGenerator.ProvideSendDateDelay += new EmailSendDateDelayProviderEventHandler(OnProvideSendDateDelay);
                }

                List<EmailOutboundEntity> messages = emailGenerator.Generate();

                if (messages.Count > 0)
                {
                    // Add the generated messages to our contxt
                    context.AddGeneratedEmail(messages);
                }
                else
                {
                    log.InfoFormat("EmailTask sent no email due to no template output results.");
                }
            }
            catch (EmailException ex)
            {
                throw new ActionTaskRunException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Callback to provide the delivery date frot the given email message that was generated
        /// </summary>
        void OnProvideSendDateDelay(object sender, EmailSendDateDelayProviderEventArgs e)
        {
            // Day of week
            if (DelayType == EmailDelayType.DayOfWeek)
            {
                DateTime date = DetermineNextDateFromDay((DayOfWeek) DelayQuantity);
                date = date.Date + DelayTimeOfDay;

                e.DelayUntilDate = date.ToUniversalTime();
            }

            // ShipDate
            else if (DelayType == EmailDelayType.ShipDate)
            {
                DateTime? date = DetermineShipDate(e.TemplateInput);

                if (date != null)
                {
                    date = date.Value.Date + DelayTimeOfDay;

                    e.DelayUntilDate = date.Value.ToUniversalTime();
                }
            }

            // Minutes
            else if (DelayType == EmailDelayType.TimeMinutes)
            {
                e.DelayUntilDate = DateTime.UtcNow.AddMinutes(DelayQuantity);
            }

            // Hours
            else if (DelayType == EmailDelayType.TimeHours)
            {
                e.DelayUntilDate = DateTime.UtcNow.AddHours(DelayQuantity);
            }

            // Days
            else if (DelayType == EmailDelayType.TimeDays)
            {
                DateTime date = DateTime.Now.AddDays(DelayQuantity);
                date = date.Date + DelayTimeOfDay;

                e.DelayUntilDate = date.ToUniversalTime();
            }

            // Weeks
            else if (DelayType == EmailDelayType.TimeWeeks)
            {
                DateTime date = DateTime.Now.AddDays(DelayQuantity * 7);
                date = date.Date + DelayTimeOfDay;

                e.DelayUntilDate = date.ToUniversalTime();
            }
            else
            {
                throw new InvalidOperationException("unhandled DelayType");
            }
        }

        /// <summary>
        /// Determine the first date after today that is the specified date of the week
        /// </summary>
        private DateTime DetermineNextDateFromDay(DayOfWeek dayOfWeek)
        {
            for (int i = 1; i <= 7; i++)
            {
                DateTime date = DateTime.Now.AddDays(i);

                if (date.DayOfWeek == dayOfWeek)
                {
                    return date;
                }
            }

            throw new InvalidOperationException("Can't get here unless someone adds days to the week.");
        }

        /// <summary>
        /// Determine the ShipDate based on the given input.  Returns null if ambiguous or undetermined.
        /// </summary>
        private DateTime? DetermineShipDate(List<TemplateInput> input)
        {
            // We are interested in the Original keys - which will be the one's that are the inputKeys to this task.
            List<long> originalKeys = input.SelectMany(i => i.OriginalKeys).ToList();

            DateTime? shipDate = null;

            // Determine the soonest ship date
            foreach (long key in originalKeys)
            {
                if (EntityUtility.GetEntityType(key) == EntityType.ShipmentEntity)
                {
                    // We don't go through the ShippingManager here - we don't need the overhead
                    // of grabbing all the sibling shipments and ther parent order and whatnot. Choose
                    // the path of adventure, Indy!
                    ShipmentEntity shipment = (ShipmentEntity) DataProvider.GetEntity(key);

                    if (shipment != null)
                    {
                        if (shipDate == null || shipDate.Value > shipment.ShipDate.ToLocalTime())
                        {
                            shipDate = shipment.ShipDate.ToLocalTime();
                        }
                    }
                }
            }

            return shipDate;
        }
    }
}
