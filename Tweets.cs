using System.Collections.Generic;

namespace SentimentAnalysis
{
    public static class Tweets
    {
        public static string[] HundredTweets { get; set; } = new string[100];
        public static List<Tweet> HundredPositiveTweets { get; set; } = new List<Tweet>();
        public static List<Tweet> HundredNeutralTweets { get; set; } = new List<Tweet>();
        public static List<Tweet> HundredNegativeTweets { get; set; } = new List<Tweet>();

        //Method to calculate the average of the sub lists
        public static float SubAverage(List<Tweet> averageList)
        {
            if (averageList.Count == 0)
            {
                return 0;
            }
            else
            {
                float average = 0;
                foreach (var item in averageList)
                {
                    average += item.Probability;
                }

                return average / averageList.Count;
            }
        }

        //Method to calculate the average of all tweets
        public static float TotalAverage()
        {
            float positive = SubAverage(HundredPositiveTweets);
            float neutral = SubAverage(HundredNeutralTweets);
            float negative = SubAverage(HundredNegativeTweets);

            return ((positive * HundredPositiveTweets.Count) + (neutral * HundredNeutralTweets.Count) +
                    (negative * HundredNegativeTweets.Count)) / (HundredPositiveTweets.Count +
                                                                 HundredNeutralTweets.Count +
                                                                 HundredNegativeTweets.Count);
        }
        
        //Method to get the confidence level as a percentage based on the totalAverage() result
        public static int ConfidencePercentage()
        {
            return (int) (TotalAverage() * 100);   
        }
    }   
}