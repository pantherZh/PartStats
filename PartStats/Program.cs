using System;
using System.IO;
using System.Threading.Tasks;
#pragma warning disable SA1600

namespace PartStats
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            if (args.Length < 2)
            {
                throw new ArgumentException("Wrong number of arguments!");
            }

            if (args[0].Equals("filesystem", StringComparison.CurrentCultureIgnoreCase))
            {
                ValidatePathDirectory(args[1]);
                FileSystem system = new FileSystem(args[1]);
                system.StartProcess();
                system.GetOutput();
            }
            else if (args[0].Equals("web", StringComparison.CurrentCultureIgnoreCase))
            {
                ValidatePathFile(args[1]);
                WebSystem system = new WebSystem(args[1]);
                system.StartProcess();
                await system.GetURL();
                system.GetOutput();
            }
            else
            {
                Console.WriteLine($"Invalid arguments!");
            }
        }

        public static void ValidatePathDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                throw new DirectoryNotFoundException("Directory doesn't exist");
            }
        }

        public static void ValidatePathFile(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("File doesn't exist");
            }
        }
    }
}
