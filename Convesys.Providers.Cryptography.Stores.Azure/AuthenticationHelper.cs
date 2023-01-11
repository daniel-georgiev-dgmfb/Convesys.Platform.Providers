
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("Pirina.Providers.Cryptography.Stores.Azure.Tests.L1")]
namespace Pirina.Providers.Cryptography.Stores.Azure
{
  internal class AuthenticationHelper
  {
    public static async Task<string> GetToken(
      string authority,
      string resource,
      string scope,
      string clientId,
      string clientSecret)
    {
      AuthenticationContext authenticationContext = new AuthenticationContext(authority);
      ClientCredential clientCredential1 = new ClientCredential(clientId, clientSecret);
      string str = resource;
      ClientCredential clientCredential2 = clientCredential1;
      AuthenticationResult authenticationResult = await authenticationContext.AcquireTokenAsync(str, clientCredential2);
      if (authenticationResult == null)
        throw new InvalidOperationException("Failed to obtain the JWT token");
      var token = authenticationResult.AccessToken;
      return token;
    }
  }
}
