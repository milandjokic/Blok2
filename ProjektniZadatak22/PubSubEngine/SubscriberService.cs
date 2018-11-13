using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace PubSubEngine
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public class SubscriberService : ISubscriber
    {
        public static IMyServiceCallBack Callback;

        public bool Subscribe(string subject)
        {
            bool exist = false;
            foreach(KeyValuePair<int, Topic> keyValuePair in Database.GetInstance().Publishers)
            {
                if (subject == keyValuePair.Value.Subject)
                {
                    
                    foreach(var v in Database.GetInstance().Subscribers)
                    {
                        if(v.Value.Contains(keyValuePair.Value))
                        {
                            Console.WriteLine("Already subscribed to [" + subject + "]");
                            return false;
                        }
                    }
                    exist = true;
                }
            }

            if (exist == false)
            {
                Console.WriteLine("Topic doesn't exist.");
                return false;
            }
            return true;
        }


        //Unsubscribe ne radi bas najbolje, zapravo ni ne radi!
        public bool Unsubsrcibe(string subject)
        {
            string[] tokens = OperationContext.Current.SessionId.Split('=');
            int id = Int32.Parse(tokens[1]);

                foreach(var v in Database.GetInstance().Subscribers[id])
                {
                    if(v.Subject == subject)
                    {
                        Console.WriteLine("Unsubscribed from [" + subject + "]");
                        Database.GetInstance().Subscribers[id].Remove(v);
                        return true;
                    }
                }

            Console.WriteLine("Topic doesn't exist");
            return false;
        }


        public bool RegisterSubscriber()
        {
            string[] tokens = OperationContext.Current.SessionId.Split('=');
            int id = Int32.Parse(tokens[1]);

            if (Database.GetInstance().Subscribers.ContainsKey(id))
            {
                Console.WriteLine("This subscriber is already registered.");
                return false;
            }
            else
            {
                Database.GetInstance().Subscribers.Add(id, new List<Topic>());
                Callback = OperationContext.Current.GetCallbackChannel<IMyServiceCallBack>();
                Database.GetInstance().Callbacks.Add(id, Callback);
                Console.WriteLine("Subscriber registered.");

                SaveSubscriberIp(id, OperationContext.Current);
                //ListAllTopics();

                return true;
            }
        }

        public bool UnregisterSubscriber()
        {
            string[] tokens = OperationContext.Current.SessionId.Split('=');
            int id = Int32.Parse(tokens[1]);

            if (Database.GetInstance().Subscribers.ContainsKey(id))
            {
                Database.GetInstance().Subscribers.Remove(id);
                Database.GetInstance().Callbacks.Remove(id);
                Console.WriteLine("This subcriber is unregistered.");
                return true;
            }
            else
            {
                Console.WriteLine("Subscriber does not exist.");
                return false;
            }
        }

        public void SaveSubscriberIp(int id, OperationContext context)
        {
            MessageProperties prop = context.IncomingMessageProperties;
            RemoteEndpointMessageProperty endpoint =
               prop[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
            string ip = "net.tcp://" + endpoint.Address + ":8888/Subscriber";

            Database.GetInstance().SubscribersIps.Add(id, new EndpointAddress(new Uri(ip)));
        }
    }
}
