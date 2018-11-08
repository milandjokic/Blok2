using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel.Security;
using System.Security.Cryptography.X509Certificates;
using Manager;

namespace PubSubEngine
{
    public class Program
    {
        static void Main(string[] args)
        {
            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

            string address = "net.tcp://localhost:9999/PubSubEngine";

            ServiceHost host = new ServiceHost(typeof(PubSubEngine));
            host.AddServiceEndpoint(typeof(IPubSubEngine), binding, address);

            host.Credentials.ClientCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.Custom;
            host.Credentials.ClientCertificate.Authentication.CustomCertificateValidator = new ServiceCertValidator();

            host.Open();
        
            Console.WriteLine("PubSubEngine is open. Press <enter> to finish...");
            Console.ReadLine();

            host.Close();

        }
    }
}
