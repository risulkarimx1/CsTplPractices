using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualBasic.CompilerServices;

namespace MultiThreading
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                TaskCreatorMethod();
            }
            catch (AggregateException ae)
            {
                foreach (var ex in ae.InnerExceptions)
                {
                    Console.WriteLine($" Handled outside {ex.GetType()} from source {ex.Source}");
                }
            }

            Console.WriteLine("Completed");
            Console.ReadKey();
        }

        private static void TaskCreatorMethod()
        {
            var t1 = new Task(() => { throw new InvalidOperationException("Invalid ops exception") {Source = "T1"}; });

            var t2 = new Task(() => { throw new AccessViolationException("invalid access") {Source = "T2"}; });

            t1.Start();
            t2.Start();

            try
            {
                // Exception only gets propagated with wait all/any is called
                Task.WaitAll(t1, t2);
            }
            catch (AggregateException ae)
            {
                ae.Handle(ex =>
                {
                    // if invalidops exception, dont propagate ... throw otherwise
                    if (ex is InvalidOperationException)
                    {
                        Console.WriteLine($"inside exception handled {ex.GetType()} from source {ex.Source}");
                        return true;
                    }

                    return false;
                });
            }
        }
    }}
