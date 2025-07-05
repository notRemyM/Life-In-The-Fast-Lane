using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;

namespace LifeInTheFastLane
{
    internal class CreateMultipleImpl
    {
        public static void SingleThreadCreate(EntityCollection collection, ServiceClient client)
        {
            // Clean single-threaded implementation shamelessly taken from Mark Carrington's blog post
            // https://markcarrington.dev/2020/12/04/improving-insert-update-delete-performance-in-d365-dataverse/
            ExecuteMultipleRequest executeMultiReq = new()
            {
                Requests = [],
                Settings = new ExecuteMultipleSettings
                {
                    ContinueOnError = false,
                    ReturnResponses = false
                }
            };

            int maxRequestsPerBatch = 100;

            foreach (var entity in collection.Entities)
            {
                client.Create(entity);

                executeMultiReq.Requests.Add(new CreateRequest { Target = entity });
                if (executeMultiReq.Requests.Count == maxRequestsPerBatch)
                {
                    client.Execute(executeMultiReq);
                    executeMultiReq.Requests.Clear();
                }

            }

            if (executeMultiReq.Requests.Count > 0)
            {
                client.Execute(executeMultiReq);
            }
        }

        public static void MultiThreadCreate(EntityCollection collection, ServiceClient client, int configuredDegreeOfParallelism)
        {
            int maxRequestsPerBatch = 100;

            // Clean multi-threaded implementation shamelessly taken from Mark Carrington's blog post (again)
            // https://markcarrington.dev/2020/12/04/improving-insert-update-delete-performance-in-d365-dataverse/

            Parallel.ForEach(collection.Entities,
              new ParallelOptions { MaxDegreeOfParallelism = configuredDegreeOfParallelism },
              () => new
              {
                  Service = client.Clone(),
                  EMR = new ExecuteMultipleRequest
                  {
                      Requests = [],
                      Settings = new ExecuteMultipleSettings
                      {
                          ContinueOnError = false,
                          ReturnResponses = false
                      }
                  }
              },
              (entity, loopState, index, threadLocalState) =>
              {
                  threadLocalState.EMR.Requests.Add(new CreateRequest { Target = entity });
                  if (threadLocalState.EMR.Requests.Count == maxRequestsPerBatch)
                  {
                      threadLocalState.Service.Execute(threadLocalState.EMR);
                      threadLocalState.EMR.Requests.Clear();
                  }
                  return threadLocalState;
              },
              (threadLocalState) =>
              {
                  if (threadLocalState.EMR.Requests.Count > 0)
                  {
                      threadLocalState.Service.Execute(threadLocalState.EMR);
                  }
                  threadLocalState.Service.Dispose();
              });

        }
    }
}
