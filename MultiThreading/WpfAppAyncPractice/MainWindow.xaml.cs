using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace WpfAppAyncPractice
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public async Task<int> GetNumberFromDBAsync()
        {
            Console.WriteLine($"requesting data from db at thread {Thread.CurrentThread}");
            await Task.Delay(5000);
            return 123;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var calculation = await GetNumberFromDBAsync();
            textBlock.Text = calculation.ToString();

            await Task.Delay(4000);
            
            using (WebClient wc = new WebClient())
            {
                var data = await wc.DownloadStringTaskAsync("http://google.com/robots.txt");
                textBlock.Text = data.Split('\n')[0].Trim();
            }
        }
    }
}
