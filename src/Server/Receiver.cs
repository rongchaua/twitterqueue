using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RawRabbit.vNext;
using RawRabbit.vNext.Disposable;

namespace Server
{
    public class Receiver<T>
    {
        private IBusClient client;
        public event Action<T> Received;

        public void Receive()
        {
            client = BusClientFactory.CreateDefault();
            client.SubscribeAsync<T>(async (message, context) => {
                await Task.Run(() => Received?.Invoke(message));
            });
        }

        public void Stop()
        {
            client.ShutdownAsync().Wait();
        }

    }
}
