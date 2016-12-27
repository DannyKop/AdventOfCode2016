using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2016.Days
{
    internal sealed class Day15 : IDay
    {
        private readonly IList<string> _input = File.ReadAllLines($"Resources/InputDay15.txt").ToList();

        public void Part1()
        {
            var discs = Parse().ToList();
            Console.WriteLine($"Part1: t: {Solve(discs)}");
        }

        public void Part2()
        {
            _input.Add("Disc #7 has 11 positions; at time=0, it is at position 0.");
            var discs = Parse().ToList();
            Console.WriteLine($"Part2: t: {Solve(discs)}");
        }

        private static int Solve(IList<Disc> discs)
        {
            return Enumerable.Range(1, int.MaxValue).First(t => discs.All(d => d.IsOpen(t)));
        }

        private IEnumerable<Disc> Parse()
        {
            const string regexpr = @"(Disc \#)(\d*)\s*(has)\s*(\d*)\s*(positions; at time=0, it is at position)\s*(\d*)(\.)";
            foreach (string line in _input)
            {
                Match match = Regex.Match(line, regexpr);
                yield return new Disc(int.Parse(match.Groups[2].Value), 
                                      int.Parse(match.Groups[4].Value), 
                                      int.Parse(match.Groups[6].Value));
            }                    
        }

    }

    internal sealed class Disc
    {
        private readonly int _number;
        private readonly int _positions;
        private readonly int _startPosition;

        public Disc(int number, int position, int startPosition)
        {
            _number = number;
            _positions = position;
            _startPosition = startPosition;
        }

        public bool IsOpen(int t)
        {
            return (_startPosition + _number + t) % _positions == 0;
        }
    }
}
