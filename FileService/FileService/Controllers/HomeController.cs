using System.IO;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FileService.Models;
using Microsoft.WindowsAzure.Storage; // Namespace for CloudStorageAccount
using Microsoft.WindowsAzure.Storage.Blob; // Namespace for Blob storage types

namespace FileService.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        //==============Upload File================================
        //[HttpPost]
        //public IActionResult UploadFile(IFormFile FileUpload)
        //{

        //    if (FileUpload != null && FileUpload.Length > 0)
        //    {
        //        var blobManager = new BlobManager();
        //        blobManager.UploadFile("mysamplecontainer", FileUpload);
        //    }

        //    return View();
        //}
        //=================download=============================
        //public ActionResult ListBlobs()
        //{
        //    BlobManager bm = new BlobManager();
        //    CloudBlobContainer container =bm.GetContainer("mysamplecontainer");
        //    List<string> blobs = new List<string>();
        //    BlobResultSegment resultSegment = container.ListBlobsSegmentedAsync(null).Result;
        //    foreach (IListBlobItem item in resultSegment.Results)
        //    {
        //        if (item.GetType() == typeof(CloudBlockBlob))
        //        {
        //            CloudBlockBlob blob = (CloudBlockBlob)item;
        //            blobs.Add(blob.Name);
        //        }
        //        else if (item.GetType() == typeof(CloudPageBlob))
        //        {
        //            CloudPageBlob blob = (CloudPageBlob)item;
        //            blobs.Add(blob.Name);
        //        }
        //        else if (item.GetType() == typeof(CloudBlobDirectory))
        //        {
        //            CloudBlobDirectory dir = (CloudBlobDirectory)item;
        //            blobs.Add(dir.Uri.ToString());
        //        }
        //    }

        //    return View(blobs);
        //}

        //public ActionResult DownLoadFile(string id)
        //{

        //    MemoryStream ms = new MemoryStream();
        //    BlobManager bm = new BlobManager();
        //    CloudStorageAccount storageAccount = bm.InitializeAccount();

        //    CloudBlobClient BlobClient = storageAccount.CreateCloudBlobClient();
        //    CloudBlobContainer c1 = BlobClient.GetContainerReference("mysamplecontainer");
        //    CloudBlob file = c1.GetBlobReference(id);
        //    file.DownloadToStreamAsync(ms);
        //    Stream blobStream = file.OpenReadAsync().Result;
        //    return File(blobStream, file.Properties.ContentType, file.Name);
        // }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
