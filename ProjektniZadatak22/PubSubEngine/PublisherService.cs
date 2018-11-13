using Contracts;
using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace PubSubEngine
{
    public class PublisherService : IPublisher
    {
        public bool RegisterPublisher(string subject)
        {
            string[] tokens = OperationContext.Current.SessionId.Split('=');
            int id = Int32.Parse(tokens[1]);

            if (Database.GetInstance().Publishers.ContainsKey(id))
            {
                Console.WriteLine("This publisher is already registered.");
                return false;
            }
            else
            {
                Database.GetInstance().Publishers.Add(id, new Topic(subject));
                Console.WriteLine("Publisher registered.");
                //Callback = OperationContext.Current.GetCallbackChannel<IMyServiceCallBack>();

                //if (Database.GetInstance().Subscribers.Count != 0)
                    //ListAllTopics();

                return true;
            }
        }

        public void Publish(Alarm alarm, byte[] signature)
        {
            string[] tokens = OperationContext.Current.SessionId.Split('=');
            int id = Int32.Parse(tokens[1]);

            X509Certificate2 clientCert = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, "SignP");
            if (DigitalSignature.Verify(alarm, "SHA1", signature, clientCert))
                Console.WriteLine("Digital signature is valid.");
            else
                Console.WriteLine("Digital signature is invalid");

            Database.GetInstance().Publishers[id].Alarms.Add(alarm);

            Console.WriteLine("Primljen alarm.");

            if (Database.GetInstance().Subscribers.Count != 0)
            {
                //SubscriberService.Callback.OnCallBack();
                foreach(KeyValuePair<int, IMyServiceCallBack> keyValuePair in Database.GetInstance().Callbacks)
                {
                    keyValuePair.Value.OnCallBack();
                }

            }
        }

        public bool UnregisterPublisher()
        {
            string[] tokens = OperationContext.Current.SessionId.Split('=');
            int id = Int32.Parse(tokens[1]);

            if (Database.GetInstance().Publishers.ContainsKey(id))
            {
                Database.GetInstance().Publishers.Remove(id);
                Console.WriteLine("This publisher is unregistered.");
                return true;
            }
            else
            {
                Console.WriteLine("Publisher does not exist.");
                return false;
            }
        }
    }
}
