using System;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading
{
    class Program
    {
        private static CountdownEvent countdownEvent = new CountdownEvent(5);
        private static Random sleepTime = new Random();
        public static void Main(string[] args)
        {
            for (int i = 0; i < 5; i++)
            {
                Task.Factory.StartNew(() =>
                {
                    Console.WriteLine($"Entering task -> {Task.CurrentId}");
                    Thread.Sleep(sleepTime.Next(3000));
                    countdownEvent.Signal(); // decreses the count by 1 
                    Console.WriteLine($"Exiting task -> {Task.CurrentId}");
                });
            }

            var anotherTask = Task.Factory.StartNew(() =>
            {
                Console.WriteLine($"entering final task with id {Task.CurrentId}");
                countdownEvent.Wait();
                Console.WriteLine($"Finished all task....");
                Console.WriteLine($"Finished final task....");
            });

            anotherTask.Wait();
        }
    }
}