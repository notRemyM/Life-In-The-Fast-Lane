using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;
using System.Collections.Concurrent;

namespace LifeInTheFastLane
{
    internal class CreateImpl
    {
        public static void SingleThreadCreate(EntityCollection collection, ServiceClient client)
        {
            // Add a CreateRequest for each entity to the request collection.
            foreach (var entity in collection.Entities)
            {
                client.Create(entity);
            }
        }

        public static void MultiThreadCreate(EntityCollection collection, ServiceClient client, int configuredDegreeOfParallelism)
        {
            ConcurrentBag<EntityReference> newEntityRefs = [];

            Parallel.ForEach(collection.Entities,
                new ParallelOptions() { MaxDegreeOfParallelism = configuredDegreeOfParallelism},
                () =>
                {
                    //Clone the CrmServiceClient for each thread
                    return client.Clone();
                },
                (entity, loopState, index, threadLocalSvc) =>
                {
                    // In each thread, create entities and add them to the ConcurrentBag
                    // as EntityReferences
                    newEntityRefs.Add(
                        new EntityReference(
                            entity.LogicalName,
                            threadLocalSvc.Create(entity)
                            )
                        );

                    return threadLocalSvc;
                },
                (threadLocalSvc) =>
                {
                    //Dispose the cloned CrmServiceClient instance
                    threadLocalSvc?.Dispose();
                });
        }

    }
}
