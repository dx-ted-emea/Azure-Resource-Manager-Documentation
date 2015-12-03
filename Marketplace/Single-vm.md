## Single Virtual Machine 

This document dedicated to single virtual machine apps in Marketplace


Publishing single VM image is one of the easiest ways to publish your application in Azure Marketplace and this can be recommened as a first step for using Azure Marketplace for software vendors. 

[Common requirements]
[Practical recommendations]
[Linux VM]
[Windows Server VM]
[Test your image]

## Common requirements

* Your VM image (aka SKU) should works on all existing ( and future) VM sizes – from A0 ( one shared CPU core, 0.75 GB RAM) up to G5 (32 CPU cores, 448 GB RAM ).
* Data disks can be as large as 1 TB. Data disk VHDs should be created as a fixed format VHD, but also be sparse. 
* When deciding on the disk size, please keep in mind that end users cannot resize VHDs within an image.
* Data disks can be empty or contain data

## Practical recommendations

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
*  SWAP space (if it needed ) can be created on the local resource disk with the Linux Agent by enable swap in  /etc/waagent.conf. It will automatically use the resource disk (which comes with every VM) to create the swap. There's no need to create a disk for it. It's highly recommended to put SWAP space onto temp drive because of performance reasons : more details see [Linux in Azure](http://bokov.net/weblog/azure/configure-linux-in-azure).
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

After you made all changes and already have generalized VHD ( please see ) - you may go to [Test your image] section.


## Windows Server VM

### Base image for virtual machine

The OS VHD for your VM Image must be based on a Microsoft Azure-approved base image, containing Windows Server or SQL Server.
Best thing to begi is create a VM from one of the following images, located at the Microsoft Azure Portal (portal.azure.com), currently basic images are ( you always can see updates list in Azure portal - or just use default Windows Server image ):
* Windows Server 2012 R2 Datacenter, 2012 Datacenter, 2008 R2 SP1
* SQL Server 2014 Enterprise/Standard/Web
* SQL Server 2012 SP2 Enterprise/Standard/Web
* SQL Server 2008 R2 SP2 Enterprise/Standard/Web
These links can also be found in the Publishing Portal under the SKU page.
Main idea for this requirement is to use well-patched and updated Windows Server kernel, currently this requirement means thatt you may use  Windows Server Images published after September 8, 2014 :
![Azure publisher portal](/Marketplace/images/azure-publisher-portal.png)

#### Create Windows Server VM image

Actually what you do is create VM under Azure portal, that’s all.
Hints:
* Choose US-* region for deployment, it would helps during certification process because when you will submit your image for certification team
* We highly recommend to do all thing in cloud, create/customize/configure VM on-premise under Hyper-V technically correct and will work if you follow documentation, but we don’t recommend it in most cases. Reality is that using on-premise for this purpose makes whole process much longer and brings very hard-to-find issues when VM is finally goes to cloud.

#### Customize your Windows Server VM using RDP

For those who not yet familiar with Azure portal here's place where RDP connection can be initiated - just click on that button
![RDP connect](/Marketplace/images/rdp-connect-button-azure-portal.png)

In case if your PC under domain don't forget to add '/' in the beginning, unless you will try to connect with your domain credentials ( and in most cases you'll fail, unless you not specially configure your AD for that purpose :-))
![RDP credentials hint for domain users](/Marketplace/images/rdp-cred-hint.png)

If you by some reason would like to use command line you can that for opening RDP as well: 
You can use powershell to access your VM (we expect that you already have RDP file in c:\tools )
```
Get-AzureAccount
Get-AzureVM
Get-AzureRemoteDesktopFile -ServiceName "abokov-ws2012DC" -Name "abokov-ws2012DC" -LocalPath "C:\tools\abokov-ws2012DC.rdp"
```

#### Configure Windows Server VM

*The Windows OS VHD in your VM Image should be created as a 128 GB fixed format VHD. If the physical size is less than 128GB, the VHD should be sparse. Base images of recommended Windows Server are already meet this, just don’t charge defaults.
*Install patches, especially critical and security
* *important* No configuration should rely on drives other than C:\ or D:\, since these are the only two drives that are always guaranteed to exist. C:\ is the OS disk and D:\ is the temporary local disk.
* Please don’t keep your Azure credentials inside images

#### Generalize Windows Server VM

Windows images should be sysprep’ed  - run command line ( not PowerShell! ), change directory to “c:\windows\system32\sysprep”
* “sysprep.eex /generalize /oobe /shutdown”
* Please be aware Remote Desktop Connection will be closed immediately
*  Wait for generalize and shutdown…
![Sysprep dialog](/Marketplace/images/sysprep-windows-server-vm-azure.png)


# Test your image

#### Link to generalized VHD

After this process will be finished your generalized images will be in your VM VHD. For example in current portal you may find that link to VHD  here :
![Your VM VHD link](/Marketplace/images/azure-portal-link-to-vhd.png)


#### How to deploy VM from generalized VHD

After your VHD is generalized is ready to be published in Marketplaced, but it really makes sense to do tests before this images goes to other user - so somehow you need to run VM on your just prepated VHD images and check how it works in different scenarios and this related to both Windows Server-based and Linux-based VHDs.
Ok, so generalized OS VHD from Azure storage account can be registered as a user VM Image which you might test, but thing is that you cannot directly deploy the VM just by providing generalized VHD URL. You need to use the Create VM Image Rest API to register VHDs as a VM Image to run it. There’s two options for that: [Invoke-WebRequest] ( direct calls to REST API ) or [Save-AzureVMIMage] ( very convenient powershell cmdlet - recommended for most of users ).

#### Save-AzureVMImage

