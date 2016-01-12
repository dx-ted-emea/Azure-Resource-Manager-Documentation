> Azure Resource Manager Community Documentation - Beta Version

> Work in progress - This community driven documentation is considered to be in preview stage at this time. Documentation might contain errors, might not cover every aspect or might lack complete parts, even important parts. Please help us make this documentation better by [contributing](CONTRIBUTING.md) anything that you think would make it better.


---

# Checking deployment operations in PowerShell
When deploying ARM templates it is often useful to be able to see the status of the operations performed in the deployment.
If a deployment has failed then drilling into the operations to find the one(s) that failed helps diagnose the issue.
As templates get larger it helps to understand the progress through the deployment, and can also be useful to check the order that things are occurring.

This section shows how to get set up with PowerShell to make it really easy to see the results for the last deployment to a resource group.

To help get started, grab [AzureHelpers.ps1](/Tips-and-tricks/scripts/AzureHelpers.ps1) and save it to disk. 
From a PowerShell prompt (in the directory you save AzureHelpers.ps1 to), dot source the script to include the functions within it in your PowerShell session:

```powershell
. .\AzureHelpers.ps1
``` 

## Examples of using the helper functions

### Starting Simple
A simple example to get started with the cmdlets is:

```powershell
Get-LastDeploymentOperation -ResourceGroupName "asetest"
```

The output from this will be something similar to:

```
Id 
-- 
/subscriptions/e96f24a6-ceee-43a3-8ad4-5e5dca55656b/resourceGroups/asetest/providers/Microsoft.Resources/de... 
/subscriptions/e96f24a6-ceee-43a3-8ad4-5e5dca55656b/resourceGroups/asetest/providers/Microsoft.Resources/de... 
/subscriptions/e96f24a6-ceee-43a3-8ad4-5e5dca55656b/resourceGroups/asetest/providers/Microsoft.Resources/de... 
/subscriptions/e96f24a6-ceee-43a3-8ad4-5e5dca55656b/resourceGroups/asetest/providers/Microsoft.Resources/de...
```

This is the set of outputs that the standard Azure cmdlet `Get-AzureRmResourceGroupDeploymentOperation` returned for the most recent deployment to the “asetest” resource group. However, this isn’t terribly easy to read!

### Making It Readable
To address the readability challenge above, we can use another cmdlet from the helpers, `ConvertTo-DeploymentOperationSummary`, and pipe the results to that:

```powershell
Get-LastDeploymentOperation -ResourceGroupName "asetest" ` 
    | ConvertTo-DeploymentOperationSummary
```

The output from this looks like:

```
Id                : BC1833006F6A1347 
ProvisioningState : Failed 
ResourceType      : Microsoft.Web/sites/Extensions 
ResourceName      : WebApp-API-foo/MSDeploy 
StartTime         : 25/11/2015 12:28:46 
EndTime           : 25/11/2015 12:29:19 
Duration          : 00:00:33 
Error             : @{Code=ResourceDeploymentFailure; Message=The resource operation completed with terminal 
                    provisioning state 'Failed'.} 

Id                : E4CE0CC89B2FA18F 
ProvisioningState : Succeeded 
ResourceType      : Microsoft.Web/sites/config 
ResourceName      : WebApp-API-foo/web 
StartTime         : 25/11/2015 12:28:45 
EndTime           : 25/11/2015 12:28:48 
Duration          : 00:00:03 
Error             :

...
```

With this output format it is easy to format into even more of a summary for, work out which steps took the longest, or just filter to see which operations failed.

If you prefer to see the output in a more interactive way then you can output it to the GridView:

```powershell
Get-LastDeploymentOperation -ResourceGroupName "asetest" ` 
    | ConvertTo-DeploymentOperationSummary ` 
    | sort -Property StartTime ` 
    | Out-GridView
```

This gives you output like the following where you can further filter and sort interactively

![alt tag](/Tips-and-tricks/images/Deployment-operations-powershell-grid.png)

### Going Further Back
The `Get-LastDeploymentOperation` cmdlet takes an additional parameter, `DeploymentsToSkip`. This specifies how many deployments to skip when before returning the operations for the deployment. It defaults to zero, i.e. just take the most recent deployment. The example below sets it to two, i.e. gets the operations for the deployment two before the most recent deployment:

```powershell
Get-LastDeploymentOperation -ResourceGroupName "asetest" ` 
                            -DeploymentsToSkip 2
```

