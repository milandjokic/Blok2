using Contracts;
using Manager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Subscriber
{
    public class Subscriber : DuplexChannelFactory<ISubscriber>, ISubscriber, IDisposable
    {
        ISubscriber factory;

        public Subscriber(NetTcpBinding binding, EndpointAddress address, MyServiceCallback callback) : base(callback, binding, address)
        {
            string cltCertCN = Manager.Formatter.ParseName(WindowsIdentity.GetCurrent().Name);

            this.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.Custom;
            this.Credentials.ServiceCertificate.Authentication.CustomCertificateValidator = new ClientCertValidator();
            this.Credentials.ServiceCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

            this.Credentials.ClientCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, cltCertCN);

            factory = this.CreateChannel();
        }

        public bool Subscribe(string subject)
        {
            if(factory.Subscribe(subject))
            {
                Console.WriteLine("Successfully subcribed to topic [" + subject + "]");
                return true;
            }
            return false;
        }

        public bool Unsubsrcibe(string subject)
        {
            if(factory.Unsubsrcibe(subject))
            {
                Console.WriteLine("Successfully unsubcribed from topic [" + subject + "]");
                return true;
            }
            Console.WriteLine("Topic doesn't exist");
            return false;
        }

        public void Dispose()
        {
            if (factory != null)
            {
                factory = null;
            }

            this.Close();
        }

        public bool RegisterSubscriber()
        {
            return factory.RegisterSubscriber();
        }

        public bool UnregisterSubscriber()
        {
            return factory.UnregisterSubscriber();
        }

        
    }
}
