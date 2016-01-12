> Azure Resource Manager Community Documentation - Beta Version

> Work in progress - This community driven documentation is considered to be in preview stage at this time. Documentation might contain errors, might not cover every aspect or might lack complete parts, even important parts. Please help us make this documentation better by [contributing](CONTRIBUTING.md) anything that you think would make it better.


---

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

