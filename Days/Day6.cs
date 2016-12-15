using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2016.Days
{
    internal sealed class Day6 : IDay
    {
       

        public void Part1()
        {            
            var dictionary = ProcessLines();
            var result = dictionary.Aggregate(string.Empty, (current, dic) => current + string.Join("", dic.Value.OrderByDescending(v => v.Value).Take(1).Select(v => v.Key)));
            Console.WriteLine($"Part1: '{result}'");
        }


        public void Part2()
        {
            var dictionary = ProcessLines();
            var result = dictionary.Aggregate(string.Empty, (current, dic) => current + string.Join("", dic.Value.OrderBy(v => v.Value).Take(1).Select(v => v.Key)));
            Console.WriteLine($"Part2: '{result}'");
        }

        private static Dictionary<int, Dictionary<char, int>> ProcessLines()
        {
            var lines = File.ReadAllLines("Resources/InputDay6.txt").ToArray();

            Dictionary<int, Dictionary<char, int>> dictionary = new Dictionary<int, Dictionary<char, int>>();

            foreach (string line in lines)
            {
                for (int column = 0; column < line.Length; column++)
                {
                    if (!dictionary.ContainsKey(column))
                        dictionary[column] = new Dictionary<char, int>();

                    if (!dictionary[column].ContainsKey(line[column]))
                        dictionary[column][line[column]] = 1;
                    else
                        dictionary[column][line[column]] += 1;
                }
            }
            return dictionary;
        }
    }
}
