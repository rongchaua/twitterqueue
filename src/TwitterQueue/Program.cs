using Infrastructure;
using Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TwitterQueue
{
    public class Program
    {
        private const string CredentialsFilename = "twitter.credentials";
        public static void Main(string[] args)
        {
            if (!File.Exists(CredentialsFilename))
                Authenticate();

            try
            {
                SendTweetToQueue($"Hello, it's now: {DateTime.Now}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            Console.ReadLine();
        }

        private static void Authenticate()
        {
            var twitterClient = new TwitterClient();
            twitterClient.AuthenticateAppViaBrowser();
            Console.Write("Please enter the pin:");
            var pin = Console.ReadLine();

            var token = twitterClient.CreateTokenFromPin(pin);
            token.WriteToFile(CredentialsFilename);
        }

        private static void SendTweetToQueue(string tweet)
        {
            var token = Token.ReadFromFile(CredentialsFilename);
            var sender = new Sender<Tweet>();
            sender.Send(new Tweet
            {
                CorrelationId = Guid.NewGuid().ToString(),
                Message = tweet,
                Token = token
            });
        }
    }
}
