using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Mime;
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

            var getData = new Func<string, IObservable<IEnumerable<string>>>(c => Observable.FromAsync(() => _indipendentProcessesMock.GetData(c)));

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

        }
    }
}
