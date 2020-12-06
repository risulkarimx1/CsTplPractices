using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualBasic.CompilerServices;

namespace MultiThreading
{
    class Program
    {

        static void WaitingTask(int time)
        {
            Console.WriteLine($"at task id {Task.CurrentId}");
            Thread.Sleep(time);
            Console.WriteLine($"Done task id {Task.CurrentId}");
        }

        static void Main(string[] args)
        {
            var t1 = new Task(() => WaitingTask(2000));
            var t2 = new Task(() => WaitingTask(3000));
            t1.Start();
            t2.Start();

            Task.WaitAll(t1, t2);

            Console.WriteLine("Done with tasks");

            Console.ReadKey();
        }
    }}
