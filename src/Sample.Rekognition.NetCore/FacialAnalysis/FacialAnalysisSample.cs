using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Amazon.Rekognition;
using Amazon.Rekognition.Model;

namespace Sample.Rekognition.NetCore.FacialAnalysis
{
    public class FacialAnalysisSample
    {
        private readonly AmazonRekognitionClient _amazonRekognitionClient;

        public FacialAnalysisSample(AmazonRekognitionClient amazonRekognitionClient)
            => _amazonRekognitionClient = amazonRekognitionClient ?? throw new ArgumentNullException(nameof(amazonRekognitionClient));

        public async Task AnalizeAsync(byte[] image, float confidence)
        {
            var count = 0;
            using(_amazonRekognitionClient)
            {
                using(var source = new MemoryStream(image))
                {
                    var request = new DetectFacesRequest { Image = new Image() { Bytes = source }, Attributes = new List<string> { "ALL" } };

                    var response = await _amazonRekognitionClient.DetectFacesAsync(request);

                    foreach(var faceDetail in response.FaceDetails)
                    {
                        Console.WriteLine($"Person {++count}");

                        Console.WriteLine(IsMale(faceDetail, confidence) ? "appears to be male" : "appears to be female");
                        Console.WriteLine(IsSmiling(faceDetail, confidence) ? "smiling" : "not smiling");
                        Console.WriteLine(HaveBeard(faceDetail, confidence) ? "has a beard" : "does not have a beard");

                        foreach(var emotion in faceDetail.Emotions)
                        {
                            if(emotion.Confidence > confidence) { Console.WriteLine($"this person may feel {emotion.Type}"); }
                        }
                    }
                }
            }
        }

        private static bool IsMale(FaceDetail faceDetail, float confidence) => faceDetail.Gender.Confidence > confidence && faceDetail.Gender.Value.Value.Equals("male", StringComparison.InvariantCultureIgnoreCase);
        private static bool IsSmiling(FaceDetail faceDetail, float confidence) => faceDetail.Smile.Confidence > confidence && faceDetail.Smile.Value is true;
        private static bool HaveBeard(FaceDetail faceDetail, float confidence) => faceDetail.Beard.Confidence > confidence && faceDetail.Beard.Value is true;
    }
}
