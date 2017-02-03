using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RawRabbit.vNext;

namespace Infrastructure
{
    public class Sender<T>
    {
        public void Send(T message)
        {
            using (var client = BusClientFactory.CreateDefault())
            {
                client.PublishAsync(message).Wait();
            }
        }
    }
}
