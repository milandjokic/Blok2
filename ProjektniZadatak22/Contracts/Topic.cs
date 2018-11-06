using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    [Serializable]
    public class Topic
    {
        private string subject;
        private List<Alarm> alarms;

        public Topic(string subject)
        {
            this.Subject = subject;
            Alarms = new List<Alarm>();
        }

        public List<Alarm> Alarms
        {
            get { return alarms; }
            set { alarms = value; }
        }


        public string Subject
        {
            get { return subject; }
            set { subject = value; }
        }

    }
}
