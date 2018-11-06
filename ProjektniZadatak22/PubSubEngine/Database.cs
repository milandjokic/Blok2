using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubSubEngine
{
    public class Database
    {
        private Dictionary<int, Topic> publishers;
        private Dictionary<int, Topic> subscribers;

        private Database()
        {
            Publishers = new Dictionary<int, Topic>();
            Subscribers = new Dictionary<int, Topic>();
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
        public Dictionary<int, Topic> Subscribers
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
