using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AzureStorageHandles
{
    public class Program
    {
        static string connectionString = "<ConnectionString>";
        static string blobUri = "blobUri";

        public static void Main(string[] args)
        {
            var cts = new CancellationTokenSource();

            Task.Run(async () =>
            {
                var aoaStroage = Microsoft.WindowsAzure.Storage.CloudStorageAccount.Parse(connectionString);
                var storage = aoaStroage.CreateCloudBlobClient();

                var blob = await storage.GetBlobReferenceFromServerAsync(new Uri(blobUri));

                while (!cts.IsCancellationRequested)
                {
                    await UpdateBlob(blob);

                    await Task.Delay(500, cts.Token);
                }
                Console.WriteLine("Exiting");
            });

            Console.WriteLine("Running, Press any key to exit");
            Console.ReadKey();
            cts.Cancel();
            Thread.Sleep(1000);

        }


        private static async Task UpdateBlob(ICloudBlob blob)
        {
            try
            {
                byte[] dataBytes = new byte[40];
                await blob.UploadFromByteArrayAsync(dataBytes, 0, 40);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
