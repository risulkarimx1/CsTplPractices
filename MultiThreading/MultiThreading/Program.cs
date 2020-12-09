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

        private static void Main(string[] arg)
        {
            BankAccount ba = new BankAccount();
            BankAccount ba2 = new BankAccount();
            var mutex = new Mutex();
            var mutex2 = new Mutex();

            var task = new List<Task>();

            for (int j = 0; j < 10; j++)
            {
                task.Add(Task.Factory.StartNew(() =>
                {
                    for (int i = 0; i < 1000; i++)
                    {
                        var canlock = mutex.WaitOne();
                        try
                        {

                            ba.AddBalance(1);
                        }
                        finally
                        {
                            if(canlock) mutex.ReleaseMutex();
                        }
                    }
                }));

                task.Add(Task.Factory.StartNew(() =>
                {
                    for (int i = 0; i < 1000; i++)
                    {
                        var canlock = mutex2.WaitOne();
                        try
                        {

                            ba2.AddBalance(1);
                        }
                        finally
                        {
                            if (canlock) mutex2.ReleaseMutex();
                        }
                    }
                }));

                task.Add(Task.Factory.StartNew(() =>
                {
                    for (int i = 0; i < 1000; i++)
                    {
                        bool haveLocked = Mutex.WaitAll(new[] {mutex, mutex2});
                        try
                        {
                            ba.Transfer(ba2,1);
                        }
                        finally
                        {
                            if (haveLocked)
                            {
                                mutex.ReleaseMutex();
                                mutex2.ReleaseMutex();
                            }
                        }
                    }
                }));
            }

            Task.WaitAll(task.ToArray());

            Console.WriteLine($"Final balance {ba.Balance}");
            Console.WriteLine($"Final balance {ba2.Balance}");
        }
    }
}