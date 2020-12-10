using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading
{
    public class BankAccount
    {
        private int _balance;

        public int Balance
        {
            get => _balance;
            private set => _balance = value;
        }

        public void AddBalance(int amt)
        {
            _balance += amt;
//            Interlocked.Add(ref _balance, amt);
        }

        public void DeductBalance(int amt)
        {
            _balance -= amt;
//            Interlocked.Add(ref _balance, -amt);
        }

        public void Transfer(BankAccount other, int amt)
        {
            other.Balance += amt;
            _balance -= amt;
        }
    }

    class Program
    {
        private static ReaderWriterLockSlim padLock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
        private static Random rand = new Random();
        private static void Main(string[] arg)
        {
            int x = 0;
            var tasks = new List<Task>();
            for (int i = 0; i < 10; i++)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    // we are trying to read data, we might change data in between
                    //padLock.EnterReadLock();
                    padLock.EnterUpgradeableReadLock();

                    Console.WriteLine($"x value after read lock is {x}");
                    // every even counter, we want to change the value of x
                    if (i % 2 == 0)
                    {
                        padLock.EnterWriteLock();
                        x = rand.Next(10);
                        padLock.ExitWriteLock();
                    }

                    Thread.Sleep(5000);
                    //padLock.ExitReadLock();
                    padLock.ExitUpgradeableReadLock();
                    Console.WriteLine($"Exited read lock with the value {x}");
                }));
            }

            try
            {
                Task.WaitAll(tasks.ToArray());
            }
            catch (AggregateException ae)
            {
                ae.Handle(e =>
                {
                    Console.WriteLine(e);
                    return true;
                });
            }

            while (true)
            {
                Console.ReadKey();
                padLock.EnterWriteLock();
                Console.WriteLine($"write lock acuire when x = {x}");
                x = rand.Next(10);

                Console.WriteLine($"writing value of x = {x}");
                padLock.ExitWriteLock();
                Console.WriteLine($"Exiting write lock with value = {x}");
            }

        }
    }
}