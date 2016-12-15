using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2016.Days
{
    internal sealed class Day4 : IDay
    {
        public void Part1()
        {
            var lines = File.ReadAllLines("Resources/InputDay4.txt");

            var sum = lines.Sum(ProcessLine);

            Console.WriteLine($"Part1: sum of sector ids: {sum}");
        }

        public void Part2()
        {
            var lines = File.ReadAllLines("Resources/InputDay4.txt");

            foreach (string line in lines)
            {
                int sectorId = ProcessLine(line);
                if (sectorId == 0)
                    continue;

                var decryptedName = Decrypt(line);                
                if (decryptedName.Contains("northpole object storage"))
                {
                    Console.WriteLine($"Sector ID of Northpole: {sectorId}");
                    break;
                }
            }
        }

        private static int ProcessLine(string input)
        {
            IList<string> elements = input.Split('-').ToList();
            var lastPart = elements.Last();
            elements.Remove(lastPart);

            IDictionary<char, int> dictionary = new Dictionary<char, int>();
            foreach (char c in string.Join("", elements))
            {
                if (!dictionary.ContainsKey(c))
                    dictionary[c] = 1;
                else
                    dictionary[c] += 1;
            }

            var tmp = string.Join("", dictionary.OrderByDescending(e => e.Value).ThenBy(e => e.Key).Select(e => e.Key).Take(5));
            int sectorId = int.Parse(lastPart.Substring(0, 3));
            string checksum = lastPart.Substring(4, 5);

            if (Equals(tmp, checksum))
                return sectorId;
            return 0;
        }

        private static string Decrypt(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;
            var result = string.Empty;

            IList<string> elements = input.Split('-').ToList();
            var lastPart = elements.Last();
            int sectorId = int.Parse(lastPart.Substring(0, 3));

            for (int i = 0; i < input.Length - 11 ; i++) // last 11 chars of string are not required
            {
                if (input[i] == '-')
                {
                    result += ' ';
                    continue;
                }

                char newChar = (char) ((input[i] + sectorId - 'a')%26 + 'a');
                result += newChar.ToString();
            }
            return result;
        }
    }
}
