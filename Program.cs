using Microsoft.ML;
using System;
using System.Threading;
using Tweetinvi;
using System.Threading.Tasks;
using System.Diagnostics;

namespace SentimentAnalysis
{
    static class Program
    {
        public static readonly CancellationTokenSource s_cts = new CancellationTokenSource();
        private static CancellationToken token = s_cts.Token;
        
        static async Task Main(string[] args)
        {
            //TWITTER API
            
            Console.Write("Enter a query - ");
            string query = Console.ReadLine();
            int waitTime = 0;
            while (waitTime < 1)
            {
                Console.WriteLine("Maximum, how many minutes you are willing to wait for data retrieval. \n The more data we have, better result we can give to you. (Minimum 1) \n 3 minutes are recommended.");
                string response = Console.ReadLine();
                if (string.IsNullOrEmpty(response))
                {
                    Console.WriteLine("Please enter a value");
                }
                else
                {
                    waitTime = int.Parse(response);
                }
            }
            try
            {
                s_cts.CancelAfter(waitTime * 60000);
                try
                {
                    Console.WriteLine("Data retrieval has started.");
                    Task.Run(() => TwitterApi.TwitterApiMethod(query), token).Wait(token);
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("You operation has canceled due too long time for retrieval.");
                }
            }
            catch (TaskCanceledException)
            {
                Console.WriteLine("\nTasks cancelled: timed out.\n");
            }
            finally
            {
                s_cts.Dispose();
            }

            //END OF TWITTER API
            
            //ML
            
            var context = new MLContext();
        
            var data = context.Data.LoadFromTextFile<SentimentData>("stock_data.csv", hasHeader: true, separatorChar: ',', allowQuoting: true);
        
            var pipeline = context.Transforms.Expression("Label", "(x) => x == 1 ? true : false", "Sentiment")
                .Append(context.Transforms.Text.FeaturizeText("Features", nameof(SentimentData.Text)))
                .Append(context.BinaryClassification.Trainers.SdcaLogisticRegression());
        
            var model = pipeline.Fit(data);
        
            var predictionEngine = context.Model.CreatePredictionEngine<SentimentData, SentimentPrediction>(model);

            if (Tweets.HundredTweets.Count == 0)
            {
                Console.WriteLine("There is no tweet we could retrieve and process waas geting too long, so the process has stopped. \n You can try later the same query or now a different query. \n We are still developing, thank you for you patience.");
            }
            else
            {
                //tweet iteration
                foreach (string item in Tweets.HundredTweets)
                {
                    //get the prediction
                    var itemPrediction = predictionEngine.Predict(new SentimentData {Text = item});

                    //classify the prediction
                    switch (itemPrediction.Probability)
                    {
                        case float p when p < .5:
                            Tweets.HundredNegativeTweets.Add(new Tweet() {Text = item, Probability = p});
                            break;
                        case float p when p >= .5 && p <= .7:
                            Tweets.HundredNeutralTweets.Add(new Tweet() {Text = item, Probability = p});
                            break;
                        case float p when p > .7:
                            Tweets.HundredPositiveTweets.Add(new Tweet() {Text = item, Probability = p});
                            break;
                    }
                }
                //END OF ML


                //Get the numeric results
                Console.WriteLine($"Confidence is {Tweets.ConfidencePercentage()}%");
                Console.WriteLine($"Folks' overall opinion is -> {Tweets.ConfidenceText()}");
                Console.WriteLine(
                    $"Percentage of positive tweets -> {Tweets.SubCategoryPercentage(Tweets.HundredPositiveTweets)}%");
                Console.WriteLine(
                    $"Percentage of neutral tweets -> {Tweets.SubCategoryPercentage(Tweets.HundredNeutralTweets)}%");
                Console.WriteLine(
                    $"Percentage of negative tweets -> {Tweets.SubCategoryPercentage(Tweets.HundredNegativeTweets)}%");

            }
        }

    }
}
