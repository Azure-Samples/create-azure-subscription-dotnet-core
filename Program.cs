using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

// Azure dependencies
using Microsoft.Rest;
using Microsoft.Rest.Azure.Authentication;
using Microsoft.Azure.Management.Billing;
using Microsoft.Azure.Management.Billing.Models;
using Microsoft.Azure.Management.Subscription;
using Microsoft.Azure.Management.Subscription.Models;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Diagnostics;

namespace CreateSubSample
{
    class Program
    {
        public static IConfiguration Configuration { get; set; }
        private static void Write(string format, params object[] items)
        {
            Console.WriteLine(String.Format(format, items));
        }
        static void Main(string[] args)
        {
            var armCreds = GetCreds();
            var billClient = new BillingManagementClient(armCreds);

            Write("Listing some enrollment accounts ...");
            var enrollmentAccounts = billClient.EnrollmentAccounts.List().Value;

            Write("{0} enrollment account(s) found.  Press ENTER to see them.", enrollmentAccounts.Count);
            Console.ReadLine();

            EnrollmentAccount chosenAccount = new EnrollmentAccount();
            foreach (var enrollmentAccount in enrollmentAccounts)
            {
                Write("\tName: {0}", enrollmentAccount.PrincipalName);
                Write("\tEnrollment account ID: {0}", enrollmentAccount.Name);
                ConsoleKey response;
                do 
                {
                    Write("Use this enrollment account to create subscriptions? [y/n] ");
                    response = Console.ReadKey(false).Key;
                    if (response != ConsoleKey.Enter) {
                        Write(Environment.NewLine);
                    }
                } while (response != ConsoleKey.Y && response != ConsoleKey.N);

                if (response == ConsoleKey.Y)
                {
                    chosenAccount = enrollmentAccount;
                }
            };

            var subClient = new SubscriptionClient(armCreds);
            Write("Press ENTER to create a subscription");
            Console.ReadLine();

            Write("Creating a new subscription, please wait...");

            var subscriptionParams = new SubscriptionCreationParameters()
            {
                DisplayName = "My New Programmatically Created Subscription",
                OfferType = "MS-AZR-0017P",
                Owners = null
            };

            var newSub = subClient.SubscriptionFactory.CreateSubscriptionInEnrollmentAccount(chosenAccount.Name, subscriptionParams);

            Write("\tSubId: {0}", newSub.SubscriptionLink);
            Console.ReadLine();

        }

        private static ServiceClientCredentials GetCreds()
        {
            SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            Configuration = builder.Build();

            Console.WriteLine($"option1 = {Configuration["Option1"]}");
            
            // Add the service principal to your enrollment account https://aka.ms/easubcreationpublicpreview
            // az role assignment create --role Owner --assignee {appId} --scope /providers/Microsoft.Billing/enrollmentAccounts/{enrollmentAccountId}
            var creds = ApplicationTokenProvider.LoginSilentAsync(
                Configuration["tenantId"],  // Tenant
                Configuration["appId"],     // Id of App registration (web app)
                Configuration["secret"]     // Secret
            ).GetAwaiter().GetResult();

            return creds;
        }
    }
}
