> Azure Resource Manager Community Documentation - Beta Version

> Work in progress - This community driven documentation is considered to be in preview stage at this time. Documentation might contain errors, might not cover every aspect or might lack complete parts, even important parts. Please help us make this documentation better by [contributing](CONTRIBUTING.md) anything that you think would make it better.

---

## Validation

Currently the guidance for verifying UIDefinition files for the Portal UI is as follows:

> **Guide to create a solution template for Azure Marketplace**<br/>
> [Create your solution template offer in the Publishing Portal > 4. Get your topology versions certified](https://azure.microsoft.com/en-us/documentation/articles/marketplace-publishing-solution-template-creation/)
>
> *You can also validate the create experience without the actual deployment for the customer by using the following steps:*
> 
> *Save the createUiDefinition.json and generate the absolute URL. The URL must be publicly accessible.*
> *Encode the URL by using the tool at http://www.url-encode-decode.com/.*
> *Replace the bold text with the location (encoded URL) of the createUiDefinition.json that needs validation.*
> *https://portal.azure.com/?clientOptimizations=false#blade/Microsoft_Azure_Compute/CreateMultiVmWizardBlade/internal_bladeCallId/anything/internal_bladeCallerParams/ {"initialData":{},"providerConfig":{"createUiDefinition":"http://yoururltocreateuidefinition.jsonURLencoded"}}*
> 
> *Copy and paste the URL in any browser and view the customer experience of your createUiDefinition.json file.*

You can also leverage [ARMViz](http://armviz.io) to simplify this validation process. Launch the Portal UI Editor. Then paste the JSON from the `createUiDefinition.json` into the textbox in the Portal UI Editor. Click the **Preview** button to launch the blades in the Portal for testing the UI. Much simpler process.

![ARMViz](/Marketplace/images/armviz.png)