using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc.DataAccess;
using ShipWorks.Templates;
using ShipWorks.Templates.Tokens;

namespace ShipWorks.Stores.Platforms.Odbc.Upload
{
    /// <summary>
    /// Custom upload query supports processing tokens in the stores upload column source
    /// </summary>
    public class OdbcCustomUploadQuery : IOdbcQuery
    {
        private readonly OdbcStoreEntity store;
        private readonly ShipmentEntity shipment;
        private readonly ITemplateTokenProcessor templateTokenProcessor;

        /// <summary>
        /// Constructor
        /// </summary>
        public OdbcCustomUploadQuery(OdbcStoreEntity store, ShipmentEntity shipment, ITemplateTokenProcessor templateTokenProcessor)
        {
            this.store = store;
            this.shipment = shipment;
            this.templateTokenProcessor = templateTokenProcessor;
        }

        /// <summary>
        /// Processes the tokens in the custom query for the shipment provided in the constructor
        /// </summary>
        public string GenerateSql()
        {
            try
            {
                return templateTokenProcessor.ProcessTokens(store.UploadColumnSource, shipment.OrderID, false);
            }
            catch (TemplateException ex)
            {
                throw new ShipWorksOdbcException(ex);
            }
        }

        /// <summary>
        /// Sets the command text to the custom query
        /// </summary>
        public void ConfigureCommand(IShipWorksOdbcCommand command)
        {
            command.ChangeCommandText(GenerateSql());
        }
    }
}