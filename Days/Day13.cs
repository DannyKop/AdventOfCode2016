using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AdventOfCode2016.Days
{
    internal sealed class Day13 : IDay
    {
        private const int Input = 1350;
        private static readonly Point StartPoint = new Point(1, 1);
        private static Point _endPoint = new Point(31, 39);
        private readonly HashSet<Point> _visited = new HashSet<Point>();
        private readonly Queue<Location> _unvisited = new Queue<Location>();


        public void Part1()
        {                        
            var mimimumSteps = Search();
            Console.WriteLine($"Part1 - mimimum steps: {mimimumSteps}");
        }

        public void Part2()
        {      
            int counter = 0;
            for (int x = 0; x < 52; x++)
            {
                for (int y = 0; y < 52; y++)
                {
                    if (!IsOpenSpace(x, y))
                        continue;
                    _visited.Clear();
                    _unvisited.Clear();
                    
                    _endPoint.X = x;
                    _endPoint.Y = y;

                    if (Search() <= 50)
                        counter++;
                }
            }
            Console.WriteLine($"Part2 - number of locations: {counter}");
        }

        private int Search()
        {
            Location startLocation = new Location()
            {
                Point = StartPoint,
                Steps = 0
            };
            _unvisited.Enqueue(startLocation);

            int mimimumSteps = int.MaxValue;
            while (_unvisited.Count > 0)
            {
                Location current = _unvisited.Dequeue();

                if (_visited.Contains(current.Point))
                    continue;
                _visited.Add(current.Point);

                if (current.Point.Equals(_endPoint))
                {
                    if (current.Steps < mimimumSteps)
                    {
                        mimimumSteps = current.Steps;
                        break;
                    }
                }

                ICollection<Location> nextMoves = GetNextLocations(current);
                foreach (var loc in nextMoves)
                    _unvisited.Enqueue(loc);
            }
            return mimimumSteps;
        }

        private ICollection<Location> GetNextLocations(Location current)
        {
            var result = new List<Location>();

            // 4 directions -> N E S W
            // N
            if (IsOpenSpace(current.Point.X, current.Point.Y - 1))
            {
                result.Add(new Location()
                {
                    Point = new Point(current.Point.X, current.Point.Y - 1),
                    Steps = current.Steps + 1
                });
            }

            // E
            if (IsOpenSpace(current.Point.X + 1, current.Point.Y ))
            {
                result.Add(new Location()
                {
                    Point = new Point(current.Point.X + 1, current.Point.Y ),
                    Steps = current.Steps + 1
                });
            }
            // S
            if (IsOpenSpace(current.Point.X, current.Point.Y + 1))
            {
                result.Add(new Location()
                {
                    Point = new Point(current.Point.X , current.Point.Y + 1),
                    Steps = current.Steps + 1
                });
            }
            // W
            if (IsOpenSpace(current.Point.X -1 , current.Point.Y))
            {
                result.Add(new Location()
                {
                    Point = new Point(current.Point.X - 1, current.Point.Y),
                    Steps = current.Steps + 1
                });
            }
            return result;
        }

        private static bool IsOpenSpace(int x, int y)
        {
            if (x < 0 || y < 0)
                return false;
            return Convert.ToString((x * x + 3 * x + 2 * x * y + y + y * y) + Input, 2).Count(c => c.Equals('1')) % 2 == 0;
        }
    }

    internal sealed class Location
    {
        public Point Point { get; set; }

        public int Steps { get; set; }
    }
}
