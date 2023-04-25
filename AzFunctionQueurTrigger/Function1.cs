using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace AzFunctionQueurTrigger
{
    public class Function1
    {
        [FunctionName("Function1")]
        public void Run([QueueTrigger("myqueue-items", Connection ="MyDemo" )]string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        }
    }
}
