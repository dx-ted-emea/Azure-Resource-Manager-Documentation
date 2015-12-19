#Azure SDK for .NET

## Introduction
WARNING!
At the time of writing this documentation the Azure SDK for .NET is still in preview. Things might not be as stable as you would expect or might even change in the near future. Use the documentation at your own risk.

Azure SDK for .NET is provided as a set of NuGet Packages that helps you call most of the APIs exposed by Azure Resrouce Manager. If the SDK doesn't expose the requried functionality you can easily combine the SDK with regular calls to the ARM REST API behind the scenes.

This documentation is not intended to describe all aspects of Azure SDK for .NET or Azure ARM APIs, but is rather provided as a fast way for you to get started.

A full downloadable sample project from where all code snippets below have been taken, can be found [here](Samples/Net).
## Authentication

Authentication against the Azure APIs are done by passign a token to your requests. You can receive that token by first authenticating against Azure AD. A more indept explanation how it all works can be found in the [REST API documentation](Rest-api.md).

### Registering your application for programatic access

The process of regestering an application for programatic access to Azure and Azure AD is described in [the Authentication documentation](../Authentication.md).
Please make sure you follow that instruction or make sure you have allready registered an Azure AD Application and have the corresponding:

* Azure AD Tenant ID
* Application Client ID
* Application Client Secret

### Receiving the AccessToken 


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


# Overview 

# Authentication Sample 

# "Hello World" Sample 

# Going deep and references
  