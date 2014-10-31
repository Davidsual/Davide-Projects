using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading;
using ZMQ;

namespace ZeroMq.Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            bool run = true;
            HashSet<int> enqueueConcurrents = new HashSet<int>();
            var worker = new Thread(new ThreadStart(() =>
            {
                using (var context = new Context())
                {
                    var socket = context.Socket(SocketType.REP);
                    socket.Bind("tcp://127.0.0.1:2346");
                    int i = 0;
                    while (run)
                    {
                        var clientMsg = socket.Recv(Encoding.UTF8);
                        Console.WriteLine("Received from client:" + clientMsg);
                        string msg = "Hallo:" + i++;
                        if (enqueueConcurrents.Contains(i))
                        {
                            throw new System.Exception("This must never happen since messages are served one by one");
                        }
                        enqueueConcurrents.Add(i);
                        Console.WriteLine("Sending: " + msg);
                        Thread.Sleep(200);
                        socket.Send(msg, Encoding.UTF8);

                    }
                    socket.Dispose();
                }
            }));
            worker.Start();
            Console.WriteLine("Press enter to stop.");
            Console.ReadLine();
            run = false;
            worker.Join();
        }
    }
}
