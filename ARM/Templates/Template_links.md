# Resources and Templates Linking

## Linked Templates
From within one Azure Resource Manager template, you can link to another template which enables you to decompose your deployment into a set of targeted, purpose-specific templates. This enables you to:
* Reuse, reference common resource templates from parent templates
* Unit test, divide smaller deployments, and test those small deployments independently.
* Maintenance & management of larger more complex templates

### Microsoft.Resources/deployments Resource
Create a template link by adding a deployments resource under the resources section of the template. The schema of the resource is:
```json
"resources": [ 
  { 
     "apiVersion": "2015-01-01", 
     "name": "resource name", 
     "type": "Microsoft.Resources/deployments", 
     "properties": { 
       "mode": "incremental", 
       "templateLink": {
          "uri": "<uri of the template. You can only provide a URI value that includes either http or https.>",
          "contentVersion": "1.0.0.0"
       }, 
       "parameters": { 
          "StorageAccountName":{"value": "[parameters('StorageAccountName')]"} 
       } 
     } 
  } 
] 
```
** Using Parameters File**
Here we used __parameters__ property to pass parameters directly to the linked template. Another option is to use a parameters file:
```json
"resources": [ 
  { 
     "apiVersion": "2015-01-01", 
     "name": "nestedTemplate", 
     "type": "Microsoft.Resources/deployments", 
     "properties": { 
       "mode": "incremental", 
       "templateLink": {
          "uri":"https://www.contoso.com/AzureTemplates/newStorageAccount.json",
          "contentVersion":"1.0.0.0"
       }, 
       "parametersLink": { 
          "uri":"https://www.contoso.com/AzureTemplates/parameters.json",
          "contentVersion":"1.0.0.0"
       } 
     } 
  } 
] 
```
> Note: The Resource Manager service must be able to access the linked template, which means you cannot specify a local file or a file that is only available on your local network for the linked template. You can only provide a URI value that includes either http or https. One option is to place your linked template in a storage account, and use the URI for that item.

**Using Variables**
When working with a large set of modular templates you might want to avoid using hard coded template URLs. The following example shows how to use a base URL to create two URLs for linked templates (sharedTemplateUrl and vmTemplate)
```json
"variables": {
    "templateBaseUrl": "https://raw.githubusercontent.com/Azure/azure-quickstart-templates/master/postgresql-on-ubuntu/",
    "sharedTemplateUrl": "[concat(variables('templateBaseUrl'), 'shared-resources.json')]",
    "tshirtSizeSmall": {
        "vmSize": "Standard_A1",
        "diskSize": 1023,
        "vmTemplate": "[concat(variables('templateBaseUrl'), 'database-2disk-resources.json')]",
        "vmCount": 2,
        "slaveCount": 1,
        "storage": {
            "name": "[parameters('storageAccountNamePrefix')]",
            "count": 1,
            "pool": "db",
            "map": [0,0],
            "jumpbox": 0
        }
    }
}
```

### Simulating Conditional Linking
Using parameters can be used to simulate dynamic or conditional tempalte linking, based on input parameters. In this example, the value of the 'jumpbox' parameter can be set to enable or disable, and in turn deploy a different template based on a condition outside of the template (in the calling code for example, or based on user input during deployment):
```json
"properties": {
    "mode": "Incremental",
    "templateLink": {
        "uri": "[concat(variables('templateBaseUrl'), parameters('jumpbox'), '.json')]",
        "contentVersion": "1.0.0.0"
    }
}
```

## Resource Links
In the Resource Manager model, resources can have several types of dependencies. Resource dependencies during deployment are detailed in the [Dependencies](../ARM/Templates/Template_Advanced_Authoring.md#dependencies) topics in this repository.
But, a dependency between resources can also continue after deployment - a link between a database and an app for example. Resource link are used to document and provide query capabililty over the relationships between resources post-deployment.

Links can be established between resources belonging to different resource groups. However, all the linked resources must belong to the same subscription. Each resource can be linked to 50 other resources. If any of the linked resources are deleted or moved, the link owner must clean up the remaining link.

### Resource Links Template schema
The link is applied to the source resource. 
Add the following to the resources section of the tempalte:
```json
{
    "type": enum,
    "apiVersion": "2015-01-01",
    "name": string,
    "dependsOn": [ array values ],
    "properties":
    {
        "targetId": string,
        "notes": string
    }
}
```
|NAME	|TYPE	|REQUIRED	|PERMITTED VALUES	|DESCRIPTION|
|-----|-----|-----------|-----------------|-----------|
|type	|enum	|Yes	|{namespace}/{type}/providers/links|	The resource type to create. The {namespace} and {type} values refer to the provider namespace and resource type of the source resource.|
|apiVersion	|enum	|Yes	|2015-01-01	|The API version to use for creating the resource.|
|name	|string	|Yes	|{resouce}/Microsoft.Resources/{linkname} up to 64 characters. It cannot contain <, > %, &, ?, or any control characters.	|A value that specifes both the name of source resource and a name for the link.|
|dependsOn	|array	|No	|A comma-separated list of a resource names or resource unique identifiers.	|The collection of resources this link depends on. If the resources you are linking are deployed in the same template, include those resource names in this element to ensure they are deployed first.|
|properties	|object	|Yes	|(shown below)	|An object that identifies the resource to link to, and notes about the link.|

Properties:

|NAME	|TYPE	|REQUIRED	|PERMITTED VALUES	|DESCRIPTION|
|-----|-----|-----------|-----------------|-----------|
|targetId	|string	|Yes	|The identifier of the target resource to link to.|
|notes	|string	|No	|512 characters	|Description of the lock.|

**Sample Template**
Apply a link between the App Service and a storage account named storagecontoso:
```json
{
"$schema": "https://schema.management.azure.com/schemas/2015-01-01/deploymentTemplate.json#",
"contentVersion": "1.0.0.0",
"parameters": {
    "hostingPlanName": {
            "type": "string"
    }
},
"variables": {
    "siteName": "[concat('site',uniqueString(resourceGroup().id))]"
},
"resources": [
    {
            "apiVersion": "2015-08-01",
            "type": "Microsoft.Web/serverfarms",
            "name": "[parameters('hostingPlanName')]",
            "location": "[resourceGroup().location]",
            "sku": {
                "tier": "Free",
                "name": "f1",
                "capacity": 0
            },
            "properties": {
                "numberOfWorkers": 1
            }
        },
    {
            "apiVersion": "2015-08-01",
            "name": "[variables('siteName')]",
            "type": "Microsoft.Web/sites",
            "location": "[resourceGroup().location]",
        "dependsOn": [ "[parameters('hostingPlanName')]" ],
            "properties": {
                "serverFarmId": "[parameters('hostingPlanName')]"
            }
    },
    {
            "type": "Microsoft.Web/sites/providers/links",
            "apiVersion": "2015-01-01",
            "name": "[concat(variables('siteName'),'/Microsoft.Resources/SiteToStorage')]",
            "dependsOn": [ "[variables('siteName')]" ],
            "properties": {
                "targetId": "[resourceId('Microsoft.Storage/storageAccounts','storagecontoso')]",
                "notes": "This web site uses the storage account to store user information."
            }
        }
]
}
```

## Resources and References
https://azure.microsoft.com/en-us/documentation/articles/resource-manager-template-links/

https://msdn.microsoft.com/library/azure/mt238499.aspx

https://azure.microsoft.com/en-gb/documentation/articles/app-service-logic-arm-with-api-app-provision/

https://azure.microsoft.com/en-us/documentation/articles/resource-group-linked-templates/
