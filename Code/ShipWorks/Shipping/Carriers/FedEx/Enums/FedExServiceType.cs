﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace ShipWorks.Shipping.Carriers.FedEx.Enums
{
    /// <summary>
    /// Valid FedEx service type values
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum FedExServiceType
    {
        [Description("FedEx Priority Overnight®")]
        PriorityOvernight = 0,

        [Description("FedEx Standard Overnight®")]
        StandardOvernight = 1,

        [Description("FedEx First Overnight®")]
        FirstOvernight = 2,

        [Description("FedEx 2Day®")]
        FedEx2Day = 3,

        [Description("FedEx Express Saver®")]
        FedExExpressSaver = 4,

        [Description("FedEx International Priority®")]
        InternationalPriority = 5,

        [Description("FedEx International Economy®")]
        InternationalEconomy = 6,

        [Description("FedEx International First®")]
        InternationalFirst = 7,

        [Description("FedEx 1Day® Freight")]
        FedEx1DayFreight = 8,

        [Description("FedEx 2Day® Freight")]
        FedEx2DayFreight = 9,

        [Description("FedEx 3Day® Freight")]
        FedEx3DayFreight = 10,

        [Description("FedEx Ground®")]
        FedExGround = 11,

        [Description("FedEx Home Delivery®")]
        GroundHomeDelivery = 12,

        [Description("FedEx International Priority® Freight")]
        InternationalPriorityFreight = 13,

        [Description("FedEx International Economy® Freight")]
        InternationalEconomyFreight = 14,

        [Description("FedEx SmartPost®")]
        SmartPost = 15,

        [Description("FedEx Europe First International Priority®")]
        FedExEuropeFirstInternationalPriority = 17,

        [Description("FedEx 2Day® A.M.")]
        FedEx2DayAM = 18,

        [Description("FedEx First Overnight® Freight")]
        FirstFreight = 19,

        [Description("FedEx One Rate® (First Overnight)")]
        OneRateFirstOvernight = 20,

        [Description("FedEx One Rate® (Priority Overnight)")]
        OneRatePriorityOvernight = 21,

        [Description("FedEx One Rate® (Standard Overnight)")]
        OneRateStandardOvernight = 22,

        [Description("FedEx One Rate® (2Day)")]
        OneRate2Day = 23,

        [Description("FedEx One Rate® (2Day A.M.)")]
        OneRate2DayAM = 24,

        [Description("FedEx One Rate® (Express Saver)")]
        OneRateExpressSaver = 25,

        [Description("FedEx Economy")]
        FedExEconomyCanada = 26,

        [Description("FedEx FIMS")]
        FedExFims = 27,

        [Description("FedEx International Ground®")]
        FedExInternationalGround = 28,

        [Description("FedEx International Ground® Distribution")]
        FedExInternationalGroundDistribution = 29,

        [Description("FedEx International DirectDistribution®")]
        FedExInternationalDirectDistribution = 30,

        [Description("FedEx International Economy DirectDistribution®")]
        FedExInternationalEconomyDirectDistribution = 31,

        [Description("FedEx International Priority DirectDistribution®")]
        FedExInternationalPriorityDirectDistribution = 32,

        [Description("FedEx International DirectDistribution® Surface Solutions U.S. to Canada")]
        FedExInternationalDirectDistributionSurfaceSolutionsUStoCanada = 33,

        [Description("FedEx International DirectDistribution® Freight")]
        FedExInternationalDirectDistributionFreight = 34,

        // Services needed for Intra certification tests
        [Description("FedEx Next Day Afternoon")]
        FedExNextDayAfternoon = 35,

        [Description("FedEx Next Day Early Morning")]
        FedExNextDayEarlyMorning = 36,

        [Description("FedEx Next Day Mid Morning")]
        FedExNextDayMidMorning = 37,

        [Description("FedEx Next Day End Of Day")]
        FedExNextDayEndOfDay = 38,

        [Description("FedEx Distance Deferred")]
        FedExDistanceDeferred = 39,

        [Description("FedEx Next Day Freight")]
        FedExNextDayFreight = 40

    }
}
