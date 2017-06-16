using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Collections;
using ShipWorks.Shipping.Carriers.Ups.LocalRating;
using ShipWorks.Tests.Shared;
using Xunit;
using Xunit.Abstractions;

namespace ShipWorks.Shipping.Tests.Carriers.UPS.LocalRating
{
    public class UpsLocalRateExcelReaderFactoryTest
    {
        private readonly ITestOutputHelper output;
        private readonly UpsLocalRateExcelReaderFactory testObject;
        
        public UpsLocalRateExcelReaderFactoryTest(ITestOutputHelper output)
        {
            this.output = output;
            testObject = new UpsLocalRateExcelReaderFactory();
        }

        [Fact]
        public void CreateZoneExcelReaders_ReturnsAllImplementationsOfIUpsZoneExcelReader()
        {
            var zoneExcelReaders = AssemblyProvider.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(type => type.GetInterfaces().Any(i => i.FullName == "ShipWorks.Shipping.Carriers.Ups.LocalRating.IUpsZoneExcelReader"))
                .ToList();

            IEnumerable<Type> returnedReaders = testObject.CreateZoneExcelReaders().Select(reader => reader.GetType()).ToArray();

            var missingReaderMessage = new StringBuilder();
            zoneExcelReaders
                .Where(readerFromAssembly => returnedReaders.None(returnedReader => returnedReader != readerFromAssembly))
                .ForEach(missingReader=>missingReaderMessage.AppendLine($"Missing {missingReader.FullName}"));
            output.WriteLine(missingReaderMessage.ToString());

            Assert.Empty(missingReaderMessage.ToString());
            Assert.Equal(zoneExcelReaders.Count(), returnedReaders.Count());
        }

        [Fact]
        public void CreateRateExcelReaders_ReturnsAllImplementationsOfIUpsRateExcelReader()
        {
            IEnumerable<Type> rateExcelReaders = AssemblyProvider.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(type => type.GetInterfaces().Any(i => i.FullName == "ShipWorks.Shipping.Carriers.Ups.LocalRating.IUpsRateExcelReader"))
                .ToList();

            IEnumerable<Type> returnedReaders = testObject.CreateRateExcelReaders().Select(reader => reader.GetType()).ToArray();

            var missingReaderMessage = new StringBuilder();
            rateExcelReaders
                .Where(readerFromAssembly => returnedReaders.None(returnedReader => returnedReader != readerFromAssembly))
                .ForEach(missingReader => missingReaderMessage.AppendLine($"Missing {missingReader.FullName}"));
            output.WriteLine(missingReaderMessage.ToString());

            Assert.Empty(missingReaderMessage.ToString());
            Assert.Equal(rateExcelReaders.Count(), returnedReaders.Count());
        }
    }
}