using System.Net.Security;
using CoreWCF;
using CoreWCF.Configuration;
using CoreWCF.Description;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using NetTcpCoreServer.Contract;
using NetTcpCoreServer.Helpers;

namespace NetTcpCoreServer
{
  public class Startup
  {
    private const string hostUri = "net.tcp://localhost/service/Encrypted";

    #region private methods

    private static NetTcpBinding GetAndSetupTcpBinding()
    {
      NetTcpBinding tcpBinding = new NetTcpBinding();

      // Base binding params setup.
      tcpBinding.Name = "encryptedTcpBinding";
      tcpBinding.MaxBufferPoolSize = 52428800;
      tcpBinding.MaxReceivedMessageSize = 16777216;
      tcpBinding.MaxBufferSize = 16777216;

      // Setup reader quotas.
      tcpBinding.ReaderQuotas.MaxStringContentLength = 65536;
      tcpBinding.ReaderQuotas.MaxArrayLength = 65536;

      // Security settings.
      tcpBinding.Security.Mode = SecurityMode.Transport;
      tcpBinding.Security.Transport.ProtectionLevel = ProtectionLevel.EncryptAndSign;
      tcpBinding.Security.Transport.ClientCredentialType = TcpClientCredentialType.None;
      return tcpBinding;
    }

    #endregion

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddServiceModelServices();
      //services.AddSingleton<IServiceBehavior>(
      //  CertificateHelper.GenerateCertificateEncryptedBehaviour());
    }

    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      app.UseServiceModel(builder =>
      {
        NetTcpBinding tcpBinding = GetAndSetupTcpBinding();

        builder
          .AddService<EchoService>()
          .AddServiceEndpoint<EchoService, IEchoService>(
            binding: tcpBinding,
            address: hostUri)
          .ConfigureServiceHostBase<IEchoService>(
            serviceHost =>
            {
              serviceHost.Description.Behaviors.Add(
                CertificateHelper
                  .GenerateCertificateEncryptedBehaviour());
            });
      });
    }

  }
}
