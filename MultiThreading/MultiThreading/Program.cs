using System;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading
{
    class Program
    {
        public static void Main(string[] args)
        {
            var manualResetEvent = new ManualResetEventSlim();

            var task1= Task.Factory.StartNew(() =>
            {
                Console.WriteLine($"starting task one {Task.CurrentId}");
                Thread.Sleep(5000);
                Console.WriteLine($"done with task one");
                manualResetEvent.Set();
            });

            var task2 = Task.Factory.StartNew(() =>
            {
                Console.WriteLine($"Starting task two {Task.CurrentId}");
                manualResetEvent.Wait();
                Console.WriteLine($"done with task two");
            });

            task2.Wait();
        }
    }
}