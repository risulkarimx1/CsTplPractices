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
                // Recommended way is to throw exception... its not propagated 
                cts.ThrowIfCancellationRequested();
                var r = rand.Next(10000);
                Console.WriteLine($"id = {Task.CurrentId} --  {r}");
            }
        }

        static void Main(string[] args)
        {
            var cts = new CancellationTokenSource();
            var token = cts.Token;
            var t = new Task(()=> NumberGenerator(token));
            t.Start();

            var cts2 = new CancellationTokenSource();
            var t2 = new Task(() => NumberGenerator(cts2.Token));
            t2.Start();

            Console.ReadKey();
            cts.Cancel();
            cts2.Cancel();


            Console.WriteLine("Main program done");
            Console.ReadKey();
        }
    }}
