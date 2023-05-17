using Azure.Storage.Queues;
using Gruppo2.WebApp.Models;
using System.Text.Json;

namespace Gruppo2.WebApp.Services
{
    public class QueueSenderService
    {
        private readonly string _cs;
        public QueueSenderService(IConfiguration configuration)
        {
            _cs = configuration.GetConnectionString("storage");
        }

        public async Task AddElementinQueue(CancellationToken stoppingToken, ActivityContent activityContent)
        {
            var queueClient = new QueueClient(_cs, "queueprojectwork");
            await queueClient.CreateIfNotExistsAsync();

            while (!stoppingToken.IsCancellationRequested)
            {
                var msg = JsonSerializer.Serialize(activityContent);
                await queueClient.SendMessageAsync(msg);//aggiunta messaggio su coda
            }
        }
    }
}
