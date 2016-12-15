using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2016
{
    internal sealed class Day1 : IDay
    {
        private const string Input = "L5, R1, R3, L4, R3, R1, L3, L2, R3, L5, L1, L2, R5, L1, R5, R1, L4, R1, R3, L4, L1, R2, R5, R3, R1, R1, L1, R1, L1, L2, L1, R2, L5, L188, L4, R1, R4, L3, R47, R1, L1, R77, R5, L2, R1, L2, R4, L5, L1, R3, R187, L4, L3, L3, R2, L3, L5, L4, L4, R1, R5, L4, L3, L3, L3, L2, L5, R1, L2, R5, L3, L4, R4, L5, R3, R4, L2, L1, L4, R1, L3, R1, R3, L2, R1, R4, R5, L3, R5, R3, L3, R4, L2, L5, L1, L1, R3, R1, L4, R3, R3, L2, R5, R4, R1, R3, L4, R3, R3, L2, L4, L5, R1, L4, L5, R4, L2, L1, L3, L3, L5, R3, L4, L3, R5, R4, R2, L4, R2, R3, L3, R4, L1, L3, R2, R1, R5, L4, L5, L5, R4, L5, L2, L4, R4, R4, R1, L3, L2, L4, R3";

        enum Direction { N, E, S, W }

        public void Part1()
        {
            var currentPosition = ProcessInput();
            Console.WriteLine($"Answer part1: {Math.Abs(currentPosition.X) + Math.Abs(currentPosition.Y)}");
        }

        public void Part2()
        {
            var currentPosition = ProcessInput(part2: true);
            Console.WriteLine($"Answer part2: {Math.Abs(currentPosition.X) + Math.Abs(currentPosition.Y)}");
        }


        private static Point ProcessInput(bool part2 = false)
        {
            var instructions = Input.Replace(" ", "").Split(',');

            var currentPosition = Point.Empty;
            var currentDirection = Direction.N;
            var positions = new List<Point>() {currentPosition};

            foreach (string instruction in instructions)
            {
                char leftOrRight = instruction[0];
                int steps = int.Parse(instruction.Substring(1));


                currentDirection = GetNewDirection(currentDirection, leftOrRight);
                for (int step = 0; step < steps; step++)
                {
                    switch (currentDirection)
                    {
                        case Direction.N:
                            currentPosition += new Size(0, 1);
                            break;
                        case Direction.E:
                            currentPosition += new Size(1, 0);
                            break;
                        case Direction.S:
                            currentPosition += new Size(0, -1);
                            break;
                        case Direction.W:
                            currentPosition += new Size(-1, 0);
                            break;
                    }
                    if (part2)
                    {
                        if (positions.Contains(currentPosition))
                            return currentPosition;
                        positions.Add(currentPosition);
                    }
                }
            }
            return currentPosition;
        }

        private static Direction GetNewDirection(Direction current, char leftOrRight)
        {
            switch (current)
            {
                case Direction.N:
                    return leftOrRight == 'L' ? Direction.W : Direction.E;
                case Direction.E:
                    return leftOrRight == 'L' ? Direction.N : Direction.S;
                case Direction.S:
                    return leftOrRight == 'L' ? Direction.E : Direction.W;
                case Direction.W:
                    return leftOrRight == 'L' ? Direction.S : Direction.N;
                default:
                    throw new ArgumentException("not possible");
            }
                
        }
    }
}
