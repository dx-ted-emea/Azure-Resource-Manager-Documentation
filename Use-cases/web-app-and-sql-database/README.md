# Azure Web App and Azure SQL Database

## Overview

The [201 Web App SQL Database](https://github.com/Azure/azure-quickstart-templates/tree/master/201-web-app-sql-database) template in Quick Starts creates an [Azure Web App](https://azure.microsoft.com/en-us/documentation/services/app-service/web/) (with [App Service Plan](https://azure.microsoft.com/en-us/documentation/articles/azure-web-sites-web-hosting-plans-in-depth-overview/)) with an accompanying [Azure SQL Database](https://azure.microsoft.com/en-us/documentation/services/sql-database/).

To get an outline, see the [template walkthrough](https://azure.microsoft.com/en-us/documentation/articles/app-service-web-arm-with-sql-database-provision/).

The template sets up [autoscaling](https://azure.microsoft.com/en-us/documentation/articles/app-service-web-arm-with-sql-database-provision/#_autoscale) for the App Servive Plan linked to CPU Utilisation of the instances.

Performing a scaling operation for the SQL Database can be performed in a number of ways and some of these are shown below.

## Scaling the database with a template

The template specifies the `edition` and `requestedServiceObjectiveName` for the SQL Database resource. This is the database tier and SKU that you want provisioned, e.g. (`Standard` + `S0`) or (`Premium` + `P3`). 

Because of the way ARM templates work, you could simply redeploy the template keeping all of the parameters the same but set the new values for `edition` and `requestedServiceObjectiveName`. 

Alternatively, you can create a new template that contains just the Server and Database that is targetted at the just the two properties that you want to set. The full version of this template is [here](./scale-database.json), and a snippet of it is below: 

```json
	"resources": [
		{
			"name": "[parameters('serverName')]",
			"type": "Microsoft.Sql/servers",
			"location": "[parameters('serverLocation')]",
			"apiVersion": "2014-04-01-preview",
			"properties": {},
			"resources": [
				{
					"name": "[parameters('databaseName')]",
					"type": "databases",
					"location": "[parameters('serverLocation')]",
					"apiVersion": "2014-04-01-preview",
					"dependsOn": [
						"[concat('Microsoft.Sql/servers/', parameters('serverName'))]"
					],
					"properties": {
						"edition": "[parameters('edition')]",
						"requestedServiceObjectiveName": "[parameters('requestedServiceObjectiveName')]"
					}
				}
			]
		}
	]
```

## Scaling the database with PowerShell/CLI database commands

### PowerShell

The Azure PowerShell cmdlets include cmdlets specific to Azure SQL Database. 
In the example below, the database `mydatabase` on server `myserver` in resource group `mygroup` is scaled to the S1 SKU in the Standard tier:
	
```powershell
Set-AzureRmSqlDatabase -ResourceGroupName mygroup -ServerName myserver -DatabaseName mydatabase -Edition Standard -RequestedServiceObjectiveName S1 
```

### CLI
Currently the CLI (verion 0.9.13) doesn't have ARM commands for working with SQL Database. 

## Scaling the database with PowerShell/CLI resource commands
In addition to the commands tailored to specific resource types, both PowerShell and the CLI have support for working with resources in a generic way.

### PowerShell
In PowerShell we have cmdlets such as `Get-AzureRmResource` and `Set-AzureRmResource` that allow you to work with resources in a generic way. To do this you need to specify the resource types and names, but these should feel familiar from the ARM template.

To scale a SQL Database with these commands we can use the Set-AzureRmResource cmdlet 
In the example below, the database `mydatabase` on server `myserver` in resource group `mygroup` is scaled to the S1 SKU in the Standard tier:

```powershell
$PropertiesObject = @{
  edition = "Standard"
  requestedServiceObjectiveName = "S1"
}
Set-AzureRmResource `
	-ResourceGroupName mygroup `
	-ResourceType Microsoft.Sql/servers/databases `
	-ResourceName myserver/mydatabase `
	-PropertyObject $PropertiesObject `
	-ApiVersion 2014-04-01-preview -Force
```

### CLI

** Note this currently (0.9.13) returns an error (it does kick off the transition) - have raised it with the PG **

```bash
azure resource set --resource-group mygroup \
	--resource-type Microsoft.Sql/servers/databases \
	--name mydatabase \
	--parent servers/myserver \
	--properties '{ \"edition\" : \"Standard\", \"requestedServiceObjectiveName\" : \"S1\"}' \
	--api-version 2014-04-01-preview 
```

## Scaling the database with REST API calls
All of the above approaches ultimately result in calls to the underlying ARM REST APIs. For more details on how to call and authenticate against the APIs see the [REST API](../../ARM/SDKs/Rest-api.md) documentation. 


In the example below, the database `mydatabase` on server `myserver` in resource group `mygroup` for subscription with ID `11111111-1111-1111-1111-111111111111` is scaled to the S1 SKU in the Standard tier.

To achieve this, send a `PUT` request as indicated below: `` with the following body:

```HTTP
PUT /subscriptions/11111111-1111-1111-1111-111111111111/resourceGroups/mygroup/providers/Microsoft.Sql/servers/myserver/databases/mydatabase?api-version=2014-04-01-preview HTTP/1.1
Host: management.azure.com
Authorization: Bearer YOUR_ACCESS_TOKEN
Content-Type: application/json

{
  "location": "North Europe",
  "properties": {
    "edition": "Standard",
    "requestedServiceObjectiveName": "S1"
  }
}
```

As well as calling the API from code, the Azure Resource Explorer is a great tool that lets you interact with the REST APIs and is great way to get to know the API schema. See the [Resource Explorer](../../Tips-and-tricks/Resource-explorer.md) section for more information.