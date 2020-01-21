using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AIMS.Infrastructure.FileTransfer
{
    public class FileUploadHelper
    {
        public async Task<string> SaveFileAsync(Stream file, string pathToUplaod,string filename)
        {
            string imageUrl = string.Empty;
            if (!Directory.Exists(pathToUplaod))
                System.IO.Directory.CreateDirectory(pathToUplaod);//Create Path of not exists

            string pathwithfileName = pathToUplaod + "\\" + filename;
            using (var fileStream = new FileStream(pathwithfileName, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            imageUrl = pathwithfileName;
            return imageUrl;
        }

        public string SaveFile(Stream file, string pathToUplaod, string filename)
        {
            string imageUrl = string.Empty;
            string pathwithfileName = pathToUplaod + "\\" + filename; //GetFileName(file, true);
            using (var fileStream = new FileStream(pathwithfileName, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }
            imageUrl = pathwithfileName;

            return imageUrl;
        }

        public string GetFileName(string fileName, bool BuildUniqeName)
        {
            string outfileName = string.Empty;
            //string strFileName = file.FileName.Substring(
            //    file.FileName.LastIndexOf("\\"))
            //    .Replace("\\", string.Empty);
            string strFileName = fileName;
            string fileExtension = GetFileExtension(fileName);
            if (BuildUniqeName)
            {
                string strUniqName = GetUniqueName("doc");
                outfileName = strUniqName + fileExtension;
            }
            else
            {
                outfileName = strFileName;
            }
            return outfileName;
        }
        public string GetUniqueName(string preFix)
        {
            string uName = preFix + DateTime.Now.ToString()
                .Replace("/", "-")
                .Replace(":", "-")
                .Replace(" ", string.Empty)
                .Replace("PM", string.Empty)
                .Replace("AM", string.Empty);
            return uName;
        }
        public string GetFileExtension(string fileName)
        {
            string fileExtension;
            fileExtension = (fileName != null) ?
                fileName.Substring(fileName.LastIndexOf('.')).ToLower()
                : string.Empty;
            return fileExtension;


        }

        public string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }
    }
}
