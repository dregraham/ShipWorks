namespace ShipWorks.Templates.Processing.TemplateXml.ElementOutlines
{
    public class BestRateEventsOutline : ElementOutline
    {
        private string eventDescription;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="BestRateEventsOutline"/> class.
        /// </summary>
        /// <param name="context"></param>
        public BestRateEventsOutline(TemplateTranslationContext context)
            : base(context)
        {
            AddTextContent(() => eventDescription.Trim());
        }

        /// <summary>
        /// Bind a new cloned instance to the specified data
        /// </summary>
        public override ElementOutline CreateDataBoundClone(object data)
        {
            return new BestRateEventsOutline(Context) { eventDescription = (string)data };
        }
    }
}
