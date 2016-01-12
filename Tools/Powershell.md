> Azure Resource Manager Community Documentation - Beta Version

> Work in progress - This community driven documentation is considered to be in preview stage at this time. Documentation might contain errors, might not cover every aspect or might lack complete parts, even important parts. Please help us make this documentation better by [contributing](CONTRIBUTING.md) anything that you think would make it better.


---

## Tools for ARM - Powershell 

The Microsoft Azure Powershell Modules allow to manage Azure Resources with Powershell cmdlets and scripts.

Instructions on how to setup Azure Powershell are available here [[TODO add link]](https://azure.microsoft.com/en-us/documentation/articles/powershell-install-configure/ "Azure Powershell")

Starting from Azure Powershell version 1.0, all Resource Manager commands are in specific modules in **AzureRM.***

Most AzureRM commandlets allow piping and do output objects.

To get a list of all AzureRM cmdlets type:

```powershell
    Get-Help AzureRM
```

Detailed help for each command is available by typing

```powershell
    Get-Help [command] -Detailed
```

In example:

```powershell
    Get-Help New-AzureRmResourceGroup -Detailed
```

...will display detailed help and examples for the command to create a Resource Group

A good way to develop and test Powershell scripts is to use the Powershell Interactive Scripting Environment, available in your start menu (search for "Powershell ISE")

Once Azure PowerShell is up and running, to leverage it with ARM templates, either stored locally or on publicly accessible http urls, you will need to authenticate to your Azure Subscription and select the active one, if you can manage many of them with the login information you are using

In order to perform a Login to the Azure Resource Manager APIs, type

```powershell
	Login-AzureRmAccount
```

This will display a code that you need to enter on the url [http://aka.ms/devicelogin](http://aka.ms/devicelogin "http://aka.ms/devicelogin") in order to complete the authentication

If you need to perform a "silent" login, you need to use an Azure Active Directory login by the following commands

```powershell
    $credential=Get-Credential
	Login-AzureRmAccount -Credential $credential
```

If you have more than one azure subscription, you can select the subscription by typing

```powershell
    Select-AzureRmSubscription -SubscriptionName [your-subscription-name]
```

or

```powershell
	Select-AzureRmSubscription -SubscriptionId [your-subscription-id]
```

Now you are all set to use the Azure Resource Manager Command to deploy resources in Azure

First, you can use the following command to create a Resource Group:

```powershell
    New-AzureRmResourceGroup -Name [your-resource-group-name] `
        -Location [an-azure-location]
```

Adding the -Tag option with a sequence of tags, will define tags in the resource group that can be used to track billing in your Azure billing reports as in the following example:

```powershell
    New-AzureRmResourceGroup -Name [your-resource-group-name] `
        -Location [an-azure-location] `
        -Tag tag1=value1;tag2=value2
```

Now you are ready to create resources in the resource group by using your deployment templates and eventually your preset parameter files with the following command

```powershell
    New-AzureRmResourceGroupDeployment -Name [deployment-name] `
            -ResourceGroupName [your-resource-group-name] `
            -TemplateFile [path-to-the-arm-template-local-json-file] `
            -TemplateParameterFile [path-to-the-arm-template-local-json-file]
```

As another option, you can provide the parameters interactively, by omitting the --parameters-file option or inline with a json formatted string as in the following example:

```powershell
	New-AzureRmResourceGroupDeployment -Name [deployment-name] `
        -ResourceGroupName [your-resource-group-name] `
        -TemplateFile [path-to-the-arm-template-local-json-file] `
        -TemplateParameterObject [json-formatted-parameter-string]
```

If you need to delete a deployment in a Resource Group you can type:

```powershell
    Remove-AzureRmResourceGroupDeployment -Name [deployment-name] `
        -ResourceGroupName [your-resource-group-name]
```

If you need to the whole Resource Group you can type:

```powershell
    Remove-AzureRmResourceGroup -Name [your-resource-group-name]
```
