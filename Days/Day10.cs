using System;   
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2016.Days
{
    internal sealed class Day10 : IDay
    {
        private readonly string[] _input = File.ReadAllLines("Resources/InputDay10.txt");

        private readonly IList<Bot> _bots = new List<Bot>();        

        public void Part1()
        {
            var instructions = Parse().ToList();

            Bot winnerBot = null;

            var valueInstructions = instructions.OfType<ValueInstruction>().ToList();
            foreach (var vi in valueInstructions)
            {
                Bot bot = _bots.FirstOrDefault(b => b.Name == vi.BotName);
                if (bot == null)
                {
                    bot = new Bot(vi.BotName);
                    _bots.Add(bot);
                }
                bot.AddMicrochip(vi.Value);
                instructions.Remove(vi);
            }

            while (instructions.Any())
            {
                var processedInstructions = new List<IInstruction>();
                // list only contains elements of type BotInstruction
                foreach (var botInstruction in instructions.OfType<BotInstruction>())
                {                    
                    int botName = botInstruction.BotName;
                    Bot bot = _bots.FirstOrDefault(b => b.Name == botName);
                    if(bot == null || !bot.HasTwoMicrochips())
                        continue;

                    if (bot.Microchips.Contains(17) && bot.Microchips.Contains(61))
                    {
                        winnerBot = bot;
                        break;
                    }

                    Bot highBot = null; 
                    Bot lowBot = null; 
                    if (botInstruction.HighType.Equals("bot"))
                        highBot = _bots.FirstOrDefault(b => b.Name == botInstruction.HightName);
                    if (botInstruction.LowType.Equals("bot"))
                        lowBot = _bots.FirstOrDefault(b => b.Name == botInstruction.LowName);

                    
                    if(!VerifyBots(new[] {lowBot, highBot}))
                        continue;

                    int highName = bot.HighValue();
                    if (botInstruction.HighType.Equals("bot"))
                    {
                        if (highBot == null)
                        {
                            highBot = new Bot(botInstruction.HightName);
                            _bots.Add(highBot);
                        }
                        highBot.AddMicrochip(highName);
                    }
                   
                    int lowName = bot.LowValue();
                    if (botInstruction.LowType.Equals("bot"))
                    {
                        if (lowBot == null)
                        {
                            lowBot = new Bot(botInstruction.LowName);
                            _bots.Add(lowBot);
                        }
                        lowBot.AddMicrochip(lowName);
                    }                    
                    processedInstructions.Add(botInstruction);
                }

                foreach (var i in processedInstructions)
                    instructions.Remove(i);

                if (winnerBot != null)
                    break;
            }            
            Console.WriteLine($"Part1: winning bot: {winnerBot?.Name}");     
        }      

        public void Part2()
        {
        }

        private static bool VerifyBots(Bot[] bots)
        {
            return bots.All(VerifyBot);
        }

        private static bool VerifyBot(Bot bot)
        {
            if (bot == null)
                return true;

            return !bot.HasTwoMicrochips();
        }

        private IEnumerable<IInstruction> Parse()
        {
            const string botExpr = @"(bot)\s*(\d*)\s*(gives low to)\s*(bot|output)\s*(\d*)\s*(and high to)\s*(bot|output)\s*(\d*)";
            const string valueExpr = @"(value)\s*(\d*)\s*(goes to bot)\s*(\d*)";

            foreach (string line in _input)
            {
                Match match = null;
                IInstruction instruction = null;
                if (line.StartsWith("value"))
                {
                    match = Regex.Match(line, valueExpr);
                    int value = int.Parse(match.Groups[2].Value);
                    int bot = int.Parse(match.Groups[4].Value);
                    instruction = new ValueInstruction(bot, value);
                }
                if (line.StartsWith("bot"))
                {
                    match = Regex.Match(line, botExpr);
                    int botNr = int.Parse(match.Groups[2].Value);
                    string lowType = match.Groups[4].Value;
                    int lowName = int.Parse(match.Groups[5].Value);
                    string highType = match.Groups[7].Value;
                    int highName = int.Parse(match.Groups[8].Value);
                    instruction = new BotInstruction(botNr, lowType, lowName, highType, highName);
                }
                yield return instruction;
            }
        }

    }

    internal interface IInstruction
    {
        int BotName { get; }
    }

    internal sealed class ValueInstruction : IInstruction
    {
        public ValueInstruction(int bot, int value)
        {
            BotName = bot;
            Value = value;
        }

        public int BotName { get; }

        public int Value { get; }
    }

    internal sealed class BotInstruction : IInstruction
    {
        public BotInstruction(int botNr, string lowType, int lowName, string highType, int highName)
        {
            BotName = botNr;
            LowType = lowType;
            LowName = lowName;
            HighType = highType;
            HightName = highName;
        }

        public int BotName { get; }
        public string LowType { get; }
        public int LowName { get; }
        public string HighType { get; }
        public int HightName { get; }
    }

    internal sealed class Bot
    {
        private readonly List<int> _microships = new List<int>();

        public Bot(int name)
        {
            Name = name;
        }
        /// <summary>
        /// returns highest value and removes the microchip from the microchips list for this bot
        /// </summary>
        /// <returns></returns>
        public int HighValue()
        {
            int max = _microships.Max();
            _microships.Remove(max);
            return max;
        }
        /// <summary>
        /// returns lowest value and removes the microchip from the microchips list for this bot
        /// </summary>
        /// <returns></returns>
        public int LowValue()
        {
            int min = _microships.Min();
            _microships.Remove(min);
            return min;
        }

        public void AddMicrochip(int mc)
        {
            _microships.Add(mc);
        }

        public List<int> Microchips => _microships;

        public bool HasTwoMicrochips()
        {
            return _microships.Count == 2;
        }

        public int Name { get; }
    }
}
