using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using NetMQ;
using Newtonsoft.Json;

namespace NetMq.Client
{
    class Program
    {
        static void Main(string[] args)
        {

            /* // REQUEST / RESPONSE
            using (NetMQContext context = NetMQContext.Create())
            {
                Task clientTask = Task.Factory.StartNew(() => ClientRequestSocket(context));
                clientTask.Wait();
                Console.ReadLine();
            }
            */

            /*//PUBLISH / SUBSCRIBER
            using (NetMQContext context = NetMQContext.Create())
            {
                Task clientTask = Task.Factory.StartNew(() => ClientSubscriberSocket(context));
                clientTask.Wait();
                Console.ReadLine();
            }
            */
            //PUSH / PULL
            using (NetMQContext context = NetMQContext.Create()) 
            {
                Task clientTask = Task.Factory.StartNew(() => ClientPullSocket(context));
                clientTask.Wait();
                Console.ReadLine();
            }
        }

        static void ClientRequestSocket(NetMQContext context)
        {
            using (NetMQSocket clientSocket = context.CreateRequestSocket())
            {
                clientSocket.Connect("tcp://127.0.0.1:5555");

                while (true)
                {
                    Console.WriteLine("Please enter your message:");
                    string message = Console.ReadLine();
                    clientSocket.Send(message);

                    string answer = clientSocket.ReceiveString();

                    Console.WriteLine("Answer from server: {0}", answer);

                    if (message == "exit")
                    {
                        break;
                    }
                }
            }
        }

        static void ClientSubscriberSocket(NetMQContext context)
        {
            using (NetMQSocket socket = context.CreateSubscriberSocket())
            {
                socket.Connect("tcp://127.0.0.1:5000");
                socket.Connect("tcp://127.0.0.1:5001");
                socket.Subscribe("");

                while (true)
                {
                    var obj = JsonConvert.DeserializeObject<EntityDto>(socket.ReceiveString());
                    Console.WriteLine("SUBSCRIBER AGE {0} from publisher port: {1}",obj.Age,obj.Port);
                }
            }
        }

        static void ClientPullSocket(NetMQContext context)
        {
            using (NetMQSocket clientSocket = context.CreatePullSocket())
            {
                clientSocket.Connect("tcp://127.0.0.1:5555");
                

                while (true)
                {                    
                    Console.WriteLine("pulled from server: {0}", clientSocket.ReceiveString());
                }
            }
        }
    }
}