This cmdlet allow you easily create VM image based on generalized VHD - for more information you migh refer to [VM Image](https://azure.microsoft.com/en-us/blog/vm-image-blog-post/)

```
Save-AzureVMImage –ServiceName “myServiceName” –Name “myVMtoCapture” –OSState “Generalized” –ImageName “myAwesomeVMImage” –ImageLabel “This is my Virtual Machine Image” -Verbose
```
Next section is about how to do almost the same things by direct REST API calls, if you good with Save-AzureVMImage (as 99.95% of users :-)) feel free to skip that part and go to [Where's my image]

#### Invoke-WebRequest


You can use the Invoke-WebRequest cmdlet to create a VM image from command line and example below is about how to create a VM image with an OS and one data disk, this example is taken from that [Guide to create a virtual machine image for the Azure Marketplace](https://azure.microsoft.com/sv-se/documentation/articles/marketplace-publishing-vm-image-creation/) - you might also need to take a look on that post in case if you need additional changes. 

```
# Image Parameters to Specify
            $ImageName='myVMImage'
            $Label='IMAGE_LABEL'
            $Description='My VM Image to Test'
            $osCaching='ReadWrite'
            $os = 'Windows'
            $state = 'Generalized'
            $osMediaLink = 'http://mystorageaccount.blob.core.windows.net/vhds/myOSvhd.vhd'
            $dataCaching='None'
            $lun='1'
            $dataMediaLink='http://mystorageaccount.blob.core.windows.net/vhds/mydatavhd.vhd'
            # Subscription Related Properties
            $SrvMngtEndPoint='https://management.core.windows.net'
            $subscription = Get-AzureSubscription -Current -ExtendedDetails
            $certificate = $subscription.Certificate
            $SubId = $subscription.SubscriptionId
            $body =
            "" +
                "" + $ImageName + "" +
                "" + $Label + "" +
                "" + $Description + "" +
                "" +
                    "" + $osCaching + "" +
                    "" + $state + "" +
                    "" + $os + "" +
                    "" + $osMediaLink + "" +
                "" +
                "" +
                        "" +
                        "" + $dataCaching + "" +
                        "" + $lun + "" +
                        "" + $dataMediaLink + "" +
                        "" +
                "" +
            ""
            $uri = $SrvMngtEndPoint + "/" + $SubId + "/" + "services/vmimages"
            $headers = @{"x-ms-version" = "2014-06-01"}
            $response = Invoke-WebRequest -Uri $uri -ContentType "application/xml" -Body $body -Certificate $certificate -Headers $headers -Method POST
            if ($response.StatusCode -ge 200 -and $response.StatusCode -lt 300)
            {
                echo "Accepted"
            }
            else
            {
                echo "Not Accepted"
            }

```


#### Where's my image

After you create your VM image based on generalized image you may run it from portal or from command line. In Azure portal you may find you image here :
![VM image](/Marketplace/images/azure-portal-vm-image.png)

_todo_ : command_line_reference



# Publish image in Marketplace

After testing is done and you ready to publish your offer you need to let us to know where your VHD is - marketplace team will took this images and process it through publication process. Note: you need to share your _generalized_ VHD. In publisher portal interface there's a field where you need to put a link on your VHD :

![VM image link](/Marketplace/images/azure-publisher-portal-vm-image-link.png)

Thing is what by default you VHD images inside your storage accounts are not accessible outside of your subscription, so to give access to that particular VHD to us you need to change default settings and make this VHD read-only for others. Recommened way for that is to generate temporary read-only credentials - Shared Access Signatures Uniform Resource Identifier  ( SAS URI - actually it looks like URL with params ) which will have some restrictions in time ( we recommend set up that period to minimum 7 business days ) and use this credentials in that dialog in publisher portal.
The SAS URI created should adhere to the following requirements :
* When generating SAS URIs for your VHDs, List and Read-Only permissions are sufficient. Do not provide Write or Delete access.
* The duration for access should be a minimum of 7 business days from when the SAS URI is created.
* To avoid immediate errors due to clock skews, specify a time 15 minutes before the current time.

Simplest way to do that is to use open sourced (AzureStorageExplorer tool)[http://AzureStorageExplorer.codeplex.com].
















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

v# Building the Offering 

# Examples 

# On-Boarding Process 


# Tip & Tricks 

* We highly recommend use basic images from Azure portal - this helps you not to struggle at least with OS/kernel level issues.
* Test VM before publishing in marketplace in different sizes of VM - minimally you need to test it on A0 (which is smallest configuration at that day), A12 and on some D/DS,G/GS-series.

# Resources and References 

### Marketplace and Azure certification

[Microsoft Azure Marketplace Publication Guidelines](aka.ms/am-guideline/)

[Microsoft Azure Certified](https://azure.microsoft.com/en-us/marketplace/partner-program/)

[Guide to create a virtual machine image for the Azure Marketplace](https://azure.microsoft.com/sv-se/documentation/articles/marketplace-publishing-vm-image-creation/)

### Virtual Machines 

[VM Image](https://azure.microsoft.com/en-us/blog/vm-image-blog-post/) - great post about how to create/use/save/etc virtual machine images.

### Linux 
[Windows Azure Linux Agent User Guide](https://azure.microsoft.com/en-us/documentation/articles/virtual-machines-linux-agent-user-guide/)

[Windows Azure Linux Agent distribution sources](https://github.com/Azure/WALinuxAgent)

[Creating and Uploading a Virtual Hard Disk that Contains the Linux Operating System](https://azure.microsoft.com/en-us/documentation/articles/virtual-machines-linux-create-upload-vhd/)

[Linux on Azure-Endorsed Distributions](https://azure.microsoft.com/en-us/documentation/articles/virtual-machines-linux-endorsed-distributions/)

### Tools

[Microsoft Azure Storage Explorer](http://storageexplorer.com/)


