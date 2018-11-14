using Contracts;
using System.Collections.Generic;

namespace PubSubEngine
{
	public class Database
	{
		public Dictionary<int, Topic> Publishers { get; set; }
		public Dictionary<int, List<Topic>> Subscribers { get; set; }
		public Dictionary<int, IMyServiceCallBack> Callbacks { get; set; }

		private static readonly Database instance = new Database();
		
		private Database()
		{
			Publishers = new Dictionary<int, Topic>();
			Subscribers = new Dictionary<int, List<Topic>>();
			Callbacks = new Dictionary<int, IMyServiceCallBack>();
		}

		public static Database GetInstance()
		{
			return instance;
		}
	}
}
