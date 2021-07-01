using System;

namespace SentimentAnalysis
{
    public class Tweet
    {
        public string Text { get; set; } = "";
        public float Probability { get; set; } = 0;
    }
}