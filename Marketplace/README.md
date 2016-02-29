> Azure Resource Manager Community Documentation - Beta Version

> Work in progress - This community driven documentation is considered to be in preview stage at this time. Documentation might contain errors, might not cover every aspect or might lack complete parts, even important parts. Please help us make this documentation better by [contributing](CONTRIBUTING.md) anything that you think would make it better.

---
The Azure Marketplace enables developers to publish their software and services for consumption by Azure subscribers. 
The Azure Marketplace publishing options include a single virtual machine (VM), a solution template (also known as 
multi-VM), data services, VM extensions, and Software-as-a-Service (SaaS) developer access. 

Publishing on the Azure Marketplace requires [certification](http://azure.com/certified). Certification ensures 
published offerings will deploy on Azure infrastructure. It is not a substitute for testing during the publishing
process.

To learn more about publishing in the Azure Marketplace, follow the [Marketplace getting started guide](https://azure.microsoft.com/en-us/documentation/articles/marketplace-publishing-getting-started/)

## Billing via Azure Marketplace

Single VM and Solution Template offerings can be billed via the Azure marketplace based on usage per minute. Charges can
be set on a per core-hour basis, enabling different charges based on the size of infrastructure the VM is deployed to. 
To charge for a Solution Template a single VM must be included that has billing configured. Publishers receive 80% of the 
software charge i.e. the normal infrastructure cost billed to the end-user does not impact the amount a publisher is paid.

VMs can also be published with a Bring Your Own Licence (BYOL) model, in this instance no charge for the software 
published on the VM is added to the infrastructure cost. BYOL can be licenced outside of the marketplace, e.g. via 
a key purhcased from the publishers website, or may not require any purchase or licence checks, e.g. open-source software.

## Software Updates

To update Single Virtual Machine, Solution Templates,  and VM Extension offers in the marketplace a new version of the 
offer must be built, re-certified (per the publishing steps of the offer), and published. This will update the offer
available in the marketplace. This does *not* update the offers that an end-user has already deployed into their own
Azure subscription. To update already deployed software on Virtual Machines the software must utilise its own update
mechanism.

# [Azure Marketplace FAQs](Marketplace-FAQ.md)

