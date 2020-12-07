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
            // add for addition... there are increment for ++ and --..
            Interlocked.Add(ref _balance, amt);
        }

        public void DeductBalance(int amt)
        {
            Interlocked.Add(ref _balance, - amt);
        }
    }
    class Program
    { 

        private static void Main(string [] arg)
        {
            var ba = new BankAccount();

            var tasks = new List<Task>();

            for (int i = 0; i < 1000; i++)
            {
                var t = new Task(() =>
                {
                    ba.AddBalance(100);
                });
                t.Start();
            }

            for (int i = 0; i < 1000; i++)
            {
                var t = new Task(() =>
                {
                    ba.DeductBalance(100);
                });
                t.Start();
            }

            Task.WaitAll(tasks.ToArray());

            Console.WriteLine($"End balance {ba.Balance}");

        }
    }}
