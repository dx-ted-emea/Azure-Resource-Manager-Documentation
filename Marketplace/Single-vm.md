## Single Virtual Machine 

This document dedicated to single virtual machine apps in Marketplace
*draft* 

Publishing single VM image is one of the easiest ways to publish your application in Azure Marketplace and this can be recommened as a first step for using Azure Marketplace for software vendors. 

## Common requirements

* Your VM image (aka SKU) should works on all existing ( and future) VM sizes – from A0 ( one shared CPU core, 0.75 GB RAM) up to G5 (32 CPU cores, 448 GB RAM ).
* Data disks can be as large as 1 TB. Data disk VHDs should be created as a fixed format VHD, but also be sparse. 
* When deciding on the disk size, please keep in mind that end users cannot resize VHDs within an image.
* Data disks can be empty or contain data

## Practical recommendation

It's recommened to create VM image for marketplace based on already published basic images from portal - it will simplify process a lot and seriously eliminate risks of any kernel components level issues. Using images from portal gallery ( basic OS images ) makes you sure that this image already have all kernel level components are properly installed and configured and what you need is just add your application layer components into that image. From practical side it means that you run VM in Azure portal based on OS which you would like to use, install your application, configure it, test, etc and do everything in Azure cloud - this is workflow makes everything much easier from practical point of view and help you to prevent struggle with very hard-to-find issues during production. Only in very few cases in might sense to create VM image based on-premise HyperV environment and in this case you have to be very carefull about configuration/versioning kernel level components/OS version and also in that case please be ready to spend some time on debugging your image in Azure cloud, digging deep into OS logs and other kernel level tasks.



## Linux VM

Before going deep in that area I would like to note that if you're creating images based on images from portal you can skip first steps and go directly to p.5 [Security tips]

### VHD requirements

Main requirements regarding VHD are quite simple:
   *  The Linux OS VHD in your VM Image should be created as a 30GB — 50GB fixed format VHD. 
   *  It cannot be less than 30GB. If the physical size is less than VHD size, the VHD should be sparse. 
   *  Linux VHDs larger than 50GB will be considered on a case by case basis. If you treat this as required please take a look onto [Mutli VM templates](Mutil-vm.md) - make be they will be better option for you.

Good reference regarding that topic is [Creating and Uploading a Virtual Hard Disk that Contains the Linux Operating System](https://azure.microsoft.com/en-us/documentation/articles/virtual-machines-linux-create-upload-vhd)

### Azure Linux agent ( waagent ) and required libs/packages 

The Azure Linux agent (waagent) provides key functions for deploying Linux IaaS deployment in Azure, such as image provisioning and networking capabilities. You get this agent from repos RPM/Deb packages  [Linux on Azure-Endorsed Distributions](https://azure.microsoft.com/en-us/documentation/articles/virtual-machines-linux-endorsed-distributions/) or from (github.com/Azure/WALinuxAgent)[https://github.com/Azure/WALinuxAgent]. 
If you took waagent from source make these steps to install waagent :
* copy 'waagent' file to /usr/sbin 
* chmod 755 /usr/sbin/waagent; /usr/sbin/waagent install
* wagent config is placed here: /etc/waagent.conf
* Don't forget about (waagent manual)[https://github.com/Azure/WALinuxAgent/blob/2.0/README]

Another libraries which you should take care about are :
* Linux Integration Services (LIS) driver should be installed, current version is v4 : (Linux Integration Services Version 4.0 for Hyper-V)[https://www.microsoft.com/en-us/download/details.aspx?id=46842]
* Python 2.6+  and pyasn1 ( Abstract Syntax Notation v1) package
* OpenSLL v1.0+
* Magical Kernel Patch for Azure I/O - usually included in latests distros from this list [Linux on Azure-Endorsed Distributions](https://azure.microsoft.com/en-us/documentation/articles/virtual-machines-linux-endorsed-distributions/), take care about non-listed kernels.




### Kernel and Logical Volume Manager (LVM) 
### Network and SSH daemon
### Security tips 
### Generalize image



Azure Marketplace is great place to publish your apps
Here's example of web link [Azure web site](http://azure.microsoft.com//) and the [Azure stuff on github](http://azure.github.io/).
Wiki also have *another font* which may be usefull too.


Example of some command line code 


    sudo apt-get install apache2

Example of some json code
```json
{ "some": "json" }
```

# Overview 

# Building the Offering 

# Examples 

# On-Boarding Process 


# Tip & Tricks 

* We highly recommend use basic images from Azure portal - this helps you not to struggle at least with OS/kernel level issues.
* Test VM before publishing in marketplace in different sizes of VM - minimally you need to test it on A0 (which is smallest configuration at that day), A12 and on some D/DS,G/GS-series.

# Resources and References 

### Marketplace and Azure certification

[Microsoft Azure Marketplace Publication Guidelines](aka.ms/am-guideline)

[Microsoft Azure Certified](https://azure.microsoft.com/en-us/marketplace/partner-program/)

### Linux 
[Windows Azure Linux Agent User Guide](https://azure.microsoft.com/en-us/documentation/articles/virtual-machines-linux-agent-user-guide/)

[Windows Azure Linux Agent distribution sources](https://github.com/Azure/WALinuxAgent)

[Creating and Uploading a Virtual Hard Disk that Contains the Linux Operating System](https://azure.microsoft.com/en-us/documentation/articles/virtual-machines-linux-create-upload-vhd/)

[Linux on Azure-Endorsed Distributions](https://azure.microsoft.com/en-us/documentation/articles/virtual-machines-linux-endorsed-distributions/)

### Tools

[Microsoft Azure Storage Explorer](http://storageexplorer.com/)

