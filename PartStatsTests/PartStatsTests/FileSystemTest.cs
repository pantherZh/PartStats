using NUnit.Framework;
using PartStats;
using System;

namespace PartStatsTests
{
    [TestFixture]
    public class FileSystemTest
    {
        [SetUp]
        public void Setup() { }

        [Test]
        public void StartThread_PermissionsAreFalse_ThrowUnauthorizedAccessException()
        {
            var system = new FileSystem(@"Path");
            var permission = false;
            Assert.Throws<UnauthorizedAccessException>(() => system.StartProcess(permission), "UnAuthorizedAccessException: Access Denied.");
        }

        [TestCase("test", false)]
        public void ProcessFile_FileIsNotCorrect_CheckThrowIOException(string file, bool expectedResult)
        {
            var system = new FileSystem(@"Path");
            var result = system.ProcessFile(file);
            Assert.AreEqual(expectedResult, result);
        }
    }
}