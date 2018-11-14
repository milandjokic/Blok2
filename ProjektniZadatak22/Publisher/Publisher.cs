using Contracts;
using Manager;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.Threading;

namespace Publisher
{
	public class Publisher : ChannelFactory<IPublisher>, IPublisher, IDisposable
	{
		public bool StopThread { get; set; } = false;
		IPublisher factory;

		public void Dispose()
		{
			if (factory != null)
			{
				factory = null;
			}

			Close();
		}

		public Publisher(NetTcpBinding binding, EndpointAddress address) : base(binding,address)
		{
			string cltCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);

			Credentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.Custom;
			Credentials.ServiceCertificate.Authentication.CustomCertificateValidator = new ClientCertValidator();
			Credentials.ServiceCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;
			Credentials.ClientCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, cltCertCN);

			factory = CreateChannel();
		}

		public bool RegisterPublisher(string subject)
		{
            try
            {
			    if(factory.RegisterPublisher(subject))
			    {
				    return true;
			    }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
			return false;			
		}
	   
		public bool UnregisterPublisher()
		{
            try
            {
                if (factory.UnregisterPublisher())
                {
                    return true;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return false;
		}
	  
		public void CreateAlarm(int period, X509Certificate2 signCert)
		{
			while(!StopThread)
			{
                try
                {
                    string[] messages = File.ReadAllLines(@"../../../Publisher/messages.txt");
				    Random randomInt = new Random();
				    Alarm alarm = new Alarm(DateTime.Now, messages[randomInt.Next(0, messages.Count())], randomInt.Next(1, 101));
				    byte[] signature = DigitalSignature.Create(alarm, "SHA1", signCert);
                
				    Publish(alarm, signature);
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }

				Thread.Sleep(period * 1000);
			}
		}

		public void Publish(Alarm alarm, byte[] signature)
		{
            try
            {
			    factory.Publish(alarm, signature);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
		}     
	}
}
