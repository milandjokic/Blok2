using Contracts;
using Manager;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Subscriber
{
    class Program
    {
        static void Main(string[] args)
        {
            string srvCertCN = "PubSubEngine";

            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

            X509Certificate2 srvCert = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, srvCertCN);

            //string address = "net.tcp://localhost:8888/Subscriber";

            EndpointAddress address = new EndpointAddress(new Uri("net.tcp://localhost:8888/Subscriber"), new X509CertificateEndpointIdentity(srvCert));

            var subscriberCallback = new MyServiceCallback();
            var instanceContext = new InstanceContext(subscriberCallback);
            
            using (Subscriber proxy = new Subscriber(binding, address, subscriberCallback))
            {
                Console.WriteLine("Subscriber is connected");
               
                
                proxy.RegisterSubscriber();
                Console.Write("Enter topic to subscribe : ");
                string topic = Console.ReadLine();
                proxy.Subscribe(topic);
                proxy.Unsubsrcibe("Topic 1");
                proxy.Unsubsrcibe("Topic 8");
                proxy.UnregisterSubscriber();
                Console.ReadLine();

         
            }



        }
    }
}
