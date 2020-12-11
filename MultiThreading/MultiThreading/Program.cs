using System;
using System.Collections.Concurrent;
using System.Runtime.Serialization.Formatters;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading
{
    class Program
    {
        // while adding 11th element, it will block the process until there are placed
        static BlockingCollection<int> messages = new BlockingCollection<int>(new ConcurrentBag<int>(), 10);
        
        static CancellationTokenSource cts = new CancellationTokenSource();
        private static Random rand = new Random();
        public static void Main(string[] args)
        {
            Task.Factory.StartNew(ProduceAndConsume, cts.Token);
            Console.ReadKey();
            cts.Cancel();
        }

        private static void ProduceAndConsume()
        {
            var producer = Task.Factory.StartNew(RunProducer);
            var consumer = Task.Factory.StartNew(Consumer);
            try
            {
                Task.WaitAll(new[] {producer, consumer}, cts.Token);
            }
            catch (AggregateException aggregate)
            {
                aggregate.Handle(e => true);
            }
        }

        private static void Consumer()
        {
            // as soon as some one adds something to the message, its going to give us this enumeration
            // its gonna wait being blocked for the next element to come in
            // GetConsuming means we consumed and got it in the variable item
            foreach (var item in messages.GetConsumingEnumerable())
            {
                cts.Token.ThrowIfCancellationRequested();
                Console.WriteLine($"removed {item}");
                Thread.Sleep(rand.Next(1000));
            }
        }

        private static void RunProducer()
        {
            while (true)
            {
                cts.Token.ThrowIfCancellationRequested();
                int i = rand.Next(10);
                messages.Add(i);
                Console.WriteLine($"added {i}");
                Thread.Sleep(rand.Next(100));
            }
        }
    }
}