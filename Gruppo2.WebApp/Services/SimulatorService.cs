namespace Gruppo2.WebApp.Services
{
    public class SimulatorService
    {
        private readonly QueueSenderService _senderQueue;
        private readonly QueueReceiverService _receiverQueue;
        public SimulatorService(QueueSenderService senderQueue, QueueReceiverService receiverQueue)
        {
            _receiverQueue= receiverQueue;
            _senderQueue= senderQueue;
        }

        //main di mattia
        public void Start() 
        { 
            //simulatore di mattia
        }

    }
}
