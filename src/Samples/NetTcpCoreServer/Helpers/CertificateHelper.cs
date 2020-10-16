using System;
using System.Security.Cryptography.X509Certificates;
using CoreWCF.Description;
using CoreWCF.Security;

namespace NetTcpCoreServer.Helpers
{
  public static class CertificateHelper
  {
    private const string cert =
      "Certificate will be taken from Azure KeyVault as base64 string. Temporary value stored here is not valid and has to be substituted with valid base64 encoded certificate";

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