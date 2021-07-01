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
        
        //Method for the confidence result as text based on the percentage
        public static string ConfidenceText()
        {
            int percentage = ConfidencePercentage();
            
            switch (percentage)
            {
                case int p when p < 50:
                    return "negative";
                    break;
                case int p when p >= 50 && p <= 70:
                    return "neutral";
                    break;
                case int p when p > 70:
                    return "positive";
                    break;
            }

            return "could not found";
        }
        
        //Get the percentage of the subarray
        public static int SubConfidencePercentage(List<Tweet> item)
        {
            float itemCount = item.Count;
            float positiveCount = HundredPositiveTweets.Count;
            float neutralCount = HundredNeutralTweets.Count;
            float negativeCount = HundredNegativeTweets.Count;
            var result = itemCount / (positiveCount + neutralCount + negativeCount) * 100;
            return (int) result;
        }
    }   
}