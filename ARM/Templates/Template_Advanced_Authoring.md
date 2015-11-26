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

## Linked Templates

## Resources and References
https://azure.microsoft.com/en-us/documentation/articles/resource-group-template-functions/#resourceid
https://azure.microsoft.com/en-us/documentation/articles/resource-group-linked-templates/
https://azure.microsoft.com/en-us/documentation/articles/resource-group-define-dependencies/
https://azure.microsoft.com/en-us/documentation/articles/resource-group-create-multiple/
https://azure.microsoft.com/en-us/documentation/articles/resource-manager-template-links/

