using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FBClone.WebAPI.Common
{
    public static class AzureUtil
    {
        public static CloudBlobContainer InitializeStorage()
        {
            var storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["StorageConnection"].ConnectionString);
            CloudBlobClient blobStorage = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobStorage.GetContainerReference(ConfigurationManager.AppSettings["BlobContainer"]);
            return container;
        }


        public static async Task<HttpResponseMessage> InvokeImageWebJob(string filename, string baseUrl, string biteId)
        {
            using (var client = new HttpClient())
            {
                string kuduUrl = ConfigurationManager.AppSettings["KUDU_API_URL"];
                client.BaseAddress = new Uri(kuduUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                var userName = ConfigurationManager.AppSettings["PUBLISH_USER"];
                var password = ConfigurationManager.AppSettings["PUBLISH_PW"];
                var encoding = new ASCIIEncoding();
                var authHeader = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(encoding.GetBytes(string.Format($"{userName}:{password}"))));
                client.DefaultRequestHeaders.Authorization = authHeader;
                var content = new System.Net.Http.StringContent("");

                HttpResponseMessage response = await client.PostAsync($"api/triggeredwebjobs/web-job-html-image-converter/run?arguments={biteId} { baseUrl}", content);
                return response;
            }
        }
    }
}