using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;

namespace SecureWebApp.Extensions
{
    // From: https://github.com/juunas11/Joonasw.ManagedIdentityDemos/blob/master/Joonasw.ManagedIdentityDemos/Extensions/WebHostBuilderExtensions.cs
    public static class WebHostBuilderExtensions
    {
        public static IWebHostBuilder UseAzureKeyVaultConfiguration(this IWebHostBuilder webHostBuilder)
        {
            return webHostBuilder.ConfigureAppConfiguration(builder =>
            {
                IConfigurationRoot config = builder.Build();
                string keyVaultUrl = config["Lab:KeyVaultBaseUrl"];

                if (!string.IsNullOrEmpty(keyVaultUrl))
                {
                    // Need token provider to use Managed Identity
                    var azureServiceTokenProvider = new AzureServiceTokenProvider();
                    var kvClient = new KeyVaultClient((authority, resource, scope) => azureServiceTokenProvider.KeyVaultTokenCallback(authority, resource, scope));
                    builder.AddAzureKeyVault(keyVaultUrl, kvClient, new DefaultKeyVaultSecretManager());
                }
            });
        }
    }
}
