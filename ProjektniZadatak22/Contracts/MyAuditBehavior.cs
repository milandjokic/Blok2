using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public class MyAuditBehavior
    {
        private static EventLog MyLogger { get; set; }
        static MyAuditBehavior()
        {
            if (!EventLog.SourceExists("Subscriber"))
            {
                EventLog.CreateEventSource("Subscriber", "SubscriberEvents");
            }
            MyLogger = new EventLog("SubscriberEvents", Environment.MachineName, "Subscriber");
        }


        public static void SubLogger(DateTime timeStamp, string dataBaseName, byte[] signature)
        {
            //string signatureKey;
            //foreach(byte key in signature)
            //{
            //    signatureKey += 
            //}

            MyLogger.WriteEntry(String.Format("{0}\t{1}\t{2}", timeStamp.ToString(), dataBaseName, Encoding.ASCII.GetString(signature)));
        }
    }
}
