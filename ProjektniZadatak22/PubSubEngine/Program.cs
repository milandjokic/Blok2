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
using System.Security.Principal;

namespace PubSubEngine
{
    public class Program
    {
        static void Main(string[] args)
        {
            string srvCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);
            Console.WriteLine(srvCertCN);
            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

            string address = "net.tcp://localhost:9999/PubSubEngine";

            ServiceHost host = new ServiceHost(typeof(PubSubEngine));
            host.AddServiceEndpoint(typeof(IPubSubEngine), binding, address);

            host.Credentials.ClientCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.Custom;
            host.Credentials.ClientCertificate.Authentication.CustomCertificateValidator = new ServerCertValidator();
            host.Credentials.ClientCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;
            
            host.Credentials.ServiceCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, srvCertCN);
            //Manager.ServerCertValidator.v
            //host.Credentials.ServiceCertificate.Certificate = CertManager.GetCertificateFromFile("PubSubEngine.pfx");

            //Console.WriteLine(host.Credentials.ClientCertificate.Certificate.PublicKey.ToString());

            try
            {
                host.Open();
                Console.WriteLine("PubSubEngine is open. Press <enter> to finish...");
                Console.ReadLine();
            }
            catch(Exception e)
            {
                Console.WriteLine("[ERROR] {0}", e.Message);
                //Console.WriteLine("[StackTrace] {0}", e.StackTrace);

            }
            finally
            {
            }
            //host.Close();

            Console.ReadLine();
        }
    }
}
