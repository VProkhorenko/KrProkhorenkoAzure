using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.RetryPolicies;

namespace KrProkhorenkoAzure
{
    public class Program
    {
        public static object ConfigurationManager { get; private set; }

        static void Main(string[] args)
        {
            Console.WriteLine("Работа с контейнером");
            string imageToUpload = "";
            string containerName = "";

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));

            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            Console.WriteLine("1. Создание контейнера");
            Console.WriteLine("Введите имя контейнера");
            containerName = Console.ReadLine();
            CloudBlobContainer container = blobClient.GetContainerReference(containerName);
            BlobRequestOptions requestOptions = new BlobRequestOptions() { RetryPolicy = new NoRetry() };
            container.CreateIfNotExists(requestOptions, null);

            container.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

            Console.WriteLine("2. Загрузка изображения");
            Console.WriteLine("Введите имя изображения");
            imageToUpload = Console.ReadLine();
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(imageToUpload);
            blockBlob.Properties.ContentType = "image/png";
            blockBlob.UploadFromFile(imageToUpload);

            Console.WriteLine("3. Список объектов в контейнере");
            foreach (IListBlobItem blob in container.ListBlobs())
            {
                Console.WriteLine("{0}", blob.Uri);
            }

            Console.WriteLine();
            Console.WriteLine("Нажмите любую клавишу для выхода");
            Console.ReadLine();
        }
    }
}
