using Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Subscriber
{
    public class Subscriber : ChannelFactory<IPubSubEngine>, IPubSubEngine, IDisposable
    {
        IPubSubEngine factory;

        public Subscriber(NetTcpBinding binding, EndpointAddress address) : base(binding, address)
        {
            factory = this.CreateChannel();
        }

        public bool Subscribe(string subject)
        {
            throw new NotImplementedException();
        }

        public bool Unsubsrcibe(string subject)
        {
            throw new NotImplementedException();
        }

        public bool RegisterPublisher(string subject)
        {
            throw new NotImplementedException();
        }

        public bool UnregisterPublisher()
        {
            throw new NotImplementedException();
        }

        public void Publish(Alarm alarm)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            if (factory != null)
            {
                factory = null;
            }

            this.Close();
        }

        public bool RegisterSubscriber()
        {
            return factory.RegisterSubscriber();
        }

        public bool UnregisterSubscriber()
        {
            throw new NotImplementedException();
        }
    }
}
