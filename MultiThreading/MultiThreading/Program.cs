using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace MultiThreading
{
    class Program
    {
        static ConcurrentDictionary<string, string> capitals = new ConcurrentDictionary<string, string>();
        public static void AddParis()
        {
            var success = capitals.TryAdd("France", "Paris");
            var who = Task.CurrentId.HasValue ? $"Task {Task.CurrentId}" : "Main Thread";
            Console.WriteLine($"{who} could add value with success {success}");
        }

        public static void Main(string[] args)
        {
            capitals["Russia"] = "Leningrad";
            capitals.AddOrUpdate("Russia", "Moscow", (key, oldValue) => $"{oldValue} -> Moscow" );
            Console.WriteLine($"capital of russia is {capitals["Russia"]}");
        }
    }
}