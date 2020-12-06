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

        static void IsCanceled(int? taskId)
        {
            Console.WriteLine($"task id {taskId} is cancelled");
        }

        static void Main(string[] args)
        {
            var cts = new CancellationTokenSource();
            var token = cts.Token;
            var t = new Task(()=> NumberGenerator(token));
            token.Register( () => IsCanceled(1)); // to get notified when task is cancelled
            t.Start();

            var cts2 = new CancellationTokenSource();
            var token2 = cts2.Token;
            var t2 = new Task(() => NumberGenerator(token2));
            t2.Start();
            token2.Register(() => IsCanceled(2));

            Console.ReadKey();
            cts.Cancel();
            cts2.Cancel();

            Console.WriteLine("Main program done");
            Console.ReadKey();
        }
    }}
