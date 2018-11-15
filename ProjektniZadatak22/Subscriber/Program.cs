using Contracts;
using Manager;

using System;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;

namespace Subscriber
{
    class Program
    {
        static void Main(string[] args)
        {
            string srvCertCN = "PubSubEngine";

            MyAuditBehavior myAuditBehavior = new MyAuditBehavior();

            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

            X509Certificate2 srvCert = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, srvCertCN);

            EndpointAddress address = new EndpointAddress(new Uri("net.tcp://localhost:8888/Subscriber"), new X509CertificateEndpointIdentity(srvCert));

            MyServiceCallback subscriberCallback = new MyServiceCallback();
            InstanceContext instanceContext = new InstanceContext(subscriberCallback);

            string subject,from, to;

            using (Subscriber proxy = new Subscriber(binding, address, subscriberCallback))
            {
                Console.WriteLine("Connected to the PubSubEngine\n");

                while(true)
                {
                    proxy.RegisterSubscriber();
                    PrintMenu();
                    int choice = int.Parse(Console.ReadLine());

                    switch (choice)
                    {
                        case 1:
                            if (proxy.UnregisterSubscriber())
                                Console.WriteLine("Successfully unregistered.");
                            else
                                Console.WriteLine("Failed to unregister.");
                            break;
                        case 2:
                            Console.Write("Enter topic name: ");
                            subject = Console.ReadLine();
                            Console.Write("Enter risk range: \nfrom: ");
                            from = Console.ReadLine();
                            Console.Write("to: ");
                            to = Console.ReadLine();
                            if (proxy.Subscribe(subject,Int32.Parse(from),Int32.Parse(to)))
                                Console.WriteLine("Successfully subscribed.");
                            else
                                Console.WriteLine("Unable to subscribe");
                            break;
                        case 3:
                            Console.Write("Enter topic name: ");
                            if (proxy.Unsubsrcibe(Console.ReadLine()))
                                Console.WriteLine("Successfully unsubscribed.");
                            else
                                Console.WriteLine("Unable to unsubscribe.");
                            break;
                        default:
                            Console.WriteLine("Bad input, try again.");
                            break;
                    }

                    if (choice == 1)
                        break;
                }
            }
        }

        public static void PrintMenu()
        {
            Console.WriteLine("*****************Menu*****************");
            Console.WriteLine("1. Unregister(and exit)\n2. Subscribe\n3. Unsubscribe");
            Console.Write("Choose option: ");
        }
    }

}