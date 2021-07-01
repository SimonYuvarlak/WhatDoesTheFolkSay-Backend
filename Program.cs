using Microsoft.ML;
using System;
using Tweetinvi;
using System.Threading.Tasks;

namespace SentimentAnalysis
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //TWITTER API
            
            string query;
            Console.Write("Enter a query - ");
            query = Console.ReadLine();
            await TwitterApi.TwitterApiMethod(query);

            //END OF TWITTER API
            
            
            //ML
            
            var context = new MLContext();
        
            var data = context.Data.LoadFromTextFile<SentimentData>("stock_data.csv", hasHeader: true, separatorChar: ',', allowQuoting: true);
        
            var pipeline = context.Transforms.Expression("Label", "(x) => x == 1 ? true : false", "Sentiment")
                .Append(context.Transforms.Text.FeaturizeText("Features", nameof(SentimentData.Text)))
                .Append(context.BinaryClassification.Trainers.SdcaLogisticRegression());
        
            var model = pipeline.Fit(data);
        
            var predictionEngine = context.Model.CreatePredictionEngine<SentimentData, SentimentPrediction>(model);

            //tweet iteration
            foreach (string item in Tweets.HundredTweets)
            {
                //get the prediction
                var itemPrediction = predictionEngine.Predict(new SentimentData { Text = item });
                
                //classify the prediction
                switch (itemPrediction.Probability)
                {
                    case float p when p < .5:
                        Tweets.HundredNegativeTweets.Add(new Tweet(){Text = item, Probability = p});
                        break;
                    case float p when p >= .5 && p <= .7:
                        Tweets.HundredNeutralTweets.Add(new Tweet(){Text = item, Probability = p});
                        break;
                    case float p when p > .7:
                        Tweets.HundredPositiveTweets.Add(new Tweet(){Text = item, Probability = p});
                        break;
                }
            }
            //END OF ML
            
            
            //Get the numeric results
            Console.WriteLine($"Confidence is {Tweets.ConfidencePercentage()}%");
            Console.WriteLine($"Folks' overall opinion is -> {Tweets.ConfidenceText()}");
            Console.WriteLine($"Percentage of positive tweets -> {Tweets.SubConfidencePercentage(Tweets.HundredPositiveTweets)}%");
            Console.WriteLine(Tweets.HundredPositiveTweets.Count);
            Console.WriteLine($"Percentage of neutral tweets -> {Tweets.SubConfidencePercentage(Tweets.HundredNeutralTweets)}%");
            Console.WriteLine($"Percentage of negative tweets -> {Tweets.SubConfidencePercentage(Tweets.HundredNegativeTweets)}%");
            
            
            //END OF GRAPH


            //print all 100 retrieved tweets 
             // for (int i = 0; i < 100; i++)
             // {
             //     Console.WriteLine(Tweets.HundredTweets[i]);
             // }
             // Console.WriteLine("Positive tweets");
             // foreach (var item in Tweets.HundredPositiveTweets)
             // {
             //     Console.WriteLine(item.Text);
             // }
             // Console.WriteLine("Neutral tweets");
             // foreach (var item in Tweets.HundredNeutralTweets)
             // {
             //     Console.WriteLine(item.Text);
             // }
             // Console.WriteLine("Negative tweets");
             // foreach (var item in Tweets.HundredNegativeTweets)
             // {
             //     Console.WriteLine(item.Text);
             // }
        }
        
    }
}
