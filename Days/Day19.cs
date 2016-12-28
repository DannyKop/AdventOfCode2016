using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2016.Days
{
    internal sealed class Day19 : IDay
    {
        private const int Input = 3012210;
        public void Part1()
        {            
            var elves = Enumerable.Range(1, Input).Select(id => new Elf(id)).ToList();

            while (elves.Count != 1)
            {
                for (int i = 0; i < elves.Count; i++)
                {
                    if (elves[i].Presents == 0)
                        continue;

                    int target = i + 1;
                    if (target == elves.Count)
                        target = 0;

                    elves[i].Presents += elves[target].Presents;
                    elves[target].Presents = 0;
                }
                elves = elves.Where(e => e.Presents != 0).ToList();                
            }
            var elf = elves.First();
            Console.WriteLine($"Part1 - Winning elf: {elf.Id}");
        }

        public void Part2()
        {
            // thx to /user/Smylers for his (Perl) explanation! 
            var elves = Enumerable.Range(1, Input).Select(id => new Elf(id)).ToList();
            var left = new LinkedList<Elf>();
            var right = new LinkedList<Elf>();

            int half = elves.Count / 2;
            for (int i = 0; i < elves.Count; i++)
            {
                if (i < half)
                    left.AddLast(elves[i]);
                else
                    right.AddLast(elves[i]);
            }

            while (left.Count + right.Count != 1)
            {
                Elf e = left.First.Value;
                left.Remove(e);
                if(left.Count == right.Count)
                    left.RemoveLast();
                else
                    right.RemoveFirst();
                right.AddLast(e);
                e = right.First.Value;
                right.Remove(e);
                left.AddLast(e);
            }

            Console.WriteLine($"Part2 - Winning elf: {left.First.Value.Id}");
        }

        internal sealed class Elf
        {
            public int Id { get; }

            public int Presents { get; set; }

            public Elf(int id)
            {
                Presents = 1;
                Id = id;
            }
        }
    }

}
