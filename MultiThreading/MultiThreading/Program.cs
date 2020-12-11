using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MultiThreading
{
    class Program
    {
        

        public static void Main(string[] args)
        {
            ConcurrentBag<int> bag = new ConcurrentBag<int>();
            var tasks = new List<Task>();
            for (int i = 0; i < 10; i++)
            {
                var i1 = i;
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    bag.Add(i1);
                    Console.WriteLine($"Task id : {Task.CurrentId} has added {i1}");
                    int val;
                    if (bag.TryPeek(out val))
                    {
                        Console.WriteLine($"Task id: {Task.CurrentId} has peeked {val}");
                    }
                }));
            }

            Task.WaitAll(tasks.ToArray());
        }
    }
}