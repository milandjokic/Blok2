using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Security.Cryptography.X509Certificates;
using Manager;

namespace Publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            string srvCertCN = "PubSubEngine";
            string signCertCN = "SignP";

            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

            X509Certificate2 srvCert = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, srvCertCN);
            EndpointAddress address = new EndpointAddress(new Uri("net.tcp://localhost:9999/PubSubEngine"), new X509CertificateEndpointIdentity(srvCert));

            using (Publisher proxy = new Publisher(binding, address))
            {
                Console.WriteLine("Publisher is connected");

                X509Certificate2 signCert = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, signCertCN);

                byte[] signature = DigitalSignature.Create(message, "SHA1", signCert);

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
}
