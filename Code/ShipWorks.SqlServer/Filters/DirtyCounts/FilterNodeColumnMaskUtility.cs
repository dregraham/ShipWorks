﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ShipWorks.SqlServer.Filters.DirtyCounts
{
    /// <summary>
    /// Utility class for dealing with the column masks that control which column changes apply to which filter nodes
    /// </summary>
    public static class FilterNodeColumnMaskUtility
    {
        private static readonly int[] tableBitCount;
        private static readonly int[] tableBitOffset;
        private static readonly int totalBytes;

        /// <summary>
        /// Static constructor
        /// </summary>
        static FilterNodeColumnMaskUtility()
        {
            tableBitCount = new int[Enum.GetValues(typeof(FilterNodeColumnMaskTable)).Length];
            tableBitCount[(int) FilterNodeColumnMaskTable.Customer] = 35;
            tableBitCount[(int) FilterNodeColumnMaskTable.Order] = 93;
            tableBitCount[(int) FilterNodeColumnMaskTable.OrderItem] = 37;
            tableBitCount[(int) FilterNodeColumnMaskTable.OrderCharge] = 7;
            tableBitCount[(int) FilterNodeColumnMaskTable.Note] = 8;
            tableBitCount[(int) FilterNodeColumnMaskTable.Shipment] = 80;
            tableBitCount[(int) FilterNodeColumnMaskTable.PrintResult] = 23;
            tableBitCount[(int) FilterNodeColumnMaskTable.EmailOutbound] = 21;
            tableBitCount[(int) FilterNodeColumnMaskTable.EmailOutboundRelation] = 4;
            tableBitCount[(int) FilterNodeColumnMaskTable.OrderPaymentDetail] = 5;
            tableBitCount[(int) FilterNodeColumnMaskTable.DownloadDetail] = 9;
            tableBitCount[(int) FilterNodeColumnMaskTable.AmazonOrder] = 8;
            tableBitCount[(int) FilterNodeColumnMaskTable.ChannelAdvisorOrder] = 11;
            tableBitCount[(int) FilterNodeColumnMaskTable.ChannelAdvisorOrderItem] = 10;
            tableBitCount[(int) FilterNodeColumnMaskTable.EbayOrder] = 26;
            tableBitCount[(int) FilterNodeColumnMaskTable.EbayOrderItem] = 19;
            tableBitCount[(int) FilterNodeColumnMaskTable.MarketplaceAdvisorOrder] = 5;
            tableBitCount[(int) FilterNodeColumnMaskTable.OrderMotionOrder] = 4;
            tableBitCount[(int) FilterNodeColumnMaskTable.PayPalOrder] = 5;
            tableBitCount[(int) FilterNodeColumnMaskTable.ProStoresOrder] = 4;
            tableBitCount[(int) FilterNodeColumnMaskTable.PostalShipment] = 23;
            tableBitCount[(int) FilterNodeColumnMaskTable.UpsShipment] = 53;
            tableBitCount[(int) FilterNodeColumnMaskTable.FedExShipment] = 163;
            tableBitCount[(int) FilterNodeColumnMaskTable.CommerceInterfaceOrder] = 2;
            tableBitCount[(int) FilterNodeColumnMaskTable.ShopifyOrder] = 4;
            tableBitCount[(int) FilterNodeColumnMaskTable.EtsyOrder] = 3;
            tableBitCount[(int) FilterNodeColumnMaskTable.YahooOrder] = 2;
            tableBitCount[(int) FilterNodeColumnMaskTable.NeweggOrder] = 4;
            tableBitCount[(int) FilterNodeColumnMaskTable.BuyDotComOrderItem] = 7;
            tableBitCount[(int) FilterNodeColumnMaskTable.SearsOrder] = 6;
            tableBitCount[(int) FilterNodeColumnMaskTable.BigCommerceOrderItem] = 7;
            tableBitCount[(int) FilterNodeColumnMaskTable.InsurancePolicy] = 13;
            tableBitCount[(int) FilterNodeColumnMaskTable.GrouponOrder] = 3;
            tableBitCount[(int) FilterNodeColumnMaskTable.LemonStandOrder] = 2;
            tableBitCount[(int) FilterNodeColumnMaskTable.WalmartOrder] = 6;
            tableBitCount[(int) FilterNodeColumnMaskTable.WalmartOrderItem] = 3;
            tableBitCount[(int) FilterNodeColumnMaskTable.AmazonOrderSearch] = 4;
            tableBitCount[(int) FilterNodeColumnMaskTable.ChannelAdvisorOrderSearch] = 4;
            tableBitCount[(int) FilterNodeColumnMaskTable.ClickCartProOrderSearch] = 4;
            tableBitCount[(int) FilterNodeColumnMaskTable.CommerceInterfaceOrderSearch] = 4;
            tableBitCount[(int) FilterNodeColumnMaskTable.EbayOrderSearch] = 7;
            tableBitCount[(int) FilterNodeColumnMaskTable.GrouponOrderSearch] = 5;
            tableBitCount[(int) FilterNodeColumnMaskTable.LemonStandOrderSearch] = 4;
            tableBitCount[(int) FilterNodeColumnMaskTable.OrderSearch] = 7;
            tableBitCount[(int) FilterNodeColumnMaskTable.MagentoOrderSearch] = 4;
            tableBitCount[(int) FilterNodeColumnMaskTable.MarketplaceAdvisorOrderSearch] = 5;
            tableBitCount[(int) FilterNodeColumnMaskTable.NetworkSolutionsOrderSearch] = 4;
            tableBitCount[(int) FilterNodeColumnMaskTable.OrderMotionOrderSearch] = 4;
            tableBitCount[(int) FilterNodeColumnMaskTable.PayPalOrderSearch] = 4;
            tableBitCount[(int) FilterNodeColumnMaskTable.ProStoresOrderSearch] = 4;
            tableBitCount[(int) FilterNodeColumnMaskTable.SearsOrderSearch] = 4;
            tableBitCount[(int) FilterNodeColumnMaskTable.ShopifyOrderSearch] = 4;
            tableBitCount[(int) FilterNodeColumnMaskTable.ThreeDCartOrderSearch] = 4;
            tableBitCount[(int) FilterNodeColumnMaskTable.WalmartOrderSearch] = 5;
            tableBitCount[(int) FilterNodeColumnMaskTable.YahooOrderSearch] = 4;
            tableBitCount[(int) FilterNodeColumnMaskTable.GenericModuleOrder] = 6;
            tableBitCount[(int) FilterNodeColumnMaskTable.OverstockOrder] = 4;

            tableBitOffset = new int[Enum.GetValues(typeof(FilterNodeColumnMaskTable)).Length];
            tableBitOffset[0] = 0;
            for (int i = 1; i < tableBitOffset.Length; i++)
            {
                tableBitOffset[i] = tableBitCount[i - 1] + tableBitOffset[i - 1];
            }

            int totalBits = tableBitCount.Sum();
            totalBytes = GetBytesRequired(totalBits);
        }

        /// <summary>
        /// Get the total number of bytes required to hold the given bitcount
        /// </summary>
        private static int GetBytesRequired(int bits)
        {
            return ((bits - 1) / 8) + 1;
        }

        /// <summary>
        /// Get the total number of bytes required by the column mask
        /// </summary>
        public static int TotalBytes
        {
            get { return totalBytes; }
        }

        /// <summary>
        /// Get the total number of bits that we used to track columns for each table
        /// </summary>
        public static int GetTableBitCount(FilterNodeColumnMaskTable table)
        {
            return tableBitCount[(int) table];
        }

        /// <summary>
        /// Get the bit mask position to represent the given table and column
        /// </summary>
        public static int GetBitPosition(FilterNodeColumnMaskTable table, int column)
        {
            return tableBitOffset[(int) table] + column;
        }

        /// <summary>
        /// Convert the given BitArray to a byte[] bitmask that can be stored in sql server
        /// </summary>
        public static byte[] ConvertBitArrayToBitmask(BitArray bits)
        {
            byte[] mask = new byte[GetBytesRequired(bits.Length)];
            bits.CopyTo(mask, 0);

            return mask.Reverse().SkipWhile(b => b == 0).ToArray();
        }

        /// <summary>
        /// Create a new bitmask that is the union if the given lists of bitmasks
        /// </summary>
        public static byte[] CreateUnionedBitmask(List<byte[]> masks)
        {
            if (masks.Count == 0)
            {
                return new byte[0];
            }

            byte[] unionMask = new byte[masks.Max(mask => mask.Length)];

            foreach (byte[] mask in masks)
            {
                for (int i = 1; i <= mask.Length; i++)
                {
                    unionMask[unionMask.Length - i] |= mask[mask.Length - i];
                }
            }

            return unionMask;
        }

        /// <summary>
        /// Determines if any column from the specified table has a bit set in the given mask
        /// </summary>
        public static bool HasAnyTableBitsSet(byte[] mask, FilterNodeColumnMaskTable table)
        {
            for (int i = 0; i < GetTableBitCount(table); i++)
            {
                int bitNumber = GetBitPosition(table, i);

                int byteBit;
                int byteNumber = Math.DivRem(bitNumber, 8, out byteBit);

                if (byteNumber >= mask.Length)
                {
                    return false;
                }

                if ((mask[mask.Length - 1 - byteNumber] & (byte) Math.Pow(2, byteBit)) != 0)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
