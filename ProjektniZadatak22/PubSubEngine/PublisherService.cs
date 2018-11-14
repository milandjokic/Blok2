using Contracts;
using Manager;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;

namespace PubSubEngine
{
	public class PublisherService : IPublisher
	{
		public bool RegisterPublisher(string subject)
		{
			string[] tokens = OperationContext.Current.SessionId.Split('=');
			int id = int.Parse(tokens[1]);

			if (Database.GetInstance().Publishers.ContainsKey(id))
			{
				return false;
			}
			else
			{
				Database.GetInstance().Publishers.Add(id, new Topic(subject));

				SendTopics();

				return true;
			}
		}

		public void Publish(Alarm alarm, byte[] signature)
		{
			string[] tokens = OperationContext.Current.SessionId.Split('=');
			int id = int.Parse(tokens[1]);

			//Database.GetInstance().Publishers[id].Alarms.Add(alarm);

			if (Database.GetInstance().Subscribers.Count != 0)
			{				
                foreach(var v in Database.GetInstance().Subscribers)
                {
                    foreach(var v2 in v.Value)
                    {
                        if(v2.Subject == Database.GetInstance().Publishers[id].Subject && v2.From <= alarm.Risk && v2.To >= alarm.Risk)
                        {
                            Database.GetInstance().Callbacks[v.Key].ReceiveAlarm(alarm, signature, v.Key);
                        }
                    }                  
                }
			}
		}

		public bool UnregisterPublisher()
		{
			string[] tokens = OperationContext.Current.SessionId.Split('=');
			int id = int.Parse(tokens[1]);

			if (Database.GetInstance().Publishers.ContainsKey(id))
			{
				Database.GetInstance().Publishers.Remove(id);
				return true;
			}
			else
			{
				return false;
			}
		}

		public void SendTopics()
		{
			string topics = "";

			foreach(Topic topic in Database.GetInstance().Publishers.Values)
			{
				topics += topic.Subject;
				topics += "\n";
			}

			foreach(IMyServiceCallBack callback in Database.GetInstance().Callbacks.Values)
			{
				callback.ReceiveTopics(topics);
			}
		}
	}
}
