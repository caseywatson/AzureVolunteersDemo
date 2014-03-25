using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
using Common;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using SendGridMail;

namespace EmailRole
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

            // TODO: Setup Sendgrid. 
            // For more information on using Sendgrid with Azure visit http://www.windowsazure.com/en-us/documentation/articles/sendgrid-dotnet-how-to-send-email/.

            var sendGridClient = Web.GetInstance(new NetworkCredential(
                "Sendgrid User Name",
                "Sendgrid Password"));

            while (true)
            {
                BrokeredMessage message = subscriptionClient.Receive();

                if (message != null)
                {
                    var volunteer = message.GetBody<Volunteer>();

                    if (volunteer != null)
                    {
                        var emailMessage = SendGrid.GetInstance();

                        emailMessage.From = new MailAddress("volunteers@demo.net");
                        emailMessage.To = new[] {new MailAddress(volunteer.EmailAddress)};
                        emailMessage.Subject = "Further Volunteer Instructions";
                        emailMessage.Text = "Sorry. There are no further instructions.";

                        sendGridClient.Deliver(emailMessage);

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
