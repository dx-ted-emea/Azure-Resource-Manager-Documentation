# ARM REST API 

## Introduction

Behind every call to Azure Resource Manager, behind every deployed template, behind every configured storage account there is one or several calls to the Azure Resource Manager’s RESTful API. This section is devoted to those APIs and how you can call them without using any SDK at all. This can be very useful if you want full control of all requests to Azure or if the SDK for you’re your language is not available or doesn’t support the operations you want to perform.

This documentation will not go through every API that is exposed in Azure, but will rather use some as an example how you go ahead and connect to them. If you understand the basics you can then go ahead and read the [Azure Resource Manager REST API Reference](https://msdn.microsoft.com/en-us/library/azure/dn790568.aspx) to find detailed information on how to use the rest of the APIs.

## Authentication

Authentication for ARM is handled by Azure Active Directory, AD. In order to connect to any APIs you first need to authenticate with Azure AD to receive an authentication token that you can pass on to every request to the APIs. As we are describing a pure calls directly to the REST APIs, we will also assume that you don’t want to authenticate with a normal username password where a pop-up-screen might prompt you for username and password and perhaps even other authentication mechanisms used in two factor authentication scenarios. Therefore, we will create what is called an Azure AD Application and a Service Principal that we will use to login with. But remember that Azure AD support several authentication procedures and all of them could be used to retrieve that authentication token that we need for subsequent API requests.

### Configuring Azure AD to allow requests from you and your application

In order to have an application authenticated and authorized to to requests against the ARM APIs you first need to register the app and the Service Principal in Azure AD. To do that we are going to use Azure Command Line Interface, CLI, a cross platform tool written in Node.js that runs on Windows, Mac and Linux (and every other computer that can run Node.JS).

In the introduction we said that we would not use any SDK to to complete the requests against Azure ARM APIs and you could think that we are now cheating by installing Azure CLI. Just remember that registering your application and service principal is a one-time registration that could be done using other tools or even the Azure Portal, so we consider that's ok for now. So let's continue and install Azure CLI.

#### Installing Azure CLI

Azure CLI is written in Node.js and can be installed directly from the terminal/command line using the following command as long as you have Node.js installed:

```
> npm install -g azure-cli
```

If you don’t have Node.js installed or want more information, please have a look at the official documentation on [how to install Azure CLI]( https://azure.microsoft.com/en-us/documentation/articles/xplat-cli/).

#### Register your Application and Service Principal in Azure AD 

The complete process of [Authenticating a service principal with Azure Resource Manager is documented here](https://azure.microsoft.com/en-us/documentation/articles/resource-group-authenticate-service-principal/), make sure you read it if you want more details or need to use other authentication mechanisms like certificates, etc.

##### Connect to your Azure Subscription

From a terminal/console window, execute the following command to switch to ARM mode (needed to access the ARM APIs in Azure)

```
> azure config mode arm
```

Attach your subscription to Azure CLI by executing

```
> azure login
```

##### Create an Azure AD Application in your current AD Tenant

In order for you or your application to be allowed to access the ARM APIs we need to register that application. During this tutorial we will not create an application but we still need to register one in order to prove how it works.

Execute the following command to register an application in Azure AD. Replace the parameters within values that describes your application.

The URLs need to be well formed URLs but don't have to be working globally accessable URLs so just use any URL for now. (We will not make use of those URLs during this tutorial, but they can be important in other scenarios)

```
azure ad app create --name "<name>" --home-page "<well formed URI to application homepage" --identifier-uris "well formed URI that identifies the application" --password "<password to use for authentication>"
```

If successfully executed, the above command will have created your application inside your Azure AD Tenant and will (among other) return an Application Id. Please make a note of that ID, since you will use it in the below command that will create a Service Principal and attach that to your application. Execute the following command and replace "application id" with the value you received above.

```
azure ad sp create <application id>
```

Among others, that command will return the "object id" of the newly created Service Principal. Note that and use it in the below command to assign the role as subscription owner to the new Service Principal

```
azure role assignment create --objectId <object id> --roleName Owner
```

By now, everything should be registered in Azure AD you you to start authenticating and calling ARM APIs.

## Calling ARM REST API

### Generic

#### Autentication

As we previously assumed, this documentation will show you how to authenticate using the above created Service Principal. Remember, this is only one way to authenticate your application or you as an end user, but full documentation on how to authenticate against Azure AD is out of scope for this documentation.

Autentication against Azure AD is done by calling out to Azure AD, located at login.microsoftonline.com. In order to authenticate you need to have the following information:

* Azure AD Tenant ID (the name of that Azure AD you are using to login, often the same as your company but not necessary)
* Application ID (created above)
* Password (that you selected while creating the Azure AD Application)

In the below HTTP request make sure to replace "Azure AD Tenant ID", "Application ID" and "Password" with the correct values.

Request:

```HTTP
POST /<Azure AD Tenant ID>.onmicrosoft.com/oauth2/token?api-version=1.0 HTTP/1.1 HTTP/1.1
Host: login.microsoftonline.com
Cache-Control: no-cache
Content-Type: application/x-www-form-urlencoded

grant_type=client_credentials&resource=https%3A%2F%2Fmanagement.core.windows.net%2F&client_id=<Application ID>&client_secret=<Password>
```

Response:
```JSON
{
  "token_type": "Bearer",
  "expires_in": "3600",
  "expires_on": "1448199959",
  "not_before": "1448196059",
  "resource": "https://management.core.windows.net/",
  "access_token": "eyJ0eXAiOiJKV1QiLCJhb...86U3JI_0InPUk_lZqWvKiEWsayA"
}
```
(The access_token in the above response have been shortened to increase readability)

The response contains an Access Token, information on how long that token is valid and for what resource you can use that token. You should implement some kind of caching mechanism to re-use the token for the duration of it's lifetime in order to increase performance while calling the ARM APIs.

The access token will be provided for all request to the ARM API as a header named "Authorization" with the value "Bearer YOUR_ACCESS_TOKEN". Notice the space between "Bearer" and your Access Token.

#### Calling ARM APIs

All [Azure Resource Manager REST APIs are documentet here](https://msdn.microsoft.com/en-us/library/azure/dn790568.aspx). Use that to figure out how each API work and what parameters you need to provide. In this documentation will not go through all of those, but rather focus on a few of them in order to show "how calling" different APIs work.

### Bash (Mac/OSX)

For this tutorial we'll be using the terminal windows in Mac OSX also called Bash.

#### Prerequisites

During this tutorial we'll use the command line tool "[./jq](https://stedolan.github.io/jq/)" that will help us parse the resulting JSON-documents. This tool is not needed in order to call the ARM REST API, but it helps a lot to visualize and parse the results.

You can install "jq" easily using "[brew](http://brew.sh/)" from the termial

```console
brew install jq
```

#### Autentication

Since the ARM API is RESTful we can easily call it using the command line tool "curl" and in order to call any other API we first need to authenticate agains Azure AD using the Service Principal we previously created. As a result of that authentication we will get a token that we then can use while calling the other ARM APIs. 

### PowerShell (Windows)

### PostMan (Chrome App)

### Authenticating

Bash

```console
curl -X POST -H "Content-Type: application/x-www-form-urlencoded" -d "grant_type=client_credentials&resource=https://management.core.windows.net&client_id=<application id>&client_secret=<password you selected for authentication>" https://login.microsoftonline.com/microsoft.onmicrosoft.com/oauth2/token?api-version=1.0
```

PowerShell

```powershell
Invoke-RestMethod -Uri https://login.microsoftonline.com/microsoft.onmicrosoft.com/oauth2/token?api-version=1.0 -Method Post
 -Body @{"grant_type" = "client_credentials"; "resource" = "https://management.core.windows.net/"; "client_id" = "<application id>"; "client_secret" = "<password you selected for authentication>" }
```



... to be provided ...
