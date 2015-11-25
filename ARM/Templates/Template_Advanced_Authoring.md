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

## Dependencies
## Linked Templates

## Resources and References
https://azure.microsoft.com/en-us/documentation/articles/resource-group-template-functions/#resourceid
https://azure.microsoft.com/en-us/documentation/articles/resource-group-linked-templates/
https://azure.microsoft.com/en-us/documentation/articles/resource-group-define-dependencies/
https://azure.microsoft.com/en-us/documentation/articles/resource-group-create-multiple/

