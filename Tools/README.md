# Azure Resource Manager Tools

As described in other sections of this documentation, Azure Resource Manager provides a flexible and consistent model to define a Cloud Deployment made of PaaS and IaaS resources in a consistent and repeatable way.
Of course the next step in this process is to check how this powerful technology can be put to work in a DevOps process where ARM templates are integrating part of the Infrastructure as Code toolkit available to Ops people to rapidly create and instrument Test, Staging and Production environments.

Microsoft offers tooling on top of the ARM rest APIs to make it easier to author and put ARM templates in action and these will be discussed in specific articles.

* [Command Line interface](CLI.md): the CLI is a cross-platform command line shell based on NodeJS that can be used on Linux, Mac and Windows to interact with Azure ARM APIs
* [Powershell](Powershell.md): Powershell is a *windows only* shell to interact with Azure ARM APIs that offers a rich and extensible programming model
* [Visual Studio](Visual-studio.md): Visual Studio is Microsoft's developer tool that provides extensions to work with ARM templates, providing benefits as suggestions and syntax checking
* [New Azure Portal](Portal.md): The new Azure Portal is accessible at [https://portal.azure.com](https://portal.azure.com) and gives the ability to test ARM templates with a easy to use GUI and to manage Resource Groups created by ARM template deployments in a visual environment. That is where the Ops Dashboard can be built
