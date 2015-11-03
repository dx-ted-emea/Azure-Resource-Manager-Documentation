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
### Azure Linux agent ( waagent )  and required libs/packages 
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

# Resources and References 
