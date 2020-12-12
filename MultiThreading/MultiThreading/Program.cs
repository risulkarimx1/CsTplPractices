using System;
using System.Threading.Tasks;

namespace MultiThreading
{
    class Program
    {
        public static void Main(string[] args)
        {
            Task<string> task1 = Task.Factory.StartNew(() => "Task 1");
            Task<string> task2 = Task.Factory.StartNew(() => "Task 2");

            var task3 = Task.Factory.ContinueWhenAny(new[] {task1, task2}, t =>
            {
                Console.WriteLine($"Task completed");
                Console.WriteLine($"{t.Result}");
            });

            task3.Wait();
        }

    }
}