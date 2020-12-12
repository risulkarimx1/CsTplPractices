using System;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading
{
    class Program
    {
        static Barrier barrier = new Barrier(2, b =>
        {
            Console.WriteLine($"Phase {b.CurrentPhaseNumber} is finished");
        });

        static void Buying()
        {
            Console.WriteLine($"Buy cement");
            Thread.Sleep(2000);
            Console.WriteLine($"Cement bought");
            barrier.SignalAndWait();
            Console.WriteLine($"Buy cranes");
            Thread.Sleep(1500);
            Console.WriteLine($"Cranes bought");
            barrier.SignalAndWait();
        }

        static void Construction()
        {
            Console.WriteLine($"Construct 3d models");
            Thread.Sleep(3000);
            Console.WriteLine($"3D Model done");
            barrier.SignalAndWait();
            Console.WriteLine($"Make bridge pillers");
            Thread.Sleep(5000);
            Console.WriteLine($"bridge piller completed");
            barrier.SignalAndWait();
        }

        public static void Main(string[] args)
        {
            Task buyingTask = Task.Factory.StartNew(Buying);
            Task constructionTask = Task.Factory.StartNew(Construction);

            var finalTask = Task.Factory.ContinueWhenAll(new[] {buyingTask, constructionTask}, tasks =>
            {
                foreach (var task in tasks)
                {
                    Console.WriteLine($"{task.Id} is completed");
                }

                Console.WriteLine($"Bridge making process completed");
            });

            finalTask.Wait();

        }

    }
}