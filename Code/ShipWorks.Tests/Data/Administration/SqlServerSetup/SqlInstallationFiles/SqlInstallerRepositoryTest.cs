using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Internal;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Data.Administration.SqlServerSetup.SqlInstallationFiles;
using Xunit;

namespace ShipWorks.Tests.Data.Administration.SqlServerSetup.SqlInstallationFiles
{
    public class SqlInstallerRepositoryTest
    {
        SqlInstallerRepository testObject;
        private Mock<IClrHelper> clrHelper;
        private Mock<IEnvironmentWrapper> environment;
        private List<Version> allClrVersions;
        private List<OperatingSystem> allOperatingSystems;

        private static OperatingSystem Xp_5_1_2600_32Bit = new OperatingSystem(PlatformID.Win32NT, new Version(5, 1, 2600, 0));
        private static OperatingSystem Xp_5_2_2600_64Bit = new OperatingSystem(PlatformID.Win32NT, new Version(5, 2, 2600, 0));
        private static OperatingSystem Vista_6_0_6002 =    new OperatingSystem(PlatformID.Win32NT, new Version(6, 0, 6002, 0));
        private static OperatingSystem Win7_6_1_7601 =     new OperatingSystem(PlatformID.Win32NT, new Version(6, 1, 7601, 0));
        private static OperatingSystem Win8_6_2_9200 =     new OperatingSystem(PlatformID.Win32NT, new Version(6, 2, 9200, 0));
        private static OperatingSystem Win81_6_3_9600 =     new OperatingSystem(PlatformID.Win32NT, new Version(6, 3, 9600, 0));
        private static OperatingSystem Win10_10_0_14393 =  new OperatingSystem(PlatformID.Win32NT, new Version(10, 0, 14393, 0));

        private static OperatingSystem Server2003_5_2 =   new OperatingSystem(PlatformID.Win32NT, new Version(5, 2, 0, 0)); // 2003
        private static OperatingSystem Server2008_6_0 =   new OperatingSystem(PlatformID.Win32NT, new Version(6, 0, 0, 0)); // 2008
        private static OperatingSystem Server2008R2_6_1 = new OperatingSystem(PlatformID.Win32NT, new Version(6, 1, 0, 0)); // 2008 R2
        private static OperatingSystem Server2012_6_2 =   new OperatingSystem(PlatformID.Win32NT, new Version(6, 2, 0, 0)); // 2012
        private static OperatingSystem Server2012R2_6_3 = new OperatingSystem(PlatformID.Win32NT, new Version(6, 3, 0, 0)); // 2012 R2
        private static OperatingSystem Server2016_10_0 =  new OperatingSystem(PlatformID.Win32NT, new Version(10, 0, 0, 0)); // 2016

        public SqlInstallerRepositoryTest()
        {
            clrHelper = new Mock<IClrHelper>();
            environment = new Mock<IEnvironmentWrapper>();

            allClrVersions = new List<Version>
            {
                new Version(1, 0, 0, 0),
                new Version(2, 0, 50727, 4927),
                new Version(3, 0, 30729, 4926),
                new Version(3, 5, 0, 0),
                new Version(3, 5, 30729, 4926),
                new Version(4, 0, 0, 0),
                new Version(4, 5, 0, 0),
                new Version(4, 5, 1, 0),
                new Version(4, 5, 2, 0),
                new Version(4, 6, 0, 0),
                new Version(4, 6, 1, 0),
                new Version(4, 6, 2, 0)
            };

            allOperatingSystems = new List<OperatingSystem>
            {
                Xp_5_1_2600_32Bit,
                Xp_5_2_2600_64Bit,
                Vista_6_0_6002,
                Win7_6_1_7601,
                Win8_6_2_9200,
                Win81_6_3_9600,
                Win10_10_0_14393,
                
                // Servers
                Server2003_5_2,
                Server2008_6_0,
                Server2008R2_6_1,
                Server2012_6_2,
                Server2012R2_6_3,
                Server2016_10_0,
            };

            clrHelper.Setup(ch => ch.ClrVersions)
                     .Returns(allClrVersions);

            environment.Setup(e => e.Is64BitOperatingSystem).Returns(true);
            environment.Setup(e => e.OSVersion).Returns(allOperatingSystems.First());
        }

        [Fact]
        public void Windows10x64_SqlServer2016LocalDB_IsFirst()
        {
            environment.Setup(e => e.Is64BitOperatingSystem).Returns(true);
            environment.Setup(e => e.OSVersion).Returns(Win10_10_0_14393);

            testObject = new SqlInstallerRepository(clrHelper.Object, environment.Object);

            ISqlInstallerInfo si = testObject.SqlInstallersForThisMachine().First();
            Assert.True(si.Edition == SqlServerEditionType.LocalDb2016 && si.Is64Bit);
        }

