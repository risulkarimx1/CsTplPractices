using System;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading
{
    class Program
    {
        public static void Main(string[] args)
        {
            var autoResetEvent = new AutoResetEvent(false);

            var task1= Task.Factory.StartNew(() =>
            {
                Console.WriteLine($"starting task one {Task.CurrentId}");
                Thread.Sleep(5000);
                Console.WriteLine($"done with task one");
                autoResetEvent.Set();
            });

            var task2 = Task.Factory.StartNew(() =>
            {
                Console.WriteLine($"Starting task two {Task.CurrentId}");
                autoResetEvent.WaitOne(); // sets the reset event to false...
                Console.WriteLine($"done with task two");
                autoResetEvent.WaitOne(); // it will hang here forever...
                                            // manualresetevent needs to be set again
            });

            task2.Wait();
        }
    }
}