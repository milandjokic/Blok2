using Contracts;
using Manager;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Security;

namespace PubSubEngine
{
	public class Program
	{
		static void Main(string[] args)
		{
			string srvCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);
			
			NetTcpBinding binding = new NetTcpBinding();
			binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

			string address = "net.tcp://localhost:9999/Publisher";

			ServiceHost hostPublisher = new ServiceHost(typeof(PublisherService));
			hostPublisher.AddServiceEndpoint(typeof(IPublisher), binding, address);

			hostPublisher.Credentials.ClientCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.Custom;
			hostPublisher.Credentials.ClientCertificate.Authentication.CustomCertificateValidator = new ServerCertValidator();
			hostPublisher.Credentials.ClientCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;
            hostPublisher.Credentials.ServiceCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, srvCertCN);
           

			address = "net.tcp://localhost:8888/Subscriber";

			ServiceHost hostSubscriber = new ServiceHost(typeof(SubscriberService));
			hostSubscriber.AddServiceEndpoint(typeof(ISubscriber), binding, address);

			hostSubscriber.Credentials.ClientCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.Custom;
			hostSubscriber.Credentials.ClientCertificate.Authentication.CustomCertificateValidator = new ServerCertValidator();
			hostSubscriber.Credentials.ClientCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;
            hostSubscriber.Credentials.ServiceCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, srvCertCN);
                  
            try
			{
				hostPublisher.Open();
				hostSubscriber.Open();
				Console.WriteLine("PubSubEngine is open. Press <enter> to finish...");
				Console.ReadLine();
			}
			catch(Exception e)
			{
				Console.WriteLine("[ERROR] {0}", e.Message);
			}
			finally
			{
				hostPublisher.Close();
				hostSubscriber.Close();
			}

		}
	}
}
