using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Concurrency.Tpl.MockProcesses;

namespace Concurrency.Tpl
{
    public interface IMainWindow
    {
        void Show();
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IMainWindow
    {
        private readonly IIndipendentProcessesMock _indipendentProcessesMock;
        private ConcurrentQueue<string> _myResult;

        public MainWindow(IIndipendentProcessesMock indipendentProcessesMock)
        {
            InitializeComponent();
            _indipendentProcessesMock = indipendentProcessesMock;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            lsMyList.ItemsSource = null;

            _myResult = new ConcurrentQueue<string>();

            Parallel.Invoke(
                () => _indipendentProcessesMock.ProcessOne(_myResult),
                    () => _indipendentProcessesMock.ProcessTwo(_myResult),
                        () => _indipendentProcessesMock.ProcessThree(_myResult)
                );


            lsMyList.ItemsSource = _myResult;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            lsMyList.ItemsSource = null;

            _myResult = new ConcurrentQueue<string>();

            Task.Factory.StartNew(() => _indipendentProcessesMock.ProcessOne(_myResult))
                .ContinueWith(res =>
                {
                    _myResult.Enqueue("TASK ONE COMPLETED");
                    _indipendentProcessesMock.ProcessTwo(_myResult);
                })
                .ContinueWith(res =>
                {
                    _myResult.Enqueue("TASK TWO COMPLETED");
                    _indipendentProcessesMock.ProcessThree(_myResult);
                }).Wait();

            lsMyList.ItemsSource = _myResult;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            lsMyList.ItemsSource = null;

            _myResult = new ConcurrentQueue<string>();
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();

            var a = Enumerable.Range(1, int.MaxValue).AsParallel().Select(c=>c * 2);

            stopwatch.Stop();

            _myResult.Enqueue(string.Format(" TIME FOR COMPLETE FIRST OPERATION: {0}", stopwatch.Elapsed));

            stopwatch.Reset();
            stopwatch.Start();

            Enumerable.Range(1, int.MaxValue).Select(c => c * 2);

            stopwatch.Stop();

            _myResult.Enqueue(string.Format(" TIME FOR COMPLETE SECOND OPERATION: {0}", stopwatch.Elapsed));

            lsMyList.ItemsSource = _myResult;
        }
    }
}
