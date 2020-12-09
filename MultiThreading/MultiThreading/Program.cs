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
        private static SpinLock sl = new SpinLock(true);
        public static void LockRecursion(int x)
        {
            bool lockTaken = false;
            try
            {
                sl.Enter(ref lockTaken);
            }
            catch (LockRecursionException e)
            {
                Console.WriteLine($"exception is {e}");
            }
            finally
            {
                if (lockTaken)
                {
                    Console.WriteLine($"Lock was taken with the value {x}");
                    LockRecursion(x-1);
                    sl.Exit();
                }
                else
                {
                    Console.WriteLine($"failed to take a lock with the value {x}");
                }
            }
        }

        private static void Main(string[] arg)
        {
            LockRecursion(5);
        }
    }
}