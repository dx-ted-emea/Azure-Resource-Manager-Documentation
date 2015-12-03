<#
 # Helper function for other cmdlets
 #>
function ParseOperationDuration($durationString){

# expected behaviour (should put in tests)
#(ParseOperationDuration "PT21.501S").ToString() # Timespan: 21.501 seconds
#(ParseOperationDuration "PT5M21.501S").ToString() # Timespan: 5 minutes 21.501 seconds
#(ParseOperationDuration "PT1H5M21.501S").ToString() # Timespan: 1 hour 5 minutes 21.501 seconds
#(ParseOperationDuration "PT 21.501S").ToString() # throws exception for unhandled format

    $timespan = $null
    switch -Regex ($durationString)  {
        "^PT(?<seconds>\d*.\d*)S$" {
            $timespan =  New-TimeSpan -Seconds $matches["seconds"]
        }
        "^PT(?<minutes>\d*)M(?<seconds>\d*.\d*)S$" {
            $timespan =  New-TimeSpan -Minutes $matches["minutes"] -Seconds $matches["seconds"]
        }
        "^PT(?<hours>\d*)H(?<minutes>\d*)M(?<seconds>\d*.\d*)S$" {
            $timespan =  New-TimeSpan -Hours $matches["hours"] -Minutes $matches["minutes"] -Seconds $matches["seconds"]
        }
    }
    if($null -eq $timespan){
        $message = "unhandled duration format '$durationString'"
        throw $message
    }
    $timespan
}

<#
.SYNOPSIS

Get a summary of Azure Resource Group Deployment Operations
.DESCRIPTION

Converts the output from Get-AzureRmResourceGroupDeploymentOperation into a summary object with commonly used information, and parses durations into TimeSpans etc

.PARAMETER DeploymentOperations

.EXAMPLE

Get-AzureRmResourceGroupDeploymentOperation -ResourceGroupName "mygroup" -DeploymentName "my deployment" | ConvertTo-DeploymentOperationSummary

#>
function ConvertTo-DeploymentOperationSummary{
    [CmdletBinding()]
    param(
    
        [Parameter(Position=0, Mandatory=$true, ValueFromPipeline=$True)]
        [object[]] $DeploymentOperations
    )

    process{
         $DeploymentOperations | ForEach-Object { 
            $timeStamp = [System.DateTime]::Parse($_.Properties.Timestamp);
            $duration = (ParseOperationDuration $_.Properties.Duration);
            [PSCustomObject]@{ 
                "Id"=$_.OperationId; 
                "ProvisioningState" = $_.Properties.ProvisioningState; 
                "ResourceType"=$_.Properties.TargetResource.ResourceType; 
                "ResourceName"=$_.Properties.TargetResource.ResourceName; 
                "StartTime" = $timeStamp - $duration; 
                "EndTime" = $timeStamp; 
                "Duration" =  $duration;
                "Error" = $_.Properties.StatusMessage.Error;
            }
        }
    }
}

<#
.SYNOPSIS

Get the latest deployment operations for an Azure resource group
.DESCRIPTION

Provides a quick way to get the latest deployment operations for an Azure resource group. 
It defaults to the operations for the most recent deployment but that behaviour can be changed with the DeploymentsToSkip parameter.
.PARAMETER ResourceGroupName
The name of the resource group to get the deployment operations for
.PARAMETER DeploymentsToSkip
How many deployments to skip for the specified resource group. By default, the most recent deployment is used. 
Setting this to 1 will 
Defaults to 0
.EXAMPLE
Get the operations for the most recent deployment for "my group"

Get-LastDeploymentOperation -ResourceGroupName "my group"

.EXAMPLE
Get the operations for the deployment before the most recent deployment for "my group"

Get-LastDeploymentOperation -ResourceGroupName "my group" -DeploymentsToSkip 1

#>
function Get-LastDeploymentOperation
{
    [CmdletBinding()]
    Param
    (
        [string]
        [Parameter(Mandatory=$true)]
        [ValidateNotNullOrEmpty()]
        $ResourceGroupName,

        [int]
        $DeploymentsToSkip=0
    )
    
    Get-AzureRmResourceGroupDeployment -ResourceGroupName $ResourceGroupName `
        | Sort-Object -Descending -Property Timestamp `
        | Select-Object -Skip $DeploymentsToSkip -First 1 `
        | Get-AzureRmResourceGroupDeploymentOperation
 
}