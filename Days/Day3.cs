using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AdventOfCode2016
{
    internal sealed class Day3 : IDay
    {
        public void Part1()
        {
            List<string> lines = File.ReadAllLines("Resources/InputDay3.txt").ToList();
            
            int valid = 0;

            foreach (string line in lines)
            {
                var numbers = line.Split(' ').Where(elem => !string.IsNullOrWhiteSpace(elem)).Select(Int32.Parse).ToList();

                if(IsValidTriangle(numbers))
                    valid++;
            }
            Console.WriteLine($"Part1 valid: {valid}");
        }

        public void Part2()
        {
            List<string> lines = File.ReadAllLines("Resources/InputDay3.txt").ToList();
            int valid = 0;
            for (int i = 0; i < lines.Count; i += 3)
            {                
                var sub = lines.Skip(i).Take(3).ToList();
                var numbers1 = sub[0].Split(' ').Where(elem => !string.IsNullOrWhiteSpace(elem)).Select(Int32.Parse).ToList();
                var numbers2 = sub[1].Split(' ').Where(elem => !string.IsNullOrWhiteSpace(elem)).Select(Int32.Parse).ToList();
                var numbers3 = sub[2].Split(' ').Where(elem => !string.IsNullOrWhiteSpace(elem)).Select(Int32.Parse).ToList();

                for (int j = 0; j < 3; j++)
                {
                    if (IsValidTriangle(new List<int>() { numbers1[j], numbers2[j], numbers3[j] }))
                        valid++;
                }
            }
            Console.WriteLine($"Part2 valid: {valid}");

        }

        private bool IsValidTriangle(List<int> numbers)
        {
            numbers.Sort();

            return numbers[0] + numbers[1] > numbers[2];
        }
    }
}
