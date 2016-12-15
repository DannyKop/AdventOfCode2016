using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace AdventOfCode2016.Days
{
    internal sealed class Day7 : IDay
    {
        private readonly string[] _lines = File.ReadAllLines("Resources/InputDay7.txt");

        public void Part1()
        {            
            int valid = 0;
            foreach (var line in _lines)
            {
                bool inBrackets = false;
                bool isAbba = false;
                bool abbaBetweenBrackets = false;
                for (int i = 0; i < line.Length - 3; i++)
                {
                    if (line[i] == '[' || line[i] == ']')
                    {
                        inBrackets = !inBrackets;
                        continue;
                    }

                    if (IsAbba(line.Substring(i, 4)))
                    {
                        if (inBrackets)
                            abbaBetweenBrackets = true;
                        isAbba = true;
                    }
                }

                if (!abbaBetweenBrackets && isAbba)
                    valid++;
            }
            Console.WriteLine($"Part1: valid TLS Ipaddresses: {valid} ");
        }

        public void Part2()
        {            
            List<string> validLines = new List<string>();

            foreach (var line in _lines)
            {
                bool inBrackets = false;
                for (int i = 0; i < line.Length - 2; i++)
                {
                    if (line[i] == '[' || line[i] == ']')
                    {
                        inBrackets = !inBrackets;
                        continue;
                    }

                    if (inBrackets)
                        continue;

                    var sequence = line.Substring(i, 3);
                    if (IsAba(sequence))
                    {
                        var matches = Regex.Matches(line, @"(\[\w*\])");
                        foreach (Match match in matches)
                        {
                            if (match.Value.Contains($"{sequence[1]}{sequence[0]}{sequence[1]}"))
                            {
                                if(!validLines.Contains(line))
                                    validLines.Add(line);
                            }
                        }
                    }
                }                
            }
                        
            Console.WriteLine($"Part2: valid SSL ipaddresses: {validLines.Count} ");
        }
        /// <summary>
        /// Checks if input is abba
        /// </summary>
        /// <param name="input">string of length 4</param>
        /// <returns></returns>
        public static bool IsAbba(string input)
        {
            return input[0] == input[3] && input[1] == input[2] && input[0] != input[1];
        }

        public static bool IsAba(string input)
        {
            return input[0] == input[2] && input[0] != input[1];
        }
    }
}
