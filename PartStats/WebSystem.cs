using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
#pragma warning disable SA1600

namespace PartStats
{
    public class WebSystem : FileSystem
    {
        private readonly string path;

        public WebSystem(string path)
            : base(path.Remove(path.LastIndexOf(@"\")) + @"\FileKeeper")
        {
            this.path = path;
            Directory.CreateDirectory(this.Path);
        }

        public bool Catch { get; set; }

        public async Task GetURL()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
            };
            HttpClient client = new HttpClient(handler);

            using (StreamReader sr = new StreamReader(this.path, Encoding.UTF8))
            {
                string line = null;
                int count = 0;

                while ((line = sr.ReadLine()) != null)
                {
                    await this.GetFileViaHttp(client, line, count++);
                }
            }
        }

        public string GenerateName(int number)
        {
            return @"\input" + number + ".txt";
        }

        public async Task GetFileViaHttp(HttpClient client, string address, int count)
        {
            this.Catch = true;
            try
            {
                HttpResponseMessage response = await client.GetAsync(address, HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    count++;
                    File.WriteAllText($"{this.Path}{this.GenerateName(count)}", await response.Content.ReadAsStringAsync(), Encoding.UTF8);
                }
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("Invalid arguments for connecting via http.");
                Console.WriteLine(e.Message);
                this.Catch = false;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("Cannot connect to this domain, line:" + count);
                Console.WriteLine("Message :{0} ", e.Message);
                this.Catch = false;
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine("InvalidOperation: line " + count + ", Path: " + this.path);
                Console.WriteLine("Message :{0} ", e.Message);
                this.Catch = false;
            }
        }
    }
}