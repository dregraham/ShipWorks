﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using DataVirtualization;
using Interapptive.Shared.Collections;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Products.UI
{
    /// <summary>
    /// View model used by the Products view designer
    /// </summary>
    public class DesignerProductsMode : IProductsMode
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DesignerProductsMode()
        {
            Products = new DataWrapper<IVirtualizingCollection<IProductListItemEntity>>(0,
                new DesignerVirtualizingCollection<IProductListItemEntity>(new[]
                {
                    new DesignerProductListItem
                    {
                        ProductVariantID = 0,
                        Name = "Product 1",
                        SKU = "ABC123",
                        Length = 1.2M,
                        Width = 123.56M,
                        Height = 3.92M,
                        Weight = 32.63M,
                        BinLocation = "A-B3",
                        ImageUrl = "https://placekitten.com/64/64",
                        IsActive = true,
                    },
                    new DesignerProductListItem
                    {
                        ProductVariantID = 0,
                        SKU = "DEF456",
                        Name = "Product 2",
                        Length = 9.23M,
                        Width = 1.33M,
                        Height = 12.96M,
                        Weight = 5.63M,
                        BinLocation = "C-3235",
                        ImageUrl = "https://placekitten.com/64/64"
                    },
                    new DesignerProductListItem(),
                    new DesignerProductListItem(),
                    new DesignerProductListItem
                    {
                        ProductVariantID = 0,
                        SKU = "XYZ-555-D",
                        Name = "Product 5",
                        Length = 1.2M,
                        Width = 123.56M,
                        Height = 3.92M,
                        Weight = 32.63M,
                        BinLocation = "A-B3",
                        ImageUrl = "https://placekitten.com/64/64"
                    },
                }, 2, 3));
        }


        public ICommand ActivateProductCommand => throw new NotImplementedException();

        public ICommand DeactivateProductCommand => throw new NotImplementedException();

        public ICommand AddProduct => throw new NotImplementedException();
        public ICommand ImportProducts => throw new NotImplementedException();
        public ICommand ExportProducts => throw new NotImplementedException();
        public ICommand RefreshProducts => throw new NotImplementedException();
        public ICommand EditProductVariant => throw new NotImplementedException();
        public ICommand SelectedProductsChanged => throw new NotImplementedException();

        public IDataWrapper<IVirtualizingCollection<IProductListItemEntity>> Products { get; private set; }
        public IList<long> SelectedProductIDs { get; set; }
        public IBasicSortDefinition CurrentSort { get; set; }
        public bool ShowInactiveProducts { get; set; }
        public string SearchText { get; set; }

        public ICommand EditProductVariantLink => throw new NotImplementedException();

        public ICommand EditProductVariantButton => throw new NotImplementedException();

        public ICommand CopyAsVariant => throw new NotImplementedException();

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Initialize(Action<Control> addControl, Action<Control> removeControl)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Non-invokable implementation of the ProductSelectionChanged event handler
        /// </summary>
        public event ProductSelectionChangedEventHandler ProductSelectionChanged
        {
            add { throw new NotImplementedException(); }
            remove { }
        }
    }

    internal class DesignerProductListItem : IProductListItemEntity
    {
        public long ProductVariantID { get; set; }

        public string SKU { get; set; }

        public string Name { get; set; }

        public decimal? Length { get; set; }

        public decimal? Width { get; set; }

        public decimal? Height { get; set; }

        public decimal? Weight { get; set; }

        public string Dimensions => String.Join("x", Length, Width, Height);

        public string BinLocation { get; set; }

        public string ImageUrl { get; set; }

        public bool IsActive { get; set; }

        public IProductListItemEntity AsReadOnly()
        {
            throw new NotImplementedException();
        }

        public IProductListItemEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            throw new NotImplementedException();
        }
    }


    internal class DesignerVirtualizingCollection<T> : IVirtualizingCollection<T> where T : class
    {
        private readonly List<IDataWrapper<T>> list;

        public DesignerVirtualizingCollection(IEnumerable<T> items, params int[] loadingIndicies)
        {
            list = items
                .Select((x, i) => loadingIndicies.Contains(i) ? new DataWrapper<T>(i, 0, () => { }) : new DataWrapper<T>(i, x))
                .OfType<IDataWrapper<T>>()
                .ToList();
        }

        public IDataWrapper<T> this[int index] { get => list[index]; set => throw new NotImplementedException(); }
        object IList.this[int index] { get => this[index]; set => throw new NotImplementedException(); }

        public int Count => list.Count;

        public bool IsEmpty => Count == 0;

        public bool IsReadOnly => true;

        public bool IsFixedSize => false;

        public object SyncRoot => this;

        public bool IsSynchronized => false;

        public void Add(IDataWrapper<T> item)
        {
            throw new NotImplementedException();
        }

        public int Add(object value)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(IDataWrapper<T> item)
        {
            throw new NotImplementedException();
        }

        public bool Contains(object value)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(IDataWrapper<T>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<IDataWrapper<T>> GetEnumerator() => list.GetEnumerator();

        public int IndexOf(IDataWrapper<T> item) => list.IndexOf(item);

        public int IndexOf(object value) => this.IndexOf((IDataWrapper<T>) value);

        public void Insert(int index, IDataWrapper<T> item)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, object value)
        {
            throw new NotImplementedException();
        }

        public bool Remove(IDataWrapper<T> item)
        {
            throw new NotImplementedException();
        }

        public void Remove(object value)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}
