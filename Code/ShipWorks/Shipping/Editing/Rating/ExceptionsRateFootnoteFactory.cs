using System;

namespace ShipWorks.Shipping.Editing.Rating
{
    /// <summary>
    /// Factory to create an ExceptionsRateFootnote
    /// </summary>
    public class ExceptionsRateFootnoteFactory : IRateFootnoteFactory
    {
        private readonly string errorMessage;
        private readonly RatingExceptionType ratingExceptionType;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionsRateFootnoteFactory"/> class.
        /// </summary>
        public ExceptionsRateFootnoteFactory(ShipmentType shipmentType, Exception exception)
        {
            ShipmentType = shipmentType;

            ratingExceptionType = IsInvalidPackageException(exception) ?
                RatingExceptionType.InvalidPackageDimensions : 
                RatingExceptionType.General;

            errorMessage = exception.Message;
        }

        /// <summary>
        /// Notes that this factory should be used in BestRate
        /// </summary>
        public bool AllowedForBestRate
        {
            get { return true; }
        }

        /// <summary>
        /// Determines whether [is invalid package exception] [the specified exception].
        /// </summary>
        private bool IsInvalidPackageException(Exception exception)
        {
            if (exception==null)
            {
                return false;
            }

            if (exception is InvalidPackageDimensionsException)
            {
                return true;
            }

            return IsInvalidPackageException(exception.InnerException);
        }

        /// <summary>
        /// Gets the corresponding shipment type for the factory.
        /// </summary>
        public ShipmentType ShipmentType { get; private set; }

        /// <summary>
        /// Creates a footnote control.
        /// </summary>
        public RateFootnoteControl CreateFootnote(FootnoteParameters parameters)
        {
            return ratingExceptionType == RatingExceptionType.InvalidPackageDimensions ?
               (RateFootnoteControl) new InvalidPackageDimensionsRateFootnoteControl(errorMessage) :
                new ExceptionsRateFootnoteControl(errorMessage);
        }
    }
}
