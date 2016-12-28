using System;
using System.Linq;
using System.Text;

namespace AdventOfCode2016.Days
{
    internal sealed class Day18 : IDay
    {
        private const string Input = ".^^.^^^..^.^..^.^^.^^^^.^^.^^...^..^...^^^..^^...^..^^^^^^..^.^^^..^.^^^^.^^^.^...^^^.^^.^^^.^.^^.^.";

        public void Part1()
        {
            Console.WriteLine($"Part1 - Safe tiles: {CalculateSafeTiles(Input, 40)}");
        }

        public void Part2()
        {            
            Console.WriteLine($"Part2 - Safe tiles: {CalculateSafeTiles(Input, 400000)}");
        }

        private static int CalculateSafeTiles(string input, int rows)
        {
            int safeTiles = 0;
            string currentRowValue = input;
            for (int i = 0; i < rows; i++)
            {
                safeTiles += currentRowValue.Count(ch => ch == '.');
                currentRowValue = $".{currentRowValue}.";

                StringBuilder tmp = new StringBuilder();
                for (int j = 1; j < currentRowValue.Length - 1; j++)               
                    tmp.Append(IsSafe(currentRowValue.Substring(j - 1, 3)) ? '.' : '^');

                currentRowValue = tmp.ToString();
            }
            return safeTiles;
        }

        private static bool IsSafe(string input)
        {
            char left = input[0];
            char middle = input[1];
            char right = input[2];

            if (left == '^' && middle == '^' && right == '.')
                return false;
            if (left == '.' && middle == '^' && right == '^')
                return false;
            if (left == '^' && middle == '.' && right == '.')
                return false;
            if (left == '.' && middle == '.' && right == '^')
                return false;
            return true;
        }
    }
}