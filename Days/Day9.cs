using System;
using System.IO;

namespace AdventOfCode2016.Days
{
    internal sealed class Day9 : IDay
    {

        private readonly string _input = File.ReadAllText("Resources/InputDay9.txt");

        public void Part1()
        {
            Console.WriteLine($"Part1 - length: {Decompress(_input)}");
        }

        public void Part2()
        {            
            Console.WriteLine($"Part2 - length: {Decompress(_input, true)}");
        }
       
        private static long Decompress(string input, bool part2 = false)
        {
            long length = 0;
            var toDecompressInput = input.Replace(" ", "");
            for (int i = 0; i < toDecompressInput.Length; i++)
            {
                if (toDecompressInput[i] == '(')
                {
                    int closingIndex = toDecompressInput.IndexOf(")", i, StringComparison.Ordinal);
                    string marker = toDecompressInput.Substring(i + 1, closingIndex - i - 1); // skipping ( + )
                    var markerElements = marker.Split('x');
                    int chars = int.Parse(markerElements[0]);
                    int repeats = int.Parse(markerElements[1]);

                    var markedInput = toDecompressInput.Substring(closingIndex + 1, chars);

                    if (part2)
                        length += Decompress(markedInput, true) * repeats;
                    else
                        length += markedInput.Length * repeats;
                    i = closingIndex + chars; // + 1 -> already added by incrementor;
                }
                else
                {
                    length++;
                }
            }
            return length;
        }
    }
}
