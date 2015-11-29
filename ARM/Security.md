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
You can add users to pre-defined platform and resource-specific roles and apply those roles to a subscription, resource group or resource to limit access. For example, you can take advantage of the pre-defined role called SQL DB Contributor that permits users to manage databases, but not database servers or security policies. You add users in your organization that need this type of access to the SQL DB Contributor role and apply the role to the subscription, resource group or resource.

In the preview portal, you can define access control by clicking on the access button.

Azure Resource Manager automatically logs user actions for auditing.



## Azure Active Directory Role-based Access Control

https://azure.microsoft.com/en-us/documentation/articles/role-based-access-control-configure/


## Managing access to resources

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



