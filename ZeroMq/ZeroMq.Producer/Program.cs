using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ZeroMq;
using ZMQ;

namespace ZeroMq.Producer
{
    class Program
    {
        static void Main(string[] args)
        {

            bool send = true;
            var worker = new Thread(new ThreadStart(() =>
            {
                using (var context = new Context())
                {
                    var socket = context.Socket(SocketType.REQ);
                    socket.Connect("tcp://127.0.0.1:2346");
                    int i = 0;
                    while (send)
                    {
                        string msg = "Hallo:" + i++;
                        Console.WriteLine("Sending: " + msg);
                        socket.Send(msg, Encoding.UTF8);
                        var s = socket.Recv(Encoding.UTF8);
                        Console.WriteLine("Server reply:" + s);
                    }
                    socket.Dispose();
                }
            }));
            worker.Start();
            Console.WriteLine("Press enter to stop.");
            Console.ReadLine();
            send = false;
            worker.Join();
        }
    }
}
