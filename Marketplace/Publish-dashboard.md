# Publisher Dashboard in Azure Marketplace

*draft*

This page covers the steps required for the Azure Marketplace Publisher Dashboard. The publisher dashboard manages the offers for [virtual machines (single VM)](Single-vm.md), [solution templates (multi-VM)](Multi-vm.md), [developer services](Developers-services.md), and [data services](Data-services.md). An offer in the publisher dashboard includes the following:

- publishers information
- description of your software
- support information
- marketplace categories
- pricing information
- sell-to countries
- samples images 

This page does not cover the development required to set up or build any software system. For information on the development of the different services please visit the page for that service.

### Pre-Requisites

For offers that will include a charge for software above the billed Azure infrastructure cost ensure the [Seller Dashboard](Seller-Dashboard.md) steps have been followed. Offers with no seller charge or a Bring Your Own Licence model do not require seller registration. 

## Registration

Visit https://publish.windowsazure.com/ with a browser. The URL will redirecte to a default [Microsoft Account](http://windows.microsoft.com/en-GB/windows-live/sign-in-what-is-microsoft-account) sign in page.

![Microsoft Account Default Sign-in Page](images/standard-sign-in-page.png)

If a seller account has been configured as mentioned above use this account to login to the publishing portal.

In the instance that no seller account is being used as there are no plans to charge for an offer via the marketplace sign in with a [Microsoft Account](http://windows.microsoft.com/en-GB/windows-live/sign-in-what-is-microsoft-account).

On the first sign in to the publishing portal the Microsoft Azure Publishing Agreement needs to be read and agreed to.

# Marketplace Publisher Types

There are four options available:

- [Developer Service](Developers-services.md)
    - Provide access to an API. Targeted at developers only.
    - *Note (Nov 2015)*: fixed monthly billing is currently the only available billing option.
- [Virtual Machine](Single-vm.md)
    - Single Virtual Machine. Generally an independent program that can be part of a larger infrastructure deployment, e.g. a virtual networking appliance, software rendering. 
- [Data Service](Data-services.md)
    - Data offered for consumption by other services. 
- [Solution Template](Multi-vm.md)
    - A complete system deployment, including Virtual Machines, networking, databases, public IP addresses and configuration to ensure components can connect with each other. 
    - *Note (Nov 2015)*: Currently requires approval to publish.


![Azure Marketplace Publisher Portal Homepage](images/azure-publisher-portal-homepage.png)

The steps for each of these options are similar in the publisher portal. 

## Virtual Machine

Select 'virtual machines' from the sidebar. Enter a title for the Virtual Machine Marketplace Offer and press 'Enter'. This title will be the title shown on the marketplace for your offer, it can be changed later (*Offer* -> *Marketing* -> *Languages* -> *Details* -> *Title*). An overview of how each field is displayed in the marketplace for an offer is [below](#offer-layout).

![Azure Marketplace Virtual Machine Overview](images/azure-publisher-portal-virtual-machine-overview.png)

**Step 1** is to apply to be a Azure Certified for your publisher account. Follow the link, http://azure.com/certified, and the directions on the page to contact Microsoft and be verified.

**Step 2** register the company on the [seller dashboard](Seller-dashboard.md). The third sub-step is to fill in company information that will appear on all the marketplace offers created in this account. This step is also explained below in the [publishers](publishers) section.

**Step 3** is to define the SKU for the offer. An offer can have multiple SKUs, each with their own Virtual Machine image and pricing configurations.

### SKU

Create a SKU new SKU selecting 'SKU' in the sub-menu for the Virtual Machine offer. Select to 'ADD A SKU' and enter a title for the SKU. If the SKU will not carry a charge on top of the Azure Infrastructure charge select the checkbox 'Billing and licensing is done externally from Azure'

![Azure Marketplace Virtual Machine Offer SKU Title](images/azure-publisher-portal-virtual-machine-sku.png)

Once the SKU is the option to hide the SKU is shown, for a Virtual Machine offer ensure the 'No' option is selected.

![Azure Marketplace Publisher Portal Hide Virtual Machine ](images/azure-publisher-portal-solution-template-option.png)

### VM Images

Follow the instructions to [build a VM](Single-vm.md). Once a VM is built and self-certified, click on 'VM Images' in the sidebar under the Virtual Machine offer. For each SKU select the Operating System Family (i.e. Windows or Linux). Enter the name of the Operating System, e.g. 'Ubuntu 14.04 LTS'.

#### Recommended VM Sizes

End users will be shown the recommended VM sizes first in the marketplace listing when selecting the offer. If the offer functions best with a certain number of cores, RAM, or premium storage, consider this when selecting the options here. 

#### Open Ports

This section specifies the public and private port mappings as well as the protocol for your VM. 

By default a VM has the following ports opened:

- Windows: 3389 -> 3389 : TCP
- Windows: 5986 -> 5986 : TCP
- Linux: 22 -> 22 : TCP

These mappings can be overridden. 

#### Virtual Machine Images

Each update of a VM requires an increment to the version number. The version starts at 1.0.0 by default. A SKU is expected to only receive minor patch updates, i.e. 1.0.0 -> 1.0.1. If a major update is being released a new SKU is the recommended practice.

The SAS URI that has been generated as part of the [single VM](Single-vm.md) steps for the OS Disk should be entered into the field for the OS VHD URL. Additional data disks as needed should also have their SAS URIs added.

The self-certification test results explained in the [single VM](Single-vm.md) steps should also be uploaded here by selecting the 'Upload Test Results' button. 

Once each of these steps are complete select 'Request Certification'. This will inform the Azure Marketplace team that the SKU and VM are ready to be certified for the Azure Marketplace. 

![Azure Publisher Portal Virtual Machine Image SKU](images/azure-publisher-portal-virtual-machine-image-sku.png)

## Solution Template

Select 'solution templates' from the sidebar. Enter a title for the Solution Template and press 'Enter'. This title will be the title shown on the marketplace for your offer, it can be changed later (*Offer* -> *Marketing* -> *Languages* -> *Details* -> *Title*). An overview of how each field is displayed in the marketplace for an offer is [below](#offer-layout).

## Developer Service

Select 'developer services' from the sidebar. Enter a title for the service and press 'Enter'. This title will be the title shown on the marketplace for your offer, it can be changed later (*Offer* -> *Marketing* -> *Languages* -> *Details* -> *Title*). An overview of how each field is displayed in the marketplace for an offer is [below](#offer-layout).

![Azure Marketplace Publisher Portal Developer Services Title](images/azure-publisher-portal-dev-service-title.png)

Once a service is created the portal will provide an overview of every step of the process.

![Azure Marketplace Publisher Portal Developer Services Overview](images/azure-publisher-portal-dev-service-overview.png)

**Step 1** is to apply to be a Azure Certified for your publisher account. Follow the link, http://azure.com/certified, and the directions on the page to contact Microsoft and be verified.

**Step 2** register the company on the [seller dashboard](Seller-dashboard.md). The third sub-step is to fill in company information that will appear on all the marketplace offers created in this account. This step is also explained below in the [publishers](publishers) section.

**Step 3** 

## Data Service


# <a name="publishers"></a>Publishers

Publisher Name space

## <a name="offer-layout"></a>Layout of a Published Marketplace Offer

What entries make up the URL?

# Promotions

What is a promotion? How do you set one up?