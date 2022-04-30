using Amazon.S3;
using Amazon.S3.Model;

namespace MusicSubscriptionApp.Data
{
    public class S3ImageUpload
    {
        public static async Task UploadToS3(string web_url, string songID, IAmazonS3 clientS3)
        {
            var wc = new HttpClient();
            Stream fileStream = await wc.GetStreamAsync(web_url);

            byte[] fileBytes = ToArrayBytes(fileStream);

            var request = new PutObjectRequest
            {
                CannedACL = S3CannedACL.PublicRead,
                BucketName = "artistimages3655612",
                Key = songID,
                ContentType = "image/jpeg",
                InputStream = new MemoryStream(fileBytes)
            };
            PutObjectResponse response = await clientS3.PutObjectAsync(request);
        }

        public static byte[] ToArrayBytes(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }


    }


}
