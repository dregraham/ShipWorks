﻿using System;
using Autofac;
using ShipWorks.ApplicationCore;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.Editing.Rating
{
    /// <summary>
    /// Factory to create an ExceptionsRateFootnote
    /// </summary>
    public class ExceptionsRateFootnoteFactory : IRateFootnoteFactory
    {
        private readonly RatingExceptionType ratingExceptionType;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionsRateFootnoteFactory"/> class.
        /// </summary>
        public ExceptionsRateFootnoteFactory(ShipmentTypeCode shipmentTypeCode, Exception exception)
        {
            ShipmentTypeCode = shipmentTypeCode;

            ratingExceptionType = IsInvalidPackageException(exception) ?
                RatingExceptionType.InvalidPackageDimensions :
                RatingExceptionType.General;

            ErrorMessage = exception.Message;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionsRateFootnoteFactory"/> class.
        /// </summary>
        public ExceptionsRateFootnoteFactory(ShipmentTypeCode shipmentTypeCode, string errorMessage)
        {
            ShipmentTypeCode = shipmentTypeCode;
            ratingExceptionType = RatingExceptionType.General;
            ErrorMessage = errorMessage;
        }

        /// <summary>
        /// Gets the error message.
        /// </summary>
        public string ErrorMessage { get; }

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
            if (exception == null)
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
        public ShipmentTypeCode ShipmentTypeCode { get; private set; }

        /// <summary>
        /// Creates a footnote control.
        /// </summary>
        public RateFootnoteControl CreateFootnote(IFootnoteParameters parameters)
        {
            return ratingExceptionType == RatingExceptionType.InvalidPackageDimensions ?
               (RateFootnoteControl) new InvalidPackageDimensionsRateFootnoteControl(ErrorMessage) :
                new ExceptionsRateFootnoteControl(ErrorMessage);
        }

        /// <summary>
        /// Get a view model that represents this footnote
        /// </summary>
        public object CreateViewModel(ICarrierShipmentAdapter shipmentAdapter) =>
            CreateViewModel(shipmentAdapter, IoC.BeginLifetimeScope());

        /// <summary>
        /// Get a view model that represents this footnote
        /// </summary>
        public object CreateViewModel(ICarrierShipmentAdapter shipmentAdapter, ILifetimeScope lifetimeScope)
        {
            IExceptionsRateFootnoteViewModel viewModel = lifetimeScope.Resolve<IExceptionsRateFootnoteViewModel>();
            viewModel.ErrorText = ratingExceptionType == RatingExceptionType.InvalidPackageDimensions ?
                "Invalid package dimensions." :
                "Some errors occurred while getting rates.";
            viewModel.DetailedMessage = ErrorMessage;
            return viewModel;
        }
    }
}
