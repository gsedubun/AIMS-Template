using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AIMS.Web.Models;
using AIMS.Infrastructure.FileTransfer;
using System.IO;

namespace AIMS.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly FtpClientWrapper ftpClient;
        private readonly FileUploadHelper uploadHelper;

        public HomeController(ILogger<HomeController> logger, FtpClientWrapper ftpClientWrapper, FileUploadHelper fileUploadHelper)
        {
            _logger = logger;
            ftpClient = ftpClientWrapper;
            uploadHelper = fileUploadHelper;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult UploadFile()
        {
            return View();
        }

        [HttpPost]
        public IActionResult UploadFile(FileUpload fileUpload)
        {
            if (ModelState.IsValid)
            {
                string fileLocation = "./" + fileUpload.filename;

                uploadHelper.SaveFile(fileUpload.file.OpenReadStream(), "./", fileUpload.filename);
                var success = ftpClient.UploadFile(fileLocation, "/files/" + fileUpload.filename, true);
                if (success)
                    return RedirectToAction("UploadFile");
                else
                    return View(fileUpload);
            }
            return View(fileUpload);

        }
    }
}
