using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualBasic.CompilerServices;

namespace MultiThreading
{
    public class BankAccount
    {
        object padLock = new object();
        public int Balacne { get; private set; }

        public void AddBalance(int amt)
        {
            lock (padLock)
            {
                Balacne += amt;
            }
        }

        public void DeductBalance(int amt)
        {
            lock (padLock)
            {
                Balacne -= amt;
            }
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

            Console.WriteLine($"End balance {ba.Balacne}");

        }
    }}
