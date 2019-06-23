using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Amazon.Rekognition;
using Amazon.Rekognition.Model;

namespace Sample.Rekognition.NetCore.ObjectsDetection
{
    public class DetectObjectsSample
    {
        private readonly AmazonRekognitionClient _amazonRekognitionClient;

        public DetectObjectsSample(AmazonRekognitionClient amazonRekognitionClient)
            => _amazonRekognitionClient = amazonRekognitionClient ?? throw new ArgumentNullException(nameof(amazonRekognitionClient));

        public async Task AnalizeAsync(byte[] image, Func<ICollection<Label>, float, bool> isCat)
        {
            using(_amazonRekognitionClient)
            {
                using(var stream = new MemoryStream(image))
                {
                    var request = new DetectLabelsRequest { Image = new Image { Bytes = stream } };

                    var response = await _amazonRekognitionClient.DetectLabelsAsync(request);

                    Console.WriteLine(isCat(response.Labels, 95) ? "is a cat" : "not is a cat");
                }
            }
        }
    }
}
