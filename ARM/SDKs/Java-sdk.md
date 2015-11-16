# Azure Resource Manager Java SDK

## Overview 
The Azure Resource Manager SDK is hosted in github [Azure Java SDK repostiory](https://github.com/azure/azure-sdk-for-java). The current SDK version is 0.8.0. Note that at the time of writing this the SDK is in **preview**. 

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
* ResourceHelper - resource groups helper class
* StorageHelper - storage helper class
 
**Maven dependency information**
```
<dependency>
    <groupId>com.microsoft.azure</groupId>
    <artifactId>azure-mgmt-utility</artifactId>
    <version>0.8.0</version>
</dependency>
```

## Authentication
Authentication against the Azure Resource Manager is achieved using a Service Principal. A complete walkthrough can be found in the [ARM Security and Authentication](ARM/Security.md) section in this repository. 
After creating the service principal, you should have:

1. Client id (GUID)
2. Client secret (string)
3. Tenant id (GUID) or domain name (string)

Example usage can be found in the SQK sample [ServicePrincipalExample](https://github.com/Azure/azure-sdk-for-java/blob/master/azure-mgmt-samples/src/main/java/com/microsoft/azure/samples/authentication/ServicePrincipalExample.java) class. 
You can also use the [AuthHelpe](https://github.com/Azure/azure-sdk-for-java/blob/master/resource-management/azure-mgmt-utility/src/main/java/com/microsoft/azure/utility/AuthHelper.java) class:
```

```

## Create a Virtual Machine 

## Deploy a template

