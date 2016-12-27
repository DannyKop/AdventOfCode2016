using System;
using System.Linq;
using System.Text;

namespace AdventOfCode2016.Days
{
    internal sealed class Day16 : IDay
    {
        private const string Input = "01110110101001000";
        public void Part1()
        {            
            Console.WriteLine($"Part1 - Checksum: {Solve(272)}");
        }

        public void Part2()
        {
            Console.WriteLine($"Part2 - Checksum: {Solve(35651584)}");
        }

        private string Solve(int length)
        {
            string processing = Input;
            while (processing.Length < length)
            {
                processing = Generate(processing);
            }
            return Checksum(string.Join("", processing.Take(length)));
        }

        private static string Generate(string value)
        {
            string a = value;
            string b = string.Join("", a.Reverse().ToArray());
            b = string.Join("", b.Select(c => c == '1' ? '0' : '1'));
            return $"{a}0{b}";
        }

        private static string Checksum(string value)
        {
            StringBuilder checksum = new StringBuilder();
            for (int i = 0; i < value.Length - 1; i += 2)
            {
                if (value[i] == value[i + 1])
                    checksum.Append("1");
                else
                    checksum.Append("0");
            }

            if(checksum.Length % 2 == 0)
                return Checksum(checksum.ToString());
            return checksum.ToString();
        }
    }
}
