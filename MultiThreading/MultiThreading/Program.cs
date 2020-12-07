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
        }

        public void DeductBalance(int amt)
        {
            _balance -= amt;
        }
    }
    class Program
    { 

        private static void Main(string [] arg)
        {
            var ba = new BankAccount();

            var tasks = new List<Task>();

            var spinLock = new SpinLock();

            for (int i = 0; i < 1000; i++)
            {
                var t = Task.Factory.StartNew(() =>
                {
                    var couldLock = false;
                    try
                    {
                        spinLock.Enter(ref couldLock);
                        if (couldLock)
                        {
                            ba.AddBalance(100);
                        }
                    }
                    finally
                    {
                        if (couldLock)
                        {
                            spinLock.Exit();
                        }
                    }
                    
                });
                tasks.Add(t);
            }

            for (int i = 0; i < 1000; i++)
            {
                var t = Task.Factory.StartNew(() =>
                {
                    var couldLock = false;
                    try
                    {
                        spinLock.Enter(ref couldLock);
                        if (couldLock)
                        {
                            ba.DeductBalance(100);
                        }
                    }
                    finally
                    {
                        if (couldLock)
                        {
                            spinLock.Exit();
                        }
                    }
                });
                tasks.Add(t);
            }


            Task.WaitAll(tasks.ToArray());

            Console.WriteLine($"End balance {ba.Balance}");

        }
    }}
