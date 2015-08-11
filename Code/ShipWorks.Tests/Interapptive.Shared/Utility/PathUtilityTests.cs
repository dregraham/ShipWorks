using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Utility;
using Xunit;

namespace ShipWorks.Tests.Interapptive.Shared.Utility
{
    public class PathUtilityTests
    {
        [Fact]
        public void CleanPath_ReturnsEmpty_WhenPathIsEmpty()
        {
            string result = PathUtility.CleanPath(string.Empty);
            Assert.AreEqual(string.Empty, result);
        }

        [Fact]
        public void CleanPath_ReturnsEmpty_WhenPathIsNull()
        {
            string result = PathUtility.CleanPath(null);
            Assert.AreEqual(string.Empty, result);
        }

        [Fact]
        public void CleanPath_DoesNotAlterPath_WhenPathDoesNotContainIllegalCharacters()
        {
            string result = PathUtility.CleanPath(@"c:\foo\bar");
            Assert.AreEqual(@"c:\foo\bar", result);
        }

        [Fact]
        public void CleanPath_DoesNotAlterPath_WhenPathDoesNotContainIllegalCharactersAndEndsWithSlash()
        {
            string result = PathUtility.CleanPath(@"c:\foo\bar\");
            Assert.AreEqual(@"c:\foo\bar\", result);
        }

        [Fact]
        public void CleanPath_DoesNotAlterPath_WhenPathIsNotRootedAndDoesNotContainIllegalCharacters()
        {
            string result = PathUtility.CleanPath(@"foo\bar");
            Assert.AreEqual(@"foo\bar", result);
        }

        [Fact]
        public void CleanPath_DoesNotAlterPath_WhenPathIsNotRootedDoesNotContainIllegalCharactersAndEndsWithSlash()
        {
            string result = PathUtility.CleanPath(@"foo\bar\");
            Assert.AreEqual(@"foo\bar\", result);
        }

        [Fact]
        public void CleanPath_ReplacesIllegalCharacters_WhenPathContainsColon()
        {
            string result = PathUtility.CleanPath(@"c:\fo:o\bar");
            Assert.AreEqual(@"c:\fo_o\bar", result);
        }

        [Fact]
        public void CleanPath_ReplacesIllegalCharacters_WhenPathContainsStar()
        {
            string result = PathUtility.CleanPath(@"c:\fo*o\bar");
            Assert.AreEqual(@"c:\fo_o\bar", result);
        }

        [Fact]
        public void CleanPath_ReplacesIllegalCharacters_WhenPathContainsQuestion()
        {
            string result = PathUtility.CleanPath(@"c:\fo?o\bar");
            Assert.AreEqual(@"c:\fo_o\bar", result);
        }

        [Fact]
        public void CleanPath_ReplacesIllegalCharacters_WhenPathContainsColonAndIsNotRooted()
        {
            string result = PathUtility.CleanPath(@"\fo:o\bar");
            Assert.AreEqual(@"\fo_o\bar", result);
        }

        [Fact]
        public void CleanPath_ReplacesIllegalCharacters_WhenPathContainsStarAndIsNotRooted()
        {
            string result = PathUtility.CleanPath(@"\fo*o\bar");
            Assert.AreEqual(@"\fo_o\bar", result);
        }

        [Fact]
        public void CleanPath_ReplacesIllegalCharacters_WhenPathContainsQuestionAndIsNotRooted()
        {
            string result = PathUtility.CleanPath(@"\fo?o\bar");
            Assert.AreEqual(@"\fo_o\bar", result);
        }

        [Fact]
        public void CleanFileName_ReturnsEmpty_WhenPathIsEmpty()
        {
            string result = PathUtility.CleanFileName(string.Empty);
            Assert.AreEqual(string.Empty, result);
        }

        [Fact]
        public void CleanFileName_ReturnsEmpty_WhenPathIsNull()
        {
            string result = PathUtility.CleanFileName(null);
            Assert.AreEqual(string.Empty, result);
        }

        [Fact]
        public void CleanFileName_DoesNotAlterName_WhenNameDoesNotContainIllegalCharacters()
        {
            string result = PathUtility.CleanFileName("foo.txt");
            Assert.AreEqual("foo.txt", result);
        }

        [Fact]
        public void CleanFileName_ReplacesIllegalCharacters_WhenPathContainsColon()
        {
            string result = PathUtility.CleanFileName("fo:o.txt");
            Assert.AreEqual("fo_o.txt", result);
        }

        [Fact]
        public void CleanFileName_ReplacesIllegalCharacters_WhenPathContainsStar()
        {
            string result = PathUtility.CleanFileName("fo*o.txt");
            Assert.AreEqual("fo_o.txt", result);
        }

        [Fact]
        public void CleanFileName_ReplacesIllegalCharacters_WhenPathContainsQuestion()
        {
            string result = PathUtility.CleanFileName("fo?o.txt");
            Assert.AreEqual("fo_o.txt", result);
        }
    }
}
