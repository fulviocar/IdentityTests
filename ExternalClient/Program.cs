using IdentityModel.Client;

var tokenClient = new HttpClient();

var doc = await tokenClient.GetDiscoveryDocumentAsync("https://localhost:5001");

var token = await tokenClient.RequestClientCredentialsTokenAsync(
    new ClientCredentialsTokenRequest
{
    Address = doc.TokenEndpoint,
    ClientId = "api-client",
    ClientSecret = "correct horse battery staple",
    Scope = "api"
});

Console.WriteLine(token.AccessToken);

var apiClient = new HttpClient();
apiClient.SetBearerToken(token.AccessToken);

var response = await apiClient.GetAsync(
"https://localhost:7153/weatherforecast");
var data = await response.Content.ReadAsStringAsync();

Console.WriteLine(data);

