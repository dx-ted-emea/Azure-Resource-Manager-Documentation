# Debugging MSDeploy errors with Azure Web Apps
When you use the MSDeploy site extension to deploy web app content from an ARM template, the errors you get back are generally pretty vague. ** TODO - Add link to scenario when written up **

```json
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
```

To find out what the underlying error was, you need to use the site's [Kudu Console](https://github.com/projectkudu/kudu/wiki/Kudu-console).

** TODO - Opening Kudu - portal
** TODO - Opening Kudu - scm url

** TODO - Opening Kudu - navigating to the MSDeploy folder, open appManagerLog.xml

** TODO - include sample log file
