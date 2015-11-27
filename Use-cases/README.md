# Scenarios
This section walks through a number of scenarios for ARM templates.

*** TODO - working notes
Things to discuss:
* what goes here?
* Starter template (VM + LB) is in templates section
* The Azure quickstarts repos contains some of the templates below, or at least some that are similar. Should we use templates from quickstarts (adding to it if needed), but then add the commentary around the template, how it is constructed etc in this repo?


* template stuff - or should this go in the ARM templates section?
  * outputs - specifying, and retrieving values in PowerShell/CLI
  * linked templates. How, why. Using linked templates for conditional behaviour 
* ARM resources
  * Compute
    * ~~single VM [in templates section]~~
    * ~~VMs with load balancer (using loop for the VMs) [in templates section]~~
    * VM Scale Sets
  * App Service
    * Create plan and include Web App with MS Deploy + config settings
    * simple template to scale plan
  * SQL Database
    * create server and database
    * simple template to scale database
    * create database and import
    * elastic pools
  * Service Fabric?
  * KeyVault?
  * Search?
* Joining it up
  * Web App + Traffic Manager + Autoscale + Redis + Database
  * Linux + 
* ???

