using System.IO;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FileService.Models;
using Microsoft.WindowsAzure.Storage; // Namespace for CloudStorageAccount
using Microsoft.WindowsAzure.Storage.Blob; // Namespace for Blob storage types
using System.Threading.Tasks;

namespace FileService.Controllers
{
    public class BlobController: Controller
    {
        public ActionResult CreateBlobContainer()
        {
            BlobManager bm = new BlobManager();
            CloudBlobContainer container = bm.GetContainer("mysamplecontainer");
            ViewBag.Success = container.CreateIfNotExistsAsync().Result;
            ViewBag.BlobContainerName = container.Name;
            return View();
        }

        public IActionResult HomePage()
        {
            return View();
        }

        //========================Upload File================================
        [HttpPost]
        public async Task<IActionResult> UploadFile(List<IFormFile> files)
        {
            var blobManager = new BlobManager();

            foreach (IFormFile file in files)
            {
                await blobManager.UploadFile("mysamplecontainer", file);
            }

            return View();
        }

        //=================download=============================
        //https://docs.microsoft.com/en-us/azure/visual-studio/vs-storage-aspnet-core-getting-started-blobs
        public ActionResult ListBlobs()
        {
            BlobManager bm = new BlobManager();
            CloudBlobContainer container = bm.GetContainer("mysamplecontainer");
            List<string> blobs = new List<string>();
            BlobResultSegment resultSegment = container.ListBlobsSegmentedAsync(null).Result;
            foreach (IListBlobItem item in resultSegment.Results)
            {
                if (item.GetType() == typeof(CloudBlockBlob))
                {
                    CloudBlockBlob blob = (CloudBlockBlob)item;
                    blobs.Add(blob.Name);
                }
                else if (item.GetType() == typeof(CloudPageBlob))
                {
                    CloudPageBlob blob = (CloudPageBlob)item;
                    blobs.Add(blob.Name);
                }
                else if (item.GetType() == typeof(CloudBlobDirectory))
                {
                    CloudBlobDirectory dir = (CloudBlobDirectory)item;
                    blobs.Add(dir.Uri.ToString());
                }
            }

            return View(blobs);
        }

        public ActionResult DownLoadFile(string id)
        {
            //MemoryStream ms = new MemoryStream();
            //BlobManager bm = new BlobManager();
            //CloudStorageAccount storageAccount = bm.InitializeAccount();
            //CloudBlobClient BlobClient = storageAccount.CreateCloudBlobClient();
            //CloudBlobContainer c1 = BlobClient.GetContainerReference("mysamplecontainer");
            ////bool hasContainer = await c1.CreateIfNotExistsAsync();
            //if (c1.CreateIfNotExistsAsync())
            //{
            //    CloudBlob file = c1.GetBlobReference(id);

            //    if (file.ExistsAsync())
            //    {
            //        file.DownloadToStreamAsync(ms);
            //        Stream blobStream = file.OpenReadAsync().Result;
            //        return File(blobStream, file.Properties.ContentType, file.Name);
            //    }
            //    else
            //    {
            //        return Content("File does not exist");
            //    }
            //}
            //else
            //{
            //    return Content("Dir does not exist");
            //}

            
            MemoryStream ms = new MemoryStream();
            BlobManager bm = new BlobManager();
            CloudStorageAccount storageAccount = bm.InitializeAccount();

            CloudBlobClient BlobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer c1 = BlobClient.GetContainerReference("mysamplecontainer");
            CloudBlob file = c1.GetBlobReference(id);
            file.DownloadToStreamAsync(ms);
            Stream blobStream = file.OpenReadAsync().Result;
            return File(blobStream, file.Properties.ContentType, file.Name);
            
        }

        [Route("DeleteFile/{fileName}")]
        [HttpGet]
        public async Task<bool> DeleteFile(string fileName)
        {
            MemoryStream ms = new MemoryStream();
            BlobManager bm = new BlobManager();
            CloudStorageAccount storageAccount = bm.InitializeAccount();
            CloudBlobClient BlobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = BlobClient.GetContainerReference("mysamplecontainer");
            try
            {
                if (await container.ExistsAsync())
                {
                    CloudBlob file = container.GetBlobReference(fileName);

                    if (await file.ExistsAsync())
                    {
                        await file.DeleteAsync();
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }


        public IActionResult DeleteBlob()
        {
            return View();
        }

        /*
        [Route("DeleteFile/{fileName}")]
        [HttpGet]
        public async Task<bool> DeleteFile(string fileName)
        {
            BlobManager bm = new BlobManager();
            CloudStorageAccount storageAccount = bm.InitializeAccount();
            CloudBlobClient BlobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer c1 = BlobClient.GetContainerReference("mysamplecontainer");
            var blob = c1.GetBlobReference(fileName);
            bool x =  await blob.DeleteIfExistsAsync();
            return x;
      
        }
        */
    }
}
