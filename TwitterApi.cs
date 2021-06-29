using Tweetinvi;
using System.Threading.Tasks;
using System;
namespace SentimentAnalysis
{
    public static class TwitterApi
    {
        public static async Task TwitterApiMethod()
        {
            // we create a client with your user's credentials
            var userClient = new TwitterClient(Constants.apiKey, Constants.apiSecretKey, Constants.accessToken, Constants.accessTokenSecret);

            // request the user's information from Twitter API
            var user = await userClient.Users.GetAuthenticatedUserAsync();
            Console.WriteLine("Hello " + user);

            // publish a tweet
            var tweet = await userClient.Tweets.PublishTweetAsync("Bu tweet de atildiysa olayin boku cikar.");
            Console.WriteLine("You published the tweet : " + tweet);
        }
        
    }
}