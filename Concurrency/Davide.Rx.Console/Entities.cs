using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Davide.Rx.Console
{
    public class Customer
    {
        public Customer() { Orders = new ObservableCollection<Order>(); }
        public string CustomerName { get; set; }
        public string Region { get; set; }
        public ObservableCollection<Order> Orders { get; private set; }
    }

    public class Order
    {
        public int OrderId { get; set; }
        public DateTimeOffset OrderDate { get; set; }
    }


    public class WcfSimulator
    {
        public async static Task<IEnumerable<Order>> GetOrder()
        {
            System.Console.WriteLine("I am WCF and I have been called");
            await Task.Delay(6000);
            return new[] { new Order() { OrderId = 1 }, new Order() { OrderId = 2 }, new Order() { OrderId = 3 }, new Order() { OrderId = 4 }, new Order() { OrderId = 5 } };
        }
    }

    public static class DatabaseSimulator
    {
        public static IObservable<string> GetProducts()
        {
            var lst = new[] {"a", "b", "c", "d", "e", "f", "g", "h"};

            return Observable.Create<string>(observer =>
            {
                System.Console.WriteLine("Connect to database");
                Thread.Sleep(2000);
                System.Console.WriteLine("Start retrieve data to database");
                for (int i = 0; i < lst.Count(); i++)
                {
                    observer.OnNext(lst[i]);
                }
                observer.OnCompleted();

                return Disposable.Create(() => System.Console.WriteLine("--Disposed--")); 
            });

        }
    }


    public class Ping : ISubject<Pong, Ping>
    {
        #region Implementation of IObserver<Pong>

        /// <summary>
        /// Notifies the observer of a new value in the sequence.
        /// </summary>
        public void OnNext(Pong value)
        {
            System.Console.WriteLine("Ping received Pong.");
        }

        /// <summary>
        /// Notifies the observer that an exception has occurred.
        /// </summary>
        public void OnError(Exception exception)
        {
            System.Console.WriteLine("Ping experienced an exception and had to quit playing.");
        }

        /// <summary>
        /// Notifies the observer of the end of the sequence.
        /// </summary>
        public void OnCompleted()
        {
            System.Console.WriteLine("Ping finished.");
        }

        #endregion

        #region Implementation of IObservable<Ping>

        /// <summary>
        /// Subscribes an observer to the observable sequence.
        /// </summary>
        public IDisposable Subscribe(IObserver<Ping> observer)
        {
            return Observable.Interval(TimeSpan.FromSeconds(2))
                .Where(n => n < 10)
                .Select(n => this)
                .Subscribe(observer);
        }

        #endregion

        #region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            OnCompleted();
        }

        #endregion
    }

    public class Pong : ISubject<Ping, Pong>
    {
        #region Implementation of IObserver<Ping>

        /// <summary>
        /// Notifies the observer of a new value in the sequence.
        /// </summary>
        public void OnNext(Ping value)
        {
            System.Console.WriteLine("Pong received Ping.");
        }

        /// <summary>
        /// Notifies the observer that an exception has occurred.
        /// </summary>
        public void OnError(Exception exception)
        {
            System.Console.WriteLine("Pong experienced an exception and had to quit playing.");
        }

        /// <summary>
        /// Notifies the observer of the end of the sequence.
        /// </summary>
        public void OnCompleted()
        {
            System.Console.WriteLine("Pong finished.");
        }

        #endregion

        #region Implementation of IObservable<Pong>

        /// <summary>
        /// Subscribes an observer to the observable sequence.
        /// </summary>
        public IDisposable Subscribe(IObserver<Pong> observer)
        {
            return Observable.Interval(TimeSpan.FromSeconds(1.5))
                .Where(n => n < 10)
                .Select(n => this)
                .Subscribe(observer);
        }

        #endregion

        #region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            OnCompleted();
        }

        #endregion
    }
}
