using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace FBClone.Service
{
    public interface IBlobStorageService
    {
        string GetBrandingImagePath(string userid, string storageConnection, string blobContainer, string type);
    }

    public class BlobStorageService : IBlobStorageService
    {
        public string GetBrandingImagePath(string userid, string storageConnection, string blobContainer, string type)
        {
            var storageAccount = CloudStorageAccount.Parse(storageConnection);
            CloudBlobClient blobStorage = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobStorage.GetContainerReference(blobContainer);
            var blobs = container.ListBlobs();
            string brandingImageUri = null;
            if (blobs.Any())
            {
                foreach (CloudBlockBlob b in blobs)
                {
                    if (b.Name.ToUpper().Contains(String.Format("{0}_{1}", type, userid).ToUpper()))
                    {
                        brandingImageUri = b.Uri.AbsoluteUri;
                    }
                }
            }
            return brandingImageUri;
        }
    }
}
