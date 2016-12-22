using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2016.Days
{
    internal sealed  class Day12 : IDay
    {
        private readonly string[] _input = File.ReadAllLines("Resources/InputDay12.txt");

        public void Part1()
        {
            var register = new Dictionary<string, int> {{"a", 0}, {"b", 0}, {"c", 0}, {"d", 0}};
            Process(register, _input);

            Console.WriteLine($"Part1: A: {register["a"]}");

        }       

        public void Part2()
        {
            var register = new Dictionary<string, int> { { "a", 0 }, { "b", 0 }, { "c", 1 }, { "d", 0 } };
            Process(register, _input);

            Console.WriteLine($"Part2: A: {register["a"]}");
        }
        // No parser this time ^_^'
        private static void Process(IDictionary<string, int> register, string[] input)
        {            
            for (var i = 0; i < input.Length; i++)
            {
                var elem = input[i].Split(' ');
                if (input[i].StartsWith("cpy"))
                {
                    var value = register.ContainsKey(elem[1]) ? register[elem[1]] : int.Parse(elem[1]);
                    register[elem.Last()] = value;
                }

                if (input[i].StartsWith("inc"))
                    register[elem.Last()]++;                

                if (input[i].StartsWith("dec"))               
                    register[elem.Last()]--;                

                if (input[i].StartsWith("jnz"))
                {
                    var value = register.ContainsKey(elem[1]) ? register[elem[1]] : int.Parse(elem[1]);
                    if (value != 0)                    
                        i += int.Parse(elem.Last()) - 1;                    
                }
            }
        }
    }
}
