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

            var task3 = Task.Factory.ContinueWhenAll(new[] {task1, task2}, tasks =>
            {
                Console.WriteLine($"Task completed");
                foreach (var task in tasks)
                {
                    Console.WriteLine($"{task.Result}");
                }

                Console.WriteLine($"All task completed");
            });

            task3.Wait();
        }

    }
}