using Infrastructure;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var receiver = new Receiver<Tweet>();
            receiver.Received += Receiver_Received;
            receiver.Receive();
            Console.WriteLine("Press any key to stop...");
            Console.ReadKey();
            receiver.Stop();
        }

        private static void Receiver_Received(Tweet tweet)
        {
            var twitterClient = new TwitterClient();
            var sender = new Sender<TweetStatus>();
            var response = twitterClient.Tweet(tweet);
            var tweetStatus = new TweetStatus()
            {
                CorrelationId = tweet.CorrelationId,
                Status = response
            };
            sender.Send(tweetStatus);
        }
    }
}
