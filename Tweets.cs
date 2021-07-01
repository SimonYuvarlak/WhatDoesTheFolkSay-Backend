using System.Collections.Generic;

namespace SentimentAnalysis
{
    public static class Tweets
    {
        public static string[] HundredTweets { get; set; } = new string[100];
        public static List<Tweet> HundredPositiveTweets { get; set; } = new List<Tweet>();
        public static List<Tweet> HundredNeutralTweets { get; set; } = new List<Tweet>();
        public static List<Tweet> HundredNegativeTweets { get; set; } = new List<Tweet>();
    }
}