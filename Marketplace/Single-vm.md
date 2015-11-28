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
* Don't forget about [waagent manual](https://github.com/Azure/WALinuxAgent/blob/2.0/README)

Another libraries which you should take care about are :
* Linux Integration Services (LIS) driver should be installed, current version is v4 : [Linux Integration Services Version 4.0 for Hyper-V](https://www.microsoft.com/en-us/download/details.aspx?id=46842)
* Python 2.6+  and pyasn1 ( Abstract Syntax Notation v1) package
* OpenSLL v1.0+
* Magical Kernel Patch for Azure I/O - usually included in latests distros from this list [Linux on Azure-Endorsed Distributions](https://azure.microsoft.com/en-us/documentation/articles/virtual-machines-linux-endorsed-distributions/), take care about non-listed kernels.


### Kernel configuration and Logical Volume Manager (LVM) 

* OS has to be placed on single root partition
*  SWAP space (if it needed ) can be created on the local resource disk with the Linux Agent by enable swap in  /etc/waagent.conf. It will automatically use the resource disk (which comes with every VM) to create the swap. There's no need to create a disk for it. It's highly recommended to put SWAP space onto temp drive because of performance reasons : more details see [Cool things with Linux in Azure](http://bokov.net/weblog/azure/configure-linux-in-azure).
* Serial console output must be always enabled even if you not allow any SSH to your VM ( and our support may provide you output from serial console )
*  Add good enough timeout for mounting cloud based storage device
*  Add this to kernel boot line “console=ttyS0 earlyprintk=ttyS0 rootdelay=300”

Do not :
* Do not use LVM Logical Volume Manager
* Do not use swap on OS or data disk


### Network and SSH daemon
We recommend enable SSH for the end user, add keep live into sshd_config by ClientAliveInterval settings - acceptable range of ClientAliveInterval is 30 to 235, recommended 180. Networking configuration should use ifcfg-eth0 file and manage it  via the ifup/ifdown. Also please make sure that network device is brought up on boot and uses DHCP
Do not:
* Do not install Network Manager package - it conflicts with waagent.
* No custom network configuration and resolv.conf file ( please “rm /etc/resolv.conf” ).
* Do not configure IPv6 – it’s not supported yet.

### Security tips 
Do classics : 
* install all security patches for your distribution ( sudo apt-get update;sudo apt-get upgrade) / follow distribution security guidelines / clean up bash history
P* lease take care about root - the image should not contain a root password  (!!!!!!) – delete it and check /etc/shadow and /etc/passwd.
* Add firewall i.e. include iptables, but do not enable any rules – default expectation from customer is that they may easily enable it right after VM is started.
Do not:
* Store your Azure account credentials on VM image (!!!!!)
* Do not create default accounts, which remain the same, across provisioning instances


### Generalize image

OS VHD must be deprovisioned: “waagent deprovision”. This command does:
*Removes the nameserver configuration in /etc/resolv.conf
*Removes cached DHCP client leases
*Resets host name to localhost.localdomain
We recommend setting /etc/waagent.conf to ensure the following actions are also completed:
*Remove all SSH host keys:    Provisioning.RegenerateSshHostKeyPair='y'
*Remore root password from /etc/shadow : Provisioning.DeleteRootPassword='y‘

#### Example of /etc/waagent.conf 
```
# Azure Linux Agent Configuration   
Role.StateConsumer=None 
Role.ConfigurationConsumer=None 
Role.TopologyConsumer=None
Provisioning.Enabled=y
Provisioning.DeleteRootPassword=n
Provisioning.RegenerateSshHostKeyPair=y
Provisioning.SshHostKeyPairType=rsa
Provisioning.onitorHostName=y
ResourceDisk.Format=y
ResourceDisk.Filesystem=ext4
ResourceDisk.MountPoint=/mnt/resource 
ResourceDisk.EnableSwap=n 
ResourceDisk.SwapSizeMB=0
LBProbeResponder=y
Logs.Verbose=n
OS.RootDeviceScsiTimeout=300
OS.OpensslPath=None
```




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

