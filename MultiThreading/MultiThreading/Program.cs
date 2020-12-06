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
            var regular = new CancellationTokenSource();
            var emergency = new CancellationTokenSource();

            var anyParanoid = CancellationTokenSource.CreateLinkedTokenSource(regular.Token, emergency.Token);
            
            var t = new Task(()=> NumberGenerator(anyParanoid.Token));
            t.Start();

            Console.ReadKey();
            regular.Cancel();
            Console.WriteLine("Main program done");
            Console.ReadKey();
        }
    }}
