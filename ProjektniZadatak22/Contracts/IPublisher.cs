using System.ServiceModel;

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
