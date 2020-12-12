using System;
using System.Threading.Tasks;

namespace MultiThreading
{
    class Program
    {
        public static void Main(string[] args)
        {
            var boil = Task.Factory.StartNew(() => { Console.WriteLine("Boiliing water"); });
            var pourWater = boil.ContinueWith(t =>
            {
                Console.WriteLine($"Completed task {boil.Id}. now Pouring water");
            });

            pourWater.Wait();
        }

    }
}