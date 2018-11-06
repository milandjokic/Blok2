using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
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
                return true;
            }
        }

        public void Subscribe(Alarm alarm)
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

        public void Unsubsrcibe(Alarm alarm)
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
    }
}
