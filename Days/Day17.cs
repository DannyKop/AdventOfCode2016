using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2016.Days
{
    internal sealed class Day17 : IDay
    {        
        private static readonly Point EndPoint = new Point(3, 3);
        private const string OpenChars = "BCDEF";        
        private const string Input = "pgflpeqp";
        private readonly IList<Vault> _solutions = new List<Vault>();
        private readonly Queue<Vault> _unvisited = new Queue<Vault>();

        public void Part1()
        {
            Search();
            var vault = _solutions.OrderBy(v => v.PathValue.Length).First();

            Console.WriteLine($"Part1 - Shortest path: {vault.PathValue.Replace(Input, "")}");
        }

        public void Part2()
        {
            var vault = _solutions.OrderByDescending(v => v.PathValue.Length).First();
            Console.WriteLine($"Part2 - Length of longest path: {vault.PathValue.Replace(Input, "").Length}");
        }

        private void Search()
        {
            Vault startPoint = new Vault(Input);
            _unvisited.Enqueue(startPoint);

            while (_unvisited.Any())
            {
                var currentVault = _unvisited.Dequeue();

                if (currentVault.Point.Equals(EndPoint))
                {
                    _solutions.Add(currentVault);
                    continue;
                }

                ICollection<Vault> nextMoves = GenerateNextStates(currentVault);
                foreach(var vault in nextMoves)
                    _unvisited.Enqueue(vault);
            }
        }

        private ICollection<Vault> GenerateNextStates(Vault currentVault)
        {
            var result = new List<Vault>();
            string directions = currentVault.NextDirections();

            // Up
            if (OpenChars.Contains(directions[0]) && currentVault.IsValid(0, -1))
            {
                result.Add(new Vault($"{currentVault.PathValue}U")
                {
                    Point = new Point(currentVault.Point.X, currentVault.Point.Y - 1)
                });                
            }
            // Down
            if (OpenChars.Contains(directions[1]) && currentVault.IsValid(0, 1))
            {
                result.Add(new Vault($"{currentVault.PathValue}D")
                {
                    Point = new Point(currentVault.Point.X, currentVault.Point.Y + 1)
                });
            }

            // Left
            if (OpenChars.Contains(directions[2]) && currentVault.IsValid(-1, 0))
            {
                result.Add(new Vault($"{currentVault.PathValue}L")
                {
                    Point = new Point(currentVault.Point.X - 1, currentVault.Point.Y)
                });
            }
            // Right
            if (OpenChars.Contains(directions[3]) && currentVault.IsValid(1, 0))
            {
                result.Add(new Vault($"{currentVault.PathValue}R")
                {
                    Point = new Point(currentVault.Point.X + 1, currentVault.Point.Y )
                });
            }
            return result;
        }
    }

    internal sealed class Vault
    {
        public Vault(string pathValue)
        {
            PathValue = pathValue;
            Point = new Point(0, 0);
        }
        public string PathValue { get; set; }
        public Point Point { get; set; }

        public string NextDirections()
        {
            MD5 md5 = new MD5Cng();
            byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(PathValue));            
            return string.Concat(BitConverter.ToString(hash).Replace("-", "").Take(4));            
        }

        public bool IsValid(int offsetX, int offsetY)
        {
            int pointX = Point.X + offsetX;
            int pointY = Point.Y + offsetY;
            if (pointX < 0 || pointX > 3)
                return false;
            if (pointY < 0 || pointY > 3)
                return false;
            return true;
        }
    }
}
