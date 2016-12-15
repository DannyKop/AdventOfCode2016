using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2016
{
    internal sealed class Day2 : IDay
    {
        private string testInput = "ULL\r\nRRDDD\r\nLURDL\r\nUUUUD";

        private string input = "URULLLLLRLDDUURRRULLLDURRDRDRDLURURURLDLLLLRUDDRRLUDDDDDDLRLRDDDUUDUDLDULUDLDURDULLRDDURLLLRRRLLRURLLUDRDLLRRLDDRUDULRRDDLUUUDRLDLURRRULURRDLLLDDDLUDURDDRLDDDLLRULDRUDDDLUDLURUDLLRURRUURUDLLLUUUUDDURDRDDDLDRRUDURDLLLULUDURURDUUULRULUDRUUUUDLRLUUUUUDDRRDDDURULLLRRLDURLDLDRDLLLUULLRRLLLLDRLRDRRDRRUDDLULUUDDDDRRUUDDLURLRDUUDRRLDUDLRRRLRRUUDURDRULULRDURDRRRDLDUUULRDDLRLRDLUUDDUDDRLRRULLLULULLDDDRRDUUUDDRURDDURDRLRDLDRDRULRLUURUDRLULRLURLRRULDRLRDUDLDURLLRLUDLUDDURDUURLUDRLUL\r\nLLLUUURUULDDDULRRDLRLLLLLLLLRURRDLURLUDRRDDULDRRRRRRLDURRULDDULLDDDRUUDLUDULLDLRRLUULULRULURDURLLDULURDUDLRRLRLLDULLRLDURRUULDLDULLRDULULLLULDRLDLDLDLDDLULRLDUDRULUDDRDDRLRLURURRDULLUULLDRRDRRDLDLLRDLDDUUURLUULDDRRRUULDULDDRDDLULUDRURUULLUDRURDRULDRUULLRRDURUDDLDUULLDDRLRRDUDRLRRRLDRLRULDRDRRUDRLLLDDUDLULLURRURRLUURDRLLDLLDUDLUUURRLRDDUDRLUDLLRULLDUUURDLUUUDUDULRLDLDRUUDULRDRRUDLULRLRDLDRRDDDUDLDLDLRUURLDLLUURDLDLRDLDRUDDUURLLLRDRDRRULLRLRDULUDDDLUDURLDUDLLRULRDURDRDLLULRRDLLLDUURRDUDDLDDRULRRRRLRDDRURLLRRLLL\r\nDRURLDDDDRLUDRDURUDDULLRRLLRLDDRLULURLDURRLDRRLRLUURDDRRDLRDLDLULDURUDRLRUDULRURURLRUDRLLDDUDDRDLDRLLDDLRRDRUUULDUUDRUULRLLDLLULLLRRDRURDLDDRRDDUDDULLDUUULDRUDLDLURLDRURUDLRDDDURRLRDDUDLLLRRUDRULRULRRLLUUULDRLRRRLLLDLLDUDDUUDRURLDLRRUUURLUDDDRRDDLDDDDLUURDDULDRLRURLULLURRDRLLURLLLURDURLDLUDUUDUULLRLDLLLLULRDDLDUDUDDDUULURRLULDLDRLRDRLULLUDDUUUUURDRURLDUULDRRDULUDUDLDDRDLUDDURUDURLDULRUDRRDLRLRDRRURLDLURLULULDDUUDLRLLLLURRURULDDRUUULLDULDRDULDDDLLLRLULDDUDLRUDUDUDURLURLDDLRULDLURD\r\nDRUDRDURUURDLRLUUUUURUDLRDUURLLDUULDUULDLURDDUULDRDDRDULUDDDRRRRLDDUURLRDLLRLRURDRRRDURDULRLDRDURUDLLDDULRDUDULRRLLUDLLUUURDULRDDLURULRURDDLRLLULUDURDRRUDLULLRLDUDLURUDRUULDUDLRDUDRRDULDDLDRLRRULURULUURDULRRLDLDULULRUUUUULUURLURLRDLLRRRRLURRUDLRLDDDLDRDRURLULRDUDLRLURRDRRLRLLDLDDLLRRULRLRLRUDRUUULLDUULLDDRLUDDRURLRLDLULDURLLRRLDLLRDDDUDDUULLUDRUDURLLRDRUDLUDLLUDRUUDLRUURRRLLUULLUUURLLLRURUULLDLLDURUUUULDDDLRLURDRLRRRRRRUDLLLRUUULDRRDLRDLLDRDLDDLDLRDUDLDDRDDDDRULRRLRDULLDULULULRULLRRLLUURUUUDLDLUDUDDDLUUDDDDUDDDUURUUDRDURRLUULRRDUUDDUDRRRDLRDRLDLRRURUUDRRRUUDLDRLRDURD\r\nDDDLRURUDRRRURUUDLRLRDULDRDUULRURRRUULUDULDDLRRLLRLDDLURLRUDRLRRLRDLRLLDDLULDLRRURDDRDLLDDRUDRRRURRDUDULUDDULRRDRLDUULDLLLDRLUDRDURDRRDLLLLRRLRLLULRURUUDDRULDLLRULDRDLUDLULDDDLLUULRRLDDUURDLULUULULRDDDLDUDDLLLRRLLLDULRDDLRRUDDRDDLLLLDLDLULRRRDUDURRLUUDLLLLDUUULDULRDRULLRDRUDULRUUDULULDRDLDUDRRLRRDRLDUDLULLUDDLURLUUUDRDUDRULULDRDLRDRRLDDRRLUURDRULDLRRLLRRLDLRRLDLDRULDDRLURDULRRUDURRUURDUUURULUUUDLRRLDRDLULDURUDUDLUDDDULULRULDRRRLRURLRLRLUDDLUUDRRRLUUUDURLDRLRRDRRDURLLL";

        private readonly int[,] KeyPad = {{1, 2, 3}, 
                                         {4, 5, 6}, 
                                         {7, 8, 9}};

        private readonly string[] RealKeyPad =
        {
            "00100",
            "02340",
            "56789",
            "0ABC0",
            "00D00"
        };

        private int X, Y;

        public void Part1()
        {
            var instructionsLines = input.Replace("\r", "").Split(null);
            var result = "";
            // Startposition --> 5
            X = 1; Y = 1;
            foreach (string instructionsLine in instructionsLines)
            {
                foreach (char instruction in instructionsLine)
                {
                    switch (instruction)
                    {
                        case 'U':
                            Y = NewPositionPart1(Y, -1);
                            break;
                        case 'D':
                            Y = NewPositionPart1(Y, 1);
                            break;
                        case 'L':
                            X = NewPositionPart1(X, -1);
                            break;
                        case 'R':
                            X = NewPositionPart1(X, 1);
                            break;
                    }
                }
                result += KeyPad[Y, X];
            }
            Console.WriteLine($"Part1: {result}");
        }
        public void Part2()
        {
            var instructionsLines = input.Replace("\r", "").Split(null);
            var result = "";
            // Startposition --> 5
            X = 0; Y = 2;
            foreach (string instructionsLine in instructionsLines)
            {
                foreach (char instruction in instructionsLine)
                {
                    switch (instruction)
                    {
                        case 'U':
                            Y = NewPositionPart2(Y, -1, 'Y');
                            break;
                        case 'D':
                            Y = NewPositionPart2(Y,  1, 'Y');
                            break;
                        case 'L':
                            X = NewPositionPart2(X, -1);
                            break;
                        case 'R':
                            X = NewPositionPart2(X, 1);
                            break;
                    }
                }
                result += RealKeyPad[Y][X];
            }
            Console.WriteLine($"Part2: {result}");

        }

        private int NewPositionPart1(int currentPosition, int addValue)
        {
            if (currentPosition == 0 && addValue < 0)
                return 0;
            if (currentPosition == 2 && addValue > 0)
                return 2;
            return currentPosition + addValue;
        }

        private int NewPositionPart2(int currentPosition, int addValue, char direction = 'X')
        {
            if (currentPosition == 0 && addValue < 0)
                return 0;
            if (currentPosition == 4 && addValue > 0)
                return 4;

            int tmpValue = currentPosition + addValue;
            if (direction == 'X')
            {
                if (RealKeyPad[Y][tmpValue] != '0')
                    return tmpValue;
                else
                    return currentPosition;
            }
            else
            { // direction == 'Y'
                if (RealKeyPad[tmpValue][X] != '0')
                    return tmpValue;
                else
                    return currentPosition;
            }           
        }


    }
}