        [Fact]
        public void Windows10x64_SqlServer2016ExpressIsAvailable()
        {
            environment.Setup(e => e.Is64BitOperatingSystem).Returns(true);
            environment.Setup(e => e.OSVersion).Returns(Win10_10_0_14393);

            testObject = new SqlInstallerRepository(clrHelper.Object, environment.Object);

            Assert.True(testObject.SqlInstallersForThisMachine().Any(si => si.Edition == SqlServerEditionType.Express2016 && si.Is64Bit));
        }

        [Fact]
        public void Windows10x86_SqlServer2016IsNotAvailable()
        {
            environment.Setup(e => e.Is64BitOperatingSystem).Returns(false);
            environment.Setup(e => e.OSVersion).Returns(Win10_10_0_14393);

            testObject = new SqlInstallerRepository(clrHelper.Object, environment.Object);

            IEnumerable<ISqlInstallerInfo> sqlInstallerInfos = testObject.SqlInstallersForThisMachine();

            Assert.True(sqlInstallerInfos.Any());
            Assert.True(sqlInstallerInfos.None(si => si.Edition == SqlServerEditionType.Express2016 || si.Edition == SqlServerEditionType.LocalDb2016));
        }

        [Fact]
        public void Windows10x86_SqlServer2014LocalDB_IsFirst()
        {
            environment.Setup(e => e.Is64BitOperatingSystem).Returns(false);
            environment.Setup(e => e.OSVersion).Returns(Win10_10_0_14393);

            testObject = new SqlInstallerRepository(clrHelper.Object, environment.Object);

            ISqlInstallerInfo si = testObject.SqlInstallersForThisMachine().First();
            Assert.True(si.Is64Bit == false && si.Edition == SqlServerEditionType.LocalDb2014);
        }

        [Fact]
        public void Windows10x86_SqlServer2014Express_IsAvailable()
        {
            environment.Setup(e => e.Is64BitOperatingSystem).Returns(false);
            environment.Setup(e => e.OSVersion).Returns(Win10_10_0_14393);

            testObject = new SqlInstallerRepository(clrHelper.Object, environment.Object);

            Assert.True(testObject.SqlInstallersForThisMachine().Any(si => si.Edition == SqlServerEditionType.Express2014 && !si.Is64Bit));
        }

        [Theory]
        [InlineData(false, SqlServerEditionType.LocalDb2014)]
        [InlineData(true, SqlServerEditionType.LocalDb2014)]
        public void Win7_SqlServerLocalDb2014_IsFirst(bool is64Bit, SqlServerEditionType expectedEdition)
        {
            environment.Setup(e => e.Is64BitOperatingSystem).Returns(is64Bit);
            environment.Setup(e => e.OSVersion).Returns(Win7_6_1_7601);

            testObject = new SqlInstallerRepository(clrHelper.Object, environment.Object);

            ISqlInstallerInfo si = testObject.SqlInstallersForThisMachine().First();
            Assert.True(si.Is64Bit == is64Bit && si.Edition == expectedEdition);
        }

        [Theory]
        [InlineData(false, SqlServerEditionType.LocalDb2014)]
        [InlineData(true, SqlServerEditionType.LocalDb2014)]
        public void Win7_SqlServerLocalDb2014AndExpress_AreAvailable(bool is64Bit, SqlServerEditionType expectedEdition)
        {
            environment.Setup(e => e.Is64BitOperatingSystem).Returns(is64Bit);
            environment.Setup(e => e.OSVersion).Returns(Win7_6_1_7601);

            testObject = new SqlInstallerRepository(clrHelper.Object, environment.Object);

            IEnumerable<ISqlInstallerInfo> sqlInstallerInfos = testObject.SqlInstallersForThisMachine();

            Assert.Equal(2, sqlInstallerInfos.Count(si => si.Edition == SqlServerEditionType.Express2014 ||
                                                    si.Edition == SqlServerEditionType.LocalDb2014));

            Assert.True(sqlInstallerInfos.None(si => si.Edition == SqlServerEditionType.Express2016 ||
                                                     si.Edition == SqlServerEditionType.LocalDb2016));
        }


