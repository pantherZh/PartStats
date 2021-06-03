using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
#pragma warning disable SA1600

namespace PartStats
{
    public class FileSystem
    {
        private readonly Dictionary<string, int> detailsDictionary = new Dictionary<string, int>();

        public FileSystem(string path)
        {
            this.Path = path;
        }

        public string Path { get; set; }

        public void StartProcess(bool permissions)
        {
            if (!permissions)
            {
                throw new UnauthorizedAccessException("UnAuthorizedAccessException: Access Denied.");
            }

            Stopwatch stopwatch = new Stopwatch();

            foreach (var file in this.GetAllDirectoryFiles())
            {
                this.ProcessFile(file);
            }

            stopwatch.Stop();
            Console.WriteLine("Processing time - " + stopwatch.ElapsedMilliseconds + "c");
        }

        public bool CheckPermissions()
        {
            var dirInfo = new DirectoryInfo(this.Path);
            DirectorySecurity dSecurity = dirInfo.GetAccessControl();
            bool readAllow = false;
            bool readDeny = false;
            if (dSecurity == null)
            {
                return false;
            }

            AuthorizationRuleCollection accessRules = dSecurity.GetAccessRules(true, true,
                                        typeof(SecurityIdentifier));
            if (accessRules == null)
            {
                return false;
            }

            foreach (FileSystemAccessRule ace in accessRules)
            {
                Console.WriteLine("Account: {0}", ace.IdentityReference.Value);
                Console.WriteLine("Type: {0}", ace.AccessControlType);
                Console.WriteLine("Rights: {0}", ace.FileSystemRights);
                Console.WriteLine("Inherited: {0}", ace.IsInherited);

                Console.WriteLine("------------------------");
            }

            foreach (FileSystemAccessRule rule in accessRules)
            {
                if ((FileSystemRights.Read & rule.FileSystemRights) != FileSystemRights.Read)
                {
                    continue;
                }

                if (rule.AccessControlType == AccessControlType.Allow)
                {
                    readAllow = true;
                }
                else if (rule.AccessControlType == AccessControlType.Deny)
                {
                    readDeny = true;
                }
            }

            return readAllow && !readDeny;
        }

        public void GetOutput()
        {
            using (StreamWriter sw = new StreamWriter("output.txt"))
            {
                foreach (KeyValuePair<string, int> keyValue in this.detailsDictionary)
                {
                    sw.WriteLine(keyValue.Key + "," + keyValue.Value);
                }
            }
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
                    string value = null;
                    string key = null;
                    string[] dataDetails = null;
                    int stringNumber = 0;

                    while ((line = sr.ReadLine()) != null)
                    {
                        stringNumber++;
                        dataDetails = line.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
                        value = dataDetails.Length > 1 ? dataDetails[1] : null;

                        if (!int.TryParse(value, out int number))
                        {
                            Console.WriteLine($"line:{stringNumber} in {file}: invalid data");
                            continue;
                        }

                        key = dataDetails[0].ToLowerInvariant();
                        if (!this.detailsDictionary.ContainsKey(key))
                        {
                            this.detailsDictionary.Add(key, number);
                        }
                        else
                        {
                            int preventNumber = this.detailsDictionary[key];
                            this.detailsDictionary.Remove(key);
                            this.detailsDictionary.Add(key, number + preventNumber);
                        }
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
