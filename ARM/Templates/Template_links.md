# Resources and Templates Linking

## Linked Templates
Resource Manager tempalte can become large and complex. Linked templates allows breaking large tempalte into smaller, more managable units. This allows you to:
* Reuse, reference common resource templates from parent templates
* Unit test, divide smaller deployments, and test those small deployments independently.
* Maintenance & management of larger more complex templates

To learn about how to link templates, folllow the [Linked Templates](https://azure.microsoft.com/en-us/documentation/articles/resource-group-linked-templates/) tutorial.

### Simulating Conditional Linking
Using parameters can be used to simulate dynamic or conditional template linking, based on input parameters. In this example, the value of the 'status' parameter can be set to enable or disable, and in turn deploy a different template based on a condition outside of the template (in the calling code for example, or based on user input during deployment):
```json
"properties": {
    "mode": "Incremental",
    "templateLink": {
        "uri": "[concat(variables('templateBaseUrl'), parameters('status'), '.json')]",
        "contentVersion": "1.0.0.0"
    }
}
```

## Resource Links
In the Resource Manager model, resources can have several types of dependencies. Resource dependencies during deployment are detailed in the [Dependencies](../ARM/Templates/Template_Advanced_Authoring.md#dependencies) topics in this repository.
But, a dependency between resources can also continue after deployment - a link between a database and an app for example. Resource link are used to document and provide query capabililty over the relationships between resources post-deployment.

To learn about resource linked, follow the [Resource Linked](https://azure.microsoft.com/en-us/documentation/articles/resource-manager-template-links/) tutorial.

## Resources and References
https://azure.microsoft.com/en-us/documentation/articles/resource-manager-template-links/

https://msdn.microsoft.com/library/azure/mt238499.aspx

https://azure.microsoft.com/en-gb/documentation/articles/app-service-logic-arm-with-api-app-provision/

https://azure.microsoft.com/en-us/documentation/articles/resource-group-linked-templates/
