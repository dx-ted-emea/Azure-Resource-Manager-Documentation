# Templates Advanced Authoring

## Expressions and functions
The basic syntax of the template is JSON; however, expressions and functions extend the JSON that is available in the template and enable you to create values that are not strict literal values.
* Expressions are enclosed with brackets ([ and ])
* Evaluated when the template is deployed
* Expressions can appear anywhere in a JSON string value and always return another JSON value. If you need to use a literal string that starts with a bracket [, you must use two brackets [[
* Function calls are formatted as functionName(arg1,arg2,arg3). Properties are referenced by using the dot and [index] operators

Functions used to construct values:
```
"variables": {
   "location": "[resourceGroup().location]",
   "usernameAndPassword": "[concat('parameters('username'), ':', parameters('password'))]",
   "authorizationHeader": "[concat('Basic ', base64(variables('usernameAndPassword')))]"
}
```
Return deployment information in the outputs section:
```
"outputs": {
  "exampleOutput": {
    "value": "[deployment()]",
    "type" : "object"
  }
}
```
Convert a user-provided parameter value to Integer:
```
"parameters": {
    "appId": { "type": "string" }
},
"variables": { 
    "intValue": "[int(parameters('appId'))]"
}
```

The full list of functions, visit [Template Functions](https://azure.microsoft.com/en-us/documentation/articles/resource-group-template-functions/)

## Multiple Instances of Resources
ARM template expressions include iterative fucntions that allow creating multiple instances of a resource. This way you can deploy several virtual machines for example, by adding a single resource section in the template file. Making the template more readable and easier to edit.

### copy, copyIndex, and length
typicly the number of iterations will be defined in a parameter or a variable. In the following snippet, numberOfInstances was defined as a variable.
To set an iterative loop, a **copy** section is defined. To access the current value of the iteration use **copyindex()** fuction.
This example creates public IPs as much as a numberOfIntances variable is set to.
```
   {
      "apiVersion": "2015-05-01-preview",
      "type": "Microsoft.Network/publicIPAddresses",
      "name": "[concat(parameters('publicIPPrefix'), copyindex())]",
      "location": "[resourceGroup().location]",
      "copy": {
        "name": "ipLoop",
        "count": "[variables('numberOfInstances')]"
      },
      "properties": {
        "publicIPAllocationMethod": "[variables('publicIPAddressType')]",
        "dnsSettings": {
          "domainNameLabel": "[concat(parameters('dnsNameforPublicIP'), copyindex())]"
        }
      }
   }
```
When using **copyindex()** values go from 0 to the set iteration count. To offset the index value, you can pass a value in the **copyindex()** function, such as **copyindex(1)**. The number of iterations to perform is still specified in the copy element, but the value of copyIndex is offset by the specified value. 

Resources can be created based on a an array of values; The **length** function is used to specify the count.
In this example a parameter of type array is set and used to for names for a web site. The length of the array is used for the number of iterations - 3 websites will be created: examplecopy-Contos, examplecopy-Fabrikam, examplecopy-Coho.
```
"parameters": { 
  "org": { 
     "type": "array", 
         "defaultValue": [ 
         "Contoso", 
         "Fabrikam", 
         "Coho" 
      ] 
  }
}, 
"resources": [ 
  { 
      "name": "[concat('examplecopy-', parameters('org')[copyIndex()])]", 
      "type": "Microsoft.Web/sites", 
      "location": "East US", 
      "apiVersion": "2015-08-01",
      "copy": { 
         "name": "websitescopy", 
         "count": "[length(parameters('org'))]" 
      }, 
      "properties": {
          "serverFarmId": "hostingPlanName"
      } 
  } 
]
```
## Dependencies 
A Resource Manager resource can have dependencies on other resources - a virtual machine must have a storage account available before its provisioned, a network interface needs a virtual network to be defined first etc.

```
"resources": [
   {
    "name": "<name-of-the-resource>",
    "type": "<resource-provider-namespace/resource-type-name>",
    "apiVersion": "<supported-api-version-of-resource>",
    "location": "<location-of-resource>",
    "tags": { <name-value-pairs-for-resource-tagging> },
    "dependsOn": [ <array-of-related-resource-names> ],
    "properties": { <settings-for-the-resource> },
    "resources": { <dependent-resources> }
   }
]
```
### dependson Property

The dependsOn property on a resource provides the ability to define this dependency for a resource. It's value can be a comma separated list of resource names. The dependencies between resources are evaluated and resources are deployed in their dependent order. When resources are not dependent on each other, they are attempted to be deployed in parallel. The lifecycle of dependsOn is just for deployment and is not available post-deployment.
Note that using the dependson property may have implications on the deployment performance.

In this example, a nic has an explicit dependency on a vnet and public ip - it will not be provioned until those resources are created:
```
{
      "apiVersion": "2015-05-01-preview",
      "type": "Microsoft.Network/networkInterfaces",
      "name": "[variables('nicName')]",
      "location": "[variables('location')]",
      "dependsOn": [
        "[concat('Microsoft.Network/publicIPAddresses/', variables('publicIPAddressName'))]",
        "[concat('Microsoft.Network/virtualNetworks/', variables('virtualNetworkName'))]"
      ],
      "properties": {
        "ipConfigurations": [
          {
            "name": "ipconfig1",
            "properties": {
              "privateIPAllocationMethod": "Dynamic",
              "publicIPAddress": {
                "id": "[resourceId('Microsoft.Network/publicIPAddresses',variables('publicIPAddressName'))]"
              },
              "subnet": {
                "id": "[variables('subnetRef')]"
              }
            }
          }
        ]
      }
    },
```
## resources Property

The resources property of the resource object allows defining child resources of the main resource:
* Child resources can only be defined 5 levels deep
* There is no explisit dependency between the main resource and the child resources. This can be defined using the dependson property.
 
In this example we defined a SQL Dataqbase on a SQL Database Server - the SQL Database is a child resource of a SQL Database server resource and depends on it:
```
{
     "$schema": "http://schema.management.azure.com/schemas/2014-04-01-preview/deploymentTemplate.json#",
     "contentVersion": "1.0.0.0",
     "resources": [
         {
             "apiVersion": "2.0",
             "name": "mysqlserver",
             "type": "Microsoft.Sql/servers",
             "location": "West US",
             "properties": {
                 "administratorLogin": "admin",
                 "administratorLoginPassword": "password$123"
             },
             "resources": [
                 {
                      "apiVersion": "2.0",
                      "name": "mysqldatabase",
                      "type": "databases",
                     "location": "West US",
                      "dependsOn": [
                           "[concat('Microsoft.Sql/servers/', 'mysqlserver')]"
                      ],
                      "properties": {
                          "edition": "Web",
                          "maxSizeBytes": "1073741824"
                      }
                 }
             ]
         }
     ]
}
```

### reference Function

This function enables an expression to derive its value from another resource's runtime state and defines an implicit dependency between resources. It is recommened to use reference and not dependson whenever possible to avoid potential performance implications. 

This example create a website and set the connection string for the SQL Database Server we priviosly created, using the reference fucntion:
```
{
    "apiVersion": "2014-04-01",
    "type": "Microsoft.Web/Sites",
    "name": "mywebsite",
    "location": "[parameters('serverLocation')]",
    "dependsOn": [
        "[concat('Microsoft.Web/serverFarms/', 'myhostplan')]"
    ],
    properties": {
        "name": "mywebsiteddf",
        "serverFarm": "myhostplan"
    },
    "resources": [{
        "apiVersion": "2014-04-01",
        "type": "config",
        "name": "web",
        "dependsOn": [
            "[concat('Microsoft.Web/Sites/', 'mywebsiteddf')]"
        ],
        "properties": {
            "connectionStrings": [{
                "ConnectionString": "[concat('Data Source=tcp:', reference(concat('Microsoft.Sql/servers/', variables('serverName'))).fullyQualifiedDomainName, ',1433;Initial Catalog=mytestdatabase;User Id=admin@', variables('serverName'), ';Password=', parameters('adminPassword'), ';')]",
                "Name": "DefaultConnection",
                "Type": 2
            }]
        }
    }]
}
```
### Resource Links

A dependency between resources can also continue after deployment - a link between a database and an app for example. Resource link are used to document and provide query capabililty over the relationships between resources post-deployment.

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

## Linked Templates

## Resources and References
https://azure.microsoft.com/en-us/documentation/articles/resource-group-template-functions/#resourceid
https://azure.microsoft.com/en-us/documentation/articles/resource-group-linked-templates/
https://azure.microsoft.com/en-us/documentation/articles/resource-group-define-dependencies/
https://azure.microsoft.com/en-us/documentation/articles/resource-group-create-multiple/
https://azure.microsoft.com/en-us/documentation/articles/resource-manager-template-links/
https://msdn.microsoft.com/library/azure/mt238499.aspx


