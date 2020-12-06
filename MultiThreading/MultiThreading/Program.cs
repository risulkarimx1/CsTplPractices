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
            var t1 = new Task(() => WaitingTask(1000));
            var t2 = new Task(() => WaitingTask(3000));
            t1.Start();
            t2.Start();

            Task.WaitAny(new Task[]{ t1, t2 }, 2000); // added a max time span
            Console.WriteLine($"Task 1 status {t1.Status}");
            Console.WriteLine($"Task 2 status {t2.Status}");

            Console.WriteLine("Done with tasks");

            Console.ReadKey();
        }
    }}
