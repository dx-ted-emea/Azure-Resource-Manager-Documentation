# Templates Advanced Authoring

## Expressions and functions
The basic syntax of the template is JSON; however, expressions and functions extend the JSON that is available in the template and enable you to create values that are not strict literal values.
* Expressions are enclosed with brackets ([ and ])
* Evaluated when the template is deployed
* Expressions can appear anywhere in a JSON string value and always return another JSON value. If you need to use a literal string that starts with a bracket [, you must use two brackets [[
* Function calls are formatted as functionName(arg1,arg2,arg3). Properties are referenced by using the dot and [index] operators

For the full list of functions, visit [Template Functions](https://azure.microsoft.com/en-us/documentation/articles/resource-group-template-functions/)

## Multiple Instances of Resources
ARM template expressions include iterative fucntions that allow creating multiple instances of a resource. This way you can deploy several virtual machines for example, by adding a single resource section in the template file. Making the template more readable and easier to edit.

For an overview on copy, copyIndex and length functions, visit [multiple instances of resources](https://azure.microsoft.com/en-us/documentation/articles/resource-group-create-multiple/) page

## Dependencies 
A Resource Manager resource can have dependencies on other resources - a virtual machine must have a storage account available before its provisioned, a network interface needs a virtual network to be defined first etc.

For an overview on defining dependencies in Azure Resource Manager templates visit [here](https://azure.microsoft.com/en-us/documentation/articles/resource-group-define-dependencies/).

```json
"resources": [
   {
    "name": "<name-of-the-resource>",
    "type": "<resource-provider-namespace/resource-type-name>",
    "apiVersion": "<supported-api-version-of-resource>",
    "location": "<location-of-resource>",
    "tags": { "<name-value-pairs-for-resource-tagging>" },
    "dependsOn": [ "<array-of-related-resource-names>" ],
    "properties": { "<settings-for-the-resource>" },
    "resources": { "<dependent-resources>" }
   }
]
```
### dependson Property
In this example, a nic has an explicit dependency on a vnet and public ip - it will not be provioned until those resources are created:
```json
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
In this example we defined a SQL Database on a SQL Database Server - the SQL Database is a child resource of a SQL Database server resource and depends on it:
```json
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
This example create a website and set the connection string for the SQL Database Server we priviosly created, using the reference fucntion:
```json
{
    "apiVersion": "2014-04-01",
    "type": "Microsoft.Web/Sites",
    "name": "mywebsite",
    "location": "[parameters('serverLocation')]",
    "dependsOn": [
        "[concat('Microsoft.Web/serverFarms/', 'myhostplan')]"
    ],
    "properties": {
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

## Resources and References
https://azure.microsoft.com/en-us/documentation/articles/resource-group-template-functions/#resourceid
https://azure.microsoft.com/en-us/documentation/articles/resource-group-define-dependencies/
https://azure.microsoft.com/en-us/documentation/articles/resource-group-create-multiple/


