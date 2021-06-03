using NUnit.Framework;
using System;
using System.IO;
using PartStats;

namespace PartStatsTests
{
    [TestFixture]
    public class ProgramTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Main_CheckParametrs_MoreThan_2()
        {
            string[] args = { "web" };
            Assert.ThrowsAsync<ArgumentException>(() => Program.Main(args));
        }
         
        [TestCase(@"D:\MyDirectory\add.txt")]
        [TestCase(@"D:\MyDirectory\")]
        public void ValidatePathFile_Test(string path) => Assert.Throws<FileNotFoundException>(() => Program.ValidatePathFile(path));

        [TestCase(@"D:\MyDirectory\add.txt")]
        [TestCase(@"D:\MyDirectory\add\")]
        public void ValidatePathDirectory_Test(string path) => Assert.Throws<DirectoryNotFoundException>(() => Program.ValidatePathDirectory(path));
    }
}