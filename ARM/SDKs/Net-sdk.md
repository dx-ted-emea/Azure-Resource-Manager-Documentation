#Azure SDK for .NET

## Introduction
WARNING!
At the time of writing this documentation the Azure SDK for .NET is still in preview. Things might not be as stable as you would expect or might even change in the near future. Use the documentation at your own risk.

Azure SDK for .NET is provided as a set of NuGet Packages that helps you call most of the APIs exposed by Azure Resrouce Manager. If the SDK doesn't expose the requried functionality you can easily combine the SDK with regular calls to the ARM REST API behind the scenes.

This documentation is not intended to describe all aspects of Azure SDK for .NET, Azure ARM APIs or Visual Studio, but is rather provided as a fast way for you to get started.

A full downloadable sample project from where all code snippets below have been taken, can be found [here](Samples/Net).

## Setting up the environment

The following NuGet Packages are needed to complete the tasks in this documentation. Install them from the graphical version of NuGet Manager or from the NuGet Manager Console. 

```bash
Install-Package Microsoft.Azure.Common.Authentication -Pre
Install-Package Microsoft.Azure.Management.Compute -Pre
Install-Package Microsoft.Azure.Management.Network -Pre
Install-Package Microsoft.Azure.Management.Resources -Pre
Install-Package Microsoft.Azure.Management.Storage -Pre
```

(As of the time of writing, these packages are only provided as "preview packages" hence you must add the "-Pre" switch in order to have them installed)

## Authentication

Authentication against the Azure APIs are done by passign a token to your requests. You can receive that token by first authenticating against Azure AD. A more indept explanation how it all works can be found in the [REST API documentation](Rest-api.md).

### Registering your application for programatic access

The process of regestering an application for programatic access to Azure and Azure AD is described in [the Authentication documentation](../Authentication.md).
Please make sure you follow that instruction or make sure you have allready registered an Azure AD Application and have the corresponding:

* Azure AD Tenant ID
* Application Client ID
* Application Client Secret

### Receiving the AccessToken from code

The authentication token can easily be aquired with the below lines of code, passing in only your Azure AD Tenant ID, your Azure AD Application Client ID and the Azure AD Application Client Secret. Save the token for several requests since it by default is valid for 1 hour.

```csharp
private static AuthenticationResult GetAccessToken(string tenantId, string clientId, string clientSecret)
{
    Console.WriteLine("Aquiring Access Token from Azure AD");
    AuthenticationContext authContext = new AuthenticationContext
        ("https://login.windows.net/" /* AAD URI */
            + $"{tenantId}.onmicrosoft.com" /* Tenant ID or AAD domain */);

    var credential = new ClientCredential(clientId, clientSecret);

    AuthenticationResult token = authContext.AcquireToken("https://management.azure.com/", credential);

    Console.WriteLine($"Token: {token.AccessToken}");
    return token;
}
```

### Querying Azure subscriptions attached to the authenticated application

One of the first things you might want to do is querying what Azure Subscriptions that are associated with the just authenticated application. The Subscription ID for your targeted subscription will be mandatory to pass to each API call you do from now on.

The below sample code queries Azure APIs directly using the REST API, i.e. not using any features in Azure SDK for .NET.

```csharp
async private static Task<List<string>> GetSubscriptionsAsync(string token)
{
    Console.WriteLine("Querying for subscriptions");
    const string apiVersion = "2015-01-01";

    var client = new HttpClient();
    client.BaseAddress = new Uri("https://management.azure.com/");
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

    var response = await client.GetAsync($"subscriptions?api-version={apiVersion}");

    var jsonResponse = response.Content.AsString();

    var subscriptionIds = new List<string>();
    dynamic json = JsonConvert.DeserializeObject(jsonResponse);

    for (int i = 0; i < json.value.Count; i++)
    {
        subscriptionIds.Add(json.value[i].subscriptionId.Value);
    }

    Console.WriteLine($"Found {subscriptionIds.Count} subscription(s)");
    return subscriptionIds;
}
```
