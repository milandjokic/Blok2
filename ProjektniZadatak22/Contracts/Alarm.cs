using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    [Serializable]
    public class Alarm
    {
        private DateTime dateTime;
        private string message;
        private int risk;

        public Alarm() { }

        public Alarm(DateTime dateTime, string message, int risk)
        {
            this.DateTime = dateTime;
            this.Message = message;
            this.Risk = risk;
        }

        public int Risk
        {
            get { return risk; }
            set { risk = value; }
        }

        public string Message
        {
            get { return message; }
            set { message = value; }
        }

        public DateTime DateTime
        {
            get { return dateTime; }
            set { dateTime = value; }
        }

        public string Serialize()
        {
            return DateTime.ToString() + ';' + Message + ';' + Risk;
        }

        public Alarm Deserialize(string param)
        {
            Alarm alarm = new Alarm();

            string[] tokens = param.Split(';');

            alarm.DateTime = DateTime.Parse(tokens[0]);
            alarm.Message = tokens[1];
            alarm.Risk = Int32.Parse(tokens[2]);

            return alarm;
        }
    }
}
