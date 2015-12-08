## Security and Authentication 

Authentication and authorization in Azure Resource Manager APIs is based on Azure Active Directory (AAD).
It means that every user or application calling the APIs must have an AAD identity.
Before  working with ARM it is highly recomended to understand the basics of Azure Active Directory (AAD).

Azure Active Directory (AAD) is Microsoft’s multi-tenant cloud based directory and identity management service.
It includes single sign-on (SSO) access to thousands of cloud SaaS Applications like for example Office365. Application developers can easily integrate with AAD for identity management.
Azure AD also includes a full suite of identity management capabilities and can be integrated with an existing Windows Server Active Directory
For details see [What is Azure Active Directory?](https://azure.microsoft.com/en-us/documentation/articles/active-directory-whatis/)

# Authentication 

All the tasks on resources using the Azure Resource Manager must be authenticated with Azure Active Directory. Authentication can be set throuh PowerShell, Azure CLI or Azure Management Portal.

Setting up authentication includes the following steps:

1. Add an application to the Azure Active Directory tenant.

    This step inclueds creating two objects:
    
    * AD Application - a directory record in AAD that identifies an application to AAD. 
    
    * Service Principal - an instance of an application in a directory.



2. Set permissions for the application that you added.
    
    The permissions sholud be granted to the Service Pincipal created in step #1.

3. Get the token for authenticating requests to Azure Resource Manager.


## Set up authentication using Azure PowerShell or Azure CLI

[Authenticating a service principal with Azure Resource Manager](https://azure.microsoft.com/en-us/documentation/articles/resource-group-authenticate-service-principal/)

## Set up authentication using the Management Portal

[Create Active Directory application and service principal using portal](https://azure.microsoft.com/en-us/documentation/articles/resource-group-create-service-principal-portal/)





# Access Control 

## Azure Active Directory Role-based Access Control


Azure Roles-Based Access Control (RBAC) enables fine-grained access management for Azure. Users, groups, and applications can be granted access to manage resources in the Azure subscription, using Azure Management Portal, Azure Command-Line tools and Azure Management APIs.
Access is granted by assigning the appropriate RBAC role to users, groups, and applications, at the right scope. Roles can be asigned at a subscription level, resource group level or specific resource level.

Azure RBAC has three basic roles that apply to all resource types: Owner, Contributor and Reader. In addition you can use [RBAC Built in Roles](https://azure.microsoft.com/en-us/documentation/articles/role-based-access-built-in-roles/) for role specifies operations, or you can create customized policy.

Each subscription in Azure belongs to one and only one directory, each resource group belongs to one and only one subscription, and each resource belongs to one and only one resource group. Access that you grant at parent scopes is inherited at child scopes.

Classic administrators (service-admin and co-admins) have full access to the Azure subscription. In the RBAC model, classic administrators are assigned the Owner role at the subscription scope. The finer-grained authorization model (Azure RBAC) is supported only by the [new management portal](https://portal.azure.com) and Azure Resource Manager APIs.

For more information and instructions how to grant access using the Azure Management Portal, see: [Azure Active Directory Role-based Access Control](https://azure.microsoft.com/en-us/documentation/articles/role-based-access-control-configure/).

For granting access through command tools like Azure PowerShell, CLI or REST API see: [Managing access to resources](https://azure.microsoft.com/en-us/documentation/articles/resource-group-rbac/).


## logging and Audit operations with Resource Manager

https://azure.microsoft.com/en-us/documentation/articles/resource-group-audit/

## Security considerations for Azure Resource Manager

https://azure.microsoft.com/en-us/documentation/articles/best-practices-resource-manager-security/

## Lock resources with Azure Resource Manager

https://azure.microsoft.com/en-us/documentation/articles/resource-group-lock-resources/

## Use Policy to manage resources and control access (Customized policy)

https://azure.microsoft.com/en-us/documentation/articles/resource-manager-policy/ 




