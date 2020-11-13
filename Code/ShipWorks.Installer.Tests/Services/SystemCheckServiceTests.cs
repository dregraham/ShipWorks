using System;
using log4net;
using Moq;
using Xunit;

namespace ShipWorks.Installer.Services.Tests
{
    public class SystemCheckServiceTests
    {
        private readonly Func<Type, ILog> logFactory = new Func<Type, ILog>((type) => LogManager.GetLogger(type));

        [Theory]
        [InlineData("Microsoft Windows 10.0.14393", true)]
        [InlineData("Windows Server 2012 R2.0.9600", true)]
        [InlineData("Microsoft Windows 8.0.9200", false)]
        [InlineData("Windows Server 2008 R2.0.7600", false)]
        public void SystemCheck_ReturnsTrue_ForGoodOS(string osVersion, bool expectedResult)
        {
            var systemInfo = new Mock<ISystemInfoService>();
            systemInfo.Setup(s => s.GetOsDescription()).Returns(osVersion);
            var service = new SystemCheckService(systemInfo.Object, logFactory);
            var result = service.CheckSystem();
            Assert.Equal(result.OsMeetsRequirement, expectedResult);
        }

        [Theory]
        [InlineData("MaxClockSpeed=2304\r\r\nNumberOfCores=8", true)]
        [InlineData("MaxClockSpeed=1500\r\r\nNumberOfCores=2", true)]
        [InlineData("MaxClockSpeed=1500\r\r\nNumberOfCores=1", false)]
        [InlineData("MaxClockSpeed=1000\r\r\nNumberOfCores=1", false)]
        public void SystemCheck_TestsCPU(string cpuInfo, bool expectedResult)
        {
            var systemInfo = new Mock<ISystemInfoService>();
            systemInfo.Setup(s => s.GetCPUInfo()).Returns(cpuInfo);
            var service = new SystemCheckService(systemInfo.Object, logFactory);
            var result = service.CheckSystem();
            Assert.Equal(result.CpuMeetsRequirement, expectedResult);
        }

        [Theory]
        [InlineData("TotalVisibleMemorySize=66829652", true)]
        [InlineData("TotalVisibleMemorySize=4194304", true)]
        [InlineData("TotalVisibleMemorySize=2097152", false)]
        public void SystemCheck_TestsRAM(string ramInfo, bool expectedResult)
        {
            var systemInfo = new Mock<ISystemInfoService>();
            systemInfo.Setup(s => s.GetRamInfo()).Returns(ramInfo);
            var service = new SystemCheckService(systemInfo.Object, logFactory);
            var result = service.CheckSystem();
            Assert.Equal(result.RamMeetsRequirement, expectedResult);
        }

        [Theory]
        [InlineData(782845734912, true)]
        [InlineData(25249710080, true)]
        [InlineData(20000000000, false)]
        public void SystemCheck_TestsHDD(long driveSpace, bool expectedResult)
        {
            var driveInfo = new Mock<IDriveInfo>();
            driveInfo.Setup(d => d.AvailableFreeSpace).Returns(driveSpace);
            var systemInfo = new Mock<ISystemInfoService>();
            systemInfo.Setup(s => s.GetDriveInfo()).Returns(new IDriveInfo[] { driveInfo.Object });
            var service = new SystemCheckService(systemInfo.Object, logFactory);
            var result = service.CheckSystem();
            Assert.Equal(result.HddMeetsRequirement, expectedResult);
        }

        [Fact]
        public void SystemCheck_DoesOtherTestsWhen_TestRamThrows()
        {
            var systemInfo = new Mock<ISystemInfoService>();
            systemInfo.Setup(s => s.GetRamInfo()).Throws(new System.Exception("Test Exception"));
            var service = new SystemCheckService(systemInfo.Object, logFactory);
            var result = service.CheckSystem();
            systemInfo.Verify(s => s.GetOsDescription());
            systemInfo.Verify(s => s.GetDriveInfo());
            systemInfo.Verify(s => s.GetCPUInfo());
            Assert.False(result.RamMeetsRequirement);
        }

        [Fact]
        public void SystemCheck_DoesOtherTestsWhen_TestCPUThrows()
        {
            var systemInfo = new Mock<ISystemInfoService>();
            systemInfo.Setup(s => s.GetCPUInfo()).Throws(new System.Exception("Test Exception"));
            var service = new SystemCheckService(systemInfo.Object, logFactory);
            var result = service.CheckSystem();
            systemInfo.Verify(s => s.GetRamInfo());
            systemInfo.Verify(s => s.GetDriveInfo());
            systemInfo.Verify(s => s.GetOsDescription());
            Assert.False(result.CpuMeetsRequirement);
        }

        [Fact]
        public void SystemCheck_DoesOtherTestsWhen_TestHDDThrows()
        {
            var systemInfo = new Mock<ISystemInfoService>();
            systemInfo.Setup(s => s.GetDriveInfo()).Throws(new System.Exception("Test Exception"));
            var service = new SystemCheckService(systemInfo.Object, logFactory);
            var result = service.CheckSystem();
            systemInfo.Verify(s => s.GetCPUInfo());
            systemInfo.Verify(s => s.GetRamInfo());
            systemInfo.Verify(s => s.GetOsDescription());
            Assert.False(result.HddMeetsRequirement);
        }

        [Fact]
        public void SystemCheck_DoesOtherTestsWhen_TestOSThrows()
        {
            var systemInfo = new Mock<ISystemInfoService>();
            systemInfo.Setup(s => s.GetRamInfo()).Throws(new System.Exception("Test Exception"));
            var service = new SystemCheckService(systemInfo.Object, logFactory);
            var result = service.CheckSystem();
            systemInfo.Verify(s => s.GetRamInfo());
            systemInfo.Verify(s => s.GetDriveInfo());
            systemInfo.Verify(s => s.GetCPUInfo());
            Assert.False(result.OsMeetsRequirement);
        }

        [Theory]
        [InlineData("C:\\", true)]
        [InlineData("c:\\", true)]
        [InlineData("D:\\", false)]
        [InlineData("d:\\", false)]
        [InlineData("E:\\", false)]
        [InlineData("e:\\", false)]
        public void DriveMeetsRequirements_Test(string driveLetter, bool expectedResult)
        {
            var driveCInfo = new Mock<IDriveInfo>();
            driveCInfo.Setup(d => d.AvailableFreeSpace).Returns(25249710080);
            driveCInfo.Setup(d => d.Name).Returns("C:\\");

            var driveDInfo = new Mock<IDriveInfo>();
            driveDInfo.Setup(d => d.AvailableFreeSpace).Returns(20000000000);
            driveDInfo.Setup(d => d.Name).Returns("D:\\");

            var systemInfo = new Mock<ISystemInfoService>();
            systemInfo.Setup(s => s.GetDriveInfo()).Returns(new IDriveInfo[] { driveCInfo.Object, driveDInfo.Object });

            var service = new SystemCheckService(systemInfo.Object, logFactory);
            var result = service.DriveMeetsRequirements(driveLetter);

            Assert.Equal(result, expectedResult);
        }
    }
}