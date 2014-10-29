using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;

namespace Concurrency.Tpl.ViewModel
{
    class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void FirePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private int itemCode = -1;

        public int ItemCode
        {
            get
            {
                return itemCode;
            }

            set
            {
                if (itemCode != value)
                {
                    itemCode = value;
                    FirePropertyChanged("ItemCode");
                }
            }
        }

        private int _percentComplete = 0;
        public int PercentComplete
        {
            get
            {
                return _percentComplete;
            }

            set
            {
                if (_percentComplete != value)
                {
                    _percentComplete = value;
                    FirePropertyChanged("PercentComplete");
                }
            }
        }
        private string description = "unknown";

        public string Description
        {
            get
            {
                return description;
            }

            set
            {
                if (description != value)
                {
                    description = value;
                    FirePropertyChanged("Description");
                }
            }
        }

        private ICommand exitCommand;

        public ICommand ExitCommand
        {
            get
            {
                return exitCommand ?? (exitCommand = new DelegateCommand(() =>
                {
                    Application.Current.MainWindow.Close();
                }));
            }
        }

        private ICommand addCommand;

        public ICommand AddCommand
        {
            get
            {
                return addCommand ?? (addCommand = new DelegateCommand(() => entries.Add(new { ItemCode = ItemCode, Description = Description })));
            }
        }

        private ObservableCollection<object> entries = new ObservableCollection<object>();

        public ObservableCollection<object> Entries
        {
            get { return entries; }
        }



        //async Task ProgressSample(IProgress<double> progress = null)
        //{
        //    bool done = false;
        //    while (!done)
        //    {
        //        await Task.Delay(200);
        //        PercentComplete = _percentComplete + 10;
        //        if (progress != null)
        //            progress.Report(PercentComplete);

        //        done = (PercentComplete >= 100);
        //    }
        //}
    }


}
