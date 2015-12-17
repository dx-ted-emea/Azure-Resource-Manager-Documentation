# Azure Resource Manager Java SDK

## Overview 
The Azure Resource Manager SDK is hosted in github [Azure Java SDK repository](https://github.com/azure/azure-sdk-for-java). Note that at the time of writing this the SDK is in **preview**. 

The following packages are available:
* Compute Management: (azure-mgmt-compute)
* DNS Management: (azure-mgmt-dns)
* Network Management: (azure-mgmt-network)
* Resource Management: (azure-mgmt-resources)
* SQL Management: (azure-mgmt-sql)
* Storage Management: (azure-mgmt-storage)
* Traffic Manager Management: (azure-mgmt-traffic-manager)
* Utilities and Helpers: (azure-mgmt-utility)
* WebSites / WebApps Management: (azure-mgmt-websites)

Follow the [Azure SDK for Java Features Wiki page](https://github.com/Azure/azure-sdk-for-java/wiki/Azure-SDK-for-Java-Features) for an up-to-date list.

The SDK contains a samples package with a collection of getting started samples: azure-mgmt-samples

## Prerequisites
1. Java v1.6+
2. [maven](https://maven.apache.org/) If you would like to develop on the SDK

## How to get the SDK
[Maven](https://maven.apache.org/) distributed jars are the recommended way of getting started with the Azure Java SDK. You can add these dependencies to many of the Java dependency managment tools (Maven, Gradle, Ivy...).
Follow this [link](http://search.maven.org/#search%7Cga%7C1%7Cg%3A%22com.microsoft.azure%22) for a listing of the libraries available in maven.

Alternativly, you grab the sdk directly from source using git. To get the source code of the SDK via git:
```
git clone https://github.com/Azure/azure-sdk-for-java.git
cd ./azure-sdk-for-java/
```

> The samples in this overview will use Maven as the source for the SDK packages.

## Helper Classes
The SDK includes helper classes for several of the main packages. The helper classes are implemeted in the *auzre-mgmt-utility* package:
* AuthHelper - authentication helper class
* ComputeHelper - compute helper class
* NetworkHelper - network helper class
* ResourceHelper - deployments helper class
* StorageHelper - storage helper class
 
**Maven dependency information**
```xml
<dependency>
    <groupId>com.microsoft.azure</groupId>
    <artifactId>azure-mgmt-utility</artifactId>
    <version>0.8.0</version>
</dependency>
```
This will use the 0.8.0 version of the utility package. Find all the packages in the [maven public repository](http://search.maven.org/#search%7Cga%7C1%7Cg%3A%22com.microsoft.azure%22)

## Authentication
Authentication against the Azure Resource Manager is achieved using a Service Principal. A complete walkthrough can be found in the [ARM Security and Authentication](ARM/Security.md) section in this repository. 
After creating the service principal, you should have:

1. Client id (GUID)
2. Client secret (string)
3. Tenant id (GUID) or domain name (string)

Once you have this values, you can obtain an Active Directory Access Token. This token is valid for one hour, and can be refreshed to last 14 days. Read more about AD token lifetime [here](http://www.cloudidentity.com/blog/2015/03/20/azure-ad-token-lifetime/).

The Java SDK include a helper class [AuthHelper](https://github.com/Azure/azure-sdk-for-java/blob/master/resource-management/azure-mgmt-utility/src/main/java/com/microsoft/azure/utility/AuthHelper.java) that creates the access token, once provided with the client id, secret and tanant id.

Example usage  [ServicePrincipalExample](https://github.com/Azure/azure-sdk-for-java/blob/master/azure-mgmt-samples/src/main/java/com/microsoft/azure/samples/authentication/ServicePrincipalExample.java): 
```java
public static Configuration createConfiguration() throws Exception {
        String baseUri = System.getenv("arm.url");

        return ManagementConfiguration.configure(
                null,
                baseUri != null ? new URI(baseUri) : null,
                System.getenv(ManagementConfiguration.SUBSCRIPTION_ID),
                AuthHelper.getAccessTokenFromServicePrincipalCredentials(
                        System.getenv(ManagementConfiguration.URI), System.getenv("arm.aad.url"),
                        System.getenv("arm.tenant"), System.getenv("arm.clientid"),
                        System.getenv("arm.clientkey"))
                        .getAccessToken());
}
```

## Create a Virtual Machine 
The utility package includes a helper class [ComputeHelper](https://github.com/Azure/azure-sdk-for-java/blob/master/resource-management/azure-mgmt-utility/src/main/java/com/microsoft/azure/utility/ComputeHelper.java) to create a virtual machine. A few samples for working with virtual machines can be found in the azure-mgmt-samples package under [compute](https://github.com/Azure/azure-sdk-for-java/tree/master/azure-mgmt-samples/src/main/java/com/microsoft/azure/samples/compute).

The follwing is a simple flow for creating a virtual machine. In this example, the helper class will create the sotrage and the netwrok as part of creating the VM:
```java
public static void main(String[] args) throws Exception {
        Configuration config = createConfiguration();
        ResourceManagementClient resourceManagementClient = ResourceManagementService.create(config);
        StorageManagementClient storageManagementClient = StorageManagementService.create(config);
        ComputeManagementClient computeManagementClient = ComputeManagementService.create(config);
        NetworkResourceProviderClient networkResourceProviderClient = NetworkResourceProviderService.create(config);

        String resourceGroupName = "javasampleresourcegroup";
        String region = "EastAsia";

        ResourceContext context = new ResourceContext(
                region, resourceGroupName, System.getenv(ManagementConfiguration.SUBSCRIPTION_ID), false);

        System.out.println("Start create vm...");

        try {
            VirtualMachine vm = ComputeHelper.createVM(
                    resourceManagementClient, computeManagementClient, networkResourceProviderClient, 
                    storageManagementClient,
                    context, vnName, vmAdminUser, vmAdminPassword)
              .getVirtualMachine();

            System.out.println(vm.getName() + " is created");
        } catch (Exception ex) {
            System.out.println(ex.toString());
        }
}
```

## Deploy a template
The [ResouceHelper](https://github.com/Azure/azure-sdk-for-java/blob/master/resource-management/azure-mgmt-utility/src/main/java/com/microsoft/azure/utility/ResourceHelper.java) class was created to ease the process of deploying an ARM template with the Java SDK. 
```java
// create a new resource group
ResourceManagementClient resourceManagementClient = createResourceManagementClient();
ResourceContext resourceContext = new ResourceContext(
        resourceGroupLocation, resourceGroupName,
        System.getenv(ManagementConfiguration.SUBSCRIPTION_ID), false);
ComputeHelper.createOrUpdateResourceGroup(resourceManagementClient, resourceContext);

// create the template deployment
DeploymentExtended deployment = ResourceHelper.createTemplateDeploymentFromURI(
        resourceManagementClient,
        resourceGroupName,
        DeploymentMode.Incremental,
        deploymentName,
        TEMPLATE_URI,
        "1.0.0.0",
        parameters);
```
More samples can be found in the samples packages under [templatedeployments](https://github.com/Azure/azure-sdk-for-java/tree/master/azure-mgmt-samples/src/main/java/com/microsoft/azure/samples/templatedeployments).

## Further Reading and Help
Azure SDK for Java documentation: [Java docs](http://azure.github.io/azure-sdk-for-java/)
If you encounter any bugs with the SDK please file an issue via [Issues](https://github.com/Azure/azure-sdk-for-java/issues) or checkout [StackOverflow for Azure Java SDK](http://stackoverflow.com/questions/tagged/azure-java-sdk).

## Resource and References
http://www.cloudidentity.com/blog/2015/03/20/azure-ad-token-lifetime/
