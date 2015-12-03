# Checking deployment operations with Azure CLI
When deploying ARM templates it is often useful to be able to see the status of the operations performed in the deployment.
If a deployment has failed then drilling into the operations to find the one(s) that failed helps diagnose the issue.
As templates get larger it helps to understand the progress through the deployment, and can also be useful to check the order that things are occurring.

This section shows how to get set up with the Azure CLI to make it really easy to see the results for the last deployment to a resource group.

The scripts in this section use the JSON output mode for the Azure CLI and then process the JSON using [jq](https://stedolan.github.io/jq/), so you'll need to ensure that you have that installed.

If you want to grab the scripts and give them a try they are
* [GetDeploymentOperationSummary.sh](/Tips-and-tricks/scripts/GetDeploymentOperationSummary.sh)
* [GetDeploymentOperationFailures.sh](/Tips-and-tricks/scripts/GetDeploymentOperationFailures.sh)

In the section below we'll walk through the scripts and their uses.

## Examples

### Standard CLI commands
To get started, we’ll take a quick look at the standard Azure CLI commands for querying deployment operations. To list the operations for a deployment we can use the `azure group deployment operation list` command. To do this we need to pass the resource group and deployment name. We can list the deployments using `azure group deployment list` command (passing the resource group).

For example:

```bash
azure group deployment list asetest
```

would give something like

```
info:    Executing command group deployment list
+ Listing deployments
data:    DeploymentName     : Microsoft.WebSiteaca1d698-920a
data:    ResourceGroupName  : asetest
data:    ProvisioningState  : Succeeded
data:    Timestamp          : 2015-11-11T15:51:55.4133075Z
data:    Mode               : Incremental
data:
data:    DeploymentName     : Microsoft.StorageAccount-20151011153834
data:    ResourceGroupName  : asetest
data:    ProvisioningState  : Succeeded
data:    Timestamp          : 2015-11-11T14:39:12.2571368Z
data:    Mode               : Incremental
data:
data:    DeploymentName     : Microsoft.AppServiceEnvironmente34fe687-9f36
data:    ResourceGroupName  : asetest
data:    ProvisioningState  : Succeeded
data:    Timestamp          : 2015-11-11T13:42:40.3244758Z
data:    Mode               : Incremental
info:    group deployment list command OK
```

From there we can get the name of the last deployment (`Microsoft.WebSiteaca1d698-920a`) and get the operations for that deployment using

```bash
azure group deployment operation list asetest Microsoft.WebSiteaca1d698-920a
```

This will give us output like

```
info:    Executing command group deployment operation list
+ Getting deployoment operations
data:    Id:                   /subscriptions/e96f24a6-ceee-43a3-8ad4-5e5dca55656b/resourceGroups/asetest/deployments/Microsoft.WebSiteaca1d698-920a/operations/242E8A24157FDA69
data:    OperationId:          242E8A24157FDA69
data:    Provisioning State:   Succeeded
data:    Timestamp:            2015-11-11T15:51:54.4304303Z
data:    Status Code:          Created
data:    Status Message:
data:    Target Resource Id:   /subscriptions/e96f24a6-ceee-43a3-8ad4-5e5dca55656b/resourceGroups/asetest/providers/microsoft.insights/alertrules/ForbiddenRequests corsstoragetest
data:    Target Resource Name: ForbiddenRequests corsstoragetest
data:    Target Resource Type: microsoft.insights/alertrules
data:    ---------------------
data:
data:    Id:                   /subscriptions/e96f24a6-ceee-43a3-8ad4-5e5dca55656b/resourceGroups/asetest/deployments/Microsoft.WebSiteaca1d698-920a/operations/38A6E8D64D86BE4B
data:    OperationId:          38A6E8D64D86BE4B
data:    Provisioning State:   Succeeded
data:    Timestamp:            2015-11-11T15:51:54.3901981Z
data:    Status Code:          Created
data:    Status Message:
data:    Target Resource Id:   /subscriptions/e96f24a6-ceee-43a3-8ad4-5e5dca55656b/resourceGroups/asetest/providers/microsoft.insights/alertrules/ServerErrors corsstoragetest
data:    Target Resource Name: ServerErrors corsstoragetest
data:    Target Resource Type: microsoft.insights/alertrules
data:    ---------------------

... [further output omitted for brevity]    
```

Alternatively, we can add the `--json` switch to get the output in JSON format

```json
[
  {
    "id": "/subscriptions/e96f24a6-ceee-43a3-8ad4-5e5dca55656b/resourceGroups/asetest/deployments/Microsoft.WebSiteaca1d698-920a/operations/242E8A24157FDA69",
    "operationId": "242E8A24157FDA69",
    "properties": {
      "provisioningState": "Succeeded",
      "timestamp": "2015-11-11T15:51:54.4304303Z",
      "statusCode": "Created",
      "targetResource": {
        "id": "/subscriptions/e96f24a6-ceee-43a3-8ad4-5e5dca55656b/resourceGroups/asetest/providers/microsoft.insights/alertrules/ForbiddenRequests corsstoragetest",
        "resourceName": "ForbiddenRequests corsstoragetest",
        "resourceType": "microsoft.insights/alertrules"
      }
    }
  },
  {
    "id": "/subscriptions/e96f24a6-ceee-43a3-8ad4-5e5dca55656b/resourceGroups/asetest/deployments/Microsoft.WebSiteaca1d698-920a/operations/38A6E8D64D86BE4B",
    "operationId": "38A6E8D64D86BE4B",
    "properties": {
      "provisioningState": "Succeeded",
      "timestamp": "2015-11-11T15:51:54.3901981Z",
      "statusCode": "Created",
      "targetResource": {
        "id": "/subscriptions/e96f24a6-ceee-43a3-8ad4-5e5dca55656b/resourceGroups/asetest/providers/microsoft.insights/alertrules/ServerErrors corsstoragetest",
        "resourceName": "ServerErrors corsstoragetest",
        "resourceType": "microsoft.insights/alertrules"
      }
    }
  },
  // ... [further output omitted for brevity]   
]
``` 

### Getting the summary
The [GetDeploymentOperationSummary.sh](/Tips-and-tricks/scripts/GetDeploymentOperationSummary.sh) script makes it easy to get a high-level summary of the most recent deployment to the "asetest" resource group:

```bash
./GetDeploymentOperationSummary.sh asetest
```

This gives the output below
```json
[
  {
    "provisioningState": "Succeeded",
    "timestamp": "2015-11-25T12:28:28.8112854Z",
    "resourceType": "Microsoft.Web/serverfarms",
    "resourceName": "AppService-foo",
    "error": null
  },
  {
    "provisioningState": "Succeeded",
    "timestamp": "2015-11-25T12:28:46.2256849Z",
    "resourceType": "Microsoft.Web/sites",
    "resourceName": "WebApp-API-foo",
    "error": null
  },
  {
    "provisioningState": "Succeeded",
    "timestamp": "2015-11-25T12:28:48.9928976Z",
    "resourceType": "Microsoft.Web/sites/config",
    "resourceName": "WebApp-API-foo/web",
    "error": null
  },
  {
    "provisioningState": "Failed",
    "timestamp": "2015-11-25T12:29:19.4734883Z",
    "resourceType": "Microsoft.Web/sites/Extensions",
    "resourceName": "WebApp-API-foo/MSDeploy",
    "error": {
      "code": "ResourceDeploymentFailure",
      "message": "The resource operation completed with terminal provisioning state 'Failed'."
    }
  }
]
```

If you want to get the output for previous deployments then you can pass an additional parameter to the script. The command below will output the operations for the deployment two before the most recent deployment

```bash
./GetDeploymentOperationFailures.sh asetest 2
```

### Getting failure information

If you just want to see the summary for failed operations then the  [GetDeploymentOperationFailures.sh](/Tips-and-tricks/scripts/GetDeploymentOperationFailures.sh) script should give you a good starting point. For example, to see the most recent failures for the asetest group:

```bash
./GetDeploymentOperationFailures.sh asetest
```

This script also supports passing a number indicating the number of deployments to skip. 