# ARM REST API 

## Introduction

Behind every call to Azure Resource Manager, behind every deployed template, behind every configured storage account there is one or several calls to the Azure Resource Manager’s RESTful API. This section is devoted to give you an understanding how you can call those APIs from within your applications. We’re not going through every API calls available, but rather going through how you can connect to the APIs and how results are received. Once you understand how that works you can easily read the [Azure Resource Manager REST API Reference](https://msdn.microsoft.com/en-us/library/azure/dn790568.aspx) to find out exactly how all other APIs, not documented here, work.

## Authentication

