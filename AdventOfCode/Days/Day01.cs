using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode.Days
{
    public class Day01 : ISolution
    {
        public string PartOne(string[] input)
        {
            
            var val = input.Select(x => Convert.ToInt32(x)).ToArray();
            var inc = 0;

            for (var i =1; i< val.Length; i++) {
                if (val[i] > val[i-1]) {
                    inc++;
                }
            }
            return inc.ToString();
        }

        public string PartTwo(string[] input)
        {
        
            var val = input.Select(x => Convert.ToInt32(x)).ToArray();
            var sums = new List<int>();


            for (var i =2; i< val.Length; i++) {
                sums.Add(val[i-2] + val[i-1] + val[i]);
            }

            var inc = 0;
            for (var i =1; i<sums.Count(); i++) {
                if (sums[i] > sums[i-1]) {
                    inc++;
                }
            }
     
            return inc.ToString();
        }

        public int Day => 01;
    }
}