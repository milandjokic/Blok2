using Contracts;
using Manager;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Publisher
{
    public class Publisher : ChannelFactory<IPublisher>, IPublisher, IDisposable
    {
        bool stopThread = false;


        IPublisher factory;

        public bool StopThread { get => stopThread; set => stopThread = value; }

        public Publisher(NetTcpBinding binding, EndpointAddress address) : base(binding,address)
        {
            string cltCertCN = Manager.Formatter.ParseName(WindowsIdentity.GetCurrent().Name);

            this.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.Custom;
            this.Credentials.ServiceCertificate.Authentication.CustomCertificateValidator = new ClientCertValidator();
            this.Credentials.ServiceCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

            this.Credentials.ClientCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, cltCertCN);

            factory = this.CreateChannel();
        }
        
        


        public bool RegisterPublisher(string subject)
        {
            if(factory.RegisterPublisher(subject))
            {
                Console.WriteLine("Publisher successfully registered.");
                return true;
            }
            else
            {
                Console.WriteLine("Publisher already registered.");
                return false;
            }
        }

       
        public bool UnregisterPublisher()
        {
            if (factory.UnregisterPublisher())
            {
                Console.WriteLine("Publisher successfully unregistered.");
                return true;
            }
            else
            {
                Console.WriteLine("Publisher does not exist.");
                return false;
            }
        }

      
        public void Dispose()
        {
            if(factory!=null)
            {
                factory = null;
            }

            this.Close();
        }

        public void CreateAlarm(int period, X509Certificate2 signCert)
        {
            while(!StopThread)
            {
                string[] messages = File.ReadAllLines(@"../../../Publisher/messages.txt");
                Random randomInt = new Random();
                Alarm alarm = new Alarm(DateTime.Now, messages[randomInt.Next(0, messages.Count())], randomInt.Next(1, 101));
                byte[] signature = DigitalSignature.Create(alarm, "SHA1", signCert);

                Console.WriteLine("Napravljen alarm");
                Publish(alarm,signature);

                Thread.Sleep(period * 1000);
            }
        }

        public void Publish(Alarm alarm, byte[] signature)
        {
            factory.Publish(alarm,signature);
        }     
    }
}
