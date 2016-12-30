using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2016.Days
{
    internal sealed class Day21 : IDay
    {
        private readonly string[] _input = File.ReadAllLines("Resources/InputDay21.txt");
        private const string ToScramble = "abcdefgh";
        private const string ToUnScramble = "fbgdceah";

        public void Part1()
        {
            var parsedInput = Parse(_input);                    
            var result = parsedInput.Aggregate(ToScramble, (current, rule) => rule.Execute(current));
            Console.WriteLine($"Part 1 - scrambling: '{ToScramble}' results in: '{result}'");
        }

        public void Part2()
        {            
            var parsedInput = Parse(_input.Reverse().ToArray(), part2: true);
            var result = parsedInput.Aggregate(ToUnScramble, (current, rule) => rule.Execute(current));         
            Console.WriteLine($"Part 2 - unscrambling: '{ToUnScramble}' results in: '{result}'");
        }

        private static IEnumerable<IScrambling> Parse(string[] input, bool part2 = false)
        {
            const string swapPos = @"(swap position)\s*(\d*)\s*(with position)\s*(\d*)";
            const string swapLetter = @"(swap letter)\s*(\w*)\s*(with letter)\s*(\w)";
            const string rotateDir = @"(rotate)\s*(left|right)\s*(\d*)\s*(step|steps)";
            const string reversePos = @"(reverse positions)\s*(\d*)\s*(through)\s*(\d*)";
            const string movPos = @"(move position)\s*(\d*)\s*(to position)\s*(\d*)";
            const string rotatePos = @"(rotate based on position of letter)\s*(\w)";

            foreach (var line in input)
            {
                IScrambling operation = null;
                Match match;

                if (line.StartsWith("swap position"))
                {
                    match = Regex.Match(line, swapPos);
                    operation = new SwapPosition(int.Parse(match.Groups[2].Value), int.Parse(match.Groups[4].Value));
                }

                if (line.StartsWith("swap letter"))
                {
                    match = Regex.Match(line, swapLetter);
                    operation = new SwapLetter(match.Groups[2].Value.First(), match.Groups[4].Value.First());                   
                }

                if (line.StartsWith("rotate left") || line.StartsWith("rotate right"))
                {
                    match = Regex.Match(line, rotateDir);
                    string dir;
                    if(!part2)
                        dir =  match.Groups[2].Value;
                    else
                        dir = match.Groups[2].Value.Equals("left") ? "right" : "left";
                    operation = new RotateDirection(dir, int.Parse(match.Groups[3].Value));
                }

                if (line.StartsWith("move position"))
                {
                    match = Regex.Match(line, movPos);
                    if(!part2)
                        operation = new MovePosition(int.Parse(match.Groups[2].Value), int.Parse(match.Groups[4].Value));
                    else
                        operation = new MovePosition(int.Parse(match.Groups[4].Value), int.Parse(match.Groups[2].Value));
                }

                if (line.StartsWith("rotate based on position of letter"))
                {
                    match = Regex.Match(line, rotatePos);
                    if(!part2)
                        operation = new RotatePosition(match.Groups[2].Value.First());
                    else
                        operation = new RotatePosition(match.Groups[2].Value.First(), undo: true);
                }

                if (line.StartsWith("reverse positions"))
                {
                    match = Regex.Match(line, reversePos);
                    operation = new ReversePositions(int.Parse(match.Groups[2].Value), int.Parse(match.Groups[4].Value));
                }
                yield return operation;
            }
        }
    }

    interface IScrambling
    {
        string Execute(string input);
    }

    internal sealed class SwapPosition : IScrambling
    {
        private readonly int _x;
        private readonly int _y;
        public SwapPosition(int x, int y)
        {
            _x = x;
            _y = y;
        }
        public string Execute(string input)
        {
            char atX = input[_x];
            char atY = input[_y];
            StringBuilder sb = new StringBuilder(input)
            {
                [_x] = atY,
                [_y] = atX
            };
            return sb.ToString();
        }
    }

    internal sealed class SwapLetter : IScrambling
    {
        private readonly char _x;
        private readonly char _y;
        public SwapLetter(char x, char y)
        {
            _x = x;
            _y = y;
        }
        public string Execute(string input)
        {
            int indexX = input.IndexOf(_x);
            int indexY = input.IndexOf(_y);

            StringBuilder sb = new StringBuilder(input)
            {
                [indexX] = _y,
                [indexY] = _x
            };
            return sb.ToString();
        }
    }

    internal sealed class RotateDirection : IScrambling
    {
        private readonly string _direction;
        private readonly int _steps;
        public RotateDirection(string dir, int steps)
        {
            _direction = dir;
            _steps = steps;
        }
        public string Execute(string input)
        {
            for (int i = 0; i < _steps; i++)
            {
                if ("left".Equals(_direction))
                    input = input.Substring(1, input.Length - 1) + input[0];
                else
                     input = input[input.Length - 1] + input.Substring(0, input.Length - 1);
            }
            return input;

        }
    }

    internal sealed class RotatePosition : IScrambling
    {
        private readonly char _x;
        private readonly bool _undo;

        public RotatePosition(char x, bool undo = false)
        {
            _x = x;
            _undo = undo;
        }
        public string Execute(string input)
        {
            int rotation = input.IndexOf(_x);

            if (rotation > 3)
                rotation++;

            rotation++;
            if (_undo)
            {
                int[] rotate = { 7, 7, 2, 6, 1, 5, 0, 4 };
                rotation = rotate[input.IndexOf(_x)];
            }

            for (int i = 0; i < rotation; i++)
            {
                input = input[input.Length - 1] + input.Substring(0, input.Length - 1);
            }   
            return input;
        }
    }

    internal sealed class ReversePositions : IScrambling
    {
        private readonly int _x;
        private readonly int _y;

        public ReversePositions(int x, int y)
        {
            _x = x;
            _y = y;
        }
        public string Execute(string input)
        {
            string result = string.Empty;

            // before
            for (int i = 0; i < _x; i++)
                result += input[i];

            result += string.Concat(input.Substring(_x, _y - _x + 1).Reverse());

            // after
            for (int i = _y + 1; i < input.Length; i++)
                result += input[i];

            return result;
        }
    }

    internal sealed class MovePosition : IScrambling
    {
        private readonly int _x;
        private readonly int _y;

        public MovePosition(int x, int y)
        {
            _x = x;
            _y = y;
        }
        public string Execute(string input)
        {
            char charAtX = input[_x];

            StringBuilder sb = new StringBuilder(input);
            sb.Remove(_x, 1);
            sb.Insert(_y, charAtX);
            return sb.ToString();
        }
    }

}
