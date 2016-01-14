using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using Shared.System.ComponentModel.DataAnnotations;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Services
{
    /// <summary>
    /// Implementation of IShipmentCustomsItemAdapter
    /// </summary>
    public class ShipmentCustomsItemAdapter : IShipmentCustomsItemAdapter, IDataErrorInfo
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentCustomsItemAdapter(ShipmentCustomsItemEntity shipmentCustomsItem)
        {
            ShipmentCustomsItemEntity = shipmentCustomsItem;
        }

        /// <summary>
        /// The backing ShipmentCustomsItemEntity
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ShipmentCustomsItemEntity ShipmentCustomsItemEntity { get; }

        /// <summary>
        /// The ShipmentCustomsItemID
        /// </summary>
        [Obfuscation(Exclude = true)]
        public long ShipmentCustomsItemID
        {
            get { return ShipmentCustomsItemEntity.ShipmentCustomsItemID; }
        }

        /// <summary>
        /// The ShipmentEntity
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ShipmentEntity Shipment
        {
            get { return ShipmentCustomsItemEntity.Shipment; }
        }

        /// <summary>
        /// The Description
        /// </summary>
        [Obfuscation(Exclude = true)]
        [MinLength(1, ErrorMessage = @"Description is required.")]
        [MaxLength(150, ErrorMessage = @"Description must be less than or equal to 150 characters.")]
        public string Description
        {
            get { return ShipmentCustomsItemEntity.Description; }
            set
            {
                ShipmentCustomsItemEntity.Description = value;
                RaisePropertyChanged(nameof(Description));
            }
        }

        /// <summary>
        /// The Quantity
        /// </summary>
        [Obfuscation(Exclude = true)]
        [DoubleCompare(0, ValueCompareOperatorType.GreaterThanOrEqualTo, ErrorMessage = @"Please selected a valid quantity.")]
        public double Quantity
        {
            get { return ShipmentCustomsItemEntity.Quantity; }
            set
            {
                ShipmentCustomsItemEntity.Quantity = value;
                RaisePropertyChanged(nameof(Quantity));
            }
        }

        /// <summary>
        /// The weight
        /// </summary>
        [Obfuscation(Exclude = true)]
        [DoubleCompare(0, ValueCompareOperatorType.GreaterThanOrEqualTo, ErrorMessage = @"Please selected a valid weight.")]
        public double Weight
        {
            get { return ShipmentCustomsItemEntity.Weight; }
            set
            {
                ShipmentCustomsItemEntity.Weight = value;
                RaisePropertyChanged(nameof(Weight));
            }
        }

        /// <summary>
        /// The unit value
        /// </summary>
        [Obfuscation(Exclude = true)]
        [DecimalCompare(0, ValueCompareOperatorType.GreaterThanOrEqualTo, ErrorMessage = @"Please selected a valid unit value.")]
        public decimal UnitValue
        {
            get { return ShipmentCustomsItemEntity.UnitValue; }
            set
            {
                ShipmentCustomsItemEntity.UnitValue = value;
                RaisePropertyChanged(nameof(UnitValue));
            }
        }

        /// <summary>
        /// The country of origin
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string CountryOfOrigin
        {
            get { return ShipmentCustomsItemEntity.CountryOfOrigin; }
            set
            {
                ShipmentCustomsItemEntity.CountryOfOrigin = value;
                RaisePropertyChanged(nameof(CountryOfOrigin));
            }
        }

        /// <summary>
        /// The harmonized code
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string HarmonizedCode
        {
            get { return ShipmentCustomsItemEntity.HarmonizedCode; }
            set
            {
                ShipmentCustomsItemEntity.HarmonizedCode = value;
                RaisePropertyChanged(nameof(HarmonizedCode));
            }
        }

        /// <summary>
        /// The number of pices
        /// </summary>
        [Obfuscation(Exclude = true)]
        [Range(0, 99999, ErrorMessage = @"Please select a valid 'Number of Pieces'.")]
        public int NumberOfPieces
        {
            get { return ShipmentCustomsItemEntity.NumberOfPieces; }
            set
            {
                ShipmentCustomsItemEntity.NumberOfPieces = value;
                RaisePropertyChanged(nameof(NumberOfPieces));
            }
        }

        /// <summary>
        /// The unit price amount
        /// </summary>
        [Obfuscation(Exclude = true)]
        [DecimalCompare(0, ValueCompareOperatorType.GreaterThanOrEqualTo, ErrorMessage = @"Please select a valid unit price amount.")]
        public decimal UnitPriceAmount
        {
            get { return ShipmentCustomsItemEntity.UnitPriceAmount; }
            set
            {
                ShipmentCustomsItemEntity.UnitPriceAmount = value;
                RaisePropertyChanged(nameof(UnitPriceAmount));
            }
        }

        /// <summary>
        /// Raise the INotifyPropertyChanged event
        /// </summary>
        /// <param name="propertyName"></param>
        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region IDataErrorInfo

        /// <summary>
        /// Accessor for property validation
        /// </summary>
        public string this[string columnName]
        {
            get
            {
                // If the shipment is processed, don't validate anything.
                if (Shipment?.Processed == true)
                {
                    return string.Empty;
                }

                return InputValidation<ShipmentCustomsItemAdapter>.Validate(this, columnName);
            }
        }

        /// <summary>
        /// IDataErrorInfo Error implementation
        /// </summary>
        public string Error => null;

        /// <summary>
        /// List of all validation errors
        /// </summary>
        /// <returns></returns>
        public ICollection<string> AllErrors()
        {
            return InputValidation<ShipmentCustomsItemAdapter>.Validate(this);
        }

        #endregion
    }
}
