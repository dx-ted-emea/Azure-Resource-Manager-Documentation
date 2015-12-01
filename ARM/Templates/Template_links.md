# Resources and Templates Linking

## Linked Templates

## Resource Links
In the Resource Manager model, resources can have several types of dependencies. Resource dependencies during deploymend are detailed in the [Dependencies](../ARM/Templates/Template_Advanced_Authoring.md#dependencies) topics in this repository.
But, a dependency between resources can also continue after deployment - a link between a database and an app for example. Resource link are used to document and provide query capabililty over the relationships between resources post-deployment.

Links can be established between resources belonging to different resource groups. However, all the linked resources must belong to the same subscription. Each resource can be linked to 50 other resources. If any of the linked resources are deleted or moved, the link owner must clean up the remaining link.

**Resource Links Template schema**
The link is applied to the source resource. 
Add the following to the resources section of the tempalte:
```
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
|targetId	|string	|Yes		|The identifier of the target resource to link to.|
|notes	|string	|No	|512 characters	|Description of the lock.|

**Query about resources**

**Sample Template**
Apply a link between the App Service and a storage account named storagecontoso:
```
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
