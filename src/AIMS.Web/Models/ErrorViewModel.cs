using AIMS.Core.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace AIMS.Web.Models
{
    public class FileUpload
    {

        public IFormFile file
        { get; set; }

        public string filename { get; set; }
    }

    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }


}
