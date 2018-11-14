using System.ServiceModel;

namespace Contracts
{

	public interface IMyServiceCallBack
	{
		[OperationContract]
		void ReceiveAlarm(Alarm alarm, byte[] signature, int id);

		[OperationContract]
		void ReceiveTopics(string topics);
	}
}
