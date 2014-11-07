

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System;
namespace Davide.Rx.Console
{
    class Program
    {

        static void Main(string[] args)
        {
            //DrillDownTest();
            //RxTransformationFlow();
            //GroupEvents();
            //BufferAStream();
            //DelayStream();
            //Window();
            //JoinTwoObservableAndMatchingByAttribute();
            //PingPongActorModelWithISubject();
            //MergeSequences();
            //SharingSubscription();
            //RetrieveDataFromDatabaseSimulation();
            //RefCountDisposable();
            //SubjectExample();
            //RefCountSample();
            //RefCountSample();
            //AsyncWcfConsuming();
            //ObservableStartAndObservableDefer();
            //CatchErrors();
            DriveFolderExercise();
        }

        private static void DriveFolderExercise()
        {
            //var observable = Observable.ToObservable(GetAllDirectories(@"c:\"), Scheduler.ThreadPool).Buffer(1000);

            //observable.Subscribe(item => System.Console.WriteLine(string.Join(Environment.NewLine, item.Select(c => c))));

            Observable.Interval(TimeSpan.FromSeconds(2), Scheduler.ThreadPool)
                .Zip(GetAllDirectories(@"c:\").ToObservable(Scheduler.ThreadPool)
                .Buffer(10), (a, b) => b)
                .Subscribe(
                item => System.Console.WriteLine(string.Join(Environment.NewLine, item.Select(c => c))),
                () => System.Console.WriteLine("***COMPLETED***"));

            System.Console.ReadLine();
        }



        private static void CatchErrors()
        {

            var source = new Subject<int>();
            var result = source.Catch<int, TimeoutException>(tx =>
            {
                System.Console.WriteLine("Bloody hell something went wrong"); 
                return Observable.Return(-1);
            }).Finally(()=> System.Console.WriteLine("Finally I can get out from the flow"));


            result.Subscribe(System.Console.WriteLine);

            source.OnNext(1);
            source.OnNext(2);
            source.OnError(new TimeoutException());
            source.OnNext(3);
            System.Console.ReadLine();
        }


        private static void ObservableStartAndObservableDefer()
        {
            var random = new Random();

            Observable.Start(() => random.Next(0, 100))
                      .Delay(TimeSpan.FromMilliseconds(1000))
                      .Repeat()
                      .Subscribe(item => System.Console.WriteLine("******Stream ONE with NO DEFER: " + item));

            Observable.Defer(() => Observable.Start(() => random.Next(0, 100)))
                      .Delay(TimeSpan.FromMilliseconds(1000))
                      .Repeat()
                      .Subscribe(item => System.Console.WriteLine("Stream TWO with DEFER: " + item));

            System.Console.ReadLine();
        }


        private static void AsyncWcfConsuming()
        {

            var orders = WcfSimulator.GetOrder();


            //var obser = Observable.FromAsync<IEnumerable<Order>>(WcfSimulator.GetOrder);

            var obser = orders.ToObservable().Delay(TimeSpan.FromSeconds(3)).Repeat(3);
            //var obser = Observable.FromAsync<IEnumerable<Order>>(() =>
            //{
            //    System.Console.WriteLine("I am gonna call the async WS: ");
            //    return WcfSimulator.GetOrder(); 
            //}).Do(j =>System.Console.WriteLine("Igot results from WS"));

            System.Console.WriteLine("Created observable for WCF");

            var disposable = obser.Subscribe(
                c => System.Console.WriteLine(string.Join(" - ", c.Select(a => a.OrderId))),

                () => System.Console.WriteLine("Completed"));
            //using ))))
            //{
            //    //bser.Connect();
            //    obser.Wait();
            //}


            //obser.Wait();

            // System.Console.WriteLine("subscription disposed");

            //disposable.Dispose();

            System.Console.ReadLine();
        }

        private static void SubjectExample()
        {
            Subject<int> subject = new Subject<int>();
            var subscription = subject.OnErrorResumeNext(Observable.Range(10, 15))
                //.Catch((Exception ex) => Observable.Empty<int>())
                .Subscribe(
                                     x => System.Console.WriteLine("Value published: {0}", x),
                                     (error) => System.Console.WriteLine("Error as occured{0}", error.Message),
                                     () => System.Console.WriteLine("Sequence Completed."));
            try
            {

                subject.OnNext(1);

                subject.OnNext(2);

                subject.OnNext(Value(1)); //RAISE EXCEPTION

                subject.OnNext(3);
            }
            catch (System.Exception ex)
            {
                subject.OnError(ex);
            }

            System.Console.WriteLine("Press any key to continue");

            System.Console.ReadKey();

            subject.OnCompleted();

            subscription.Dispose();
        }

        private static void RefCountSample()
        {
            var period = TimeSpan.FromSeconds(1);
            var observable = Observable.Interval(period)
              .Do(l => System.Console.WriteLine("Publishing {0}", l)) //produce Side effect to show it is running.
              .Publish()
              .RefCount();
            //observable.Connect(); Use RefCount instead now
            System.Console.WriteLine("Press any key to subscribe");
            System.Console.ReadKey();

            var subscription = observable.Subscribe(i => System.Console.WriteLine("subscription one : {0}", i));
            System.Console.WriteLine("Second subscription");
            var subscriptiontwo = observable.Subscribe(i => System.Console.WriteLine("subscription two : {0}", i));

            System.Console.WriteLine("Press any key to unsubscribe first subscription");

            System.Console.ReadKey();

            subscription.Dispose();

            System.Console.WriteLine("Press any key to unsubscribe second subscription");

            System.Console.ReadKey();

            subscriptiontwo.Dispose();

            System.Console.WriteLine("Press any key to exit.");
            System.Console.ReadKey();
            /* Ouput:
             Press any key to subscribe
             Press any key to unsubscribe.
             Publishing 0
             subscription : 0
             Publishing 1
             subscription : 1
             Publishing 2
             subscription : 2
             Press any key to exit.
             */
        }

        private static void RefCountDisposable()
        {
            RefCountDisposable refDisposable = new RefCountDisposable(
                Disposable.Create(() =>
                    System.Console.WriteLine("Underlying disposable has been disposed")));

            IDisposable ref1 = refDisposable.GetDisposable();
            IDisposable ref2 = refDisposable.GetDisposable();
            IDisposable ref3 = refDisposable.GetDisposable();

            System.Console.WriteLine("Disposing ref2");
            ref2.Dispose();

            System.Console.WriteLine("Disposing RefCountDisposable");
            refDisposable.Dispose();

            System.Console.WriteLine("Disposing ref3");
            ref3.Dispose();

            System.Console.WriteLine("Disposing ref1");
            ref1.Dispose();

            System.Console.ReadLine();
        }

        private static void RetrieveDataFromDatabaseSimulation()
        {
            var productStream = DatabaseSimulator.GetProducts().Take(3);
            productStream.Subscribe(System.Console.WriteLine);//Here is starting to query database
            System.Console.ReadLine();
        }

        private static void SharingSubscription()
        {
            var unshared = Observable.Range(1, 4);

            // Each subscription starts a new sequence
            unshared.Subscribe(i => System.Console.WriteLine("Unshared Subscription A: " + i));
            unshared.Subscribe(i => System.Console.WriteLine("Unshared Subscription B: " + i));

            System.Console.WriteLine();

            // By using publish the subscriptions are shared, but the sequence doesn't start until Connect() is called.
            var shared = unshared.Publish();
            shared.Subscribe(i => System.Console.WriteLine("Shared Subscription AA " + i));
            Thread.Sleep(2000);
            shared.Subscribe(i => System.Console.WriteLine("Shared Subscription BB: " + i));
            System.Console.WriteLine(" add 2 subscriber... but still not display.. wait 2 seconds until connect");
            Thread.Sleep(2000);
            shared.Connect();

            System.Console.WriteLine("Press any key to exit");
            System.Console.ReadKey();
        }

        private static void MergeSequences()
        {
            //Generate values 0,1,2
            var stream1 = Observable.Interval(TimeSpan.FromSeconds(1)).Take(10);
            //Generate values 100,101,102,103,104
            var stream2 = Observable.Interval(TimeSpan.FromMilliseconds(500)).Take(20).Select(i => i + 100);

            var stream3 = Observable.Interval(TimeSpan.FromMilliseconds(100)).Take(10).Select(i => i + 200);

            var b = Observable.Merge(stream3);//merge and then contact (after finish first sequence start the next one

            var a = stream1
                .Merge(stream2).Concat(b)
                .Subscribe(streamMeged =>
                {
                    System.Console.WriteLine(streamMeged);
                });


            //.Subscribe(streamMerged => System.Console.WriteLine("3 streams merged together {0}", streamMerged));



            System.Console.ReadLine();
        }

        private static void PingPongActorModelWithISubject()
        {
            var ping = new Ping();
            var pong = new Pong();

            System.Console.WriteLine("Press any key to stop ...");

            var pongSubscription = ping.Subscribe(pong);
            var pingSubscription = pong.Subscribe(ping);

            System.Console.ReadKey();

            pongSubscription.Dispose();
            pingSubscription.Dispose();

            System.Console.WriteLine("Ping Pong has completed.");
        }

        private static void JoinTwoObservableAndMatchingByAttribute()
        {

            var l = Enumerable.Range(0, 10).ToObservable();
            var r = Enumerable.Range(8, 20).ToObservable();

            var q = l.GroupJoin(r,
                _ => Observable.Never<Unit>(), // windows from each left event going on forever
                _ => Observable.Never<Unit>(), // windows from each right event going on forever
                (left, obsOfRight) => Tuple.Create(left, obsOfRight)); // create tuple of left event with observable of right events

            // e is a tuple with two items, left and obsOfRight
            q.Subscribe(e =>
            {
                var xs = e.Item2;
                xs.Where(
                 x => x == e.Item1)
                 .Subscribe(
                 v =>
                 {
                     System.Console.WriteLine(
                        string.Format("{0},{1} and {2},{3} occur at the same time",
                        e.Item1,
                        e.Item1,
                        v,
                        v
                     ));
                 });
            });
            System.Console.ReadLine();
        }

        private static void Window()
        {
            IObservable<long> mainSequence = Observable.Interval(TimeSpan.FromSeconds(1));
            IObservable<IObservable<long>> seqWindowed = mainSequence.Window(() =>
            {
                //open a window of 6 second
                IObservable<long> seqWindowControl = Observable.Interval(TimeSpan.FromSeconds(6));
                return seqWindowControl;
            });

            seqWindowed.Subscribe(seqWindow =>
            {
                //window open
                System.Console.WriteLine("\nA new window into the main sequence has opened: {0}\n",
                                    DateTime.Now.ToString());
                //for 6 second stream data
                seqWindow.Subscribe(x => { System.Console.WriteLine("Integer : {0}", x); });
            });

            System.Console.ReadLine();
        }

        private static void DelayStream()
        {
            var oneNumberEveryFiveSeconds = Observable.Interval(TimeSpan.FromSeconds(5));

            // Instant echo
            oneNumberEveryFiveSeconds.Subscribe(num =>
            {
                System.Console.WriteLine(num);
            });

            // One second delay
            oneNumberEveryFiveSeconds.Delay(TimeSpan.FromSeconds(1)).Subscribe(num =>
            {
                System.Console.WriteLine("...{0}...", num);
            });

            // Two second delay
            oneNumberEveryFiveSeconds.Delay(TimeSpan.FromSeconds(2)).Subscribe(num =>
            {
                System.Console.WriteLine("......{0}......", num);
            });

            System.Console.ReadKey();
        }

        private static void BufferAStream()
        {
            var timeToStop = new ManualResetEvent(false);
            var keyPresses = KeyPresses().ToObservable();

            System.Console.WriteLine("Press Enter to stop.  Now bang that keyboard!");

            keyPresses.Buffer(TimeSpan.FromSeconds(10)).Subscribe(items =>
            {
                System.Console.WriteLine("");
                System.Console.WriteLine("In 10 seconds I recevided all these events: " + string.Join(" ", items.Select(c => c.Key)));
            });
            timeToStop.WaitOne();
        }

        private static void GroupEvents()
        {
            var timeToStop = new ManualResetEvent(false);
            var keyPresses = KeyPresses().ToObservable();

            var groupedKeyPresses =
                from k in keyPresses
                group k by k.Key into keyPressGroup
                select keyPressGroup;

            System.Console.WriteLine("Press Enter to stop.  Now bang that keyboard!");

            groupedKeyPresses.Subscribe(keyPressGroup =>
            {
                int numberPresses = 0;

                keyPressGroup.Subscribe(keyPress =>
                {
                    System.Console.WriteLine(
                        "You pressed the {0} key {1} time(s)!",
                        keyPress.Key,
                        ++numberPresses);
                },
                () => timeToStop.Set());
            });

            timeToStop.WaitOne();

        }

        static IEnumerable<ConsoleKeyInfo> KeyPresses()
        {
            for (; ; )
            {
                var currentKey = System.Console.ReadKey(true);

                if (currentKey.Key == ConsoleKey.Enter)
                    yield break;
                else
                    yield return currentKey;
            }
        }

        private static void RxTransformationFlow()
        {
            var oneNumberPerSecond = Observable.Interval(TimeSpan.FromSeconds(1));

            var stringsFromNumbers = from n in oneNumberPerSecond
                                     select new string('*', (int)n);

            System.Console.WriteLine("Strings from numbers:");

            stringsFromNumbers.Subscribe(num =>
            {
                System.Console.WriteLine(num);
            });

            System.Console.ReadKey();
        }

        private static void DrillDownTest()
        {
            var customers = new ObservableCollection<Customer>();

            var customerChanges = Observable.FromEventPattern(
                (EventHandler<NotifyCollectionChangedEventArgs> ev)
                   => new NotifyCollectionChangedEventHandler(ev),
                ev => customers.CollectionChanged += ev,
                ev => customers.CollectionChanged -= ev);

            var watchForNewCustomersFromWashington =
                from c in customerChanges
                where c.EventArgs.Action == NotifyCollectionChangedAction.Add
                from cus in c.EventArgs.NewItems.Cast<Customer>().ToObservable()
                where cus.Region == "WA"
                select cus;

            var watchFCustomerCollectionRemoveItem =
                    from c in customerChanges
                    where c.EventArgs.Action == NotifyCollectionChangedAction.Remove
                    from cus in c.EventArgs.OldItems.Cast<Customer>().ToObservable()
                    select cus;

            System.Console.WriteLine("New customers from Washington and their orders:");

            var a = watchForNewCustomersFromWashington.Subscribe(cus =>
            {
                System.Console.WriteLine("Customer {0}:", cus.CustomerName);

                foreach (var order in cus.Orders)
                {
                    System.Console.WriteLine("Order {0}: {1}", order.OrderId, order.OrderDate);
                }
            });

            watchFCustomerCollectionRemoveItem.Subscribe(cus => System.Console.WriteLine("this bloody Customer {0}: has been removed", cus.CustomerName));

            customers.Add(new Customer
            {
                CustomerName = "Lazy K Kountry Store",
                Region = "WA",
                Orders = { new Order { OrderDate = DateTimeOffset.Now, OrderId = 1 } }
            });

            Thread.Sleep(1000);
            customers.Add(new Customer
            {
                CustomerName = "Joe's Food Shop",
                Region = "NY",
                Orders = { new Order { OrderDate = DateTimeOffset.Now, OrderId = 2 } }
            });

            Thread.Sleep(1000);
            customers.Add(new Customer
            {
                CustomerName = "Trail's Head Gourmet Provisioners",
                Region = "WA",
                Orders = { new Order { OrderDate = DateTimeOffset.Now, OrderId = 3 } }
            });


            Thread.Sleep(1600);
            customers.RemoveAt(0);

            a.Dispose();
            System.Console.ReadKey();
        }

        private static int Value(int val)
        {
            throw new Exception("Test");
        }

        static IEnumerable<string> GetAllDirectories(string path)
        {
            string[] subdirs = null;

            // Some directories may be inaccessible.
            try
            {
                subdirs = Directory.GetDirectories(path);
            }
            catch (IOException)
            {
            }
            catch (Exception)
            {
            }

            if (subdirs != null)
            {
                foreach (var subdir in subdirs)
                {
                    yield return subdir;
                    
                    foreach (var grandchild in GetAllDirectories(subdir))
                    {
                        yield return grandchild;
                    }
                }
            }
        }
    }
}
