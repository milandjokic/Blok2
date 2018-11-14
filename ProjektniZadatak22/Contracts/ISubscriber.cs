using System.ServiceModel;

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
		bool Subscribe(string subject, int from, int to);

		[OperationContract]
		bool Unsubsrcibe(string subject);
	}
}
