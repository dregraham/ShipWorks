using Xunit;

namespace ShipWorks.Escalator.Tests
{
    public class InstallFileTest
    {
        private string filePath = @"Artifacts\ChecksumTestFile.txt";
        private string goodHash = "820EB62B7660A216F711BD0DF37AC8A176B662A159959870EDC200B857262DAF";
        private string badHash = "bad hash";

        [Fact]
        public void Constructor_SetsFilePath()
        {
            InstallFile testObject = new InstallFile(filePath, goodHash);

            Assert.Equal(filePath, testObject.Path);
        }
            
        [Fact]
        
        public void IsValid_ReturnsTrue_WhenChecksumIsCorrect()
        {
            InstallFile testObject = new InstallFile(filePath, goodHash);
            
            Assert.True(testObject.IsValid);
        }

        [Fact]
        public void IsValid_ReturnsFalse_WhenChecksumIsIncorrect()
        {
            InstallFile testObject = new InstallFile(filePath, badHash);
            
            Assert.False(testObject.IsValid);
        }
        
        [Fact]
        public void IsValid_ReturnsFalse_WhenAnExceptionOccursCalculatingChecksum()
        {
            InstallFile testObject = new InstallFile(string.Empty, goodHash);
            
            Assert.False(testObject.IsValid);
        }
    }
}