using NUnit.Framework;
using PartStats;
using System.Net.Http;
using System;
using System.Net;
using System.IO;
using System.Threading.Tasks;

namespace PartStatsTests
{
    [TestFixture]
    public class WebSystemTest
    {
        [SetUp]
        public void Setup()
        {
            
        }


        [TestCase(@"D:MyDirectory\", 1, ExpectedResult = @"\input1.txt")]
        [TestCase(@"D:MyDirectory\", 2, ExpectedResult = @"\input2.txt")]

        public string GenerateName_Detecs_Names(string path, int number)
        {
            var system = new WebSystem(path);
            return system.GenerateName(number);
        }

        [TestCase(@"htp://178.154.215.204/txt/1", 4, false)]
        [TestCase(@"http://178.154.215.204/txt", 4, false)]
        [TestCase(@"test", 1, false)]
        public async Task GetFileViaHttp_CatchExc_ArgumentExeption_HttpRequestException_InvalidOperationException(string address, int count, bool expectedResult)
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };
            var writer = new StringWriter();
            Console.SetOut(writer);

            HttpClient httpClient = new HttpClient(handler);
            string path = @"D:\MyDirectory\address.txt";
            var system = new WebSystem(path);

            await system.GetFileViaHttp(httpClient, address, count);
            bool result = system.Catch;
            Assert.AreEqual(expectedResult, result);
        }
        [Test]
        public void GetFileViaHttp_CheckFile()
        {
            string path = @"D:\MyDirectory\address.txt";
            var system = new WebSystem(path);
            Assert.IsTrue(File.Exists(system.Path + @"\input1.txt"));
        }
    }
}