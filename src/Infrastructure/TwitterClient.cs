using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Model;
using Tweetinvi;
using Tweetinvi.Core.Exceptions;
using Tweetinvi.Models;

namespace Infrastructure
{
    public class TwitterClient
    {
        private const string ConsumerKey = "R2XwAvgabWEL2hUne6vhTQ";
        private const string ConsumerSecret = "7vpFE5vmivLahIZm0rMZFYvYuhsg8tG4xFdIWhGbE";
        private IAuthenticationContext authenticationContext;

        public TwitterClient()
        {
            ExceptionHandler.WebExceptionReceived += (o, args) =>
            {
                throw new Exception(args.Value.TwitterDescription);
            };
        }
        public void AuthenticateAppViaBrowser()
        {
            var appCredentials = new TwitterCredentials(ConsumerKey, ConsumerSecret);
            authenticationContext = AuthFlow.InitAuthentication(appCredentials);
            Open(authenticationContext.AuthorizationURL);
        }

        public void Open(string url)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                url = url.Replace("&", "^&");
                Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
            }
        }

        public Token CreateTokenFromPin(string pin)
        {
            var userCredentials = AuthFlow.CreateCredentialsFromVerifierCode(pin, authenticationContext);
            return new Token { Key = userCredentials.AccessToken, Secret = userCredentials.AccessTokenSecret };
        }

        public string Tweet(Model.Tweet tweet)
        {
            var user = GetAuthenticatedUser(tweet.Token);
            var response = user.PublishTweet(tweet.Message);
            if (response == null)
            {
                var twitterException = ExceptionHandler.GetLastException();
                var status = GetExceptionInfos(twitterException);
                return status;
            }
            return response.IdStr;
        }

        private static string GetExceptionInfos(ITwitterException twitterException)
        {
            var messages = string.Join("\n", twitterException.TwitterExceptionInfos.Select(x => x.Message));
            return $"{twitterException.TwitterDescription}\n   {messages}";
        }

        private IAuthenticatedUser GetAuthenticatedUser(Token token)
        {
            var credentials = Auth.SetUserCredentials(ConsumerKey, ConsumerSecret, token.Key, token.Secret);
            return User.GetAuthenticatedUser(credentials);
        }
    }
}
