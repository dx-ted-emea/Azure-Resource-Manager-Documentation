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
