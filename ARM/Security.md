# Authentication 

Authentication and authorization in Azure Resource Manager APIs is based on Azure Active Directory (AAD).
It means that every user or application calling the APIs must have an AAD identity.
Before  working with ARM it is highly recommended to understand the basics of Azure Active Directory (AAD).

Azure Active Directory (AAD) is Microsoft’s multi-tenant cloud based directory and identity management service.
It includes single sign-on (SSO) access to thousands of cloud SaaS Applications such as Office365. Application developers can easily integrate with AAD for identity management.
Azure AD also includes a full suite of identity management capabilities and can be integrated with an existing Windows Server Active Directory.
For details see [What is Azure Active Directory?](https://azure.microsoft.com/en-us/documentation/articles/active-directory-whatis/)


All the tasks on resources using the Azure Resource Manager must be authenticated with Azure Active Directory. 

Setting up authentication includes the following steps:

1. Add an application to the Azure Active Directory tenant.

    This step includes creating two objects:
    
    * AD Application - a directory record in AAD that identifies an application to AAD. 
    
    * Service Principal - an instance of an application in a directory.



2. Set permissions for the application that you added.
    
    The permissions should be granted to the Service Principal created in step #1.

3. Get the token for authenticating requests to Azure Resource Manager.

Authentication can be set through PowerShell, Azure CLI or Azure Management Portal.

For setting up authentication using Azure PowerShell or Azure CLI see: [Authenticating a service principal with Azure Resource Manager](https://azure.microsoft.com/en-us/documentation/articles/resource-group-authenticate-service-principal/)

For setting up authentication using the Management Portal see: [Create Active Directory application and service principal using portal](https://azure.microsoft.com/en-us/documentation/articles/resource-group-create-service-principal-portal/)

## Set Authentication for REST API Calls

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



# Authorization / Access Control

## Azure Active Directory Role-based Access Control


Azure Roles-Based Access Control (RBAC) enables fine-grained access management for Azure. Users, groups, and applications can be granted access to manage resources in the Azure subscription, using Azure Management Portal, Azure Command-Line tools and Azure Management APIs.
Access is granted by assigning the appropriate RBAC role to users, groups, and applications, at the right scope. Roles can be assigned at a subscription level, resource group level or specific resource level.

Azure RBAC has three basic roles that apply to all resource types: Owner, Contributor and Reader. In addition you can use [RBAC Built-In Roles](https://azure.microsoft.com/en-us/documentation/articles/role-based-access-built-in-roles/) for role specifies operations, or you can create [Custom Roles](http://blogs.technet.com/b/ad/archive/2015/12/10/custom-roles-in-azure-rbac-is-now-ga.aspx).

Azure RBAC Built-In roles is a list of roles that can be assigned to users, groups, and services. You can’t modify the definition of Built-In roles. With RBAC Custom-Roles you can define custom roles by composing a set of actions from a list of available actions that can be performed on Azure resources. Currently you can create Custom-Roles using PowerShell or Azure CLI. The Custom-Role creation operation involves import of a JSON file with a list of approved actions for the specific role on a specific resource type, a best practice to build a custom role is to export the JSON file with the approved actions for a build-in role and modify it.

Each subscription in Azure belongs to one and only one directory, each resource group belongs to one and only one subscription, and each resource belongs to one and only one resource group. Access that you grant at parent scopes is inherited at child scopes.

Classic administrators (service-admin and co-admins) have full access to the Azure subscription. In the RBAC model, classic administrators are assigned the Owner role at the subscription scope. The finer-grained authorization model (Azure RBAC) is supported only by the [new management portal](https://portal.azure.com) and Azure Resource Manager APIs.

For more information and instructions how to grant access using the Azure Management Portal, see: [Azure Active Directory Role-based Access Control](https://azure.microsoft.com/en-us/documentation/articles/role-based-access-control-configure/).

For granting access through command tools like Azure PowerShell, CLI or REST API see: [Managing access to resources](https://azure.microsoft.com/en-us/documentation/articles/resource-group-rbac/).

## Lock resources

As an administrator, there are scenarios where you will want to place a lock on a subscription, resource group or resource to prevent other users in your organization from committing write actions or accidentally deleting a critical resource. 

Azure Resource Manager provides the ability to restrict operations on resources through resource management locks. Locks are policies which enforce a lock level at a particular scope. The scope can be a subscription, resource group or resource. The lock level identifies the type of enforcement for the policy, which presently has two values – CanNotDelete and ReadOnly. CanNotDelete means authorized users can still read and modify resources, but they can't delete any of the restricted resources. ReadOnly means authorized users can only read from the resource, but they can't modify or delete any of the restricted resources.

Locks are different from using role-based access control to assign user permissions to perform certain actions. Unlike role-based access control, you use management locks to apply a restriction across all users and roles, and you typically apply the locks for only limited duration.

To create or delete management locks, you must have access to Microsoft.Authorization/* or Microsoft.Authorization/locks/* actions of the built-in roles, only Owner and User Access Administrator are granted with those actions.

When you apply a lock at a parent scope, all child resources inherit the same lock.
If you apply more than one lock to a resource, the most restrictive lock takes precedence. 

Locks can be created through an ARM template, REST API or PowerShell.

For more details see: [Lock resources with Azure Resource Manager](https://azure.microsoft.com/en-us/documentation/articles/resource-group-lock-resources/)



##  Customized policy

Azure Resource Manager now allows you to control access through custom policies. With policies, you can prevent users in your organization from breaking conventions that are needed to manage your organization's resources. 

You create policy definitions that describe the actions or resources that are specifically denied. You assign those policy definitions at the desired scope, such as the subscription, resource group, or an individual resource. 

There are a few key differences between policy and role-based access control, but the first thing to understand is that policies and RBAC work together. To be able to use policy, the user must be authenticated through RBAC. Unlike RBAC, policy is a default allow and explicit deny system. 

RBAC focuses on the actions a user can perform at different scopes. For example, a particular user is added to the contributor role for a resource group at the desired scope, so the user can make changes to that resource group. 

Policy focuses on resource actions at various scopes. For example, through policies, you can control the types of resources that can be provisioned or restrict the locations in which the resources can be provisioned.

Policy definition is created using JSON. It consists of one or more conditions/logical operators which define the actions and an effect which tells what happens when the conditions are fulfilled.

Basically, a policy contains the following:

* Condition/Logical operators: It contains a set of conditions which can be manipulated through a set of logical operators.

* Effect: This describes what the effect will be when the condition is satisfied – either deny or audit. An audit effect will emit a warning event service log. 

 
Policies can be applied at different scopes like subscription, resource groups and individual resources. Policies are inherited by all child resources. So if a policy is applied to a resource group, it will be applicable to all the resources in that resource group.

Creating policy and applying policy can be done through REST API or PowerShell.


For mor details see:  [Use Policy to manage resources and control access](https://azure.microsoft.com/en-us/documentation/articles/resource-manager-policy/ )

# Security related topics

## Audit logs

Azure Resource Manager creates audit log for any operation on resources.  The audit log contains all actions performed on your resources, including the user, the action and the time.

There are two important limitations to keep in mind when working with audit logs:
1.Audit logs are only retained for 90 days.
2.You can only query for a range of 15 days or less.

You can retrieve information from the audit logs through Azure PowerShell, Azure CLI, REST API, or the Azure portal.

For more details see: [logging and Audit operations with Resource Manager](https://azure.microsoft.com/en-us/documentation/articles/resource-group-audit/)


## Security considerations for Azure Resource Manager


When looking at aspects of security for your Azure Resource Manager templates, there are several areas to consider:

* Secrets and certificates
* Network security groups
* Role-based access control (covered above in this article)

### Secrets and certificates
Azure Virtual Machines, Azure Resource Manager and Azure Key Vault are fully integrated to provide support for the secure handling of certificates which are to be deployed in the VM. Utilizing Azure Key Vault with Resource Manager to orchestrate and store VM secrets and certificates is a best practice and provides the following advantages:
* The templates only contain URI references to the secrets, which means the actual secrets are not in code, configuration or source * code repositories. 
* Secrets stored in the Key Vault are under full RBAC control of a trusted operator. 
Full compartmentalization of all assets.
* The loading of secrets into a VM at deployment time occurs via direct channel between the Azure Fabric and the Key Vault within the confines of the Microsoft datacenter. 
* Key Vaults are always regional, so the secrets always have locality (and sovereignty) with the VMs. There are no global Key Vaults.

Additionally, a best practice is to maintain separate templates for:

1. Creation of vaults (which will contain the key material)
2. Deployment of the VMs (with URI references to the keys contained in the vaults)


### Network Security Groups (NSG)
Many scenarios will have requirements that specify how traffic to one or more VM instances in your virtual network is controlled. You can use a Network Security Group (NSG) to do this as part of an ARM template deployment.
A network security group is a top-level object that is associated with your subscription. An NSG contains access control rules that allow or deny traffic to VM instances. The rules of an NSG can be changed at any time, and changes are applied to all associated instances. To use an NSG, you must have a virtual network that is associated with a region (location). 

You can associate an NSG with a VM, or to a subnet within a virtual network. When associated with a VM, the NSG applies to all the traffic that is sent and received by the VM instance. When applied to a subnet within your virtual network, it applies to all the traffic that is sent and received by all the VM instances in the subnet. A VM or subnet can be associated with only 1 NSG, but each NSG can contain up to 200 rules. You can have 100 NSGs per subscription.

For details see: [What is a Network Security Group?](https://azure.microsoft.com/en-us/documentation/articles/virtual-networks-nsg/)

### Resources

To read more about security considerations with Azure Resource Manager see:

[Security considerations for Azure Resource Manager](https://azure.microsoft.com/en-us/documentation/articles/best-practices-resource-manager-security/)

This topic is part of a larger whitepaper. To read the full paper, download [World Class ARM Templates Considerations and Proven Practices](http://download.microsoft.com/download/8/E/1/8E1DBEFA-CECE-4DC9-A813-93520A5D7CFE/World%20Class%20ARM%20Templates%20-%20Considerations%20and%20Proven%20Practices.pdf).







