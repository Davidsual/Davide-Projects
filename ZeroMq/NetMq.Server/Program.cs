using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NetMQ;
using Newtonsoft.Json;

namespace NetMq.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            /* //REQUEST / RESPONSE
            using (NetMQContext context = NetMQContext.Create())
            {
                Task serverTask = Task.Factory.StartNew(() => ServerResponseSocket(context));
                serverTask.Wait();
                Console.ReadLine();
            }
            */
            /* // PUBLISH / SUBSCRIBER
            using (NetMQContext context = NetMQContext.Create())
            {
                Task serverTask = Task.Factory.StartNew(() => ServerPublisherSocket(context, 5000));
                Task serverTaskTwo = Task.Factory.StartNew(() => ServerPublisherSocket(context, 5001));
                Task.WaitAll(serverTask, serverTaskTwo);
                Console.ReadLine();
            }
            */
            // PUSH / PULL
            using (NetMQContext context = NetMQContext.Create())
            {
                Task serverTask = Task.Factory.StartNew(() => ServerPushSocket(context));
                serverTask.Wait();
                Console.ReadLine();
            }
        }

        static void ServerResponseSocket(NetMQContext context)
        {
            using (NetMQSocket serverSocket = context.CreateResponseSocket())
            {
                serverSocket.Bind("tcp://*:5555");

                while (true)
                {
                    string message = serverSocket.ReceiveString();

                    Console.WriteLine("Receive message {0}", message);

                    string messageReply = Console.ReadLine();
                    serverSocket.Send(messageReply);
                    

                    if (message == "exit")
                    {
                        break;
                    }
                }
            }
        }
        static void ServerPublisherSocket(NetMQContext context, int port)
        {
            using (NetMQSocket serverSocket = context.CreatePublisherSocket())
            {
                serverSocket.Bind(string.Format("tcp://*:{0}", port));

                EntityDto test;
                while (true)
                {
                    string message = string.Format("{0} I am the message from publisher from port: {1}", DateTime.Now.ToString("hh.mm.ss.ffffff"), port);
                    
                    test = new EntityDto()
                    {
                        Age = new Random().Next(1,99),
                        IsActive = true,
                        Name = "Davide",
                        Port = port,
                        RelatedEntity = new EntityDto()
                    };
                    
                    serverSocket.Send(JsonConvert.SerializeObject(test), Encoding.UTF8);
                    Console.WriteLine("PUBLISHER Message send from server on port: " + port);

                    //Thread.Sleep(new Random().Next(100,2000));
                    if(port == 5000)
                        Thread.Sleep(2000);
                    else
                        Thread.Sleep(500);
 
                }
            }
        }

        static void ServerPushSocket(NetMQContext context)
        {
            using (NetMQSocket serverSocket = context.CreatePushSocket())
            {
                serverSocket.Bind("tcp://*:5555");
                int val = 0;
                while (true)
                {
                    serverSocket.Send(string.Format("{0} custom message n: {1}", DateTime.Now.ToString("hh.mm.ss.ffffff"), val++));
                    Console.WriteLine("Pushed message from the server");
                    //Thread.Sleep(100);
                }
            }
        }
    }
}