        [Theory]
        [InlineData(true, SqlServerEditionType.LocalDb2016)]
        public void Server2012_SqlServerLocalDb2016_IsFirst(bool is64Bit, SqlServerEditionType expectedEdition)
        {
            environment.Setup(e => e.Is64BitOperatingSystem).Returns(is64Bit);
            environment.Setup(e => e.OSVersion).Returns(Server2012_6_2);

            testObject = new SqlInstallerRepository(clrHelper.Object, environment.Object);

            ISqlInstallerInfo si = testObject.SqlInstallersForThisMachine().First();
            Assert.True(si.Is64Bit == is64Bit && si.Edition == expectedEdition);
        }

        [Theory]
        [InlineData(true, SqlServerEditionType.LocalDb2016)]
        public void Server2012_SqlServerLocalDb2016AndExpress_AreAvailable(bool is64Bit, SqlServerEditionType expectedEdition)
        {
            environment.Setup(e => e.Is64BitOperatingSystem).Returns(is64Bit);
            environment.Setup(e => e.OSVersion).Returns(Server2012_6_2);

            testObject = new SqlInstallerRepository(clrHelper.Object, environment.Object);

            IEnumerable<ISqlInstallerInfo> sqlInstallerInfos = testObject.SqlInstallersForThisMachine();

            Assert.Equal(3, sqlInstallerInfos.Count(si => si.Edition == SqlServerEditionType.Express2016 ||
                                                          si.Edition == SqlServerEditionType.LocalDb2014 ||
                                                          si.Edition == SqlServerEditionType.Express2014));
        }

        [Theory]
        [InlineData(true, SqlServerEditionType.LocalDb2016)]
        public void Server2012R2_SqlServerLocalDb2016_IsFirst(bool is64Bit, SqlServerEditionType expectedEdition)
        {
            environment.Setup(e => e.Is64BitOperatingSystem).Returns(is64Bit);
            environment.Setup(e => e.OSVersion).Returns(Server2012R2_6_3);

            testObject = new SqlInstallerRepository(clrHelper.Object, environment.Object);

            ISqlInstallerInfo si = testObject.SqlInstallersForThisMachine().First();
            Assert.True(si.Is64Bit == is64Bit && si.Edition == expectedEdition);
        }

        [Theory]
        [InlineData(true, SqlServerEditionType.LocalDb2016)]
        public void Server2012R2_SqlServerLocalDb2016AndExpress_AreAvailable(bool is64Bit, SqlServerEditionType expectedEdition)
        {
            environment.Setup(e => e.Is64BitOperatingSystem).Returns(is64Bit);
            environment.Setup(e => e.OSVersion).Returns(Server2012R2_6_3);

            testObject = new SqlInstallerRepository(clrHelper.Object, environment.Object);

            IEnumerable<ISqlInstallerInfo> sqlInstallerInfos = testObject.SqlInstallersForThisMachine();

            Assert.Equal(3, sqlInstallerInfos.Count(si => si.Edition == SqlServerEditionType.Express2016 ||
                                                          si.Edition == SqlServerEditionType.LocalDb2014 ||
                                                          si.Edition == SqlServerEditionType.Express2014));
        }

        [Theory]
        [InlineData(true, SqlServerEditionType.LocalDb2014)]
        public void Server2012_SqlServerLocalDb2014AndExpress_AreAvailable_WhenClrIs45OrLess(bool is64Bit, SqlServerEditionType expectedEdition)
        {
            environment.Setup(e => e.Is64BitOperatingSystem).Returns(is64Bit);
            environment.Setup(e => e.OSVersion).Returns(Server2012_6_2);
            clrHelper.Setup(ch => ch.ClrVersions).Returns(allClrVersions.Where(v => v <= new Version(4, 5, 0, 0)));

            testObject = new SqlInstallerRepository(clrHelper.Object, environment.Object);

            IEnumerable<ISqlInstallerInfo> sqlInstallerInfos = testObject.SqlInstallersForThisMachine();

            Assert.Equal(2, sqlInstallerInfos.Count(si => si.Edition == SqlServerEditionType.LocalDb2014 ||
                                                          si.Edition == SqlServerEditionType.Express2014));
        }

        [Theory]
        [InlineData(true, SqlServerEditionType.LocalDb2014)]
        public void Server2012R2_SqlServerLocalDb2014AndExpress_AreAvailable_WhenClrIs451OrLess(bool is64Bit, SqlServerEditionType expectedEdition)
        {
            environment.Setup(e => e.Is64BitOperatingSystem).Returns(is64Bit);
            environment.Setup(e => e.OSVersion).Returns(Server2012R2_6_3);
            clrHelper.Setup(ch => ch.ClrVersions).Returns(allClrVersions.Where(v => v <= new Version(4, 5, 1, 0)));

            testObject = new SqlInstallerRepository(clrHelper.Object, environment.Object);

            IEnumerable<ISqlInstallerInfo> sqlInstallerInfos = testObject.SqlInstallersForThisMachine();

            Assert.Equal(2, sqlInstallerInfos.Count(si => si.Edition == SqlServerEditionType.LocalDb2014 ||
                                                          si.Edition == SqlServerEditionType.Express2014));
        }

