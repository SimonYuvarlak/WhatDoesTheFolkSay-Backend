using Tweetinvi;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace SentimentAnalysis
{
    public static class TwitterApi
    {
        public static async Task TwitterApiMethod(string query)
        {
            // we create a client with your user's credentials
            var userClient = new TwitterClient(Constants.apiKey, Constants.apiSecretKey, Constants.accessToken, Constants.accessTokenSecret);

            
            // Create a simple stream containing only tweets with the keyword France
            var stream = userClient.Streams.CreateFilteredStream();
            stream.AddTrack(query);
            
            int index = 0;

            stream.MatchingTweetReceived += (sender, eventReceived) =>
            {
                if (index == 100)
                {
                    stream.Stop();
                    Console.WriteLine("Tweet retrieval process has finished.");
                    return;
                }

                // Console.WriteLine(eventReceived.Tweet.FullText);
                Tweets.HundredTweets[index] = eventReceived.Tweet.FullText;
                // Console.WriteLine(Tweets.HundredTweets[index]);
                // Console.WriteLine(index);
                index++;
            };

            
            await stream.StartMatchingAllConditionsAsync();
        }
        
    }
}