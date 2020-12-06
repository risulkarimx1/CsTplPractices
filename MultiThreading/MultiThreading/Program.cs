using System;
using System.Threading.Tasks;
using Microsoft.VisualBasic.CompilerServices;

namespace MultiThreading
{
    class Program
    {
        // ways to make tasks

        public static void Write(Char c)
        {
            var i = 1000;
            while (i-- > 0)
            {
                Console.Write(c);
            }
        }

        static void Main(string[] args)
        {
            Task.Factory.StartNew(() => Write('_')); // starts right away
            Task t = new Task(()=> Write('-')); // creates a task
            t.Start();// now start
            Write(','); // running on main thread
        }
    }
}
