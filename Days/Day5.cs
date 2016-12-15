using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace AdventOfCode2016.Days
{
    internal sealed class Day5 : IDay
    {
        private const string input = "wtnhxymk";
        private readonly Stopwatch stopwatch = new Stopwatch();
        private readonly MD5CryptoServiceProvider hasher = new MD5CryptoServiceProvider();

        public void Part1()
        {            
            var password = string.Empty;            

            int i = 0;
            stopwatch.Restart();            
            while(password.Length < 8)
            { 
                var name = $"{input}{i}";
                var hex = BitConverter.ToString(hasher.ComputeHash(Encoding.UTF8.GetBytes(name))).Replace("-", "");

                if (hex.StartsWith("00000"))
                    password += hex[5];
                i++;
            }
            stopwatch.Stop();
                                     
            Console.WriteLine($"Part1: '{password}', found in {stopwatch.ElapsedMilliseconds} ms");
        }

        public void Part2()
        {
            Dictionary<int, char> password = new Dictionary<int, char>();            
            
            int i = 0;
            stopwatch.Restart();            
            while (password.Count < 8)
            {
                var name = $"{input}{i}";
                var hex = BitConverter.ToString(hasher.ComputeHash(Encoding.UTF8.GetBytes(name))).Replace("-", "");

                if (hex.StartsWith("00000"))
                {
                    int pos = -1; 
                    if (!int.TryParse(hex[5].ToString(), out pos))
                    {
                        i++;
                        continue;
                    }
                        
                    if (pos >= 0 && pos < 8 )
                    {
                        if(!password.ContainsKey(pos))
                            password[pos] = hex[6];                        
                    }
                }                    
                i++;
            }
            stopwatch.Stop();
            Console.WriteLine($"Part2: '{string.Join("", password.OrderBy(p => p.Key).Select(p => p.Value))}', found in {stopwatch.ElapsedMilliseconds} ms");
        }
    }
}
