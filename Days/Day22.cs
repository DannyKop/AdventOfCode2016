using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2016.Days
{
    internal sealed class Day22 : IDay
    {
        private readonly string[] _input = File.ReadAllLines("Resources/InputDay22.txt");
        private List<Node> _nodesFromInPut;

        public void Part1()
        {
            _nodesFromInPut = Parse(_input).ToList();                        
            int pairs = _nodesFromInPut.Where(n1 => n1.Used != 0).Sum(n1 => _nodesFromInPut.Count(n2 => !n1.Equals(n2) && n1.Used <= n2.Avail));
            Console.WriteLine($"Part1 - pairs: {pairs}");
        }

        public void Part2()
        {
            Node[,] nodeMatrix = new Node[_nodesFromInPut.Max(n => n.Y) + 1, _nodesFromInPut.Max(n => n.X) + 1];
            foreach (var node in _nodesFromInPut)
                nodeMatrix[node.Y, node.X] = node;
            Console.WriteLine($"Part2 - Count manual:");
            for (int y = 0; y < nodeMatrix.GetLength(0); y++)
            {
                for (int x = 0; x < nodeMatrix.GetLength(1); x++)
                    Console.Write(nodeMatrix[y, x].Symbol);
                Console.WriteLine();
            }                       
        }

        private static IEnumerable<Node> Parse(string[] input)
        {
            const string regexpr = @"\/dev\/grid\/node-x(\d*)-y(\d*)\s*(\d*)T\s*(\d*)T\s*(\d*)T\s*(\d*)\%";
            Match match;
            foreach (var line in input)
            {
                match = Regex.Match(line, regexpr);
                if (match.Success)
                {
                    yield return new Node()
                    {
                        X = int.Parse(match.Groups[1].Value),
                        Y = int.Parse(match.Groups[2].Value),
                        Size = int.Parse(match.Groups[3].Value),
                        Used = int.Parse(match.Groups[4].Value),
                        Avail = int.Parse(match.Groups[5].Value),
                        Use = int.Parse(match.Groups[6].Value),
                    };
                }
            }
        }
    }

    internal sealed class Node
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Size { get; set; }
        public int Used { get; set; }
        public int Avail { get; set; }
        public int Use { get; set; }

        public string FileSystem => $"/dev/grid/node-x{X}-y{Y}";

        public string Symbol
        {
            get
            {
                if (Used == 0)
                    return "_";
                if (Used > 100)
                    return "#";
                if (X == 36 && Y == 0)
                    return "G";
                if (X == 0 && Y == 0)
                    return "S"; // (.) in assignment, but S is for better displaying alignment
                return ".";

            }
        }
    }

}