        [Theory]
        [InlineData(false, SqlServerEditionType.LocalDb2014)]
        public void Win8_SqlServerLocalDb2014_IsFirst_WhenNot64Bit(bool is64Bit, SqlServerEditionType expectedEdition)
        {
            environment.Setup(e => e.Is64BitOperatingSystem).Returns(is64Bit);
            environment.Setup(e => e.OSVersion).Returns(Win8_6_2_9200);

            testObject = new SqlInstallerRepository(clrHelper.Object, environment.Object);

            ISqlInstallerInfo si = testObject.SqlInstallersForThisMachine().First();
            Assert.True(si.Is64Bit == is64Bit && si.Edition == expectedEdition);
        }

        [Theory]
        [InlineData(false, SqlServerEditionType.LocalDb2014)]
        public void Win8_SqlServerLocalDb2014AndExpress_AreAvailable(bool is64Bit, SqlServerEditionType expectedEdition)
        {
            environment.Setup(e => e.Is64BitOperatingSystem).Returns(is64Bit);
            environment.Setup(e => e.OSVersion).Returns(Win8_6_2_9200);

            testObject = new SqlInstallerRepository(clrHelper.Object, environment.Object);

            IEnumerable<ISqlInstallerInfo> sqlInstallerInfos = testObject.SqlInstallersForThisMachine();

            Assert.Equal(2, sqlInstallerInfos.Count(si => si.Edition == SqlServerEditionType.Express2014 ||
                                                    si.Edition == SqlServerEditionType.LocalDb2014));

            Assert.True(sqlInstallerInfos.None(si => si.Edition == SqlServerEditionType.Express2016 ||
                                                     si.Edition == SqlServerEditionType.LocalDb2016));
        }

        [Theory]
        [InlineData(true, SqlServerEditionType.LocalDb2016)]
        public void Win8_SqlServerLocalDb2016_IsFirst_When64Bit(bool is64Bit, SqlServerEditionType expectedEdition)
        {
            environment.Setup(e => e.Is64BitOperatingSystem).Returns(is64Bit);
            environment.Setup(e => e.OSVersion).Returns(Win8_6_2_9200);

            testObject = new SqlInstallerRepository(clrHelper.Object, environment.Object);

            ISqlInstallerInfo si = testObject.SqlInstallersForThisMachine().First();
            Assert.True(si.Is64Bit == is64Bit && si.Edition == expectedEdition);
        }

        [Theory]
        [InlineData(true, SqlServerEditionType.LocalDb2016)]
        public void Win8_SqlServerLocalDb2016AndExpress_AreAvailable_When64Bit(bool is64Bit, SqlServerEditionType expectedEdition)
        {
            environment.Setup(e => e.Is64BitOperatingSystem).Returns(is64Bit);
            environment.Setup(e => e.OSVersion).Returns(Win8_6_2_9200);

            testObject = new SqlInstallerRepository(clrHelper.Object, environment.Object);

            IEnumerable<ISqlInstallerInfo> sqlInstallerInfos = testObject.SqlInstallersForThisMachine();

            Assert.Equal(4, sqlInstallerInfos.Count(si => si.Edition == SqlServerEditionType.Express2016 ||
                                                     si.Edition == SqlServerEditionType.LocalDb2016 ||
                                                     si.Edition == SqlServerEditionType.LocalDb2014 ||
                                                     si.Edition == SqlServerEditionType.Express2014));

            Assert.True(sqlInstallerInfos.None(si => !si.Is64Bit));
        }

        [Theory]
        [InlineData(false, SqlServerEditionType.LocalDb2014)]
        public void Win81_SqlServerLocalDb2014_IsFirst_WhenNot64Bit(bool is64Bit, SqlServerEditionType expectedEdition)
        {
            environment.Setup(e => e.Is64BitOperatingSystem).Returns(is64Bit);
            environment.Setup(e => e.OSVersion).Returns(Win81_6_3_9600);

            testObject = new SqlInstallerRepository(clrHelper.Object, environment.Object);

            ISqlInstallerInfo si = testObject.SqlInstallersForThisMachine().First();
            Assert.True(si.Is64Bit == is64Bit && si.Edition == expectedEdition);
        }

