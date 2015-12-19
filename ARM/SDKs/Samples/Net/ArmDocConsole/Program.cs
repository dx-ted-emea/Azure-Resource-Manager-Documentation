// The MIT License (MIT)

// Copyright (c) 2015 Microsoft DX TED EMEA

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using Microsoft.Azure.Management.Compute;
using Microsoft.Azure.Management.Compute.Models;
using Microsoft.Azure.Management.Network;
using Microsoft.Azure.Management.Network.Models;
using Microsoft.Azure.Management.Resources;
using Microsoft.Azure.Management.Resources.Models;
using Microsoft.Azure.Management.Storage;
using Microsoft.Azure.Management.Storage.Models;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Rest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ArmDocConsole
{
    class Program
    {
        static void Main()
        {
            ArmApiSample().Wait();
        }

        /// <summary>
        /// Creates new Virtual Machine using ARM APIs, one by one, building a complete Virtual Machine from the bottom up including:
        /// Resource Group, Storage Account, Public IP Address, Virtual Network, Network Interface Card and the Virtual Machine itself.
        /// 
        /// Sample showcases (among others):
        /// * Authentication against Azure AD
        /// * ARM API calls using REST
        /// * Arm API calls using Azure SDK for .NET
        /// * How parts of the deployment can be optimized using parallel tasks
        /// </summary>
        /// <returns></returns>
        async static Task ArmApiSample()
        {
//#error Replace tenantId, clientId and clientSecret and then remove or comment this row!!!

            // Authentication parameters
            const string tenantId = "yourtenantid";                             // Identifies your tenant in Azure AD, often your company name (<tenantid>.onmicrosoft.com)
            const string clientId = "1a11a111-bbb2-33c3-d4dd-55e55e5e55e5";     // ClientId as registered in Azure AD
            const string clientSecret = "yourclientsecret";                     // ClientSecret as registered in Azure AD

//#error Replace the values for storageAccountName and pipDnsName to a globally unique value and then remove or comment this row!!!
            // Globally unique values used in DNS names
            const string storageAccountName = "armstorage";                     // Globally unique name used for the created storage account, pick a name no one else could have chosen
            const string pipDnsName = "armdns";                                 // Globally unique name that will be used as part of the DNS name for your public IP and Virtual Machine

            // Parameters used in creation of Virtual Machine
            const string resourceGroup = "sampleresourcegroup";                 // Name of resource group to deploy to
            const string location = "North Europe";                             // Location of deployed resources
            const string pipAddressName = "pip001";                             // Internal name for Public IP Address
            const string vNetName = "vnet001";                                  // Internal name for Virtual Network
            const string vNetAddressPrefix = "10.0.0.0/16";                     // Virtual network IP Address Range
            const string vNetSubnetName = "subnet001";                          // Internal name for Subnet
            const string vNetSubnetPrefix = "10.0.0.0/24";                      // Subnet IP Address Range
            const string nicName = "nic001";                                    // Internal name for Network Interface Card, NIC
            const string nicIPConfig = "nicipconfig001";                        // Internal name for NIC Configuration
            const string vmName = "windowsvm001";                               // Internal (in the Virtual Network) name for Virtual Machine
            const string vmSize = "Standard_D2";                                // Virtual Machine Size, notice that different location provides availability of different VM Sizes
            const string vmAdminUsername = "sampleuser";                        // Username of created user
            const string vmAdminPassword = "P@ssword!";                         // Password of created user
            const string vmImagePublisher = "MicrosoftWindowsServer";           // Publisher of used source virtual harddrive image
            const string vmImageOffer = "WindowsServer";                        // Identifies offer from selected Publisher
            const string vmImageSku = "2012-R2-Datacenter";                     // Identifies SKU of selected offer
            const string vmImageVersion = "latest";                             // Version of selected Image SKU
            const string vmOSDiskName = "osdisk";                               // Internal name for VM Operating System Disk

            // Authenticate against Azure AD using client id and client secret
            var token = GetAccessToken(tenantId, clientId, clientSecret).AccessToken;
            var credentials = new TokenCredentials(token);

            // List subscriptions and save first subscription id (might be incorrect if you have several subscriptions)
            var subscriptionId = (await GetSubscriptionsAsync(token)).FirstOrDefault();
            if (subscriptionId == null) throw new Exception("No subscription found");

            // Create Resource Group
            //   Await the creation of the storage group since everything else in this deployment
            //   depend on the existense of the resource group.
            await CreateResourceGroupAsync(
                credentials, 
                subscriptionId, 
                resourceGroup, 
                location);

            // Create Storage Account Async
            var createStorageAccountTask = CreateStorageAccountAsync(
                credentials, 
                subscriptionId, 
                resourceGroup, 
                location, 
                storageAccountName);

            // Create Public IP Address Async
            var createPipTask = CreatePublicIPAddressAsync(credentials, 
                subscriptionId, 
                resourceGroup, 
                location, 
                pipAddressName, 
                pipDnsName);

            // Create Virtual Network Async
            var subnets = new[] { new Subnet(vNetSubnetPrefix, vNetSubnetName) };
            var createVNetTask = CreateVirtualNetworkAsync(
                credentials, 
                subscriptionId, 
                resourceGroup, 
                location, 
                vNetName, 
                vNetAddressPrefix, 
                subnets);

            // Wait for Public IP Address and Virtual Network to be created before continuing
            // creation of Virtual Network Interface, since it has a dependency to those.
            Console.WriteLine("Waiting for Public IP Address and Virtual Network to be created");
            Task.WaitAll(
                createPipTask,
                createVNetTask);
            Console.WriteLine("Proceeding");

            var pip = createPipTask.Result;
            var vNet = createVNetTask.Result;
            var subnet = vNet.Subnets.First();

            // Create Network Interface
            var createNicTask = CreateNetworkInterfaceAsync(
                credentials, 
                subscriptionId,
                resourceGroup, 
                location, 
                nicName, 
                nicIPConfig, 

                pip, 
                subnet);

            // Wait for Storage Account and Virtual Network Interface to be created before continuing
            // creation of Virtual Machine, since it has a dependency to those.
            Console.WriteLine("Waiting for Storage Account and Network Interface to be created");
            Task.WaitAll(
                createStorageAccountTask,
                createNicTask);
            Console.WriteLine("Proceeding");

            var storageAccount = createStorageAccountTask.Result;
            var nic = createNicTask.Result;

            // Create Virtual Machine
            //   This is last task to complete so await the creation of the Virtual Machine
            var vm = await CreateVirtualMachineAsync(
                credentials, 
                subscriptionId, 
                resourceGroup, 
                location, 
                storageAccountName, 
                vmName, 
                vmSize, 
                vmAdminUsername, 
                vmAdminPassword, 
                vmImagePublisher, 
                vmImageOffer, 
                vmImageSku, 
                vmImageVersion, 
                vmOSDiskName, 
                nic.Id);

            Console.WriteLine("Success!!!");
            Console.WriteLine($"VM ProvisioningState: {vm.ProvisioningState}");

        }

        /// <summary>
        /// Autenticates against Azure AD using clientId and clientSecret. Requires that you previously have registered
        /// an app in your tenantId's AD.
        /// </summary>
        /// <remarks>
        /// There are several other ways of authenticating against Azure AD and they all work in somewhat different ways
        /// but still very similar. This sample has the assumption that you've registered an Application in Azure AD and
        /// gotten the Client ID and Client Secret for that application.
        /// Also application must also have the authorization to access management.azure.com in order for this sample to
        /// work.
        /// </remarks>
        /// <param name="tenantId">Identifies your tenant in Azure AD, often the same as your company's name</param>
        /// <param name="clientId">ID that identifies this application to Azure AD</param>
        /// <param name="clientSecret">Secret password used to validate authentication of App</param>
        /// <returns>Returns AuthenticationResult containing AccessToken to be used in requests to other APIs</returns>
        private static AuthenticationResult GetAccessToken(string tenantId, string clientId, string clientSecret)
        {
            Console.WriteLine("Aquiring Access Token from Azure AD");
            AuthenticationContext authContext = new AuthenticationContext
                ("https://login.windows.net/" /* AAD URI */
                    + $"{tenantId}.onmicrosoft.com" /* Tenant ID or AAD domain */);

            var credential = new ClientCredential(clientId, clientSecret);

            AuthenticationResult token = authContext.AcquireToken("https://management.azure.com/", credential);

            Console.WriteLine($"Token: {token.AccessToken}");
            return token;
        }

        /// <summary>
        /// Showcase how you can call Azure ARM APIs directly using C# and .NET Framework without any additional SDK
        /// or NuGet Package. In order to use this method, the only thing you need is an authentication token from
        /// Azure AD.
        /// </summary>
        /// <param name="token">Valid AccessToken returned from Azure AD that identifies his caller as having
        /// assess rights to the ARM APIs</param>
        /// <returns>Returns a list of Subcription IDs that the identified application has access right to</returns>
        async private static Task<List<string>> GetSubscriptionsAsync(string token)
        {
            Console.WriteLine("Querying for subscriptions");
            const string apiVersion = "2015-01-01";

            var client = new HttpClient();
            client.BaseAddress = new Uri("https://management.azure.com/");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await client.GetAsync($"subscriptions?api-version={apiVersion}");

            var jsonResponse = response.Content.AsString();

            var subscriptionIds = new List<string>();
            dynamic json = JsonConvert.DeserializeObject(jsonResponse);

            for (int i = 0; i < json.value.Count; i++)
            {
                subscriptionIds.Add(json.value[i].subscriptionId.Value);
            }

            Console.WriteLine($"Found {subscriptionIds.Count} subscription(s)");
            return subscriptionIds;
        }

        /// <summary>
        /// Creates (or updates existing) resource group
        /// </summary>
        /// <param name="credentials">Credentials to authorize application</param>
        /// <param name="subscriptionId">SubscriptionID that identifies subscription to create resoruce group in</param>
        /// <param name="resourceGroup">Name of resource group</param>
        /// <param name="location">Location for resource group</param>
        /// <returns>Awaitable task</returns>
        private static async Task<ResourceGroup> CreateResourceGroupAsync(TokenCredentials credentials, string subscriptionId, string resourceGroup, string location)
        {
            Console.WriteLine($"Creating Resource Group {resourceGroup}");
            var resourceClient = new ResourceManagementClient(credentials) { SubscriptionId = subscriptionId };
            return await resourceClient.ResourceGroups.CreateOrUpdateAsync(resourceGroup,
                new ResourceGroup
                {
                    Location = location
                });
        }

        /// <summary>
        /// Creates storage account
        /// </summary>
        /// <param name="credentials">Credentials to authorize application</param>
        /// <param name="subscriptionId">SubscriptionID that identifies subscription to create resoruce in</param>
        /// <param name="resourceGroup">Name of resource group</param>
        /// <param name="location">Location for resource</param>
        /// <param name="storageAccountName">Globally unique name. Will be part of Fully Qualified Domain Name, FQDN, for Storage Account, i.e. storageAccountnName.blob.core.windows.net, etc.</param>
        /// <param name="accountType">Type of storage account to create</param>
        /// <returns>Awaitable task</returns>
        private static async Task<StorageAccount> CreateStorageAccountAsync(TokenCredentials credentials, string subscriptionId, string resourceGroup, string location, string storageAccountName, AccountType accountType = AccountType.StandardLRS)
        {
            Console.WriteLine("Creating Storage Account");
            var storageClient = new StorageManagementClient(credentials) { SubscriptionId = subscriptionId };
            return await storageClient.StorageAccounts.CreateAsync(resourceGroup, storageAccountName,
                new StorageAccountCreateParameters
                {
                    Location = location,
                    AccountType = accountType,
                });
        }

        /// <summary>
        /// Create Public IP Address (with DNS name)
        /// </summary>
        /// <param name="credentials">Credentials to authorize application</param>
        /// <param name="subscriptionId">SubscriptionID that identifies subscription to create resoruce in</param>
        /// <param name="resourceGroup">Name of resource group</param>
        /// <param name="location">Location for resource</param>
        /// <param name="pipAddressName">Internal name used to identify Public IP Address within your subscription</param>
        /// <param name="pipDnsName">Globally unique name. Will be part of Fully Qualified Domain Name, FQDN, used to access resources in Azure</param>
        /// <returns>Awaitable task</returns>
        private static Task<PublicIPAddress> CreatePublicIPAddressAsync(TokenCredentials credentials, string subscriptionId, string resourceGroup, string location, string pipAddressName, string pipDnsName)
        {
            Console.WriteLine("Creating Public IP");
            var networkClient = new NetworkManagementClient(credentials) { SubscriptionId = subscriptionId };
            var createPipTask = networkClient.PublicIPAddresses.CreateOrUpdateAsync(resourceGroup, pipAddressName,
                new PublicIPAddress
                {
                    Location = location,
                    DnsSettings = new PublicIPAddressDnsSettings { DomainNameLabel = pipDnsName },
                    PublicIPAllocationMethod = "Dynamic" // This sample doesn't support Static IP Addresses but could be extended to do so
                });

            return createPipTask;
        }

        /// <summary>
        /// Create Virtual Network
        /// </summary>
        /// <param name="credentials">Credentials to authorize application</param>
        /// <param name="subscriptionId">SubscriptionID that identifies subscription to create resoruce in</param>
        /// <param name="resourceGroup">Name of resource group</param>
        /// <param name="location">Location for resource</param>
        /// <param name="vNetName">Internal name used to identify Virtual Network within your subscription</param>
        /// <param name="vNetAddressPrefix">IP Address range for Virtual Network</param>
        /// <param name="subnets">List of Subnets to be provisioned inside the Virtual Network</param>
        /// <returns>Awaitable task</returns>
        private static Task<VirtualNetwork> CreateVirtualNetworkAsync(TokenCredentials credentials, string subscriptionId, string resourceGroup, string location, string vNetName, string vNetAddressPrefix, Subnet[] subnets)
        {
            Console.WriteLine("Creating Virtual Network");
            var networkClient1 = new NetworkManagementClient(credentials) { SubscriptionId = subscriptionId };
            var createVNetTask = networkClient1.VirtualNetworks.CreateOrUpdateAsync(resourceGroup, vNetName,
                new VirtualNetwork
                {
                    Location = location,
                    AddressSpace = new AddressSpace(new[] { vNetAddressPrefix }),
                    Subnets = subnets
                });

            return createVNetTask;
        }

        /// <summary>
        /// Create Virtual Network Interface Card, NIC
        /// </summary>
        /// <param name="credentials">Credentials to authorize application</param>
        /// <param name="subscriptionId">SubscriptionID that identifies subscription to create resoruce in</param>
        /// <param name="resourceGroup">Name of resource group</param>
        /// <param name="location">Location for resource</param>
        /// <param name="nicName">Internal name for NIC</param>
        /// <param name="nicIPConfigName">Internal name for NIC Configuration. Sample only provides one configuration, but could be extended to provide more</param>
        /// <param name="pip">Public IP Address to be assigned to NIC</param>
        /// <param name="subnet">Subnet to use for current configuration</param>
        /// <returns>Awaitable task</returns>
        private static Task<NetworkInterface> CreateNetworkInterfaceAsync(TokenCredentials credentials, string subscriptionId, string resourceGroup, string location, string nicName, string nicIPConfigName, PublicIPAddress pip, Subnet subnet)
        {
            Console.WriteLine("Creating Network Interface");
            var networkClient = new NetworkManagementClient(credentials) { SubscriptionId = subscriptionId };
            var createNicTask = networkClient.NetworkInterfaces.CreateOrUpdateAsync(resourceGroup, nicName,
                new NetworkInterface()
                {
                    Location = location,
                    IpConfigurations = new[] {
                        new NetworkInterfaceIPConfiguration
                        {
                            Name = nicIPConfigName,
                            PrivateIPAllocationMethod = "Dynamic",
                            PublicIPAddress = pip,
                            Subnet = subnet
                        }
                    }
                });

            return createNicTask;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="credentials">Credentials to authorize application</param>
        /// <param name="subscriptionId">SubscriptionID that identifies subscription to create resoruce in</param>
        /// <param name="resourceGroup">Name of resource group</param>
        /// <param name="location">Location for resource</param>
        /// <param name="storageAccountName">Name of Storage Account used to store Virtual HDD on</param>
        /// <param name="vmName">Name of Virtual Machine</param>
        /// <param name="vmSize">VM Size as allowed for the current location</param>
        /// <param name="vmAdminUsername">Admin username</param>
        /// <param name="vmAdminPassword">Admin password</param>
        /// <param name="vmImagePublisher">Publisher of VM Image</param>
        /// <param name="vmImageOffer">Offer from Publisher</param>
        /// <param name="vmImageSku">SKU of Offer</param>
        /// <param name="vmImageVersion">Version of SKU</param>
        /// <param name="vmOSDiskName">Internal name for operating system disk</param>
        /// <param name="nicId">NIC Identifer, used to attach NIC to VM</param>
        /// <returns>Awaitable task</returns>
        private static async Task<VirtualMachine> CreateVirtualMachineAsync(TokenCredentials credentials, string subscriptionId, string resourceGroup, string location, string storageAccountName, string vmName, string vmSize, string vmAdminUsername, string vmAdminPassword, string vmImagePublisher, string vmImageOffer, string vmImageSku, string vmImageVersion, string vmOSDiskName, string nicId)
        {
            Console.WriteLine("Creating Virtual Machine (this may take a while)");
            var computeClient = new ComputeManagementClient(credentials) { SubscriptionId = subscriptionId };
            var vm = await computeClient.VirtualMachines.CreateOrUpdateAsync(resourceGroup, vmName,
                new VirtualMachine
                {
                    Location = location,
                    HardwareProfile = new HardwareProfile(vmSize),
                    OsProfile = new OSProfile(vmName, vmAdminUsername, vmAdminPassword),
                    StorageProfile = new StorageProfile(
                        new ImageReference
                        {
                            Publisher = vmImagePublisher,
                            Offer = vmImageOffer,
                            Sku = vmImageSku,
                            Version = vmImageVersion
                        },
                        new OSDisk
                        {
                            Name = vmOSDiskName,
                            Vhd = new VirtualHardDisk($"http://{storageAccountName}.blob.core.windows.net/vhds/{vmOSDiskName}.vhd"),
                            Caching = "ReadWrite",
                            CreateOption = "FromImage"
                        }),
                    NetworkProfile = new NetworkProfile(
                        new[] { new NetworkInterfaceReference { Id = nicId } }),
                    DiagnosticsProfile = new DiagnosticsProfile(
                        new BootDiagnostics
                        {
                            Enabled = true,
                            StorageUri = $"http://{storageAccountName}.blob.core.windows.net"
                        })
                });

            return vm;
        }
    }
}
