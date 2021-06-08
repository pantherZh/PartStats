using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
#pragma warning disable SA1600

namespace PartStats
{
    public class FileSystem
    {

        public FileSystem(string path)
        {
            this.Path = path;
        }

        public string Path { get; set; }

        public void StartProcess()
        {
            Stopwatch stopwatch = new Stopwatch();

            foreach (var file in this.GetAllDirectoryFiles())
            {
                this.ProcessFile(file);
            }

            stopwatch.Stop();
            Console.WriteLine("Processing time - " + stopwatch.ElapsedMilliseconds + "c");
        }

        public List<string> GetAllDirectoryFiles()
        {
            List<string> listFiles = new List<string>();

            DirectoryInfo d = new DirectoryInfo(this.Path);
            var files = d.EnumerateFiles("*.txt", SearchOption.AllDirectories);

            foreach (FileInfo file in files)
            {
                Console.WriteLine(file.FullName);
                listFiles.Add(file.FullName);
            }

            return listFiles;
        }

        public bool ProcessFile(string file)
        {
            try
            {
                using (StreamReader sr = new StreamReader(file, Encoding.UTF8))
                {
                    string line = null;
                    while ((line = sr.ReadLine()) != null)
                    {
                        Data.SetValue(line, file);
                    }
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
                return false;
            }

            return true;
        }
    }
}
