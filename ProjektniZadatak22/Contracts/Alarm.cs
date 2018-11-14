using System;

namespace Contracts
{
	[Serializable]
	public class Alarm
	{
		public DateTime TimeStamp { get; set; }
		public string Message { get; set; }
		public int Risk { get; set; }

		public Alarm(DateTime timeStamp, string message, int risk)
		{
			TimeStamp = timeStamp;
			Message = message;
			Risk = risk;
		}

		public string Serialize()
		{
			return TimeStamp.ToString() + ';' + Message + ';' + Risk;
		}

		public Alarm Deserialize(string param)
		{
			string[] tokens = param.Split(';');

			Alarm alarm = new Alarm(DateTime.Parse(tokens[0]), tokens[1], int.Parse(tokens[2]));

			return alarm;
		}
	}
}
