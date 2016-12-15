using System;
using AdventOfCode2016.Days;

namespace AdventOfCode2016
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"AoC - Day 10");
            IDay day = new Day10();
            day.Part1();
            day.Part2();

            Console.WriteLine("\n\nPress enter to quit");
            Console.ReadLine();
        }
    }
}
