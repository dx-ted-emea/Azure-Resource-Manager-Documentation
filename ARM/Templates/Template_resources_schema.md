# Resource Providers Schema

To find what providers are available and how their schema looks like you can use Powershell or CLI commands.

## Querying providers - PowerShell

For a full list of the resource providers namespaces, you can call a powershell command:
```powershell
Get-AzureRmResourceProvider -ListAvailable
```
To get resource type names, locations and supported versions for a specific resource:
```powershell
(Get-AzureRmResourceProvider -ProviderNamespace Microsoft.Compute).ResourceTypes
```
Here is the output for the `Microsoft.Compute` provider:
```
ResourceTypeName                                          Locations                                    ApiVersions                     
----------------                                          ---------                                    -----------                     
availabilitySets                                          {East US, East US 2, West US, Central US...} {2015-06-15, 2015-05-01-preview}
virtualMachines                                           {East US, East US 2, West US, Central US...} {2015-06-15, 2015-05-01-preview}
virtualMachines/extensions                                {East US, East US 2, West US, Central US...} {2015-06-15, 2015-05-01-preview}
virtualMachines/diagnosticSettings                        {East US, East US 2, West US, Central US...} {2014-04-01}                    
virtualMachines/metricDefinitions                         {East US, East US 2, West US, Central US...} {2014-04-01}                    
virtualMachineScaleSets                                   {East US, East US 2, West US, Central US...} {2015-06-15, 2015-05-01-preview}
virtualMachineScaleSets/extensions                        {East US, East US 2, West US, Central US...} {2015-06-15, 2015-05-01-preview}
virtualMachineScaleSets/virtualMachines                   {East US, East US 2, West US, Central US...} {2015-06-15, 2015-05-01-preview}
virtualMachineScaleSets/networkInterfaces                 {East US, East US 2, West US, Central US...} {2015-06-15, 2015-05-01-preview}
virtualMachineScaleSets/virtualMachines/networkInterfaces {East US, East US 2, West US, Central US...} {2015-06-15, 2015-05-01-preview}
locations                                                 {}                                           {2015-06-15, 2015-05-01-preview}
locations/operations                                      {East US, East US 2, West US, Central US...} {2015-06-15, 2015-05-01-preview}
locations/vmSizes                                         {East US, East US 2, West US, Central US...} {2015-06-15, 2015-05-01-preview}
locations/usages                                          {East US, East US 2, West US, Central US...} {2015-06-15, 2015-05-01-preview}
locations/publishers                                      {East US, East US 2, West US, Central US...} {2015-06-15, 2015-05-01-preview}
operations                                                {East US, East US 2, West US, Central US...} {2015-06-15, 2015-05-01-preview}
```
If you want to create a virtual machine, the value of **type** will be a combination of the compute namespace Microsoft.Compute and the resource type name virtualMachines: `Microsoft.Compute/virtualMachines`.

## Querying providers - CLI

For a full list of the resource providers namespaces via the CLI use:
```bash
azure provider list
```
To get resource type names, locations and supported versions for a specific resource use the command below. Note that the `--json` switch is optional.
```bash
azure provider show Microsoft.Compute --json
```
Here is the output for the `Microsoft.Compute` provider:
```json
{
  "resourceTypes": [
    {
      "apiVersions": [ 
          "2015-06-15", 
          "2015-05-01-preview"
          ],
      "locations": ["East US", "East US 2", "West US", "Central US", "South Central US", "North Europe", "West Europe", "East Asia", "Southeast Asia", "Japan East", "Japan West", "North Central US", "Australia East", "Australia Southeast","Brazil South"],
      "properties": {},
      "name": "availabilitySets"
    },
    {
      "apiVersions": [
        "2015-06-15",
        "2015-05-01-preview"
      ],
      "locations": [ "East US", "East US 2", "West US", "Central US", "South Central US", "North Europe", "West Europe", "East Asia", "Southeast Asia", "Japan East", "Japan West", "North Central US", "Australia East", "Australia Southeast", "Brazil South"
      ],
      "properties": {},
      "name": "virtualMachines"
    },
    {
      "apiVersions": [
        "2015-06-15",
        "2015-05-01-preview"
      ],
      "locations": [ "East US", "East US 2", "West US", "Central US", "South Central US", "North Europe", "West Europe", "East Asia", "Southeast Asia", "Japan East", "Japan West", "North Central US", "Australia East", "Australia Southeast", "Brazil South"
      ],
      "properties": {},
      "name": "virtualMachines/extensions"
    },
    // rest of the output omitted for brevity
  ],
  "id": "/subscriptions/e96f24a6-ceee-43a3-8ad4-5e5dca55656b/providers/Microsoft.Compute",
  "namespace": "Microsoft.Compute",
  "registrationState": "Registered"
}


```
If you want to create a virtual machine, the value of **type** will be a combination of the compute namespace Microsoft.Compute and the resource type name virtualMachines: `Microsoft.Compute/virtualMachines`.

