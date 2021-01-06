using System.Threading.Tasks;

namespace MultiThreading
{
    public class Foo
    {
        private Foo()
        {
        }

        private async Task<Foo> InitAsync()
        {
            await Task.Delay(1000); // calling heavy function
            return this;
        }

        public static async Task<Foo> CreateAsync()
        {
            var result = new Foo();
            return await result.InitAsync();
        }
    }
    class Program
    {
        public static async Task Main(string[] args)
        {
            Foo foo = await Foo.CreateAsync();
        }
    }
}