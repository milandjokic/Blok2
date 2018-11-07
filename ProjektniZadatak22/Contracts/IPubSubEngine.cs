using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    [ServiceContract]
    public interface IPubSubEngine
    {
        [OperationContract]
        bool RegisterPublisher(string subject);

        [OperationContract]
        bool UnregisterPublisher();

        [OperationContract]
        bool RegisterSubscriber();

        [OperationContract]
        bool UnregisterSubscriber();

        [OperationContract]
        bool Subscribe(string subject);

        [OperationContract]
        bool Unsubsrcibe(string subject);

        [OperationContract]
        void Publish(Alarm alarm);
    }
}
