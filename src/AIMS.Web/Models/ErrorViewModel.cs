using AIMS.Core.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace AIMS.Web.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }

    
}
