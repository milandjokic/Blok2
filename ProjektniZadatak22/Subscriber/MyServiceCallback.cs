using Contracts;
using Manager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace Subscriber
{
	public class MyServiceCallback : IMyServiceCallBack
	{
        public void ReceiveAlarm(Alarm alarm, byte[] signature, int id)
		{
            X509Certificate2 clientCert = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, "SignP");
            if (DigitalSignature.Verify(alarm, "SHA1", signature, clientCert))
            {
                File.AppendAllText("alarms" + id + ".txt", alarm.Serialize() + '\n');
            }
		}

		public void ReceiveTopics(string topics)
		{
            Console.WriteLine("Topics:");
			Console.Write(topics);
		}
	}
}
