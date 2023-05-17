using Azure.Storage.Queues;
using Gruppo2.WebApp.Models;
using System.Text.Json;

namespace Gruppo2.WebApp.Services
{
    public class QueueReceiverService
    {
        private readonly string _cs;
        public QueueReceiverService(IConfiguration configuration)
        {
            _cs = configuration.GetConnectionString("storage");
        }

        public async Task TakeElementFromQueue(CancellationToken stoppingToken)
        {
            var queueClient = new QueueClient(_cs, "queueprojectwork");
            await queueClient.CreateIfNotExistsAsync();

            while (!stoppingToken.IsCancellationRequested)
            {
                var queueMessage = await queueClient.ReceiveMessageAsync();
                if (queueMessage is not null)
                {
                    //scoda
                    var message = JsonSerializer.Deserialize<ActivityContent>(queueMessage.Value.Body.ToString());

                    //elimino il messaggio dalla coda
                    await queueClient.DeleteMessageAsync(queueMessage.Value.MessageId, queueMessage.Value.PopReceipt);

                    //post su influxdb
                    //manca

                }
                else
                {
                    await Task.Delay(1000, stoppingToken);
                }
            }
        }
    }
}
