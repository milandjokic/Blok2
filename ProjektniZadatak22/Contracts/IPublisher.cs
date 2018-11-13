using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    [ServiceContract]
    public interface IPublisher
    {
        [OperationContract]
        bool RegisterPublisher(string subject);

        [OperationContract]
        bool UnregisterPublisher();

        [OperationContract]
        void Publish(Alarm alarm, byte[] signature);
    }
}
