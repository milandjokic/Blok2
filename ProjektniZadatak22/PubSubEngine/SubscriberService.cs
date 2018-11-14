using Contracts;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace PubSubEngine
{
	[ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant)]
	public class SubscriberService : ISubscriber
	{
		public bool RegisterSubscriber()
		{
			string[] tokens = OperationContext.Current.SessionId.Split('=');
			int id = int.Parse(tokens[1]);

			if (Database.GetInstance().Subscribers.ContainsKey(id))
			{
				return false;
			}
			else
			{
				Database.GetInstance().Subscribers.Add(id, new List<Topic>());

				IMyServiceCallBack Callback = OperationContext.Current.GetCallbackChannel<IMyServiceCallBack>();
				Database.GetInstance().Callbacks.Add(id, Callback);

				SendTopics();

				return true;
			}
		}

		public bool UnregisterSubscriber()
		{
			string[] tokens = OperationContext.Current.SessionId.Split('=');
			int id = int.Parse(tokens[1]);

			if (Database.GetInstance().Subscribers.ContainsKey(id))
			{
				Database.GetInstance().Subscribers.Remove(id);
				Database.GetInstance().Callbacks.Remove(id);
				return true;
			}
			else
			{
				return false;
			}
		}

		public bool Subscribe(string subject, int from, int to)
		{
			string[] tokens = OperationContext.Current.SessionId.Split('=');
			int id = int.Parse(tokens[1]);
			
			foreach(Topic topic in Database.GetInstance().Subscribers[id])
			{
				if(topic.Subject == subject)
				{
					return false;
				}
			}

			bool exist = false;
			foreach(KeyValuePair<int, Topic> publisher in Database.GetInstance().Publishers)
			{
				if(publisher.Value.Subject == subject)
				{
					exist = true;
					break;
				}
			}

			if (exist == false)
			{
				return false;
			}
			else
			{
				Database.GetInstance().Subscribers[id].Add(new Topic(subject, from, to));
				return true;
			}
		}

		public bool Unsubsrcibe(string subject)
		{
			string[] tokens = OperationContext.Current.SessionId.Split('=');
			int id = int.Parse(tokens[1]);

			foreach(Topic topic in Database.GetInstance().Subscribers[id])
			{
				if(topic.Subject == subject)
				{
					Database.GetInstance().Subscribers[id].Remove(topic);
					return true;
				}
			}

			return false;
		}

		public void SendTopics()
		{
			string[] tokens = OperationContext.Current.SessionId.Split('=');
			int id = int.Parse(tokens[1]);

			string topics = "";

			foreach (Topic topic in Database.GetInstance().Publishers.Values)
			{
				topics += topic.Subject;
				topics += "\n";
			}

			Database.GetInstance().Callbacks[id].ReceiveTopics(topics);
		}
	}
}
