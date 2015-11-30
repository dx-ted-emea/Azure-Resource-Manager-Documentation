# Azure Resource Manager Templates

Azure Resource Manager allows you to provision your applications using a declarative template. In a single template, you can deploy multiple services along with their dependencies. 
The same template can be used to repeatedly deploy your application during every stage of the application lifecycle.
You can also use the template for updates to the infrastructure; If the template specifies creating a new resource but that resource already exists,
Azure Resource Manager performs an update instead of creating a new asset. Azure Resource Manager updates the existing asset to the same state as it would be as new.

* [Templates Basic Concepts](Templates_Basics.md)
* [Resource Provider Schema](resources_schema.md)
* [Templates Advanced Authoring](Template_Advanced_Authoring.md)
* [Create Your First Template](My_First_Template.md)
* [Deploy a Template](Template_Deploy.md)
 
## Azure Resource Manager QuickStart Templates
The quickstart template gallery repository contains various templates, in all sorts of complexity levels you can use as-is for your deployments, or edit and extend to suit you individual needs.
[QuickStart Templates](https://github.com/Azure/azure-quickstart-templates)

