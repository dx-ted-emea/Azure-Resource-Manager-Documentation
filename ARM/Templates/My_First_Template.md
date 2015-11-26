# Create Your First Resource Manager Template

Lets look at a very simple architecture:
* Two virtual machines that use the same storage account, are in the same availability set, and on the same subnet of a virtual network.
* A single NIC and VM IP address for each virtual machine.
* An external load balancer that distributes Internet traffic to the NICs of the two virtual machines.

![alt tag](../ARM/images/arm_arch.png)
