using Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Publisher
{
    public class Publisher : ChannelFactory<IPubSubEngine>, IPubSubEngine, IDisposable
    {
        bool stopThread = false;

        IPubSubEngine factory;

        public bool StopThread { get => stopThread; set => stopThread = value; }

        public Publisher(NetTcpBinding binding, EndpointAddress address) : base(binding,address)
        {
            factory = this.CreateChannel();
        }
        
        


        public bool RegisterPublisher(string subject)
        {
            if(factory.RegisterPublisher(subject))
            {
                Console.WriteLine("Publisher successfully registered.");
                return true;
            }
            else
            {
                Console.WriteLine("Publisher already registered.");
                return false;
            }
        }

        public void Subscribe(Alarm alarm)
        {
            throw new NotImplementedException();
        }

        public bool UnregisterPublisher()
        {
            if (factory.UnregisterPublisher())
            {
                Console.WriteLine("Publisher successfully unregistered.");
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

        public void Dispose()
        {
            if(factory!=null)
            {
                factory = null;
            }

            this.Close();
        }

        public void CreateAlarm(int period)
        {
            while(!StopThread)
            {
                string[] messages = File.ReadAllLines("messages.txt");
                Random randomInt = new Random();
                Alarm alarm = new Alarm(DateTime.Now, messages[randomInt.Next(0, messages.Count())], randomInt.Next(0, 101));

                Console.WriteLine("Napravljen alarm");
                Publish(alarm);

                Thread.Sleep(period * 1000);
            }
        }

        public void Publish(Alarm alarm)
        {
            factory.Publish(alarm);
        }
    }
}
