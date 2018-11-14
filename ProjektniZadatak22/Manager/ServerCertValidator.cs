using System;
using System.IdentityModel.Selectors;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;

namespace Manager
{
	public class ServerCertValidator : X509CertificateValidator
	{
		public override void Validate(X509Certificate2 certificate)
		{
			X509Certificate2 cert = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, Formatter.ParseName(WindowsIdentity.GetCurrent().Name));

			if (!certificate.Issuer.Equals(cert.Issuer))
			{
				throw new Exception("Certificate is not from the valid issuer.");
			}

			if(certificate.NotAfter.Ticks <= DateTime.Now.Ticks)
			{
				throw new Exception("Certificate has expired.");
			}
		}
	}
}
