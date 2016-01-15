> Azure Resource Manager Community Documentation - Beta Version

> Work in progress - This community driven documentation is considered to be in preview stage at this time. Documentation might contain errors, might not cover every aspect or might lack complete parts, even important parts. Please help us make this documentation better by [contributing](CONTRIBUTING.md) anything that you think would make it better.


---

# Azure Resource Manager Templates

Azure Resource Manager allows you to provision your applications using a declarative template. In a single template, you can deploy multiple services along with their dependencies. 
The same template can be used to repeatedly deploy your application during every stage of the application lifecycle.
You can also use the template for updates to the infrastructure; If the template specifies creating a new resource but that resource already exists,
Azure Resource Manager performs an update instead of creating a new asset. Azure Resource Manager updates the existing asset to the same state as it would be as new.

* [Templates Basic Concepts](Templates_Basics.md)
* [Resource Providers Schema](Template_resources_schema.md)
* [Templates Advanced Authoring](Template_Advanced_Authoring.md)
* [Template and Resources Links](Template_links.md)
* [Create Your First Template](My_First_Template.md)
* [Deploy a Template](Template_Deploy.md)
 
## Azure Resource Manager QuickStart Templates
The quickstart template gallery repository contains various templates, in all sorts of complexity levels you can use as-is for your deployments, or edit and extend to suit you individual needs.
[QuickStart Templates](https://github.com/Azure/azure-quickstart-templates)


