using System;

namespace Sample.Rekognition.NetCore
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var awsAccessKeyId = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID");
            var awsSecretAccessKey = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY");

            Console.WriteLine("Hello World!");
        }
    }
}
