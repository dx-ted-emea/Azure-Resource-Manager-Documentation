## Tools for ARM - Command Line Interface 

The Azure Command Line Interface (CLI) is a cross platform shell that runs both on Windows and Linux.

Instructions on how to setup the Azure CLI are available here: [https://azure.microsoft.com/en-us/documentation/articles/xplat-cli-install/](https://azure.microsoft.com/en-us/documentation/articles/xplat-cli-install/ "Windows / Linux / OS X")

As Azure CLI can output JSON objects as operation results, it is usually a nice idea to use it in combination with JSON parsers as [https://stedolan.github.io/jq/download/](https://stedolan.github.io/jq/download/ "JQ")

In order to use the Azure CLI with ARM, the first command that one needs to run is:
    
	azure config mode arm

By typing
	
	azure

...you will now be able to see the list of commands available.

Help for each command is available by adding the --help option to the command itself, in example

    azure group --help

will display help for commands related to Resource Groups

Once your CLI is up and running, to leverage it with ARM templates, either stored locally or on publicly accessible http urls, you will need to authenticate to your Azure Subscription and select the active one, if you can manage many of them with the login information you are using

In order to perform a Login to the Azure Resource Manager APIs, type

	azure login

This will display a code that you need to enter on the url [http://aka.ms/devicelogin](http://aka.ms/devicelogin "http://aka.ms/devicelogin") in order to complete the authentication

If you need to perform a "silent" login, you need to use an Azure Active Directory login by the following command
    
	azure login --username [your-azure-ad-login-name] --password [your-azure-ad-login-password]

If you have more than one azure subscription, you can select the subscription by typing

    azure account set [your-subscription-name or your-subscription-id]

Now you are all set to use the Azure Resource Manager Command to deploy resources in Azure

First, you can use the following command to create a Resource Group:
    
	azure group create --name [your-resource-group-name] --location [an-azure-location]

Adding the --tags option with a sequence of tags, will define tags in the resource group that can be used to track billing in your Azure billing reports as in the following example:

	--tags tag1=value1;tag2=value2

Possible Azure Locations can be gathered by typing:

	azure location list`

Now you are ready to create resources in the resource group by using your deployment templates and eventually your preset parameter files with the following command

    azure group deployment create --resource-group [your-resource-group-name] --name [deployment-name] --template-file [path-to-the-arm-template-json-file] --parameters-file [path-to-the-arm-template-json-file]

as another option, you can provide the parameters interactively, by omitting the --parameters-file option or inline with a json formatted string as in the following example:

	azure group deployment create --resource-group [your-resource-group-name] --name [deployment-name] --template-file [path-to-the-arm-template-json-file] --parameters [json-formatted-string-of-parameters]

If you need to delete a deployment in a Resource Group you can type:

    azure group deployment delete --resource-group [your-resource-group-name] --name [deployment-name]

If you need to delete the whole Resource Group you can type:

    azure group delete --name [your-resource-group-name]

You can append the **--json** option to any of those commands to get json formatted output and parse it
