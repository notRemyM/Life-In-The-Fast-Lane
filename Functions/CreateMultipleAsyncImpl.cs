using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using System.Collections.Concurrent;

namespace LifeInTheFastLane
{
    internal class CreateMultipleAsyncImpl
    {
        public static async Task CreateAsync(EntityCollection collection, ServiceClient client, int configuredDegreeOfParallelism)
        {
            ConcurrentBag<EntityReference> newEntityRefs = [];
            string tableSchemaName = "rm_DemoEntity";
            string tableLogicalName = tableSchemaName.ToLower();
            int chunkSize = 100;

            await Parallel.ForEachAsync(
                            source: collection.Entities.Chunk(chunkSize),
                            parallelOptions: new ParallelOptions() { MaxDegreeOfParallelism = configuredDegreeOfParallelism },
                            async (entities, token) =>
                            {
                                
                                CreateMultipleRequest createMultipleRequest = new()
                                {
                                    Targets = new EntityCollection(entities)
                                    {
                                        EntityName = tableLogicalName
                                    }
                                };

                                CreateMultipleResponse response = (CreateMultipleResponse)await client.ExecuteAsync(
                                     request: createMultipleRequest,
                                     cancellationToken: token);

                                //// Set the id values for the entities
                                //for (int i = 0; i < entities.Length; i++)
                                //{
                                //    entities[i].Id = response.Ids[i];
                                //}

                            });
        }
    }
}
