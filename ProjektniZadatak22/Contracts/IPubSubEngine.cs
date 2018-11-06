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
        void Subscribe(Alarm alarm);

        [OperationContract]
        void Unsubsrcibe(Alarm alarm);

        [OperationContract]
        void Publish(Alarm alarm);
    }
}
