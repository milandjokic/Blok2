using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Subscriber
{
    class Program
    {
        static void Main(string[] args)
        {
            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:8888/Subscriber";

            ServiceHost host = new ServiceHost(typeof(SubscriberHost));
            host.AddServiceEndpoint(typeof(ISubscriber), binding, address);

            host.Open();

            address = "net.tcp://localhost:9999/PubSubEngine";
            Subscriber proxy = new Subscriber(binding, new EndpointAddress(new Uri(address)));

            proxy.RegisterSubscriber();

            Console.ReadLine();

            host.Close();
        }
    }
}
