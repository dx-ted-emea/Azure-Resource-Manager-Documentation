## Authentication for programatic access

Authentication for ARM is handled by Azure Active Directory, AD. In order to connect to any APIs you first need to authenticate with Azure AD to receive an authentication token that you can pass on to every request to the APIs. There are many ways to authenticate, but since we are describing scenarios where you programatically access the ARM APIs, we will  assume that you don’t want to authenticate with a normal username password where a pop-up-screen might prompt you for username and password and perhaps even other authentication mechanisms used in two factor authentication scenarios. Therefore, we will create what is called an Azure AD Application and a Service Principal that we will use to login with. But remember that Azure AD support several authentication procedures and all of them could be used to retrieve that authentication token that we need for subsequent API requests.

### Configuring Azure AD to allow requests from you and your application

In order to have an application authenticated and authorized to to requests against the ARM APIs you first need to register the app and the Service Principal in Azure AD. To do that we are going to use Azure Command Line Interface, CLI, a cross platform tool written in Node.js that runs on Windows, Mac and Linux (and every other computer that can run Node.JS).

In the introduction we said that we would not use any SDK to to complete the requests against Azure ARM APIs and you could think that we are now cheating by installing Azure CLI. Just remember that registering your application and service principal is a one-time registration that could be done using other tools or even the Azure Portal, so we consider that's ok for now. So let's continue and install Azure CLI.

#### Installing Azure CLI

Azure CLI is written in Node.js and can be installed directly from the terminal/command line using the following command as long as you have Node.js installed:

```bash
> npm install -g azure-cli
```

If you don’t have Node.js installed or want more information, please have a look at the official documentation on [how to install Azure CLI]( https://azure.microsoft.com/en-us/documentation/articles/xplat-cli/).

#### Register your Application and Service Principal in Azure AD 

The complete process of [Authenticating a service principal with Azure Resource Manager is documented here](https://azure.microsoft.com/en-us/documentation/articles/resource-group-authenticate-service-principal/), make sure you read it if you want more details or need to use other authentication mechanisms like certificates, etc.

##### Connect to your Azure Subscription

From a terminal/console window, execute the following command to switch to ARM mode (needed to access the ARM APIs in Azure)

```bash
> azure config mode arm
```

Attach your subscription to Azure CLI by executing

```bash
> azure login
```

##### Create an Azure AD Application in your current AD Tenant

In order for you or your application to be allowed to access the ARM APIs we need to register that application. During this tutorial we will not create an application but we still need to register one in order to prove how it works.

Execute the following command to register an application in Azure AD. Replace the parameters within values that describes your application.

The URLs need to be well formed URLs but don't have to be working globally accessable URLs so just use any URL for now. (We will not make use of those URLs during this tutorial, but they can be important in other scenarios)

```bash
azure ad app create --name "<name>" --home-page "<well formed URI to application homepage" --identifier-uris "well formed URI that identifies the application" --password "<password to use for authentication>"
```

If successfully executed, the above command will have created your application inside your Azure AD Tenant and will (among other) return an Application Id. Please make a note of that ID, since you will use it in the below command that will create a Service Principal and attach that to your application. Execute the following command and replace "application id" with the value you received above.

```bash
azure ad sp create <application id>
```

Among others, that command will return the "object id" of the newly created Service Principal. Note that and use it in the below command to assign the role as subscription owner to the new Service Principal

```bash
azure role assignment create --objectId <object id> --roleName Owner
```

By now, everything should be registered in Azure AD you you to start authenticating and calling ARM APIs.
