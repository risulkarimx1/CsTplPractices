using System;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading
{
    class Program
    {
        public static void Main(string[] args)
        {
            var semaphore = new SemaphoreSlim(2, 10); // initial count 2, max 10
            for (int i = 0; i < 20; i++)
            {
                Task.Factory.StartNew(() =>
                {
                    Console.WriteLine($"Entering task id {Task.CurrentId} > > ");
                    semaphore.Wait(); // Release count --
                    Console.WriteLine($"> > finished task {Task.CurrentId} XX");
                });
            }

            while (semaphore.CurrentCount <= 2)
            {
                Console.WriteLine($"Semaphore count {semaphore.CurrentCount}");
                Console.ReadKey();// press any key, releases 2 semaphore flags
                semaphore.Release(2); // releaseCount += 2
            }
        }
    }
}