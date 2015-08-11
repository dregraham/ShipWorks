using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using ShipWorks.SqlServer.Filters.DirtyCounts;
using System.Collections;

namespace ShipWorks.Tests.SqlServer
{
    public class FilterTests
    {
        [Fact]
        public void EmptyMask()
        {
            bool set = FilterNodeColumnMaskUtility.HasAnyTableBitsSet(new byte[0], FilterNodeColumnMaskTable.Order);

            Assert.False(set);
        }

        [Fact]
        public void MaskWithHigherTableSet()
        {
            BitArray mask = new BitArray(FilterNodeColumnMaskUtility.TotalBytes * 8);
            mask[FilterNodeColumnMaskUtility.GetBitPosition(FilterNodeColumnMaskTable.Customer, 1)] = true;

            bool set = FilterNodeColumnMaskUtility.HasAnyTableBitsSet(FilterNodeColumnMaskUtility.ConvertBitArrayToBitmask(mask), FilterNodeColumnMaskTable.Order);

            Assert.False(set);
        }

        [Fact]
        public void MaskWithLowerTableSet()
        {
            BitArray mask = new BitArray(FilterNodeColumnMaskUtility.TotalBytes * 8);
            mask[FilterNodeColumnMaskUtility.GetBitPosition(FilterNodeColumnMaskTable.Order, 1)] = true;

            bool set = FilterNodeColumnMaskUtility.HasAnyTableBitsSet(FilterNodeColumnMaskUtility.ConvertBitArrayToBitmask(mask), FilterNodeColumnMaskTable.Customer);

            Assert.False(set);
        }

        [Fact]
        public void MaskWithMatchingFirstBitSet1()
        {
            BitArray mask = new BitArray(FilterNodeColumnMaskUtility.TotalBytes * 8);
            mask[FilterNodeColumnMaskUtility.GetBitPosition(FilterNodeColumnMaskTable.OrderItem, 0)] = true;

            bool set = FilterNodeColumnMaskUtility.HasAnyTableBitsSet(FilterNodeColumnMaskUtility.ConvertBitArrayToBitmask(mask), FilterNodeColumnMaskTable.OrderItem);

            Assert.True(set);
        }

        [Fact]
        public void MaskWithMatchingFirstBitSet2()
        {
            BitArray mask = new BitArray(FilterNodeColumnMaskUtility.TotalBytes * 8);
            mask[FilterNodeColumnMaskUtility.GetBitPosition(FilterNodeColumnMaskTable.Customer, 0)] = true;

            bool set = FilterNodeColumnMaskUtility.HasAnyTableBitsSet(FilterNodeColumnMaskUtility.ConvertBitArrayToBitmask(mask), FilterNodeColumnMaskTable.Customer);

            Assert.True(set);
        }

        [Fact]
        public void MaskWithMatchingLastBitSet1()
        {
            BitArray mask = new BitArray(FilterNodeColumnMaskUtility.TotalBytes * 8);
            mask[FilterNodeColumnMaskUtility.GetBitPosition(FilterNodeColumnMaskTable.Customer, FilterNodeColumnMaskUtility.GetTableBitCount(FilterNodeColumnMaskTable.Customer) - 1)] = true;

            BitArray test = new BitArray(24);
            test[8] = true;
            test[23] = true;

            byte[] result = FilterNodeColumnMaskUtility.ConvertBitArrayToBitmask(test);

            bool set = FilterNodeColumnMaskUtility.HasAnyTableBitsSet(FilterNodeColumnMaskUtility.ConvertBitArrayToBitmask(mask), FilterNodeColumnMaskTable.Customer);

            Assert.True(set);
        }

        [Fact]
        public void MaskWithMatchingLastBitSet2()
        {
            BitArray mask = new BitArray(FilterNodeColumnMaskUtility.TotalBytes * 8);
            mask[FilterNodeColumnMaskUtility.GetBitPosition(FilterNodeColumnMaskTable.OrderItem, FilterNodeColumnMaskUtility.GetTableBitCount(FilterNodeColumnMaskTable.OrderItem) - 1)] = true;

            bool set = FilterNodeColumnMaskUtility.HasAnyTableBitsSet(FilterNodeColumnMaskUtility.ConvertBitArrayToBitmask(mask), FilterNodeColumnMaskTable.OrderItem);

            Assert.True(set);
        }
    }
}
