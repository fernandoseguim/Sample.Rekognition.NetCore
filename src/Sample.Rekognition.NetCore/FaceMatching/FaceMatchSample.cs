using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Rekognition;
using Amazon.Rekognition.Model;

namespace Sample.Rekognition.NetCore.FaceMatching
{
    public class FaceMatchSample
    {
        private readonly AmazonRekognitionClient _amazonRekognitionClient;

        public FaceMatchSample(AmazonRekognitionClient amazonRekognitionClient)
            => _amazonRekognitionClient = amazonRekognitionClient ?? throw new ArgumentNullException(nameof(amazonRekognitionClient));

        public async Task AnalizeAsync(byte[] sourceImage, byte[] targetImage , float similaririty, float confidence)
        {
            using(_amazonRekognitionClient)
            {
                using(var source = new MemoryStream(sourceImage))
                    using(var target = new MemoryStream(targetImage))
                    {
                        var request = new CompareFacesRequest() { SourceImage = new Image() { Bytes = source }, TargetImage = new Image() { Bytes = target }, SimilarityThreshold = similaririty };

                        var reponse = await _amazonRekognitionClient.CompareFacesAsync(request);

                        Console.WriteLine(HaveSamePerson(reponse.FaceMatches, similaririty, confidence)
                                ? "The person in the source image is in the target image"
                                : "The person in the souce image isn't in the target image");
                    }
            }
        }

        private static bool HaveSamePerson(IEnumerable<CompareFacesMatch> facesMatches, float similarity, float confidence)
            => facesMatches.Any(label => label.Similarity > similarity && label.Face.Confidence > confidence);
    }
}
