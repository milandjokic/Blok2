using System;
using System.IdentityModel.Selectors;
using System.Security.Cryptography.X509Certificates;

namespace Manager
{
	public class ClientCertValidator : X509CertificateValidator
	{
		public override void Validate(X509Certificate2 certificate)
		{
			if (certificate.NotAfter.Ticks <= DateTime.Now.Ticks)
				throw new Exception("Certificate has expired.");

            if (!certificate.SubjectName.Name.Equals(string.Format("CN={0}", "PubSubEngine")))
                throw new Exception("CN is not corresponding.");

            if (!certificate.Subject.Equals(certificate.Issuer))            
				throw new Exception("Certificate is not self issued.");

			
		}
	}
}
