using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2016.Days
{
    internal sealed class Day14 : IDay
    {
        private static readonly string Input = "ihaygndm";

        private readonly IDictionary<int, string> _keys = new Dictionary<int, string>();
        private readonly IDictionary<int, string> _hashes = new Dictionary<int, string>();
        
        public void Part1()
        {
            Hashing();
            Console.WriteLine($"Part1: index of hash #64: {_keys.Last()}");
        }

        public void Part2()
        {
            Hashing(useStrechted: true);
            Console.WriteLine($"Part2: Index of hash #64: {_keys.Last()}");
        }

        private void Hashing(bool useStrechted = false)
        {
            int i = 0;
            _keys.Clear();
            _hashes.Clear(); 
            while (_keys.Count < 64)
            {
                if (!_hashes.ContainsKey(i))
                    _hashes.Add(i, CreateHash(i, useStrechted));

                char? c = FindTripleChar(_hashes[i]);
                if (!c.HasValue)
                {
                    i++;
                    continue;
                }
                char letter = c.Value;
                if (IsValidInRange(letter, i, useStrechted))
                    if (!_keys.ContainsKey(i))
                        _keys.Add(i, _hashes[i]);
                i++;
            }
        }

        private static char? FindTripleChar(string value)
        {
            Match match = Regex.Match(value, @"(.)\1\1");
            if (match.Success)
                return match.Groups[0].Value.First();
            return null;
        }

        private bool IsValidInRange(char input, int currentIndex, bool useStrechted = false)
        {
            for (int i = currentIndex + 1; i < currentIndex + 1000; i++)
            {
                if (!_hashes.ContainsKey(i))
                    _hashes.Add(i, CreateHash(i, useStrechted));
                
                if(_hashes[i].Contains(new string(input, 5)))                
                    return true;
            }
            return false;
        }

        private  string CreateHash(int index, bool useStretched = false)
        {
            MD5 md5 = new MD5Cng();           
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes($"{Input}{index}"));

            if (useStretched)
            {
                for (int i = 0; i < 2016; i++)
                {
                    hash = md5.ComputeHash(Encoding.UTF8.GetBytes(BitConverter.ToString(hash).Replace("-", "").ToLower()));                    
                }
            }
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }  
    }
}
