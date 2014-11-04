using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Mime;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
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
using Concurrency.Rx.MockProcesses;

namespace Concurrency.Rx
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

        public MainWindow(IIndipendentProcessesMock indipendentProcessesMock)
        {
            InitializeComponent();
            _indipendentProcessesMock = indipendentProcessesMock;

            this.lsMyList.ItemsSource = _indipendentProcessesMock.GetData(string.Empty).Result;

            var getData =
                new Func<string, IObservable<IEnumerable<string>>>(
                    c => Observable.FromAsync(() => _indipendentProcessesMock.GetData(c)));

            var textObservable = Observable.FromEventPattern(this.txtSearch, "TextChanged")
                .Select(evt => ((TextBox)evt.Sender).Text)
                .Where(text => text != null && text.Length >= 1)
                .DistinctUntilChanged();

            var res = from searchValue in textObservable
                      from dictionaryData in getData(searchValue)
                          .TakeUntil(textObservable)
                      select dictionaryData;

            res.ObserveOn(SynchronizationContext.Current).Subscribe(c =>
            {
                this.lsMyList.ItemsSource = c;
            });



            var myObserver =
                Observable.FromEventPattern(this.btnConsume, "Click").ObserveOn(SynchronizationContext.Current).Subscribe(c =>
                {

                    Observable.FromAsync(() => _indipendentProcessesMock.ProcessOne("I am process One and I took 2 seconds for completing"))
                        .ObserveOn(Scheduler.ThreadPool)
                        .Zip(Observable.FromAsync(() => _indipendentProcessesMock.ProcessTwo("I am process Two and I took 7 seconds for completing")),
                            (left, right) => new[] { left, right })
                        .ObserveOn(SynchronizationContext.Current)
                        .Subscribe(items =>
                        {
                            this.lsMyList.ItemsSource = items;
                        });

                    Observable
                          .Timer(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(1)).Timestamp()
                          .ObserveOn(SynchronizationContext.Current)
                          .Subscribe(
                              x =>
                              {
                                  // Do Stuff Here
                                  this.lblTimer.Content = string.Format("{0}: {1}", x.Value, x.Timestamp.TimeOfDay);
                                  // Console WriteLine Prints
                                  // 0
                              });
                });


            //Hook mouse position
            IDisposable a = Observable.FromEventPattern<MouseEventArgs>(this, "MouseMove")
                .ObserveOn(SynchronizationContext.Current)
                .Subscribe(item => lblMousePosition.Content = item.EventArgs.GetPosition(this));
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            this.lsMyList.ItemsSource = null;
            Console.WriteLine("Shows use of Start to start on a background thread:");
            var o = Observable.Start(() =>
            {
                //This starts on a background thread.
                Console.WriteLine("From background thread. Does not block main thread.");
                Console.WriteLine("Calculating...");
                Thread.Sleep(1000);
                Console.WriteLine("Background work completed.");
            });
            await o.FirstAsync();
            Console.WriteLine("Main thread completed.");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.lsMyList.ItemsSource = null;
            var o = Observable.CombineLatest(
                Observable.Start(() => { Console.WriteLine("Executing 1st on Thread: {0}", Thread.CurrentThread.ManagedThreadId); return "Result A"; }),
                Observable.Start(() => { Console.WriteLine("Executing 2nd on Thread: {0}", Thread.CurrentThread.ManagedThreadId); return "Result B"; }),
                Observable.Start(() => { Console.WriteLine("Executing 3rd on Thread: {0}", Thread.CurrentThread.ManagedThreadId); return "Result C"; })
            ).Finally(() => Console.WriteLine("Done!"));

            foreach (string r in o.First())
                Console.WriteLine(r);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.lsMyList.ItemsSource = null;
            IObservable<int> ob =
                Observable.Create<int>(o =>
                {
                    var cancel = new CancellationDisposable(); // internally creates a new CancellationTokenSource
                    
                    NewThreadScheduler.Default.Schedule(() =>
                    {
                        int i = 0;
                        for (; ; ) //infinite loop
                        {
                            Thread.Sleep(200);  // here we do the long lasting background operation
                            if (!cancel.Token.IsCancellationRequested)    // check cancel token periodically
                                o.OnNext(i++);
                            else
                            {
                                Console.WriteLine("Aborting because cancel event was signaled!");
                                o.OnCompleted();
                                return;
                            }
                        }
                    }
                    );

                    return cancel;
                }
                );

            IDisposable subscription = ob.Subscribe(i => Console.WriteLine(i));
            Console.WriteLine("Press any key to cancel");
            Thread.Sleep(2000);
            subscription.Dispose();
            Console.WriteLine("Press any key to quit");
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            IDisposable myTimer =
                Observable.Interval(TimeSpan.FromSeconds(1)).Timestamp().ObserveOn(SynchronizationContext.Current).Subscribe(sec =>
                {
                    this.lblTime.Content = string.Format("{0}:{1}", sec.Timestamp.Second,sec.Timestamp.Millisecond);
                });

            Observable.FromEventPattern<RoutedEventHandler, RoutedEventArgs>(h => btnDisposeTimer.Click += h,
                h => btnDisposeTimer.Click -= h)
                .ObserveOn(SynchronizationContext.Current)
                .Subscribe(item =>
                {
                    myTimer.Dispose();
                    this.lblTime.Content = "Disposed";
                });

            //continue from http://rxwiki.wikidot.com/101samples#toc1   start from Where - Drilldown
        }
    }
                                                      
}
