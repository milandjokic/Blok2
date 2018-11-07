using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subscriber
{
    public class SubscriberHost : ISubscriber
    {
        public void ListAllTopics(List<string> subjects)
        {
            Console.WriteLine("Available topics: ");
            foreach(string subject in subjects)
            {
                Console.WriteLine("\t-" + subject);
            }
        }
    }
}
