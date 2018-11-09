using System;
using System.Collections.Generic;
using System.IdentityModel.Selectors;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Manager
{
    public class ClientCertValidator : X509CertificateValidator
    {
        public override void Validate(X509Certificate2 certificate)
        {
            if (certificate.NotAfter.Ticks <= DateTime.Now.Ticks)
                throw new Exception("Certificate has expired.");

            if(!certificate.Subject.Equals(certificate.Issuer))            
                throw new Exception("Certificate is not self issued.");

            if (!certificate.SubjectName.Name.Equals(string.Format("CN={0}", "PubSubEngine")))
                throw new Exception("CN is not corresponding.");
        }
    }
}