        [Theory]
        [InlineData(false, SqlServerEditionType.LocalDb2014)]
        public void Win81_SqlServerLocalDb2014AndExpress_AreAvailable(bool is64Bit, SqlServerEditionType expectedEdition)
        {
            environment.Setup(e => e.Is64BitOperatingSystem).Returns(is64Bit);
            environment.Setup(e => e.OSVersion).Returns(Win81_6_3_9600);

            testObject = new SqlInstallerRepository(clrHelper.Object, environment.Object);

            IEnumerable<ISqlInstallerInfo> sqlInstallerInfos = testObject.SqlInstallersForThisMachine();

            Assert.Equal(2, sqlInstallerInfos.Count(si => si.Edition == SqlServerEditionType.Express2014 ||
                                                    si.Edition == SqlServerEditionType.LocalDb2014));

            Assert.True(sqlInstallerInfos.None(si => si.Edition == SqlServerEditionType.Express2016 ||
                                                     si.Edition == SqlServerEditionType.LocalDb2016));
        }

        [Theory]
        [InlineData(true, SqlServerEditionType.LocalDb2016)]
        public void Win81_SqlServerLocalDb2016_IsFirst_When64Bit(bool is64Bit, SqlServerEditionType expectedEdition)
        {
            environment.Setup(e => e.Is64BitOperatingSystem).Returns(is64Bit);
            environment.Setup(e => e.OSVersion).Returns(Win81_6_3_9600);

            testObject = new SqlInstallerRepository(clrHelper.Object, environment.Object);

            ISqlInstallerInfo si = testObject.SqlInstallersForThisMachine().First();
            Assert.True(si.Is64Bit == is64Bit && si.Edition == expectedEdition);
        }

        [Theory]
        [InlineData(true, SqlServerEditionType.LocalDb2016)]
        public void Win81_SqlServerLocalDb2016AndExpress_AreAvailable_When64Bit(bool is64Bit, SqlServerEditionType expectedEdition)
        {
            environment.Setup(e => e.Is64BitOperatingSystem).Returns(is64Bit);
            environment.Setup(e => e.OSVersion).Returns(Win81_6_3_9600);

            testObject = new SqlInstallerRepository(clrHelper.Object, environment.Object);

            IEnumerable<ISqlInstallerInfo> sqlInstallerInfos = testObject.SqlInstallersForThisMachine();

            Assert.Equal(4, sqlInstallerInfos.Count(si => si.Edition == SqlServerEditionType.Express2016 ||
                                                     si.Edition == SqlServerEditionType.LocalDb2016 ||
                                                     si.Edition == SqlServerEditionType.LocalDb2014 ||
                                                     si.Edition == SqlServerEditionType.Express2014));

            Assert.True(sqlInstallerInfos.None(si => !si.Is64Bit));
        }

        [Theory]
        [InlineData(true, SqlServerEditionType.LocalDb2016)]
        public void Server2016_SqlServerLocalDb2016_IsFirst_When64Bit(bool is64Bit, SqlServerEditionType expectedEdition)
        {
            environment.Setup(e => e.Is64BitOperatingSystem).Returns(is64Bit);
            environment.Setup(e => e.OSVersion).Returns(Server2016_10_0);

            testObject = new SqlInstallerRepository(clrHelper.Object, environment.Object);

            ISqlInstallerInfo si = testObject.SqlInstallersForThisMachine().First();
            Assert.True(si.Is64Bit == is64Bit && si.Edition == expectedEdition);
        }

        [Theory]
        [InlineData(true, SqlServerEditionType.LocalDb2016)]
        public void Server2016_SqlServerLocalDb2016AndExpress_AreAvailable_When64Bit(bool is64Bit, SqlServerEditionType expectedEdition)
        {
            environment.Setup(e => e.Is64BitOperatingSystem).Returns(is64Bit);
            environment.Setup(e => e.OSVersion).Returns(Server2016_10_0);

            testObject = new SqlInstallerRepository(clrHelper.Object, environment.Object);

            IEnumerable<ISqlInstallerInfo> sqlInstallerInfos = testObject.SqlInstallersForThisMachine();

            Assert.Equal(4, sqlInstallerInfos.Count(si => si.Edition == SqlServerEditionType.Express2016 ||
                                                     si.Edition == SqlServerEditionType.LocalDb2016 ||
                                                     si.Edition == SqlServerEditionType.LocalDb2014 ||
                                                     si.Edition == SqlServerEditionType.Express2014));

            Assert.True(sqlInstallerInfos.None(si => !si.Is64Bit));
        }
    }
}
