#!/bin/bash
# Pass the resource group name as the 1st parameter to this script
# Pass the number of deployments to skip as the 2nd parameter (optional)
resourceGroupName=$1
if [ "x$2" = "x" ]; then deploymentsToSkip=0; else deploymentsToSkip=$2; fi

# Get the name of the last deployment
deploymentName=$(azure group deployment list $resourceGroupName --json | jq "[.[] | {name:.name, timestamp: .properties.timestamp } ] | sort_by(.timestamp) | reverse | .[$deploymentsToSkip].name" --raw-output)

# Get a summary of the operations for the last deployment 
azure group deployment operation list --resource-group $resourceGroupName --name $deploymentName --json | jq "[.[] | .properties | { provisioningState : .provisioningState, timestamp: .timestamp, resourceType:.targetResource.resourceType, resourceName:.targetResource.resourceName, error:.statusMessage.error}] | sort_by(.timestamp)"
