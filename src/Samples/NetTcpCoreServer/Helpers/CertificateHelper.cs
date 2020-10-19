using System;
using System.Security.Cryptography.X509Certificates;
using CoreWCF.Description;
using CoreWCF.Security;

namespace NetTcpCoreServer.Helpers
{
  public static class CertificateHelper
  {
    /// <summary>
    /// Certificate as base64 encoded string.
    /// NOTE:
    ///   Certificate will be taken from Azure KeyVault as base64 string.
    ///   Temporary value stored here is not valid and has to be substituted with valid base64 encoded certificate.
    ///   Current value taken from here: https://www.ibm.com/support/knowledgecenter/en/SSLTBW_2.1.0/com.ibm.zos.v2r1.icha700/basesix.htm
    /// </summary>
    private const string cert =
      "MIICYzCCAcygAwIBAgIBADANBgkqhkiG9w0BAQUFADAuMQswCQYDVQQGEwJVUzEMMAoGA1UEChMDSUJNMREwDwYDVQQLEwhMb2NhbCBDQTAeFw05OTEyMjIwNTAwMDBaFw0wMDEyMjMwNDU5NTlaMC4xCzAJBgNVBAYTAlVTMQwwCgYDVQQKEwNJQk0xETAPBgNVBAsTCExvY2FsIENBMIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQD2bZEo7xGaX2/0GHkrNFZvlxBou9v1Jmt/PDiTMPve8r9FeJAQ0QdvFST/0JPQYD20rH0bimdDLgNdNynmyRoS2S/IInfpmf69iyc2G0TPyRvmHIiOZbdCd+YBHQi1adkj17NDcWj6S14tVurFX73zx0sNoMS79q3tuXKrDsxeuwIDAQABo4GQMIGNMEsGCVUdDwGG+EIBDQQ+EzxHZW5lcmF0ZWQgYnkgdGhlIFNlY3VyZVdheSBTZWN1cml0eSBTZXJ2ZXIgZm9yIE9TLzM5MCAoUkFDRikwDgYDVR0PAQH/BAQDAgAGMA8GA1UdEwEB/wQFMAMBAf8wHQYDVR0OBBYEFJ3+ocRyCTJw067dLSwr/nalx6YMMA0GCSqGSIb3DQEBBQUAA4GBAMaQzt+zaj1GU77yzlr8iiMBXgdQrwsZZWJo5exnAucJAEYQZmOfyLiMD6oYq+ZnfvM0n8G/Y79q8nhwvuxpYOnRSAXFp6xSkrIOeZtJMY1h00LKp/JX3Ng1svZ2agE126JHsQ0bhzN5TKsYfbwfTwfjdWAGy6Vf1nYi/rO+ryMO";

    /// <summary>
    /// Takes base64 string and parses it as a certificate.
    /// </summary>
    public static X509Certificate2 GetCertificateFromBase64()
    {
      return GetCertificateFromBase64(cert);
    }

    /// <summary>
      /// Takes base64 string and parses it as a certificate.
      /// </summary>
      public static X509Certificate2 GetCertificateFromBase64(string base64)
    {
      if (string.IsNullOrWhiteSpace(base64))
      {
        Console.WriteLine("Base64 string is empty...");
        return null;
      }
      else
      {
        Console.WriteLine("Base64 string received.");
        Console.WriteLine("Reading raw data...");
        var rawData = Convert.FromBase64String(base64);
        Console.WriteLine("Creating X509Certificate...");

        var x509Certificate = new X509Certificate2(
          rawData,
          (string)null,
          X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable | X509KeyStorageFlags.PersistKeySet);

        Console.WriteLine("Certificate created.");
        return x509Certificate;
      }
    }

    public static ServiceCredentials GenerateCertificateEncryptedBehaviour()
    {
      ServiceCredentials certBehavior = new ServiceCredentials();
      certBehavior.ServiceCertificate.Certificate = GetCertificateFromBase64(cert);
      certBehavior.UserNameAuthentication.UserNamePasswordValidationMode =
        UserNamePasswordValidationMode.Custom;
      certBehavior.ClientCertificate.Authentication.CertificateValidationMode =
        X509CertificateValidationMode.None;
      return certBehavior;
    }
  }
}