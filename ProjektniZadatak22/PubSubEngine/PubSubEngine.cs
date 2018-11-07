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
    public class PubSubEngine : IPubSubEngine
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

                if(Database.GetInstance().Subscribers.Count != 0)
                    ListAllTopics();

                return true;
            }
        }

        public bool Subscribe(string subject)
        {
            throw new NotImplementedException();
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

        public bool Unsubsrcibe(string subject)
        {
            throw new NotImplementedException();
        }

        public void Publish(Alarm alarm)
        {
            string[] tokens = OperationContext.Current.SessionId.Split('=');
            int id = Int32.Parse(tokens[1]);

            Database.GetInstance().Publishers[id].Alarms.Add(alarm);

            Console.WriteLine("Primljen alarm.");
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
                Console.WriteLine("Subscriber registered.");

                SaveSubscriberIp(id, OperationContext.Current);
                ListAllTopics();

                return true;
            }
        }

        public bool UnregisterSubscriber()
        {
            throw new NotImplementedException();
        }

        public void SaveSubscriberIp(int id, OperationContext context)
        {
            MessageProperties prop = context.IncomingMessageProperties;
            RemoteEndpointMessageProperty endpoint =
               prop[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
            string ip = "net.tcp://" + endpoint.Address + ":8888/Subscriber";

            Database.GetInstance().SubscribersIps.Add(id, new EndpointAddress(new Uri(ip)));
        }

        public void ListAllTopics()
        {
            List<string> subjects = new List<string>();
            foreach (KeyValuePair<int, Topic> element in Database.GetInstance().Publishers)
            {
                subjects.Add(element.Value.Subject);
            }

            foreach (KeyValuePair<int, EndpointAddress> element in Database.GetInstance().SubscribersIps)
            {
                ChannelFactory<ISubscriber> factory = new ChannelFactory<ISubscriber>(new NetTcpBinding(), element.Value);
                ISubscriber proxy = factory.CreateChannel();

                proxy.ListAllTopics(subjects);

                proxy = null;
            }
        }
    }
}
