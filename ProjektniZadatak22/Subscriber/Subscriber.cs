using Contracts;
using Manager;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;

namespace Subscriber
{
	public class Subscriber : DuplexChannelFactory<ISubscriber>, ISubscriber, IDisposable
	{
		ISubscriber factory;

		public Subscriber(NetTcpBinding binding, EndpointAddress address, MyServiceCallback callback) : base(callback, binding, address)
		{
			string cltCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);

			Credentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.Custom;
			Credentials.ServiceCertificate.Authentication.CustomCertificateValidator = new ClientCertValidator();
			Credentials.ServiceCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;
			Credentials.ClientCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, cltCertCN);

			factory = CreateChannel();
		}

		public void Dispose()
		{
			if (factory != null)
			{
				factory = null;
			}

			Close();
		}

		public bool Subscribe(string subject, int from, int to)
		{

            try
            {
			    if(factory.Subscribe(subject, from, to))
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

		public bool Unsubsrcibe(string subject)
		{
            try
            {
			    if(factory.Unsubsrcibe(subject))
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

		public bool RegisterSubscriber()
		{
            try
            {
			    return factory.RegisterSubscriber();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
		}

		public bool UnregisterSubscriber()
		{
            try
            {
			    return factory.UnregisterSubscriber();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
		}
	}
}
