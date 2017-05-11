using System;
using System.Collections.Generic;
using System.Linq;
using ShipWorks.Shipping.Carriers.Ups.LocalRating;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.UPS.LocalRating
{
    public class UpsLocalRateExcelReaderFactoryTest
    {
        private readonly UpsLocalRateExcelReaderFactory testObject;
        private readonly IEnumerable<Type> rateExcelReaders;
        private readonly IEnumerable<Type> zoneExcelReaders;

        public UpsLocalRateExcelReaderFactoryTest()
        {
            testObject = new UpsLocalRateExcelReaderFactory();
            
            rateExcelReaders = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(r => typeof(IUpsRateExcelReader).IsAssignableFrom(r) && r != typeof(IUpsRateExcelReader));
            
            zoneExcelReaders = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(r => typeof(IUpsZoneExcelReader).IsAssignableFrom(r) && r != typeof(IUpsZoneExcelReader));
        }

        [Fact]
        public void CreateZoneExcelReaders_ReturnsAllImplementationsOfIUpsZoneExcelReader()
        {
            IEnumerable<Type> readers = testObject.CreateZoneExcelReaders().Select(reader => reader.GetType()).ToArray();
            
            Assert.True(readers.All(r => zoneExcelReaders.Contains(r)) && readers.Count() == rateExcelReaders.Count());
        }

        [Fact]
        public void CreateRateExcelReaders_ReturnsAllImplementationsOfIUpsRateExcelReader()
        {
            IEnumerable<Type> readers = testObject.CreateRateExcelReaders().Select(reader => reader.GetType()).ToArray();
            
            Assert.True(readers.All(r => rateExcelReaders.Contains(r)) && readers.Count() == rateExcelReaders.Count());
        }
    }
}