using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2016.Days
{
    internal sealed class Day8 : IDay
    {
        private readonly string[] _lines = File.ReadAllLines("Resources/InputDay8.txt");
        private bool[,] _matrix = new bool[6, 50];

        public void Part1()
        {            
            var operations = Parse();

            foreach (var operation in operations)
                _matrix = operation.Operate(_matrix);
            
            var beLit = _matrix.Cast<bool>().Count(b => b);

            Console.WriteLine($"Part1: be lit: {beLit}");
        }

        public void Part2()
        {
            Console.WriteLine($"Part2 - message/code:");
            Display(_matrix);
        }

        private static void Display(bool[,] matrix)
        {
            for (int row = 0; row < 6; row++)
            {
                for (int column = 0; column < 50; column++)                
                    Console.Write(matrix[row, column] ? "#" : " ");                
                Console.WriteLine();
            }
        }
        /// <summary>
        /// Wrote a little parser, just for fun :-)
        /// </summary>
        /// <returns></returns>
        private IEnumerable<IOperation> Parse()
        {
            const string rectPatterrn = @"(rect)\s*(\d*)(x)(\d*)";
            const string rotateColPattern = @"(rotate column)\s*(x)(=)(\d*)\s*(by)\s*(\d*)";
            const string rotateRowPattern = @"(rotate row)\s*(y)(=)(\d*)\s*(by)\s*(\d*)";

            foreach (var line in _lines)
            {
                Match match;
                IOperation operation = null;
                if (line.StartsWith("rect"))
                {
                    match = Regex.Match(line, rectPatterrn);
                    int columns = int.Parse(match.Groups[2].Value);
                    int rows = int.Parse(match.Groups[4].Value);
                    operation = new RectOperation(columns, rows);
                }
                if (line.StartsWith("rotate col"))
                {
                    match = Regex.Match(line, rotateColPattern);
                    int column = int.Parse(match.Groups[4].Value);
                    int rotations = int.Parse(match.Groups[6].Value);
                    operation = new RotateColumnOperation(column, rotations);
                }
                if(line.StartsWith("rotate row"))
                {
                    match = Regex.Match(line, rotateRowPattern);
                    int row = int.Parse(match.Groups[4].Value);
                    int rotations = int.Parse(match.Groups[6].Value);
                    operation = new RotateRowOperation(row, rotations);
                }
                yield return operation;
            }
        }
    }

    internal interface IOperation
    {
        bool[,] Operate(bool[,] matrix);
    }

    internal sealed class RectOperation : IOperation
    {
        private readonly int _rows;
        private readonly int _columns;

        public RectOperation(int columns, int rows)
        {
            _rows = rows;
            _columns = columns;
        }

        public bool[,] Operate(bool[,] matrix)
        {
            for(int r = 0; r < _rows; r++)
                for(int c = 0; c < _columns; c++)
                    matrix[r, c] = true;
            return matrix;
        }
    }

    internal sealed class RotateColumnOperation : IOperation
    {
        private readonly int _column;
        private readonly int _rotations;

        public RotateColumnOperation(int column, int rotations)
        {
            _column = column;
            _rotations = rotations;
        }

        public bool[,] Operate(bool[,] matrix)
        {
            for (int i = 0; i < _rotations; i++)
            {
                for (int j = 6 - 1; j > 0; j--) // 6 rows
                {
                    bool temp = matrix[j, _column];
                    matrix[j, _column] = matrix[j -1, _column];
                    matrix[j -1, _column] = temp;
                }
            }
            return matrix;
        }
    }

    internal sealed class RotateRowOperation : IOperation
    {

        private readonly int _row;
        private readonly int _rotations;

        public RotateRowOperation(int row, int rotations)
        {
            _row = row;
            _rotations = rotations;
        }
        public bool[,] Operate(bool[,] matrix)
        {
            for (int i = 0; i < _rotations; i++)
            {
                for (int j = 50 - 1; j > 0; j--) // 50 columns
                {
                    bool temp = matrix[_row,j];
                    matrix[_row, j] = matrix[_row, j - 1];
                    matrix[_row, j - 1] = temp;
                }
            }
            return matrix;
        }
    }
}
