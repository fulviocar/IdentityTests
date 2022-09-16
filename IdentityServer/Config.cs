using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
                {
                new ApiScope("api", "API")
                };

        public static IEnumerable<Client> Clients =>
            new Client[]
                {
                new Client()
{
ClientId = "api-client",
ClientSecrets =
{
new Secret("correct horse battery staple".Sha256())
},
AllowedScopes = { "api" },
AllowedGrantTypes = GrantTypes.ClientCredentials
}, new Client()
{
ClientId = "web",
ClientSecrets =
{
new Secret("correct horse battery staple".Sha256())
},
AllowedScopes = new List<string>
    {
IdentityServerConstants.StandardScopes.OpenId,
IdentityServerConstants.StandardScopes.Profile
},
AllowedGrantTypes = GrantTypes.Code,
RedirectUris = { "https://localhost:7213/signin-oidc" },
PostLogoutRedirectUris = {
        "https://localhost:7213/signout-callback-oidc" }
}

                };
    }
}