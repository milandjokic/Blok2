using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    [ServiceContract(CallbackContract = typeof(IMyServiceCallBack))]
    public interface ISubscriber
    {
        [OperationContract]
        bool RegisterSubscriber();

        [OperationContract]
        bool UnregisterSubscriber();

        [OperationContract]
        bool Subscribe(string subject);

        [OperationContract]
        bool Unsubsrcibe(string subject);
    }
}
