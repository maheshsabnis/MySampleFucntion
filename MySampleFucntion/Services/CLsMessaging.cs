using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySampleFucntion.Services
{
    public class ClsMessaging
    {
        public async Task AddMessageAsync(string data)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse("CONN");
            CloudQueueClient cloudQueueClient = storageAccount.CreateCloudQueueClient();
            CloudQueue cloudQueue = cloudQueueClient.GetQueueReference("myqueue-items");
            CloudQueueMessage queueMessage = new CloudQueueMessage(data);
            await cloudQueue.AddMessageAsync(queueMessage);
            Console.WriteLine("Message sent");

        }
    }
}
