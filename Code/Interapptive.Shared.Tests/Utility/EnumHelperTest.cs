﻿using System;
using System.ComponentModel;
using System.Reflection;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Utility;
using Xunit;

namespace Interapptive.Shared.Tests.Utility
{
    public class EnumHelperTest
    {
        [Fact]
        public void Details_ReturnsDetails_WhenDetailsAreAvailable()
        {
            var details = EnumHelper.GetDetails(TestEnum.HasDetails);
            Assert.Equal("Blah", details);
        }

        [Fact]
        public void Details_ReturnsEmptyString_WhenDetailsAreNotSet()
        {
            Assert.Equal(string.Empty, EnumHelper.GetDetails(TestEnum.NoDetails));
        }

        [Fact]
        public void Description_ReturnsDescription_WhenDescriptionIsAvailable()
        {
            var details = EnumHelper.GetDescription(TestEnum.HasDetails);
            Assert.Equal("some desc", details);
        }

        [Fact]
        public void Description_ThrowsException_WhenDescriptionIsNotSet()
        {
            Assert.Throws<NullReferenceException>(() => { EnumHelper.GetDescription(TestEnum.NoDetails); });
        }

        [Fact]
        public void TryParseEnum_ReturnsNull_WhenNoMatchingEnum()
        {
            var result = EnumHelper.TryParseEnum<CurrencyType>("blah");
            Assert.Null(result);
        }

        [Fact]
        public void TryParseEnum_ReturnsMatchingEnum_WhenMatchingEnum()
        {
            var result = EnumHelper.TryParseEnum<CurrencyType>("USD");
            Assert.Equal(CurrencyType.USD, result);
        }

        [Fact]
        public void GetApiValue_ForEnum_ReturnsApiEnumValue()
        {
            OtherTestEnum? testResult = EnumHelper.GetApiValue<OtherTestEnum>(TestEnum.HasDetails);

            Assert.Equal(OtherTestEnum.BlahOnOtherEnum, testResult.Value);
        }

        [Fact]
        public void TryGetByApiValue_ForEnum_ReturnsTrueWhenValueFound()
        {
            var result = EnumHelper.TryGetEnumByApiValue("BlahOnOtherEnum", out TestEnum? retrievedEnum);
            Assert.True(result);
            Assert.True(retrievedEnum.HasValue);
            Assert.Equal(TestEnum.HasDetails, retrievedEnum.Value);
        }
        
        [Fact]
        public void TryGetByApiValue_ForEnum_ReturnsFalseWhenValueNotFound()
        {
            var result = EnumHelper.TryGetEnumByApiValue("lsjdflk", out TestEnum? retrievedEnum);
            Assert.False(result);
            Assert.False(retrievedEnum.HasValue);
        }

        [Fact]
        public void GetByApiValue_ForEnum_ReturnsApiValue()
        {
            var result = EnumHelper.GetEnumByApiValue<TestEnum>("BlahOnOtherEnum");
            
            Assert.Equal(TestEnum.HasDetails, result);
        }
    }

    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public enum TestEnum
    {
        NoDetails = 0,

        [Description("some desc")]
        [ApiValue("BlahOnOtherEnum")]
        [Details("Blah")]
        HasDetails = 1,
    }

    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public enum OtherTestEnum
    {
        [Description("BlahOnOtherEnum desc")]
        [Details("BlahOnOtherEnum")]
        BlahOnOtherEnum = 1,
    }
}
