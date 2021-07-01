using Tweetinvi;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using Tweetinvi.Models;

namespace SentimentAnalysis
{
    public static class TwitterApi
    {
        public static DateTime Start = DateTime.Now;
        public static async Task TwitterApiMethod(string query)
        {
            Console.WriteLine("New thread execution has started.");

            // we create a client with your user's credentials
            var userClient = new TwitterClient(Constants.apiKey, Constants.apiSecretKey, Constants.accessToken, Constants.accessTokenSecret);

            
            // Create a simple stream containing only tweets with the keyword France
            var stream = userClient.Streams.CreateFilteredStream();
            stream.AddTrack(query);
            
            //Temporary English Language filter for bugless development process
            stream.AddLanguageFilter(LanguageFilter.English); 
            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!1
            
            int index = 0;

            stream.MatchingTweetReceived += (sender, eventReceived) =>
            {
                DateTime end = DateTime.Now;
                var result = end.Subtract(Start).TotalMinutes;
                Console.WriteLine(result);
                if (index == 100 || result >= 1.0)
                {
                    stream.Stop();
                    Console.WriteLine("Tweet retrieval process has finished.");
                    return;
                }
                
                Tweets.HundredTweets.Add(eventReceived.Tweet.FullText);;
                Console.WriteLine("Tweet retrieved " + index);
                index++;
            };

            
            await stream.StartMatchingAllConditionsAsync();
        }
        
    }
}