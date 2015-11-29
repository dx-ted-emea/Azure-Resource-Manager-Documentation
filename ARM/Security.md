## Security and Authentication 

*draft*

Azure Marketplace is great place to publish your apps 
Here's example of web link [Azure web site](http://azure.microsoft.com//) and the [Azure stuff on github](http://azure.github.io/).
Wiki also have *another font* which may be usefull too.


Example of some command line code 

    sudo apt-get install apache2

Example of some json code
```json
{ "some": "json" }
```



# Access Control 

Azure Resource Manager enables you to control who has access to specific actions on your resources. It natively integrates OAuth and Role-Based Access Control (RBAC) into the management platform and applies that access control to all services in your resource group. 
You can add users to pre-defined platform and resource-specific roles and apply those roles to a subscription, resource group or resource to limit access. For example, you can grant a user access to a specific virtual machine in a subscription, or give a user the ability to manage all websites in a subscription but no other resources.


## Azure Active Directory Role-based Access Control


Azure Roles-Based Access Control (RBAC) enables fine-grained access management for Azure. Users, groups, and applications can be granted access to manage resources in the Azure subscription, using Azure Management Portal, Azure Command-Line tools and Azure Management APIs.
Access is granted by assigning the appropriate RBAC role to users, groups, and applications, at the right scope. Roles can be asigned at a subscription level, resource group level or specific resource level.

Azure RBAC has three basic roles that apply to all resource types: Owner, Contributor and Reader. In addition you can use [RBAC Built in Roles](https://azure.microsoft.com/en-us/documentation/articles/role-based-access-built-in-roles/) for role specifies operations, or you can create customized policy.

Each subscription in Azure belongs to one and only one directory, each resource group belongs to one and only one subscription, and each resource belongs to one and only one resource group. Access that you grant at parent scopes is inherited at child scopes.

Classic administrators (service-admin and co-admins) have full access to the Azure subscription. In the RBAC model, classic administrators are assigned the Owner role at the subscription scope. The finer-grained authorization model (Azure RBAC) is supported only by the [new management portal](https://portal.azure.com) and Azure Resource Manager APIs.

For more information and instructions how to grant access using the Azure Management Portal, Azure Command-Line tools and Azure Management APIs see: [Azure Active Directory Role-based Access Control](https://azure.microsoft.com/en-us/documentation/articles/role-based-access-control-configure/)


## Managing access to resources

Granting a user with specific permissions can be done through the preview portal or through command tools like Azure PowerShell, CLI or REST API.

https://azure.microsoft.com/en-us/documentation/articles/resource-group-rbac/

## logging and Audit operations with Resource Manager

https://azure.microsoft.com/en-us/documentation/articles/resource-group-audit/

## Security considerations for Azure Resource Manager

https://azure.microsoft.com/en-us/documentation/articles/best-practices-resource-manager-security/

## Lock resources with Azure Resource Manager

https://azure.microsoft.com/en-us/documentation/articles/resource-group-lock-resources/

## Use Policy to manage resources and control access (Customized policy)

https://azure.microsoft.com/en-us/documentation/articles/resource-manager-policy/ 


# Authentication 

## Authenticating a service principal with Azure Resource Manager

https://azure.microsoft.com/en-us/documentation/articles/resource-group-authenticate-service-principal/

## Create Active Directory application and service principal using portal

https://azure.microsoft.com/en-us/documentation/articles/resource-group-create-service-principal-portal/



