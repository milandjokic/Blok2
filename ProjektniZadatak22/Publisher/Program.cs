using Manager;
using System;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Threading;


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
			EndpointAddress address = new EndpointAddress(new Uri("net.tcp://localhost:9999/Publisher"), new X509CertificateEndpointIdentity(srvCert));

			using (Publisher proxy = new Publisher(binding, address))
			{
				Console.WriteLine("Publisher is connected");

				X509Certificate2 signCert = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, signCertCN);

                string topic;
                Console.Write("Enter topic to publish: ");
                topic = Console.ReadLine();
				proxy.RegisterPublisher(topic);

				Console.Write("Enter timeout (in seconds) between publishes: ");
				int period = int.Parse(Console.ReadLine());

				Thread threadCreateAlarm = new Thread(() => proxy.CreateAlarm(period, signCert));
				threadCreateAlarm.Start();

				Console.ReadLine();

				proxy.StopThread = true;
				threadCreateAlarm.Join();

				proxy.UnregisterPublisher();
			}
		}
	}
}
