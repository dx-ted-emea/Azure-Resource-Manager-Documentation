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

** TODO - Opening Kudu - navigating to the MSDeploy folder (D:\home\LogFiles\SiteExtensions\MSDeploy), open appManagerLog.xml

** TODO - include sample log file

```xml
<?xml version="1.0" encoding="utf-8"?>
<entries>
    <entry time="2015-11-25T12:29:11.3340574+00:00" type="Message">
        <message>Downloading metadata for package path '' from blob 'http://example.com'</message>
    </entry>
    <entry time="2015-11-25T12:29:11.3506776+00:00" type="Error">
        <message>AppGallery Deploy Failed: 'System.ArgumentException: The argument must not be empty string.
Parameter name: blobAbsoluteUriString
   at Microsoft.WindowsAzure.StorageClient.CommonUtils.AssertNotNullOrEmpty(String paramName, String value)
   at Microsoft.WindowsAzure.StorageClient.CloudBlobClient.GetBlobReference(String blobAddress, Nullable`1 snapshotTime)
   at Microsoft.Web.Deployment.WebApi.AppGalleryPackage.IsPremiumApp()
   at Microsoft.Web.Deployment.WebApi.DeploymentController.CheckCanDeployIfAppIsPremium(AppGalleryPackageInfo packageInfo, Boolean&amp;amp; isPremium)'</message>
    </entry>
    <entry time="2015-11-25T12:29:11.6153171Z" type="Message">
        <message>Downloading package path '' from blob 'http://example.com'</message>
    </entry>
    <entry time="2015-11-25T12:29:11.6153171Z" type="Error">
        <message>Failed to download package.</message>
    </entry>
    <entry time="2015-11-25T12:29:11.6309427Z" type="Error">
        <message>AppGallery Deploy Failed: 'System.ArgumentException: The argument must not be empty string.
Parameter name: blobAbsoluteUriString
   at Microsoft.WindowsAzure.StorageClient.CommonUtils.AssertNotNullOrEmpty(String paramName, String value)
   at Microsoft.WindowsAzure.StorageClient.CloudBlobClient.GetBlobReference(String blobAddress, Nullable`1 snapshotTime)
   at Microsoft.Web.Deployment.WebApi.AppGalleryPackage.&lt;Download&gt;d__4.MoveNext()
--- End of stack trace from previous location where exception was thrown ---
   at System.Runtime.CompilerServices.TaskAwaiter.ThrowForNonSuccess(Task task)
   at System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   at Microsoft.Web.Deployment.WebApi.AppGalleryPackage.&lt;Download&gt;d__0.MoveNext()
--- End of stack trace from previous location where exception was thrown ---
   at System.Runtime.CompilerServices.TaskAwaiter.ThrowForNonSuccess(Task task)
   at System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   at Microsoft.Web.Deployment.WebApi.DeploymentController.&lt;DownloadAndDeployPackage&gt;d__b.MoveNext()'</message>
    </entry>
</entries>
```
