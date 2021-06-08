using System;
using System.Collections.Generic;
using System.IO;
#pragma warning disable SA1600

namespace PartStats
{
    public static class Data
    {
        private static readonly Dictionary<string, int> DetailsDictionary = new Dictionary<string, int>();

        public static void SetValue(string line, string file)
        {
            string value = null;
            string key = null;
            string[] dataDetails = null;
            int stringNumber = 0;

            stringNumber++;
            dataDetails = line.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
            value = dataDetails.Length > 1 ? dataDetails[1] : null;

            if (!int.TryParse(value, out int number))
            {
                Console.WriteLine($"line:{stringNumber} in {file}: invalid data");
                return;
            }

            key = dataDetails[0].ToLowerInvariant();
            if (!DetailsDictionary.ContainsKey(key))
            {
                DetailsDictionary.Add(key, number);
            }
            else
            {
                int preventNumber = DetailsDictionary[key];
                DetailsDictionary[$"{key}"] = number + preventNumber;
            }
        }

        public static void GetOutput()
        {
            using (StreamWriter sw = new StreamWriter("output.txt"))
            {
                foreach (KeyValuePair<string, int> keyValue in DetailsDictionary)
                {
                    sw.WriteLine(keyValue.Key + "," + keyValue.Value);
                }
            }
        }
    }
}
