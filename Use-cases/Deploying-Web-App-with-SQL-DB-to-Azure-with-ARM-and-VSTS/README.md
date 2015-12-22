# Deploying a Web application to Azure using Visual Studio Team Services (VSTS) and ARM templates

#
##Overview
In this scenario we will walk you through the necessary steps to deploy an application with ARM templates and VSTS to the Azure platform. There are two variants  in this scenario that enable different DevOps practices.

- Deploying and Web app by using Build process

- Deploying and Web app by using Release management


##Deploying to Azure using an ARM template task in the build process
In this scenario we focus on VSTS as a source code repository, Continuous Integration (CI) and Continuous Deployment (CD) components to manage the code. Define a build process with an ARM template task as Infrastructure as Code (IaC) component, conduct tests and deploy your application to Azure

[Deploying a Web application using the build process](../Deploying-Web-App-with-SQL-DB-to-Azure-with-ARM-and-VSTS/Deploying-Web-app-using-Build-process.md) 


##Deploying to Azure using ARM templates in Release management
In this scenario we focus on VSTS as a source code repository, Continuous Integration(CI), Continuous Deployment(CD) and Release Management(RM) components to manage the code. Release management is used for deployment of the application to multiple environments in Azure using ARM templates to build these environments and with a approval and acceptance chain.

[Deploying and Web app by using Release management](../Deploying-Web-App-with-SQL-DB-to-Azure-with-ARM-and-VSTS/Deploying-Web-app-using-RM.md)







