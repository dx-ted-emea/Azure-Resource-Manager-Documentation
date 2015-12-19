#Azure SDK for .NET

## Introduction


## Authentication

Authentication against the Azure APIs are done by passign a token to your requests. You can receive that token by first authenticating against Azure AD. A more indept explanation how it all works can be found in the [REST API documentation](Rest-api.md).

### Registering

The process of regestering an application for programatic access to Azure and Azure AD is described in [the Authentication documentation](../Authentication.md).
Please make sure you follow that instruction or make sure you have allready registered an Azure AD Application and have the corresponding:

* Azure AD Tenant ID
* Application Client ID
* Application Client Secret

### Calling 
*draft*

Azure Marketplace is great place to publish your apps 
Here's example of web link [Azure web site](http://azure.microsoft.com//) and the [Azure stuff on github](http://azure.github.io/).
Wiki also have *another font* which may be usefull too.


Example of some command line code 

    sudo apt-get install apache2

Example of some json code
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
  