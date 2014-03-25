using System.Net;
using Common;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;

namespace StorageRole
{
    public class WorkerRole : RoleEntryPoint
    {
        public override void Run()
        {
            // TODO: Setup service bus.
            // For more information on using Azure Service Bus visit http://www.windowsazure.com/en-us/documentation/articles/service-bus-dotnet-how-to-use-topics-subscriptions/.

            SubscriptionClient subscriptionClient = SubscriptionClient.CreateFromConnectionString(
                "Azure Service Bus Namespace Connection String",
                "Azure Service Bus Topic Name",
                "Azure Service Bus Subscription Name");

            // TODO: Setup storage account;
            // For more information on using Azure Storage visit http://www.windowsazure.com/en-us/documentation/articles/storage-dotnet-how-to-use-table-storage-20/.

            var storageAccount = new CloudStorageAccount(new StorageCredentials(
                "Azure Storage Account Name",
                "Azure Storage Access Key"), false);

            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("volunteers");

            table.CreateIfNotExists();

            while (true)
            {
                BrokeredMessage message = subscriptionClient.Receive();

                if (message != null)
                {
                    var volunteer = message.GetBody<Volunteer>();

                    if (volunteer != null)
                    {
                        var volunteerEntity = new VolunteerTableEntity();

                        volunteerEntity.FirstName = volunteer.FirstName;
                        volunteerEntity.LastName = volunteer.LastName;
                        volunteerEntity.PartitionKey = volunteer.State;
                        volunteerEntity.PhoneNumber = volunteer.PhoneNumber;
                        volunteerEntity.RowKey = volunteer.EmailAddress;

                        TableOperation tableOperation = TableOperation.InsertOrReplace(volunteerEntity);

                        table.Execute(tableOperation);

                        message.Complete();
                    }
                }
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            return base.OnStart();
        }
    }
}