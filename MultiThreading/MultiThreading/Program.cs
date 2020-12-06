using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualBasic.CompilerServices;

namespace MultiThreading
{
    class Program
    {
        // ways to stop task
        public static void NumberGenerator(CancellationToken cts)
        {
            var rand = new Random();
            while (true)
            {
                cts.ThrowIfCancellationRequested();
                var r = rand.Next(10000);
                Console.WriteLine($"id = {Task.CurrentId} --  {r}");
                // waits for 5 seconds... check if it was cancelled manually from outside
                var cancelled = cts.WaitHandle.WaitOne(5000);
                Console.WriteLine(cancelled ? "thread cancelled" : "was not cancelled");
            }
        }

        static void Main(string[] args)
        {
            var cts = new CancellationTokenSource();
            var token = cts.Token;
            
            var t = new Task(()=> NumberGenerator(token));
            t.Start();
            Console.ReadKey();
            cts.Cancel();
            Console.ReadKey();
        }
    }}
