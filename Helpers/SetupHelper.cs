using Microsoft.Crm.Sdk.Messages;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System.Configuration;
using System.Net;

namespace LifeInTheFastLane
{
    internal class SetupHelper
    {
        private static string GetConnectionStringFromAppConfig(string name)
        {
            //Adapted from PowerApps-Samples
            //Verify cds/App.config contains a valid connection string with the name.
            try
            {
                return ConfigurationManager.ConnectionStrings[name].ConnectionString;
            }
            catch (Exception)
            {
                Console.WriteLine("Please set cds connection string in config before running this application");
                return string.Empty;
            }
        }

        public static ServiceClient GetServiceClient()
        {
            try
            {
                //Shamelessly borrowed from PowerApps-Samples/dataverse/orgsvc/C#-NETCore/BulkOperations/ParallelCreateUpdateMultiple/Program.cs
                //Change max connections from .NET to a remote service default: 2
                ServicePointManager.DefaultConnectionLimit = 65000;
                //Bump up the min threads reserved for this app to ramp connections faster - minWorkerThreads defaults to 4, minIOCP defaults to 4
                ThreadPool.SetMinThreads(100, 100);
                //Turn off the Expect 100 to continue message - 'true' will cause the caller to wait until it round-trip confirms a connection to the server
                ServicePointManager.Expect100Continue = false;
                //Can decrease overall transmission overhead but can cause delay in data packet arrival
                ServicePointManager.UseNagleAlgorithm = false;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                //Once all that's set, get ServiceClient
                Console.WriteLine("Getting ServiceClient...");
                ServiceClient client = new(GetConnectionStringFromAppConfig("Connect"));
                return client;
            }
            catch
            {
                Console.WriteLine("Failed to connect to Dataverse. Please check your connection or connection string and try again.");
                throw;
            }

        }

        public static void CreateBulkDeleteJobForDemoEntity(ServiceClient service)
        {
            QueryExpression qe = new("rm_demoentity");

            BulkDeleteRequest req = new()
            {
                QuerySet = [qe],
                StartDateTime = DateTime.Now,
                RecurrencePattern = string.Empty,
                SendEmailNotification = false,
                JobName = "Demo Bulk Delete Job " + DateTime.Now,
                ToRecipients = [],  // Required
                CCRecipients = []   // Required
            };

            // we don't care about the response unless it faults
            service.Execute(req);
            Console.WriteLine("Bulk delete created!");
        }

        public static EntityCollection GetCollectionOfEntitiesForCreate(int entityAmount)
        {
            EntityCollection collection = new();
            foreach (int index in Enumerable.Range(0, entityAmount))
            {
                Entity ent = new("rm_demoentity", Guid.NewGuid());
                ent.Attributes.Add("rm_demofield1", "Demo Field Content");
                ent.Attributes.Add("rm_demofield2", "Yet Another Demo Field Content");
                ent.Attributes.Add("rm_demofield3", "Even More Demo Field Content");
                ent.Attributes.Add("rm_demofield4", "I Can't Believe It's Not Demo Field Content");
                ent.Attributes.Add("rm_demofield5", "Hmm, It Could Be Demo Field Content");
                ent.Attributes.Add("rm_demofield6", "Don't You Forget About Demo Field Content");
                ent.Attributes.Add("rm_demofield7", "I've Run Out Of Witty Stuff");
                ent.Attributes.Add("rm_demofield8", "Demo Field Content");
                ent.Attributes.Add("rm_demofield9", "Demo Field Content");
                ent.Attributes.Add("rm_demofield10", "Demo Field Content");
                collection.Entities.Add(ent);
            }
            return collection;

        }
    }
}
