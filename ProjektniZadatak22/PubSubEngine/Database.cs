using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace PubSubEngine
{
    public class Database
    {
        private Dictionary<int, Topic> publishers;
        private Dictionary<int, List<Topic>> subscribers;
        private Dictionary<int, EndpointAddress> subscribersIps;

        public Dictionary<int, EndpointAddress> SubscribersIps
        {
            get { return subscribersIps; }
            set { subscribersIps = value; }
        }


        private Database()
        {
            Publishers = new Dictionary<int, Topic>();
            Subscribers = new Dictionary<int, List<Topic>>();
            SubscribersIps = new Dictionary<int, EndpointAddress>();
        }

        private static Database instance;

        public Dictionary<int, Topic> Publishers
        {
            get
            {
                return publishers;
            }
            set
            {
                publishers = value;
            }
        }
        public Dictionary<int, List<Topic>> Subscribers
        {
            get
            {
                return subscribers;
            }
            set
            {
                subscribers = value;
            }
        }

        public static Database GetInstance()
        {
            if(instance == null)
            {
                instance = new Database();
            }

            return instance;
        }
    }
}
