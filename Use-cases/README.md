> Azure Resource Manager Community Documentation - Beta Version

> Work in progress - This community driven documentation is considered to be in preview stage at this time. Documentation might contain errors, might not cover every aspect or might lack complete parts, even important parts. Please help us make this documentation better by [contributing](CONTRIBUTING.md) anything that you think would make it better.


---

# Scenarios
The [ARM](../ARM/README.md) section gives you a good walkthrough of the available ARM APIs, and how to create and use ARM templates. This section gives some examples of how you can use ARM templates in different scenarios.

|Scenario|Summary|
|--------|-------|
|[Web App and SQL Database](web-app-and-sql-database/README.md)| Create an App Service Plan with a Web App and an accompanying SQL Database. The template configures auto-scaling on the Web App, and gives examples of using different ARM APIs to scale the database|
|[Using ARM templates in DevOps scenarios](Deploying-Web-App-with-SQL-DB-to-Azure-with-ARM-and-VSTS)| Deploy your application to Azure using ARM template tasks in Visual Studio Team Services directly from your build process or through Release Management|

# TODO
This section contains notes for things to consider including. We should probably move this to Issues ;-)

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
    * ~~create server and database~~
    * ~~simple template to scale database~~
    * create database and import
    * elastic pools
  * Service Fabric?
  * KeyVault?
  * Search?
* Joining it up
  * Web App + Traffic Manager + Autoscale + Redis + Database
  * WAF + nginx on Linux in VM Scale Set + MySQL 
* ???


