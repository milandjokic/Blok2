using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:9999/Publisher";

            Publisher proxy = new Publisher(binding, new EndpointAddress(new Uri(address)));

            Console.WriteLine("Publisher is connected");

            proxy.RegisterPublisher("Topic 1");

            Console.Write("Enter timeout (in seconds) between publishes:");
            int period = Int32.Parse(Console.ReadLine());

            Thread threadCreateAlarm = new Thread(() => proxy.CreateAlarm(period));
            threadCreateAlarm.Start();

            Console.ReadLine();

            proxy.StopThread = true;
            threadCreateAlarm.Join();

            proxy.UnregisterPublisher();

        }
    }
}
