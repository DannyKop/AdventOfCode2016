using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2016.Days
{
    internal sealed class Day11 : IDay
    {
        private readonly string[] _input = File.ReadAllLines("Resources/InputDay11.txt");
        private readonly Queue<Building> _unvisited = new Queue<Building>();
        private readonly HashSet<string> _visited = new HashSet<string>();

        public void Part1()
        {            
            Console.WriteLine($"Part1 - minimum number of steps: {Search(_input)}");
        } 

        public void Part2()
        {
            _input[0] =
                "The first newFloorNr contains a polonium generator, a elerium generator, a elerium-compatible microchip, a dilithium generator, a dilithium-compatible microchip, a thulium generator, " +
                "a thulium-compatible microchip, a promethium generator, a ruthenium generator, a ruthenium-compatible microchip, a cobalt generator, and a cobalt-compatible microchip.";

            Console.WriteLine($"Part2 - minimum number of steps: {Search(_input)}");           
        }

        private int Search(string[] input)
        {
            var startBuilding = ParseInput(input);
            _unvisited.Enqueue(startBuilding);

            int minimumSteps = int.MaxValue;
            while (_unvisited.Count > 0)
            {
                Building current = _unvisited.Dequeue();

                if (_visited.Contains(current.GetState()))
                    continue;
                _visited.Add(current.GetState());

                if (current.IsSolution())
                {
                    if (current.Steps < minimumSteps)
                    {
                        minimumSteps = current.Steps;
                        break;
                    }
                }

                ICollection<Building> nextMoves = GenerateNextStates(current);
                foreach (var building in nextMoves)
                    _unvisited.Enqueue(building);
            }
            return minimumSteps;
        }


        private ICollection<Building> GenerateNextStates(Building current)
        {
            var result = new List<Building>();
            var currentFloor = current.Floors[current.CurrentFloor];

            var compatibleItems = new List<Tuple<string, string>>();
            List<string> allItems = currentFloor.Generators.Concat(currentFloor.Microchips).ToList();            
            compatibleItems.AddRange(
                allItems.SelectMany(x => allItems, Tuple.Create)
                    .Where(t => t.Item1 != t.Item2).Where(x => ItemsCompatible(x.Item1, x.Item2)));
            compatibleItems.AddRange(allItems.Select(x => Tuple.Create<string, string>(x, null)));

            foreach (Tuple<string, string> t in compatibleItems)
            {
                ICollection<string> items = new List<string>() {t.Item1, t.Item2};
                if (current.CurrentFloor > 0)
                {
                    // down
                    Building newState = current.Clone();
                    newState = GenerateState(newState, items, current.CurrentFloor - 1);
                    if(newState != null && !_visited.Contains(newState.GetState()))
                        result.Add(newState);
                }

                if (current.CurrentFloor < 3)
                {
                    // up
                    Building newState = current.Clone();
                    newState = GenerateState(newState, items, current.CurrentFloor + 1);
                    if (newState != null && !_visited.Contains(newState.GetState()))
                        result.Add(newState);
                }
            }
            return result;
        }

        private static Building GenerateState(Building newState, ICollection<string> items , int newFloorNr)
        {            
            newState.Steps += 1;
            Floor currentFloor = newState.Floors[newState.CurrentFloor];            
            currentFloor.RemoveItems(items);

            Floor newFloor = newState.Floors[newFloorNr];
            newFloor.AddItems(items);

            newState.CurrentFloor = newFloorNr;            

            if(currentFloor.IsValid() && newFloor.IsValid())
                return newState;
            return null;
        }

        private static bool ItemsCompatible(string item1, string item2)
        {
            var item1Elements = item1.Split(' ');
            var item2Elements = item2.Split(' ');

            return Equals(item1Elements.Last(), item2Elements.Last()) ||
                   Equals(item1Elements.First(), item2Elements.First());
        }      

        private static Building ParseInput(IEnumerable<string> input)
        {
            Building resultBuilding = new Building();
            foreach (var line in input)
            {
                var elements = Regex.Split(line, " a ");
                var microchips = elements.Where(e => e.Contains("microchip")).Select(e => e.Replace("and", "").Replace(",", "").Replace(".", "").Replace("-compatible", "").Trim()).ToList();
                var generators = elements.Where(e => e.Contains("generator")).Select(e => e.Replace("and", "").Replace(",", "").Replace(".", "").Trim()).ToList();
                var floor = new Floor
                {
                    Generators = generators,
                    Microchips = microchips
                };
                resultBuilding.Floors.Add(floor);
            }
            resultBuilding.CurrentFloor = 0;
            resultBuilding.Steps = 0;
            return resultBuilding;
        }            
    }

    internal sealed class Building
    {
        public Building()
        {
            Floors = new List<Floor>();
        }
        public int CurrentFloor { get; set; }
        public int Steps { get; set; }
        public IList<Floor> Floors { get; }

        public Building Clone()
        {
            var clone = new Building()
            {
                Steps = Steps,
                CurrentFloor = CurrentFloor
            };
            foreach(var f in Floors)
                clone.Floors.Add(f.Clone());                            
            return clone;
        }

        public bool IsSolution()
        {
            return Floors.Last().IsComplete() && Floors.Take(Floors.Count - 1).All(f => f.IsEmpty());               
        }

        public string GetState()
        {
            var result = new StringBuilder();
            result.Append($"{CurrentFloor}");
            for (int i = 0; i < Floors.Count; i++)
            {
                result.Append($"{i}{Floors[i].GetState()}");
            }
            return result.ToString();
        }      
    }

    internal sealed class Floor
    {        
        public IList<string> Microchips { get; set; }
        public IList<string> Generators { get; set; }


        public Floor Clone()
        {
            var clone = new Floor
            {
                Generators = new List<string>(),
                Microchips = new List<string>()
            };
            foreach(var mc in Microchips)
                clone.Microchips.Add(mc);
            foreach(var g in Generators)
                clone.Generators.Add(g);
            return clone;
        }

        public bool IsValid()
        {             
            return Microchips.All(mc => Generators.Contains(mc.Replace("microchip", "generator"))) || !Generators.Any();        
        }

        public bool IsComplete()
        {
            return IsValid() && Microchips.Any() && Generators.Any();
        }

        public bool IsEmpty()
        {
            return !Microchips.Any() && !Generators.Any();
        }

        public void RemoveItems(ICollection<string> items)
        {
            foreach (var mc in items.Where(i => i != null && i.Contains("microchip")))
                Microchips.Remove(mc);
            foreach (var g in items.Where(i => i != null &&  i.Contains("generator")))
                Generators.Remove(g);
        }

        public void AddItems(ICollection<string> items)
        {
            foreach (var mc in items.Where(i => i != null && i.Contains("microchip")))
                Microchips.Add(mc);
            foreach (var g in items.Where(i => i != null &&  i.Contains("generator")))
                Generators.Add(g);
        }

        public string GetState()
        {
            var mc = Microchips.Count(m => !Generators.Any( g => g.Contains(m.Replace("microchip", "generator"))));
            var gen = Generators.Count(g => !Microchips.Any(m => m.Contains(g.Replace("microchip", "generator"))));
            var p = Generators.Count(g => Microchips.Any(m => m.Contains(g.Replace("generator", "microchip"))));

            return $"{mc}{gen}{p}";
        }       
    }
}
