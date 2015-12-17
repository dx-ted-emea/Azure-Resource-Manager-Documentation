

[Is dev test process for extensions]

[Is it only about Windows ?]
[Where supported OS is specified ?]
[How can I see my extension ( which is private mode) in portal ?]
[How can I see my extension ( which is private mode) in portal ?]
[Is is possible that not only one extensions can be running on VM on same time?]
[Can my extensions get some updates from my web site ?]
[How testing process from Microsoft side looks – how you validate our extensions? ]
[Can VM extensions check to updates of application and make that updates?]
[How extension should be documented?]
[Do not (bad practice from real life ) 


## Is dev test process for extensions
 Yes, there is – you can publish your extension which will be visible only for your subscription and test it from SDK and UI ( azure.com portal ). Be care that this is not a really private space – it just not show your extension in list of available extensions for all users, if user know extension name and version they can manually add it. 

## Is it only about Windows ?
A: Linux supported as well as Windows. Actually almost everything which works under HyperV may be published here.


## Where supported OS is specified ?
A: It specified here in Extension.def.xml file like 
  <SupportedOS>Windows</SupportedOS>

## How can I see my extension ( which is private mode) in portal ?
A: Use this link - https://portal.azure.com/?HubsExtension_ItemHideKey=<ext_name >  ( for example 
https://portal.azure.com/?HubsExtension_ItemHideKey=ESETFileSecurity )


## Is is possible that not only one extensions can be running on VM on same time?
A: Yes, user can install several extensions on VM, so not only your extension can be running there. Please keep in mind that sharing resource can be changed/readed by some one other.


## Can my extensions get some updates from my web site ?
A: Yes, it’s possible – incoming to Azure traffic is free  But keep in mind that user may change default settings on VM and it may happened that no traffic allowed, some default port isn’t open, HTTPS is disabled, etc.

## How testing process from Microsoft side looks – how you validate our extensions? 
A: We do validate your xml/json/config files to check that they are correct, but we do not check functionality/any scope/ of your extension – this part is completely on your side. We surely help you to make your application be better, help you with publishing updates for app, but testing of how your application works as VM extension is on your side.

Q: Can VM extensions check to updates of application and make that updates?
A: Yes. If your VM extension ( like many other Windows application ) checking from time to time for never version, download it and update it, then you may use this functionality here as well. Hidded 
Q: How extension should be documented?
A: It would be great if on your web site will be a landing page dedicated to this VM extensions – not just to product  ( like Windows Antivirus ), but to VM extension product ( like VM extension Windows Anvtirus ). This page should contain basic information with description of functionality of extensions, licensing model ( how it works without licence, where to buy licence, how to use existing licences, etc ) and so on. Usually adding some VM extension specific information to existing page with product description works good enough, but if you improve it a little it may bring you more benefits. So on page dedicated to VM extension it’s better to cover most important topics which end-users will looking for – I split their questions into two areas – business and technical.
So business side of topics:
1)	Benefits of running VM in Azure with ESET VM extension ( ‘Why?’ ) ( example – VM are more secure )
2)	Licensing part – how can I get/use/re-use existing licenses. What will happen in license is not updated. (‘How to buy?’) ( example – you can run ESET with default setting, no licence key required to start working, but if you need fresh updates –please get licence key )
3)	How get I support for this product from ESET (‘Still have questions’ ) ( example – here’s web form and our contacts )

Technical part may include :
1)	How it works from tech side – does it have default virus base, how to configure/customize VM extension
2)	Performance topics – how running extension effect on VM ( example – it’s doesn’t takes too much resources in most of time, but significantly increase level of security for your VM ). What kind of resources VM ext using ( example : 120 Mb on disk to store virus DB, 20 GM memory for extension, and less than 0.5% of CPU )
3)	Advanced users – how to operate with VM extension using Powershell SDK.


Do not (bad practice from real life ) :
-	Put non-working links on page with description ( something which refers to 404 or nowhere, I got examples of that )
-	Don’t use forever loops :  like refer ‘please see more’ to another page and on another page ‘please see more’ with referring to previous page
-	Using non-English language page for incoming users– please use English by default.   
Good documentation may seriously increase popularity of VM extension and it make sense to put some efforts on that. 
Good examples are :
                Microsoft AntiMailware : http://azure.microsoft.com/blog/2014/10/30/microsoft-antimalware-for-azure-cloud-services-and-virtual-machines/
                Docker VM extension ( looks complex, but okay for docker ) :  https://azure.microsoft.com/en-us/documentation/articles/virtual-machines-docker-with-portal/

