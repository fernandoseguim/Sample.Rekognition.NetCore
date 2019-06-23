using System;
using System.IO;
using System.Threading.Tasks;
using Amazon;
using Amazon.Rekognition;
using Amazon.Rekognition.Model;
using Sample.Rekognition.NetCore.FacialAnalysis;

namespace Sample.Rekognition.NetCore
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var awsAccessKeyId = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID");
            var awsSecretAccessKey = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY");

            using(var amazonRekognitionClient = new AmazonRekognitionClient(awsAccessKeyId, awsSecretAccessKey, RegionEndpoint.USEast1))
            {
                var images = Directory.GetFiles("assets\\facedetection");

                var image = await File.ReadAllBytesAsync(images[0]);

                await new FacialAnalysisSample(amazonRekognitionClient).AnalizeAsync(image, 90);
            }
        }
    }
}
