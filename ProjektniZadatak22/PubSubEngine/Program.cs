using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace PubSubEngine
{
    public class Program
    {
        static void Main(string[] args)
        {
            NetTcpBinding binding = new NetTcpBinding();

            string address = "net.tcp://localhost:9999/PubSubEngine";

            ServiceHost host = new ServiceHost(typeof(PubSubEngine));
            host.AddServiceEndpoint(typeof(IPubSubEngine), binding, address);

            host.Open();
        
            Console.WriteLine("PubSubEngine is open. Press <enter> to finish...");
            Console.ReadLine();

            host.Close();

        }
    }
}
