using System;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading
{
    class Program
    {
        public static void Main(string[] args)
        {
            var parentTask = new Task(() =>
            {
                var childTask = new Task(() =>
                {
                    Console.WriteLine($"child task started");
                    Thread.Sleep(5000);
                    Console.WriteLine("Child task finished");
                }, TaskCreationOptions.AttachedToParent);
                childTask.Start();
            });

            parentTask.Start();
            parentTask.Wait();
        }

    }
}