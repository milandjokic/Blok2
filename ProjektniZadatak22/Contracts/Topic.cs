using System;
using System.Collections.Generic;

namespace Contracts
{
	[Serializable]
	public class Topic
	{
		public string Subject { get; set; }
        public int From { get; set; }
        public int To { get; set; }

        public Topic(string subject)
        {
            Subject = subject;
        }

		public Topic(string subject, int from, int to)
		{
			Subject = subject;
            From = from;
            To = to;
		}
	}
}
