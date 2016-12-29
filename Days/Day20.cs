using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2016.Days
{
    internal sealed class Day20 : IDay
    {
        private readonly string[] _input = File.ReadAllLines("Resources/InputDay20.txt");
        private readonly IList<IpRange> _ranges;

        public Day20()
        {
            var blacklist = ParseToBlacklist(_input);
            _ranges = RemoveOverlapping(blacklist);        
        }
        
        public void Part1()
        {         
            long result = -1;
            for (int i = 0; i < _ranges.Count - 1; i++)
            {
                if (_ranges[i].End + 1 != _ranges[i + 1].Start)
                {
                    result = _ranges[i].End + 1;
                    break;
                }
            }
            Console.WriteLine($"Part 1 - First ip address: {result}");
        }

        public void Part2()
        {
            long result = 0;
            for (int i = 0; i < _ranges.Count - 1; i++)
            {
                if (_ranges[i].End + 1 != _ranges[i + 1].Start)
                {
                    result += _ranges[i + 1].Start - _ranges[i].End - 1;                    
                }
            }           
            Console.WriteLine($"Part 2 - Total allowed ip adresses: {result}");   
        }

        private static IList<IpRange> ParseToBlacklist(string[] input)
        {
            List<IpRange> blacklist = new List<IpRange>();
            foreach (var line in input)
            {
                var splitted = line.Split('-');
                blacklist.Add(new IpRange()
                {
                    Start = long.Parse(splitted.First()),
                    End = long.Parse(splitted.Last())
                });
            }
            return blacklist.OrderBy(range => range.Start).ToList();
        }

        private static IList<IpRange> RemoveOverlapping(IList<IpRange> input)
        {
            List<IpRange> result = new List<IpRange> {input.First()};
            foreach (var range in input.Skip(1))
            {
                var last = result.Last();
                if (range.Start >= last.Start && range.End <= last.End)
                    continue;
                if (range.Start >= last.Start && range.Start < last.End && range.End > last.End)
                {
                    last.End = range.End;
                    continue;                    
                }
                result.Add(range);
            }
            return result;
        }
    }

    internal sealed class IpRange
    {
        public long Start { get; set; }
        public long End { get; set; }
    }    
}
