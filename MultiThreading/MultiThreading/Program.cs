using System;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading
{
    class Program
    {
        public static void Main(string[] args)
        {
            var parentTask = new Task(() =>
            {
                var childTask = new Task(() =>
                {
                    Console.WriteLine($"child task started");
                    Thread.Sleep(5000);
                    Console.WriteLine("Child task finished");
                    throw new Exception();
                }, TaskCreationOptions.AttachedToParent);

                Task successCompletionHandle = childTask.ContinueWith(
                    t => { Console.WriteLine($"SUCCESS! Task {t.Id} status {t.Status}"); },
                    TaskContinuationOptions.AttachedToParent | TaskContinuationOptions.OnlyOnRanToCompletion);

                Task failureCompletionHandle = childTask.ContinueWith(
                    t => { Console.WriteLine($"FAIULURE! Task {t.Id} status {t.Status}"); },
                    TaskContinuationOptions.AttachedToParent | TaskContinuationOptions.NotOnRanToCompletion);

                childTask.Start();
            });

            parentTask.Start();
            parentTask.Wait();
        }

    }
}