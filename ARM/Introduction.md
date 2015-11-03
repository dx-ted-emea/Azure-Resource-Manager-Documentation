## Introduction to Azure Resource Manager

### Overview
In April 2015, Microsoft announced the public preview for template-based deployments of Azure resources using the Azure Resource Manager. With this release, building, deploying and managing large-scale complex applications was transformed into simple actions.
**Azure Resource Manager**, a.k.a. **ARM**, also referred to as **V2**, is a set of JSON-based APIs used to manage Azure resources (virtual machines, networks, storage etc.) This is an evolution from the previous approach, Azure Service Management (a.k.a ASM), also known as CLASSIC.

ARM present a different approach for deploying resources - instead of creating and managing individual resources, you are able to manage entire topologies of resources together as logical units, by constructing JSON template to deploy and manage all these resources.

> **Note that Classic mode is obsolete and going to be supported for legacy purposes only in the future.**

> **IaaS components deployed using ARM cannot be mixed with components created using the Classic APIs. This means that for  example, virtual machines deployed with the classic deployment model cannot be included in a virtual network deployed with  Resource Manager.**

> **Components deployed using the Classic APIs, will be presented in the Azure preview portal https://portal.azure.com under the "classic" label:**

![alt tag](/ARM/images/classic_arm_portal.png)

The adoption of ARM for IaaS moves the solution to IaaS v2 and introduces capabilities such as:
* Rich template based deployment including dependencies between objects in the template which are expressed in the template
* Simplification of deployment
* Role-based Access Control (RBAC)
* Tagging of resources for identification of resources and visibility during billing
* Parallel deployment of VMs

### Resource Manager Concepts
#### Resource Group
A resource group is a container that holds related resources for an application. The resource group could include all of the resources for an application, or only those resources that are logically grouped together. You can decide how you want to allocate resources to resource groups based on what makes the most sense for your organization.

#### Template deployment
With Resource Manager, you can create a simple template (in JSON format) that defines deployment and configuration of your application. This template is known as a Resource Manager template and provides a declarative way to define deployment. By using a template, you can repeatedly deploy your application throughout the app lifecycle and have confidence your resources are deployed in a consistent state.

#### Tags
Resource Manager provides a tagging feature that enables you to categorize resources according to your requirements for managing or billing. You might want to use tags when you have a complex collection of resource groups and resources, and need to visualize those assets in the way that makes the most sense to you. For example, you could tag resources that serve a similar role in your organization or belong to the same department.

#### Access control
Resource Manager enables you to control who has access to specific actions for your organization. It natively integrates OAuth and Role-Based Access Control (RBAC) into the management platform and applies that access control to all services in your resource group. You can add users to pre-defined platform and resource-specific roles and apply those roles to a subscription, resource group or resource to limit access. For example, you can take advantage of the pre-defined role called SQL DB Contributor that permits users to manage databases, but not database servers or security policies. You add users in your organization that need this type of access to the SQL DB Contributor role and apply the role to the subscription, resource group or resource.

#### Resource Providers
Resource providers are services that provide resources for your application. Using the resource provider APIs, you can manage, create, delete and update ARM resources.
ARM itself is a coordination layer across a set of underlying resource providers, and each resource provider defines the structure of the JSON that goes into the template document. A Network Interface is a resource exposed by Microsoft.Network Provider, same for all other resources. Each is exposed in a set of APIs. 

### Architecture
In the classic ASM model, virtual machines exist within a cloud service. Virtual machines are automatically provided with a network interface card (NIC) and an IP address assigned by Azure. Additionally, the cloud service contains an external load balancer instance, a public IP address, and default endpoints to allow remote desktop and remote PowerShell traffic for Windows-based virtual machines and Secure Shell (SSH) traffic for Linux-based virtual machines.

This is a diagram of a typical ASM deployment:
![alt tag](/ARM/images/asm_arch.png)

ARM model removes the use of the Cloud Service (PaaS still uses Cloud Services). Each resouce need to be created seperatly, using the appropriate resource provider.



### References and Resources
https://azure.microsoft.com/en-us/documentation/articles/resource-group-overview/
https://azure.microsoft.com/en-us/documentation/articles/resource-manager-supported-services/
https://azure.microsoft.com/en-us/documentation/articles/resource-manager-deployment-model/
https://azure.microsoft.com/en-us/documentation/articles/virtual-machines-azure-resource-manager-architecture/
https://azure.microsoft.com/en-us/documentation/articles/virtual-machines-azurerm-versus-azuresm/
http://windowsitpro.com/azure/what-are-key-differences-iaas-v2-enabled-through-azure-resource-manager

